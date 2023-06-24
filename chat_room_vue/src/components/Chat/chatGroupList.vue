<template>
    <div class="chat_list" ref="chatbox">
        <div ref="isLoad" class="loader_box">
            <Loader></Loader>
        </div>

        <div v-if="switchStore.isGroupChat" ref="isGroupShow" style="display: none">

            <div v-if="isMore" class="more" @click="handelMoreList">
                <div><i class="fa fa-clock-o" style="margin-right: 10px;"></i>
                    <span>查看更多聊天记录</span>
                </div>
            </div>

            <!-- 聊天记录 -->
            <div v-for="(item,index) in msglogList" :key="index">
                <div v-if="item.sender.chatUserGuId != userStore.userinfo.chatUserGuId && item.receiverGuId == chatStore.chatGroup.groupGuId"
                    class="chat_other_box">
                    <div class="chat_img_box">
                        <img class="chat_img" :src=item.sender.chatUserImg alt="" @click="handleimg(item.sender)">
                    </div>
                    <div v-html="item.chatLogContent" class="message_box">
                    </div>
                </div>
                <div v-if="item.sender.chatUserGuId == userStore.userinfo.chatUserGuId && item.receiverGuId == chatStore.chatGroup.groupGuId"
                    class="chat_self_box">
                    <div v-html="item.chatLogContent" class="message_self_box">
                    </div>
                    <div class="chat_img_box">
                        <img class="chat_img" :src=userStore.userinfo.chatUserImg alt="">
                    </div>
                </div>
            </div>

            <!-- 实时聊天列表 -->
            <div v-for="(item,index) in msgList" :key="index">
                <!-- <div v-if="item.receiver == chatStore.chatUser.friendGuId && item.sender == userStore.userinfo.chatUserGuId"> -->
                <div v-if="item.sender.chatUserGuId != userStore.userinfo.chatUserGuId && item.receiver == chatStore.chatGroup.groupGuId"
                    class="chat_other_box">
                    <div class="chat_img_box">
                        <img class="chat_img" :src=item.sender.chatUserImg alt="" @click="handleimg(item.sender)">
                    </div>
                    <div v-html="item.message" class="message_box">
                    </div>
                </div>
                <div v-if="item.sender.chatUserGuId == userStore.userinfo.chatUserGuId && item.receiver == chatStore.chatGroup.groupGuId"
                    class="chat_self_box">
                    <div v-html="item.message" class="message_self_box">
                    </div>
                    <div class="chat_img_box">
                        <img class="chat_img" :src=userStore.userinfo.chatUserImg alt="">
                    </div>
                </div>

            </div>

        </div>


    </div>
</template>

<script setup>
import { onMounted, ref, watch, computed, nextTick, reactive } from 'vue'
import useChatStore from "../../store/modules/chat";
import useSwitchStore from '../../store/modules/switch'
import useSocketCahtStore from '../../store/modules/socketchat'
import useUserStore from '../../store/modules/user'
import signalr from '../../utils/signalR'
import { chatGroupLogList } from '../../api/chatLog/chatLog'
import { checkFriendStatus } from '../../api/login'
import modal from '../../plugins/modal';
import Loader from '../../components/Login/loader.vue'

const switchStore = useSwitchStore()
const chatStore = useChatStore()
const userStore = useUserStore()
const SocketCahtStore = useSocketCahtStore()

const chatbox = ref();
const isLoad = ref();
const isGroupShow = ref();
let msgList = ref([])

let msglogList = ref([])
let selfMsgList = ref([])
let ortherMsgList = ref([])

let isOnline = ref(false);
let isMore = ref(false);

let getListData = ref({
    pageSize: 20,
    pageNum: 1,
    ChatLogType: null,
    UserGuId: userStore.userinfo.chatUserGuId,
    FriendsGuId: null,
})

const postDacta = reactive({
    UserGuId: userStore.userinfo.chatUserGuId
})

let selfconnectionID = reactive()

watch(() => chatStore.chatGroup, async (value) => {
    getListData.value.pageNum = 1
    msgList.value.length = 0;
    isMore.value = false

    nextTick(() => {
        isGroupShow.value.style.display = `none`
        isLoad.value.style.display = `block`
    })
    if (msglogList.value.length != 0) {
        msglogList.value.length = 0
    }

    SocketCahtStore.chatOnlineUsers.forEach(function (item, index) {
        if (item.userGuid == userStore.userinfo.chatUserGuId) {
            selfconnectionID = item.connectionId
            // console.log(selfconnectionID,'自己的连接Id');
        }
    })

    signalr.SR.off("EnterRoom")
    signalr.SR.invoke('EnterRoom', selfconnectionID, value.groupName)
        .catch(function (err) {
            console.error(err.toString())
        })

    signalr.SR.off("groupMessages")

    signalr.SR.on("groupMessages", data => {
        console.log(data, 'data!!!!!');
        msgList.value.push(data);
        chatStore.getList(postDacta)
        // console.log(msgList.value,'after----------');
    });


    if (switchStore.isGroupChat) {
        getListData.value.FriendsGuId = chatStore.chatGroup.groupGuId
        getListData.value.ChatLogType = 2
    }

    // TODO: 修改
    await getfriendList()

    nextTick(() => {
        // console.log(chatbox.value);
        setTimeout(() => {
            setTimeout(async () => {
                isLoad.value.style.display = `none`
            }, 100);
            isGroupShow.value.style.display = `block`
            isGroupShow.value.scrollIntoView({ block: "end" })
        }, 100);
    })

},
    {
        immediate: true,
        deep: true
    })


watch(() => msgList.value, async (value) => {
    // console.log(value, "在线消息");
    if (value != null) {
        nextTick(() => {
            setTimeout(() => {
                isGroupShow.value.scrollIntoView({ block: "end" })
            }, 100);
        })
        await chatStore.getList(postDacta)
    }
},
    {
        immediate: true,
        deep: true
    })


// TODO: 修改
async function getfriendList() {
    await chatGroupLogList(getListData.value).then((res) => {
        if (res.code == 200) {
            // console.log(res.data);
            msglogList.value = res.data.result
            msglogList.value = msglogList.value.sort(sortData)
            // console.log(msglogList.value, "记录");
        }
    })

    getListData.value.pageNum += 1
    await chatGroupLogList(getListData.value).then((res) => {
        if (res.code == 200) {
            if (res.data.result.length != 0) {
                isMore.value = true
                // msglogList.value = res.data.result
                // msglogList.value = msglogList.value.sort(sortData)
            }
            getListData.value.pageNum -= 1
        }
    })
}


function sortData(a, b) {
    return new Date(a.chatLogSendTime).getTime() - new Date(b.chatLogSendTime).getTime()
}

async function handelMoreList() {
    // console.log(111);
    getListData.value.pageNum += 1
    console.log(getListData.value.pageNum);
    await chatGroupLogList(getListData.value).then((res) => {
        if (res.code == 200) {
            // console.log(res.data);
            res.data.result.forEach(e => {
                msglogList.value.push(e)
            });
            msglogList.value = msglogList.value.sort(sortData)
            console.log(msglogList.value, "下一页");
        }
    })

    getListData.value.pageNum += 1
    await chatGroupLogList(getListData.value).then((res) => {
        if (res.code == 200) {
            if (res.data.result.length != 0) {
                isMore.value = true
                console.log(111);
                // msglogList.value = res.data.result
                // msglogList.value = msglogList.value.sort(sortData)
            }
            else {
                isMore.value = false
            }
            getListData.value.pageNum -= 1
        }
    })
}

function handleimg(v) {
    switchStore.isFriendIntro = true
    switchStore.isPersonal = false
    switchStore.isFriends = false
    switchStore.isChat = false
    switchStore.isGroupChat = false

    switchStore.FriendIntroData = v
}

</script>

<style scoped>
.chat_list {
    width: 95.5%;
    height: 400px;
    /* background-color: red; */
    border-bottom: rgba(121, 163, 159, 0.3) 1px solid;
    overflow-y: scroll;
    /* overflow: auto; */
    padding: 15px;
}

/* .chat_list::-webkit-scrollbar { width: 0; height: 0; color: transparent; } */

.chat_other_box {
    display: flex;
    margin-bottom: 20px;
}

.chat_self_box {
    display: flex;
    margin-bottom: 20px;
    justify-content: flex-end;
}

.chat_img_box {
    width: 45px;
    height: 45px;
    border-radius: 50%;
    background-color: #fff;
    cursor: pointer;
}

.chat_img {
    width: 45px;
    height: 45px;
    object-fit: cover;
    border-radius: 50%;
}

.message_box {
    padding: 10px;
    width: auto;
    height: auto;
    background-color: #e5e5e5;
    margin-top: 10px;
    margin-left: 5px;
    margin: 10px 5px;
    border-radius: 10px;
    word-break: break-all;
}

.message_self_box {
    padding: 10px;
    width: auto;
    height: auto;
    margin-top: 10px;
    margin-left: 5px;
    margin: 10px 5px;
    border-radius: 10px;
    background-color: #8a98c9 !important;
    word-break: break-all;
}

.more {
    width: auto;
    display: flex;
    justify-content: center;
    cursor: pointer;
    margin: 0.5rem 0;
}

.loader_box {
    position: fixed;
    height: 100%;
    left: 60%;
}

.loader span {
    width: 100px !important;
    height: 100px !important;
    transition: 0.3s;
}
</style>
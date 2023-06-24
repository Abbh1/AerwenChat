<template>
    <div class="chat_list" ref="chatbox">
        <div ref="isLoad" class="loader_box">
            <Loader></Loader>
        </div>

        <div v-if="switchStore.isChat" ref="isShow" style="display: none">

            <div v-if="isMore" class="more" @click="handelMoreList">
                <div><i class="fa fa-clock-o" style="margin-right: 10px;"></i>
                    <span>查看更多聊天记录</span>
                </div>
            </div>

            <!-- 聊天记录 -->
            <div v-for="(item,index) in msglogList" :key="index">
                <!-- <div v-if="item.receiver == chatStore.chatUser.friendGuId && item.sender == userStore.userinfo.chatUserGuId"> -->
                <div v-if="item.senderGuId == chatStore.chatUser.friendGuId && item.receiverGuId == userStore.userinfo.chatUserGuId"
                    class="chat_other_box">
                    <div class="chat_img_box">
                        <img class="chat_img" :src=chatStore.chatUser.friendImg alt=""
                            @click="handleimg(chatStore.chatUser)">
                    </div>
                    <div v-html="item.chatLogContent" class="message_box"></div>
                </div>
                <div v-if="item.senderGuId == userStore.userinfo.chatUserGuId && item.receiverGuId == chatStore.chatUser.friendGuId"
                    class="chat_self_box">
                    <div v-html="item.chatLogContent" class="message_self_box"></div>
                    <div class="chat_img_box">
                        <img class="chat_img" :src=userStore.userinfo.chatUserImg alt="">
                    </div>
                </div>

            </div>

            <!-- 聊天列表 -->
            <div v-for="(item,index) in msgList" :key="index">

                <!-- <div v-if="item.receiver == chatStore.chatUser.friendGuId && item.sender == userStore.userinfo.chatUserGuId"> -->
                <div v-if="item.sender == chatStore.chatUser.friendGuId && item.receiver == userStore.userinfo.chatUserGuId"
                    class="chat_other_box">
                    <div class="chat_img_box">
                        <img class="chat_img" :src=chatStore.chatUser.friendImg alt=""
                            @click="handleimg(chatStore.chatUser)">
                    </div>
                    <div v-html="item.message" class="message_box" ></div>
                </div>
                <div v-if="item.sender == userStore.userinfo.chatUserGuId && item.receiver == chatStore.chatUser.friendGuId"
                    class="chat_self_box">
                    <div v-html="item.message" class="message_self_box"></div>
                    <div class="chat_img_box">
                        <img class="chat_img" :src=userStore.userinfo.chatUserImg alt="">
                    </div>
                </div>

            </div>

            <!-- 好友不在线时 -->
            <div v-if="!isOnline && selfMsgList.length != 0">
                <div v-for="(item,index) in selfMsgList" :key="index">
                    <div class="chat_self_box">
                        <div v-html="item.chatLogContent"  class="message_self_box"></div>
                        <div class="chat_img_box">
                            <img class="chat_img" :src=userStore.userinfo.chatUserImg alt="">
                        </div>
                    </div>
                </div>
            </div>



            <!-- </div> -->

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
import { chatFriendLogList, slefNewMsg } from '../../api/chatLog/chatLog'
import { checkFriendStatus } from '../../api/login'
import modal from '../../plugins/modal';
import Loader from '../../components/Login/loader.vue'

const switchStore = useSwitchStore()
const chatStore = useChatStore()
const userStore = useUserStore()
const SocketCahtStore = useSocketCahtStore()

const chatbox = ref();
const isShow = ref();
const isLoad = ref();
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

const queryParams = ref({
    ChatUserGuid: chatStore.chatUser.friendGuId
})

const postDacta = reactive({
    UserGuId: userStore.userinfo.chatUserGuId
})

watch(() => chatStore.chatUser, async (value) => {
    isOnline.value = false
    msgList.value.length = 0;
    getListData.value.pageNum = 1
    isMore.value = false
    // console.log("1次");

    // console.log(msgList.value,'test----------');

    SocketCahtStore.chatOnlineUsers.forEach(function (item, index) {
        // console.log(item);
        if (item.userGuid == chatStore.chatUser.friendGuId) {
            isOnline.value = true
            // console.log(selfconnectionID,'自己的连接Id');
        }
    })

    nextTick(() => {
        isShow.value.style.display = `none`
        isLoad.value.style.display = `block`
    })
    selfMsgList.value.length = 0
    // console.log(isShow.value);
    // console.log(value,"----------变了");
    if (msglogList.value.length != 0) {
        msglogList.value.length = 0
    }

    if (isOnline.value) {
        // await signalr.init(import.meta.env.VITE_APP_SOCKET_API)
        signalr.SR.off("ReceiveMessage")

        signalr.SR.on("ReceiveMessage", data => {
            // console.log(data,'data!!!!!');
            msgList.value.push(data);
            chatStore.getList(postDacta)
            // console.log(msgList.value,'after----------');
        });
    }


    if (switchStore.isChat) {
        getListData.value.FriendsGuId = chatStore.chatUser.friendGuId
        getListData.value.ChatLogType = 1
    }

    await getfriendList()



    nextTick(() => {
        // console.log(chatbox.value);
        setTimeout(async () => {
            isLoad.value.style.display = `none`
        }, 100);
        setTimeout(() => {
            isShow.value.style.display = `block`
            isShow.value.scrollIntoView({ block: "end" })
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
                isShow.value.scrollIntoView({ block: "end" })
            }, 100);
        })
        await chatStore.getList(postDacta)
    }
},
    {
        immediate: true,
        deep: true
    })

watch(() => switchStore.notOnlineChat, async (value) => {
    if (value == true) {
        await getSlefNewMsg()
        // console.log('最新消息执行中');
        // console.log(selfMsgList.value, '最新消息');

        switchStore.notOnlineChat = false
        await chatStore.getList(postDacta)

        nextTick(() => {
            setTimeout(() => {
                isShow.value.scrollIntoView({ block: "end" })
            }, 100);
        })
    }
},
    {
        immediate: true,
        deep: true
    })


function handleimg(v) {
    switchStore.isFriendIntro = true
    switchStore.isPersonal = false
    switchStore.isFriends = false
    switchStore.isChat = false
    switchStore.isGroupChat = false

    switchStore.FriendIntroData = v
}

async function getfriendList() {
    await chatFriendLogList(getListData.value).then((res) => {
        if (res.code == 200) {
            // console.log(res.data);
            msglogList.value = res.data.result
            msglogList.value = msglogList.value.sort(sortData)
        }
    })

    getListData.value.pageNum += 1
    await chatFriendLogList(getListData.value).then((res) => {
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

// async function getfriendList2() {
//     await getfriendList1()

//     await chatFriendLogList2(getListData.value).then((res) => {
//         if (res.code == 200) {
//             msglogList.value = msglogList.value.concat(res.data.result)

//             msglogList.value = msglogList.value.sort(sortData)
//             // console.log(msglogList.value);
//         }
//     })
// }

async function getSlefNewMsg() {
    await slefNewMsg(getListData.value).then((res) => {
        if (res.code == 200) {
            selfMsgList.value.push(res.data)
        }
    })
}

async function checkStatus() {
    await checkFriendStatus(queryParams.value).then((res) => {
        if (res.code == 200) {
            if (res.data == "空") {
                return
            }
            else {
                isOnline.value = true
                // console.log(res.data,'好友状态');
            }
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
    await chatFriendLogList(getListData.value).then((res) => {
        if (res.code == 200) {
            // console.log(res.data);
            res.data.result.forEach(e => {
                msglogList.value.push(e)
            });
            msglogList.value = msglogList.value.sort(sortData)
            // console.log(msglogList.value, "下一页");
        }
    })

    getListData.value.pageNum += 1
    await chatFriendLogList(getListData.value).then((res) => {
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
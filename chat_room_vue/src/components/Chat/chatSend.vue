<template>
    <div>
        <div class="toolbar" v-if="switchStore.isChat || switchStore.isGroupChat">
            <div class="tools">
                <!-- <UploadImage2 v-model="" ref="uploadRef" :drag="true" /> -->
                <V3Emoji style="padding-bottom: 2.5px" size="mid" :recent="true" fix-pos="upright"
                    :options-name="optionsName" :custom-size="customSize" @click-emoji="appendText">
                    <img src="../../assets/img/emoji.png" alt="">
                </V3Emoji>

                <el-upload class="avatar-uploader" :action="uploadImgUrl" :show-file-list="false"
                    :on-success="handleImgSuccess" :data="{
                        chat_img: msg,
                    }">
                    <img src="../../assets/img/pic.png" alt="">
                </el-upload>
            </div>
            <div @click="hanleChatHistoryLog" class="history">
                <img src="../../assets/img/history.png" alt="">
            </div>
        </div>
        <div class="send_box">
            <!-- onkeydown="if (event.keyCode === 13) event.preventDefault();" -->

            <div ref="inputRef" v-if="switchStore.isChat" class="textarea" maxlength="800" autofocus
                @keydown="handleEntry($event)" contenteditable="true">
            </div>

            <div ref="inputRef" v-if="switchStore.isGroupChat" class="textarea" maxlength="800" autofocus
                @keydown="handleGroupEntry($event)" contenteditable="true">
            </div>


            <textarea v-if="!switchStore.isChat && !switchStore.isGroupChat" class="textarea" maxlength="800" autofocus
                disabled></textarea>
        </div>
        <div class="btn_box">
            <button v-if="switchStore.isChat || switchStore.isGroupChat" class="close" @click="handleColse">关闭(c)</button>
            <button v-if="switchStore.isChat" @click="handelSend">发送(Enter)</button>
            <button v-if="switchStore.isGroupChat" @click="handelGroupSend()">发送(Enter)</button>
        </div>
    </div>

    <el-dialog v-model="isAdd" title="好友历史聊天记录" :width=tcWidth @close="isAdd = false" :modal="false">
        <el-input v-model="logQueryData" placeholder="搜索"></el-input>

        <div class="historyBox">
            <!-- <div v-if="isMore" class="more" @click="handelMoreList">
                <div><i class="fa fa-clock-o" style="margin-right: 10px;"></i>
                    <span>查看更多聊天记录</span>
                </div>
            </div> -->

            <div class="_message" v-for="(item, index) in msglogList" :key="index">
                <div class="imgbox"
                    v-if="item.senderGuId == chatStore.chatUser.friendGuId && item.receiverGuId == userStore.userinfo.chatUserGuId">
                    <img :src=chatStore.chatUser.friendImg alt="头像">
                </div>
                <div class="imgbox"
                    v-if="item.senderGuId == userStore.userinfo.chatUserGuId && item.receiverGuId == chatStore.chatUser.friendGuId">
                    <img :src=userStore.userinfo.chatUserImg alt="头像">
                </div>

                <div class="_message_content_box">
                    <div class="_message_name"
                        v-if="item.senderGuId == chatStore.chatUser.friendGuId && item.receiverGuId == userStore.userinfo.chatUserGuId">
                        {{ chatStore.chatUser.friendNickName }}
                    </div>
                    <div class="_message_name"
                        v-if="item.senderGuId == userStore.userinfo.chatUserGuId && item.receiverGuId == chatStore.chatUser.friendGuId">
                        {{ userStore.userinfo.chatUserNickName }}
                    </div>
                    <div class="_message_content" v-html="item.chatLogContent"></div>
                </div>

                <div class="_message_time">
                    <div>{{ item.chatLogSendTime }}</div>
                </div>
            </div>

        </div>
    </el-dialog>

    <el-dialog v-model="isGroupHistory" title="群聊历史聊天记录" :width=tcWidth @close="isGroupHistory = false" :modal="false">
        <el-input v-model="logQueryData" placeholder="搜索"></el-input>

        <div class="historyBox">
            <!-- <div v-if="isMore" class="more" @click="handelMoreList">
                <div><i class="fa fa-clock-o" style="margin-right: 10px;"></i>
                    <span>查看更多聊天记录</span>
                </div>
            </div> -->

            <div class="_message" v-for="(item, index) in msglogList" :key="index">
                <div class="imgbox"
                    v-if="item.sender.chatUserGuId != userStore.userinfo.chatUserGuId && item.receiverGuId == chatStore.chatGroup.groupGuId">
                    <img :src=item.sender.chatUserImg alt="头像">
                </div>
                <div class="imgbox"
                    v-if="item.sender.chatUserGuId == userStore.userinfo.chatUserGuId && item.receiverGuId == chatStore.chatGroup.groupGuId">
                    <img :src=userStore.userinfo.chatUserImg alt="头像">
                </div>

                <div class="_message_content_box">
                    <div class="_message_name"
                        v-if="item.sender.chatUserGuId != userStore.userinfo.chatUserGuId && item.receiverGuId == chatStore.chatGroup.groupGuId">
                        {{ item.sender.chatUserNickName }}
                    </div>
                    <div class="_message_name"
                        v-if="item.sender.chatUserGuId == userStore.userinfo.chatUserGuId && item.receiverGuId == chatStore.chatGroup.groupGuId">
                        {{ userStore.userinfo.chatUserNickName }}
                    </div>
                    <div class="_message_content" v-html="item.chatLogContent"></div>
                </div>

                <div class="_message_time">
                    <div>{{ item.chatLogSendTime }}</div>
                </div>
            </div>

        </div>
    </el-dialog>
</template>

<script setup>
import { onMounted, ref, reactive, watch, computed } from 'vue'
import signalr from '../../utils/signalR'
import useUserStore from '../../store/modules/user'
import useSwitchStore from '../../store/modules/switch'
import useSocketCahtStore from '../../store/modules/socketchat'
import useChatStore from "../../store/modules/chat";
import modal from '../../plugins/modal';
import { addChatFriendLog, read, groupRead, chatFriendLogHistoryList, chatGroupLogHistoryList } from '../../api/chatLog/chatLog'
import { checkFriendStatus } from '../../api/login'
import { upload } from '../../api/common'
import V3Emoji from 'vue3-emoji'
import 'vue3-emoji/dist/style.css'
import { AddLocation } from '@element-plus/icons-vue'


const baseUrl = "/dev-api"
const uploadImgUrl = ref(baseUrl + "/Common/UploadFile") // 上传的图片服务器地址

const userStore = useUserStore()
const switchStore = useSwitchStore()
const SocketCahtStore = useSocketCahtStore()
const chatStore = useChatStore()

let userInfo = userStore.userinfo
let connectionID = reactive()
let selfconnectionID = reactive()
const msg = ref();
const inputRef = ref();
const titleName = ref();
let isAdd = ref(false)
let isGroupHistory = ref(false)
const isOnline = ref(false);

let isMore = ref(false);
let msglogList = ref([])
let logQueryData = ref(null)
let tcWidth = ref()

let getListData = ref({
    pageSize: 20,
    pageNum: 1,
    ChatLogType: null,
    UserGuId: userStore.userinfo.chatUserGuId,
    FriendsGuId: null,
    ChatLogContent: null
})

const optionsName = {
    'Smileys & Emotion': '笑脸&表情',
    'Food & Drink': '食物&饮料',
    'Animals & Nature': '动物&自然',
    'Travel & Places': '旅行&地点',
    'People & Body': '人物&身体',
    Objects: '物品',
    Symbols: '符号',
    Flags: '旗帜',
    Activities: '活动'
};

const customSize = {
    //   'V3Emoji-width': '300px',
    'V3Emoji-height': '20rem',
    'V3Emoji-fontSize': '1rem',
    'V3Emoji-itemSize': '2rem'
};

const appendText = (val) => {
    // console.log(val);
    inputRef.value.innerHTML += val;
};

const queryParams = ref({
    ChatUserGuid: null
})
const queryGroupParams = ref({
    ChatGroupGuid: null
})

let addData = ref({
    ChatLogType: null,
    SenderGuId: userInfo.chatUserGuId,
    ReceiverGuId: null,
    ChatLogContent: null,
})

let getNewListData = reactive({
    UserGuId: userInfo.chatUserGuId
})


watch(() => chatStore.chatUser, async (value) => {
    getListData.pageNum = 1
})

watch(() => chatStore.chatGroup, async (value) => {
    getListData.pageNum = 1
})

watch(() => logQueryData.value, async (value) => {
    // console.log(value);
    getListData.value.ChatLogContent = value
    console.log(getListData.value);
    if (switchStore.isChat) {
        getListData.value.FriendsGuId = chatStore.chatUser.friendGuId
        getfriendList()
    }
    if (switchStore.isGroupChat) {
        getListData.value.FriendsGuId = chatStore.chatGroup.groupGuId
        getGroupList()
    }
})

async function handelSend() {
    isOnline.value = false

    if (!switchStore.isChat && !switchStore.isGroupChat) {
        modal.msgWarning("快去找好友聊天吧~")
        return
    }

    if (inputRef.value.innerHTML == false || inputRef.value.innerHTML == null) {
        modal.msgError('请输入内容');
        inputRef.value.innerHTML = null
        return
    }

    queryParams.value.ChatUserGuid = chatStore.chatUser.friendGuId

    // console.log(SocketCahtStore.chatOnlineUsers, "在线人");
    //TODO : 在线人数列表查看是否有好友id ------------------------------
    SocketCahtStore.chatOnlineUsers.forEach(function (item, index) {
        // console.log(item);
        if (item.userGuid == chatStore.chatUser.friendGuId) {
            isOnline.value = true
        }
    })

    if (isOnline.value) {
        // console.log("在线发送");

        SocketCahtStore.chatOnlineUsers.forEach(function (item, index) {
            if (item.userGuid == userInfo.chatUserGuId) {
                selfconnectionID = item.connectionId
                // console.log(selfconnectionID,'自己的连接Id');
            }
            if (item.userGuid == chatStore.chatUser.friendGuId) {
                connectionID = item.connectionId
                // console.log(connectionID,'对方的连接Id');
            }
        })
        // console.log(inputRef.value.innerHTML,"xianshang asd");
        signalr.SR.invoke('SendFriendsChat', selfconnectionID, connectionID, userInfo.chatUserGuId, chatStore.chatUser.friendGuId, inputRef.value.innerHTML).catch(function (err) {
            console.error(err.toString())
        })

        addData.value.ChatLogType = 1
        // addData.value.SenderGuId = userInfo.chatUserGuId
        addData.value.ReceiverGuId = chatStore.chatUser.friendGuId
        addData.value.ChatLogContent = inputRef.value.innerHTML

        // console.log(addData.value,"----- 添加数据 ------");

        const { code, data, msgessage } = addChatFriendLog(addData.value);
        if (code == 200) {
            // chatStore.getList(getNewListData)
            inputRef.value.innerHTML = null
        }
    }
    else {
        console.log("离线---------");
        // console.log(inputRef.value,3213213);
        addData.value.ChatLogType = 1
        // addData.value.SenderGuId = userInfo.chatUserGuId
        addData.value.ReceiverGuId = chatStore.chatUser.friendGuId
        addData.value.ChatLogContent = inputRef.value.innerHTML

        // console.log(addData.value);
        const { code, data, msgessage } = addChatFriendLog(addData.value);
        if (code == 200) {
            // chatStore.getList(getNewListData)
            // modal.msgSuccess("添加成功")
            inputRef.value.innerHTML = null
        }

        setTimeout(() => {
            switchStore.notOnlineChat = true
        }, 500);

    }

    inputRef.value.innerHTML = null
}

// 群聊发送消息
async function handelGroupSend() {

    if (!switchStore.isChat && !switchStore.isGroupChat) {
        modal.msgWarning("快去找好友聊天吧~")
        return
    }

    if (inputRef.value.innerHTML == false || inputRef.value.innerHTML == null) {
        modal.msgError('请输入内容');
        inputRef.value.innerHTML = null
        return
    }

    queryGroupParams.value.ChatGroupGuid = chatStore.chatGroup.groupGuId

    // console.log("在线发送");

    SocketCahtStore.chatOnlineUsers.forEach(function (item, index) {
        if (item.userGuid == userInfo.chatUserGuId) {
            selfconnectionID = item.connectionId
            // console.log(selfconnectionID,'自己的连接Id');
        }
    })

    signalr.SR.invoke('SendGroupChat', chatStore.chatGroup.groupName, chatStore.chatGroup.groupGuId, selfconnectionID, userInfo.chatUserGuId, chatStore.chatGroup.groupGuId, inputRef.value.innerHTML).catch(function (err) {
        console.error(err.toString())
    })

    addData.value.ChatLogType = 2
    // addData.value.SenderGuId = userInfo.chatUserGuId
    addData.value.ReceiverGuId = chatStore.chatGroup.groupGuId
    addData.value.ChatLogContent = inputRef.value.innerHTML

    // console.log(addData.value,"----- 添加数据 ------");

    const { code, data, msgessage } = addChatFriendLog(addData.value);
    if (code == 200) {
        // chatStore.getList(getNewListData)
        inputRef.value.innerHTML = null
    }

    inputRef.value.innerHTML = null
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


function handleColse() {
    inputRef.value.innerHTML = null

    switchStore.isChat = false
    switchStore.isGroupChat = false
}

const handleImgSuccess = (value) => {
    if (value.code != 200) {
        modal.msgError(value.msg);
        return;
    }
    modal.msgSuccess(value.msg);
    console.log(value.data);

    msg.value = value.data.url;
    inputRef.value.innerHTML += `<img style="width: 10rem!important;height: auto;" src=${msg.value} alt="">`
    console.log(msg.value, "shusdasd");
};

function handleEntry(event) {
    let e = event.keyCode
    if (event.shiftKey && e === 13) {
    } else if (e === 13) {
        event.preventDefault()
        handelSend()
    }

}

function handleGroupEntry(event) {
    let e = event.keyCode
    if (event.shiftKey && e === 13) {
    } else if (e === 13) {
        event.preventDefault()
        handelGroupSend()
    }
}

function hanleChatHistoryLog() {

    if (switchStore.isChat) {
        isAdd.value = true
        getListData.value.FriendsGuId = chatStore.chatUser.friendGuId
        getfriendList()
    }
    if (switchStore.isGroupChat) {
        isGroupHistory.value = true
        getListData.value.FriendsGuId = chatStore.chatGroup.groupGuId
        getGroupList()
    }

}

async function getfriendList() {
    await chatFriendLogHistoryList(getListData.value).then((res) => {
        if (res.code == 200) {
            // console.log(res.data);
            msglogList.value = res.data
            msglogList.value = msglogList.value.sort(sortData)
        }
    })
}

async function getGroupList() {
    await chatGroupLogHistoryList(getListData.value).then((res) => {
        if (res.code == 200) {
            console.log(res.data);
            msglogList.value = res.data
            msglogList.value = msglogList.value.sort(sortData)
            // console.log(msglogList.value, "记录");
        }
    })
}

function sortData(a, b) {
    return new Date(a.chatLogSendTime).getTime() - new Date(b.chatLogSendTime).getTime()
}

createMediaList({
    1100(ctx) {
        if(ctx.matches){
            tcWidth.value = "100%"
        }
        else{
            tcWidth.value = "50%"
        }
    },
})

function createMediaList(opt) {
        for (let optKey in opt) {
            let mediaCtx = window !== undefined ? window.matchMedia(`(max-width: ${optKey}px)`) : global.matchMedia(`(max-width: ${optKey}px)`)
            if (mediaCtx?.matches) {
                opt[optKey](mediaCtx)
            }
            mediaCtx.addListener(opt[optKey])
        }
}

</script>

<style lang="scss" scoped>
.toolbar {
    width: 97%;
    height: 35px;
    // background-color: red;
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0 10px;
}

.tools {
    display: flex;
    height: 1.5rem;
    align-items: center;
}

.tool,
.tools img {
    margin: 0 5px;
    width: 1.5rem;
    cursor: pointer;
}

.input_send {
    width: 99%;
    height: 75px;
    border: 0;
    background: none;
    /* outline: none; */
}

.history img {
    width: 1.7rem;
    cursor: pointer;
}

.history img:hover {
    background-color: rgba(216, 216, 216, 0.4);
    transition: .3s;
    border-radius: 5px;
}

.send_box {}

/* .send_box::-webkit-scrollbar { width: 0; height: 0; color: transparent; } */
.textarea {
    overflow-x: hidden;
    box-sizing: border-box;
    height: 120px;
    width: 100%;
    outline: none;
    border: none;
    padding: 0 10px;
    border: 0;
    border-radius: 5px;
    background: none;
    font-size: 1rem;
    padding: 10px;
    resize: none;

    img {
        width: 50px;
    }
}

.btn_box {
    display: flex;
    justify-content: flex-end;

    button {
        padding: 10px;
        border: 0;
        background-color: #b0e1f8;
        border-radius: 5px;
        margin: 0 8px;
        cursor: pointer;
    }
}

.chat_img {
    width: 1rem !important;
    height: auto;
}

.historyBox {
    margin-top: 2rem;
    width: 100%;
    height: 100%;

    ._message {
        width: 100%;
        height: auto;
        display: flex;
        align-items: center;
        border-bottom: #f5f5f5 1px solid;
        position: relative;
        margin: 1rem 0;
        padding: 10px 0;

        .imgbox {
            display: flex;
            margin: 0 15px 0 0;
            align-items: center;
        }

        .imgbox img {
            width: 2.5rem;
            height: 2.5rem;
            border-radius: 10px;
            object-fit: cover;
        }

        ._message_content_box {
            width: 64%;

            ._message_name {
                font-size: 0.3rem;
                margin-bottom: 4px;
                color: rgb(121, 163, 159);
            }
        }

        ._message_time {
            position: absolute;
            right: 0;
            color: rgb(121, 163, 159);
            font-size: 0.3rem;
        }

    }
}

.more {
    width: auto;
    display: flex;
    justify-content: center;
    cursor: pointer;
    margin: 0.5rem 0;
}
</style>
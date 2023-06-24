<template>
    <div class="middlebox">
        <div class="search flex">
            <input class="search_input" type="text" v-model="friendname" placeholder="请输入好友用户名 / 群聊名称"
                @keyup.enter="hanleSearch">
            <i class="fa fa-search search_btn" @click="hanleSearch"></i>
        </div>

        <div class="shell">


            <ul v-if="!isFindFriend" class="shell-bottom">
                <li class="list" @click="handleMessage">
                    <img v-if="isMessage" src="../../assets/img/chat_active.png">
                    <img v-else src="../../assets/img/chat.png">
                </li>
                <li class="list" @click="handleFirends">
                    <img v-if="isFriends" src="../../assets/img/friendfavor_active.png">
                    <img v-else src="../../assets/img/friendfavor.png">
                </li>
                <li class="list" @click="handleGroup">
                    <img v-if="isGroup" src="../../assets/img/group_active.png">
                    <img v-else src="../../assets/img/group.png">
                </li>
            </ul>

            <div class="shell-top">
                <ul class="main">

                    <!-- 查找好友 -->
                    <li class="friend" v-if="isFindFriend">
                        <div @click="handleback" class="backer"><i class="fa fa-grav"></i>&nbsp;返回</div>

                        <div class="friendBox">
                            <div class="message_img_box messageBoxLeft">
                                <img class="message_img" :src=userImg alt="">
                            </div>
                            <div class="messageBoxRight">
                                <div class="message_info">
                                    <div>{{ username }}</div>
                                    <!-- <span v-for="(item, index) in FriendsDataList" :key="index"> -->
                                    <!-- <span>{{item.friendName}}</span> -->
                                    <!-- && item.friendName != username -->
                                    <div v-if="username != userName" class="add" @click="isAdd = true">+</div>
                                    <!-- </span> -->
                                </div>
                            </div>
                        </div>

                        <el-dialog v-model="isAdd" title="填写附言" :width=tcWidth>
                            <el-input v-model="postscript" placeholder="请输入附言"></el-input>
                            <template #footer>
                                <span class="dialog-footer">
                                    <el-button @click="isAdd = false">取消</el-button>
                                    <el-button type="primary" @click="hanleApply">确定</el-button>
                                </span>
                            </template>
                        </el-dialog>
                    </li>

                    <!-- 消息 -->
                    <li v-if="isMessage && !isFindFriend">
                        <div v-for="(item, index) in chatStore.chatLogList" :key="index">
                            <div class="messageBox" v-if="item.chatUserObject.friendGuId != '0'"
                                @click="handelChat(item.chatUserObject, item.chatUserObject.friendGuId)">
                                <div class="message_img_box messageBoxLeft">
                                    <div class="point" v-if="item.isRead == false"></div>
                                    <img class="message_img" :src=item.chatUserObject.friendImg alt="">
                                </div>
                                <div class="messageBoxRight">
                                    <div class="message_info">
                                        <div v-if="item.chatUserObject.friendNote != null">
                                            {{
                                                item.chatUserObject.friendNote.length >= 20
                                                ? item.chatUserObject.friendNote.substr(0, 20) + "..."
                                                : item.chatUserObject.friendNote }}</div>
                                        <div v-else>
                                            {{
                                                item.chatUserObject.friendNickName.length >= 20
                                                ? item.chatUserObject.friendNickName.substr(0, 20) + "..."
                                                : item.chatUserObject.friendNickName }}
                                        </div>
                                    </div>
                                    <div class="message_end">
                                        <div style="font-size: 12px;color: #79a39f;">
                                            {{
                                                item.chatLogContent.length && ((tag, len) => {
                                                    let imgReg = /\<img /g, brReg = /\<br\>/g,
                                                        spaceReg = /nbsp;/g
                                                    imgMatch = tag.match(imgReg),
                                                        brMatch = tag.match(brReg),
                                                        spaceMatch = tag.match(spaceReg),
                                                        rtxSwitch = (tag) => {
                                                            let rtx = tag,
                                                                match1 = [{ m: rtx.match(/lt;/g), r: '<' }, { m: rtx.match(/gt;/g), r: '>' }],
                                                                match2 = rtx.match(/nbsp;/g)
                                                                match3 = rtx.match(/<img /g) 
                                                                match4 = rtx.match(/\<br\>/g)
                                                            // rtx = rtx.replace(/lt;/g, '')
                                                            // rtx = rtx.replace(/gt;/g, '')
                                                            // rtx = rtx.replace(/quot;/g, "'")
                                                            // rtx = rtx.replace(/ldquo;/g, "")
                                                            // rtx = rtx.replace(/amp;/g, "")
                                                            // rtx = rtx.replace(/rdquo;/g, "")
                                                            // rtx = rtx.replace(/nbsp;/g, "");
                                                            // consolea.log(match[1])
                                                            if (match3) {
                                                                return '[图片]'
                                                            }
                                                            match1.map(item => {
                                                                if (item.m) {
                                                                    rtx = rtx.replaceAll('&' + item.m[0], item.r)
                                                                    // len+='&'+item[0]
                                                                }
                                                            })
                                                            match2 && (() => {
                                                                rtx = rtx.replaceAll('&' + match2[0], '')
                                                                // len += match2[0].split(',') + match2.length
                                                            })()
                                                            match4 && (() => {
                                                                rtx = rtx.replaceAll(match4[0], '')
                                                                // len += match4[0].split(',')
                                                            })()
                                                            return rtx
                                                        }
                                                    res = rtxSwitch(String(tag))
                                                    if (len > 7) {
                                                        res = res.substr(0, 7) + '...'
                                                    } else {
                                                        res = res
                                                    }
                                                    return res
                                                })(item.chatLogContent, item.chatLogContent.length)
                                            }}
                                        </div>
                                        <div style="color: #79a39f;font-size: 10px;">{{ item.chatLogSendTime }}</div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- 群聊最新消息 -->
                        <div v-for="(item, index) in chatStore.chatLogList" :key="index">

                            <div class="messageBox" v-if="item.chatUserObject.friendGuId == '0'"
                                @click="handelGroupChat(item.groupObject)">
                                <div class="message_img_box messageBoxLeft">
                                    <div class="point" v-if="item.groupObject.groupIsRead == false"></div>
                                    <img class="message_img" :src=item.groupObject.groupImg alt="">
                                </div>
                                <div class="messageBoxRight">
                                    <div class="message_info">
                                        <div v-if="item.groupObject.groupName != null">
                                            {{
                                                item.groupObject.groupName.length >= 20
                                                ? item.groupObject.groupName.substr(0, 20) + "..."
                                                : item.groupObject.groupName }}</div>
                                    </div>
                                    <div class="message_end">
                                        <div style="font-size: 12px;color: #79a39f;">
                                            {{
                                                item.chatLogContent.length && ((tag, len) => {
                                                    let imgReg = /\<img /g, brReg = /\<br\>/g,
                                                        spaceReg = /nbsp;/g
                                                    imgMatch = tag.match(imgReg),
                                                        brMatch = tag.match(brReg),
                                                        spaceMatch = tag.match(spaceReg),
                                                        rtxSwitch = (tag) => {
                                                            let rtx = tag,
                                                                match1 = [{ m: rtx.match(/lt;/g), r: '<' }, { m: rtx.match(/gt;/g), r: '>' }],
                                                                match2 = rtx.match(/nbsp;/g)
                                                                match3 = rtx.match(/<img /g)
                                                                match4 = rtx.match(/\<br\>/g)
                                                            // rtx = rtx.replace(/lt;/g, '')
                                                            // rtx = rtx.replace(/gt;/g, '')
                                                            // rtx = rtx.replace(/quot;/g, "'")
                                                            // rtx = rtx.replace(/ldquo;/g, "")
                                                            // rtx = rtx.replace(/amp;/g, "")
                                                            // rtx = rtx.replace(/rdquo;/g, "")
                                                            // rtx = rtx.replace(/nbsp;/g, "");
                                                            // consolea.log(match[1])
                                                            if (match3) {
                                                                return '[图片]'
                                                            }
                                                            match1.map(item => {
                                                                if (item.m) {
                                                                    rtx = rtx.replaceAll('&' + item.m[0], item.r)
                                                                    // len+='&'+item[0]
                                                                }
                                                            })
                                                            match2 && (() => {
                                                                rtx = rtx.replaceAll('&' + match2[0], '')
                                                                // len += match2[0].split(',') + match2.length
                                                            })()
                                                            match4 && (() => {
                                                                rtx = rtx.replaceAll(match4[0], '')
                                                                // len += match4[0].split(',')
                                                            })()
                                                            return rtx
                                                        }
                                                    res = rtxSwitch(String(tag))
                                                    if (len > 7) {
                                                        res = res.substr(0, 7) + '...'
                                                    } else {
                                                        res = res
                                                    }
                                                    return res
                                                })(item.chatLogContent, item.chatLogContent.length)
                                            }}
                                        </div>
                                        <div style="color: #79a39f;font-size: 10px;">{{ item.chatLogSendTime }}</div>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="nullSty" v-if="chatStore.chatLogList.length == 0">
                            <img style="margin-top: 30px" src="../../assets/img/1.png">
                            <div class="box-one"></div>
                            <div class="box-two"></div>
                            <div class="box-one"></div>
                            <div>快去找好友聊天哟~</div>
                        </div>
                    </li>

                    <!-- 好友 -->
                    <li v-if="isFriends && !isFindFriend">
                        <div class="newFriendBox" @click="switchStore.isFriends = !switchStore.isFriends">
                            <div>新朋友</div>
                            <div class="newFriendIcon">></div>
                        </div>
                        <div class="messageBox" v-for="(item, index) in FriendsDataList" :key="index">
                            <div class="message_img_box messageBoxLeft" @click="showFriendIntro(item)">
                                <img class="message_img" :src=item.friendImg alt="">
                            </div>
                            <div class="messageBoxRight" @click="handelChat(item, item.friendGuId)">
                                <div class="message_info">
                                    <div v-if="item.friendNote != null">{{ item.friendNote }}</div>
                                    <div v-else>{{ item.friendNickName }}</div>
                                </div>
                            </div>
                        </div>
                        <div class="nullSty" v-if="FriendsDataList.length == 0">
                            <img style="margin-top: 30px" src="../../assets/img/2.png">
                            <div class="box-one"></div>
                            <div class="box-two"></div>
                            <div class="box-one"></div>
                            <div>暂无好友哟~</div>
                        </div>

                    </li>

                    <!-- 群聊 -->
                    <li v-if="isGroup && !isFindFriend">
                        <div class="flex">
                            <el-button class="addGroup_btn" type="primary" @click="isAddGroup = true">+ 创建群聊</el-button>
                        </div>
                        <div class="messageBox" v-for="(item, index) in GroupDataList" :key="index"
                            @click="handelGroupChat(item)">
                            <div class="message_img_box messageBoxLeft">
                                <img class="message_img" :src=item.groupImg alt="">
                            </div>
                            <div class="groupBoxRight">
                                <div class="message_info">
                                    <div>{{ item.groupName }}</div>
                                    <span style="color: #79a39f;font-size: 13px;">{{ item.groupNum }}</span>
                                </div>
                            </div>
                        </div>
                        <div class="nullSty" v-if="GroupDataList.length == 0">
                            <img src="../../assets/img/3.png">
                            <div class="box-one"></div>
                            <div class="box-two"></div>
                            <div class="box-one"></div>
                            <div>暂无群聊唷~</div>
                        </div>
                    </li>

                    <el-dialog v-model="isAddGroup" title="添加群聊" :width=tcWidth>
                        <el-row>
                            <el-col style="display: flex;justify-content: center;margin: 15px 0 30px 0;">
                                <UploadImage ref="uploadRef" v-model="addGroupData.GroupImg" :limit="1" :fileSize="5"
                                    :drag="true" />
                            </el-col>
                        </el-row>
                        <el-row>
                            <el-input v-model="addGroupData.GroupName" placeholder="请输入群名称"></el-input>
                        </el-row>

                        <template #footer>
                            <span class="dialog-footer">
                                <el-button @click="isAddGroup = false">取消</el-button>
                                <el-button type="primary" @click="addGroup">确定</el-button>
                            </span>
                        </template>
                    </el-dialog>

                </ul>
            </div>
        </div>

    </div>
</template>

<script setup>

import { ref, reactive, watch } from 'vue';
import { findFriends, friendsList } from '../../api/friends/friend'
import { sendApply } from '../../api/friends/apply'
import { createGroup, groupList } from '../../api/group/group'
import { getChatLogList, read, groupRead } from '../../api/chatLog/chatLog'
import moadl from '../../plugins/modal'
import useUserStore from '../../store/modules/user'
import { ElMessage, ElMessageBox, ElNotification, ElLoading } from 'element-plus'
import img from '../../assets/img/profile.jpg'
import useSwitchStore from "../../store/modules/switch";
import useChatStore from "../../store/modules/chat";
import signalr from '../../utils/signalR'
import useSocketCahtStore from '../../store/modules/socketchat'

const SocketCahtStore = useSocketCahtStore()
const consolea = console
let friendname = ref();
let findFriendsArr = ref({});
let isFindFriend = ref(false);
let isAdd = ref(false);
let isAddGroup = ref(false);
const userStore = useUserStore()
const switchStore = useSwitchStore()
const chatStore = useChatStore()

let isMessage = ref(true);
const messageArr = ref([]);
let isFriends = ref(false);
const friendsArr = ref([]);
let isGroup = ref(false);

const groupArr = ref([]);
const postDacta = ref([]);
const msgList = ref([]);
let postscript = ref();
let username = ref();
let userImg = ref();
let groupNum = ref();
let selfconnectionID = reactive()
let tcWidth = ref()

const chatLogList = ref([]);

let userName = userStore.userinfo.chatUserName

const applyFriendData = ref({
    SenderGuId: userStore.userinfo.chatUserGuId,
    ReceiverGuId: null,
    Postscript: null,
    IsGroup: false
});

const addGroupData = reactive({
    UserGuId: userStore.userinfo.chatUserGuId,
    GroupName: null,
    GroupImg: null,
})

let getListData = ref({
    pageSize: 20,
    pageNum: 1,
    UserGuId: userStore.userinfo.chatUserGuId,
    FriendsGuId: null,
})

const readData = reactive({
    UserGuId: userStore.userinfo.chatUserGuId,
    FriendsGuId: null,
})

function handleMessage() {
    isMessage.value = true;
    isFriends.value = false;
    isGroup.value = false;
    // messageArr = data
}

function handleFirends() {
    isFriends.value = true;
    isMessage.value = false;
    isGroup.value = false;
    // messageArr = data
}

function handleGroup() {
    isGroup.value = true;
    isFriends.value = false;
    isMessage.value = false;
    // messageArr = data
}

// 搜索
async function hanleSearch() {
    if (friendname.value == undefined) {
        ElMessage.error('请输入好友的用户名')
        return
    }

    const { code, data } = await findFriends(friendname.value);
    if (code == 200) {
        ElMessage.success('查找成功')
        findFriendsArr.value = data

        if (data.chatUserGuId != null) {
            applyFriendData.value.ReceiverGuId = data.chatUserGuId
            username.value = data.chatUserName
            userImg.value = data.chatUserImg
        }
        else {
            applyFriendData.value.ReceiverGuId = data.groupGuId
            applyFriendData.value.IsGroup = true
            username.value = data.groupName
            userImg.value = data.groupImg
        }

        isFindFriend.value = true;
    }
    else {
        modal.msgError('该用户不存在')
    }

}

// 返回
function handleback() {
    isFindFriend.value = false;
    friendname.value = null
}

// 提交申请
async function hanleApply() {
    applyFriendData.value.Postscript = postscript.value
    isAdd.value = false

    const { code, data, msg } = await sendApply(applyFriendData.value);
    if (code == 200) {
        ElMessage.success(data)
    }
    else {
        modal.msgError(data)
    }
}

// 添加群聊
async function addGroup() {
    const { code, data, msg } = await createGroup(addGroupData);
    if (code == 200) {
        ElMessage.success(data)
        isAddGroup.value = false
    }
    else {
        modal.msgError(data)
    }
}

// 列表数据----------------------------------------------
const FriendsDataList = ref([])
const GroupDataList = ref([])
let userGuid = userStore.userinfo.chatUserGuId
const queryParams = ref({
    UserGuId: userGuid
})

// 好友列表
async function getFriendsList() {
    await friendsList(queryParams.value).then((res) => {
        if (res.code == 200) {
            FriendsDataList.value = res.data.result
        }
    })
}

// 群聊列表
async function getGroupList() {
    await groupList(queryParams.value).then((res) => {
        if (res.code == 200) {
            GroupDataList.value = res.data.result
        }
    })
}


// 简介
async function showFriendIntro(v) {
    // console.log(v);
    switchStore.isFriendIntro = true
    switchStore.isPersonal = false
    switchStore.isFriends = false
    switchStore.isChat = false
    switchStore.isGroupChat = false

    switchStore.FriendIntroData = v
}



watch(async (v) => {
    await getFriendsList();
    await getGroupList();
    await chatStore.getList(getListData.value);
    // console.log(chatStore.chatLogList);
    // signalr.SR.on("ReceiveMessage", data => {
    //     msgList.value.push(data);
    //     chatStore.getList(getListData.value)
    // });
},
    {
        immediate: true,
        deep: true
    })

watch(() => msgList.value, async (value) => {
    // console.log(value, "在线消息11");
    if (value != null) {
        await chatStore.getList(getListData.value)
    }
},
    {
        immediate: true,
        deep: true
    })



//  开始聊天------------
async function handelChat(v, v2, type) {
    switchStore.isFriendIntro = false
    switchStore.isPersonal = false
    switchStore.isFriends = false
    switchStore.isGroupChat = false

    readData.FriendsGuId = v2
    const { code } = await read(readData);

    chatStore.getList(getListData.value)
    // console.log(v);
    chatStore.chatUser = v
    // console.log(chatStore.chatUser);
    switchStore.isChat = true


}

async function handelGroupChat(v) {
    // await signalr.init(import.meta.env.VITE_APP_SOCKET_API)

    switchStore.isChat = false
    switchStore.isFriendIntro = false
    switchStore.isPersonal = false

    setTimeout(() => {
        switchStore.isGroupChat = true
        chatStore.chatGroup = v
    }, 500);

    chatStore.getList(getListData.value)

    readData.FriendsGuId = v.groupGuId
    const { code } = await groupRead(readData);

    // console.log(v.groupName);
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

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Bebas+Neue&display=swap');

.flex {
    display: flex;
    justify-content: center;
    align-items: center;
}

.search_btn {
    cursor: pointer;
    color: #bebcb9;
    font-size: 1.3rem;
    position: absolute;
    right: 30px;
}

.backer {
    font-size: 1.2rem;
    display: flex;
    align-items: center;
    font-family: 'Bebas Neue', cursive;
    cursor: pointer;
    margin: 15px 0;
}

.middlebox {
    width: 330px;
    height: 680px;
    padding: 10px;
    overflow: hidden;
}

.search {
    margin-top: 5px;
    position: relative;
}

.search_input {
    width: 90%;
    height: 40px;
    padding-left: 10px;
    font-size: 15px;
    border: 0;
    border-radius: 5px;
    outline: transparent;
    box-shadow: 1px 2px 5px rgba(0, 0, 0, 0.4);
}

* {
    padding: 0;
    margin: 0;
    list-style: none;
    transition: .5s;
}

.messageBox {
    width: 90%;
    height: 80px;
    /* background-color: #add4d0; */
    display: flex;
    align-items: center;
    padding-left: 5%;
    border-radius: 10px;
    cursor: pointer;
    margin: 5px 0;
}

.messageBox:hover {
    transition: .3s;
    background-color: rgba(157, 172, 225, 0.5);
}

.friendBox {
    width: 90%;
    height: 80px;
    /* background-color: #add4d0; */
    display: flex;
    align-items: center;
    padding-left: 5%;
    border-radius: 10px;
}

.friendBox:hover {
    transition: .3s;
    background-color: rgba(157, 172, 225, 0.5);
}

.message_img_box {
    /* overflow: hidden; */
    position: relative;
}

.message_img {
    width: 50px !important;
    height: 50px !important;
    background-color: #fff;
    border-radius: 50%;
    overflow: hidden;
    object-fit: cover;
    /* border-radius: 50%!important; */
}

.messageBoxRight {
    display: flex;
    flex-direction: column;
    width: 80%;
    height: 60%;
    margin-left: 10px;
}

.groupBoxRight {
    display: flex;
    flex-direction: column;
    width: 80%;
    margin-left: 10px;
}

.message_info {
    display: flex;
    width: 90%;
    justify-content: space-between;
}

.point {
    position: absolute;
    top: 0;
    right: -3px;
    width: 12px;
    height: 12px;
    background-color: red;
    border-radius: 50%;
    z-index: 99;
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: 5px;
    color: #fff;
}

/* .message_end{
    justify-content: flex-end;
} */
.message_end {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-top: 5px;
    width: 95%;
}

.shell {
    width: 94%;
    height: 90%;
    background-color: #fff;
    box-shadow: 1px 2px 10px rgba(0, 0, 0, 0.4);
    display: flex;
    flex-direction: column;
    margin-top: 20px;
    margin-left: 10px;
    border-radius: 10px !important;
    overflow-y: scroll;
}

.shell::-webkit-scrollbar {
    width: 0;
    height: 0;
    color: transparent;
}

.shell-top {
    width: 100%;
    height: 100%;
    border-bottom: 1px solid rgba(223, 223, 223);
    overflow-x: hidden;
}

.shell-top ul {
    width: 100%;
    height: 100%;
    display: flex;
    position: relative;
    transition: .3s;
    left: 0;
}

.shell-top ul li {
    width: 100%;
    /* display: flex; */
    /* flex-direction: column; */
    /* align-items: center; */
    margin: 0 0 0 10px;
    overflow-y: scroll;
    position: relative;
}

.shell-top ul li::-webkit-scrollbar {
    width: 0;
    height: 0;
    color: transparent;
}


.friend {
    width: 100%;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: flex-start !important;
    animation: ani 1s;
}

.shell-top ul li img {
    width: 150px;
}

.shell-top ul li .box-one {
    width: 70%;
    height: 10px;
    border-radius: 10px;
    background-color: rgb(220, 220, 220);
    margin: 15px 0;
}

.shell-top ul li .box-two {
    width: 50%;
    height: 10px;
    border-radius: 10px;
    background-color: rgb(220, 220, 220);
}

.shell-bottom {
    width: 100%;
    display: flex;
    align-items: center;
    justify-content: space-evenly;
    height: 58px;
}

.shell-bottom li {
    height: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    filter: grayscale(0.5)
}

.shell-bottom li img {
    width: 30px;
    cursor: pointer;
}

.add {
    font-size: 2.5rem;
    cursor: pointer;
}

.dialog-footer button {
    width: 50px;
}

.nullSty {
    display: flex;
    flex-direction: column;
    align-items: center;
    margin-top: 15px;
}

.addGroup_btn {
    width: 100px;
    letter-spacing: 1px;
    font-size: 13.5px;
}

.main li {
    animation: ani 1s;
}

.newFriendBox{
    width: 100%;
    height: 50px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0 20px;
    cursor: pointer;
    position: relative;
}
.newFriendIcon{
    position: absolute;
    right: 15%;
}

@keyframes ani {
    0% {
        transform: translateX(600px);
    }

    100% {
        transform: translateX(0px);
    }
}

@media (max-width: 1100px) {
    .middlebox {
        width: 100% !important;
        padding: 0 !important;
        margin-top: 20px;
    }
}
</style>
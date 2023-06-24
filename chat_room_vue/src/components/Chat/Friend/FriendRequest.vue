<template>
    <div class="container1">
        <div class="topbar">
            <div>好友申请</div>
            <div @click="back" class="backer"><i class="fa fa-grav"></i>&nbsp;返回</div>
        </div>

        <div class="Box">
            <div class="requestBox" v-for="(item,index) in dataList" :key="index">
                <div class="imgbox">
                    <img :src=item.senderImg alt="头像">
                </div>
                <div class="info">
                    <div class="info_name">
                        <span>{{  item.senderName  }}</span>
                        &nbsp;
                        <span class="agreetxt">{{  item.sendTime  }}</span>
                    </div>
                    <div style="margin-top: 8px;">
                        <span v-if="item.isAgree == null && item.isGroupApply == null">申请添加你为好友</span>
                        <span v-if="item.isAgree == true && item.isGroupApply == null">申请添加你为好友</span>

                        <span v-if="item.isAgree == null && item.isGroupApply == true">申请添加你的群聊</span>
                        <span v-if="item.isAgree == true && item.isGroupApply == true">申请添加你的群聊</span>

                        <span v-if="item.isAgree == false && item.senderGuId != userGuid">已拒绝</span>
                        <span v-if="item.isAgree == true && item.senderGuId == userGuid">对方已拒绝</span>
                        &nbsp;
                        <span class="agreetxt" v-if="item.isAgree == null">附言：{{  item.postscript  }}</span>
                        <span class="agreetxt" v-if="item.isAgree == false">回复：{{  item.reply  }}</span>
                    </div>
                </div>

                <div class="requestBtnBox">
                    <el-button v-if="item.isAgree == null && item.isGroupApply == null" class="request_btn" type="primary" @click="addFriendDig(item.senderGuId)">同意</el-button>
                    <el-button v-if="item.isAgree == null && item.isGroupApply == true" class="request_btn" type="primary" @click="addGroup(item.senderGuId)">同意</el-button>
                    <el-button v-if="item.isAgree == null" class="request_btn" @click="RefuseDig(item.senderGuId)">拒绝</el-button>
                    <div class="agreetxt" v-if="item.isAgree == true">已通过</div>
                    <div class="agreetxt" v-if="item.isAgree == false">已拒绝</div>
                </div>

                <el-dialog v-model="isAdd" title="填写备注" :width=tcWidth @close="isAdd = false" :modal="false">
                    <el-input v-model="addFriendData.FriendsNote"></el-input>
                    <template #footer>
                        <span class="dialog-footer">
                            <el-button @click="isAdd = false">取消</el-button>
                            <el-button type="primary" @click="addFriend()">确定</el-button>
                        </span>
                    </template>
                </el-dialog>

                <el-dialog v-model="isRefuse" title="填写回复" :width=tcWidth @close="isRefuse = false">
                    <el-input v-model="addFriendData.Reply"></el-input>
                    <template #footer>
                        <span class="dialog-footer">
                            <el-button @click="isRefuse = false">取消</el-button>
                            <el-button type="primary" @click="handelRefuse(item.senderGuId)">确定</el-button>
                        </span>
                    </template>
                </el-dialog>

            </div>
        </div>

    </div>
</template>

<script setup>
import useSwitchStore from "../../../store/modules/switch";
import useUserStore from '../../../store/modules/user'
import img from '../../../assets/img/profile.jpg'
import { applyList } from '../../../api/friends/apply'
import { add } from '../../../api/friends/friend'
import { ad } from '../../../api/group/group'
import moadl from '../../../plugins/modal'
import { ref, reactive } from 'vue'

const userStore = useUserStore()
const switchStore = useSwitchStore()
let userGuid = userStore.userinfo.chatUserGuId
const queryParams = ref({
    UserGuId: userGuid
})
const dataList = ref([])
let userImg = ref();
let isAdd = ref(false)
let isRefuse = ref(false)

let addFriendData = reactive({
    UserGuId: userGuid,
    FriendsGuId: "",
    FriendsNote: null,
    isAgree: null,
    Reply: null,
})

let addGroupData = reactive({
    GroupManagerGuId: userGuid,
    UserGuId: "",
    isAgree: null,
    Reply: null,
})

// 查询数据
async function getList() {
    await applyList(queryParams.value).then((res) => {
        if (res.code == 200) {
            dataList.value = res.data.result
        }
    })
}
getList();

let uid = ref()
function addFriendDig(v){
    isAdd.value = true
    uid.value = v
}

function RefuseDig(v){
    isRefuse.value = true
    uid.value = v
}

// 添加好友
async function addFriend() {
    addFriendData.FriendsGuId = uid.value
    addFriendData.isAgree = true
    await add(addFriendData).then((res) => {
        if (res.code == 200) {
            moadl.msgSuccess('添加成功，请刷新查看！')
            isAdd.value = false
            getList()
        }
    })
}

// 添加群聊
async function addGroup(v) {
    addGroupData.UserGuId = v
    addGroupData.isAgree = true
    await ad(addGroupData).then((res) => {
        if (res.code == 200) {
            moadl.msgSuccess('申请通过，请刷新查看！')
        }
    })
}

// 拒绝申请
async function handelRefuse(v) {
    addFriendData.FriendsGuId = uid.value
    addFriendData.isAgree = false
    await add(addFriendData).then((res) => {
        if (res.code == 200) {
            moadl.msgSuccess('已拒绝对方的请求')
            isRefuse.value = false
        }
    })
}

function back() {
    switchStore.isPersonal = false
    switchStore.isFriends = false
}


let tcWidth = ref()
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
.container1 {
    width: 100%;
    height: 100%;
    animation: ani 1s;
}

@keyframes ani {
    0% {
        transform: translateX(600px);
    }

    100% {
        transform: translateX(0px);
    }
}

.topbar {
    width: 95%;
    height: 70px;
    // background-color: red;
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 0 20px;
    color: grey;
    font-size: 1.5rem;
    font-family: 'Bebas Neue', cursive;
    border-bottom: rgba(121, 163, 159, 0.3) 1px solid;
    letter-spacing: 2px;
}

.backer {
    font-size: 1.2rem;
    display: flex;
    align-items: center;
    cursor: pointer;
}

.Box {
    width: 100%;
    height: 90%;
    overflow: hidden;
    overflow-y: scroll;
    // background-color: red;
    // background: rgb(red, green, blue);
}
.Box::-webkit-scrollbar { width: 0; height: 0; color: transparent; }

.requestBox {
    width: 100%;
    height: 90px;
    // background-color: red;
    display: flex;
    align-items: center;
    border-bottom: rgba(121, 163, 159, 0.3) 1px solid;
    position: relative;
}
.el-overlay{
     background-color: rgba(247, 247, 247, 0.2)!important;
     backdrop-filter: blur(10px) !important;
}
.imgbox {
    display: flex;
    margin: 0 15px;
    align-items: center;
}

.imgbox img {
    width: 70px;
    height: 70px;
    border-radius: 10px;
    object-fit: cover;
}

.info {
    font-size: 15px;
}

.requestBtnBox {
    position: absolute;
    right: 1rem;
}

.request_btn {
    width: 80px;
    height: 45px;
}
.agreetxt{
    font-size: 13px!important;
    color: #79a39f;
}
</style>
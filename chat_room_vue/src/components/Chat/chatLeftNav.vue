<template>
    <nav class="shell-header">
        <div class="buttons" style="margin: 0px">
            <div class="shell-header-box">
                <div class="head" @click="clickPersonal">
                    <img :src="userinfo.chatUserImg" alt="">
                    <div class="head-tetx" style="text-align: center;" v-if="userinfo.chatUserNickName != null">{{
                        userinfo.chatUserNickName.length >= 4
                        ? userinfo.chatUserNickName.substr(0, 3) + "..."
                        : userinfo.chatUserNickName
                    }}</div>
                    <div style="text-align: center;" v-else>{{
                        userinfo.chatUserName.length >= 4
                        ? userinfo.chatUserName.substr(0, 3) + "..."
                        : userinfo.chatUserName
                    }}</div>
                </div>
            </div>

            <div class="shell-header-box tool-box" @click="clickFriends(), handleRead()">
                <div class="tool">
                    <div v-if="isReadValue == false" class="point"></div>
                    <img src="../../assets/img/addfirend.png" alt="">
                </div>
            </div>

            <div class="shell-header-box" @click="loginout">
                <div class="set">
                    <img src="../../assets/img/loginout.png" alt="">
                </div>
            </div>

        </div>
    </nav>
</template>

<script setup>
import { ref, reactive, watch } from 'vue';
import useUserStore from '../../store/modules/user';
import useSwitchStore from "../../store/modules/switch";
import { applyList, checkApply, read } from '../../api/friends/apply'
import { useRoute, useRouter } from 'vue-router'
import moadl from '../../plugins/modal'
import Cookies from 'js-cookie'

const router = useRouter()
const userStore = useUserStore()
const switchStore = useSwitchStore()
let userGuid = userStore.userinfo.chatUserGuId
const queryParams = ref({
    UserGuId: userGuid
})
const dataList = ref([])
const postData = reactive({
    userGuid: userStore.userinfo.chatUserGuId
})

const userinfo = ref({ ...userStore.userinfo });

function clickPersonal() {
    switchStore.isGroupChat = false
    switchStore.isChat = false
    switchStore.isFriendIntro = false
    switchStore.isFriends = false
    switchStore.isPersonal = true
}

function clickFriends() {
    switchStore.isGroupChat = false
    switchStore.isChat = false
    switchStore.isFriendIntro = false
    switchStore.isPersonal = false
    switchStore.isFriends = true
}

let isReadValue = ref(true);

watch(() => userStore.userinfo, async (value) => {
    // console.log(value)
    userinfo.value = value
    // console.log(formData.value);
})

// 查询数据
async function getList() {
    await applyList(queryParams.value).then((res) => {
        if (res.code == 200) {
            dataList.value = res.data.result
            // console.log(dataList.value);
            // console.log(dataList.value[dataList.value.length -1].isRead);
            if (dataList.value.length != 0) {
                // console.log(dataList.value[dataList.value.length - 1])
                isReadValue.value = dataList.value[dataList.value.length - 1].isRead
            }
        }
    })
}
getList();

async function handleRead() {

    const { code, data, msg } = await read(postData);
    if (code == 200) {
        // chatLogList.value = data
        // console.log(data,'asdasds');
        await getList();
        // console.log(chatLogList.value,'asdasds');
    }
}

function loginout() {
    moadl.msgSuccess("登出成功！")

    router.push({
        path: "/login"
    })

    Cookies.remove('username')
    localStorage.clear()

    setTimeout(() => {
        location.reload()
    }, 500);
}
// const chatUserName = useUserStore

// getInfo();
</script>

<style>
.shell-header {
    position: relative;
    width: 90px;
    height: 700px;
    border-top-left-radius: 10px;
    border-bottom-left-radius: 10px;
    background-color: #8a98c9;

}

.buttons {
    width: 100%;
    height: 100%;
    display: flex;
    flex-direction: column;
    justify-content: space-evenly;
    align-items: center;
    color: #fff;
}

ul {
    margin: 0 !important;
}

.head {
    position: absolute;
    left: 20%;
    top: 2%;
    cursor: pointer;
}

.head img {
    width: 60px;
    height: 60px;
    object-fit: cover;
    border-radius: 50%;
}

.tool {
    /* position: absolute; */
    /* left: 33%; */
    /* top: 25%; */
}

.tool img,
.set img {
    width: 33px;
    cursor: pointer;
}

.point {
    position: absolute;
    top: 0;
    right: 0px;
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

.set {
    /* position: absolute; */
    /* left: 30%; */
    /* bottom: 2%; */
}

.shell-header-box {
    letter-spacing: 2px;
    font: 600 14px '';
    transition: .3s;
    list-style: none;
}

@media (max-width: 1100px) {
    .shell-header {
        width: 100% !important;
        height: 72px !important;
        border-radius: 0 !important;
    }

    .buttons {
        justify-content: space-between;
        flex-direction: row !important;
    }

    .tool-box {
        display: none !important;
    }

    .set {
        margin-right: 30px;
    }

    .head {
        display: flex;
        align-items: center;
        top: 10%;
        left: 5%;
    }
    .head-tetx{
        margin-left: 10px;
    }
}
</style>

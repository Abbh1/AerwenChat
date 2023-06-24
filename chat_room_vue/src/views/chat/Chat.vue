<template>
    <!-- <ChatBackground></ChatBackground> -->
    <main class="pc-box">
        <div class="box">
            <div class="m-box">
                <ChatLeftNav></ChatLeftNav>
                <ChatMiddleNav></ChatMiddleNav>
                <div class="chatContent">
                    <PersonalCenter v-if="switchStore.isPersonal"></PersonalCenter>
                    <FriendCenter v-else-if="switchStore.isFriendIntro"></FriendCenter>
                    <FriendRequest v-else-if="switchStore.isFriends"></FriendRequest>
                    <ChatContent v-else></ChatContent>
                </div>
            </div>
        </div>
    </main>


    <div class="phone-box">
        <div class="phone-m-box">
            <ChatLeftNav></ChatLeftNav>
            <ChatMiddleNav
                v-if="!switchStore.isChat && !switchStore.isGroupChat && !switchStore.isPersonal && !switchStore.isFriendIntro && !switchStore.isFriends">
            </ChatMiddleNav>
            <div class="chatContent">
                <PersonalCenter v-if="switchStore.isPersonal"></PersonalCenter>
                <FriendCenter v-else-if="switchStore.isFriendIntro"></FriendCenter>
                <FriendRequest v-else-if="switchStore.isFriends"></FriendRequest>
                <ChatContent v-if="switchStore.isChat || switchStore.isGroupChat"></ChatContent>
            </div>
        </div>

        <!-- <div class="phone-bottom-pos-box">
                <div class="phone-bottom-box">
                    <div class="phone-bottom-img-box">
                        <img src="../../assets/img/chat_active.png">
                    </div>
                    <div class="phone-bottom-text">聊天</div>
                </div>

                <div class="phone-bottom-box">
                    <div class="phone-bottom-img-box">
                        <img src="../../assets/img/friendfavor_active.png">
                    </div>
                    <div class="phone-bottom-text">好友</div>
                </div>

            </div> -->
    </div>
</template>

<script setup>
import ChatBackground from '../../components/Chat/chatBackground.vue'
import ChatLeftNav from '../../components/Chat/chatLeftNav.vue'
import ChatMiddleNav from '../../components/Chat/chatMiddleNav.vue'
import ChatContent from '../../components/Chat/chatContent.vue'
import PersonalCenter from '../../components/Chat/Personal/personalCenter.vue'
import FriendCenter from '../../components/Chat/Personal/friendCenter.vue'
import FriendRequest from '../../components/Chat/Friend/FriendRequest.vue'
import useUserStore from '../../store/modules/user'
import useSwitchStore from '../../store/modules/switch'
import Cookies from 'js-cookie'
import { watch, ref, onBeforeMount } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import moadl from '../../plugins/modal'

const userStore = useUserStore()
const switchStore = useSwitchStore()

let userJsonStr = sessionStorage.getItem('userInfo');
let info = JSON.parse(userJsonStr);
const router = useRouter()

watch(async (v) => {
    if (sessionStorage.getItem('userInfo') == null) {
        moadl.msgError("请登录！")
        router.push({
            path: "/login"
        })
        setTimeout(() => {
            location.reload()
        }, 500);
        return
    }
    await userStore.getUserInfo(info.chatUserName);
});
userStore.userinfo = info



</script>


<style scoped>
.pc-box{
}

main {
    display: flex;
    justify-content: center;
    align-items: center;
    min-height: 98vh;
    background-image: linear-gradient(to top, #fff1eb 0%, #ace0f9 100%);
}

.box {
    width: 1100px;
    height: 700px;
    background-color: #f5f5f5;
    border-radius: 10px;
}

.m-box {
    display: flex;
    width: 100%;
    height: 100%;
}

.chatContent {
    width: 63%;
    height: 100%;
    /* background-color: red; */
    /* border-left: rgba(121, 163, 159, 0.3) 1px solid; */
    overflow: hidden;
}

/* 手机端 */
.phone-box {
    width: 100%;
    height: 100%;
    display: none;
}

.phone-bottom-pos-box {
    width: 100%;
    height: 80px;
    position: fixed;
    bottom: 0;
    background: #8a98c9;
    display: flex;
    justify-content: space-around;
    align-items: center;
    overflow: hidden;
}

.phone-m-box {
    padding-bottom: 80px;
}

.phone-bottom-box {
    /* width: 100%; */
    height: 80%;
    cursor: pointer;
    display: flex;
    justify-content: center;
    align-content: center;
    flex-direction: column;
}

.phone-bottom-img-box {}

.phone-bottom-img-box img {
    height: 40px;
}

.phone-bottom-text {
    text-align: center;
}

@media (max-width: 1100px) {
    .chatContent {
        width: 100% !important;
    }

    .pc-box {
        display: none !important;
    }

    .phone-box {
        display: block !important;
    }
}
</style>
<template>
  <body>
    <div class="container">
      <div>
        <div style="margin-bottom:80px">
          <BackGround></BackGround>
        </div>
        <!-- 输入框 -->
        <div style="display: flex;justify-content: center;">
          <div style="margin-bottom: 20px;">
            <div v-if="isRegist" class="mb15">
              <input class="login_input nick_name" v-model="nickname" type="text" placeholder="请输入你的昵称" autofocus>
            </div>
            <div class="mb15">
              <input class="login_input" type="text" v-model="username" placeholder="请输入你的账号" autofocus>
            </div>
            <div class="mb15">
              <input v-if="!isRegist" class="login_input" type="password" v-model="password" @keyup.enter="hanleLogin"
                placeholder="请输入你的密码">
              <input v-else class="login_input" type="password" v-model="password" @keyup.enter="hanleRegist"
                placeholder="请输入你的密码">
            </div>
          </div>
        </div>

        <div v-if="!isRegist" style="display: flex;justify-content: center;">
          <button @click="hanleLogin" class="login_btn">{{ lodding }}</button>
        </div>
        <div style="display: flex;justify-content: center;margin-top: 30px;">
          <button v-if="!isRegist" @click="ClcikRegist" class="login_btn regist_btn">Rsegist</button>
          <button v-else @click="hanleRegist" class="login_btn regist_btn2">{{ registLodding }}</button>
        </div>
        <div v-if="isRegist" @click="ClcikBack" style="display: flex;justify-content: center;margin-top: 10px;">
          <i class="fa fa-arrow-circle-left back"></i>
        </div>
      </div>
    </div>
  </body>
</template>

<script setup>
import BackGround from '../components/Login/loginBackgrouond.vue'
import { ref, reactive } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import Cookies from 'js-cookie'
import { login, register } from '../api/login'
import useUserStore from '../store/modules/user'
import moadl from '../plugins/modal'


const userStore = useUserStore()

const router = useRouter()
const isRegist = ref(false);

const nickname = ref();
const username = ref();
const password = ref();

const lodding = ref("Login Now");
const registLodding = ref("Regist");

async function ClcikRegist() {
  isRegist.value = true;
  nickname.value = undefined;
  username.value = undefined;
  password.value = undefined;
}

function ClcikBack() {
  isRegist.value = false;
  nickname.value = undefined;
  username.value = undefined;
  password.value = undefined;
}


async function hanleLogin() {
  // console.log(nickname.value, username.value, password.value);

  if (username.value == undefined) {
    moadl.msgError("请输入用户名！")
    return
  }

  if (password.value == undefined) {
    moadl.msgError("请输入密码！")
    return
  }

  const { code, msg } = await login(nickname.value, username.value, password.value)
  if (code == 200) {
    moadl.msgSuccess("登录成功！")
    lodding.value = "Lodding..."

    await userStore.getUserInfo(username.value);

    Cookies.set('username', username.value, { expires: 30 })

    setTimeout(() => {
      router.push({
        path: "/load"
      })
    }, 1000);
  }
  else {
    moadl.msgError(msg)
  }
}

async function hanleRegist() {
  // console.log(nickname.value, username.value, password.value);
  if (isRegist.value && nickname.value == undefined) {
    moadl.msgError("请输入昵称！")
    return
  }

  if (username.value == undefined) {
    moadl.msgError("请输入用户名！")
    return
  }

  if (password.value == undefined) {
    moadl.msgError("请输入密码！")
    return
  }

  const { code, msg } = await register(nickname.value, username.value, password.value)
  if (code == 200) {
    moadl.msgSuccess("注册成功！")

    registLodding.value = "Lodding..."

    await userStore.getUserInfo(username.value);

    setTimeout(() => {
      router.push({
        path: "/load"
      })
    }, 500);
  }
  else {
    moadl.msgError(code)
  }
}

</script>



<style>
@import url('https://fonts.googleapis.com/css2?family=Bebas+Neue&display=swap');

body {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 96.5vh;
  background-image: linear-gradient(to top, #fff1eb 0%, #ace0f9 100%);
}

.mb15 {
  margin-bottom: 15px;
}

.container {}

.back {
  font-size: 3em;
  color: #20fe37;
  -webkit-text-stroke: 2px black;
  cursor: pointer;
}

.login_input {
  width: 450px;
  height: 50px;
  padding-left: 50px;
  font-size: 36px;
  font-family: 'Bebas Neue', cursive;
  background: linear-gradient(45deg, transparent 5%, rgba(1, 19, 67, 0.8) 5%);
  border: 0;
  border-radius: 0px;
  /* line-height: 88px; */
  color: #fff;
  box-shadow: 6px 0px 0px #00a1ff;
  outline: transparent;
  position: relative;
}

.nick_name {
  animation: animateNickName 1s;
}

.nick_name::after {
  animation: 1s glitch;
}

@keyframes animateNickName {
  0% {
    transform: translateX(600px);
  }

  100% {
    transform: translateX(0px);
  }
}

.login_btn,
.login_btn::after {
  --slice-0: inset(50% 50% 50% 50%);
  --slice-1: inset(80% -6px 0 0);
  --slice-2: inset(50% -6px 30% 0);
  --slice-3: inset(50% -6px 85% 0);
  --slice-4: inset(40% -6px 43% 0);
  --slice-5: inset(80% -6px 5% 0);


  width: 350px;
  height: 70px;
  font-size: 36px;
  font-family: 'Bebas Neue', cursive;
  background: linear-gradient(45deg, transparent 5%, #FF013C 5%);
  border: 0;
  letter-spacing: 3px;
  /* line-height: 88px; */
  color: #fff;
  box-shadow: 6px 0px 0px #00E6F6;
  outline: transparent;
  position: relative;
  cursor: pointer;
}

.regist_btn,
.regist_btn::after {
  width: 200px;
  height: 45px;
  font-size: 30px;
  box-shadow: #00E6F6 0px 0px 50px;
}

.regist_btn2 {
  width: 400px;
  height: 70px;
  font-size: 30px;
  box-shadow: #00E6F6 0px 0px 100px;
}

.regist_btn2::after {
  width: 400px;
  height: 70px;
}

.login_btn::after {
  content: 'Login Now';
  display: block;
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: linear-gradient(45deg, transparent 3%, #00E6F6 3%, #00E6F6 5%, #FF013C 5%);
  text-shadow: -3px -3px 0px #F8F005, 3px 3px 0px #00E6F6;
  clip-path: var(--slice-0);
  transform: translate(-20px, 10px);
}

.login_btn:hover:after {
  animation: 1s glitch;
  animation-timing-function: steps(1, end);
}

@keyframes glitch {
  0% {
    clip-path: var(--slice-1);
    transform: translate(-20px, -10px);
  }

  10% {
    clip-path: var(--slice-3);
    transform: translate(10px, 10px);
  }

  20% {
    clip-path: var(--slice-5);
    transform: translate(-20px, -10px);
  }

  30% {
    clip-path: var(--slice-2);
    transform: translate(0px, 5px);
  }

  40% {
    clip-path: var(--slice-4);
    transform: translate(-5px, 0px);
  }

  50% {
    clip-path: var(--slice-3);
    transform: translate(5px, 0px);
  }

  60% {
    clip-path: var(--slice-4);
    transform: translate(5px, 10px);
  }

  70% {
    clip-path: var(--slice-4);
    transform: translate(-10px, 10px);
  }

  80% {
    clip-path: var(--slice-5);
    transform: translate(20px, -10px);
  }

  90% {
    clip-path: var(--slice-5);
    transform: translate(-10px, 0px);
  }

  100% {
    clip-path: var(--slice-1);
    transform: translate(0);
  }
}

@media (max-width:480px) {
  .login_input {
    width: 300px !important;
  }


}
</style>
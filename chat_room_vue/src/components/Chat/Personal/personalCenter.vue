<template>
    <div class="container">
        <div class="topbar">
            <div>个人中心</div>
            <div @click="back" class="backer"><i class="fa fa-grav"></i>&nbsp;返回</div>
        </div>

        <div class="mt20 personal">
            <el-form ref="formRef" :model="formData.value" :rules="rules">
                <el-row>
                    <el-col :span="24" style="display: flex;justify-content: center;margin: 15px 0 30px 0;">
                        <UploadImage ref="uploadRef" v-model="formData.chatUserImg" :limit="1" :fileSize="5"
                            :drag="true" />
                    </el-col>
                </el-row>
                <el-row>
                    <el-col :span="12">
                        <el-form-item :label-width="labelWidth" label="用户名">
                            <el-input type="text" v-model="formData.chatUserName" disabled></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item :label-width="labelWidth" label="昵称" prop="chatUserNickName">
                            <el-input type="text" v-model="formData.chatUserNickName" placeholder="请输入昵称"></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row>
                    <el-col :span="12">
                        <el-form-item :label-width="labelWidth" label="年龄">
                            <el-input type="text" v-model="formData.age"></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item :label-width="labelWidth" label="性别">
                            <el-radio-group v-model="formData.sex" class="ml-4">
                                <el-radio label="1" size="large">男</el-radio>
                                <el-radio label="2" size="large">女</el-radio>
                            </el-radio-group>
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row>
                    <el-col :span="12">
                        <el-form-item :label-width="labelWidth" label="电话">
                            <el-input type="text" v-model="formData.phone" placeholder="请输入电话"></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item :label-width="labelWidth" label="邮箱" prop="">
                            <el-input type="text" v-model="formData.email" placeholder="请输入邮箱"></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>

            </el-form>

            <el-row>
                <el-col :span="24" style="display: flex;justify-content: center;">
                    <el-button @click="hanleSubmit(formRef)" type="primary" style="width: 200px;height:50px">保存
                    </el-button>
                </el-col>
            </el-row>
        </div>


    </div>
</template>

<script setup>
import { reactive, ref, watch } from "vue";
import useUserStore from "../../../store/modules/user";
import useSwitchStore from "../../../store/modules/switch";
import modal from "../../../plugins/modal";
import { updateChatUser } from '../../../api/login'

const userStore = useUserStore()
const switchStore = useSwitchStore()
// console.log(userStore.userinfo);

const formData = ref({ ...userStore.userinfo });

const formRef = ref();
const labelWidth = 100;
let userJsonStr = sessionStorage.getItem('userInfo');
let info = JSON.parse(userJsonStr);

const rules = reactive({
    chatUserNickNam: [
        {
            required: true,
            message: "请输入昵称",
        },
    ],
})


const hanleSubmit = async (formEl) => {

    if (!formData.value.chatUserNickName) {
        modal.msgError('昵称不能为空')
        return
    }

    formData.value.age = Number(formData.value.age)

    const { code } = await updateChatUser(formData.value);
    if (code == 200) {
        modal.msgSuccess('保存成功')
    }
    await userStore.getUserInfo(info.chatUserName);

}


function back() {
    switchStore.isChat = false
    switchStore.isFriendIntro = false
    switchStore.isPersonal = false
    switchStore.isGroupChat = false
    switchStore.FriendIntroData = false
    switchStore.isFriendIntro = false
}

</script>

<style lang="scss" scoped>
@import url('https://fonts.googleapis.com/css2?family=Bebas+Neue&display=swap');

.container {
    width: 100%;
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

.container2 {
    width: 100%;
    animation: ani2 1.2s;
}

@keyframes ani2 {
    0% {
        transform: translateX(0px);
    }

    100% {
        transform: translateX(700px);
    }
}

.ml15 {
    margin-left: 15px;
}

.mt20 {
    margin-top: 20px;
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

.personal {
    margin-right: 30px;
}
</style>
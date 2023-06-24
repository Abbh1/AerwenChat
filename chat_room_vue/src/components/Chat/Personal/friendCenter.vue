<template>
    <div class="container">
        <div class="topbar">
            <div>好友简介</div>
            <div @click="back" class="backer"><i class="fa fa-grav"></i>&nbsp;返回</div>
        </div>

        <div class="mt20 personal">
            <el-form ref="formRef" :model="formData.value">
                <el-row>
                    <el-col :span="24" style="display: flex;justify-content: center;margin: 15px 0 30px 0;">
                        <UploadImage :isDisabled="true" ref="uploadRef" v-model="formData.friendImg" :limit="1"
                            :fileSize="5" :drag="true" />
                    </el-col>
                </el-row>
                <el-row>
                    <el-col :span="20" style="display: flex;justify-content: center;margin: 0px 0 30px 0;">
                        <el-form-item :label-width="labelWidth">
                            <i @click="showInput" class="fa fa-pencil-square-o"
                                style="cursor: pointer;margin-right: 5px;"></i>
                            <el-tag :disable-transitions="false">
                                {{formData.friendNote}}
                            </el-tag>
                            <el-input v-if="inputVisible" ref="inputRef" v-model="inputValue" placeholder="按Enter确认"
                                @keyup.enter="handleInputConfirm" @blur="handleInputConfirm" />

                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row>
                    <el-col :span="12">
                        <el-form-item :label-width="labelWidth" label="用户名">
                            <el-input type="text" v-model="formData.friendName" disabled></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item :label-width="labelWidth" label="昵称" prop="">
                            <el-input type="text" v-model="formData.friendNickName" disabled></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row>
                    <el-col :span="12">
                        <el-form-item :label-width="labelWidth" label="年龄">
                            <el-input type="text" v-model="formData.age" disabled></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item :label-width="labelWidth" label="性别" disabled>
                            <el-radio-group v-model="formData.sex" class="ml-4">
                                <el-radio label="1" disabled size="large">男</el-radio>
                                <el-radio label="2" disabled size="large">女</el-radio>
                            </el-radio-group>
                        </el-form-item>
                    </el-col>
                </el-row>
                <el-row>
                    <el-col :span="12">
                        <el-form-item :label-width="labelWidth" label="电话" disabled>
                            <el-input type="text" v-model="formData.phone" disabled></el-input>
                        </el-form-item>
                    </el-col>
                    <el-col :span="12">
                        <el-form-item :label-width="labelWidth" label="邮箱" prop="" disabled>
                            <el-input type="text" v-model="formData.email" disabled></el-input>
                        </el-form-item>
                    </el-col>
                </el-row>

            </el-form>

        </div>


    </div>
</template>

<script setup>
import { reactive, ref, watch } from "vue";
import useUserStore from "../../../store/modules/user";
import useSwitchStore from "../../../store/modules/switch";
import modal from "../../../plugins/modal";
import { updateChatUser } from '../../../api/login'
import { updateNote } from '../../../api/friends/friend'

const userStore = useUserStore()
const switchStore = useSwitchStore()

const formData = ref({ ...switchStore.FriendIntroData });

const formRef = ref();
const labelWidth = 100;

function back() {
    switchStore.isChat = false
    switchStore.isFriendIntro = false
    switchStore.isPersonal = false
    switchStore.isGroupChat = false
    switchStore.FriendIntroData = false
    switchStore.isFriendIntro = false
}


const inputRef = ref()
const inputVisible = ref(false)
const inputValue = ref('')
const noteData = ref({
    UserGuId: userStore.userinfo.chatUserGuId,
    FriendsGuId: formData.value.friendGuId,
    FriendsNote: null
})
// 显示标签输入
const showInput = () => {
    inputVisible.value = true
}
// 标签确认
async function handleInputConfirm() {
    if (inputValue.value) {
        noteData.value.FriendsNote = inputValue.value

        await updateNote(noteData.value).then((res) => {
            if (res.code == 200) {
                setTimeout(() => {
                    location.reload()
                }, 500);
            }
        })
        modal.msgSuccess("修改成功")

    }
    inputVisible.value = false
    inputValue.value = ''
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
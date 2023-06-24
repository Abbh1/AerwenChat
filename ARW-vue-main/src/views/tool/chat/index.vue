<template>
  <el-card class="box-card">
    <template #header>
      <div class="card-header">
        <span>Card name</span>
        <el-button class="button" text>Operation button</el-button>
      </div>
    </template>
    <div v-for="(v, k) in msgList" :key="k" class="text item">{{ v.user + ":" + v.message }}</div>
  </el-card>

  <el-row style="margin:20px,0;">
    <el-col :lg="16">
      <el-input v-model="message" type="textarea" :rows="10">
      </el-input>
        <el-button  type="success" size="large" @click="sendMsg">发送</el-button>
    </el-col>
  </el-row>
</template>

<script >
import useUserStore from '@/store/modules/user'
import * as signalR from '@microsoft/signalr'

const userInfo = computed(() => {
  return useUserStore().userInfo
})


export default {
  data() {
    return {
      connection: "",
      user: userInfo.value.nickName,
      message: "",
      msgList: []
    };
  },
  created() {
    this.init();
  },
  methods: {
    init() {
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl("http://chatvue.aerwen.net/msghub", {})
        .configureLogging(signalR.LogLevel.Error)
        .build();
      this.connection.on("ReceiveMessage", data => {
        this.msgList.push(data);
        console.log(this.msgList);
      });
      this.connection.start();
    },
    sendMsg() {
      this.connection.invoke("SendChat", this.user, this.message);
      console.log(this.msgList);
    }
  }
};
</script>

<style>
.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.text {
  font-size: 14px;
}

.item {
  margin-bottom: 18px;
}

.box-card {
  width: 1105px;
  height: 500px;
}
</style>

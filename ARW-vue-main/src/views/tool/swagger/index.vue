<template>
  <div v-loading="loading" :style="'height:'+ height">
    <el-button type="primary" @click="onCopy" class="right">复制 JWT</el-button>
    <iframe :src="src" frameborder="no" style="width: 100%;height: 100%" scrolling="auto" />
  </div>
</template>
<script>
import { onMounted } from "vue";
import { getToken } from '@/utils/auth'

export default {
  name: "Swagger",
  setup() {
    const src = ref(import.meta.env.VITE_APP_BASE_API + "/swagger/index.html");
    const height = ref(document.documentElement.clientHeight - 94.5 + "px;");
    const loading = ref(true);
    onMounted(() => {
      setTimeout(() => {
        loading.value = false;
      }, 230);

      window.onresize = function temp() {
        height.value = document.documentElement.clientHeight - 94.5 + "px;";
      };
    });
    return {
      src,
      height,
      loading,
    };
  },
  
methods:{
    onCopy(){
      const headers = 'Bearer ' + getToken()
      let oInput = document.createElement('input')
      oInput.value = headers
      document.body.appendChild(oInput)
      oInput.select() // 选择对象;
      document.execCommand('Copy') // 执行浏览器复制命令
      this.$message({
        message: '复制成功',
        type: 'success'
      })
      oInput.remove()
  }
}

};
</script>

<style>
  .right{
    position: absolute;
    right: 2.5em;
    bottom: 31rem;
  }
</style>
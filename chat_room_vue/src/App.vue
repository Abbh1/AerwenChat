<template>
  <RouterView></RouterView>
</template>

<script setup>
import { onMounted, ref,watch,computed } from 'vue'
import Cookies from 'js-cookie'
import signalr from './utils/signalR'

let beforeUnloadTime = ref(0)
let gapTime = ref(0)
let user =  computed(() => {
  return Cookies.get('username')
})
watch(
  user,async (val) => {
    if (val) {
      await signalr.init('/chathub')
    }
  },
  {
    immediate: true,
    deep: true,
  },
)

const beforeunloadHandler = () => {
  beforeUnloadTime.value = new Date().getTime();
}

const unloadHandler = () => {
  gapTime.value = new Date().getTime() - beforeUnloadTime.value;
  //判断是窗口关闭还是刷新
  if (gapTime.value <= 5) {//浏览器关闭
    Cookies.remove('username')
    localStorage.clear()
  }
}

onMounted(() => {
  //监听页面卸载（关闭）或刷新
  window.addEventListener('beforeunload', () => beforeunloadHandler())

  window.addEventListener('unload', () => unloadHandler())
})

</script>

<style scoped>
</style>

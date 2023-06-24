<template>
<body>
  <Loader></Loader>
</body>
 
</template>

<script setup>
import Loader from '../components/Login/loader.vue'

import { ref, reactive } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const router = useRouter()

  setTimeout(() => {
      router.push({
        path:"/chat"
    })
    setTimeout(() => {
            location.reload()
    }, 500);
  }, 2000);

</script>

<style scoped>
  body{
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    background-image: linear-gradient(to top, #fff1eb 0%, #ace0f9 100%);
    margin: 0;
    padding: 0;
}
</style>
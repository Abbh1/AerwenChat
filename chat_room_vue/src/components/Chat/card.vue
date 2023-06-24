<template>
  <body>
    <!-- 加载动画 -->
    <div class="shell" :class="{ shell2: isClick }" @click="clickCard">
      <div class="card">
        <h1><span class="word">Aerwen</span>Chat</h1>
      </div>
    </div>
  </body>
</template>

<script setup>
import { ref, reactive } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const router = useRouter()
let isClick = ref(false);

function clickCard() {
  isClick.value = true
  setTimeout(() => {
    router.push({
      path: "/login"
    })
  }, 500);

}
</script>

<style lang="scss" scoped>
* {
  margin: 0;
  padding: 0;
}

body {
  background: linear-gradient(to bottom, rgba(255, 255, 255, 0.15) 0%, rgba(0, 0, 0, 0.15) 100%), radial-gradient(at top center, rgba(255, 255, 255, 0.40) 0%, rgba(0, 0, 0, 0.40) 120%) #989898;
  background-blend-mode: multiply, multiply;
  display: flex;
  justify-content: center;
  align-items: center;
  height: 98.2vh;
  overflow: hidden;
}

.shell {
  position: relative;
  perspective: 700px;
  transform-style: preserve-3d;
  display: flex;
  cursor: pointer;
  animation: animateRotate 1.8s;
}

@keyframes animateRotate {
  0% {
    transform: rotateY(0deg);

  }

  100% {
    transform: rotateY(360deg);

  }
}

.shell2 {
  position: relative;
  perspective: 700px;
  transform-style: preserve-3d;
  display: flex;
  animation: animateCard 1s;
}

@keyframes animateCard {
  0% {
    transform: translateY(0px);
  }

  100% {
    transform: translateY(-900px);
  }
}

.card {
  height: 260px;
  width: 560px;
  transform: translateX(10px) rotateY(25deg) rotateX(10deg);
  background-color: rgba(250, 200, 30, .8);
  display: flex;
  align-items: center;
}

h1 {
  font-size: 70px;
}

.card .word {
  background-color: #000;
  line-height: 1;
  color: rgba(250, 200, 30, 1);
  padding: 0 10px;
  margin: 0 15px 0 30px;
  display: inline-flex;
}

.shell::before {
  content: '';
  height: 260px;
  width: 560px;
  position: absolute;
  margin-top: -9px;
  margin-left: -9px;
  border: 9px solid #000;
  transform: translateX(-60px) rotateY(-30deg) rotateX(15deg) scale(1.03);
}

.shell:hover>div,
.shell:hover::before {
  transform: none;
}

.shell>div,
.shell::before {
  transition: .3s;
}

@media (max-width: 480px) {
  .card {
    width: 350px !important;
    height: 250px !important;
  }

  .shell::before {
    width: 350px !important;
    height: 250px !important;
    margin-left: 25px !important;
  }
}
</style>
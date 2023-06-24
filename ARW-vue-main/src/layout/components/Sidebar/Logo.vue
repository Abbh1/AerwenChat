<template>
  <div class="sidebar-logo-backgruond">
    <div style="background-color: #72d0ff" class="sidebar-logo-container" :class="{ collapse: collapse }">
      <transition name="sidebarLogoFade">
        <router-link v-if="collapse" key="collapse" class="sidebar-logo-link" to="/">
          <!-- <img v-if="logo" :src="logo" class="sidebar-logo" /> -->
          <h1 class="sidebar-title">Aerwen / Chat</h1>
        </router-link>
        <router-link v-else key="expand" class="sidebar-logo-link" to="/">
          <!-- <img v-if="logo" :src="logo" class="sidebar-logo" /> -->
          <h1 class="sidebar-title">Aerwen / Chat</h1>
        </router-link>
      </transition>
    </div>
  </div>

</template>

<script setup>
import logo from '@/assets/logo/logo.png'
import useSettingsStore from '@/store/modules/settings'

defineProps({
  collapse: {
    type: Boolean,
    required: true,
  },
})

const title = ref(import.meta.env.VITE_APP_TITLE)
const settingsStore = useSettingsStore();
const sideTheme = computed(() => settingsStore.sideTheme);
</script>

<style lang="scss" scoped>
.sidebar-logo-backgruond {
  background-color: #72d0ff;
}

.sidebarLogoFade-enter-active {
  transition: opacity 1.5s;
}

.sidebarLogoFade-enter,
.sidebarLogoFade-leave-to {
  opacity: 0;
}

.sidebar-logo-container {
  position: relative;
  width: 100%;
  height: 45px;
  line-height: 45px;
  background: var(--base-menu-background);
  text-align: center;
  overflow: hidden;
  margin: 15px 0;

  & .sidebar-logo-link {
    height: 100%;
    width: 100%;

    & .sidebar-logo {
      width: 70%;
      height: 60%;
      vertical-align: middle;
      object-fit: cover;
      margin: 0 12px;
    }

    & .sidebar-title {
      display: inline-block;
      margin: 0;
      color: var(--base-logo-title-color);
      font-weight: 600;
      line-height: 50px;
      font-size: 1.3rem;
      font-family: Avenir, Helvetica Neue, Arial, Helvetica, sans-serif;
      vertical-align: middle;
    }
  }

  &.collapse {
    .sidebar-logo {
      margin-right: 0px;
    }
  }
}
</style>

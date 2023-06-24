<template>
<div>
    <!-- 加载动画 -->
    <div class="loader">
        <div class="rocket">
            <i class="fa fa-rocket"></i>
            <i class="fa fa-cloud" style="--i:0"></i>
            <i class="fa fa-cloud" style="--i:1"></i>
            <i class="fa fa-cloud" style="--i:2"></i>
            <i class="fa fa-cloud" style="--i:3"></i>
        </div>
        <span><i></i></span>
    </div>
</div>
</template>

<script setup>
import '../../assets/style/login/loader.css'
</script>

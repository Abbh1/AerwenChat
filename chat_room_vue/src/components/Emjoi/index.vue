<template>
    <V3Emoji size="mid" :recent="true" fix-pos="upright" :options-name="optionsName"  :custom-size="customSize" @click-emoji="appendText"/>
</template>

<script setup>
import V3Emoji from 'vue3-emoji'
import 'vue3-emoji/dist/style.css'

const props = defineProps({
  modelValue: [String, Object, Array],
})

const optionsName = {
 'Smileys & Emotion': '笑脸&表情',
 'Food & Drink': '食物&饮料',
 'Animals & Nature': '动物&自然',
 'Travel & Places': '旅行&地点',
 'People & Body': '人物&身体',
 Objects: '物品',
 Symbols: '符号',
 Flags: '旗帜',
 Activities: '活动'
};

const customSize= {
//   'V3Emoji-width': '300px',
  'V3Emoji-height': '20rem',
  'V3Emoji-fontSize': '1rem',
  'V3Emoji-itemSize': '2rem'
};

const appendText = (val) => {
    console.log(val);
    // console.log(ref);
    // inputRef.value.innerHTML += val;
    emit('update:modelValue', listToString(fileList.value))
};

</script>

<style lang="scss" scoped>
.tool {
    width: 1.5rem;
    cursor: pointer;
}
</style>
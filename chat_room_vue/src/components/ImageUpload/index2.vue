<template>
  <div class="component-upload-image">
    <el-upload
      multiple
      :action="uploadImgUrl"
      :on-success="handleUploadSuccess"
      :before-upload="handleBeforeUpload"
      :limit="limit"
      :on-error="handleUploadError"
      :on-exceed="handleExceed"
      name="file"
      :data="data"
      :on-remove="handleRemove"
      :show-file-list="false"
      :file-list="fileList"
      :on-preview="handlePictureCardPreview"
      :disabled="isDisabled"
      >
      <img style="width: 1.5rem;" src="../../assets/img/pic.png" alt="">
    </el-upload>

    <!-- <el-dialog v-model="dialogVisible" title="预览" width="800px" append-to-body>
      <img :src="dialogImageUrl" style="display: block; max-width: 100%; margin: 0 auto" />
    </el-dialog> -->
  </div>
</template>

<script setup>
import moadl from '../../plugins/modal'
import { ref,computed,watch } from 'vue';
const props = defineProps({
  modelValue: [String, Object, Array],
  // 图片数量限制
  limit: {
    type: Number,
    default: 5,
  },
  // 大小限制(MB)
  fileSize: {
    type: Number,
    default: 5,
  },
  // 文件类型, 例如['png', 'jpg', 'jpeg']
  fileType: {
    type: Array,
    default: () => ['png', 'jpg', 'jpeg'],
  },
  // 是否显示提示
  isShowTip: {
    type: Boolean,
    default: true,
  },
  isDisabled: {
    type: Boolean,
    default: false,
  },
  // 上传携带的参数
  data: {
    type: Object,
  },
})

// const { proxy } = getCurrentInstance()
const emit = defineEmits()
const number = ref(0)
const uploadList = ref([])
const dialogImageUrl = ref('')
const dialogVisible = ref(false)
const baseUrl = "/dev-api"
const uploadImgUrl = ref(baseUrl + "/Common/UploadFile") // 上传的图片服务器地址
const fileList = ref([])
const showTip = computed(() => props.isShowTip && (props.fileType || props.fileSize))

watch(
  () => props.modelValue,
  (val) => {
    if (val) {
      // 首先将值转为数组
      const list = Array.isArray(val) ? val : props.modelValue.split(',')
      // 然后将数组转为对象数组
      fileList.value = list.map((item) => {
        if (typeof item === 'string') {
          // if (item.indexOf(baseUrl) === -1) {
          //   item = { name: baseUrl + item, url: baseUrl + item }
          // } else {
          item = { name: item, url: item }
          // }
        }
        return item
      })
    } else {
      fileList.value = []
      return []
    }
  },
  { deep: true, immediate: true },
)
        // console.log(fileList.value.length);


// 删除图片
function handleRemove(file, files) {
  emit('update:modelValue', listToString(fileList.value))
}

// 上传成功回调
function handleUploadSuccess(res) {
  if (res.code != 200) {
    moadl.msgError(`上传失败，原因:${res.msg}!`)
    moadl.closeLoading()
    fileList.value = fileList.value.slice(0, fileList.value.length - 1)
    return
  }
  uploadList.value.push({ name: res.data.fileName, url: res.data.url })
  if (uploadList.value.length === number.value) {
    fileList.value = fileList.value.filter((f) => f.url !== undefined).concat(uploadList.value)
    uploadList.value = []
    number.value = 0
    emit('update:modelValue', listToString(fileList.value))
  }
  moadl.closeLoading()
}

// 上传前loading加载
function handleBeforeUpload(file) {
  let isImg = false
  if (props.fileType.length) {
    let fileExtension = ''
    if (file.name.lastIndexOf('.') > -1) {
      fileExtension = file.name.slice(file.name.lastIndexOf('.') + 1)
    }
    isImg = props.fileType.some((type) => {
      if (file.type.indexOf(type) > -1) return true
      if (fileExtension && fileExtension.indexOf(type) > -1) return true
      return false
    })
  } else {
    isImg = file.type.indexOf('image') > -1
  }
  if (!isImg) {
    moadl.msgError(`文件格式不正确, 请上传${props.fileType.join('/')}图片格式文件!`)
    return false
  }
  if (props.fileSize) {
    const isLt = file.size / 1024 / 1024 < props.fileSize
    if (!isLt) {
      moadl.msgError(`上传头像图片大小不能超过 ${props.fileSize} MB!`)
      return false
    }
  }
  moadl.loading('正在上传图片，请稍候...')
  number.value++
}

// 文件个数超出
function handleExceed() {
  moadl.msgError(`上传文件数量不能超过 ${props.limit} 个!`)
}

// 上传失败
function handleUploadError() {
  moadl.msgError('上传图片失败')
  moadl.closeLoading()
}

// 预览
function handlePictureCardPreview(file) {
  dialogImageUrl.value = file.url
  dialogVisible.value = true
}

// 对象转成指定字符串分隔
function listToString(list, separator) {
  let strs = ''
  separator = separator || ','
  for (let i in list) {
    if (undefined !== list[i].url && list[i].url.indexOf('blob:') !== 0) {
      strs += list[i].url + separator
    }
  }
  return strs != '' ? strs.substr(0, strs.length - 1) : ''
}
</script>

<style>


.put{
  width: 120px;
  height: 120px;
  margin-top: 10px;
  font-size: 1rem;
}

/* // 隐藏picture-card 上传按钮 */
.hide .el-upload--picture-card {
  display: none;
  outline: invert;
}
</style>
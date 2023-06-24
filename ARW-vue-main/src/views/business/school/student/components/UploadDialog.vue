<template>
    <el-dialog title="导入" v-model="modelValue" width="400px" @closed="closeDialog">
      <el-upload
        name="file"
        ref="uploadRef"
        :limit="1"
        accept=".xlsx, .xls"
        :headers="upload.headers"
        :action="upload.url + '?updateSupport=' + upload.updateSupport"
        :disabled="upload.isUploading"
        :on-progress="handleFileUploadProgress"
        :on-success="handleFileSuccess"
        :auto-upload="false"
        drag>
        <el-icon class="el-icon--upload">
          <upload-filled />
        </el-icon>
        <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
        <template #tip>
          <div class="el-upload__tip text-center">
            <div class="el-upload__tip">
              <el-checkbox v-model="upload.updateSupport" /> 是否更新已经存在的学生数据
            </div>
            <span>仅允许导入xls、xlsx格式文件。</span>
            <el-link type="primary" :underline="false" style="font-size: 12px; vertical-align: baseline" @click="importTemplate">下载模板</el-link>
          </div>
        </template>
      </el-upload>
      <template #footer>
        <el-button @click="closeDialog">{{ $t('btn.cancel') }}</el-button>
        <el-button type="primary" @click="submitFileForm">{{ $t('btn.submit') }}</el-button>
      </template>
    </el-dialog>
</template>


<script setup>
import { reactive, ref, watch } from "vue";
import { ElMessage } from 'element-plus'
import { getToken } from '@/utils/auth'
import { addClass } from '@/api/business/school/class.js';


// 业务参数
const formRef = ref();
const labelWidth = 100;
const { proxy } = getCurrentInstance()

/*** 导入参数 */
const upload = reactive({
  // 是否禁用上传
  isUploading: false,
  // 是否更新已经存在的数据
  updateSupport: 0,
  // 设置上传的请求头部
  headers: { Authorization: 'Bearer ' + getToken() },
  // 上传的地址
  url: import.meta.env.VITE_APP_BASE_API + '/business/student/importData',
})



// -业务方法
/** 下载模板操作 */
function importTemplate() {
  proxy.download('/business/student/importTemplate', '学生数据导入模板')
}
/**文件上传中处理 */
const handleFileUploadProgress = (event, file, fileList) => {
  upload.isUploading = true
}
/** 文件上传成功处理 */
const handleFileSuccess = (response, file, fileList) => {
  closeDialog()
  upload.isUploading = false
  proxy.$refs['uploadRef'].clearFiles()
  proxy.$alert("<div style='overflow: auto;overflow-x: hidden;max-height: 70vh;padding: 10px 20px 0;'>" + response.msg + '</div>', '导入结果', {
    dangerouslyUseHTMLString: true,
  })
  props.done()
}
/** 提交上传文件 */
function submitFileForm() {
  proxy.$refs['uploadRef'].submit()
}


// -基础参数
const emits = defineEmits(["update:modelValue"]); 
const dialogVisible = ref(props.modelValue);
const formData = reactive({});
const props = defineProps({
  modelValue: Boolean,
  done: Function,
});

// watch(props,async (v) => {
//   dialogVisible.value = v.modelValue;
// });

// 验证
const rules = reactive({
  className: [
    {
      required: true,
      message: "请输入班级名称",
    },
  ],
});

// -基础方法
const closeDialog = () => {
  dialogVisible.value = false;
  emits("update:modelValue", false);
};
</script>
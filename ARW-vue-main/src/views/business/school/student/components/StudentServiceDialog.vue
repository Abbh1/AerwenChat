<template>
  <el-dialog
    v-model="dialogVisible"
    title="学生服务"
    width="400px"
    @closed="closeDialog"
  >
    <el-form
      ref="formRef"
      :model="formData"
    >
        <el-form-item :label-width="labelWidth" label="服务类型" prop="">
          <el-input
            v-model="serviceList.service_name"
            type="text"
            placeholder="请输入服务类型"
          ></el-input>
        </el-form-item>

        <el-form-item :label-width="labelWidth" label="价格" prop="">
          <el-input
            v-model="serviceList.service_price"
            placeholder="请输入价格"
            type="number"
          ></el-input>
        </el-form-item>

    </el-form>
    <template #footer>
      <span class="dialog-footer">
        <el-button type="primary" @click="handleEditClick(formRef)"
          >添加</el-button
        >
        <el-button @click="handleResetClick(formRef)">重置</el-button>
      </span>
    </template>
  </el-dialog>
</template>

<script setup>
import { reactive, ref, watch } from 'vue';
import { isEmptyObject,deepClone } from '@/utils/index';
import { ElMessage } from 'element-plus'

// 业务参数
const formRef = ref();
const labelWidth = 100;
const serviceList = ref({
    service_name: "",
    service_price: ""
});

// 基础参数
const emits = defineEmits(['update:modelValue']);
const dialogVisible = ref(false);
const props = defineProps({
  modelValue: Object,
  done: Function
});
const formData = ref({
  ...props.modelValue
});
watch(props, (v) => {
  formData.value = v.modelValue;
  dialogVisible.value = !isEmptyObject(v.modelValue);
});


// -业务方法



// -基础方法
const closeDialog = () => {
  handleResetClick(formRef.value);
  dialogVisible.value = false;
  emits('update:modelValue', {});
};

// 提交 
const handleEditClick = async (formEl) => {
  if (!formEl) return;
  if(serviceList.value.service_name == "" || serviceList.value.service_price == ""){
    ElMessage({
    message: '请填写服务类型和价格',
    type: 'error',
    })
    return;
  }
  formEl.validate(async (valid) => {
    if (!valid) {
      return;
    }
    const obj = formData.value.studentService;
      var arr = []
      for (let i in obj) {
          arr.push(obj[i]); //属性
      }
    let newObj = deepClone(serviceList.value);
    arr.push(newObj)
    // let newObj = deepClose(serviceList.value);
    // arr.push(newObj)
    formData.value.studentService = arr
    if(formData.value.studentService != null){
      closeDialog();
    }
  });
};

const handleResetClick = async formEl => {
  if (!formEl) return;
  formEl.resetFields();
};
</script>


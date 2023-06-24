<template>
  <el-dialog v-model="modelValue" title="添加班级信息" width="500px" @closed="closeDialog">
    <el-form ref="formRef" :model="formData" :rules="rules">
      <el-row>
        <el-col :span="24">
          <el-form-item :label-width="labelWidth" label="班级名称" prop="className">
            <el-input v-model="formData.className" type="text" placeholder="请输入班级名称"></el-input>
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>

    <template #footer>
      <div class="dialog-footer">
        <el-button type="primary" @click="handleAddClick(formRef)">添加</el-button>
        <el-button @click="handleResetClick(formRef)">重置</el-button>
      </div>
    </template>
  </el-dialog>
</template>


<script setup>
import { reactive, ref, watch } from "vue";
import { ElMessage } from 'element-plus'
import { addClass } from '@/api/business/school/class.js';


// 业务参数
const formRef = ref();
const labelWidth = 100;
const { proxy } = getCurrentInstance()


const emits = defineEmits(["update:modelValue"]);
const dialogVisible = ref(props.modelValue);
const formData = reactive({
  classTag: []
});
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

// -业务方法


// -基础方法
const handleAddClick = async (formEl) => {
  // console.log(formData)
  if (!formEl) return;
  formEl.validate(async (valid) => {
    if (!valid) {
      return;
    }
    const { code } = await addClass(formData);
    if (code == 200) {
      proxy.$modal.msgSuccess('添加成功')
      closeDialog();
      props.done();
    }
  });
};
const closeDialog = () => {
  handleResetClick(formRef.value);
  dialogVisible.value = false;
  emits("update:modelValue", false);
};
const handleResetClick = async (formEl) => {
  if (!formEl) return;
  formEl.resetFields();
};
</script>
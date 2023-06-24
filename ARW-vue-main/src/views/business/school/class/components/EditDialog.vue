<template>
  <el-dialog v-model="modelValue" title="修改学生信息" width="500px" @closed="closeDialog">
    <el-form ref="formRef" :model="data" :rules="rules">
      <el-row>
        <el-col :span="24">
          <el-form-item :label-width="labelWidth" label="学生名称" prop="className">
            <el-input v-model="data.className" type="text" placeholder="请输入学生名称"></el-input>
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>

    <template #footer>
      <div class="dialog-footer">
        <el-button type="primary" @click="handleEditClick(formRef)">编辑</el-button>
        <el-button @click="handleResetClick(formRef)">重置</el-button>
      </div>
    </template>
  </el-dialog>
</template>

<script setup>
import { ElMessage } from 'element-plus'
import { reactive, ref, watch } from "vue";
import { updateClass } from "@/api/business/school/class.js";

// 基础参数
const props = defineProps({
  modelValue: Boolean,
  data: Object,
  done: Function,
});

const emits = defineEmits(["update:modelValue"]);

// watch(props, (v) => {
  // data = v.modelValue;
  // console.log(v.modelValue)
// });

// 业务参数
const formRef = ref();
const labelWidth = 100;
const { proxy } = getCurrentInstance()


// 验证
const rules = reactive({
    className: [
    {
      required: true,
      message: "请输入学生名称",
    },
  ],
});

// -业务方法


// -基础方法
// 提交
const handleEditClick = async (formEl) => {
  if (!formEl) return;
  formEl.validate(async (valid) => {
    if (!valid) {
      return;
    }
    const { code } = await updateClass(props.data);
    if (code == 200) {
      proxy.$modal.msgSuccess('修改成功')
      closeDialog();
      props.done();
    }
  });
}

const handleResetClick = async (formEl) => {
  if (!formEl) return;
  formEl.resetFields();
}
const closeDialog = () => {
  props.done();
  emits("update:modelValue", false);
};

</script>

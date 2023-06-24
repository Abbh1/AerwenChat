<template>
  <el-dialog v-model="modelValue" title="班级信息详情" width="500px" @closed="closeDialog">
    <el-form ref="formRef" :model="data" :disabled="true">

      <el-row>
        <el-col :span="24">
          <el-form-item :label-width="labelWidth" label="班级名称" >
            <el-input v-model="data.className" type="text" ></el-input>
          </el-form-item>
        </el-col>
      </el-row>

    </el-form>

  </el-dialog>
</template>

<script setup>
import Editor from '@/components/Editor'
import { ElMessage } from 'element-plus'
import { reactive, ref, watch } from "vue";


// 业务参数
const formRef = ref();
const labelWidth = 100;
const { proxy } = getCurrentInstance()


// -业务方法


// 基础参数
const props = defineProps({
  modelValue: Boolean,
  data: Object,
  done: Function,
});
const emits = defineEmits(["update:modelValue"]);

// -基础方法
const closeDialog = () => {
  props.done();
  emits("update:modelValue", false);
};
const handeldisable = editor => {
  editor.disable()
}
</script>

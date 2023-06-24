<template>
  <el-dialog v-model="modelValue" title="学生信息详情" width="900px" @closed="closeDialog">
    <el-form ref="formRef" :model="data" :disabled="true">
      <el-row>
        <el-col :span="12">
          <el-form-item :label-width="labelWidth" label="学生名称" >
            <el-input v-model="data.studentName" type="text" ></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item :label-width="labelWidth" label="班级" >
            <el-select v-model="data.classId" style="width: 100%">
              <el-option 
              v-for="item in classArr.value"
              :key="item.classId"
              :label="item.className"
              :value="item.classId"/>
            </el-select>  
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="12">
          <el-form-item :label-width="labelWidth" label="性别" >
            <el-radio-group v-model="data.sex" >
              <el-radio v-for="dict in sexOptions" :key="dict.dictValue" :label="dict.dictValue">{{ dict.dictLabel }}</el-radio>
            </el-radio-group>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item :label-width="labelWidth" label="学生标签">
            <el-tag v-for="tag in data.studentTag" class="mr10" :disable-transitions="false">
              {{ tag }}
            </el-tag>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="12">
          <el-form-item :label-width="labelWidth" label="学生服务" >
            <div v-for="(item, index) in data.studentService" :key="index">
              <div style="margin-left:10px">
                {{item.service_name}}
              </div>
            </div>      
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item :label-width="labelWidth" label="学生图片" >
            <UploadImage ref="uploadRef" v-model="data.studentImg" :limit="1" :fileSize="5" :drag="true" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="24">
          <el-form-item :label-width="labelWidth" label="学生描述" >
            <editor v-model="data.studentDescribe" :editorConfig="{readOnly: true}" :min-height="196" />
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
import { getClassList } from "@/api/business/school/student.js";


// 业务参数
const formRef = ref();
const labelWidth = 100;
const { proxy } = getCurrentInstance()
const classArr = reactive([]);
const sexOptions = ref([])


// -业务方法
// 获取班级名称(下拉框)
getClassList().then(v => {
  classArr.value = v.data
});
// 字典获取
proxy.getDicts('sys_user_sex').then((response) => {
  sexOptions.value = response.data
})


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

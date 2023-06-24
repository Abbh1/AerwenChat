<template>
  <el-dialog v-model="modelValue" title="添加学生信息" width="900px" @closed="closeDialog">
    <el-form ref="formRef" :model="formData" :rules="rules">
      <el-row>
        <el-col :span="12">
          <el-form-item :label-width="labelWidth" label="学生名称" prop="studentName">
            <el-input v-model="formData.studentName" type="text" placeholder="请输入学生名称"></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item :label-width="labelWidth" label="班级" prop="classId">
            <el-select v-model="formData.classId" placeholder="请选择班级" style="width: 100%">
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
          <el-form-item :label-width="labelWidth" label="性别" prop="sex">
            <el-radio-group v-model="formData.sex" placeholder="请选择用户性别">
              <el-radio v-for="dict in sexOptions" :key="dict.dictValue" :label="dict.dictValue">{{ dict.dictLabel }}</el-radio>
            </el-radio-group>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item :label-width="labelWidth" label="学生标签">
            <el-tag v-for="tag in formData.studentTag"  class="mr10" closable :disable-transitions="false" @close="handleCloseTag(tag)">
              {{ tag }}
            </el-tag>
            <el-input
              v-if="inputVisible"
              ref="inputRef"
              v-model="inputValue"
              class="w20"
              @keyup.enter="handleInputConfirm"
              @blur="handleInputConfirm" />

            <el-button v-else class="button-new-tag" size="small" @click="showInput">+ 学生标签</el-button>
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="12">
          <el-form-item :label-width="labelWidth" label="学生服务" prop="studentService" style="display:flex;align-items: center;">
            <div v-for="(item, index) in formData.studentService" :key="index">
              <div style="margin-left:10px">
                {{item.service_name}}
                <el-button size="small"  :circle="true" icon="minus" @click="handleCancel(index)"></el-button>
              </div>
            </div>      
            <el-button style="margin-left:10px" :circle="true" icon="plus" @click="StudentServiceDialogVisible = formData"></el-button>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item :label-width="labelWidth" label="学生图片" prop="studentImg">
            <UploadImage ref="uploadRef" v-model="formData.studentImg" :limit="1" :fileSize="5" :drag="true" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="24">
          <el-form-item :label-width="labelWidth" label="学生描述" prop="studentDescribe">
            <editor v-model="formData.studentDescribe" :min-height="196" />
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
   <!-- 学生服务 -->
  <StudentServiceDialog v-model="StudentServiceDialogVisible"></StudentServiceDialog>
</template>


<script setup>
import Editor from '@/components/Editor'
import { reactive, ref, watch } from "vue";
import { ElMessage } from 'element-plus'
import { addStudent, getClassList } from '@/api/business/school/student.js';
import StudentServiceDialog from '../components/StudentServiceDialog.vue';


// 业务参数
const formRef = ref();
const labelWidth = 100;
const { proxy } = getCurrentInstance()
const classArr = reactive([]);
const sexOptions = ref([])
const inputRef = ref()
const inputVisible = ref(false)
const inputValue = ref('')

const StudentServiceDialogVisible = ref({});



const emits = defineEmits(["update:modelValue"]);
const dialogVisible = ref(props.modelValue);
const formData = reactive({
  studentTag: []
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
  studentName: [
    {
      required: true,
      message: "请输入学生名称",
    },
  ],
  classId: [
    {
      required: true,
      message: "请选择班级",
    },
  ],
  sex: [
    {
      required: true,
      message: "请选择性别",
    },
  ],
  studentTag: [
    {
      required: true,
      message: "请输入学生标签",
    },
  ],
  studentService: [
    {
      required: true,
      message: "请输入学生服务",
    },
  ],
  studentImg: [
    {
      required: true,
      message: "请上传学生图片",
    },
  ],
  studentDescribe: [
    {
      required: true,
      message: "请输入学生描述",
    },
  ],
});

// -业务方法

getClassList().then(v => {
  // console.log(v.data);
  classArr.value = v.data
});
// 字典获取
proxy.getDicts('sys_user_sex').then((response) => {
  sexOptions.value = response.data
})

// 删除标签
function handleCloseTag(tag) {
  formData.studentTag.splice(formData.studentTag.indexOf(tag), 1)
}
// 显示标签输入
const showInput = () => {
  inputVisible.value = true
  nextTick(() => {
    inputRef.value.input.focus()
  })
}
// 标签确认
function handleInputConfirm() {
  if (inputValue.value) {
    // console.log(formData.studentTag)
    formData.studentTag.push(inputValue.value)
  }
  inputVisible.value = false
  inputValue.value = ''
}
// 服务删除
const handleCancel = function (index) {
  const arr = formData.studentService;
  arr.splice(index, 1);
  ElMessage({
    message: "删除成功",
    type: "success",
  });
};




// -基础方法
const handleAddClick = async (formEl) => {
  // console.log(formEl)
  formData.studentTag =  formData.studentTag.toString();
  // console.log(formData)
  if (!formEl) return;
  formEl.validate(async (valid) => {
    if (!valid) {
      return;
    }
    const { code } = await addStudent(formData);
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
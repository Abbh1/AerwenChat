<template>
  <el-dialog v-model="modelValue" title="修改学生信息" width="900px" @closed="closeDialog">
    <el-form ref="formRef" :model="data" :rules="rules">
      <el-row>
        <el-col :span="12">
          <el-form-item :label-width="labelWidth" label="学生名称" prop="studentName">
            <el-input v-model="data.studentName" type="text" placeholder="请输入学生名称"></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item :label-width="labelWidth" label="班级" prop="classId">
            <el-select v-model="data.classId" placeholder="请选择班级" style="width: 100%">
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
            <el-radio-group v-model="data.sex" placeholder="请选择用户性别">
              <el-radio v-for="dict in sexOptions" :key="dict.dictValue" :label="dict.dictValue">{{ dict.dictLabel }}</el-radio>
            </el-radio-group>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item :label-width="labelWidth" label="学生标签">
            <el-tag v-for="tag in data.studentTag"  class="mr10" closable :disable-transitions="true" @close="handleCloseTag(tag)">
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
            <div v-for="(item, index) in data.studentService" :key="index">
              <div style="margin-left:10px">
                {{item.service_name}}
                <el-button size="small"  :circle="true" icon="minus" @click="handleCancel(index)"></el-button>
              </div>
            </div>      
            <el-button style="margin-left:10px" :circle="true" icon="plus" @click="StudentServiceDialogVisible = props.data"></el-button>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item :label-width="labelWidth" label="学生图片" prop="studentImg">
            <UploadImage ref="uploadRef" v-model="data.studentImg" :limit="1" :fileSize="5" :drag="true" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-row>
        <el-col :span="24">
          <el-form-item :label-width="labelWidth" label="学生描述" prop="studentDescribe">
            <editor v-model="data.studentDescribe" :min-height="196" />
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
  <!-- 学生服务 -->
  <StudentServiceDialog v-model="StudentServiceDialogVisible"></StudentServiceDialog>
</template>

<script setup>
import Editor from '@/components/Editor'
import { ElMessage } from 'element-plus'
import { reactive, ref, watch } from "vue";
import { isEmptyObject } from "@/utils/index.js";
import { updateStudent,getClassList } from "@/api/business/school/student.js";
import StudentServiceDialog from '../components/StudentServiceDialog.vue';

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
const classArr = reactive([]);
const sexOptions = ref([])
const inputRef = ref()
const inputVisible = ref(false)
const inputValue = ref('')

const StudentServiceDialogVisible = ref({});


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
// 获取班级名称(下拉框)
getClassList().then(v => {
  classArr.value = v.data
});
// 字典获取
proxy.getDicts('sys_user_sex').then((response) => {
  sexOptions.value = response.data
})

// 删除标签
function handleCloseTag(tag) {
  props.data.studentTag.splice(props.data.studentTag.indexOf(tag), 1)
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
    // console.log(data.studentTag)
    props.data.studentTag.push(inputValue.value)
  }
  inputVisible.value = false
  inputValue.value = ''
}
// 服务删除
const handleCancel = function (index) {
  const arr = data.studentService;
  arr.splice(index, 1);
  ElMessage({
    message: "删除成功",
    type: "success",
  });
};

// -基础方法
// 提交
const handleEditClick = async (formEl) => {
  props.data.studentTag =  props.data.studentTag.toString();

  if (!formEl) return;
  formEl.validate(async (valid) => {
    if (!valid) {
      return;
    }
    const { code } = await updateStudent(props.data);
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

<template>
  <div class="app-container">
    <el-row :gutter="24" style="margin-left: 5px;">
      <!-- :model属性用于表单验证使用 比如下面的el-form-item 的 prop属性用于对表单值进行验证操作 -->
      <el-form :model="queryParams" label-position="left" inline ref="queryForm" label-width="100px" v-show="showSearch"
        @submit.prevent>
        <el-form-item label="学生名称" prop="studentName">
          <el-input v-model="queryParams.studentName" placeholder="请输入学生名称" />
        </el-form-item>
        <el-form-item label="班级" prop="classId">
          <el-select v-model="queryParams.classId">
            <el-option v-for="item in classArr.value"
              :key="item.classId"
              :label="item.className"
              :value="item.classId"></el-option>
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="search" @keyup.enter="handleQuery"  @click="handleQuery">搜索</el-button>
          <el-button icon="refresh" @click="resetQuery">重置</el-button>
        </el-form-item>
      </el-form>
    </el-row>

    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button type="primary" plain icon="plus" v-hasPermi="['business:student:add']"
          @click="AddDialogVisible = true">添加</el-button>
        <AddDialog v-model="AddDialogVisible" :done="() => resetQuery()"></AddDialog>
      </el-col>
      <el-col :span="1.5">
        <el-button type="danger" :disabled="multiple" v-hasPermi="['business:student:delete']" plain icon="delete"
          @click="handleDelete">
          {{ $t('btn.delete') }}
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button type="info" plain icon="Upload" @click="UploadDialogVisible = true" v-hasPermi="['business:student:import']">导入</el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button type="warning" plain icon="download" @click="handleExport" v-hasPermi="['business:student:export']">
          {{ $t('btn.export') }}
        </el-button>
      </el-col>
      <right-toolbar :showSearch="showSearch"></right-toolbar>
    </el-row>

    <el-table :data="dataList" ref="tableRef" border highlight-current-row @selection-change="handleSelectionChange">
      <!-- <el-table-column prop="studentId" label="id" width="60" sortable> </el-table-column> -->
      <el-table-column type="selection" width="50" align="center" />
      <el-table-column prop="studentName" label="学生名称" width="120" :show-overflow-tooltip="true"> </el-table-column>
      <el-table-column prop="studentImg" label="学生图片" width="90" :show-overflow-tooltip="true">
        <template #default="{ row }">
          <el-image 
          preview-teleported 
          :src="row.studentImg" 
          :preview-src-list="[row.studentImg]"
          :hide-on-click-modal="true" fit="contain" lazy class="el-avatar">
          </el-image>
        </template>
      </el-table-column>
      <el-table-column prop="className" label="班级"> </el-table-column>
      <el-table-column sortable prop="sex" label="性别">
        <template #default="scope">
          {{ scope.row.status == '2' ? '女' : '男' }}
        </template>
      </el-table-column>
      <el-table-column prop="studentTag" label="标签" :show-overflow-tooltip="true"> </el-table-column>
      <el-table-column prop="studentService" label="学生服务" align="center">
        <template #default="scope">
          <div v-for="(item, index) in scope.row.studentService " :key="index">
            <span>{{ item.service_name +"："+item.service_price+"元" }}</span>
          </div>
        </template>
      </el-table-column>
      <!--<el-table-column prop="studentDescribe" label="学生描述" :show-overflow-tooltip="true"></el-table-column> -->

      <el-table-column label="操作" align="center" width="250">
        <template #default="scope">
          <el-button text size="small" icon="edit" @click="handleUpdate(scope.row)"
            v-hasPermi="['business:student:update']">编辑</el-button>
          <el-button text size="small" icon="delete" @click="handleDelete(scope.row)"
            v-hasPermi="['business:student:delete']">删除</el-button>
          <el-button text size="small" icon="view" @click="handleDetail(scope.row)">查看</el-button>
        </template>
      </el-table-column>

    </el-table>
    <pagination :total="total" v-model:page="queryParams.pageNum" v-model:limit="queryParams.pageSize"
      @pagination="getList" />
  </div>
  <!-- 编辑 -->
  <EditDialog v-model="EditDialogVisible" :data="EditDialogRow" :done="() => resetQuery()"></EditDialog>
  <!-- 详情 -->
  <DetailDialog v-model="DetailDialogVisible" :data="DetailDialogRow" :done="() => resetQuery()"></DetailDialog>
  <!-- 导入 -->
  <UploadDialog v-model="UploadDialogVisible" :done="() => resetQuery()"></UploadDialog>
</template>
<script setup >
import { ElMessageBox } from 'element-plus'
import { studentList, delStudent , exportStudent , getClassList } from '@/api/business/school/student.js'
import AddDialog from "./components/AddDialog.vue";
import EditDialog from "./components/EditDialog.vue";
import DetailDialog from "./components/DetailDialog.vue";
import UploadDialog from "./components/UploadDialog.vue";


const AddDialogVisible = ref(false);
const EditDialogVisible = ref(false);
const EditDialogRow = ref({});
const DetailDialogVisible = ref(false);
const DetailDialogRow = ref({});
const UploadDialogVisible = ref(false);


const { proxy } = getCurrentInstance()
const router = useRouter()

// 业务参数
const classArr = reactive([]);


// 业务方法
getClassList().then(v => {
  // console.log(v.data);
  classArr.value = v.data
});


// 选中categoryId数组数组
const ids = ref([])
// 非单选禁用
const single = ref(true)
// 非多个禁用
const multiple = ref(true)
// 显示搜索条件
const showSearch = ref(true)
// 文章状态下拉框
const statusOptions = ref([])
// 数据列表
const dataList = ref([])
// 总记录数
const total = ref(0)

const data = reactive({
  form: {},
  queryParams: {},
})

const { queryParams } = toRefs(data)

proxy.getDicts('sys_article_status').then((response) => {
  statusOptions.value = response.data
})

// 查询数据
function getList() {
  studentList(queryParams.value).then((res) => {
    if (res.code == 200) {
      dataList.value = res.data.result
      total.value = res.data.totalNum

      dataList.value.forEach(function(e,i) {
        e['studentService'] = JSON.parse(e['studentService']);
        e['studentTag'] = e['studentTag'].split(",")
      });

      // arr = dataList.value.studentTag.splice(",");
      // console.log(dataList.value)
    }
  })
}
// 多选框选中数据
function handleSelectionChange(selection) {
  ids.value = selection.map((item) => item.studentId)
  single.value = selection.length != 1
  multiple.value = !selection.length
}

/** 重置查询操作 */
function resetQuery() {
  proxy.resetForm('queryForm')
  handleQuery()
}
/** 搜索按钮操作 */
function handleQuery() {
  getList()
}
// 导出按钮操作
function handleExport() {
  proxy
    .$confirm('是否确认导出所有学生数据项?', '警告', {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: 'warning',
    })
    .then(function () {
      return exportStudent(queryParams)
    })
    .then((response) => {
      proxy.download(response.data.path)
    })
}

/** 删除按钮操作 */
function handleDelete(row) {
  const Ids = row.studentId || ids.value

    ElMessageBox.confirm("是否确认删除？", "系统提示", {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: "warning",
    })
    .then(function () {
      return delStudent(Ids)
    })
    .then(() => {
      handleQuery()
      proxy.$modal.msgSuccess('删除成功')
    })
    .catch(() => { })
}

function handleUpdate(row) {
  EditDialogVisible.value = true
  EditDialogRow.value = row
}

// 详情
function handleDetail(row) {
  console.log(111)
  DetailDialogVisible.value = true
  DetailDialogRow.value = row
}

handleQuery()
</script>

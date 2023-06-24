<template>
  <div class="app-container">
    <el-row :gutter="24">
      <!-- 搜索框 queryParams.需要搜索的字段 -->
      <el-form :model="queryParams" label-position="left" inline ref="queryForm" label-width="100px" v-show="showSearch"
        @submit.prevent>
        <el-form-item label="班级名称" prop="className">
          <el-input v-model="queryParams.className" placeholder="请输入班级名称" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="search" @click="handleQuery">搜索</el-button>
          <el-button icon="refresh" @click="resetQuery">重置</el-button>
        </el-form-item>
      </el-form>
    </el-row>

    <!-- 按钮 -->
    <el-row :gutter="10" class="mb8">
      <el-col :span="1.5">
        <el-button type="primary" plain icon="plus" v-hasPermi="['business:class:add']"
          @click="AddDialogVisible = true">添加</el-button>
        <AddDialog v-model="AddDialogVisible" :done="() => resetQuery()"></AddDialog>
      </el-col>
      <right-toolbar :showSearch="showSearch"></right-toolbar>
    </el-row>

    <!-- 表格渲染  prop="对应的字段"-->
    <el-table :data="dataList" ref="tableRef" border highlight-current-row @selection-change="handleSelectionChange">
      <!-- <el-table-column prop="classId" label="id" width="60" sortable> </el-table-column> -->
      <el-table-column type="selection" width="50" align="center" />
      <el-table-column prop="className" label="班级名称" width="120" :show-overflow-tooltip="true"> </el-table-column>

      <el-table-column label="操作" align="center" width="250">
        <template #default="scope">
          <el-button text size="small" icon="edit" @click="handleUpdate(scope.row)"
            v-hasPermi="['business:class:update']">编辑</el-button>
          <el-button text size="small" icon="delete" @click="handleDelete(scope.row)"
            v-hasPermi="['business:class:delete']">删除</el-button>
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
</template>
<script setup >
import { ElMessageBox } from 'element-plus'
import { classList, delClass } from '@/api/business/school/class.js'
import AddDialog from "./components/AddDialog.vue";
import EditDialog from "./components/EditDialog.vue";
import DetailDialog from "./components/DetailDialog.vue";


const AddDialogVisible = ref(false);
const EditDialogVisible = ref(false);
const EditDialogRow = ref({});
const DetailDialogVisible = ref(false);
const DetailDialogRow = ref({});

const { proxy } = getCurrentInstance()

// 业务参数

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
// 文章预览地址
const previewUrl = ref('')
const data = reactive({
  form: {},
  queryParams: {},
})
const { queryParams } = toRefs(data)


// 查询数据
function getList() {
  classList(queryParams.value).then((res) => {
    if (res.code == 200) {
      dataList.value = res.data.result
      total.value = res.data.totalNum
    }
  })
}
// 多选框选中数据
function handleSelectionChange(selection) {
  ids.value = selection.map((item) => item.classId)
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

/** 删除按钮操作 */
function handleDelete(row) {
  const Ids = row.classId || ids.value

    ElMessageBox.confirm("是否确认删除？", "系统提示", {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: "warning",
    })
    .then(function () {
      return delClass(Ids)
    })
    .then(() => {
      handleQuery()
      proxy.$modal.msgSuccess('删除成功')
    })
    .catch(() => { })
}

// 修改
function handleUpdate(row) {
  EditDialogVisible.value = true
  EditDialogRow.value = row
}

// 详情
function handleDetail(row) {
  DetailDialogVisible.value = true
  DetailDialogRow.value = row
}

handleQuery()
</script>

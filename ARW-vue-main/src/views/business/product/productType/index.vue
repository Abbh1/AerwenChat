<template>
  <div productType="app-container">
    <el-row :gutter="24">
      <!-- :model属性用于表单验证使用 比如下面的el-form-item 的 prop属性用于对表单值进行验证操作 -->
      <el-form :model="queryParams" label-position="left" style="margin:15px;"  inline ref="queryForm" label-width="100px" v-show="showSearch"
        @submit.prevent>
        <el-form-item label="产品类型名称" prop="productTypeName">
          <el-input v-model="queryParams.productTypeName" placeholder="请输入产品类型名称" />
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="search" @click="handleQuery">搜索</el-button>
          <el-button icon="refresh" @click="resetQuery">重置</el-button>
        </el-form-item>
      </el-form>
    </el-row>

    <el-row :gutter="10" productType="mb8" style="margin-bottom:20px;">
      <el-col :span="1.5">
        <el-button type="primary" plain icon="plus" v-hasPermi="['business:productType:add']"
          @click="AddDialogVisible = true">添加</el-button>
        <AddDialog v-model="AddDialogVisible" :done="() => resetQuery()"></AddDialog>
      </el-col>
       <el-col :span="1.5">
        <el-button type="info" plain icon="sort" @click="toggleExpandAll">展开/折叠</el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button type="danger" :disabled="multiple" v-hasPermi="['business:productType:delete']" plain icon="delete" @click="handleDelete">
          {{ $t('btn.delete') }}
        </el-button>
      </el-col>
      <right-toolbar :showSearch="showSearch"></right-toolbar>
    </el-row>

    <el-table
    v-loading="loading"
    v-if="refreshTable"
    :data="dataList"
    ref="tableRef"
    border 
    highlight-current-row 
    @selection-change="handleSelectionChange"
    :default-expand-all="isExpandAll"
    row-key="productTypeId"
    :tree-props="{ children: 'children', hasChildren: 'hasChildren' }"
    >
      <!-- <el-table-column prop="productTypeId" label="id" width="60" sortable> </el-table-column> -->
      <el-table-column type="selection" width="50" align="center" />
      <el-table-column prop="productTypeName" label="产品类型名称" :show-overflow-tooltip="true"> </el-table-column>
      <el-table-column prop="productTypeId" label="产品类型id" align="center" />
      <el-table-column prop="parentId" label="父级id" align="center" />

      <el-table-column label="操作" align="center" width="250">
        <template #default="scope">
          <el-button text size="small" icon="edit" @click="handleUpdate(scope.row)"
            v-hasPermi="['business:productType:update']">编辑</el-button>
          <el-button text size="small" icon="delete" @click="handleDelete(scope.row)"
            v-hasPermi="['business:productType:delete']">删除</el-button>
          <el-button text size="small" icon="view" @click="handleDetail(scope.row)">查看</el-button>
        </template>
      </el-table-column>

    </el-table>
   
  </div>
  <!-- 编辑 -->
  <EditDialog v-model="EditDialogVisible" :data="EditDialogRow" :done="() => resetQuery()"></EditDialog>
  <!-- 详情 -->
  <DetailDialog v-model="DetailDialogVisible" :data="DetailDialogRow" :done="() => resetQuery()"></DetailDialog>
</template>
<script setup >
import { ElMessageBox } from 'element-plus'
import { productTypeTreeList, delProductType } from '@/api/business/product/productType.js'
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
// 是否展开，默认全部折叠
const isExpandAll = ref(false)
const refreshTable = ref(true)


// 选中categoryId数组数组
const ids = ref([])
// 非单选禁用
const single = ref(true)
// 非多个禁用
const multiple = ref(true)
// 遮罩层
const loading = ref(false)
// 显示搜索条件
const showSearch = ref(true)
// 数据列表
const dataList = ref([])

const data = reactive({
  form: {},
  queryParams: {
    pageNum: 1,
    pageSize: 10,
    sort: undefined,
    sortType: undefined,
  },
})
const { queryParams } = toRefs(data)


// -业务方法
function toggleExpandAll() {
  refreshTable.value = false
  isExpandAll.value = !isExpandAll.value
  nextTick(() => {
    refreshTable.value = true
  })
}


// -基础方法
// 查询数据
function getList() {
  loading.value = true
  productTypeTreeList(queryParams.value).then((res) => {
    if (res.code == 200) {
      dataList.value = res.data
      loading.value = false
    }
  })
}
// 多选框选中数据
function handleSelectionChange(selection) {
  ids.value = selection.map((item) => item.productTypeId)
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
  const Ids = row.productTypeId || ids.value

    ElMessageBox.confirm("是否确认删除？", "系统提示", {
      confirmButtonText: '确定',
      cancelButtonText: '取消',
      type: "warning",
    })
    .then(function () {
      return delProductType(Ids)
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

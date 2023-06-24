<template>
  <div class="app-container">
    <el-row :gutter="24">
      <!-- 搜索框 queryParams.需要搜索的字段 -->
      <el-form :model="queryParams"  inline ref="queryForm"
        label-width="80px" v-show="showSearch" @submit.prevent>

        <el-form-item label="用户名称:" prop="chatUserName">
          <el-input v-model="queryParams.chatUserName" placeholder="请输入用户名称" />
        </el-form-item>
        <el-form-item label="昵称:" prop="chatUserNickName">
          <el-input v-model="queryParams.chatUserNickName" placeholder="请输入昵称" />
        </el-form-item>
        <el-form-item label="登录状态:" prop="status">
          <el-select v-model="queryParams.chatUserStatus" class="m-2" placeholder="请选择登录状态">
            <el-option
              v-for="item in options"
              :key="item.value"
              :label="item.label"
              :value="item.value"
            />
          </el-select>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="search" @click="handleQuery">搜索</el-button>
          <el-button icon="refresh" @click="resetQuery">重置</el-button>
        </el-form-item>

      </el-form>
    </el-row>

    <!-- 按钮 -->
    <el-row :gutter="10" class="mb8">
      <right-toolbar :showSearch="showSearch"></right-toolbar>
    </el-row>

    <!-- 表格渲染  prop="对应的字段"-->
    <el-table :data="dataList" ref="tableRef" border highlight-current-row @selection-change="handleSelectionChange">
      <!-- <el-table-column prop="classId" label="id" width="60" sortable> </el-table-column> -->
      <el-table-column type="selection" width="50" align="center" />
      <el-table-column prop="chatUserName" label="用户名" width="200"> </el-table-column>
      <el-table-column prop="chatUserNickName" label="昵称" width="200"> </el-table-column>
      <el-table-column prop="age" label="年龄" width="200"> </el-table-column>
      <el-table-column prop="phone" label="电话" width="200"> </el-table-column>
      <el-table-column prop="email" label="邮箱" width="200"> </el-table-column>
      <el-table-column prop="status" label="登录状态" width="200">
        <template #default="scope">
          <el-switch v-model="scope.row.status" :loading="loading1" @click="beforeChange1(scope.row)" />
        </template>
      </el-table-column>

    </el-table>
    <pagination :total="total" v-model:page="queryParams.pageNum" v-model:limit="queryParams.pageSize"
      @pagination="getList" />
  </div>
</template>
<script setup >
import { ElMessageBox } from 'element-plus'
import { getChatUserList, updateChatUserStatus } from '@/api/business/chatUser.js'
import modal from '@/plugins/modal';
import { param } from '@/utils';

const { proxy } = getCurrentInstance()

// 业务参数
const loading1 = ref(false)

const options = [
  {
    value: '0',
    label: '未登录',
  },
  {
    value: '1',
    label: '已登陆',
  },
]

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

const statusData = ref({
  chatUserId: null,
  status: null
})

// 查询数据
function getList() {
  getChatUserList(queryParams.value).then((res) => {
    if (res.code == 200) {
      dataList.value = res.data.result
      total.value = res.data.totalNum

      dataList.value.forEach(function (e, i) {
        if (e['status'] == 0) {
          e['status'] = false;
        }
        if (e['status'] == 1) {
          e['status'] = true;
        }
      });

      // console.log(dataList.value,'我是你');
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
  queryParams.value.length = 0
  proxy.resetForm('queryForm')
  handleQuery()
}
/** 搜索按钮操作 */
function handleQuery() {
  getList()
}

const beforeChange1 = (v) => {
    loading1.value = true
    // console.log(v);
    statusData.value.chatUserId = v.chatUserId
    if(v.status == true){
      statusData.value.status = 1
    }else{
      statusData.value.status = 0
    }

    // console.log(statusData.value);

    updateChatUserStatus(statusData.value).then((res) => {
      if (res.code == 200) {
        modal.msgSuccess(res.data)
        loading1.value = false
        handleQuery()
      }
      else{
        modal.msgError(res.data)
      }
    })

    // setTimeout(() => {
    //   loading1.value = false
    //   ElMessage.success('Switch success')
    // }, 1000)
    
}


handleQuery()
</script>

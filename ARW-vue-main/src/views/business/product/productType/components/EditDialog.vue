<template>
  <el-dialog v-model="modelValue" title="修改产品类型信息" width="500px" @closed="closeDialog">
    <el-form ref="formRef" :model="data" :rules="rules">
      <el-row>
        <el-col :span="24">
          <el-form-item :label-width="labelWidth" label="产品类型" prop="productTypeName">
            <el-input v-model="data.productTypeName" type="text" placeholder="请输入产品类型"></el-input>
          </el-form-item>
        </el-col>

          <el-col :lg="24">
            <el-form-item  :label-width="labelWidth" label="父级id" prop="parentId">
              <el-cascader
                class="w100"
                :options="dataList"
                :props="{ checkStrictly: true, value: 'productTypeId', label: 'productTypeName', emitPath: false }"
                placeholder="请选择上级菜单"
                clearable
                v-model="data.parentId">
                <template #default="{ node, data }">
                  <span>{{ data.productTypeName }}</span>
                  <span v-if="!node.isLeaf"> ({{ data.children.length }}) </span>
                </template>
              </el-cascader>
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
import { productTypeTreeList , updateProductType } from '@/api/business/product/productType.js';


// 业务参数
const formRef = ref();
const labelWidth = 100;
const { proxy } = getCurrentInstance()
const dataList = ref([])


// 验证
const rules = reactive({
    className: [
    {
      required: true,
      message: "请输入产品类型名称",
    },
  ],
});

// -业务方法


// 基础参数
const props = defineProps({
  modelValue: Boolean,
  data: Object,
  done: Function,
});
const queryParams = reactive({
  pageNum: 1,
  pageSize: 10,
  sort: undefined,
  sortType: undefined,
})
const emits = defineEmits(["update:modelValue"]);

// watch(props, (v) => {
  // data = v.modelValue;
  // console.log(v.modelValue)
// });

// -基础方法
function getList() {
  productTypeTreeList(queryParams).then((res) => {
    if (res.code == 200) {
      dataList.value = res.data
    }
  })
}
// 查询
function handleQuery() {
  getList()
}
// 提交
const handleEditClick = async (formEl) => {
  if (!formEl) return;
  formEl.validate(async (valid) => {
    if (!valid) {
      return;
    }
    const { code } = await updateProductType(props.data);
    if (code == 200) {
      proxy.$modal.msgSuccess('修改成功')
      closeDialog();
      props.done();
      handleQuery();
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

handleQuery()
</script>

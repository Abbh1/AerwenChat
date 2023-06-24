<template>
  <el-dialog v-model="modelValue" title="添加产品类型信息" width="500px" @closed="closeDialog">
    <el-form ref="formRef" :model="formData" :rules="rules">
      <el-row>
        <el-col :span="24">
          <el-form-item :label-width="labelWidth" label="产品类型" prop="productTypeName">
            <el-input v-model="formData.productTypeName" type="text" placeholder="请输入产品类型"></el-input>
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
                v-model="formData.parentId">
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
      <div productType="dialog-footer">
        <el-button type="primary" @click="handleAddClick(formRef)">添加</el-button>
        <el-button @click="handleResetClick(formRef)">重置</el-button>
      </div>
    </template>
  </el-dialog>
</template>


<script setup>
import { reactive, ref, watch } from "vue";
import { ElMessage } from 'element-plus'
import { productTypeTreeList , addProductType } from '@/api/business/product/productType.js';


// 业务参数
const formRef = ref();
const labelWidth = 100;
const { proxy } = getCurrentInstance()
const dataList = ref([])


const emits = defineEmits(["update:modelValue"]);
const dialogVisible = ref(props.modelValue);
const formData = reactive({
});
const props = defineProps({
  modelValue: Boolean,
  done: Function,
});
const queryParams = reactive({
  pageNum: 1,
  pageSize: 10,
  sort: undefined,
  sortType: undefined,
})
// watch(props,async (v) => {
//   dialogVisible.value = v.modelValue;
// });

// 验证
const rules = reactive({
  productTypeName: [
    {
      required: true,
      message: "请输入产品类型名称",
    },
  ],
});

// -业务方法


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
const handleAddClick = async (formEl) => {
  // console.log(formData)
  if (!formEl) return;
  formEl.validate(async (valid) => {
    if (!valid) {
      return;
    }
    const { code } = await addProductType(formData);
    if (code == 200) {
      proxy.$modal.msgSuccess('添加成功')
      closeDialog();
      props.done();
      handleQuery();
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

handleQuery()
</script>
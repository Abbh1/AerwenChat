import request from '@/utils/request'

// 查询树列表
export function productTypeTreeList(query) {
  return request({
    url: '/business/productType/getProductTypeTreeList',
    method: 'get',
    params: query
  })
}

// 新增
export function addProductType(data) {
  return request({
    url: '/business/productType/addProductType',
    method: 'post',
    data: data,
  })
}

// 修改
export function updateProductType(data) {
  return request({
    url: '/business/productType/updateProductType',
    method: 'put',
    data: data
  })
}

// 删除
export function delProductType(ids) {
  return request({
    url: '/business/productType/'+ ids,
    method: 'delete'
  })
}


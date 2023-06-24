import request from '@/utils/request'

// 查询列表
export function classList(query) {
  return request({
    url: '/business/class/GetClassList',
    method: 'get',
    params: query
  })
}

// 新增
export function addClass(data) {
  return request({
    url: '/business/class/AddClass',
    method: 'post',
    data: data,
  })
}

// 修改
export function updateClass(data) {
  return request({
    url: '/business/class/UpdateClass',
    method: 'put',
    data: data
  })
}

// 删除
export function delClass(ids) {
  return request({
    url: '/business/class/'+ ids,
    method: 'delete'
  })
}


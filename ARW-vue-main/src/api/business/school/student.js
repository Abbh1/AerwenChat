import request from '@/utils/request'

// 查询列表
export function studentList(query) {
  return request({
    url: '/business/student/GetStudentList',
    method: 'get',
    params: query
  })
}

// 新增
export function addStudent(data) {
  return request({
    url: '/business/student/AddStudent',
    method: 'post',
    data: data,
  })
}

// 修改
export function updateStudent(data) {
  return request({
    url: '/business/student/UpdateStudent',
    method: 'put',
    data: data
  })
}

// 删除
export function delStudent(ids) {
  return request({
    url: '/business/student/'+ ids,
    method: 'delete'
  })
}

// 查询班级列表
export function getClassList(query) {
  return request({
    url: '/business/student/getClassList',
    method: 'get',
    params: query
  })
}

// 导出学生目录
export function exportStudent(query) {
  return request({
    url: 'business/student/export',
    method: 'get',
    params: query
  })
}
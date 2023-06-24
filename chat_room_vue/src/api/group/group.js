import request from '../../utils/request'


// 群聊列表
export function groupList(query) {
  return request({
    url: '/business/group/groupList',
    method: 'get',
    params: query
  })
}

// 添加群聊
export function ad(data) {
  return request({
    url: '/business/groupUser/addGroup',
    method: 'post',
    data: data
  })
}

// 创建群聊
export function createGroup(data) {
  return request({
    url: '/business/group/CreateGroup',
    method: 'post',
    data: data
  })
}

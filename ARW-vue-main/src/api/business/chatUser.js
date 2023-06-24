import request from '@/utils/request'

// 查询列表
export function getChatUserList(query) {
  return request({
    url: '/business/chatUser/getChatUserList',
    method: 'get',
    params: query
  })
}

// 修改
export function updateChatUserStatus(data) {
  return request({
    url: '/business/chatUser/updateChatUserStatus',
    method: 'put',
    data: data
  })
}



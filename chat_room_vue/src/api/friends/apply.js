import request from '../../utils/request'

// 申请列表
export function applyList(query) {
  return request({
    url: '/business/apply/applyList',
    method: 'get',
    params: query
  })
}


// 发送申请
export function sendApply(data) {
  return request({
    url: '/business/apply/sendApply',
    method: 'post',
    data: data
  })
}

// 查看申请
export function checkApply(data) {
  return request({
    url: '/business/apply/checkApply',
    method: 'post',
    data: data
  })
}

/**
 * 注册
 * @returns 
 */
export function register(chatusernickname,chatusername, password) {
  const data = {
    chatusernickname,
    chatusername,
    password
  }
  return request({
    url: '/business/friends/register',
    method: 'post',
    data: data
  })
}

// 已读处理
export function read(data) {
  return request({
    url: '/business/apply/read',
    method: 'post',
    data: data
  })
}
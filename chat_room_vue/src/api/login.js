import request from '../utils/request'

// 登录方法
export function login(chatnickname,chatusername, password) {
  const data = {
    chatnickname,
    chatusername,
    password
  }
  return request({
    url: '/business/chatLogin/login',
    method: 'POST',
    data: data,
  })
}

// 修改用户信息
export function updateChatUser(data) {
  return request({
    url: '/business/chatLogin/updateChatUser',
    method: 'put',
    data: data
  })
}

// 获取用户详细信息
export function getInfo(chatusername) {
  const data = {
    chatusername
  }
  return request({
    url: '/business/chatLogin/getInfo',
    method: 'POST',
    data: data
  })
}

// 退出方法
export function logout() {
  return request({
    url: '/LogOut',
    method: 'POST'
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
    url: '/business/chatLogin/register',
    method: 'post',
    data: data
  })
}

/**
 * 查看好友状态
 */
export function checkFriendStatus(params) {
  return request({
    url: '/business/chatLogin/checkFriendStatus',
    method: 'get',
    params: params
  })
}
import request from '../../utils/request'


// 好友列表
export function friendsList(query) {
  return request({
    url: '/business/friends/friendsList',
    method: 'get',
    params: query
  })
}

// 查找好友
export function findFriends(friendName) {
  const data = {
    friendName
  }
  return request({
    url: '/business/friends/findFriends',
    method: 'POST',
    data: data
  })
}

// 添加好友
export function add(data) {
  return request({
    url: '/business/friends/addFriend',
    method: 'post',
    data: data
  })
}

// 修改好友备注
export function updateNote(data) {
  return request({
    url: '/business/friends/updateNote',
    method: 'put',
    data: data
  })
}
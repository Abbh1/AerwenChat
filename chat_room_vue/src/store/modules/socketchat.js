import { defineStore,storeToRefs} from 'pinia'

const useSocketCahtStore = defineStore('socketchat', {
  state: () => ({
    onlineNum: 0,
    chatOnlineUsers: [],
    noticeList: [],
    noticeDot: false
  }),
  actions: {
    //更新在线人数
    setOnlineUserNum(num) {
      this.onlineNum = num
    },
    // 更新系统通知
    setNoticeList(data) {
      this.noticeList = data
      this.noticeDot = data.length > 0
    },

    // 在线用户
    setOnlineChatUsers(data) {
      this.chatOnlineUsers = data
    },
  }
})
export default useSocketCahtStore

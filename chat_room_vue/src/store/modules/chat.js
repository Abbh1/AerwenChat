import { defineStore,storeToRefs} from 'pinia'
import useSwitchStore from './switch'
import { getChatLogList } from '../../api/chatLog/chatLog'

const switchStore = useSwitchStore()

const useChatStore = defineStore('chat', {
    state: () => {
        return {
            chatUser: null,
            chatGroup: null,
            chatLogList: [],
        }
    },
    actions: {

      
        async getList(v){
            return new Promise((resolve, reject) => {
                getChatLogList(v)
                .then((res) => {
                  if(res.code == 200){
                    this.chatLogList = res.data
                    resolve(res.data)
                  }
                  else{
                    console.log('get error ', res)
                  }
                })
                .catch((err) => {
                  console.log('get error ', err)
                  reject('获取最近聊天记录信息失败')
                })
              })
        }
    },
    getters: {

    }
})

export default useChatStore;
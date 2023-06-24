import { login, logout, getInfo } from '../../api/login'
import { setToken, removeToken } from '../../utils/auth'
import defAva from '../../assets/img/profile.jpg'
import img from '../../assets/img/profile.jpg'
import Cookies from 'js-cookie'
import {defineStore} from 'pinia'
// import Cookies from 'js-cookie'
import { encrypt } from '../../utils/jsencrypt'
const useUserStore = defineStore('user', {
  state: () => ({
    name: '',
    userinfo:'',
    status: 0
  }),
  actions: {
    getUserInfo(username){
      // console.log(username);
      return new Promise((resolve, reject) => {
        getInfo(username)
        .then((res) => {
          if(res.code == 200){
            this.userinfo = res.data
            this.status = 1
            if (res.data.chatUserImg == null) {
              res.data.chatUserImg = img
            }
            sessionStorage.setItem('userInfo', JSON.stringify(res.data));
            Cookies.set('username', username, { expires: 30 })
            resolve(res.data)
          }
          else{
            console.log('get error ', res)
          }
        })
        .catch((err) => {
          console.log('get error ', err)
          reject('获取用户信息失败')
        })
      })
  
    }
  }
})
export default useUserStore

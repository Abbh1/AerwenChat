import * as signalR from '@microsoft/signalr'
import { ElNotification } from 'element-plus'
import useSocketCahtStore from '../store/modules/socketchat'
export default {
  // signalR对象
  SR: {},
  // 失败连接重试次数
  failNum: 4,
  baseUrl: '',
  async init(url) {
    var socketUrl = window.location.origin + url
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(socketUrl, {
      })
      .withAutomaticReconnect() //自动重新连接
      .configureLogging(signalR.LogLevel.Warning)
      .build();
    this.SR = connection
    // 断线重连
    connection.onclose(async () => {
      console.log('断开连接了')
      console.assert(connection.state === signalR.HubConnectionState.Disconnected);
      // 建议用户重新刷新浏览器
      await this.start()
    })

    connection.onreconnected(() => {
      console.log('断线重新连接成功')
    })
    this.receiveMsg(connection)
    // 启动
    await this.start();
  },
  /**
   * 调用 this.signalR.start().then(async () => { await this.SR.invoke("method")})
   * @returns 
   */
  async start() {
    var that = this
    // console.log(this.SR);
    try {
      //使用async和await 或 promise的then 和catch 处理来自服务端的异常
      await this.SR.start()

      //console.assert(this.SR.state === signalR.HubConnectionState.Connected);
      console.log('signalR', this.SR.state);
      return true
    } catch (error) {
      that.failNum--;
      // console.log(`失败重试剩余次数${that.failNum}`, error)
      if (that.failNum > 0 && this.SR.state.Disconnected) {
        setTimeout(async () => {
          await this.SR.start()
        }, 5000)
      }
      return false
    }
  },
  // 接收消息处理
  receiveMsg(connection) {
    // 接收在线用户
    connection.on("onlineChatUser", (data) => {
      useSocketCahtStore().setOnlineChatUsers(data)
    })
  }
}
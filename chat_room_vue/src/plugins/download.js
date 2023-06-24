import axios from 'axios'
import { ElMessage } from 'element-plus'
import { saveAs } from 'file-saver'
import errorCode from '../utils/errorCode'
import { blobValidate } from '../utils/ruoyi'

const baseURL = "/dev-api"

export default {
  name(name, isDelete = true) {
    var url = baseURL + "/common/download?fileName=" + encodeURI(name) + "&delete=" + isDelete
    axios({
      method: 'get',
      url: url,
      responseType: 'blob',
    }).then(async (res) => {
      const isLogin = await blobValidate(res.data);
      if (isLogin) {
        const blob = new Blob([res.data])
        this.saveAs(blob, decodeURI(res.headers['download-filename']))
      } else {
        this.printErrMsg(res.data);
      }
    })
  },
  resource(resource) {
    var url = baseURL + "/common/download/resource?resource=" + encodeURI(resource);
    axios({
      method: 'get',
      url: url,
      responseType: 'blob',
    }).then(async (res) => {
      const isLogin = await blobValidate(res.data);
      if (isLogin) {
        const blob = new Blob([res.data])
        this.saveAs(blob, decodeURI(res.headers['download-filename']))
      } else {
        this.printErrMsg(res.data);
      }
    })
  },
  zip(url, name) {
    var url = baseURL + url
    axios({
      method: 'get',
      url: url,
      responseType: 'blob',
    }).then(async (res) => {
      const isLogin = await blobValidate(res.data);
      if (isLogin) {
        const blob = new Blob([res.data], { type: 'application/zip' })
        this.saveAs(blob, name)
      } else {
        this.printErrMsg(res.data);
      }
    })
  },
  saveAs(text, name, opts) {
    saveAs(text, name, opts);
  },
  async printErrMsg(data) {
    const resText = await data.text();
    const rspObj = JSON.parse(resText);
    const errMsg = errorCode[rspObj.code] || rspObj.msg || errorCode['default']
    ElMessage.error(errMsg);
  }
}


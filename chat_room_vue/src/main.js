import { createApp,watch } from 'vue'
// import './style.css'
import App from './App.vue'
import router from './router'
import pinia from './store/index'
import ElementPlus from 'element-plus'
import ImagePreview from "./components/ImagePreview/index.vue"
import ImageUpload from './components/ImageUpload/index.vue'
import ImageUpload2 from './components/ImageUpload/index2.vue'
import Emjoi from './components/Emjoi/index.vue'
import 'element-plus/dist/index.css'
import 'font-awesome/css/font-awesome.min.css'
import plugins from './plugins' // plugins
import elementIcons from './components/SvgIcon/svgicon'
import signalR from './utils/signalR'

let app = createApp(App)
// signalR.init(
//     import.meta.env.VITE_APP_SOCKET_API)
app.config.globalProperties.signalr = signalR

app.component('UploadImage', ImageUpload)
app.component('UploadImage2', ImageUpload2)
app.component('ImagePreview', ImagePreview)
app.component('Emjoi', Emjoi)

app.use(pinia).use(router).use(plugins).use(ElementPlus, {}).use(elementIcons)
app.mount('#app')

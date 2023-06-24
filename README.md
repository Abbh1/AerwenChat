

# ✨Aerwen / Chat

#### 基于.NET6 + vue3.x前后端分离的实时聊天室



##  🍭概述

- 本项目适合有一定NetCore和 vue基础的开发人员

- 基于NET6实现的实时聊天室。整合最新技术高效快速开发，前后端分离模式，vue3Js 进行对接。

- > 现有功能：
  >
  > 1、登录
  >
  > 2、搜索&添加 好友/群聊
  >
  > 3、好友/群聊 实时图文聊天
  >
  > 4、emjoi表情
  >
  > 5、配套后台管理系统

```
如果需要帮助，可以加微信：a2679599887(收到会及时回复~)
或者在底下留言~
```

## 🍿在线体验

- 在线聊天室 体验：[http://chat.aerwen.net](http://chat.aerwen.net)
- 体验号 账号密码：test/test  |  test2/test2
- 在线后台管理 体验：[http://chatvue.aerwen.net](http://chatvue.aerwen.net)
- 账号密码：admin/123456



## 🌺特别鸣谢

[后端框架：ZrAdminNet (在此框架二次开发)](https://gitee.com/izory/ZrAdminNetCore?_from=gitee_search)

[前端参考: b站@**山羊の前端小窝**](https://space.bilibili.com/266664645/video)



## 🍕内置功能

1. 登录/注册
    
![登录](ARW-net/document/%E7%99%BB%E5%BD%95%E7%95%8C%E9%9D%A2.jpg)
   注册流程：

   ![注册流程](ARW-net/document/image/%E6%B3%A8%E5%86%8C%E6%B5%81%E7%A8%8B.gif)

2. 个人中心：修改个人信息。

   ![输入图片说明](ARW-net/document/image/%E4%B8%AA%E4%BA%BA%E4%B8%AD%E5%BF%83.jpg)

3. 添加好友/群聊：通过用户名或群名查找。

   
![输入图片说明](ARW-net/document/image/%E8%81%8A%E5%A4%A9%E9%A6%96%E9%A1%B5.jpg)
   好友申请：

   ![输入图片说明](ARW-net/document/image/%E5%A5%BD%E5%8F%8B%E7%94%B3%E8%AF%B7.jpg)

4. 好友简介：查看好友信息,可以修改好友备注。

   ![输入图片说明](ARW-net/document/image/%E5%A5%BD%E5%8F%8B%E7%AE%80%E4%BB%8B.jpg)

5. 聊天：与好友实时聊天或者留言。

   ![输入图片说明](ARW-net/document/image/%E8%81%8A%E5%A4%A9%E6%88%AA%E5%9B%BE.jpg)

   ![输入图片说明](ARW-net/document/image/%E5%AE%9E%E6%97%B6%E8%81%8A%E5%A4%A9%E6%BC%94%E7%A4%BA.gif)

   上传图片：

   ![输入图片说明](ARW-net/document/image/%E4%B8%8A%E4%BC%A0%E5%9B%BE%E7%89%87.gif)
   群聊：

   ![输入图片说明](ARW-net/document/image/%E7%BE%A4%E8%81%8A%E6%BC%94%E7%A4%BA.gif)

6. emjoi表情包：支持发送emjoi表情。

   [组件github地址](https://github.com/ADKcodeXD/Vue3-Emoji)

   ![输入图片说明](ARW-net/document/image/emjoi%E8%A1%A8%E6%83%85%E5%8C%85.gif)
7. 配套后台管理 (附源码)

   ![输入图片说明](ARW-net/document/image/%E5%90%8E%E5%8F%B0%E9%A6%96%E9%A1%B5.jpg)

![输入图片说明](ARW-net/document/image/%E5%90%8E%E5%8F%B0%E8%B4%A6%E6%88%B7%E7%AE%A1%E7%90%86.jpg)



## 🍞项目结构

```
├─ARW-net	->[.Net 后端]
├─ARW-vue-main	->[后台管理vue]
├─chat_room_vue	->[聊天室前端vue]
```



## 🔧使用介绍

后端启动：打开 **ARWAdmin.sln** 后 ctrl+F5

详细文档： [ZRAdmin.NET在线文档 (izhaorui.cn)](http://www.izhaorui.cn/doc/)



前端启动： 

```
进入项目目录：chat_room_vue

# 安装依赖
npm install --registry=https://registry.npm.taobao.org

# 本地开发 启动项目
npm run dev
```



详细技术要点：https://blog.csdn.net/weixin_60435181/article/details/127203154?spm=1001.2014.3001.5502

请作者喝杯奶茶
![输入图片说明](http://tool.aerwen.net/prod-api/Uploads/uploads/20230624/90DD095DB2EA3183.jpg)
![输入图片说明](http://tool.aerwen.net/prod-api/Uploads/uploads/20230624/BCF6A4B9D65154A8.jpg)

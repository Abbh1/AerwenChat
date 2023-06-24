<h2 align="center"> Vue后台开发流程</h2>



## 🎉优势

1. 前台系统不用编写登录、授权、认证模块；只负责编写业务模块即可
2. 后台系统无需任何二次开发，直接发布即可使用
3. 前台与后台系统分离，分别为不同的系统（域名可独立）
4. 全局异常统一处理
5. 自定义的代码生成功能
6. 国际化



## 🍁前端运行

```bash
# 进入项目vue目录
cd ARW-vue-main

# 安装依赖
npm i

# 启动服务
npm run dev

# 前端访问地址 http://localhost:8887
```

确保**后端**的服务已启动：

![](https://drive.kongwu.top/image/image/1dcb6d8203fddda558d50486225ae051.jpg)



**账号：**

- 管理员：admin
- 密  码：123456



------

## ✨ 开发流程

点击这里查看 (文档)[http://www.izhaorui.cn/doc/frontend.html#%E5%BC%80%E5%8F%91%E8%A7%84%E8%8C%83]



###  1、新增 view

> 在 `src/views/business` 文件下 创建对应的文件夹，
>
> 一般性一个路由对应一个文件，
>
> 各个功能模块维护自己的 utils 或 components 组件。

**Tip：可以复制粘贴，修改字段，删除不需要的字段或按钮即可**

示例：添加`学校管理(一级目录)`下的 `班级管理(二级菜单)`

![](https://drive.kongwu.top/image/image/6ed2c509c2d404a9446750a3d7a04734.jpg)



```json
# 具体功能可以参考 模板页面 `class/index(简易版)` | `student/index(具体功能版)`

// 搜索框
// 功能按钮(添加、删除)
// 表格渲染
// 弹窗

# 修改 api 接口位置
import { xxxList, delxxx } from '@/api/business/xxx模块/xxx.js'
```



------



### 2、弹窗组件

![](https://drive.kongwu.top/image/image/c004bd21f769a25a7a1fae07ed683e3c.jpg)

* 添加 
* 修改
* 详情

```js
# 修改 api 接口位置
import { xxxList, delxxx } from '@/api/business/xxx模块/xxx.js'
```



> 报表主要几个核心功能：
>
> 输入框
>
> 下拉框
>
> 字典选项
>
> 标签
>
> json
>
> 图片
>
> 富文本
>
> 弹窗

```json
// 具体操作请看模板
```



------



### 3、新增 api

在 `src/api/business `文件夹下创建本模块对应的 api 服务。

![image-20220727105124808](https://drive.kongwu.top/image/image/70787392f978e2fd24662cb6869f3f88.jpg)

```js
# 其中一个请求示例，修改 url请求地址 和 method请求方法

// 查询列表
export function classList(query) {
  return request({
    url: '/business/class/GetClassList',
    method: 'get',
    params: query
  })
}
```



------



### 4、添加菜单

打开 `系统管理` 的 `菜单管理` **点击新增按钮**

![](http://119.23.54.43/oss/test/20220727/F8A6A9BFD98036BA.jpg)



![](http://119.23.54.43/oss/test/20220727/E0E09E81BBC35E03.jpg)



![](http://119.23.54.43/oss/test/20220727/4FFC6A70AB40C4B9.jpg)

```
# 因为需要做 `权限管理` 所有使用的是权限字符
```

![](http://119.23.54.43/oss/test/20220727/5D9EA6F2808A960E.jpg)

```
# 使用了权限字符，就要给对应的按钮，添加权限字符(与后端沟通，或者看任务表)
```

![](http://119.23.54.43/oss/test/20220727/3C5DF2089ECDC331.jpg)

![](http://119.23.54.43/oss/test/20220727/B8493ECDF4A79630.jpg)





------



### 5、查看接口数据

[Swagger官网教程](https://docs.microsoft.com/zh-cn/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-5.0)



![](https://drive.kongwu.top/image/image/9e32cc653ce2d3dc782ab293e4d1b38b.jpg)

![](https://drive.kongwu.top/image/image/3e1374276316e1a284458320e017e08c.jpg)

![](http://119.23.54.43/oss/test/20220727/BAE6BA89F9B2BBAB.jpg)

![](http://119.23.54.43/oss/test/20220727/A1FADA3B87909DE1.jpg)

![](http://119.23.54.43/oss/test/20220727/521DA421B69E4C20.jpg)







## ⚙ 参数修改

项目标题： 在 `.env` 中的 `VITE_APP_TITLE`。

主题颜色：在 `src` 中的 `settings.js` 里面有后台的主题参数。

或者直接在后台修改~

![](http://119.23.54.43:8888/uploads/20220729/25F435E16122D654.jpg)

![](http://119.23.54.43/oss/20220729/8782D5526D7419CC.jpg)

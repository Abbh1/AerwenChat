# .NET后端开发流程



基于.NET6 + vue3 前后端分离的.net快速开发框架



##  🍟 概述

- 本项目适合有一定**NetCore**和 **vue基础**的开发人员
- 基于**.NET6**实现的通用权限管理平台（RBAC模式）。整合最新技术高效快速开发，前后端分离模式，开箱即用。



## 🍀 后端技术

- 核心框架：.Net6.0 + **Web API** + **sqlsugar** + **swagger** + signalR + IpRateARWmit + Quartz.net + Redis
- 定时计划任务：Quartz.Net组件，支持执行程序集或者http网络请求,**备份数据库**
- 安全支持：过滤器(数据权限过滤)、Sql注入、请求伪造
- 日志管理：NLog、登录日志、操作日志、定时任务日志
- 工具类：验证码、丰富公共功能
- 接口限流：支持接口限流，避免恶意请求导致服务层压力过大
- 代码生成：高效率开发，代码生成器可以一键生成所有前后端代码
- 数据字典：支持数据字典，可以方便对一些状态进行管理
- 分库分表：使用orm sqlsugar可以很轻松的实现分库分库性能优越
- 多 租 户：支持多租户功能
- 缓存数据：内置内存缓存和Redis



##  🔧 配置

1. 用 Vs 打开项目解决方案文件 `ARWAdmin.sln`

   下载地址:https://visualstudio.microsoft.com/zh-hans/

2. 连接服务器数据库 （navicat）

3. 修改数据库连接配置 `ARW.WebApi` 目录下的 `appsettings.json` 配置

```json
{
  "ConnectionStrings": {
		"conn_db": "Data Source=localhost;Initial Catalog=ARWAdmin;User ID=sa;Password=ARWadmin123;Trusted_Connection=SSPI", //当前项目连接数据库,
		"conn_db_type": 1, //选择对应的数据库类型MySql = 0, SqlServer = 1。其他数据库自行添加基础数据
  }
```

​	现在尝试启动项目吧 ~



##  🍻项目结构

```
├─Infrastructure                ->[基础设施]：提供基础系统工具类。
├─ARW.WebApi               ->[webapi接口]：为Vue版或其他三方系统提供接口服务。
├─ARW.Model                		->[实体层类库]：提供项目中的数据库表、数据传输对象；
├─ARW.Common              		->[常用工具]：提供项目中常用方法；
├─ARW.Repository                  ->[仓库层类库]：方便提供有执行存储过程的操作；
├─ARW.Service             		->[服务层类库]：提供WebApi接口调用；
├─ARW.Tasks               		->[定时任务类库]：提供项目定时任务实现功能；
├─ARW.CodeGenerator               ->[代码生成功能]：包含代码生成的模板、方法、代码生成的下载。
```



## 🔍 开发规范

>开发时 不管是 哪一个层
>
>都要 **按模块** **分好文件夹**
>
>都要 **写好注释！！！！**
>
>/// 使用这个注释



## 🧬 开发流程

![1](https://drive.kongwu.top/image/image/c3ffb787475b88c557b271414ef67c12.png)



------



### 1、创建模型 (Model层)

#### 1.1  Model (数据交互)

数据交互：与**数据库**打交道的类，最后接收数据返回到这个类，

​				 然后映射到数据库 ，数据库再把结果返回 （详细去了解ORM）



这里我们使用的是 **SqlSugar** 作为 ORM  

官方文档：https://www.donet5.com/Home/Doc?typeId=1182

[实体常用属性](https://blog.csdn.net/weixin_60435181/article/details/125729548?spm=1001.2014.3001.5502)

> 具体如何写实体类，可以参考实例实体 
>
> ARW.Model -> Models -> Business -> Student 或者 Class

* 需要注意的是：
  * 业务表的前缀是 : tb_
  * 系统表的前缀是 : sys_
  * 记得继承 ：BusinessBase
  * Id 使用的是 **雪花ID**
  * **时间(DateTime)**的需要进行转换
  * excel导出 表头特要标记 EpplusTableColumn[ ]

​	



#### 1.2  Dto (数据传输)

只接收指定参数，更加**清晰**知道传入什么参数

```c#
// 接受的参数 是 Dto类型的
// [FromBody] 是 json请求体
public IActionResult AddStudent([FromBody] StudentDto parm)
```

**传统Model层：**

![](https://drive.kongwu.top/image/image/bacbca5d023bccae0233070ce7aaa297.jpg)

**Dto层：更加清晰 知道传入什么值，方便前端对接~**

![](https://drive.kongwu.top/image/image/14fe22617c61d6fc7dca9cd8b22763e4.jpg)





#### 1.3  Vo (数据展示)

![](https://drive.kongwu.top/image/image/04228d577e85b9906a8ce1f90c1f6676.png)

**展现需要的字段：**

![](https://drive.kongwu.top/image/image/4f04bcaf9ce79eee0b52ba5af51be3b0.jpg)



定义一个新的类，**只展示我们需要的字段**

使用 **Select()** 把实体值 赋值给 新的类就可以了。 

```c#
var query = _studentRepository
                .Queryable()
                .LeftJoin<Class>((s, c) => s.ClassId == c.ClassId)
                .Where(predicate.ToExpression())
                .Select((s, c) => new StudentVo
                {
                    StudentId = s.StudentId,
                    ClassId = c.ClassId,
                    ClassName = c.ClassName,
                    StudentName = s.StudentName,
                    Sex = s.Sex,
                    Age = s.Age,
                    StudentImg = s.StudentImg,
                    StudentTag = s.StudentTag,
                    StudentService= s.StudentService.ToString(),
                    StudentDescribe = s.StudentDescribe
                });

```



------



### 2、创建接口和实现类 （Service层）

作用：

1、代码整洁

2、分层，好维护

3、可以注册服务，不需要实例化



#### 2.1 创建接口

在 `ARW.Service` -> `Business` -> `IBusinessService` 中 创建**接口** (interface)

注意：记得 继承 **IBaseService<对应的数据实体类>**

* 命名规范：**IxxxService** 
  * 前缀 I
  * 后缀 Service



**Interface(接口)：**

```c#
    public interface IStudentService : IBaseService<Student>
    {
        /// <summary>
        /// 获取学生分页列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        PagedInfo<StudentVo> GetStudentList(StudentQueryDto parm);

    }
```



#### 2.2 创建实现类

在 `ARW.Service` -> `Business` -> `BusinessService` 中 创建**实现类** (Impl)

注意：记得 继承 **BaseService<对应的数据实体类>，相对应的接口**

​	**添加服务到容器**( 特性 )： [AppService(ServiceType = typeof( `相对应的接口` ), ServiceLifetime = LifeTime.Transient)]

```c#
[AppService(ServiceType = typeof(IStudentService), ServiceLifetime = LifeTime.Transient)]
public class StudentServiceImpl : BaseService<Student>, IStudentService
```



`实现类要实现 接口所定义的方法`

```c#
		// 写业务方法

		public PagedInfo<StudentVo> GetStudentList(StudentQueryDto parm)
            {
                //开始拼装查询条件d
                var predicate = Expressionable.Create<Student>();

                //搜索条件查询语法参考Sqlsugar
                predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.StudentName), s => s.StudentName.Contains(parm.StudentName));
                predicate = predicate.AndIF(parm.ClassId != null, s => s.ClassId == parm.ClassId);

                var query = _studentRepository
                    .Queryable()
                    .LeftJoin<Class>((s, c) => s.ClassId == c.ClassId)
                    .Where(predicate.ToExpression())
                    .Select((s, c) => new StudentVo
                    {
                        StudentId = s.StudentId,
                        ClassId = c.ClassId,
                        ClassName = c.ClassName,
                        StudentName = s.StudentName,
                        Sex = s.Sex,
                        Age = s.Age,
                        StudentImg = s.StudentImg,
                        StudentTag = s.StudentTag,
                        StudentService= s.StudentService.ToString(),
                        StudentDescribe = s.StudentDescribe
                    });


                return query.ToPage(parm);
            }
```



### 3、创建仓储层(Repository层)

在 `ARW.Repository` -> `Business`  中 创建**仓储类** (Repository)、

作用：用于超级复杂的业务逻辑，当 `接口实现类 ` 都写的很乱时，可以使用这个层

一般走流程需要创建一个，所以参考以下模板创建：

```c#
[AppService(ServiceLifetime = LifeTime.Transient)]
public class StudentRepository : BaseRepository<Student>
{
    /*
    * 复杂的业务逻辑代码
    */
}
```



------



### 4、创建控制层 (Controller层)

**-- 这里细节比较多，请注意看 --**



**控制器头部：**

```c#
  [Verify] // 身份认证
  [Route("business/[controller]")] // 路由
  public class StudentController : BaseController // 继承 BaseController
```



**依赖注入(构造函数注入)：**

```c#
    /// <summary>
    /// 依赖注入
    /// </summary>
    /// <param name="studentService">学生服务</param>
    /// <param name="classService">班级服务</param>
    public StudentController(IStudentService studentService, IClassService classService)
    {
        _studentService = studentService;
        _classService = classService;
    }
```



**方法：**

* 请求参数格式：

​			**[FromBody]：**json请求体

​			**[FromQuery]：** url 获取参数

​			**[FromForm(Name = "xxx")] ：**获取From表单中的参数

* 抛出异常：

  ​	**throw new CustomException(" ")**

* 返回结果：

​			**SUCCESS("消息")**

​			**ToResponse(数据，"消息")**



```c#
// 头部规范：
[HttpPost("addStudent")] // [请求方法("请求路由")]
[ActionPermissionFilter(Permission = "business:student:add")] // 权限标识符
[Log(Title = "学生添加", BusinessType = BusinessType.INSERT)] // 日志
public IActionResult AddStudent([FromBody] StudentDto parm)
{
    // 抛出异常
    if (parm == null)
    {
    	throw new CustomException("请求参数错误");
    }

    // 模型映射：Dto层需要把接受的数据 给到 Model层，才能和数据库交互
    var modal = parm.Adapt<Student>().ToCreate(HttpContext);

    var response = _studentService.Insertable(modal).ExecuteReturnSnowflakeId();
    return SUCCESS("添加成功!");
}
```



>查询分页(联表)
>
>查询树形
>
>查询单条
>
>添加
>
>修改
>
>删除
>
>导入Excel
>
>导出Excel
>
>下载模板Excel

具体操作方法可参考：

`ARW.WebApi` -> `Business` -> `StudentController(基础方法)` / `ProductTypeController(树形)`


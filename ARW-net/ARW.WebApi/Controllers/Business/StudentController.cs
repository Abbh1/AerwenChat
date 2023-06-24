using Infrastructure;
using Infrastructure.Attribute;
using Infrastructure.Enums;
using Infrastructure.Model;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ARW.Admin.WebApi.Extensions;
using ARW.Admin.WebApi.Filters;
using ARW.Common;
using ARW.Model.Dto.Business;
using ARW.Model.Models.Business;
using ARW.Model.Vo;
using ARW.Service.Business.IBusinessService;

namespace ARW.Admin.WebApi.Controllers.Business
{
    /// <summary>
    /// 学生控制器
    /// </summary>
    [Verify]
    [Route("business/[controller]")]
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;
        private readonly IClassService _classService;

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

        /// <summary>
        /// 获取学生列表
        /// </summary>
        /// <param name="parm">查询参数</param>
        [HttpGet("getStudentList")]
        [ActionPermissionFilter(Permission = "business:student:list")]
        public IActionResult GetStudentARWst([FromQuery] StudentQueryDto parm)
        {
            var res = _studentService.GetStudentList(parm);
            //res.Result = res.Result.Select(a => new Student{ StudentGuid = a.StudentGuid,Name = a.Name}).ToList();
            return SUCCESS(res);
        }

        /// <summary>
        /// 添加学生
        /// </summary>
        /// <returns></returns>
        [HttpPost("addStudent")]
        [ActionPermissionFilter(Permission = "business:student:add")]
        [Log(Title = "学生添加", BusinessType = BusinessType.INSERT)]
        public IActionResult AddStudent([FromBody] StudentDto parm)
        {
            if (parm == null)
            {
                throw new CustomException("请求参数错误");
            }

            var modal = parm.Adapt<Student>().ToCreate(HttpContext);

            var response = _studentService.Insertable(modal).ExecuteReturnSnowflakeId();
            return SUCCESS("添加成功!");
        }

        /// <summary>
        /// 更新学生
        /// </summary>
        /// <returns></returns>
        [HttpPut("updateStudent")]
        [ActionPermissionFilter(Permission = "business:student:update")]
        [Log(Title = "学生修改", BusinessType = BusinessType.UPDATE)]
        public IActionResult UpdateStudent([FromBody] StudentDto parm)
        {
            if (parm == null)
            {
                throw new CustomException("请求参数错误");
            }

            var modal = parm.Adapt<Student>().ToUpdate(HttpContext);

            var data = _studentService.GetFirst(it => it.StudentId == modal.StudentId);
            if(data == null)
            {
                throw new CustomException("学生不存在");
            }
         

            var response = _studentService.Update(it => it.StudentId == modal.StudentId,
                f => new Student
                {
                    ClassId = modal.ClassId,
                    DictType = modal.DictType,
                    StudentName = modal.StudentName,
                    Sex = modal.Sex,
                    Age = modal.Age,
                    StudentTag = modal.StudentTag,
                    StudentImg = modal.StudentImg,
                    StudentService = modal.StudentService,
                    StudentDescribe = modal.StudentDescribe,
                });

            return SUCCESS(response);
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{ids}")]
        [ActionPermissionFilter(Permission = "business:student:delete")]
        [Log(Title = "学生删除", BusinessType = BusinessType.DELETE)]
        public IActionResult DeleteStudent(string ids)
        {
            long[] idsArr = Tools.SpitLongArrary(ids);

            if (idsArr.Length <= 0) { return ToResponse(ApiResult.Error($"删除失败Id 不能为空")); }

            var response = _studentService.Delete(idsArr);

            return ToResponse(response,"删除成功!");
        }

        /// <summary>
        /// 获取班级列表(下拉框)
        /// </summary>
        /// <returns></returns>
        [HttpGet("getClassList")]
        [ActionPermissionFilter(Permission = "business:student:classlist")]
        public IActionResult getClassList()
        {
            var res = _classService.GetAll();
            //res.Result = res.Result.Select(a => new Student{ StudentGuid = a.StudentGuid,Name = a.Name}).ToList();
            return SUCCESS(res);
        }


        /// <summary>
        /// 导出学生
        /// </summary>
        /// <param name="formFile">使用IFromFile必须使用name属性否则获取不到文件</param>
        /// <returns></returns>
        [HttpPost("importData")]
        [Log(Title = "学生导入", BusinessType = BusinessType.IMPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        [ActionPermissionFilter(Permission = "business:student:import")]
        public IActionResult ImportExcel ([FromForm(Name = "file")] IFormFile formFile)
        {
            IEnumerable<StudentDto> parm = ExcelHelper<StudentDto>.ImportData(formFile.OpenReadStream());

            var student = new StudentDto();
            foreach (StudentDto item in parm)
            {
                student = item;
            }

            var modal = student.Adapt<Student>().ToCreate(HttpContext);
            var response = _studentService.Insertable(modal).ExecuteReturnSnowflakeId();

            //TODO 业务逻辑,自行插入数据到db
            return SUCCESS(response);
        }

        /// <summary>
        /// 学生导入模板下载
        /// </summary>
        /// <returns></returns>
        [HttpGet("importTemplate")]
        [Log(Title = "学生模板", BusinessType = BusinessType.EXPORT, IsSaveRequestData = false, IsSaveResponseData = false)]
        [AllowAnonymous]
        public IActionResult ImportTemplateExcel()
        {
            List<StudentDto> student = new List<StudentDto>();
            MemoryStream stream = new MemoryStream();

            string sFileName = DownloadImportTemplate(student, stream, "学生列表");
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{sFileName}");
        }


        /// <summary>
        /// 导出学生
        /// </summary>
        /// <returns></returns>
        [Log(Title = "学生导出", BusinessType = BusinessType.EXPORT, IsSaveResponseData = false)]
        [HttpGet("export")]
        [ActionPermissionFilter(Permission = "business:student:export")]
        public IActionResult ExportExcel ([FromQuery] StudentQueryDto parm)
        {
            parm.PageSize = 10000;
            var list = _studentService.GetStudentList(parm).Result;

            string sFileName = ExportExcel(list, "Student", "学生列表");
            return SUCCESS(new { path = "/export/" + sFileName, fileName = sFileName });
        }

    }
}

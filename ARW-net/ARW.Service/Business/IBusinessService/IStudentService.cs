using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Dto.Business;
using ARW.Model.Models.Business;
using ARW.Model.Vo;

namespace ARW.Service.Business.IBusinessService
{
    public interface IStudentService : IBaseService<Student>
    {
        /// <summary>
        /// 获取学生分页列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        PagedInfo<StudentVo> GetStudentList(StudentQueryDto parm);


    }
}

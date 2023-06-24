using Infrastructure.Attribute;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARW.Model;
using ARW.Model.Dto.Business;
using ARW.Model.Models.Business;
using ARW.Model.Vo;
using ARW.Repository;
using ARW.Repository.Business;
using ARW.Service.Business.IBusinessService;

namespace ARW.Service.Business.BusinessService
{
    /// <summary>
    /// 学生接口实现类
    /// </summary>
    [AppService(ServiceType = typeof(IStudentService), ServiceLifetime = LifeTime.Transient)]
    public class StudentServiceImpl : BaseService<Student>, IStudentService
    {
        private readonly StudentRepository _studentRepository;

        public StudentServiceImpl(StudentRepository studentRepository)
        {
            this._studentRepository = studentRepository;
        }

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


    }
}

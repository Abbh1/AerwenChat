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
using ARW.Repository;
using ARW.Repository.Business;
using ARW.Service.Business.IBusinessService;

namespace ARW.Service.Business.BusinessService
{
    /// <summary>
    /// 班级接口实现类
    /// </summary>
    [AppService(ServiceType = typeof(IClassService), ServiceLifetime = LifeTime.Transient)]
    public class ClassServiceImpl : BaseService<Class>, IClassService
    {
        private readonly ClassRepository _classRepository;

        public ClassServiceImpl(ClassRepository classRepository)
        {
            this._classRepository = classRepository;
        }

        public PagedInfo<Class> GetClassList(ClassQueryDto parm)
        {
            //开始拼装查询条件d
            var predicate = Expressionable.Create<Class>();

            //搜索条件查询语法参考Sqlsugar
            predicate = predicate.AndIF(!string.IsNullOrEmpty(parm.ClassName), it => it.ClassName.Contains(parm.ClassName)) ;


            var query = _classRepository
                .Queryable()
                .Where(predicate.ToExpression());

            return query.ToPage(parm);
        }


    }
}

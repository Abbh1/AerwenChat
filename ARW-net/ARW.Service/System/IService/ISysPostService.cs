using System;
using System.Collections.Generic;
using System.Text;
using ARW.Model.System;
using ARW.Repository;

namespace ARW.Service.System.IService
{
    public interface ISysPostService : IBaseService<SysPost>
    {
        string CheckPostNameUnique(SysPost sysPost);
        string CheckPostCodeUnique(SysPost sysPost);
        List<SysPost> GetAll();
    }
}

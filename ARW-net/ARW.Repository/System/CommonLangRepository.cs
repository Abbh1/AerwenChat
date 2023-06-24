using System;
using Infrastructure.Attribute;
using ARW.Repository.System;
using ARW.Model.Models;

namespace ARW.Repository
{
    /// <summary>
    /// 多语言配置仓储
    ///
    /// @author zr
    /// @date 2022-05-06
    /// </summary>
    [AppService(ServiceLifetime = LifeTime.Transient)]
    public class CommonLangRepository : BaseRepository<CommonLang>
    {
        #region 业务逻辑代码
        #endregion
    }
}
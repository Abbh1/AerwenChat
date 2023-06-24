using System;
using System.Collections.Generic;
using ARW.Model.System;

namespace ARW.Service.System.IService
{
    /// <summary>
    /// 通知公告表service接口
    ///
    /// @author 
    /// 
    /// @date 2021-12-15
    /// </summary>
    public interface ISysNoticeService: IBaseService<SysNotice>
    {
        List<SysNotice> GetSysNotices();
    }
}

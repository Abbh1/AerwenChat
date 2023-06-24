using System;
using ARW.Model;
using ARW.Model.Dto;
using ARW.Model.Models;
using System.Collections.Generic;

namespace ARW.Service.System.IService
{
    /// <summary>
    /// 多语言配置service接口
    ///
    /// @author 
    /// 
    /// @date 2022-05-06
    /// </summary>
    public interface ICommonLangService : IBaseService<CommonLang>
    {
        PagedInfo<CommonLang> GetList(CommonLangQueryDto parm);
        List<CommonLang> GetLangList(CommonLangQueryDto parm);
        dynamic GetListToPivot(CommonLangQueryDto parm);
        void StorageCommonLang(CommonLangDto parm);
        Dictionary<string, object> SetLang(List<CommonLang> msgList);
    }
}

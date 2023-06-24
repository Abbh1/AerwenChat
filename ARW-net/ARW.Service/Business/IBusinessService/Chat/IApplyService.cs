using ARW.Model.Models.Business.Chat;
using ARW.Model.Chat;
using ARW.Model.Dto.Business;
using ARW.Model.Vo;
using ARW.Model;
using ARW.Model.Vo.Chat;

namespace ARW.Service.Business.IBusinessService.Chat
{
    public interface IApplyService : IBaseService<Apply>
    {

        PagedInfo<ChatApplyRequestVo> GetApplyList(ApplyQueryDto parm);

    }
}

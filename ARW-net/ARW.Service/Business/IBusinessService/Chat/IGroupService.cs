using ARW.Model.Models.Business.Chat;
using ARW.Model.Chat;
using ARW.Model;
using ARW.Model.Vo.Chat;

namespace ARW.Service.Business.IBusinessService.Chat
{
    public interface IGroupService : IBaseService<Group>
    {
        
        PagedInfo<GroupVo> GetGroupList(GroupQueryDto parm);
    }
}

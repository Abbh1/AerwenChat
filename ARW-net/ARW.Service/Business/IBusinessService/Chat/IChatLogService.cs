using ARW.Model.Models.Business.Chat;
using ARW.Model.Chat;
using ARW.Model;
using ARW.Model.Vo.Chat;
using System.Collections.Generic;

namespace ARW.Service.Business.IBusinessService.Chat
{
    public interface IChatLogService : IBaseService<ChatLog>
    {
        List<ChatLogListVo> GetChatLogList(ChatLogQueryDto parm);

        PagedInfo<ChatLogVo> GetChatFriendLogList(ChatLogQueryDto parm);

        PagedInfo<ChatLogVo> GetChatGroupLogList(ChatLogQueryDto parm);

        List<ChatLogVo> GetChatFriendHistoryLogList(ChatLogQueryDto parm);

        List<ChatLogVo> GetChatGroupLogHistoryList(ChatLogQueryDto parm);

        public long AddChatFriendLog(ChatLogDto dto);

        public ChatLogVo SelfNewMsg(ChatLogQueryDto parm);

        public void Read(ChatLogQueryDto parm);

        public void GroupRead(ChatLogQueryDto parm);

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using ARW.Model.System;

namespace ARW.Model.System.Dto
{
    public class SysRoleDto: SysBase
    {
        public long RoleId { get; set; }
        /// <summary>
        /// 角色分配菜单
        /// </summary>
        public List<long> MenuIds { get; set; } = new List<long>();
        public string RoleName { get; set; }
        public string RoleKey { get; set; }
        public int RoleSort { get; set; }
        public string Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using ARW.Model.System;
using ARW.Model.System.Vo;

namespace ARW.Service.System.IService
{
    public interface ISysDeptService : IBaseService<SysDept>
    {
        List<SysDept> GetSysDepts(SysDept dept);
        string CheckDeptNameUnique(SysDept dept);
        int InsertDept(SysDept dept);
        int UpdateDept(SysDept dept);
        void UpdateDeptChildren(long deptId, string newAncestors, string oldAncestors);
        List<SysDept> GetChildrenDepts(List<SysDept> depts, long deptId);
        List<SysDept> BuildDeptTree(List<SysDept> depts);
        List<TreeSelectVo> BuildDeptTreeSelect(List<SysDept> depts);
        List<SysRoleDept> SelectRoleDeptByRoleId(long roleId);

        List<long> SelectRoleDepts(long roleId);
        bool DeleteRoleDeptByRoleId(long roleId);
        int InsertRoleDepts(SysRole role);
    }
}

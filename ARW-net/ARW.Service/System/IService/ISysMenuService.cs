using System.Collections.Generic;
using ARW.Model.System.Dto;
using ARW.Model.System;
using ARW.Model.System.Vo;

namespace ARW.Service.System.IService
{
    public interface ISysMenuService
    {
        //List<SysMenu> SelectMenuList(long userId);

        List<SysMenu> SelectMenuList(MenuQueryDto menu, long userId);
        List<SysMenu> SelectTreeMenuList(MenuQueryDto menu, long userId);

        SysMenu GetMenuByMenuId(int menuId);
        List<SysMenu> GetMenusByMenuId(int menuId);
        int AddMenu(SysMenu menu);

        int EditMenu(SysMenu menu);

        int DeleteMenuById(int menuId);

        string CheckMenuNameUnique(SysMenu menu);

        int ChangeSortMenu(MenuDto menuDto);

        bool HasChildByMenuId(long menuId);

        List<SysMenu> SelectMenuTreeByUserId(long userId);

        List<SysMenu> SelectMenuPermsListByUserId(long userId);

        List<string> SelectMenuPermsByUserId(long userId);

        bool CheckMenuExistRole(long menuId);

        List<RouterVo> BuildMenus(List<SysMenu> menus);

        List<TreeSelectVo> BuildMenuTreeSelect(List<SysMenu> menus);
    }
}

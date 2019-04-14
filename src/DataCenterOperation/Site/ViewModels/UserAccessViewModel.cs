using DataCenterOperation.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataCenterOperation.Site.ViewModels
{
    public enum UserPermission
    {
        UnKnow,
        Admin,
        Normal
    }

    public class UserAccessViewModel
    {
        public User User { get; set; }

        public UserPermission UserPermission { get; set; }

        public static UserAccessViewModel GetUserAcessInfo(User user)
        {
            UserAccessViewModel accessViewModel = new UserAccessViewModel();

            accessViewModel.User = user;

            UserPermissionMapping(accessViewModel);

            return accessViewModel;
        }

        public static void UserPermissionMapping(UserAccessViewModel accessViewModel)
        {
            if (accessViewModel.User.IsAdmin)
            {
                accessViewModel.UserPermission = UserPermission.Admin;
            }
            else
            {
                accessViewModel.UserPermission = UserPermission.Normal;
            }
        }
    }
}

using Adrack.Core;
using Adrack.Core.Domain.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Security;
using Adrack.PlanManagement;

namespace Adrack.WebApi.Models.Users
{
    public class UserInfoViewModel
    {
        public long Id { get; set; }
        public string TestVar { get; set; }
        public long ParentId { get; set; }
        public UserTypes UserType { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public long? DepartmentId { get; set; }
        public string BuiltInName { get; set; }
        public bool BuiltIn { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime ActivityDate { get; set; }
        public RoleModel Role { get; set; }

        public string ProfilePicture { get; set; }

        public short TrialDays { get; set; }
        public bool hasCard { get; set; }

        public List<UserAddonsModel> Addons { get; set; }
        public List<IdNameModel> Campaigns { get; set; }
        public List<IdNameModel> Buyers { get; set; }
        public List<IdNameModel> BuyerChannels { get; set; }
        public List<IdNameModel> Affiliates { get; set; }
        public List<IdNameModel> AffiliateChannels { get; set; }
        public IList<AdrackPlanVerificationStatus> PlanLimitation { get; set; }

        public static explicit operator UserInfoViewModel(User user)
        {
            return new UserInfoViewModel()
            {
                Id = user.Id,
                ActivityDate = user.ActivityDate,
                BuiltIn = user.BuiltIn,
                BuiltInName = user.BuiltInName,
                DepartmentId = user.DepartmentId,
                LoginDate = user.LoginDate,
                Email = user.Email,
                ParentId = user.ParentId,
                UserName = user.Username,
                UserType = user.UserType,
                ProfilePicture = user.ProfilePicturePath,
                Campaigns = new List<IdNameModel>(),
                BuyerChannels = new List<IdNameModel>(),
                Buyers = new List<IdNameModel>(),
                AffiliateChannels = new List<IdNameModel>(),
                Affiliates = new List<IdNameModel>(),
                PlanLimitation = new List<AdrackPlanVerificationStatus>()
            };
        }
    }
}
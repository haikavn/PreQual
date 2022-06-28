using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adrack.WebApi.Models.BaseModels;
using Adrack.WebApi.Models.Interfaces;
using Adrack.WebApi.Models.Users;

namespace Adrack.WebApi.Models.Support
{
    public class TicketsUsersInfoModel : BaseModel, IBaseOutModel
    {
        #region Constructor

        public TicketsUsersInfoModel()
        {
            UsersNameList = new List<KeyValuePair<string, long>>();
        }

        #endregion

        #region Properties

        public List<KeyValuePair<string, long>> UsersNameList { get; internal set; }
        public long AffiliateId { get; internal set; }
        public long BuyerId { get; internal set; }
        public long SelectedBuyerId { get; internal set; }
        public UserSimpleModel ManagerUser { get; internal set; }

        #endregion
    }
}
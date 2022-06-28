// ***********************************************************************
// Assembly         : Adrack.Controller
// Author           : Arman Zakaryan
// Created          : 12-02-2021
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 12-02-2021
// ***********************************************************************
// <copyright file="GetAllPaymentsModel.cs" company="Adrack.com">
//     Copyright © 2021
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.WebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Adrack.WebApi.Models.Billing
{
    public class GetAllPaymentsModel : IBaseInModel
    {
        public List<PaymentsModel> paymentsModelList;
        public int? CurrentPingsLimit { get; set; }
        public int? CurrentLeadsLimit { get; set; }

        public int? RemainingPingsLimit { get; set; }
        public int? RemainingLeadsLimit { get; set; }


        public string CurrentPlanName { get; set; }

        public DateTime ExpireDate { get; set; }

        public List<GetAddonPaymentsModel> addonPaymentsModelList;
        public GetAllPaymentsModel()
        {
            paymentsModelList = new List<PaymentsModel>();
            addonPaymentsModelList = new List<GetAddonPaymentsModel>();
        }

    }
}
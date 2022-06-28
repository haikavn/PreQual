// ***********************************************************************
// Assembly         : Adrack.Controller
// Author           : Arman Zakaryan
// Created          : 12-02-2022
//
// Last Modified By : Arman Zakaryan
// Last Modified On : 12-02-2022
// ***********************************************************************
// <copyright file="GetAddonPaymentsModel.cs" company="Adrack.com">
//     Copyright © 2022
// </copyright>
// <summary></summary>
// ***********************************************************************

using Adrack.WebApi.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Adrack.WebApi.Models.Billing
{
    public class GetAddonPaymentsModel : IBaseInModel
    {
        public long Id { get; set; }

        public DateTime PaymentDate { get; set; }

        public DateTime ExpireDate { get; set; }

        public short PaymentMethod { get; set; }

        public string PaymentMethodName { get; set; }

        public double Price { get; set; }

        public bool Annually { get; set; }


        public string AddonName { get; set; }


        public int InvoiceNumber { get; set; }
        public string InvoiceUrl { get; set; }

        public short Status { get; set; }

        public int? CreditCardType { get; set; }



        public GetAddonPaymentsModel()
        {
            
        }

    }
}
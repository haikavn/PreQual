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

namespace Adrack.WebApi.Models.Billing
{
    public class PaymentsModel : IBaseInModel
    {
        public long Id { get; set; }

        public DateTime PaymentDate { get; set; }

        public DateTime ExpireDate { get; set; }

        public short PaymentMethod { get; set; }
        
        public string PaymentMethodName { get; set; }

        public double Price { get; set; }

        public bool Annually { get; set; }

        public short Plan { get; set; }
        
        public string PlanName { get; set; }

        public int PingsLimit { get; set; }

        public int InvoiceNumber { get; set; }
        public string InvoiceUrl { get; set; }

        public short Status { get; set; }

        public int? LeadsLimit { get; set; }

        public int? CreditCardType { get; set; }

    }
}
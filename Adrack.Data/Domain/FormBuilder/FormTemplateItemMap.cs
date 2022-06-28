// ***********************************************************************
// Assembly         : Adrack.Core
// Author           : Adrack Team
// Created          : 03-15-2019
//
// Last Modified By : Arman
// Last Modified On : 04-08-2019
// ***********************************************************************
// <copyright file="FormTemplate.cs" company="Adrack.com">
//     Copyright © 2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using Adrack.Data;
using System;

namespace Adrack.Core.Domain.Lead
{
    public partial class FormTemplateItemMap : AppEntityTypeConfiguration<FormTemplateItem>
    {
        public FormTemplateItemMap()
        {
            this.ToTable("FormTemplateItem");

            this.HasKey(x => x.Id);
        }
    }
}
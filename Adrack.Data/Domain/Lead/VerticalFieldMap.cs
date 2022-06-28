using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adrack.Core.Domain.Lead;

namespace Adrack.Data.Domain.Lead
{
    /// <summary>
    /// Class CampaignMap.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.VerticalField}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Lead.VerticalField}" />
    public partial class VerticalFieldMap : AppEntityTypeConfiguration<VerticalField>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public VerticalFieldMap() // elite group
        {
            this.ToTable("VerticalField");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}

using Adrack.Core.Domain.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Data.Domain.Content
{
    /// <summary>
    /// Class EntityChangeHistory.
    /// Implements the <see cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.EntityChangeHistory}" />
    /// </summary>
    /// <seealso cref="Adrack.Data.AppEntityTypeConfiguration{Adrack.Core.Domain.Content.EntityChangeHistory}" />
    public partial class EntityChangeHistoryMap : AppEntityTypeConfiguration<EntityChangeHistory>
    {
        #region Constructor

        /// <summary>
        /// Address Map
        /// </summary>
        public EntityChangeHistoryMap()
        {
            this.ToTable("EntityChangeHistory");

            this.HasKey(x => x.Id);
        }

        #endregion Constructor
    }
}

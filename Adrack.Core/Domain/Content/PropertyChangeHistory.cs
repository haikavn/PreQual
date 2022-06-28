using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adrack.Core.Domain.Content
{
    /// <summary>
    /// Property Changed Class
    /// </summary>
    public class PropertyChangeHistory
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the OldValue
        /// </summary>
        public string OldValue { get; set; }

        /// <summary>
        /// Gets or sets the NewValue
        /// </summary>
        public string NewValue { get; set; }
    }
}

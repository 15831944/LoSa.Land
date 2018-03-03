using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoSa.Land.Settings
{
    /// <summary>
    /// Интерфейс Settings
    /// </summary>
    public interface ISettings
    {
        /*
        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        ISettings Default { get; }
        */
    }

    public abstract class ASettings :ISettings
    {
        public static ISettings Default { get; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TTMS.Data.Common
{
    public enum TravelerStatus
    {
        /// <summary>
        /// Traveler is no longer authorized to use the system
        /// </summary>
        ///
        Inactive,
        /// <summary>
        /// Traveler is eligible for new assignments
        /// </summary>
        Active,
        /// <summary>
        /// Traveler's current position in time is unknown or untrackable
        /// </summary>
        [Description("M.I.A.")]
        MIA,
        /// <summary>
        /// Traveler's decease has been confirmed
        /// </summary>
        [Description("K.I.A.")]
        KIA
    }
}

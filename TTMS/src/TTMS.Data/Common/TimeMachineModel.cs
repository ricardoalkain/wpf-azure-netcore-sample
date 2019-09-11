using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TTMS.Data.Common
{
    public enum TimeMachineModel
    {
        [Description("Fixed Time Capsule (Long Range HGW-60)")]
        Capsule,
        [Description("Suitcase (1895 Victorian)")]
        Suitcase,
        [Description("Pocket Watch")]
        Watch,
        [Description("Vehicle (Modified 1986 DeLorean)")]
        DeLorean,
        [Description("Telephone Booth (2015 Tardis)")]
        Tardis,
        [Description("< Unknowm method >")]
        Unknown
    }
}

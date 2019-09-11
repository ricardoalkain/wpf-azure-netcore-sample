using System.ComponentModel;

namespace TTMS.Data.Common
{
    public enum TimeMachineModel
    {
        [Description("")]
        None,
        [Description("Long Range Machine (Sled HGW-95)")]
        Capsule,
        [Description("Suitcase (1891 model)")]
        Suitcase,
        [Description("Pocket Watch (Mount Royal model)")]
        Watch,
        [Description("1985 DMC-12 DeLorean")]
        DeLorean,
        [Description("Telephone Booth (Tardis)")]
        Tardis,
        [Description("SkyNet's Time Displacement Sphere")]
        SkyNet,
        [Description("< Unknowm method >")]
        Unknown
    }
}

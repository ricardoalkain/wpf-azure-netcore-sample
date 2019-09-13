using System.ComponentModel;

namespace TTMS.Common.Enums
{
    public enum DeviceModel
    {
        [Description("")]
        None,
        [Description("Long Range Device (Time Sled HGW-95)")]
        Capsule,
        [Description("Suitcase (1891 model)")]
        Suitcase,
        [Description("Pocket Watch (1963 Mount Royal)")]
        Watch,
        [Description("1985 DMC-12 DeLorean")]
        DeLorean,
        [Description("Telephone Booth (Tardis)")]
        Tardis,
        [Description("SkyNet's Time Displacement Sphere")]
        SkyNet,
        [Description("<< Unknowm method >>")]
        Unknown,
        [Description("Short Range Device (Primer-2004)")]
        PrimerBox
    }
}

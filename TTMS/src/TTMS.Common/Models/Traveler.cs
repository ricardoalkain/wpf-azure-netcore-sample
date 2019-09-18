using System;
using TTMS.Common.Enums;

namespace TTMS.Common.Models
{
    public class Traveler
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public string Skills { get; set; }

        public byte[] Picture { get; set; }

        public DateTime BirthDate { get; set; }

        public int BirthTimelineId { get; set; }

        public string BirthLocation { get; set; }

        public DateTime LastDateTime { get; set; }

        public int LastTimelineId { get; set; }

        public string LastLocation { get; set; }

        public TravelerType Type { get; set; }

        public TravelerStatus Status { get; set; }

        public DeviceModel DeviceModel { get; set; }
    }
}

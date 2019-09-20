using System;
using System.ComponentModel.DataAnnotations;
using TTMS.Common.Enums;

namespace TTMS.Common.DTO
{
    public class TravelerRequest
    {
        public Guid? Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Alias { get; set; }

        public string Skills { get; set; }

        public byte[] Picture { get; set; }

        public DateTime BirthDate { get; set; }

        public int BirthTimelineId { get; set; }

        public string BirthLocation { get; set; }

        public DateTime LastDateTime { get; set; } = DateTime.Now;

        public int LastTimelineId { get; set; }

        public string LastLocation { get; set; }

        [Required]
        public TravelerType Type { get; set; }

        public TravelerStatus Status { get; set; } = TravelerStatus.Active;

        public DeviceModel DeviceModel { get; set; } = DeviceModel.Unknown;
    }
}

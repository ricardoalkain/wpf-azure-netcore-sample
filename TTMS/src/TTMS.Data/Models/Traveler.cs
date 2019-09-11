using System;
using System.Collections.Generic;
using TTMS.Data.Common;

namespace TTMS.Data.Models
{
    public class Traveler
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public string Skills { get; set; }

        public string PictureUri { get; set; }

        public byte[] Picture { get; set; }

        public DateTime BirthDate { get; set; }

        public int BirthTimelineId { get; set; }

        public string BirthLocation { get; set; }

        public DateTime LastDateTime { get; set; }

        public int LastTimelineId { get; set; }

        public string LastLocation { get; set; }

        public TravelerType TravelerType { get; set; }

        public TravelerStatus Status { get; set; }

        public TimeMachineModel TimeMachineModel { get; set; }
    }
}

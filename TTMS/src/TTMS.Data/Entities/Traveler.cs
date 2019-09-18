﻿using System;
using Microsoft.WindowsAzure.Storage.Table;
using TTMS.Common.Enums;

namespace TTMS.Data.Entities
{
    public class Traveler : TableEntity
    {
        // RowKey => Id
        //PartitionKey => Type

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

        public TravelerStatus Status { get; set; }

        public DeviceModel DeviceModel { get; set; }
    }
}

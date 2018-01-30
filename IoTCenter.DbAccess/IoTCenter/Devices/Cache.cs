namespace IoTCenter.DbAccess.IoTCenter.Devices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("devices.Cache")]
    public partial class Cache
    {
        public int Id { get; set; }

        public int DeviceId { get; set; }

        public string Data { get; set; }

        public DateTime? DateLogged { get; set; }
    }
}

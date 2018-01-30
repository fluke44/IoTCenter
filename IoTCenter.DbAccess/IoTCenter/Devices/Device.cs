namespace IoTCenter.DbAccess.IoTCenter.Devices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("devices.Device")]
    public partial class Device
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(17)]
        public string Mac { get; set; }

        [Required]
        [StringLength(15)]
        public string Ip { get; set; }

        [Required]
        [StringLength(100)]
        public string Type { get; set; }

        public bool Registered { get; set; }

        public DateTime? DateRegistered { get; set; }
    }
}

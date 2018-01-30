namespace IoTCenter.DbAccess.IoTCenter.Devices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("devices.vwCachedData")]
    public partial class CachedDataView
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(255)]
        public string Name { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string Type { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string Mac { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(15)]
        public string Ip { get; set; }

        public string Data { get; set; }

        public DateTime? DateLogged { get; set; }
    }
}

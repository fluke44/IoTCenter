using IoTCenter.DbAccess.IoTCenter.Devices;

namespace IoTCenter.DbAccess.IoTCenter
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class IoTCenterContext : DbContext
    {
        public IoTCenterContext()
            : base("name=IoTCenter")
        {
        }

        //public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<Cache> Cache { get; set; }
        public virtual DbSet<CachedDataView> CachedDataView { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}

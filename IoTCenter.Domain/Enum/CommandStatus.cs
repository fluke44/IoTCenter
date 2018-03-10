using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoTCenter.Domain.Enum
{
    public enum CommandStatus
    {
        [Description("")]
        Unknown,
        [Description("Pending")]
        Pending,
        [Description("Complete")]
        Complete,
        [Description("Failed")]
        Failed
    }
}

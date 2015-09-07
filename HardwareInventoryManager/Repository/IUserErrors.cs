using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareInventoryManager.Repository
{
    public interface IUserErrors
    {
        string[] Errors { get; set; }
    }
}

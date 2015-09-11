using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Models
{
    public class AssetDetail : ModelEntity, ITenant
    {
        [Key]
        public int AssetDetailId { get; set; }

        public int TenantId { get; set; }

        public string ComputerName { get; set; }
        public string WMIStatus { get; set; }
        public string SSHStatus { get; set; }
        public string CurrentOperatingSystem { get; set; }
        public string ServicePackLevel { get; set; }
        public string ActiveNetworkAdapter { get; set; }
        public string IPAddress { get; set; }
        public string MACAddress { get; set; }
        public string DNSServer { get; set; }
        public string SubnetMask { get; set; }
        public string WINSServer { get; set; }
        public string RegisteredUserName { get; set; }
        public string DomainWorkgroup { get; set; }
        public string DaysSinceDomainAccountUpdate { get; set; }
        public string NumberOfProcessors { get; set; }
        public string NumberOfCores { get; set; }
        public string LogicalProcessorCount { get; set; }
        public string CPU { get; set; }
        public string SystemMemoryMB { get; set; }
        public string VideoCard { get; set; }
        public string VideoCardMemoryMB { get; set; }
        public string SoundCard { get; set; }
        public string DiskDrive { get; set; }
        public string DiskDriveSizeGB { get; set; }
        public string OpticalDrive { get; set; }
        public string BIOS { get; set; }
        public string BIOSSerialNumber { get; set; }
        public string BIOSManufacturer { get; set; }
        public string BIOSReleaseDate { get; set; }
        public string MachineType { get; set; }

    }
}
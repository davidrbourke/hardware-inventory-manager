using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.ViewModels
{
    public class AssetDetailViewModel
    {
        public int AssetDetailId { get; set; }

        public int TenantId { get; set; }
        
        [Display(Name="Computer Name")]
        public string ComputerName { get; set; }
        
        [Display(Name="WMI Status")]
        public string WMIStatus { get; set; }

        [Display(Name="SSH Status")]
        public string SSHStatus { get; set; }

        [Display(Name="Operating System")]
        public string CurrentOperatingSystem { get; set; }

        [Display(Name="Service Pack")]
        public string ServicePackLevel { get; set; }

        [Display(Name="Active Network Adapter")]
        public string ActiveNetworkAdapter { get; set; }

        [Display(Name="IP Address")]
        public string IPAddress { get; set; }

        [Display(Name="MAC Address")]
        public string MACAddress { get; set; }

        [Display(Name="DNS Server")]
        public string DNSServer { get; set; }

        [Display(Name="Subnet Mask")]
        public string SubnetMask { get; set; }

        [Display(Name="WINS Server")]
        public string WINSServer { get; set; }

        [Display(Name="Registered User Name")]
        public string RegisteredUserName { get; set; }

        [Display(Name="Domain/WorkGroup")]
        public string DomainWorkgroup { get; set; }

        [Display(Name="Days Since Domain Account Update")]
        public string DaysSinceDomainAccountUpdate { get; set; }

        [Display(Name="Number of Processors")]
        public string NumberOfProcessors { get; set; }

        [Display(Name="Number of Cores")]
        public string NumberOfCores { get; set; }

        [Display(Name="Logical Processor Count")]
        public string LogicalProcessorCount { get; set; }

        [Display(Name="CPU")]
        public string CPU { get; set; }

        [Display(Name="System Memory (MB)")]
        public string SystemMemoryMB { get; set; }

        [Display(Name="Video Card")]
        public string VideoCard { get; set; }

        [Display(Name="Video Card Memory (MB)")]
        public string VideoCardMemoryMB { get; set; }

        [Display(Name="Sound Card")]
        public string SoundCard { get; set; }

        [Display(Name="Disk Drive")]
        public string DiskDrive { get; set; }

        [Display(Name="Disk Drive Size (GB)")]
        public string DiskDriveSizeGB { get; set; }

        [Display(Name="Optical Drive")]
        public string OpticalDrive { get; set; }

        [Display(Name="BIOS")]
        public string BIOS { get; set; }

        [Display(Name="BIOS Serial Number")]
        public string BIOSSerialNumber { get; set; }

        [Display(Name="BIOS Manufacturer")]
        public string BIOSManufacturer { get; set; }

        [Display(Name="BIOS Release Date")]
        public string BIOSReleaseDate { get; set; }

        [Display(Name="Machine Type")]
        public string MachineType { get; set; }
    }
}
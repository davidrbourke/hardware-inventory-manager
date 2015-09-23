using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using HardwareInventoryManager.Helpers;

namespace HardwareInventoryManager.Models
{
    public class Setting: ModelEntity
    {
        [Key]
        public int SettingId { get; set; }

        public string Key { get; set; }

        public EnumHelper.AppSettingDataType DataType { get; set; }

        public EnumHelper.ApplicationSettingType SettingType { get; set; }
    }
}
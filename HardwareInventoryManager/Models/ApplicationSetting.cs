﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HardwareInventoryManager.Models
{
    public class ApplicationSetting: ModelEntity, ITenant
    {
        [Key]
        public int ApplicationSettingId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public Helpers.EnumHelper.AppSettingDataType DataType { get; set; }

        public Helpers.EnumHelper.AppSettingScopeType ScopeType { get; set; }

        public int TenantId { get; set;}

        [NotMapped]
        public bool BoolValue
        {
            get
            {
                bool convertedValue;
                if(bool.TryParse(Value, out convertedValue))
                {
                    return convertedValue;
                }
                return false;
            }
            set
            {
                Value = value ? "true" : "false";
            }
        }
    }
}
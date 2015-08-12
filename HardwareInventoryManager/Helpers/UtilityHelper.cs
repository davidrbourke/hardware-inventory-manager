using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HardwareInventoryManager.Helpers
{
    public class UtilityHelper
    {
        /// <summary>
        /// Generates a temporary password to create new user accounts
        /// </summary>
        /// <returns></returns>
        public string GeneratePassword()
        {
            Random rand = new Random();
            byte[] generatedBytes = new byte[8];
            for(int i = 0; i < 3; i++)
            {
                int j = rand.Next(65, 95);
                generatedBytes[i] = (byte)j;
            }
            for (int i = 3; i < 6; i++)
            {
                int j = rand.Next(97, 122);
                generatedBytes[i] = (byte)j;
            }
            for (int i = 6; i < 7; i++)
            {
                int j = rand.Next(48, 57);
                generatedBytes[i] = (byte)j;
            }
            for (int i = 7; i < 8; i++)
            {
                int j = rand.Next(33, 47);
                generatedBytes[i] = (byte)j;
            }
            return Encoding.ASCII.GetString(generatedBytes);
        }
    }
}
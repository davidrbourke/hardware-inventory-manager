using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HardwareInventoryManager.Services
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

        public static string EncodeUrlCode(string code)
        {
            return HttpUtility.UrlEncode(code);
        }

        public static string DecodeUrlCode(string code)
        {
            return HttpUtility.UrlDecode(code);
        }

        public static MvcHtmlString GoogleAnalytics()
        {
            #if !DEBUG
            return new MvcHtmlString(@"  
                <script>
                    (function (i, s, o, g, r, a, m) {
                        i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                            (i[r].q = i[r].q || []).push(arguments)
                        }, i[r].l = 1 * new Date(); a = s.createElement(o),
                        m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
                    })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

                    ga('create', 'UA-67842273-1', 'auto');
                    ga('send', 'pageview');

                </script>");

            #else
                return new MvcHtmlString(string.Empty);
            #endif


        }
    }
}
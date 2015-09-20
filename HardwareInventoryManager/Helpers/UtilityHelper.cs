using HardwareInventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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

        /// <summary>
        /// Returns javascript for google analytics in non-debug mode
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns true if the captcha is successfully validated by google
        /// </summary>
        /// <param name="captchaCode"></param>
        /// <param name="userIpAddress"></param>
        /// <returns></returns>
        public static bool IsGoogleReCaptchaValid(string captchaCode, string userIpAddress)
        {
            try
            {
                WebClient client = new WebClient();
                NameValueCollection collection = new NameValueCollection()
            {
                {"secret", "6LdpLA0TAAAAALj2bvRJr4OEnpTq8ATFwLxm5_YU"},
                {"response", captchaCode},
                {"remoteip", userIpAddress}
            };

                byte[] resp = client.UploadValues("https://www.google.com/recaptcha/api/siteverify", collection);
                string output = System.Text.Encoding.UTF8.GetString(resp);

                //JObject o = JObject.Parse(output);
                JavaScriptSerializer jsonSerialiser = new JavaScriptSerializer();
                CaptchaResponse response = jsonSerialiser.Deserialize<CaptchaResponse>(output);

                return response.success == "True" ? true : false;
            }
            catch(Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return false;
            }
        }
    }
}
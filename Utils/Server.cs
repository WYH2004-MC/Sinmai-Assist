using System;
using System.Collections.Generic;
using MAI2.Util;
using Manager;

namespace Utils
{
    public class Server
    {
        private static readonly Dictionary<string, string> TitleServerMappings = new Dictionary<string, string>
        {
            { "maimai-gm.wahlap.com", "CN Wahlap Official" },
            { "aquadx.hydev.org", "AquaDX Network" },
            { "naominet.live", "RinNET" },
            { "msm.moe", "Msm Aqua" }
        };

        public static string GetTitleServerUri()
        {
            try
            {
                string uri = Singleton<OperationManager>.Instance.GetBaseUri();

                foreach (var mapping in TitleServerMappings)
                {
                    if (uri.Contains(mapping.Key))
                    {
                        return mapping.Value;
                    }
                }

                return uri;
            }
            catch (Exception e) when (e is NullReferenceException || e is InvalidOperationException)
            {
                return "Unknown";
            }
        }
    }
}

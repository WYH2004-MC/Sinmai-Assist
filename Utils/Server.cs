using MAI2.Util;
using Manager;

namespace Utils
{
    public class Server
    {
        public static string GetTitleServerUri()
        {
            try
            {
                string uri = Singleton<OperationManager>.Instance.GetBaseUri();
                if (uri.Contains("maimai-gm.wahlap.com")) { return "CN Wahlap Official"; }
                if (uri.Contains("aquadx.hydev.org")) { return "AquaDX Network"; }
                return uri;
            }catch
            {
                return "Unknown";
            }
        }
    }
}

using MAI2.Util;
using Manager;

namespace Utils
{
    public class Server
    {
        public static string GetTitleServerUri()
        {
            string uri = Singleton<OperationManager>.Instance.GetBaseUri();
            if (uri.Contains("maimai-gm.wahlap.com")) { return "CN Wahlap Official"; }
            return uri;
        }
    }
}

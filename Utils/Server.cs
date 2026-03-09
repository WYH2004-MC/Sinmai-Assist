using System;
using MAI2.Util;
using Manager;

namespace SinmaiAssist.Utils
{
    public class Server
    {
        public static string GetTitleServerUri()
        {
            try
            {
                string title = Singleton<OperationManager>.Instance.GetBaseUri();
                if (string.IsNullOrEmpty(title)) return "Unknown";
                return SinmaiAssist.MainConfig.ModSetting.MaskTitleServerUrl ? MaskServerUri(title) : title;
            }
            catch (Exception e) when (e is NullReferenceException || e is InvalidOperationException || e is UriFormatException)
            {
                return "Unknown";
            }
        }

        private static string MaskServerUri(string uriText)
        {
            if (!Uri.TryCreate(uriText, UriKind.Absolute, out Uri uri))
            {
                return MaskHost(uriText);
            }

            string maskedHost = MaskHost(uri.Host);
            string hostForDisplay = uri.HostNameType == UriHostNameType.IPv6 ? $"[{maskedHost}]" : maskedHost;
            string userInfo = string.IsNullOrEmpty(uri.UserInfo) ? string.Empty : uri.UserInfo + "@";
            string port = uri.IsDefaultPort ? string.Empty : ":" + uri.Port;
            return $"{uri.Scheme}://{userInfo}{hostForDisplay}{port}{uri.PathAndQuery}{uri.Fragment}";
        }

        private static string MaskHost(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                return host;
            }

            string[] segments = host.Split('.');
            int lastIndex = segments.Length - 1;
            for (int i = 0; i < segments.Length; i++)
            {
                if (i == lastIndex)
                {
                    continue;
                }
                segments[i] = MaskSegment(segments[i]);
            }
            return string.Join(".", segments);
        }

        private static string MaskSegment(string segment)
        {
            if (string.IsNullOrEmpty(segment))
            {
                return segment;
            }

            if (segment.Length <= 2)
            {
                return new string('*', segment.Length);
            }

            return segment[0] + new string('*', segment.Length - 2) + segment[segment.Length - 1];
        }
    }
}

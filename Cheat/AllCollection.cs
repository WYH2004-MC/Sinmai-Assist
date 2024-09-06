using HarmonyLib;
using MAI2.Util;
using Manager;
using Manager.MaiStudio;
using Manager.UserDatas;
using System.Collections.Generic;
using System.Linq;

namespace Cheat
{
    public class AllCollection
    {
        private enum CollectionType
        {
            Frame,
            Icon,
            Plate,
            Partner,
            Title,
            None
        }
        private static CollectionType collectionType = CollectionType.None;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CollectionProcess), "CreateFrameData")]
        public static void CreateFrameDataPrefix() { collectionType = CollectionType.Frame; }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CollectionProcess), "CreateFrameData")]
        public static void CreateFrameDataPostfix() { collectionType = CollectionType.None; }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CollectionProcess), "CreateIconData")]
        public static void CreateIconDataPrefix() { collectionType = CollectionType.Icon; }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CollectionProcess), "CreateIconData")]
        public static void CreateIconDataPostfix() { collectionType = CollectionType.None; }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CollectionProcess), "CreatePlateData")]
        public static void CreatePlateDataPrefix() { collectionType = CollectionType.Plate; }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CollectionProcess), "CreatePlateData")]
        public static void CreatePlateDataPostfix() { collectionType = CollectionType.None; }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CollectionProcess), "CreatePartnerData")]
        public static void CreatePartnerDataPrefix() { collectionType = CollectionType.Partner; }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CollectionProcess), "CreatePartnerData")]
        public static void CreatePartnerDataPostfix() { collectionType = CollectionType.None; }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(CollectionProcess), "CreateTitleData")]
        public static void CreateTitleDataPrefix() { collectionType = CollectionType.Title; }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CollectionProcess), "CreateTitleData")]
        public static void CreateTitleDataPostfix() { collectionType = CollectionType.None; }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UserData), "get_FrameList")]
        public static void FrameList(ref List<UserItem> __result, CollectionProcess __instance)
        {
            if (collectionType == CollectionType.Frame)
            {
                List<int> list2 = (from i in __result
                                   where i.stock > 0
                                   select i.itemId).ToList();

                foreach (KeyValuePair<int, FrameData> frame2 in Singleton<DataManager>.Instance.GetFrames())
                {
                    if (!list2.Contains(frame2.Value.GetID()))
                    {
                        list2.Add(frame2.Value.GetID());
                        __result.Add(new UserItem(frame2.Value.GetID()));
                    }
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UserData), "get_IconList")]
        public static void IconList(ref List<UserItem> __result, CollectionProcess __instance)
        {
            if (collectionType == CollectionType.Icon)
            {
                List<int> list2 = (from i in __result
                                   where i.stock > 0
                                   select i.itemId).ToList();

                foreach (KeyValuePair<int, IconData> icon2 in Singleton<DataManager>.Instance.GetIcons())
                {
                    if (!list2.Contains(icon2.Value.GetID()))
                    {
                        list2.Add(icon2.Value.GetID());
                        __result.Add(new UserItem(icon2.Value.GetID()));
                    }
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UserData), "get_PlateList")]
        public static void PlateList(ref List<UserItem> __result, CollectionProcess __instance)
        {
            if (collectionType == CollectionType.Plate)
            {
                List<int> list2 = (from i in __result
                                   where i.stock > 0
                                   select i.itemId).ToList();

                foreach (KeyValuePair<int, PlateData> plate2 in Singleton<DataManager>.Instance.GetPlates())
                {
                    if (!list2.Contains(plate2.Value.GetID()))
                    {
                        list2.Add(plate2.Value.GetID());
                        __result.Add(new UserItem(plate2.Value.GetID()));
                    }
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UserData), "get_PartnerList")]
        public static void PartnerList(ref List<UserItem> __result, CollectionProcess __instance)
        {
            if (collectionType == CollectionType.Partner)
            {
                List<int> list2 = (from i in __result
                                   where i.stock > 0
                                   select i.itemId).ToList();

                foreach (KeyValuePair<int, PartnerData> partner2 in Singleton<DataManager>.Instance.GetPartners())
                {
                    if (!list2.Contains(partner2.Value.GetID()))
                    {
                        list2.Add(partner2.Value.GetID());
                        __result.Add(new UserItem(partner2.Value.GetID()));
                    }
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UserData), "get_TitleList")]
        public static void TitleList(ref List<UserItem> __result, CollectionProcess __instance)
        {
            if (collectionType == CollectionType.Title)
            {
                List<int> list2 = (from i in __result
                                   where i.stock > 0
                                   select i.itemId).ToList();

                foreach (KeyValuePair<int, TitleData> title2 in Singleton<DataManager>.Instance.GetTitles())
                {
                    if (!list2.Contains(title2.Value.GetID()))
                    {
                        list2.Add(title2.Value.GetID());
                        __result.Add(new UserItem(title2.Value.GetID()));
                    }
                }
            }
        }
    }
}

using HarmonyLib;
using MAI2.Util;
using Manager;
using Monitor;
using System;
using System.Collections.ObjectModel;
using static NoteJudge;
using MelonLoader;

namespace SinmaiAssist.Cheat
{
    public class AutoPlay
    {
        public enum AutoPlayMode
        {
            None,
            Critical,
            Perfect,
            Great,
            Good,
            Random,
            RandomAllPerfect,
            RandomFullComboPlus,
            RandomFullCombo
        }

        private static readonly ReadOnlyCollection<NoteJudge.ETiming> RandCriticalTiming = Array.AsReadOnly(new NoteJudge.ETiming[5]
        {
            NoteJudge.ETiming.FastPerfect2nd,
            NoteJudge.ETiming.FastPerfect,
            NoteJudge.ETiming.Critical,
            NoteJudge.ETiming.LatePerfect,
            NoteJudge.ETiming.LatePerfect2nd
        });

        private static readonly ReadOnlyCollection<NoteJudge.ETiming> RandomJudgeTiming = Array.AsReadOnly(new NoteJudge.ETiming[]
        {
            NoteJudge.ETiming.Critical,
            NoteJudge.ETiming.FastPerfect,
            NoteJudge.ETiming.FastPerfect2nd,
            NoteJudge.ETiming.LatePerfect,
            NoteJudge.ETiming.LatePerfect2nd,
            NoteJudge.ETiming.LateGreat2nd,
            NoteJudge.ETiming.FastGreat2nd,
            NoteJudge.ETiming.LateGood,
            NoteJudge.ETiming.FastGood,
            NoteJudge.ETiming.TooLate
        });


        public static AutoPlayMode autoPlayMode = AutoPlayMode.None;
        public static bool DisableUpdate = false;

        public static bool IsAutoPlay()
        {
            return autoPlayMode != AutoPlayMode.None;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameManager), "IsAutoPlay")]
        public static bool SetIsAutoPlay(ref bool __result)
        {
            __result = IsAutoPlay();
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameManager), "set_AutoPlay")]
        public static void AutoPlayUpdate()
        {
            var mode = GameManager.AutoPlay;
            if (DisableUpdate) return;
            autoPlayMode = (AutoPlayMode)mode;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameManager), "AutoJudge")]
        public static void AutoJudge(ref NoteJudge.ETiming __result)
        {
            var random = UnityEngine.Random.Range(0, 1000);
            var random2 = 2;
            if (random < 350)
            {
                random2 += UnityEngine.Random.Range(-2, 3);
            }

            switch (autoPlayMode)
            {
                case AutoPlayMode.Critical:
                    __result = NoteJudge.ETiming.Critical;
                    break;
                case AutoPlayMode.Perfect:
                    __result = NoteJudge.ETiming.LatePerfect2nd;
                    break;
                case AutoPlayMode.Great:
                    __result = NoteJudge.ETiming.LateGreat;
                    break;
                case AutoPlayMode.Good:
                    __result = NoteJudge.ETiming.LateGood;
                    break;
                case AutoPlayMode.Random:
                    __result = RandomJudgeTiming[UnityEngine.Random.Range(0, RandomJudgeTiming.Count)];
                    break;
                case AutoPlayMode.RandomAllPerfect:
                    __result = RandCriticalTiming[random2];
                    break;
                case AutoPlayMode.RandomFullComboPlus:
                    if (random >= 10)
                    {
                        __result = RandCriticalTiming[random2];
                    }
                    else
                    {
                        if (UnityEngine.Random.Range(0, 2) == 1)
                        {
                            __result = NoteJudge.ETiming.LateGreat;
                        }
                        else
                        {
                            __result = NoteJudge.ETiming.FastGreat;
                        }
                    }
                    break;
                case AutoPlayMode.RandomFullCombo:
                    if (random >= 80)
                    {
                        __result = RandCriticalTiming[random2];
                    }
                    else if (random >= 20)
                    {
                        if (UnityEngine.Random.Range(0, 2) == 1)
                        {
                            __result = NoteJudge.ETiming.LateGreat;
                        }
                        else
                        {
                            __result = NoteJudge.ETiming.FastGreat;
                        }
                    }
                    else
                    {
                        if (UnityEngine.Random.Range(0, 2) == 1)
                        {
                            __result = NoteJudge.ETiming.LateGood;
                        }
                        else
                        {
                            __result = NoteJudge.ETiming.FastGood;
                        }
                    }
                    break;
                default:
                    __result = NoteJudge.ETiming.TooFast;
                    break;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(NoteBase), "SetAutoPlayJudge")]
        public static bool NoteBaseAutoPlayJudge(NoteBase __instance)
        {
            if (!IsAutoPlay()) return true;
            var appearMsec = (float)AccessTools.Field(typeof(NoteBase), "AppearMsec").GetValue(__instance);
            var isExNote = (bool)AccessTools.Field(typeof(NoteBase), "IsExNote").GetValue(__instance);
            var playJudgeSeMethod = AccessTools.Method(typeof(NoteBase), "PlayJudgeSe");

            if (NotesManager.GetCurrentMsec() > appearMsec - 4.1666665f && IsAutoPlay())
            {
                if ((autoPlayMode == AutoPlayMode.RandomAllPerfect || autoPlayMode == AutoPlayMode.RandomFullCombo || autoPlayMode == AutoPlayMode.RandomFullComboPlus) && isExNote)
                {
                    AccessTools.Field(typeof(NoteBase), "JudgeResult").SetValue(__instance, NoteJudge.ETiming.Critical);
                }
                else
                {
                    AccessTools.Field(typeof(Monitor.NoteBase), "JudgeResult").SetValue(__instance, GameManager.AutoJudge());
                }
                playJudgeSeMethod.Invoke(__instance, null);
            }
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SlideRoot), "Judge")]
        public static bool SlideRootJudge(SlideRoot __instance, ref bool __result)
        {
            if (!IsAutoPlay()) return true;
            var isNoteCheckTimeStartMethod = AccessTools.Method(typeof(SlideRoot), "IsNoteCheckTimeStart");
            bool isNoteCheckTimeStart = (bool)isNoteCheckTimeStartMethod.Invoke(__instance, new object[] { Singleton<GamePlayManager>.Instance.GetGameScore(__instance.MonitorId).UserOption.GetJudgeTimingFrame() });
            if (!isNoteCheckTimeStart)
            {
                __result = false;
                return false;
            }

            var judgeTimingDiffMsecField = AccessTools.Field(typeof(SlideRoot), "JudgeTimingDiffMsec");
            var judgeResultField = AccessTools.Field(typeof(SlideRoot), "JudgeResult");
            var tailMsecField = AccessTools.Field(typeof(SlideRoot), "TailMsec");
            var lastWaitTimeField = AccessTools.Field(typeof(SlideRoot), "lastWaitTime");
            var judgeTypeField = AccessTools.Field(typeof(SlideRoot), "JudgeType");
            var lastWaitTimeForJudgeField = AccessTools.Field(typeof(SlideRoot), "lastWaitTimeForJudge");

            float judgeTimingDiffMsec = NotesManager.GetCurrentMsec() - (float)tailMsecField.GetValue(__instance) + (float)lastWaitTimeField.GetValue(__instance);
            judgeTimingDiffMsecField.SetValue(__instance, judgeTimingDiffMsec);
            ETiming judgeResult = (ETiming)judgeResultField.GetValue(__instance);
            judgeResult = (IsAutoPlay() ? GameManager.AutoJudge() : NoteJudge.GetSlideJudgeTiming(ref judgeTimingDiffMsec, Singleton<GamePlayManager>.Instance.GetGameScore(__instance.MonitorId).UserOption.GetJudgeTimingFrame(), (EJudgeType)judgeTypeField.GetValue(__instance), (int)lastWaitTimeForJudgeField.GetValue(__instance)));

            if (autoPlayMode == AutoPlayMode.RandomAllPerfect ||
                autoPlayMode == AutoPlayMode.RandomFullComboPlus ||
                autoPlayMode == AutoPlayMode.RandomFullCombo)
            {
                judgeResult = NoteJudge.ETiming.Critical;
            }

            if (judgeResult != NoteJudge.ETiming.End)
            {
                if (judgeResult == NoteJudge.ETiming.TooFast)
                {
                    judgeResult = NoteJudge.ETiming.FastGood;
                }
                judgeResultField.SetValue(__instance, judgeResult);
                __result = true;
                return false;
            }
            __result = false;
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TouchNoteB), "Judge")]
        public static bool TouchNoteBJudge(TouchNoteB __instance, ref bool __result)
        {
            if (!IsAutoPlay()) return true;
            var isNoteCheckTimeStartMethod = AccessTools.Method(typeof(NoteBase), "IsNoteCheckTimeStart");
            bool isNoteCheckTimeStart = (bool)isNoteCheckTimeStartMethod.Invoke(__instance, new object[] { Singleton<GamePlayManager>.Instance.GetGameScore(__instance.MonitorId).UserOption.GetJudgeTimingFrame() });
            if (isNoteCheckTimeStart)
            {
                __result = false;
                return false;
            }
            var judgeTimingDiffMsecField = AccessTools.Field(typeof(NoteBase), "JudgeTimingDiffMsec");
            var judgeResultField = AccessTools.Field(typeof(NoteBase), "JudgeResult");
            var appearMsecField = AccessTools.Field(typeof(NoteBase), "AppearMsec");
            var judgeTypeField = AccessTools.Field(typeof(NoteBase), "JudgeType");
            var buttonIdField = AccessTools.Field(typeof(NoteBase), "ButtonId");
            var TouchAreaField = AccessTools.Field(typeof(TouchNoteB), "TouchArea");
            var playJudgeSeMethod = AccessTools.Method(typeof(TouchNoteB), "PlayJudgeSe");

            var judgeTimingDiffMsec = NotesManager.GetCurrentMsec() - (float)appearMsecField.GetValue(__instance);
            judgeTimingDiffMsecField.SetValue(__instance, judgeTimingDiffMsec);

            ETiming judgeResult = (ETiming)judgeResultField.GetValue(__instance);
            judgeResult = NoteJudge.GetJudgeTiming(ref judgeTimingDiffMsec, Singleton<GamePlayManager>.Instance.GetGameScore(__instance.MonitorId).UserOption.GetJudgeTimingFrame(), (EJudgeType)judgeTypeField.GetValue(__instance));
            if (autoPlayMode == AutoPlayMode.RandomAllPerfect ||
                autoPlayMode == AutoPlayMode.RandomFullComboPlus ||
                autoPlayMode == AutoPlayMode.RandomFullCombo)
            {
                judgeResult = NoteJudge.ETiming.Critical;
            }
            judgeResultField.SetValue(__instance, judgeResult);

            TouchSensorType touchArea = (TouchSensorType)TouchAreaField.GetValue(__instance);
            if (judgeResult != NoteJudge.ETiming.End)
            {
                playJudgeSeMethod.Invoke(__instance, null);
                int buttonId = (int)buttonIdField.GetValue(__instance);
                if (touchArea == TouchSensorType.B)
                {
                    InputManager.SetUsedThisFrame(__instance.MonitorId, (InputManager.TouchPanelArea)(8 + buttonId));
                }
                else if (touchArea == TouchSensorType.E)
                {
                    InputManager.SetUsedThisFrame(__instance.MonitorId, (InputManager.TouchPanelArea)(26 + buttonId));
                }
                else if (touchArea == TouchSensorType.A)
                {
                    InputManager.SetUsedThisFrame(__instance.MonitorId, (InputManager.TouchPanelArea)(0 + buttonId));
                }
                else if (touchArea == TouchSensorType.D)
                {
                    InputManager.SetUsedThisFrame(__instance.MonitorId, (InputManager.TouchPanelArea)(18 + buttonId));
                }
                else if (touchArea == TouchSensorType.C)
                {
                    InputManager.SetUsedThisFrame(__instance.MonitorId, InputManager.TouchPanelArea.C1);
                    InputManager.SetUsedThisFrame(__instance.MonitorId, InputManager.TouchPanelArea.C2);
                }
                __result = true;
                return false;
            }
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TouchNoteB), "NoteCheck")]
        public static void NoteCheck(TouchNoteB __instance)
        {
            if (!IsAutoPlay()) return;
            var judgeResultField = AccessTools.Field(typeof(NoteBase), "JudgeResult");
            var playJudgeSeMethod = AccessTools.Method(typeof(TouchNoteB), "PlayJudgeSe");
            float appearMsec = (float)AccessTools.Field(typeof(NoteBase), "AppearMsec").GetValue(__instance);
            if ((autoPlayMode == AutoPlayMode.RandomAllPerfect ||
                autoPlayMode == AutoPlayMode.RandomFullComboPlus ||
                autoPlayMode == AutoPlayMode.RandomFullCombo) &&
                NotesManager.GetCurrentMsec() > appearMsec - 4.1666665f &&
                IsAutoPlay())
            {
                judgeResultField.SetValue(__instance, NoteJudge.ETiming.Critical);
                playJudgeSeMethod.Invoke(__instance, null);
            }
        }
    }
}


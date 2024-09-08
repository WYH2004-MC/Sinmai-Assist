using DB;
using HarmonyLib;
using MAI2.Util;
using Manager;
using MelonLoader;
using Monitor;
using Process;
using System;
using System.Reflection;
using UnityEngine;

namespace Cheat
{
    public class ChartTimer
    {
        private enum GameSequence
        {
            Init,
            Sync,
            Start,
            StartWait,
            Play,
            PlayEnd,
            Result,
            ResultEnd,
            FinalWait,
            Release
        }

        public enum Button
        {
            None,
            Pause,
            Reset,
            TimeSkipAdd,
            TimeSkipAdd2,
            TimeSkipAdd3,
            TimeSkipSub,
            TimeSkipSub2,
            TimeSkipSub3
        }

        public static Button ButtonStatus = Button.None;
        public static bool IsPlaying = false;
        public static double Timer = 0.0;
        private static MovieController gameMovie;
        private static NotesManager notesManager;
        private static GameProcess gameProcess;
        private static GameMonitor[] monitors;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameProcess), "OnUpdate")]
        private static void OnUpdate(GameProcess __instance)
        {
            try
            {
                gameProcess = __instance;
                GameSequence sequence = (GameSequence)typeof(GameProcess).GetField("_sequence", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
                monitors = (GameMonitor[])typeof(GameProcess).GetField("_monitors", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gameProcess);
                gameMovie = (MovieController)typeof(GameProcess).GetField("_gameMovie", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
                notesManager = new NotesManager();
                if (!notesManager.IsPlaying())
                {
                    IsPlaying = true;
                    Timer = 0.0;
                }
                if (sequence == GameSequence.Play)
                {
                    var IsPartyPlayMethod = typeof(GameProcess).GetMethod("IsPartyPlay", BindingFlags.NonPublic | BindingFlags.Instance);
                    bool IsPartyPlay = (bool)IsPartyPlayMethod.Invoke(__instance, null);
                    if (IsPlaying)
                    {
                        Timer += GameManager.GetGameMSecAddD();
                    }
                    if (ButtonStatus == Button.Pause || DebugInput.GetKeyDown(KeyCode.Return))
                    {
                        if (IsPlaying)
                        {
                            SoundManager.PauseMusic(true);
                            gameMovie.Pause(true);
                            NotesManager.Pause(true);
                        }
                        else
                        {
                            SoundManager.PauseMusic(false);
                            gameMovie.Pause(false);
                            NotesManager.Pause(false);
                            //debugFeature.DebugTimeSkip(0);
                        }
                        IsPlaying = !IsPlaying;
                    }
                    else if (DebugInput.GetKey(KeyCode.LeftArrow) && !IsPlaying)
                    {
                        Singleton<GamePlayManager>.Instance.Initialize(IsPartyPlay);
                        TimeSkip(-16);
                    }
                    else if (DebugInput.GetKey(KeyCode.RightArrow) && !IsPlaying)
                    {
                        Singleton<GamePlayManager>.Instance.Initialize(IsPartyPlay);
                        TimeSkip(16);
                    }
                    else if (ButtonStatus != Button.None && ButtonStatus != Button.Pause)
                    {
                        Singleton<GamePlayManager>.Instance.Initialize(IsPartyPlay);
                        switch (ButtonStatus)
                        {
                            case Button.TimeSkipAdd:
                                TimeSkip(100);
                                break;
                            case Button.TimeSkipAdd2:
                                TimeSkip(1000);
                                break;
                            case Button.TimeSkipAdd3:
                                TimeSkip(2500);
                                break;
                            case Button.TimeSkipSub:
                                TimeSkip(-100);
                                break;
                            case Button.TimeSkipSub2:
                                TimeSkip(-1000);
                                break;
                            case Button.TimeSkipSub3:
                                TimeSkip(-2500);
                                break;
                            case Button.Reset:
                                TimeSkip(-999999);
                                break;
                            default:
                                break;
                        }
                    }
                }
                ReUpdate();
            }
            catch(Exception e)
            {
                MelonLogger.Error(e);
            }
        }

        private static void TimeSkip(int addMsec)
        {
            gameMovie.Pause(true);
            NotesManager.Pause(true);
            if (addMsec >= 0)
            {
                Timer += addMsec;
            }
            else
            {
                Timer = ((Timer + (double)addMsec >= 0.0) ? (Timer + (double)addMsec) : 0.0);
            }
            gameMovie.SetSeekFrame(Timer);
            SoundManager.SeekMusic((int)Timer);
            for (int i = 0; i < monitors.Length; i++)
            {
                monitors[i].Seek((int)Timer);
            }
            int num = 91;
            NotesManager.StartPlay((int)Timer + num);
            NotesManager.Pause(true);
            if (IsPlaying)
            {
                SoundManager.PauseMusic(pause: false);
                gameMovie.Pause(pauseFlag: false);
                NotesManager.Pause(false);
            }
            else
            {
                gameMovie.Pause(pauseFlag: true);
            }
            gameProcess.UpdateNotes();
        }

        private static void ReUpdate()
        {
            int[] _skipPhase = new int[2] { -1, -1 };
            int[] _retryPhase = new int[2] { -1, -1 };
            System.Type processBaseType = typeof(GameProcess).BaseType;
            var containerField = processBaseType.GetField("container", BindingFlags.NonPublic | BindingFlags.Instance);
            ProcessDataContainer container = (ProcessDataContainer)containerField.GetValue(gameProcess);
            for (int num30 = 0; num30 < monitors.Length; num30++)
            {
                if (Singleton<GamePlayManager>.Instance.GetGameScore(num30).IsTrackSkip || Singleton<GamePlayManager>.Instance.IsQuickRetry())
                {
                    for (int num31 = 0; num31 < 35; num31++)
                    {
                        InputManager.SetUsedThisFrame(num30, (InputManager.TouchPanelArea)num31);
                    }
                }
                monitors[num30].ViewUpdate();
                if (!Singleton<UserDataManager>.Instance.GetUserData(num30).IsEntry)
                {
                    continue;
                }
                if (_skipPhase[num30] != monitors[num30].GetPushPhase())
                {
                    _skipPhase[num30] = monitors[num30].GetPushPhase();
                    switch (_skipPhase[num30])
                    {
                        case -1:
                            container.processManager.ForcedCloseWindow(num30);
                            break;
                        case 0:
                            container.processManager.EnqueueMessage(num30, WindowMessageID.TrackSkip3Second, WindowPositionID.Middle);
                            break;
                        case 1:
                            container.processManager.EnqueueMessage(num30, WindowMessageID.TrackSkip2Second, WindowPositionID.Middle);
                            break;
                        case 2:
                            container.processManager.EnqueueMessage(num30, WindowMessageID.TrackSkip1Second, WindowPositionID.Middle);
                            break;
                        case 3:
                            container.processManager.CloseWindow(num30);
                            break;
                    }
                }
                if (_retryPhase[num30] != monitors[num30].GetPushPhaseRetry())
                {
                    _retryPhase[num30] = monitors[num30].GetPushPhaseRetry();
                    switch (_retryPhase[num30])
                    {
                        case -1:
                            container.processManager.ForcedCloseWindow(num30);
                            break;
                        case 0:
                            container.processManager.EnqueueMessage(num30, WindowMessageID.QuickRetry2Second, WindowPositionID.Middle);
                            break;
                        case 1:
                            container.processManager.EnqueueMessage(num30, WindowMessageID.QuickRetry1Second, WindowPositionID.Middle);
                            break;
                        case 2:
                            container.processManager.CloseWindow(num30);
                            break;
                    }
                }
            }
            Singleton<GamePlayManager>.Instance.PlayLastUpdate();
        }
    }
}

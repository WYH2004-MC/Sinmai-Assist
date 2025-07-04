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

namespace SinmaiAssist.Cheat;

public class ChartController
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
        TimeSkipSub3,
        Set,
        Back
    }

    public static Button ButtonStatus = Button.None;
    public static bool IsPlaying = false;
    public static double Timer = 0.0;
    public static int RecordTime = 0;
    private static MovieController _gameMovie;
    private static NotesManager _notesManager;
    private static GameProcess _gameProcess;
    private static GameMonitor[] _monitors;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameProcess), "OnUpdate")]
    private static void OnUpdate(GameProcess __instance)
    {
        try
        {
            _gameProcess = __instance;
            GameSequence sequence = (GameSequence)typeof(GameProcess).GetField("_sequence", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
            _monitors = (GameMonitor[])typeof(GameProcess).GetField("_monitors", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_gameProcess);
            _gameMovie = (MovieController)typeof(GameProcess).GetField("_gameMovie", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
            _notesManager = new NotesManager();
            if (!_notesManager.IsPlaying())
            {
                IsPlaying = true;
                Timer = 0.0;
                RecordTime = 0;
            }
            if (sequence == GameSequence.Play)
            {
                var IsPartyPlayMethod = typeof(GameProcess).GetMethod("IsPartyPlay", BindingFlags.NonPublic | BindingFlags.Instance);
                bool IsPartyPlay = (bool)IsPartyPlayMethod.Invoke(__instance, null);
                if (IsPlaying)
                {
                    Timer += GameManager.GetGameMSecAddD();
                }
                if (ButtonStatus == Button.Pause || DebugInput.GetKeyDown(SinmaiAssist.KeyBindConfig.ChartController.Pause.KeyCode))
                {
                    if (IsPlaying)
                    {
                        SoundManager.PauseMusic(true);
                        GameMoviePause(true);
                        NotesManager.Pause(true);
                    }
                    else
                    {
                        SoundManager.PauseMusic(false);
                        GameMoviePause(false);
                        NotesManager.Pause(false);
                        //TimeSkip(0);
                    }
                    IsPlaying = !IsPlaying;
                }
                else if (DebugInput.GetKey(SinmaiAssist.KeyBindConfig.ChartController.Backward.KeyCode) && !IsPlaying)
                {
                    Singleton<GamePlayManager>.Instance.Initialize(IsPartyPlay);
                    TimeSkip(-16);
                }
                else if (DebugInput.GetKey(SinmaiAssist.KeyBindConfig.ChartController.Forward.KeyCode) && !IsPlaying)
                {
                    Singleton<GamePlayManager>.Instance.Initialize(IsPartyPlay);
                    TimeSkip(16);
                }
                else if (DebugInput.GetKeyDown(SinmaiAssist.KeyBindConfig.ChartController.SetRecord.KeyCode) || ButtonStatus == Button.Set)
                {
                    RecordTime = (int)Timer;
                    MelonLogger.Msg($"Record Time: {RecordTime}");
                }
                else if (DebugInput.GetKeyDown(SinmaiAssist.KeyBindConfig.ChartController.ReturnRecord.KeyCode) || ButtonStatus == Button.Back)
                {
                    int time = RecordTime == 0 ? 999999 : (int)Timer - RecordTime;
                    TimeSkip(-time);
                    TimeSkip(0);
                    MelonLogger.Msg($"Time Jump: {RecordTime}({-time})");
                }
                else if (ButtonStatus != Button.None)
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
                            Singleton<GamePlayManager>.Instance.SetQuickRetryFrag(flag: true);
                            break;
                        default:
                            break;
                    }
                }
            }
            ReUpdate();
        }
        catch (Exception e)
        {
            MelonLogger.Error(e);
        }
    }

    private static void TimeSkip(int addMsec)
    {
        GameMoviePause(true);
        NotesManager.Pause(true);
        if (addMsec >= 0)
        {
            Timer += addMsec;
        }
        else
        {
            Timer = ((Timer + (double)addMsec >= 0.0) ? (Timer + (double)addMsec) : 0.0);
        }
        GameMovieSetSeekFrame(Timer);
        SoundManager.SeekMusic((int)Timer);
        for (int i = 0; i < _monitors.Length; i++)
        {
            _monitors[i].Seek((int)Timer);
        }
        int num = 91;
        NotesManager.StartPlay((int)Timer + num);
        NotesManager.Pause(true);
        if (IsPlaying)
        {
            SoundManager.PauseMusic(pause: false);
            GameMoviePause(false);
            NotesManager.Pause(false);
        }
        else
        {
            GameMoviePause(true);
        }
        _gameProcess.UpdateNotes();
    }

    private static void ReUpdate()
    {
        int[] _skipPhase = new int[2] { -1, -1 };
        int[] _retryPhase = new int[2] { -1, -1 };
        System.Type processBaseType = typeof(GameProcess).BaseType;
        var containerField = processBaseType.GetField("container", BindingFlags.NonPublic | BindingFlags.Instance);
        ProcessDataContainer container = (ProcessDataContainer)containerField.GetValue(_gameProcess);
        for (int num30 = 0; num30 < _monitors.Length; num30++)
        {
            if (Singleton<GamePlayManager>.Instance.GetGameScore(num30).IsTrackSkip || Singleton<GamePlayManager>.Instance.IsQuickRetry())
            {
                for (int num31 = 0; num31 < 35; num31++)
                {
                    InputManager.SetUsedThisFrame(num30, (InputManager.TouchPanelArea)num31);
                }
            }
            _monitors[num30].ViewUpdate();
            if (!Singleton<UserDataManager>.Instance.GetUserData(num30).IsEntry)
            {
                continue;
            }
            if (_skipPhase[num30] != _monitors[num30].GetPushPhase())
            {
                _skipPhase[num30] = _monitors[num30].GetPushPhase();
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
            if (_retryPhase[num30] != _monitors[num30].GetPushPhaseRetry())
            {
                _retryPhase[num30] = _monitors[num30].GetPushPhaseRetry();
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
    
    //修复1.55后 因SBGA程序员闲的没事觉得自己不写点东西就要被优化了,灵机一动给GameMovie类下的方法加上屏幕号 导致Mod调用旧方法时让你最新最热原地爆炸的问题
    private static void GameMoviePause(bool pause)
    {
        var method = _gameMovie.GetType().GetMethod("Pause");
        if (method.GetParameters().Length == 1)
        {
            method.Invoke(_gameMovie, [pause]);
        }
        else
        {
            method.Invoke(_gameMovie, [0, pause]);
            method.Invoke(_gameMovie, [1, pause]);
        }
    }
    
    private static void GameMovieSetSeekFrame(double msec)
    {
        var method = _gameMovie.GetType().GetMethod("SetSeekFrame");
        if (method.GetParameters().Length == 1)
        {
            method.Invoke(_gameMovie, [msec]);
        }
        else
        {
            method.Invoke(_gameMovie, [0, msec]);
            method.Invoke(_gameMovie, [1, msec]);
        }
    }
}
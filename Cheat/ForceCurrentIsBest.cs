using HarmonyLib;
using Manager;
using Manager.UserDatas;
using MelonLoader;
using Process;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using DB;
using Game;
using Datas;
using MAI2.Util;
using Manager.MaiStudio;
using Monitor;

namespace SinmaiAssist.Cheat
{
    public class ForceCurrentIsBest
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ResultProcess), "OnStart")]

        public static void Postfix(ResultProcess __instance)
        {
            try
            {
                var userDataField = AccessTools.Field(typeof(ResultProcess), "_userData");
                var userScoresField = AccessTools.Field(typeof(ResultProcess), "_userScores");
                var musicIDField = AccessTools.Field(typeof(ResultProcess), "_musicID");
                var isNewRecordField = AccessTools.Field(typeof(ResultProcess), "_isNewRecord");
                var monitorsField = AccessTools.Field(typeof(ResultProcess), "_monitors");
                var userData = (UserData[])userDataField.GetValue(__instance);
                var userScores = (UserScore[])userScoresField.GetValue(__instance);
                int musicID = (int)(musicIDField.GetValue(__instance));
                var isNewRecord = (bool[])(isNewRecordField.GetValue(__instance));
                var monitors = (ResultMonitor[])(monitorsField.GetValue(__instance));

                MusicData music = Singleton<DataManager>.Instance.GetMusic(musicID);
                if (music == null) return;

                for (int playerIndex = 0; playerIndex < userData.Length; playerIndex++)
                {
                    if (userData[playerIndex] == null) continue;

                    int difficulty = GameManager.SelectDifficultyID[playerIndex];

                    if (userData[playerIndex].ScoreDic[difficulty].TryGetValue(musicID, out UserScore historyScore))
                    {
                        bool isDoublePlay = music.utagePlayStyle == UtagePlayStyle.DoublePlayerScore;
                        uint oldAchivement = historyScore.achivement;
                        uint oldDeluxscore = historyScore.deluxscore;

                        historyScore.achivement = userScores[playerIndex].achivement;
                        historyScore.combo = userScores[playerIndex].combo;
                        historyScore.sync = userScores[playerIndex].sync;
                        historyScore.deluxscore = userScores[playerIndex].deluxscore;
                        historyScore.scoreRank = GameManager.GetClearRank(
                            (int)historyScore.achivement,
                            isDoublePlay
                        );

                        int theoreticalValue = isDoublePlay ? 2020000 : 1010000;
                        if (userScores[playerIndex].achivement >= theoreticalValue)
                        {
                            historyScore.extNum1++;
                        }
                        else
                        {
                            historyScore.extNum1 = 0;//Maybe not necessary
                        }

                        isNewRecord[playerIndex] = true;
                        int dxFluctuation = (int)historyScore.deluxscore - (int)oldDeluxscore;
                        int totalNotes = music.notesData[difficulty].maxNotes * 3;
                        int percent = totalNotes > 0 ?
                            (int)(historyScore.deluxscore * 100) / totalNotes : 0;
                        DeluxcorerankrateID dxRank = GameManager.GetDeluxcoreRank(percent);

                        monitors[playerIndex].SetDxScore(
                            historyScore.deluxscore,
                            dxFluctuation,
                            totalNotes,
                            dxRank
                        );
                        monitors[playerIndex].SetMyBestAchievement(
                            oldAchivement,
                            historyScore.achivement - oldAchivement,
                            true
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Error($"ForceCurrentIsBestMoudleError: {ex.Message}");
            }
        }
    } 
}
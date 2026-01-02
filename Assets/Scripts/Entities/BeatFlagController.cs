using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

namespace Assets.Scripts
{
    public class BeatFlagController
    {
        /// <summary>
        /// Contains whole batch of beat flags with timePoint and armAround
        /// </summary>
        public List<BeatFlagItem> BeatFlags { get; set; }
        public static string FlagFilePath { get; set; }

        public BeatFlagController(string flagFilePath)
        {
            FlagFilePath = flagFilePath;

            BeatFlags = ReadAllFlags();
        }

        /// <summary>
        /// By certain time point (seconds), returns respective flag index
        /// </summary>
        /// <param name="timeFlag"></param>
        /// <returns></returns>
        public int GetFlagIndex(float timeFlag)
        {
            for (int i = 0; i < BeatFlags.Count; i++)
            {
                bool isTimeIntersects = timeFlag >= BeatFlags[i].TimePoint - BeatFlags[i].ArmAround && timeFlag <= BeatFlags[i].TimePoint + BeatFlags[i].ArmAround;
                if (isTimeIntersects)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Checks whether timeFlag for time point (seconds) exists or not.
        /// Refers to GetFlagIndex(float).
        /// </summary>
        /// <param name="timeFlag"></param>
        /// <returns></returns>
        public bool IsFlagExists(float timeFlag)
        {
            return GetFlagIndex(timeFlag) != -1;
        }

        /// <summary>
        /// Checks for available existing flags at the timepoint.
        /// Enables to hide current obtained flag immediately.
        /// </summary>
        /// <param name="timeFlag"></param>
        /// <returns></returns>
        public bool IsUntakenFlagExists(float timeFlag)
        {
            return IsFlagExists(timeFlag) && !IsFlagTaken(timeFlag);
        }

        /// <summary>
        /// Sets flag, including certain time point (seconds), as taken.
        /// Refers to GetFlagIndex(float).
        /// </summary>
        /// <param name="timeFlag"></param>
        /// <returns></returns>
        public bool SetFlagTaken(float timeFlag)
        {
            int flagIndex = GetFlagIndex(timeFlag);
            if (flagIndex != -1)
            {
                BeatFlags[flagIndex].IsTaken = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks whether or not certain time point (seconds) is taken.
        /// Refers to GetFlagIndex(float).
        /// </summary>
        /// <param name="timeFlag"></param>
        /// <returns></returns>
        public bool IsFlagTaken(float timeFlag)
        {
            int flagIndex = GetFlagIndex(timeFlag);
            if (flagIndex == -1)
                return true;
            return BeatFlags[flagIndex].IsTaken;
        }

        /// <summary>
        /// Seconds left to take matching flag
        /// </summary>
        /// <param name="timeFlag"></param>
        /// <returns></returns>
        public float TimeLeftThisSpan(float timeFlag)
        {
            float timeLeft = -1;
            if (IsFlagExists(timeFlag))
            {
                // Маємо індекс флагу
                int flagIndex = GetFlagIndex(timeFlag);

                // Обчислюємо час від поточної точки до кінця флагу
                float finalTime = BeatFlags[flagIndex].TimePoint + BeatFlags[flagIndex].ArmAround;

                timeLeft = finalTime - timeFlag;
                return timeLeft;
            }
            return timeLeft;
        }

        /// <summary>
        /// Checks whether or not all flags have been taken
        /// </summary>
        /// <returns></returns>
        public bool IsWinAchieved()
        {
            foreach (BeatFlagItem flag in BeatFlags)
                if (!flag.IsTaken)
                    return false;
            return true;
        }

        /// <summary>
        /// Checks whether or not some flag has not been taken, and timepoint already passed it
        /// </summary>
        /// <param name="timePoint"></param>
        /// <returns></returns>
        public bool IsLoseAchieved(float timePoint)
        {
            foreach (BeatFlagItem flag in BeatFlags)
                if (!flag.IsTaken && timePoint > flag.TimePoint + flag.ArmAround)
                    return true;
            return false;
        }

        /// <summary>
        /// IsWinAchieved() or IsLoseAchieved()
        /// </summary>
        /// <param name="timePoint"></param>
        /// <returns>Is it an endgame?</returns>
        public bool IsGameEnded(float timePoint)
        {
            return IsWinAchieved() || IsLoseAchieved(timePoint);
        }

        #region FLAG CRUD
        public static bool WriteAllTimeFlags(List<BeatFlagItem> flags)
        {
            if (!File.Exists(FlagFilePath))
                File.Create(FlagFilePath).Close();

            if (File.Exists(FlagFilePath))
            {
                List<string> content = new List<string>();
                for (int i = 0; i < flags.Count; i++)
                {
                    string flagLine = JsonUtility.ToJson(flags[i]);
                    content.Add(flagLine);
                }
                File.WriteAllLines(FlagFilePath, content);

                return true;
            }

            return false;
        }

        public static List<BeatFlagItem> ReadAllFlags()
        {
            List<BeatFlagItem> flags = new();

            string[] flagLines = File.ReadAllLines(FlagFilePath);
            foreach (string line in flagLines)
            {
                BeatFlagItem item = JsonUtility.FromJson<BeatFlagItem>(line);
                flags.Add(item);
            }

            return flags;
        }

        public static bool AppendTimeFlag(BeatFlagItem flag)
        {
            if (!File.Exists(FlagFilePath))
                File.Create(FlagFilePath).Close();

            if (File.Exists(FlagFilePath))
            {
                string flagLine = JsonUtility.ToJson(flag);
                File.AppendAllText(FlagFilePath, flagLine + Environment.NewLine);
                return true;
            }
            else
                return false;
        }

        public static bool RemoveFlagsAt(params int[] flagIndexes)
        {
            if (!File.Exists(FlagFilePath))
                File.Create(FlagFilePath).Close();

            if (File.Exists(FlagFilePath))
            {
                List<BeatFlagItem> flags = ReadAllFlags();
                List<BeatFlagItem> cleanFlags = new();
                for (int i = 0; i < flags.Count; i++)
                    if (i < flags.Count)
                        if (!flagIndexes.Contains(i))
                            cleanFlags.Add(flags[i]);
                WriteAllTimeFlags(cleanFlags);

                return true;
            }

            return false;
        }
        #endregion

        public List<int> GetFloodedFlags(int flagIndex, float sensitivity = 0)
        {
            List<int> flags = new List<int>();
            for (int i = 0; i < BeatFlags.Count; i++)
                if (BeatFlags[i].Interferes(BeatFlags[flagIndex], sensitivity))
                    flags.Add(i);
            return flags;
        }

        public List<int> GetAllFloodedFlags()
        {
            List<int> floodFlagIndexes = new List<int>();
            for (int i = 0; i < BeatFlags.Count; i++)
            {
                floodFlagIndexes.AddRange(GetFloodedFlags(i));
            }
            floodFlagIndexes = floodFlagIndexes.Distinct().ToList();
            
            return floodFlagIndexes;
        }
    }
}

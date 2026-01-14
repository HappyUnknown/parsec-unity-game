using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.ShaderGraph.Serialization;
using UnityEditor.UIElements;
using UnityEngine;

namespace Assets.Scripts
{
    public class BeatFlagController
    {
        public static string FlagFilePath { get; set; }

        public BeatFlagController(string flagFilePath)
        {
            FlagFilePath = flagFilePath;
        }

        /// <summary>
        /// By certain time point (seconds), returns respective flag index
        /// </summary>
        /// <param name="timeFlag"></param>
        /// <returns></returns>
        public int GetFlagIndex(float timeFlag)
        {
            var beatFlags = ReadAllFlags();
            for (int i = 0; i < beatFlags.Count; i++)
            {
                bool isTimeIntersects = timeFlag >= beatFlags[i].TimePoint - beatFlags[i].ArmAround && timeFlag <= beatFlags[i].TimePoint + beatFlags[i].ArmAround;
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
            var beatFlags = ReadAllFlags();
            int flagIndex = GetFlagIndex(timeFlag);
            if (flagIndex != -1)
            {
                beatFlags[flagIndex].IsTaken = true;
                WriteAllTimeFlags(beatFlags);
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
            var beatFlags = ReadAllFlags();
            int flagIndex = GetFlagIndex(timeFlag);
            if (flagIndex == -1)
                return true;
            return beatFlags[flagIndex].IsTaken;
        }

        /// <summary>
        /// Seconds left to take matching flag
        /// </summary>
        /// <param name="timeFlag"></param>
        /// <returns></returns>
        public float TimeLeftThisSpan(float timeFlag)
        {
            var beatFlags = ReadAllFlags();
            float timeLeft = -1;
            if (IsFlagExists(timeFlag))
            {
                // Маємо індекс флагу
                int flagIndex = GetFlagIndex(timeFlag);

                // Обчислюємо час від поточної точки до кінця флагу
                float finalTime = beatFlags[flagIndex].TimePoint + beatFlags[flagIndex].ArmAround;

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
            var beatFlags = ReadAllFlags();
            foreach (BeatFlagItem flag in beatFlags)
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
            var beatFlags = ReadAllFlags();
            foreach (BeatFlagItem flag in beatFlags)
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

        public static List<(int, int)> GetFloodedFlagsAt(int flagIndex)
        {
            var beatFlags = ReadAllFlags();
            List<(int, int)> floodFlags = new();
            for (int i = 0; i < beatFlags.Count; i++)
                if (beatFlags[i].Interferes(beatFlags[flagIndex]))
                    floodFlags.Add((i, flagIndex));
            return floodFlags;
        }

        /// <summary>
        /// Estimates flags, which timespans overlap
        /// </summary>
        /// <param name="invariant">Flip tuple by ascension?</param>
        /// <param name="sensitivity">Additional timeflag radius</param>
        /// <returns>Tuples, where Item1 - left join, Item2 - right join</returns>
        public static List<int> GetFloodedFlagIndexes(bool invariant = true)
        {
            var beatFlags = ReadAllFlags();
            List<(int, int)> floodFlagPairs = new();
            for (int i = 0; i < beatFlags.Count; i++)
                floodFlagPairs.AddRange(GetFloodedFlagsAt(i));

            if (invariant)
                for (int i = 0; i < floodFlagPairs.Count; i++)
                    if (floodFlagPairs[i].Item1 > floodFlagPairs[i].Item2)
                        floodFlagPairs[i] = (floodFlagPairs[i].Item2, floodFlagPairs[i].Item1);
            floodFlagPairs = floodFlagPairs.Distinct().ToList();

            // Get only second one of overlaping entities
            List<int> floodFlagIndexes = new();
            foreach ((int, int) flagTuple in floodFlagPairs)
                floodFlagIndexes.Add(flagTuple.Item2);

            return floodFlagIndexes;
        }

        public static List<int> GetCollapsedFlagIndexes(float reactionTime)
        {
            var beatFlags = ReadAllFlags();
            List<int> collapsedFlagIndexes = new();
            for (int i = 0; i < beatFlags.Count; i++)
                if (beatFlags[i].IsCollapsed(reactionTime))
                    collapsedFlagIndexes.Add(i);
            return collapsedFlagIndexes;
        }

        public static bool ClearProblematicFlagIndexes(float reactionTime)
        {
            // Collapsed are cleared first, because they are non-interactive conflicts,
            // and so, their removal can cause resolve of interactive ones
            int[] clpsIndexes = GetCollapsedFlagIndexes(reactionTime).ToArray();
            RemoveFlagsAt(clpsIndexes);
            int[] fldIndexes = GetFloodedFlagIndexes().ToArray();
            RemoveFlagsAt(fldIndexes);

            return clpsIndexes.Length > 0 || fldIndexes.Length > 0;
        }
    }
}

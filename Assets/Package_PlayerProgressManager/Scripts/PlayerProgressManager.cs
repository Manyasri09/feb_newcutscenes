using PlayerProgressSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerProgressSystem
{
    public class PlayerProgressManager
    {
        //private const string TotalLevelsCompletedKey = "MainLevelsCompleted";
        //private const string TotalSubLevelsCompletedKey = "SubLevelsCompleted";
        private const string MainLevelKeyPrefix = "MainLevel_";
        private const string MainLevelRetryKey = "MainLevelRetry";
        private const string MainLevelTypePrefix = "MainLevelType_";
        private const string LastCompletedMainLevel = "LastCompletedMainLevel";


        private const string SubLevelKeyPrefix = "SubLevel_";
        private const string SubLevelRetryKey = "SubLevelRetry";
        private const string SubLevelTypePrefix = "SubLevelType_";
        private const string LastSubLevelCompleted= "LastCompletedSubLevel";


        private const string UsernameKey = "Username";
        private const string LastLoginKey = "LastLogin";
        private const string LatestLoginKey = "LatestLogin";


        private const string ExperienceKey = "Xp_";
        private const string CoinsKey = "Coins_";
        private const string GemsKey = "Gems_";
        private const string lastClaimedRewardKey = "Reward_Claim_DateTime";
        private const string DailyRewardIndexKey = "DailyRewardIndex";
        private const string LevelRewardPrefix = "LevelReward_";

        private readonly IPlayerProgressStorage _storage;

        public PlayerProgressManager(IPlayerProgressStorage storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }


        #region Username

        /// <summary>
        /// Set the username for the player.
        /// </summary>
        public void SetUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentException("Username cannot be null or empty", nameof(username));

            _storage.SetString(UsernameKey, username);
            _storage.Save();
        }

        /// <summary>
        /// Get the username of the player.
        /// </summary>
        public string GetUsername() => _storage.GetString(UsernameKey, "Guest");

        /// <summary>
        /// Reset the username to default.
        /// </summary>
        public void ResetUsername()
        {
            _storage.DeleteKey(UsernameKey);
            _storage.Save();
            Debug.Log("Username reset to default.");
        }

        #endregion

        #region MainLevel

        /// <summary>
        /// Call this method when a main level is completed.
        /// </summary>
        public void MainLevelCompleted(string mainLevelID)
        {
            string mainLevelKey = MainLevelKeyPrefix + mainLevelID;
            _storage.SetInt(mainLevelKey, 1);
            _storage.SetString(LastCompletedMainLevel, mainLevelID);
            _storage.Save();
        }
        /// <summary>
        /// Check if a specific main level is completed.
        /// </summary>
        public bool HasCompletedMainLevel(string mainLevelID)
        {
            string mainLevelKey = MainLevelKeyPrefix + mainLevelID;
            return _storage.GetInt(mainLevelKey, 0) == 1;
        }

        public void MainLevelRetries(int retryCount)
        {
            _storage.SetInt(MainLevelRetryKey, retryCount);
            _storage.Save();

        }

        public void MainLevelCompletedType(string type)
        {
            string mainLevelTypeKey = MainLevelTypePrefix + type;
            _storage.SetString(mainLevelTypeKey, type);
        }

        public string GetLastCompletedMainLevel()
        {
            return _storage.GetString(LastCompletedMainLevel, "0");
        }



        #endregion

        #region Sub Levels

        /// <summary>
        /// Call this method when a sub-level is completed.
        /// </summary>
        public void CompleteSubLevel(string subLevelID)
        {
            string subLevelKey = SubLevelKeyPrefix + subLevelID;
            _storage.SetInt(subLevelKey, 1);
            _storage.SetString(LastSubLevelCompleted, subLevelID);
            _storage.Save();
        }
        /// <summary>
        /// Check if a specific sub-level is completed.
        /// </summary>
        public bool HasCompletedSubLevel(string subLevelID)
        {
            string subLevelKey = SubLevelKeyPrefix + subLevelID;
            return _storage.GetInt(subLevelKey, 0) == 1;
        }

        public void SubLevelRetries(int subRetryCount)
        {
            _storage.SetInt(SubLevelRetryKey, subRetryCount);
            _storage.Save();
        }

        public void SubLevelCompletedType(string type)
        {
            string subLevelTypeKey = SubLevelTypePrefix + type;
            _storage.SetString(subLevelTypeKey, type);
        }

        public bool HasCompletedSubLevelType(string type)
        {
            string subLevelTypeKey = SubLevelTypePrefix + type;
            return _storage.GetInt(subLevelTypeKey, 0) == 1;
        }

        public string GetLastCompletedSubLevel()
        {
            return _storage.GetString("LastCompletedSubLevel", "No sub-levels completed yet.");
        }


        #endregion

        #region Login Tracking
        /// <summary>
        /// Call this method to save the login timestamps.
        /// </summary>

        public void UpdateLoginTime()
        {
            string currentLoginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (_storage.HasKey(LatestLoginKey))
            {
                string previousLogin = _storage.GetString(LatestLoginKey);
                _storage.SetString(LastLoginKey, previousLogin);
            }

            _storage.SetString(LatestLoginKey, currentLoginTime);
            _storage.Save();
        }

        /// <summary>
        /// Get the last login time (before the most recent login).
        /// </summary>
        public string GetLastLoginTime()
        {
            return _storage.GetString(LastLoginKey, "No previous login recorded.");
        }

        /// <summary>
        /// Get the latest login time (most recent login).
        /// </summary>
        public string GetLatestLoginTime()
        {
            return _storage.GetString(LatestLoginKey, "No login recorded yet.");
        }

        #endregion

        #region Currency Management

        public void UpdateCoins(int coins)
        {
            int storedCoins = _storage.GetInt(CoinsKey);
            storedCoins += coins;
            _storage.SetInt(CoinsKey, storedCoins);
            _storage.Save();
        }

        public int GetCoins() => _storage.GetInt(CoinsKey);

        public void UpdateGems(int gems)
        {
            _storage.SetInt(GemsKey, gems);
            _storage.Save();
        }

        public int GetGems() => _storage.GetInt(GemsKey);

        public void UpdateExperience(int xp)
        {
            _storage.SetInt(ExperienceKey, xp);
            _storage.Save();
        }

        public int GetExperience() => _storage.GetInt(ExperienceKey);

        #endregion

        #region Reward Tracking

        public bool HasLevelRewardClaimed(int level)
        {
            string levelRewardClaimedKey = LevelRewardPrefix + level.ToString();
            return _storage.HasKey(levelRewardClaimedKey);
        }

        public void SetLevelRewardClaimed(int level)
        {
            string levelRewardClaimedKey = LevelRewardPrefix + level.ToString();
            _storage.SetInt(levelRewardClaimedKey, 1);
        }

        public void SetLastRewardClaimedDateTime(DateTime dateTime)
        {
            _storage.SetString(lastClaimedRewardKey, dateTime.ToString("o"));
        }

        public string GetLastRewardClaimedDateTime()
        {
            return _storage.GetString(lastClaimedRewardKey);
        }

        public void SetDailyRewardIndex(int index)
        {
            _storage.SetInt(DailyRewardIndexKey, index);
            _storage.Save();
        }

        public int GetDailyRewardIndex()
        {
            return _storage.GetInt(DailyRewardIndexKey, 0);
        }

        public bool HasLastRewardClaimedDateTime()
        {
            return _storage.HasKey(lastClaimedRewardKey);
        }


        #endregion

        public void DeleteAllRecords() => _storage.DeleteAll();

    }
}

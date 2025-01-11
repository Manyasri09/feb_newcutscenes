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
            // Create a key for the main level
            string mainLevelKey = MainLevelKeyPrefix + mainLevelID;
            // Set the value of the key to 1
            _storage.SetInt(mainLevelKey, 1);
            // Set the last completed main level to the current main level
            _storage.SetString(LastCompletedMainLevel, mainLevelID);
            // Save the changes
            _storage.Save();
        }
        /// <summary>
        /// Check if a specific main level is completed.
        /// </summary>
        public bool HasCompletedMainLevel(string mainLevelID)
        {
            // Create a key for the main level
            string mainLevelKey = MainLevelKeyPrefix + mainLevelID;
            // Return true if the value of the key is 1, otherwise return false
            return _storage.GetInt(mainLevelKey, 0) == 1;
        }

        public void MainLevelRetries(int retryCount)
        {
            // Set the value of the main level retry key to the current retry count
            _storage.SetInt(MainLevelRetryKey, retryCount);
            // Save the changes
            _storage.Save();

        }

        public void MainLevelCompletedType(string type)
        {
            // Create a key for the main level type
            string mainLevelTypeKey = MainLevelTypePrefix + type;
            // Set the value of the key to the current type
            _storage.SetString(mainLevelTypeKey, type);
        }

        public string GetLastCompletedMainLevel()
        {
            // Return the last completed main level
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
            // Normalize the type to ensure consistent case handling
            string normalizedType = type.ToUpperInvariant();
            Debug.Log($"<color=red>Normalized type: {normalizedType}</color>");

            // Create the storage key
            string subLevelTypeKey = SubLevelTypePrefix + normalizedType;

            // Store a boolean value as an integer (1 for completed)
            _storage.SetInt(subLevelTypeKey, 1);
        }

        public bool HasCompletedSubLevelType(string type)
        {
            // Normalize the type to match the storage format
            string normalizedType = type.ToUpperInvariant();

            // Use the same key generation pattern
            string subLevelTypeKey = SubLevelTypePrefix + normalizedType;

            // Read the stored integer value and convert to boolean
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

        // Update the number of coins in the storage
        public void UpdateCoins(int coins)
        {
            // Get the current number of coins from the storage
            int storedCoins = _storage.GetInt(CoinsKey);
            // Add the new coins to the current number of coins
            storedCoins += coins;
            // Set the new number of coins in the storage
            _storage.SetInt(CoinsKey, storedCoins);
            // Save the changes to the storage
            _storage.Save();
        }

        // Get the number of coins from the storage
        public int GetCoins() => _storage.GetInt(CoinsKey);

        // Update the number of gems in the storage
        public void UpdateGems(int gems)
        {
            // Set the new number of gems in the storage
            _storage.SetInt(GemsKey, gems);
            // Save the changes to the storage
            _storage.Save();
        }

        // Get the number of gems from the storage
        public int GetGems() => _storage.GetInt(GemsKey);

        // Update the experience in the storage
        public void UpdateExperience(int xp)
        {
            // Set the new experience in the storage
            _storage.SetInt(ExperienceKey, xp);
            // Save the changes to the storage
            _storage.Save();
        }

        // Get the experience from the storage
        public int GetExperience() => _storage.GetInt(ExperienceKey);

        #endregion

        #region Reward Tracking

        // Check if the level reward has been claimed
        public bool HasLevelRewardClaimed(int level)
        {
            // Create a key for the level reward
            string levelRewardClaimedKey = LevelRewardPrefix + level.ToString();
            // Return true if the key exists in the storage
            return _storage.HasKey(levelRewardClaimedKey);
        }

        // Set the level reward as claimed
        public void SetLevelRewardClaimed(int level)
        {
            // Create a key for the level reward
            string levelRewardClaimedKey = LevelRewardPrefix + level.ToString();
            // Set the value of the key to 1
            _storage.SetInt(levelRewardClaimedKey, 1);
        }

        // Set the last reward claimed date and time
        public void SetLastRewardClaimedDateTime(DateTime dateTime)
        {
            // Set the value of the last claimed reward key to the date and time
            _storage.SetString(lastClaimedRewardKey, dateTime.ToString("o"));
        }

        // Get the last reward claimed date and time
        public string GetLastRewardClaimedDateTime()
        {
            // Return the value of the last claimed reward key
            return _storage.GetString(lastClaimedRewardKey);
        }

        // Set the daily reward index
        public void SetDailyRewardIndex(int index)
        {
            // Set the value of the daily reward index key to the index
            _storage.SetInt(DailyRewardIndexKey, index);
            // Save the storage
            _storage.Save();
        }

        // Get the daily reward index
        public int GetDailyRewardIndex()
        {
            // Return the value of the daily reward index key
            return _storage.GetInt(DailyRewardIndexKey, 0);
        }

        // Check if the last reward claimed date and time exists
        public bool HasLastRewardClaimedDateTime()
        {
            // Return true if the last claimed reward key exists in the storage
            return _storage.HasKey(lastClaimedRewardKey);
        }


        #endregion

        public void DeleteAllRecords() => _storage.DeleteAll();

    }
}

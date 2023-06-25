using System;
using UnityEngine;
using System.Collections.Generic;
namespace AJ.Generic.Extension
{
    public static class PauseExtension
    {
        private static bool gamePause = false;
        private static Dictionary<Type, bool> gamesPause = new();
        public static bool GamePause => gamePause;
        #region Games Pause
        public static bool IsGamesPause(this MonoBehaviour mono, Type key)
        {
            var have = gamesPause.TryGetValue(key, out var value);
            if (!have)
            {
                return false;
            }
            return value;
        }
        public static void GamesPause(this MonoBehaviour mono, Type key)
        {
            gamesPause[key] = true;
        }
        public static void GamesPlay(this MonoBehaviour mono, Type key)
        {
            gamesPause[key] = false;
        }
        public static void GamesPauseClear(this MonoBehaviour mono)
        {
            gamesPause.Clear();
        }
        public static void GamesPauseRemove(this MonoBehaviour mono, Type key)
        {
            gamesPause.Remove(key);
        }
        #endregion
        #region Game Pause
        public static bool IsPause(this MonoBehaviour mono)
        {
            return gamePause;
        }
        public static void Pause(this MonoBehaviour mono)
        {
            gamePause = true;
        }
        public static void Play(this MonoBehaviour mono)
        {
            gamePause = false;
        }
        public static void TimeSpeed(float timeScale)
        {
            Time.timeScale = timeScale;
        }
        #endregion
    }
}

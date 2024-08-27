using System.Collections.Generic;
using Main.Scripts.Data.Core;
using Main.Scripts.Infrastructure.Providers.Events;
using UnityEngine;

namespace Main.Scripts.Data.Services
{
    public interface IProgressDataService
    {
        int CurrentLevel { get; }
        float LevelProgress { get; }
        IReadOnlyCollection<BallData> BallsData { get; }
        int PlatformsCount { get; }
        int BallSpawnCount { get; }
        int BallMergeCount { get; }
        IReadOnlyCollection<Vector3> DynamicPlatforms { get; }
        void SetCurrentLevel(int levelId, bool autosave = true);
        void SwitchToNextLevel(bool autosave = true);
        void SaveData();
        void SetLevelProgress(float progress, bool autosave = true);
        void SetBallsData(List<BallData> ballsData, bool autosave = true);
        void IncreasePlatformsCount(bool autosave = true);
        void IncreaseBallsSpawnCount(bool autosave = true);
        void ResetLevelProgress(bool autosave = true);
        void IncreaseBallsMergeCount(bool autosave = true);
        void IncreaseDynamicPlatformsCount(bool autosave = true);
        void SetDynamicPlatformPosition(int id, Vector3 position, bool autosave = false);
    }

    public class ProgressDataService : DataService, IProgressDataService
    {
        private readonly ProgressData _progressData;
        private readonly GlobalEventProvider _globalEventProvider;

        public int CurrentLevel => _progressData.CurrentLevel;
        public float LevelProgress => _progressData.LevelProgress;
        public IReadOnlyCollection<BallData> BallsData => _progressData.BallsData;
        public int PlatformsCount => _progressData.PlatformsCount;
        public IReadOnlyCollection<Vector3> DynamicPlatforms => _progressData.DynamicPlatforms;
        public int BallSpawnCount => _progressData.BallSpawnCount;
        public int BallMergeCount => _progressData.BallMergeCount;

        protected ProgressDataService(IDatabase database, GlobalEventProvider globalEventProvider) : base(database)
        {
            _globalEventProvider = globalEventProvider;
            _progressData = database.GetData<ProgressData>();
        }

        public void SetCurrentLevel(int levelId, bool autosave = true)
        {
            if (levelId < 0)
                levelId = 0;

            _progressData.CurrentLevel = levelId;
            _globalEventProvider.Invoke<LevelSwitchEvent, int>(levelId);
            TryToSave(autosave);
        }

        public void SwitchToNextLevel(bool autosave = true)
        {
            int nextLevel = CurrentLevel + 1;
            SetCurrentLevel(nextLevel, false);

            TryToSave(autosave);
        }

        public void SetLevelProgress(float progress, bool autosave = true)
        {
            if (progress < 0)
                progress = 0;

            _progressData.LevelProgress = progress;
            _globalEventProvider.Invoke<ProgressChangeEvent, float>(progress);

            TryToSave(autosave);
        }

        public void SetBallsData(List<BallData> ballsData, bool autosave = true)
        {
            _progressData.BallsData.Clear();
            _progressData.BallsData.AddRange(ballsData);

            TryToSave(autosave);
        }

        public void IncreasePlatformsCount(bool autosave = true)
        {
            _progressData.PlatformsCount += 1;
            TryToSave(autosave);
        }

        public void IncreaseDynamicPlatformsCount(bool autosave = true)
        {
            _progressData.DynamicPlatforms.Add(Vector3.zero);
            TryToSave(autosave);
        }

        public void SetDynamicPlatformPosition(int id, Vector3 position, bool autosave = false)
        {
            _progressData.DynamicPlatforms[id] = position;
            TryToSave(autosave);
        }

        public void IncreaseBallsSpawnCount(bool autosave = true)
        {
            _progressData.BallSpawnCount += 1;
            TryToSave(autosave);
        }

        public void IncreaseBallsMergeCount(bool autosave = true)
        {
            _progressData.BallMergeCount += 1;
            TryToSave(autosave);
        }

        public void ResetLevelProgress(bool autosave = true)
        {
            SetLevelProgress(0, false);
            _progressData.PlatformsCount = 0;
            _progressData.BallSpawnCount = 0;
            _progressData.BallMergeCount = 0;
            _progressData.DynamicPlatforms.Clear();
            SetBallsData(new List<BallData>(), false);

            TryToSave(autosave);
        }

        public void SaveData() => TryToSave(true);
    }
}
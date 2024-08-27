namespace Main.Scripts.GameLogic.GameFlow
{
    public interface IGameFlowProvider
    {
        bool IsLoaded { get; }
        bool CanMergeBalls { get; }
        bool CanSpawnStaticPlatforms { get; }

        void Initialize();
        void StartGame();

        void SpawnBall(int count = 1);
        void MergeBalls();
        void RespawnBalls();
        void SpawnStaticPlatform();
        void SetMoneyMultiplier(float multiplier = 1);
        void SpawnDynamicPlatform();
    }
}
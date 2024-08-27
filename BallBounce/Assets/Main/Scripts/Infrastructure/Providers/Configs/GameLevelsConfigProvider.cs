using System.Linq;
using Main.Scripts.Configs;
using Main.Scripts.Configs.Levels;
using Main.Scripts.Core.Constants;
using Main.Scripts.Infrastructure.Providers.Assets;

namespace Main.Scripts.Infrastructure.Providers.Configs
{
    public interface IGameLevelsConfigProvider
    {
        GameLevelsConfig Config { get; }
        LevelConfig GetLevel(int i);
    }

    public class GameLevelsConfigProvider : IGameLevelsConfigProvider
    {
        public GameLevelsConfig Config { get; }

        public GameLevelsConfigProvider(IAssetProvider assetProvider) =>
            Config = assetProvider.Load<GameLevelsConfig>(ConfigsPaths.LEVELS_CONFIG_PATH);

        public LevelConfig GetLevel(int i)
        {
            if (i >= Config.LevelConfigs.Count)
                i = 0;

            return Config.LevelConfigs.ElementAt(i);
        }
    }
}
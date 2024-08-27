using Main.Scripts.Configs;
using Main.Scripts.Core.Constants;
using Main.Scripts.Infrastructure.Providers.Assets;

namespace Main.Scripts.Infrastructure.Providers.Configs
{
    public interface IBallProgressionConfigProvider
    {
        BallProgressionConfig Config { get; }
    }

    public class BallProgressionConfigProvider : IBallProgressionConfigProvider
    {
        public BallProgressionConfig Config { get; }

        public BallProgressionConfigProvider(IAssetProvider assetProvider) =>
            Config = assetProvider.Load<BallProgressionConfig>(ConfigsPaths.BALLS_CONFIG_PATH);
    }
}
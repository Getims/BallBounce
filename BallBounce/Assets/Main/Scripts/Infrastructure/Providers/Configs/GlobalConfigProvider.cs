using Main.Scripts.Configs;
using Main.Scripts.Core.Constants;
using Main.Scripts.Infrastructure.Providers.Assets;

namespace Main.Scripts.Infrastructure.Providers.Configs
{
    public interface IGlobalConfigProvider
    {
        GlobalConfig Config { get; }
    }

    public class GlobalConfigProvider : IGlobalConfigProvider
    {
        public GlobalConfig Config { get; }

        public GlobalConfigProvider(IAssetProvider assetProvider) =>
            Config = assetProvider.Load<GlobalConfig>(ConfigsPaths.GLOBAL_CONFIG_PATH);
    }
}
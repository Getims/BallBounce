using Main.Scripts.Configs.UI;
using Main.Scripts.Core.Constants;
using Main.Scripts.Infrastructure.Providers.Assets;

namespace Main.Scripts.Infrastructure.Providers.Configs
{
    public interface IUIConfigProvider
    {
        UIConfig Config { get; }
    }

    public class UIConfigProvider : IUIConfigProvider
    {
        public UIConfig Config { get; }

        public UIConfigProvider(IAssetProvider assetProvider) =>
            Config = assetProvider.Load<UIConfig>(ConfigsPaths.UI_CONFIG_PATH);
    }
}
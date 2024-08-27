using System;
using Main.Scripts.Core.Utilities;

namespace Main.Scripts.Infrastructure.Ads
{
    public static class AdsManager
    {
        private static Action<bool> _onRewarded;

        public static void ShowRewarded(Action<bool> onRewarded)
        {
            _onRewarded = onRewarded;
            Utils.ReworkPoint("Reward logic");
            OnRewarded(true);
        }

        public static bool CanShowRewarded() => true;

        private static void OnRewarded(bool giveReward) =>
            _onRewarded?.Invoke(giveReward);
    }
}
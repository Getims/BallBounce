using System;
using Main.Scripts.Infrastructure.Ads;

namespace Main.Scripts.UI.GameMenu.Controls
{
    public abstract class ControlLogic
    {
        protected int _cost = 0;
        protected bool _canShowAds = false;

        protected Action<int> _setPrice;
        private Action<bool> _setState;
        private Action<bool> _setAds;

        public virtual void Initialize(Action<bool> setState, Action<bool> setAds, Action<int> setPrice)
        {
            _setPrice = setPrice;
            _setAds = setAds;
            _setState = setState;

            UpdatePrice();
            UpdateState();
        }

        public virtual void UpdateState()
        {
            UpdatePrice();
            UpdateAds();

            if (!IsConditionComplete())
            {
                _setState?.Invoke(false);
                _setAds?.Invoke(false);
                return;
            }

            if (IsEnoughMoney())
            {
                _setState?.Invoke(true);
                _setAds?.Invoke(false);
            }
            else
            {
                _setState?.Invoke(false);
                _setAds?.Invoke(IsAdsConditionComplete());
            }
        }

        public virtual void OnClick()
        {
        }

        public virtual void OnAdsClick()
        {
        }

        protected virtual bool IsAdsConditionComplete() =>
            _canShowAds && AdsManager.CanShowRewarded();

        protected abstract bool IsEnoughMoney();
        protected abstract bool IsConditionComplete();
        protected abstract void UpdatePrice();
        protected abstract void UpdateAds();
    }
}
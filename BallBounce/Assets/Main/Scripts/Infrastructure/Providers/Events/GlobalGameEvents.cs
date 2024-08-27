namespace Main.Scripts.Infrastructure.Providers.Events
{
    public class GameLoadCompleteEvent : GameEvent
    {
    }

    public class SoundSwitchEvent : GameEvent<bool>
    {
    }

    public class MusicSwitchEvent : GameEvent<bool>
    {
    }

    public class MoneyChangedEvent : GameEvent<int>
    {
    }

    public class LevelSwitchEvent : GameEvent<int>
    {
    }

    public class ProgressChangeEvent : GameEvent<float>
    {
    }

    public class GameOverEvent : GameEvent<bool>
    {
    }

    public class GivePrizeRewardEvent : GameEvent<int>
    {
    }
}
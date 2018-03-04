using UniRx;

namespace RedChild
{
    public interface IGameStateProvider
    {
        IReadOnlyReactiveProperty<GameState> CurrentGameState { get; }
    }
}

using System;
using UnityEngine;
using UniRx;

namespace RedChild
{
    public enum GameState
    {
        Initializing,
        Title,
        Tutorial,
        Ready,
        Battle,
        Judgement,
        Result,
        Finished
    }

    public class GameStateReactiveProperty : ReactiveProperty<GameState>
    {
        public GameStateReactiveProperty()
        {
        }

        public GameStateReactiveProperty(GameState initialValue) : base(initialValue)
        {
        }
    }
}

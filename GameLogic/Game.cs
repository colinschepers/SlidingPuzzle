using Core.Games;
using System;
using System.Linq;

namespace GameLogic
{
    public class Game : IGame
    {
        public event Action<IGame> StartOfGame = delegate { };
        public event Action<IState, IPlayer> StartOfTurn = delegate { };
        public event Action<IState, IPlayer, IMove> EndOfTurn = delegate { };
        public event Action<IGame> EndOfGame = delegate { };

        public IState State { get; }
        public IPlayer[] Players { get; }
         
        public Game(IState state, IPlayer[] players)
        { 
            State = state.Clone();
            Players = players;
        }

        public void Start()
        {
            if (State == null)
            {
                throw new InvalidOperationException("Unable to start game because no state was set.");
            }
            if (Players == null || Players.Any(p => p == null) || Players.Length != 1)
            {
                throw new InvalidOperationException("Unable to start game because invalid number of players.");
            }

            StartOfGame(this);

            while (!State.GameOver)
            {
                var playerNr = State.PlayerToMove;
                var player = Players[playerNr];

                StartOfTurn(State, player);
                var move = player.GetMove(State);
                State.Play(move);
                InformOpponents(move, playerNr);
                EndOfTurn(State, player, move);
            }

            EndOfGame(this);
        }

        private void InformOpponents(IMove move, int playerNr)
        {
        }
    }
}
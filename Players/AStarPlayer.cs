using Core.Algorithms;
using Core.Games;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Players
{
    public class AStarPlayer : IPlayer
    {
        private readonly AStar<IState> _aStar;
        private readonly IState _goal;
        private readonly Dictionary<IState, IState> _transitions = new Dictionary<IState, IState>();

        public AStarPlayer(AStar<IState> aStar, IState goal)
        {
            _aStar = aStar;
            _goal = goal;
        }

        public IMove GetMove(IState state)
        {
            if (!_transitions.ContainsKey(state))
            {
                var path = _aStar.GetShortestPath(state, _goal, out double cost);
                path.Insert(0, state);

                for (int i = 0; i < path.Count - 1; i++)
                {
                    _transitions[path[i]] = path[i + 1];
                }
            }
            return GetMove(state, _transitions[state]);
        }

        private IMove GetMove(IState from, IState to)
        {
            foreach (var move in from.GetValidMoves())
            {
                var copy = from.Clone();
                copy.Play(move);

                if (copy.Equals(to))
                {
                    return move;
                }
            }
            throw new ArgumentException();
        }

        public void OpponentMoved(IMove move)
        {
        }

        public override string ToString()
        {
            return $"AStarPlayer()";
        }
    }
}

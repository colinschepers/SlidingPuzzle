using Core.Algorithms;
using Core.Games;
using GameLogic;
using Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SlidingPuzzle
{
    public class Solver : AStarPlayer
    {
        public Solver(int width, int height) : base(GetAStar(), new State(width, height))
        {
        }

        private static AStar<IState> GetAStar()
        {
            return new AStar<IState>((s) => GetNeighbors(s), 
                (s1, s2) => Cost((State)s1, (State)s2), 
                (s1, s2) => BooleanHeuristic((State)s1, (State)s2));
        }
        
        private static List<IState> GetNeighbors(IState state)
        {
            return state.GetValidMoves()
                .Select(m => { var copy = state.Clone(); copy.Play(m); return copy; })
                .ToList();
        }

        private static double Cost(State state1, State state2)
        {
            return state2.Round - state1.Round;
        }

        private static double BooleanHeuristic(State state1, State state2)
        {
            var perm1 = state1.Permutation;
            var perm2 = state2.Permutation;
            return perm1.Select((x, i) => x == perm2[i] ? 0 : 1).Sum();
        } 

        private static double ManhattanHeuristic(State state1, State state2)
        {
            double sum = 0;

            var permutation = state1.Permutation;
            var state2Dictionary = state2.Permutation.Select((item, idx) => new { item, idx }).ToDictionary(x => x.item, x => x.idx);

            for (int i = 0; i < permutation.Length; i++)
            {
                var tile = permutation[i];
                var x1 = i % state1.Width;
                var y1 = i / state1.Width;

                if(!state2Dictionary.ContainsKey(tile))
                {
                    throw new ArgumentException("Invalid state provided");
                }

                var x2 = state2Dictionary[tile] % state2.Width;
                var y2 = state2Dictionary[tile] / state2.Width;

                sum += Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
            }

            return sum;
        }

        public override string ToString()
        {
            return $"SlidingPuzzle.Solver()";
        }
    }
}

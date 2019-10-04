using Core.Algorithms;
using Core.Games;

namespace Players
{
    public class MCTSPlayer : IPlayer
    {
        public long ResponseTimeInMilliseconds { get; set; }
        private readonly MCTS _mcts;

        public MCTSPlayer(double utcC, int responseTimeInMilliseconds)
        {
            _mcts = new MCTS(utcC);
            ResponseTimeInMilliseconds = responseTimeInMilliseconds;
        }

        public IMove GetMove(IState state)
        {
            return _mcts.GetBestMove(state, ResponseTimeInMilliseconds);
        }

        public void OpponentMoved(IMove move)
        {
            _mcts.OpponentMoved(move);
        }
        
        public override string ToString()
        {
            return $"MCTSPlayer({_mcts})";
        }
    }
}

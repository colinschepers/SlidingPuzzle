using Core.Arithmetic;
using Core.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLogic
{
    public class State : IState
    {
        private static readonly Random _random = new Random();
        private static readonly int[] _primes = PrimeGenerator.Generate(20, 100).ToArray();

        public int PlayerToMove => 0;
        public int Round { get; private set; }
        public bool GameOver => _permutation.Select((x, i) => x == i).All(x => x);

        public int Width { get; }
        public int Height { get; }
        public int[] Permutation => _permutation.ToArray();

        private int[] _permutation;
        private int _openPosition;
        private int _hashCode;

        public State(int width, int height)
        {
            Width = width;
            Height = height;
            _permutation = Enumerable.Range(0, width * height).ToArray();
            _openPosition = width * height - 1;
            _hashCode = CalculateHashCode();
        }

        public void Shuffle(int count)
        {
            foreach (var i in Enumerable.Range(0, count))
            {
                var moves = GetValidMoves().ToList();
                Play(moves[_random.Next(moves.Count)]);
            }
            _openPosition = Array.IndexOf(_permutation, Width * Height - 1);
            _hashCode = CalculateHashCode();
            Round = 0;
        }

        public void Play(IMove move)
        {
            if (!IsValid(move))
            {
                throw new ArgumentException("Invalid move " + move);
            }

            var m = (Move)move;
            var idx = m.Direction == Direction.Up ? _openPosition + Width
                    : m.Direction == Direction.Down ? _openPosition - Width
                    : m.Direction == Direction.Left ? _openPosition + 1
                    : m.Direction == Direction.Right ? _openPosition - 1
                    : throw new ArgumentException("Invalid Direction in move");

            _hashCode += (_permutation[_openPosition] - _permutation[idx]) * _primes[idx % _primes.Length];
            _hashCode += (_permutation[idx] - _permutation[_openPosition]) * _primes[_openPosition % _primes.Length];
            
            _permutation[_openPosition] = _permutation[idx];
            _permutation[idx] = Width * Height - 1;
            _openPosition = idx;
            
            Round++;
        }

        public bool IsValid(IMove move)
        {
            return move is Move m &&
                ((m.Direction == Direction.Up && _openPosition < Width * Height - Width) ||
                 (m.Direction == Direction.Down && _openPosition >= Width) ||
                 (m.Direction == Direction.Left && (_openPosition + 1) % Width != 0) ||
                 (m.Direction == Direction.Right && _openPosition % Width != 0));
        }

        public IEnumerable<IMove> GetValidMoves()
        {
            var moves = new List<Move>(4);
            if (_openPosition < Width * Height - Width)
            {
                moves.Add(new Move(Direction.Up));
            }
            if (_openPosition >= Width)
            {
                moves.Add(new Move(Direction.Down));
            }
            if ((_openPosition + 1) % Width != 0)
            {
                moves.Add(new Move(Direction.Left));
            }
            if (_openPosition % Width != 0)
            {
                moves.Add(new Move(Direction.Right));
            }
            return moves;
        }

        public double GetScore(int playerNr)
        {
            return -Round;
        }

        public void SetPermutation(int[] permutation)
        {
            if (permutation.Length != Width * Height)
            {
                throw new ArgumentException("Array should have length equal to width * height.");
            }
            if (permutation.Min() != 0 || permutation.Max() != permutation.Length - 1
                || permutation.GroupBy(x => x).Any(g => g.Count() > 1))
            {
                throw new ArgumentException("Invalid permutation.");
            }
            
            _permutation = permutation.ToArray();
            _openPosition = Array.IndexOf(permutation, Width * Height - 1);
            _hashCode = CalculateHashCode();
        }

        public void Set(IState state)
        {
            var s = (State)state;
            Round = s.Round;
            _permutation = s._permutation.ToArray();
            _openPosition = s._openPosition;
            _hashCode = s._hashCode;
        }

        public IState Clone()
        {
            var state = new State(Width, Height);
            state.Set(this);
            return state;
        }
        
        public override bool Equals(object obj)
        {
            return obj is State state && Width == state.Width && Height == state.Height
                && _permutation.Select((x, i) => x == state._permutation[i]).All(x => x);
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        private int CalculateHashCode()
        {
            return _permutation.Select((x, i) => _primes[i % _primes.Length] * x).Sum();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            int maxLength = (Width * Height - 1).ToString().Length;
            for (int r = 0; r < Height; r++)
            {
                for (int c = 0; c < Width; c++)
                {
                    var value = ((_permutation[r * Width + c] + 1) % (Width * Height)).ToString();
                    builder.Append(new string(' ', maxLength - value.Length));
                    builder.Append(value == "0" ? "." : value);
                    builder.Append(" ");
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}

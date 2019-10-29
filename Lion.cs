using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Lion : Piece
    {
        public Lion(byte t) : base(t)
        {
            tile = "lion_" + t;
        }

        public override List<byte[][]> GetMove(Piece[,] board)
        {
            // Assume cannot move
            List<byte[][]> moves = new List<byte[][]>();

            // Find piece
            byte[] pos = GetPos(board);

            for (byte seed = 0; seed < 24; seed++)
            {
                sbyte[] dir = { (sbyte)(((seed + 3) % 5) - 2), (sbyte)((((seed/5) + 3) % 5) - 2) };
                byte[] finalDir = { (byte)(pos[0] + dir[0]), (byte)(pos[1] + dir[1]) };

                if (finalDir[0] < 0 || finalDir[0] >= board.GetLength(0) || finalDir[1] < 0 || finalDir[1] >= board.GetLength(1))
                {
                    continue;
                }
                if (board[finalDir[0], finalDir[1]] != null)
                {
                    if (board[finalDir[0], finalDir[1]].team != this.team)
                    {
                        byte[][] steps = new byte[3][];
                        steps[0] = new byte[] { finalDir[0], finalDir[1], byte.MaxValue, byte.MaxValue };
                        steps[1] = new byte[] { finalDir[0], finalDir[1], pos[0], pos[1] };
                        steps[2] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                        moves.Add(steps);
                    }
                }
                else
                {
                    byte[][] steps = new byte[2][];
                    steps[0] = new byte[] { finalDir[0], finalDir[1], pos[0], pos[1] };
                    steps[1] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                    moves.Add(steps);
                }
            }

            return moves;
        }

        public override List<byte[][]> GetAttack(Piece[,] board)
        {
            // Assume cannot move
            List<byte[][]> moves = new List<byte[][]>();

            // Find piece
            byte[] pos = GetPos(board);

            for (byte seed = 0; seed < 24; seed++)
            {
                sbyte[] dir = { (sbyte)(((seed + 3) % 5) - 2), (sbyte)((((seed / 5) + 3) % 5) - 2) };
                byte[] finalDir = { (byte)(pos[0] + dir[0]), (byte)(pos[1] + dir[1]) };

                if (finalDir[0] < 0 || finalDir[0] >= board.GetLength(0) || finalDir[1] < 0 || finalDir[1] >= board.GetLength(1))
                {
                    continue;
                }
                if (board[finalDir[0], finalDir[1]] != null)
                {
                    if (board[finalDir[0], finalDir[1]].team != this.team)
                    {
                        byte[][] steps = new byte[3][];
                        steps[0] = new byte[] { finalDir[0], finalDir[1], byte.MaxValue, byte.MaxValue };
                        steps[1] = new byte[] { finalDir[0], finalDir[1], pos[0], pos[1] };
                        steps[2] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                        moves.Add(steps);
                    }
                }
            }

            return moves;
        }
    }
}

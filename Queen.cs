using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Queen : Piece
    {
        public Queen(byte t) : base(t)
        {
            tile = "queen_" + t;
        }

        public override List<byte[][]> GetMove(Piece[,] board)
        {
            List<byte[][]> moves = new List<byte[][]>();

            byte[] pos = GetPos(board);

            for (byte seed = 0; seed < 8; seed++)
            {
                sbyte dX = (sbyte)(((seed + 2) % 3) - 1);
                sbyte dY = (sbyte)((((seed / 3) + 2) % 3) - 1);
                int[] newPos = { (pos[0] + dX), (pos[1] + dY) };


                while (0 <= newPos[0] && newPos[0] < board.GetLength(0) && 0 <= newPos[1] && newPos[1] < board.GetLength(1))
                {
                    if (board[newPos[0], newPos[1]] != null)
                    {
                        if (board[newPos[0], newPos[1]].team != this.team)
                        {
                            byte[][] move = new byte[3][];
                            move[0] = new byte[] { (byte)newPos[0], (byte)newPos[1], byte.MaxValue, byte.MaxValue };
                            move[1] = new byte[] { (byte)newPos[0], (byte)newPos[1], pos[0], pos[1] };
                            move[2] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                            moves.Add(move);
                        }
                        break;
                    }
                    else
                    {
                        byte[][] move = new byte[2][];
                        move[0] = new byte[] { (byte)newPos[0], (byte)newPos[1], pos[0], pos[1] };
                        move[1] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                        moves.Add(move);
                    }

                    newPos[0] += dX;
                    newPos[1] += dY;
                }
            }
            return moves;
        }

        public override List<byte[][]> GetAttack(Piece[,] board)
        {
            List<byte[][]> moves = new List<byte[][]>();

            byte[] pos = GetPos(board);

            for (byte seed = 0; seed < 8; seed++)
            {
                sbyte dX = (sbyte)(((seed + 2) % 3) - 1);
                sbyte dY = (sbyte)((((seed / 3) + 2) % 3) - 1);
                int[] newPos = { (pos[0] + dX), (pos[1] + dY) };


                while (0 <= newPos[0] && newPos[0] < board.GetLength(0) && 0 <= newPos[1] && newPos[1] < board.GetLength(1))
                {
                    if (board[newPos[0], newPos[1]] != null)
                    {
                        if (board[newPos[0], newPos[1]].team != this.team)
                        {
                            byte[][] move = new byte[3][];
                            move[0] = new byte[] { (byte)newPos[0], (byte)newPos[1], byte.MaxValue, byte.MaxValue };
                            move[1] = new byte[] { (byte)newPos[0], (byte)newPos[1], pos[0], pos[1] };
                            move[2] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                            moves.Add(move);
                        }
                        break;
                    }

                    newPos[0] += dX;
                    newPos[1] += dY;
                }
            }
            return moves;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Antelope : Piece
    {
        public Antelope(byte t) : base(t)
        {
            tile = "antelope_" + t;
        }

        public override List<byte[][]> GetMove(Piece[,] board)
        {
            // Assume cannot move
            List<byte[][]> moves = new List<byte[][]>();

            // Find piece
            byte[] pos = GetPos(board);

            // Elephant movement
            for (byte seed = 0; seed < 8; seed++)
            {
                sbyte[] dir = { (sbyte)(((seed % 2) * 2) - 1), (sbyte)((((byte)(seed / 4)) * 2) - 1) };
                sbyte mag = (sbyte)(((seed / 2) % 2) + 2);
                byte[] finalDir = { (byte)(pos[0] + (dir[0] * mag)), (byte)(pos[1] + (dir[1] * mag)) };

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

            // Machine's movement
            for (byte seed = 0; seed < 8; seed++)
            {
                sbyte[] dir = { (sbyte)(((seed / 4) + 1) % 2), (sbyte)(seed / 4) };
                dir[0] *= (sbyte)(dir[0] * (((seed / 2) % 2) * 2) - 1);
                dir[1] *= (sbyte)(dir[1] * ((((seed / 2) + 1) % 2) * 2) - 1);
                sbyte mag = (sbyte)((seed % 2) + 2);
                byte[] finalDir = { (byte)(pos[0] + (dir[0] * mag)), (byte)(pos[1] + (dir[1] * mag)) };

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

            // Elephant movement
            for (byte seed = 0; seed < 8; seed++)
            {
                sbyte[] dir = { (sbyte)(((seed % 2) * 2) - 1), (sbyte)((((byte)(seed / 4)) * 2) - 1) };
                sbyte mag = (sbyte)(((seed / 2) % 2) + 2);
                byte[] finalDir = { (byte)(pos[0] + (dir[0] * mag)), (byte)(pos[1] + (dir[1] * mag)) };

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

            // Machine's movement
            for (byte seed = 0; seed < 8; seed++)
            {
                sbyte[] dir = { (sbyte)(((seed / 4) + 1) % 2), (sbyte)(seed / 4) };
                dir[0] *= (sbyte)(dir[0] * (((seed / 2) % 2) * 2) - 1);
                dir[1] *= (sbyte)(dir[1] * ((((seed / 2) + 1) % 2) * 2) - 1);
                sbyte mag = (sbyte)((seed % 2) + 2);
                byte[] finalDir = { (byte)(pos[0] + (dir[0] * mag)), (byte)(pos[1] + (dir[1] * mag)) };

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Diablo : Piece
    {
        public Diablo(byte t) : base(t)
        {
            tile = "diablo_" + t;
        }

        public override List<byte[][]> GetMove(Piece[,] board)
        {
            // Assume cannot move
            List<byte[][]> moves = new List<byte[][]>();

            byte[] pos = GetPos(board);

            // Bishop's movement
            for (int seed = 0; seed < 4; seed++)
            {
                sbyte dX = (sbyte)(((seed % 2) * 2) - 1);
                sbyte dY = (sbyte)(((seed / 2) * 2) - 1);

                for (int i = 1; i < board.GetLength(0) + board.GetLength(0); i++)
                {
                    byte[] newPos = { (byte)(pos[0] + (i * dX)), (byte)(pos[1] + (i * dY)) };

                    if (!(newPos[0] < 0 || board.GetLength(0) <= newPos[0] || newPos[1] < 0 || board.GetLength(1) <= newPos[1]))
                    {
                        if (board[newPos[0], newPos[1]] != null)
                        {
                            if (board[newPos[0], newPos[1]].team != this.team)
                            {
                                byte[][] spot = new byte[3][];
                                spot[0] = new byte[] { newPos[0], newPos[1], byte.MaxValue, byte.MaxValue };
                                spot[1] = new byte[] { newPos[0], newPos[1], pos[0], pos[1] };
                                spot[2] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                                moves.Add(spot);
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            byte[][] spot = new byte[2][];
                            spot[0] = new byte[] { newPos[0], newPos[1], pos[0], pos[1] };
                            spot[1] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                            moves.Add(spot);
                        }
                    }
                }
            }

            // Knight's movement
            for (byte seed = 0; seed < 8; seed++)
            {
                byte[] dir = { (byte)(((seed / 4) + 1) % 2), (byte)(seed / 4) };
                sbyte xSign = (sbyte)((((seed / 2) % 2) * 2) - 1);
                sbyte ySign = (sbyte)(((seed % 2) * 2) - 1);

                byte[] finalDir = { (byte)(pos[0] + (dir[0] + 1) * xSign), (byte)(pos[1] + (dir[1] + 1) * ySign) };
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

            byte[] pos = GetPos(board);

            // Bishop's movement
            for (int seed = 0; seed < 4; seed++)
            {
                sbyte dX = (sbyte)(((seed % 2) * 2) - 1);
                sbyte dY = (sbyte)(((seed / 2) * 2) - 1);

                for (int i = 1; i < board.GetLength(0) + board.GetLength(0); i++)
                {
                    byte[] newPos = { (byte)(pos[0] + (i * dX)), (byte)(pos[1] + (i * dY)) };

                    if (!(newPos[0] < 0 || board.GetLength(0) <= newPos[0] || newPos[1] < 0 || board.GetLength(1) <= newPos[1]))
                    {
                        if (board[newPos[0], newPos[1]] != null)
                        {
                            if (board[newPos[0], newPos[1]].team != this.team)
                            {
                                byte[][] spot = new byte[3][];
                                spot[0] = new byte[] { newPos[0], newPos[1], byte.MaxValue, byte.MaxValue };
                                spot[1] = new byte[] { newPos[0], newPos[1], pos[0], pos[1] };
                                spot[2] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                                moves.Add(spot);
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            // Knight's movement
            for (byte seed = 0; seed < 8; seed++)
            {
                byte[] dir = { (byte)(((seed / 4) + 1) % 2), (byte)(seed / 4) };
                sbyte xSign = (sbyte)((((seed / 2) % 2) * 2) - 1);
                sbyte ySign = (sbyte)(((seed % 2) * 2) - 1);

                byte[] finalDir = { (byte)(pos[0] + (dir[0] + 1) * xSign), (byte)(pos[1] + (dir[1] + 1) * ySign) };
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

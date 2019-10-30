using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Gryphon : Piece
    {
        public Gryphon(byte t) : base(t)
        {
            tile = "gryphon_" + t;
        }

        public override List<byte[][]> GetMove(Piece[,] board)
        {
            // Assume cannot move
            List<byte[][]> moves = new List<byte[][]>();

            // Find piece
            byte[] pos = GetPos(board);

            for (byte seed = 0; seed < 4; seed++)
            {
                sbyte[] dir = { (sbyte)((((seed % 2) * 2) - 1)), (sbyte)(((seed / 2) * 2) - 1) };

                // Do X dir
                bool doY = true;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    byte[] finalDir = { (byte)(pos[0] + (dir[0] * (i + 1))), (byte)(pos[1] + dir[1]) };
                    if (finalDir[0] < 0 || finalDir[0] >= board.GetLength(0) || finalDir[1] < 0 || finalDir[1] >= board.GetLength(1))
                    {
                        if (i == 0)
                        {
                            doY = false;
                        }
                        break;
                    }
                    else
                    {
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

                            // If its the first step then don't bother with y, thats where it intersects with y
                            if (i == 0)
                            {
                                doY = false;
                            }
                            break;
                        }
                        else
                        {
                            byte[][] steps = new byte[2][];
                            steps[0] = new byte[] { finalDir[0], finalDir[1], pos[0], pos[1] };
                            steps[1] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                            moves.Add(steps);
                        }
                    }

                }

                // Do Y dir
                if (doY)
                {
                    for (int i = 0; i < board.GetLength(1); i++)
                    {
                        byte[] finalDir = { (byte)(pos[0] + dir[0]), (byte)(pos[1] + (dir[1] * (i + 1))) };
                        if (finalDir[0] < 0 || finalDir[0] >= board.GetLength(0) || finalDir[1] < 0 || finalDir[1] >= board.GetLength(1))
                        {
                            break;
                        }
                        else
                        {
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
                                break;
                            }
                            else
                            {
                                byte[][] steps = new byte[2][];
                                steps[0] = new byte[] { finalDir[0], finalDir[1], pos[0], pos[1] };
                                steps[1] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                                moves.Add(steps);
                            }
                        }

                    }
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

            for (byte seed = 0; seed < 4; seed++)
            {
                sbyte[] dir = { (sbyte)((((seed % 2) * 2) - 1)), (sbyte)(((seed / 2) * 2) - 1) };

                // Do X dir
                bool doY = true;
                for (int i = 0; i < board.GetLength(0); i++)
                {
                    byte[] finalDir = { (byte)(pos[0] + (dir[0] * (i + 1))), (byte)(pos[1] + dir[1]) };
                    if (finalDir[0] < 0 || finalDir[0] >= board.GetLength(0) || finalDir[1] < 0 || finalDir[1] >= board.GetLength(1))
                    {
                        if (i == 0)
                        {
                            doY = false;
                        }
                        break;
                    }
                    else
                    {
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

                            // If its the first step then don't bother with y, thats where it intersects with y
                            if (i == 0)
                            {
                                doY = false;
                            }
                            break;
                        }
                    }

                }

                // Do Y dir
                if (doY)
                {
                    for (int i = 0; i < board.GetLength(1); i++)
                    {
                        byte[] finalDir = { (byte)(pos[0] + dir[0]), (byte)(pos[1] + (dir[1] * (i + 1))) };
                        if (finalDir[0] < 0 || finalDir[0] >= board.GetLength(0) || finalDir[1] < 0 || finalDir[1] >= board.GetLength(1))
                        {
                            break;
                        }
                        else
                        {
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
                                break;
                            }
                        }

                    }
                }
            }

            return moves;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Cannon : Piece
    {
        bool hasMoved;
        public Cannon(byte t) : base(t)
        {
            tile = "cannon_" + t;
        }

        public override List<byte[][]> GetMove(Piece[,] board)
        {
            // Assume cannot move
            List<byte[][]> move = new List<byte[][]>();

            byte[] pos = GetPos(board);

            for (byte seed = 0; seed < 4; seed++)
            {
                sbyte[] delta = { (sbyte)(seed / 2), (sbyte)((((seed / 2)) + 1) % 2) };
                delta[0] = (sbyte)((((seed % 2) * 2) - 1) * delta[0]);
                delta[1] = (sbyte)((((seed % 2) * 2) - 1) * delta[1]);

                // Index when found piece, or off end of board
                int stopped = int.MaxValue;
                bool isEndOfBoard = false;
                for (int i = 1; i < board.GetLength(0) + board.GetLength(1); i++)
                {
                    byte[] newPos = new byte[] { ((byte)(pos[0] + (i * delta[0]))), ((byte)(pos[1] + (i * delta[1]))) };

                    if (!(newPos[0] < 0 || board.GetLength(0) <= newPos[0] || newPos[1] < 0 || board.GetLength(1) <= newPos[1]))
                    {
                        // Update if blank, mark and stop if not
                        if (board[newPos[0], newPos[1]] == null)
                        {
                            byte[][] spot = new byte[2][];
                            spot[0] = new byte[] { newPos[0], newPos[1], pos[0], pos[1] };
                            spot[1] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                            move.Add(spot);
                        }
                        else
                        {
                            stopped = i;
                            break;
                        }
                    }
                    else
                    {
                        isEndOfBoard = true;
                        break;
                    }
                }

                // Now continue until finds piece AFTER the first piece, add first occasion of enemy or stop when found other than empty
                for (int i = (stopped+1); i < board.GetLength(0) + board.GetLength(1); i++)
                {
                    byte[] newPos = new byte[] { ((byte)(pos[0] + (i * delta[0]))), ((byte)(pos[1] + (i * delta[1]))) };

                    if (!(newPos[0] < 0 || board.GetLength(0) <= newPos[0] || newPos[1] < 0 || board.GetLength(1) <= newPos[1]))
                    {
                        if (board[newPos[0], newPos[1]] != null)
                        {
                            // Stopped at this spot
                            stopped = i;

                            if (board[newPos[0], newPos[1]].team != this.team)
                            {
                                byte[][] spot = new byte[3][];
                                spot[0] = new byte[] { newPos[0], newPos[1], byte.MaxValue, byte.MaxValue };
                                spot[1] = new byte[] { newPos[0], newPos[1], pos[0], pos[1] };
                                spot[2] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                                move.Add(spot);
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return move;
        }

        public override List<byte[][]> GetAttack(Piece[,] board)
        {
            // Assume cannot move
            List<byte[][]> move = new List<byte[][]>();

            byte[] pos = GetPos(board);

            for (byte seed = 0; seed < 4; seed++)
            {
                sbyte[] delta = { (sbyte)(seed / 2), (sbyte)((((seed / 2)) + 1) % 2) };
                delta[0] = (sbyte)((((seed % 2) * 2) - 1) * delta[0]);
                delta[1] = (sbyte)((((seed % 2) * 2) - 1) * delta[1]);

                // Index when found piece, or off end of board
                int stopped = int.MaxValue;
                bool isEndOfBoard = false;
                for (int i = 1; i < board.GetLength(0) + board.GetLength(1); i++)
                {
                    byte[] newPos = new byte[] { ((byte)(pos[0] + (i * delta[0]))), ((byte)(pos[1] + (i * delta[1]))) };

                    if (!(newPos[0] < 0 || board.GetLength(0) <= newPos[0] || newPos[1] < 0 || board.GetLength(1) <= newPos[1]))
                    {
                        // Update if blank, mark and stop if not
                        if (board[newPos[0], newPos[1]] == null)
                        {
                            byte[][] spot = new byte[2][];
                            spot[0] = new byte[] { newPos[0], newPos[1], pos[0], pos[1] };
                            spot[1] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                            move.Add(spot);
                        }
                        else
                        {
                            stopped = i;
                            break;
                        }
                    }
                    else
                    {
                        isEndOfBoard = true;
                        break;
                    }
                }

                // Now continue until finds piece AFTER the first piece, add first occasion of enemy or stop when found other than empty
                for (int i = (stopped + 1); i < board.GetLength(0) + board.GetLength(1); i++)
                {
                    byte[] newPos = new byte[] { ((byte)(pos[0] + (i * delta[0]))), ((byte)(pos[1] + (i * delta[1]))) };

                    if (!(newPos[0] < 0 || board.GetLength(0) <= newPos[0] || newPos[1] < 0 || board.GetLength(1) <= newPos[1]))
                    {
                        if (board[newPos[0], newPos[1]] != null)
                        {
                            // Stopped at this spot
                            stopped = i;

                            if (board[newPos[0], newPos[1]].team != this.team)
                            {
                                byte[][] spot = new byte[3][];
                                spot[0] = new byte[] { newPos[0], newPos[1], byte.MaxValue, byte.MaxValue };
                                spot[1] = new byte[] { newPos[0], newPos[1], pos[0], pos[1] };
                                spot[2] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                                move.Add(spot);
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return move;
        }
    }
}

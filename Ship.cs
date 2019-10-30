using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Ship : Piece
    {
        public Ship(byte t) : base(t)
        {
            tile = "ship_" + t;
            prom.Insert(0, typeof(Gryphon));
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

                // Do Y dir
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

                // Do Y dir
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

            return moves;
        }

        public override Game1.GameState Move(Piece[,] board, int code)
        {
            byte[] currPos = GetPos(board);
            // X == 0 || X == board.GetLength(0)-1
            byte supposeY = (byte)(((team + 1) % 2) * (board.GetLength(1) - 1));

            if (currPos[1] == supposeY && (currPos[0] == 0 || currPos[0] == (board.GetLength(0) - 1)))
            {
                listening = true;
                return Game1.GameState.prom;
            }

            return Game1.GameState.blank;
        }
    }
}

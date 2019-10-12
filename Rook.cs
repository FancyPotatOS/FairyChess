using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Rook : Piece
    {
        bool hasMoved;
        public Rook(byte t) : base(t)
        {
            tile = "rook_" + t;
            hasMoved = false;
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

                for (int i = 1; i < board.GetLength(0) + board.GetLength(1); i++)
                {
                    byte[] newPos = new byte[] { ((byte)(pos[0] + (i * delta[0]))), ((byte)(pos[1] + (i * delta[1]))) };

                    if (!(newPos[0] < 0 || board.GetLength(0) <= newPos[0] || newPos[1] < 0 || board.GetLength(1) <= newPos[1]))
                    {
                        if (board[newPos[0],newPos[1]] != null)
                        {
                            if (board[newPos[0], newPos[1]].team != this.team)
                            {
                                byte[][] spot = new byte[3][];
                                spot[0] = new byte[] { newPos[0], newPos[1], byte.MaxValue, byte.MaxValue };
                                spot[1] = new byte[] { newPos[0], newPos[1], pos[0], pos[1] };
                                spot[2] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue-1 };
                                move.Add(spot);
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
                            spot[1] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue-1 };
                            move.Add(spot);
                        }
                    }
                }
            }

            // Castling
            if (!Game1.KingInCheck(team, board))
            {

            }

            return move;

        }

        public override Game1.GameState Move(Piece[,] board, int code)
        {
            hasMoved = true;
            return Game1.GameState.blank;
        }
    }
}

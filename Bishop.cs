using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Bishop : Piece
    {
        public Bishop(byte t) : base(t)
        {
            tile = "bishop_" + t;
        }

        public override List<byte[][]> GetMove(Piece[,] board)
        {
            List<byte[][]> moves = new List<byte[][]>();
            byte[] pos = GetPos(board);

            for (int seed = 0; seed < 4; seed++)
            {
                sbyte dX = (sbyte)(((seed%2)*2)-1);
                sbyte dY = (sbyte)(((seed/2)*2)-1);

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
                                spot[2] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue-1 };
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
                            spot[1] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue-1 };
                            moves.Add(spot);
                        }
                    }
                }
            }

            return moves;
        }
    }
}

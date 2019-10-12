using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class King : Piece
    {
        public bool hasMoved;

        public King(byte t) : base(t)
        {
            tile = "king_" + t;
            hasMoved = false;
        }

        public override Game1.GameState Move(Piece[,] board, int code)
        {
            hasMoved = true;
            return Game1.GameState.blank;
        }

        public override List<byte[][]> GetMove(Piece[,] board)
        {
            List<byte[][]> moves = new List<byte[][]>();
            byte[] pos = GetPos(board);

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    int[] newPos = { (pos[0] + x), (pos[1] + y) };

                    if (newPos[0] < 0 || board.GetLength(0) <= newPos[0] || newPos[1] < 0 || board.GetLength(1) <= newPos[1])
                    {
                        continue;
                    }
                    else
                    {
                        if (board[newPos[0], newPos[1]] != null)
                        {
                            if (board[newPos[0], newPos[1]].team != this.team)
                            {
                                byte[][] move = new byte[3][];
                                move[0] = new byte[]{ (byte)newPos[0], (byte)newPos[1], byte.MaxValue, byte.MaxValue };
                                move[1] = new byte[] { (byte)newPos[0], (byte)newPos[1], pos[0], pos[1] };
                                move[2] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue-1 };
                                moves.Add(move);
                            }
                        }
                        else
                        {
                            byte[][] move = new byte[2][];
                            move[0] = new byte[] { (byte)newPos[0], (byte)newPos[1], pos[0], pos[1] };
                            move[1] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue-1 };
                            moves.Add(move);
                        }
                    }
                }
            }
            return moves;
        }
    }
}

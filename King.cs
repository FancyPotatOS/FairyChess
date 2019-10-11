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

        public override void Move()
        {
            hasMoved = true;
        }

        public override bool[,] GetMove(Piece[,] board)
        {
            bool[,] move = new bool[board.GetLength(0), board.GetLength(1)];
            byte[] pos = GetPos(board);

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }
                    if (!((pos[0] + i) < 0 || (pos[0] + i) >= board.GetLength(0) || (pos[1] + j) < 0 || (pos[1] + j) >= board.GetLength(1)))
                    {
                        if (board[pos[0] + i, pos[1] + j] != null)
                        {
                            if (board[pos[0] + i, pos[1] + j].team != this.team)
                            {
                                move[pos[0] + i, pos[1] + j] = true;
                            }
                        }
                        else
                        {
                            move[pos[0] + i, pos[1] + j] = true;
                        }
                    }
                }
            }
            return move;
        }
    }
}

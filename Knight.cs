using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Knight : Piece
    {
        public Knight(byte t) : base(t)
        {
            tile = "knight_" + t;
        }

        public override bool[,] GetMove(Piece[,] board)
        {
            // Assume cannot move
            bool[,] move = new bool[board.GetLength(0), board.GetLength(1)];

            // Find piece
            byte[] pos = GetPos(board);

            for (sbyte seed = 0; seed < 8; seed++)
            {
                sbyte[] dir = { (sbyte)(((seed/4) + 1) % 2), (sbyte)(seed/4) };
                sbyte xSign = (sbyte)((((seed/2)%2)*2)-1);
                sbyte ySign = (sbyte)(((seed%2)*2)-1);

                sbyte[] finalDir = { (sbyte)(pos[0] + (dir[0] + 1) * xSign), (sbyte)(pos[1] + (dir[1] + 1) * ySign) };
                if (finalDir[0] < 0 || finalDir[0] >= board.GetLength(0) || finalDir[1] < 0 || finalDir[1] >= board.GetLength(1))
                {
                    continue;
                }
                if (board[finalDir[0], finalDir[1]] != null)
                {
                    if (board[finalDir[0], finalDir[1]].team != this.team)
                    {
                        move[finalDir[0], finalDir[1]] = true;
                    }
                }
                else
                {
                    move[finalDir[0], finalDir[1]] = true;
                }
            }

            return move;

        }
    }
}

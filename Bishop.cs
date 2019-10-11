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

        public override bool[,] GetMove(Piece[,] board)
        {
            // Assume cannot move
            bool[,] move = new bool[board.GetLength(0), board.GetLength(1)];

            // Find piece
            byte[] pos = GetPos(board);

            for (int seed = 0; seed < 4; seed++)
            {
                sbyte[] dir = { (sbyte)(((seed / 2) * 2) - 1), (sbyte)(1 - ((seed / 2) * 2)) };
                sbyte sign = (sbyte)(((seed%2)*2)-1);
                int[] currPos = { (pos[0] + (dir[0]*sign)), (pos[1] + dir[1]) };
                while (0 <= currPos[0] && currPos[0] < board.GetLength(0) && 0 <= currPos[1] && currPos[1] < board.GetLength(1))
                {
                    if (board[currPos[0],currPos[1]] != null)
                    {
                        if (board[currPos[0], currPos[1]].team != this.team)
                        {
                            move[currPos[0], currPos[1]] = true;
                            break;
                        }
                    }
                    else
                    {
                        move[currPos[0], currPos[1]] = true;
                    }

                    currPos[0] += (dir[0] * sign);
                    currPos[1] += dir[1];
                }
            }

            return move;

        }
    }
}

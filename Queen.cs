using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Queen : Piece
    {
        public Queen(byte t) : base(t)
        {
            tile = "queen_" + t;
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
                sbyte sign = (sbyte)(((seed % 2) * 2) - 1);
                int[] currPos = { (pos[0] + (dir[0] * sign)), (pos[1] + dir[1]) };
                while (0 <= currPos[0] && currPos[0] < board.GetLength(0) && 0 <= currPos[1] && currPos[1] < board.GetLength(1))
                {
                    if (board[currPos[0], currPos[1]] != null)
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

            // Make direction vectors
            int[][] dirs = new int[4][];
            dirs[0] = new int[] { 1, 0 };
            dirs[1] = new int[] { -1, 0 };
            dirs[2] = new int[] { 0, 1 };
            dirs[3] = new int[] { 0, -1 };

            // Move in direction vectors until hits piece, making true
            for (int i = 0; i < 4; i++)
            {
                for (int j = 1; j < board.Length; j++)
                {
                    int spotX = (pos[0] + (dirs[i][0] * j));
                    int spotY = (pos[1] + (dirs[i][1] * j));
                    if (spotX < 0 || spotX >= board.GetLength(0) || spotY < 0 || spotY >= board.GetLength(1))
                    {
                        break;
                    }
                    else if (board[spotX, spotY] == null)
                    {
                        move[spotX, spotY] = true;
                    }
                    else
                    {
                        if (board[spotX, spotY].team != this.team)
                        {
                            move[spotX, spotY] = true;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return move;
        }
    }
}

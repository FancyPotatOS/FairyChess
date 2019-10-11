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

        public override bool[,] GetMove(Piece[,] board)
        {
            // Assume cannot move
            bool[,] move = new bool[board.GetLength(0), board.GetLength(1)];

            // Find piece
            byte[] pos = { 0, 0 };
            bool found = false;
            for (byte x = 0; x < board.GetLength(0) && !found; x++)
            {
                for (byte y = 0; y < board.GetLength(1) && !found; y++)
                {
                    if (board[x,y] == this)
                    {
                        pos[0] = x;
                        pos[1] = y;
                        found = true;
                    }
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
                    else if (board[spotX,spotY] == null)
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

        public byte[] GetCastle(Piece[,] board)
        {
            // Make sure can castle
            if (hasMoved) { return null; }

            byte[] pos = { 0, 0 };
            bool foundThis = false;
            byte[] kingPos = { 0, 0 };
            bool foundKing = false;
            King king = new King(Byte.MaxValue);

            for (byte x = 0; x < board.GetLength(0) && !(foundKing && foundThis); x++)
            {
                for (byte y = 0; y < board.GetLength(0) && !(foundKing && foundThis); y++)
                {
                    if (board[x,y] != null)
                    {
                        if (board[x, y] == this)
                        {
                            foundThis = true;
                            pos[0] = x;
                            pos[1] = y;
                        }
                        else if (board[x, y].team == this.team && board[x, y].GetType() == typeof(King))
                        {
                            foundKing = true;
                            kingPos[0] = x;
                            kingPos[1] = y;
                            king = (King)board[x, y];
                        }
                        if (foundKing && foundThis)
                        {
                            break;
                        }
                    }
                }
            }

            if (kingPos[1] != pos[1])
            {
                return null;
            }
            else if (!king.hasMoved)
            {
                int dX = kingPos[0] - pos[0];
                sbyte dir = (sbyte)((dX) / (Math.Abs(dX)));
                for (int i = 1; i < board.GetLength(0); i++)
                {
                    int currX = pos[0] + (dir * i);
                    if (currX == kingPos[0])
                    {
                        return new byte[] { (byte)(pos[0] + (dX * (((double)3)/((double)4)))), pos[1] };
                    }
                    else if (board[currX, pos[1]] != null)
                    {
                        return null;
                    }
                }
                
            }

            return null;

        }

        public override void Move()
        {
            hasMoved = true;
        }
    }
}

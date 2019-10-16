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
                byte[][] castleMove = GetCastle(board);
                if (castleMove != null)
                {
                    move.Add(castleMove);
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

                for (int i = 1; i < board.GetLength(0) + board.GetLength(1); i++)
                {
                    byte[] newPos = new byte[] { ((byte)(pos[0] + (i * delta[0]))), ((byte)(pos[1] + (i * delta[1]))) };

                    if (!(newPos[0] < 0 || board.GetLength(0) <= newPos[0] || newPos[1] < 0 || board.GetLength(1) <= newPos[1]))
                    {
                        if (board[newPos[0], newPos[1]] != null)
                        {
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
                }
            }

            return move;
            
         }

        public byte[][] GetCastle(Piece[,] board)
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
                    if (board[x, y] != null)
                    {
                        if (board[x, y] == this)
                        {
                            foundThis = true;
                            pos[0] = x;
                            pos[1] = y;
                        }
                        else if (board[x, y].team == this.team && board[x, y].GetType() == typeof(King))
                        {
                            King temp = (King)board[x, y];
                            if (!temp.hasMoved)
                            {
                                foundKing = true;
                                kingPos[0] = x;
                                kingPos[1] = y;
                                king = temp;
                            }
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
                        byte[][] move = new byte[4][];
                        move[0] = new byte[] { (byte)(kingPos[0] + (-dir * 2)), kingPos[1], kingPos[0], kingPos[1] };
                        move[1] = new byte[] { kingPos[0], kingPos[1], byte.MaxValue-1, byte.MaxValue };
                        move[2] = new byte[] { (byte)(pos[0] + (dX * (((double)3) / ((double)4)))), pos[1], pos[0], pos[1] };
                        move[3] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                        return move;
                    }
                    else if (board[currX, pos[1]] != null)
                    {
                        return null;
                    }
                }

            }

            return null;

        }

        public override Game1.GameState Move(Piece[,] board, int code)
        {
            hasMoved = true;
            return Game1.GameState.blank;
        }
    }
}

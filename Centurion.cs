using System.Collections.Generic;
using System;

namespace Chess
{
    class Centurion : Pawn
    {

        public Centurion(byte t) : base(t)
        {
            tile = "centurion_" + t;
            prom.Clear();
            prom.Insert(0, typeof(Queen));
            prom.Insert(0, typeof(Lion));
            prom.Insert(0, typeof(Gryphon));
        }

        public override List<byte[][]> GetMove(Piece[,] board)
        {
            List<byte[][]> moves = new List<byte[][]>();
            byte[] pos = GetPos(board);

            sbyte sign = (sbyte)((((team + 1) % 2) * 2) - 1);
            sbyte[] vec = { (sbyte)(sign * (team / 2)), (sbyte)((((team / 2) + 1) % 2) * sign) };

            // Move one, then two
            for (int i = 1; i < 3; i++)
            {

                byte[] tempPos = { (byte)(pos[0] + (vec[0] * i)), (byte)(pos[1] + (vec[1] * i)) };
                if (0 <= tempPos[0] && tempPos[0] < board.GetLength(0) && 0 <= tempPos[1] && tempPos[1] < board.GetLength(1))
                {

                    if (board[tempPos[0], tempPos[1]] == null)
                    {
                        byte[][] move = new byte[2][];
                        move[0] = new byte[] { tempPos[0], tempPos[1], pos[0], pos[1] };
                        move[1] = new byte[] { pos[0], pos[1], byte.MaxValue, (byte)(byte.MaxValue - i) };
                        moves.Add(move);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // Check if can capture
            for (int i = -1; i < 2; i += 2)
            {
                byte[] tempPos = { (byte)(pos[0] + vec[0]), (byte)(pos[1] + vec[1]) };
                tempPos[0] += (byte)(((vec[0] + 1) % 2) * i);
                tempPos[1] += (byte)(((vec[1] + 1) % 2) * i);

                if (0 <= tempPos[0] && tempPos[0] < board.GetLength(0) && 0 <= tempPos[1] && tempPos[1] < board.GetLength(1))
                {
                    if (board[tempPos[0], tempPos[1]] != null)
                    {
                        if (board[tempPos[0], tempPos[1]].team != this.team)
                        {
                            byte[][] move = new byte[3][];
                            move[0] = new byte[] { tempPos[0], tempPos[1], byte.MaxValue, byte.MaxValue };
                            move[1] = new byte[] { tempPos[0], tempPos[1], pos[0], pos[1] };
                            move[2] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                            moves.Add(move);
                        }
                    }
                    else
                    {
                        byte[][] move = new byte[2][];
                        move[0] = new byte[] { tempPos[0], tempPos[1], pos[0], pos[1] };
                        move[1] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                        moves.Add(move);
                    }
                }
            }

            // Check for En Passant
            for (int i = -1; i < 2; i += 2)
            {
                byte[] tempPos = { (byte)(pos[0]), (byte)(pos[1]) };
                tempPos[0] += (byte)(((vec[0] + 1) % 2) * i);
                tempPos[1] += (byte)(((vec[1] + 1) % 2) * i);

                if (0 <= tempPos[0] && tempPos[0] < board.GetLength(0) && 0 <= tempPos[0] && tempPos[0] < board.GetLength(1))
                {
                    if (board[tempPos[0], tempPos[1]] != null)
                    {
                        if (board[tempPos[0], tempPos[1]].team != this.team)
                        {

                            if (board[tempPos[0], tempPos[1]].GetType().IsSubclassOf(typeof(Pawn)) || board[tempPos[0], tempPos[1]].GetType() == typeof(Pawn))
                            {
                                Pawn p = (Pawn)board[tempPos[0], tempPos[1]];
                                if (p.movedTwice)
                                {
                                    byte[][] move = new byte[3][];
                                    move[0] = new byte[] { tempPos[0], tempPos[1], byte.MaxValue, byte.MaxValue };

                                    tempPos[0] = (byte)(tempPos[0] + vec[0]);
                                    tempPos[1] = (byte)(tempPos[1] + vec[1]);

                                    if (board[tempPos[0], tempPos[1]] != null)
                                    {
                                        continue;
                                    }

                                    move[1] = new byte[] { tempPos[0], tempPos[1], pos[0], pos[1] };
                                    move[2] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                                    moves.Add(move);
                                }
                            }
                        }
                    }
                }
            }

            return moves;
        }

        public override List<byte[][]> GetAttack(Piece[,] board)
        {
            List<byte[][]> moves = new List<byte[][]>();
            byte[] pos = GetPos(board);

            sbyte sign = (sbyte)((((team + 1) % 2) * 2) - 1);
            sbyte[] vec = { (sbyte)(sign * (team / 2)), (sbyte)((((team / 2) + 1) % 2) * sign) };

            // Check if can capture
            for (int i = -1; i < 2; i += 2)
            {
                byte[] tempPos = { (byte)(pos[0] + vec[0]), (byte)(pos[1] + vec[1]) };
                tempPos[0] += (byte)(((vec[0] + 1) % 2) * i);
                tempPos[1] += (byte)(((vec[1] + 1) % 2) * i);

                if (0 <= tempPos[0] && tempPos[0] < board.GetLength(0) && 0 <= tempPos[1] && tempPos[1] < board.GetLength(1))
                {
                    if (board[tempPos[0], tempPos[1]] != null)
                    {
                        if (board[tempPos[0], tempPos[1]].team != this.team)
                        {
                            byte[][] move = new byte[3][];
                            move[0] = new byte[] { tempPos[0], tempPos[1], byte.MaxValue, byte.MaxValue };
                            move[1] = new byte[] { tempPos[0], tempPos[1], pos[0], pos[1] };
                            move[2] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                            moves.Add(move);
                        }
                    }
                }
            }

            // Check for En Passant
            for (int i = -1; i < 2; i += 2)
            {
                byte[] tempPos = { (byte)(pos[0]), (byte)(pos[1]) };
                tempPos[0] += (byte)(((vec[0] + 1) % 2) * i);
                tempPos[1] += (byte)(((vec[1] + 1) % 2) * i);

                if (0 <= tempPos[0] && tempPos[0] < board.GetLength(0) && 0 <= tempPos[0] && tempPos[0] < board.GetLength(1))
                {
                    if (board[tempPos[0], tempPos[1]] != null)
                    {
                        if (board[tempPos[0], tempPos[1]].team != this.team)
                        {

                            if (board[tempPos[0], tempPos[1]].GetType().IsSubclassOf(typeof(Pawn)) || board[tempPos[0], tempPos[1]].GetType() == typeof(Pawn))
                            {
                                Pawn p = (Pawn)board[tempPos[0], tempPos[1]];
                                if (p.movedTwice)
                                {
                                    byte[][] move = new byte[3][];
                                    move[0] = new byte[] { tempPos[0], tempPos[1], byte.MaxValue, byte.MaxValue };

                                    tempPos[0] = (byte)(tempPos[0] + vec[0]);
                                    tempPos[1] = (byte)(tempPos[1] + vec[1]);

                                    if (board[tempPos[0], tempPos[1]] != null)
                                    {
                                        continue;
                                    }

                                    move[1] = new byte[] { tempPos[0], tempPos[1], pos[0], pos[1] };
                                    move[2] = new byte[] { pos[0], pos[1], byte.MaxValue, byte.MaxValue - 1 };
                                    moves.Add(move);
                                }
                            }
                        }
                    }
                }
            }

            return moves;
        }

        public override void Update()
        {
            if (skipMoveTwice)
            {
                skipMoveTwice = false;
            }
            else
            {
                movedTwice = false;
            }
        }

        public override Game1.GameState Move(Piece[,] board, int code)
        {
            if (code == 2)
            {
                movedTwice = true;
                skipMoveTwice = true;
            }

            byte[] currPos = GetPos(board);
            if (((byte)(team / 2)) > 0)
            {
                byte supposeX = (byte)(((byte)(team / 2)) * (team % 2) * board.GetLength(0));
                if (supposeX == currPos[0])
                {
                    listening = true;
                    return Game1.GameState.prom;
                }
            }
            else
            {
                byte supposeY = (byte)(((((byte)(team / 2)) + 1) % 2) * (((team % 2) + 1) % 2) * (board.GetLength(1) - 1));
                if (supposeY == currPos[1])
                {
                    listening = true;
                    return Game1.GameState.prom;
                }
            }

            return Game1.GameState.blank;
        }
    }
}

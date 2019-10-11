using System.Collections.Generic;
using System;

namespace Chess
{
    class Pawn : Piece
    {

        bool hasMoved;
        public bool movedTwice;

        public Pawn(byte t) : base(t)
        {
            tile = "pawn_" + t;
            hasMoved = false;
            movedTwice = false;
            
            prom.Insert(0, typeof(Queen));
            prom.Insert(0, typeof(Knight));
            prom.Insert(0, typeof(Rook));
            prom.Insert(0, typeof(Bishop));
        }

        public override void Move()
        {
            hasMoved = true;
        }

        public bool CanEnPassant(Piece[,] board)
        {
            byte[] pos = GetPos(board);
            sbyte dY = (sbyte)(1 - (team * 2));

            for (int i = -1; i <= 1; i+=2)
            {
                // Has piece there
                if (board[pos[0] + i, pos[1]] != null)
                {
                    // Is Pawn
                    if (board[pos[0] + i, pos[1]].GetType().Equals(typeof(Pawn)))
                    {
                        // Is opposite team
                        if (board[pos[0] + i, pos[1]].team != this.team)
                        {
                            // Did move 2 spaces last
                            if (((Pawn)board[pos[0] + i, pos[1]]).movedTwice)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public override bool[,] GetMove(Piece[,] board)
        {
            bool[,] move = new bool[board.GetLength(0), board.GetLength(1)];

            byte[] pos = GetPos(board);

            for (int i = 0; i < 2; i++)
            {
                sbyte dY = (sbyte)(1 - (team * 2));
                for (sbyte j = -1; j <= 1; j+=2)
                {
                    if ((pos[0] + j) < 0 || move.GetLength(0) <= (pos[0] + j) || (pos[1] + dY) < 0 || move.GetLength(1) <= (pos[1] + dY)) { continue; }


                    if (board[(pos[0] + j), (pos[1] + dY)] != null)
                    {
                        if (board[(pos[0] + j), (pos[1] + dY)].team != this.team)
                        {
                            move[(pos[0] + j), (pos[1] + dY)] = true;
                        }
                    }
                }

                // Do not check last forward movement if already moved once
                if (hasMoved && i > 0)
                {
                    continue;
                }
                else if (i == 1 && board[pos[0], pos[1] + dY] != null)
                {
                    continue;
                }

                dY *= (sbyte)(i+1);

                if (pos[1] + dY < 0 || board.GetLength(1) <= pos[1] + dY)
                {
                    continue;
                }

                if (board[pos[0], pos[1] + dY] == null)
                {
                    move[pos[0], pos[1] + dY] = true;
                }
            }

            // En Passant
            for (int i = -1; i <= 1; i += 2)
            {

                if ((pos[0] + i) < 0 || move.GetLength(0) <= (pos[0] + i) || (pos[1]) < 0 || move.GetLength(1) <= (pos[1])) { continue; }

                // Has piece there
                if (board[pos[0] + i, pos[1]] != null)
                {
                    // Is Pawn
                    if (board[pos[0] + i, pos[1]].GetType().Equals(typeof(Pawn)))
                    {
                        // Is opposite team
                        if (board[pos[0] + i, pos[1]].team != this.team)
                        {
                            // Did move 2 spaces last
                            if (((Pawn)board[pos[0] + i, pos[1]]).movedTwice)
                            {
                                move[pos[0] + i, pos[1] + (1 - (team * 2))] = true;
                            }
                        }
                    }
                }
            }

            return move;
        }
    }
}

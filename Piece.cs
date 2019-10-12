using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    class Piece
    {
        public List<Type> prom;

        public String tile;
        public readonly byte team;
        public bool listening;

        public Piece(byte t)
        {
            prom = new List<Type>();
            tile = "blank";
            team = t;
            listening = false;
        }

        public virtual List<byte[][]> GetMove(Piece[,] board)
        {
            return null;
        }

        public virtual List<byte[][]> GetSafeMove(Piece[,] board)
        {
            List<byte[][]> allMoves = GetMove(board);
            List<byte[][]> safeMoves = new List<byte[][]>();
            
            foreach (byte[][] curr in allMoves)
            {
                Piece[,] copy = Game1.CopyBoard(board);

                for (int i = 0; i < curr.Length; i++)
                {
                    if (curr[i][2] == byte.MaxValue || curr[i][3] == byte.MaxValue)
                    {
                        copy[curr[i][0], curr[i][1]] = null;
                    }
                    else
                    {
                        copy[curr[i][0], curr[i][1]] = copy[curr[i][2], curr[i][3]];
                    }
                }
                if (!Game1.KingInCheck(team, copy))
                {
                    safeMoves.Add(curr);
                }
            }
            return safeMoves;
        }

        // Updates piece just moved, might show what state board should be
        public virtual Game1.GameState Move(Piece[,] board, int code)
        {
            return Game1.GameState.blank;
        }
        
        // Updates every other piece besides moved piece
        public virtual void Update() { }

        // Provides what type to change piece to
        public virtual void Provide(Type t, Piece[,] realBoard)
        {
            if (listening)
            {
                byte[] pos = GetPos(realBoard);
                Piece p = (Piece)t.GetConstructor(new Type[] { typeof(byte) }).Invoke(new object[] { team });
                realBoard[pos[0], pos[1]] = p;
                listening = false;
            }
        }

        public byte[] GetPos(Piece[,] board)
        {
            byte[] pos = { 0, 0 };
            bool found = false;
            for (byte x = 0; x < board.GetLength(0) && !found; x++)
            {
                for (byte y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x, y] == this)
                    {
                        pos[0] = x;
                        pos[1] = y;
                        found = true;
                        break;
                    }
                }
            }
            return pos;
        }

    }
}

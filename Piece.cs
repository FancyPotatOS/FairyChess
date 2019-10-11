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

        public Piece(byte t)
        {
            prom = new List<Type>();
            tile = "blank";
            team = t;
        }

        public virtual bool[,] GetMove(Piece[,] board)
        {
            return null;
        }
        public virtual void Move() { }
        
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

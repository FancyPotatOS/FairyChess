using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Chess
{

        // TODO: 
        // Castling
        // En Passant
        // King is in check/checkmate check
        // 


        // Assume 0 means headed +Y/
        // 1 means -Y
        // 2 means +X
        // 3 means -X
        //
        // sign = ((((seed+1)%2)*2)-1)
        // x = (seed/2) * sign
        // y = ((seed/2)+1)%2) * sign

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        readonly Piece[,] board;

        readonly static int DRAWSIZE = 54;
        readonly static int BOARDSIZE = 16;
        

        List<byte[][]> currMoves;
        byte moveSel;
        readonly byte[] currSel;
        readonly byte[] freeSel;

        byte currTurn;

        byte promoteSel;
        Piece promotee;

        byte winner;

        GameState currState;
        MoveType currMType;

        public enum GameState
        {
            boardSel, boardMove, blank, prom
        }

        public enum MoveType
        {
            list, free
        }

        readonly List<Keys> accountedKeys;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            board = new Piece[BOARDSIZE, BOARDSIZE+1];
            currSel = new byte[] { 0, 0 };
            freeSel = new byte[] { 0, 0 };

            moveSel = 0;

            currMoves = new List<byte[][]>();

            currTurn = 1;
            currState = GameState.boardSel;
            currMType = MoveType.free;

            promoteSel = 0;
            promotee = null;
            winner = 0;

            //board = GetDefault();

            board[12, 8] = new Antelope(1);
            board[12, 12] = new King(1);
            board[12, 13] = new Bow(1);

            board[6, 8] = new Antelope(0);
            board[6, 9] = new Diablo(0);
            board[6, 10] = new Antelope(0);
            board[6, 11] = new King(0);
            board[6, 12] = new Unicorn(0);
            board[6, 13] = new Lion(0);

            accountedKeys = new List<Keys>();
        }
        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferWidth = (DRAWSIZE * board.GetLength(0));
            graphics.PreferredBackBufferHeight = (DRAWSIZE * board.GetLength(1));
            graphics.ApplyChanges();

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
                Exit();

            // Pressed keys to list
            Keys[] pKeys = Keyboard.GetState().GetPressedKeys();
            List<Keys> pressedKeys = new List<Keys>();
            for (int i = 0; i < pKeys.Length; i++)
            {
                pressedKeys.Add(pKeys[i]);
            }

            // Find new keys
            List<Keys> newKeys = new List<Keys>();
            for (int i = 0; i < pressedKeys.Count; i++)
            {
                if (!accountedKeys.Contains(pressedKeys[i]))
                {
                    newKeys.Add(pressedKeys[i]);
                    accountedKeys.Add(pressedKeys[i]);
                }
            }

            // Removed accounted unpressed keys
            for (int i = 0; i < accountedKeys.Count; i++)
            {
                if (!pressedKeys.Contains(accountedKeys[i]))
                {
                    accountedKeys.RemoveAt(i);
                    i--;
                }
            }

            if (newKeys.Contains(Keys.E))
            {
                { }
            }

            switch (currState)
            {
                case GameState.boardMove:
                    {

                        // Switch from list of moves to free selection 
                        if (newKeys.Contains(Keys.R))
                        {
                            if (currMType == MoveType.list)
                            {
                                currMType = MoveType.free;
                            }
                            else if (currMType == MoveType.free)
                            {
                                currMType = MoveType.list;
                            }
                        }

                        if (currMType == MoveType.list)
                        {
                            if (newKeys.Contains(Keys.A))
                            {
                                moveSel = (byte)((moveSel + 1) % currMoves.Count);
                            }
                            if (newKeys.Contains(Keys.W))
                            {
                                moveSel = (byte)((moveSel + 1) % currMoves.Count);
                            }
                            if (newKeys.Contains(Keys.D))
                            {
                                moveSel = (byte)((moveSel + currMoves.Count - 1) % currMoves.Count);
                            }
                            if (newKeys.Contains(Keys.S))
                            {
                                moveSel = (byte)((moveSel + currMoves.Count - 1) % currMoves.Count);
                            }
                            if (newKeys.Contains(Keys.Enter))
                            {
                                if (board[currSel[0], currSel[1]].team == currTurn)
                                {
                                    currMoves = board[currSel[0], currSel[1]].GetSafeMove(board);
                                    if (currMoves != null)
                                    {
                                        if (currMoves.Count > 0)
                                        {
                                            MoveToChoice();
                                        }
                                    }
                                }
                            }
                        }
                        else if (currMType == MoveType.free)
                        {
                            if (newKeys.Contains(Keys.A))
                            {
                                freeSel[0] = (byte)Math.Max(0, freeSel[0] - 1);
                            }
                            if (newKeys.Contains(Keys.D))
                            {
                                freeSel[0] = (byte)Math.Min(board.GetLength(0) - 1, freeSel[0] + 1);
                            }
                            if (newKeys.Contains(Keys.S))
                            {
                                freeSel[1] = (byte)Math.Min(board.GetLength(1) - 1, freeSel[1] + 1);
                            }
                            if (newKeys.Contains(Keys.W))
                            {
                                freeSel[1] = (byte)Math.Max(0, freeSel[1] - 1);
                            }
                            if (newKeys.Contains(Keys.Enter))
                            {
                                if (board[currSel[0], currSel[1]].team == currTurn)
                                {
                                    bool cont = true;
                                    for (int i = 0; i < currMoves.Count && cont; i++)
                                    {
                                        byte[][] curr = currMoves[i];
                                        for (int j = 0; j < curr.Length && cont; j++)
                                        {
                                            // Test if replacing position of piece selected
                                            if (curr[j][2] == currSel[0] && curr[j][3] == currSel[1] && 
                                                // Test if destination placement is at freeSel
                                                freeSel[0] == curr[j][0] && freeSel[1] == curr[j][1])
                                            {
                                                moveSel = (byte)i;
                                                MoveToChoice();
                                                currSel[0] = freeSel[0];
                                                currSel[1] = freeSel[1];
                                                cont = false;
                                                continue;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (newKeys.Contains(Keys.Escape))
                        {
                            currState = GameState.boardSel;
                        }
                        break;
                    }
                case GameState.boardSel:
                    {
                        if (newKeys.Contains(Keys.A))
                        {
                            currSel[0] = (byte)Math.Max(0, currSel[0] - 1);
                        }
                        if (newKeys.Contains(Keys.D))
                        {
                            currSel[0] = (byte)Math.Min(board.GetLength(0) - 1, currSel[0] + 1);
                        }
                        if (newKeys.Contains(Keys.S))
                        {
                            currSel[1] = (byte)Math.Min(board.GetLength(1) - 1, currSel[1] + 1);
                        }
                        if (newKeys.Contains(Keys.W))
                        {
                            currSel[1] = (byte)Math.Max(0, currSel[1] - 1);
                        }
                        if (newKeys.Contains(Keys.Enter))
                        {
                            if (board[currSel[0], currSel[1]] != null)
                            {
                                if (board[currSel[0], currSel[1]].team == currTurn)
                                {
                                    moveSel = 0;

                                    currMoves = board[currSel[0], currSel[1]].GetSafeMove(board);
                                    if (currMoves != null)
                                    {
                                        if (currMoves.Count != 0)
                                        {
                                            currState = GameState.boardMove;
                                            freeSel[0] = currSel[0];
                                            freeSel[1] = currSel[1];
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    }
                case GameState.prom:
                    {
                        if (newKeys.Contains(Keys.A))
                        {
                            promoteSel = (byte)((promoteSel + promotee.prom.Count - 1) % promotee.prom.Count);
                        }
                        if (newKeys.Contains(Keys.D))
                        {
                            promoteSel = (byte)((promoteSel + 1) % promotee.prom.Count);
                        }
                        if (newKeys.Contains(Keys.Enter))
                        {
                            promotee.Provide(promotee.prom[promoteSel], board);
                            currState = GameState.boardSel;
                            currTurn = (byte)((currTurn + 1) % 2);
                        }
                        break;
                    }
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        
}

        void MoveToChoice()
        {
            byte[][] move = currMoves[moveSel];
            for (int i = 0; i < move.GetLength(0); i++)
            {
                if (move[i][2] == byte.MaxValue && move[i][3] == byte.MaxValue)
                {
                    board[move[i][0], move[i][1]] = null;
                }
                else if (move[i][2] == byte.MaxValue)
                {
                    byte code = (byte)(byte.MaxValue - move[i][3]);
                    board[move[i][0], move[i][1]] = null;
                    GameState tempState = board[move[i - 1][0], move[i - 1][1]].Move(board, code);
                    if (tempState == GameState.prom)
                    {
                        promotee = board[move[i - 1][0], move[i - 1][1]];
                        promoteSel = 0;
                        currState = tempState;
                    }
                }
                else if (move[i][2] == byte.MaxValue - 1)
                {
                    board[move[i][0], move[i][1]] = null;
                }
                else
                {
                    board[move[i][0], move[i][1]] = board[move[i][2], move[i][3]];
                }
            }
            if (currState != GameState.prom)
            {
                currTurn = (byte)((currTurn + 1) % 2);
                currState = GameState.boardSel;
            }

            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x, y] != null)
                    {
                        board[x, y].Update();
                    }
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp);

            switch (currState)
            {
                case GameState.boardSel:
                    {
                        PrintBoard();
                        PrintPieces();
                        PrintMovement(currSel[0],currSel[1]);
                        PrintSel("selected");
                        break;
                    }
                case GameState.boardMove:
                    {

                        PrintBoard();
                        PrintPieces();
                        PrintMove();
                        PrintSel("choice");
                        PrintMove();
                        break;

                    }
                case GameState.prom:
                    {
                        PrintPromote();
                        break;
                    }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        void PrintWon()
        {

        }

        void PrintMove()
        {
            Texture2D tex;
            Rectangle rect;
            if (currMType == MoveType.list)
            {
                if (currMoves == null)
                {
                    return;
                }
                else if (currMoves.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < currMoves[moveSel].Length; i++)
                {
                    if (currMoves[moveSel][i][3] == byte.MaxValue)
                    {
                        tex = Content.Load<Texture2D>("captureable");
                        rect = new Rectangle(new Point(currMoves[moveSel][i][0] * DRAWSIZE, currMoves[moveSel][i][1] * DRAWSIZE), new Point(DRAWSIZE, DRAWSIZE));
                        spriteBatch.Draw(tex, rect, Color.White);
                    }
                    else if (currMoves[moveSel][i][2] == byte.MaxValue - 1)
                    {
                        tex = Content.Load<Texture2D>("captureable");
                        rect = new Rectangle(new Point(currMoves[moveSel][i][0] * DRAWSIZE, currMoves[moveSel][i][1] * DRAWSIZE), new Point(DRAWSIZE, DRAWSIZE));
                        spriteBatch.Draw(tex, rect, Color.White);
                    }
                    else if (currMoves[moveSel][i][2] == byte.MaxValue)
                    {
                        tex = Content.Load<Texture2D>("choice");
                        rect = new Rectangle(new Point(currMoves[moveSel][i][0] * DRAWSIZE, currMoves[moveSel][i][1] * DRAWSIZE), new Point(DRAWSIZE, DRAWSIZE));
                        spriteBatch.Draw(tex, rect, Color.White);
                    }
                    else
                    {
                        tex = Content.Load<Texture2D>("moveable");
                        rect = new Rectangle(new Point(currMoves[moveSel][i][0] * DRAWSIZE, currMoves[moveSel][i][1] * DRAWSIZE), new Point(DRAWSIZE, DRAWSIZE));
                        spriteBatch.Draw(tex, rect, Color.White);
                    }
                }
            }
            else if (currMType == MoveType.free)
            {
                for (int i = 0; i < currMoves.Count; i++)
                {
                    byte[][] curr = currMoves[i];
                    for (int j = 0; j < curr.Length; j++)
                    {
                        // Test if replacing position of piece selected
                        if (curr[j][2] == currSel[0] && curr[j][3] == currSel[1] &&
                            // Test if destination placement is at freeSel
                            freeSel[0] == curr[j][0] && freeSel[1] == curr[j][1])
                        {
                            tex = Content.Load<Texture2D>("moveable");
                            rect = new Rectangle(new Point(freeSel[0] * DRAWSIZE, freeSel[1] * DRAWSIZE), new Point(DRAWSIZE, DRAWSIZE));
                            spriteBatch.Draw(tex, rect, Color.White);
                            return;
                        }
                    }
                }
                tex = Content.Load<Texture2D>("blank");
                rect = new Rectangle(new Point(freeSel[0] * DRAWSIZE, freeSel[1] * DRAWSIZE), new Point(DRAWSIZE, DRAWSIZE));
                spriteBatch.Draw(tex, rect, Color.White);
                return;
            }
        }

        void PrintSel(String file)
        {
            // Prints the piece in question
            Texture2D tex = Content.Load<Texture2D>(file);
            Rectangle rect = new Rectangle(new Point(currSel[0] * DRAWSIZE, currSel[1] * DRAWSIZE), new Point(DRAWSIZE, DRAWSIZE));
            spriteBatch.Draw(tex, rect, Color.White);
        }

        void PrintBoard()
        {
            Texture2D tex;
            Rectangle rect;

            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    tex = Content.Load<Texture2D>("empty_" + ((x + y) % 2));
                    rect = new Rectangle(new Point(x * DRAWSIZE, y * DRAWSIZE), new Point(DRAWSIZE, DRAWSIZE));
                    spriteBatch.Draw(tex, rect, Color.White);
                }
            }
        }

        void PrintPieces()
        {
            Texture2D tex;
            Rectangle rect;

            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x,y] != null)
                    {
                        tex = Content.Load<Texture2D>(board[x,y].tile);
                        rect = new Rectangle(new Point(x * DRAWSIZE, y * DRAWSIZE), new Point(DRAWSIZE, DRAWSIZE));
                        spriteBatch.Draw(tex, rect, Color.White);
                    }
                }
            }
        }

        
        void PrintMovement(byte x, byte y)
        {
            Texture2D tex;
            Rectangle rect;

            if (board[x,y] != null)
            {
                if (board[x,y].team != currTurn) { return; }

                List<byte[][]> moves = board[x, y].GetSafeMove(board);
                int moveCount = moves.Count;
                for (int i = 0; i < moveCount; i++)
                {
                    byte[][] curr = moves[0];
                    moves.RemoveAt(0);

                    for (int j = 0; j < curr.Length; j++)
                    {
                        if (curr[j][2] == byte.MaxValue && curr[j][3] == byte.MaxValue)
                        {
                            tex = Content.Load<Texture2D>("captureable");
                            rect = new Rectangle(new Point(curr[j][0] * DRAWSIZE, curr[j][1] * DRAWSIZE), new Point(DRAWSIZE, DRAWSIZE));
                            spriteBatch.Draw(tex, rect, Color.White);
                        }
                        else if (curr[j][2] == byte.MaxValue)
                        {
                            continue;
                        }
                        else if (curr[j][2] == byte.MaxValue - 1)
                        {
                            tex = Content.Load<Texture2D>("castle");
                            rect = new Rectangle(new Point(curr[j][0] * DRAWSIZE, curr[j][1] * DRAWSIZE), new Point(DRAWSIZE, DRAWSIZE));
                            spriteBatch.Draw(tex, rect, Color.White);
                        }
                        else
                        {
                            tex = Content.Load<Texture2D>("moveable");
                            rect = new Rectangle(new Point(curr[j][0] * DRAWSIZE, curr[j][1] * DRAWSIZE), new Point(DRAWSIZE, DRAWSIZE));
                            spriteBatch.Draw(tex, rect, Color.White);
                        }
                    }
                }
            }
        }
        

        void PrintPromote()
        {
            int middleX = graphics.PreferredBackBufferWidth / 2;
            int middleY = graphics.PreferredBackBufferHeight / 2;

            Texture2D tex;
            Rectangle rect;

            List<Type> prom = promotee.prom;
            for (int i = 0; i < prom.Count; i++)
            {
                ConstructorInfo ci = prom[i].GetConstructor(new Type[] { typeof(byte) });
                Piece p = (Piece)(ci.Invoke(new object[] { currTurn }));
                tex = Content.Load<Texture2D>(p.tile);
                rect = new Rectangle(new Point(middleX - (prom.Count * 32) + (i * DRAWSIZE), middleY-32), new Point(DRAWSIZE, DRAWSIZE));
                spriteBatch.Draw(tex, rect, Color.White);
            }
            tex = Content.Load<Texture2D>("selected");
            rect = new Rectangle(new Point(middleX - (prom.Count * 32) + (promoteSel * DRAWSIZE), middleY - 32), new Point(DRAWSIZE, DRAWSIZE));
            spriteBatch.Draw(tex, rect, Color.White);
        }

        internal static byte[] GetPos(byte team, Type t, Piece[,] copy)
        {
            for (byte i = 0; i < copy.GetLength(0); i++)
            {
                for (byte j = 0; j < copy.GetLength(1); j++)
                {
                    if (copy[i,j] != null)
                    {
                        if (copy[i, j].team == team)
                        {
                            if (copy[i, j].GetType().Equals(t))
                            {
                                return new byte[] { i, j };
                            }
                        }
                    }
                }
            }
            return new byte[]{ Byte.MaxValue, Byte.MaxValue};
        }

        internal static bool KingInCheck(byte te, Piece[,] copy)
        {
            byte[] kingPos = GetPos(te, typeof(King), copy);

            for (int x = 0; x < copy.GetLength(0); x++)
            {
                for (int y = 0; y < copy.GetLength(1); y++)
                {
                    if (copy[x,y] != null)
                    {
                        if (copy[x,y].team != te)
                        {
                            List<byte[][]> moveSet = copy[x, y].GetAttack(copy);
                            for (int i = 0; i < moveSet.Count; i++)
                            {
                                for (int j = 0; j < moveSet[i].Length; j++)
                                {
                                    if (moveSet[i][j][0] == kingPos[0] && moveSet[i][j][1] == kingPos[1] && moveSet[i][j][2] == byte.MaxValue && moveSet[i][j][3] == byte.MaxValue)
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        internal static bool CanGetKingOutOfCheck()
        {
            return false;
        }

        internal static Piece[,] CopyBoard(Piece[,] copy)
        {
            Piece[,] newB = new Piece[copy.GetLength(0), copy.GetLength(1)];
            for (int x = 0; x < copy.GetLength(0); x++)
            {
                for (int y = 0; y < copy.GetLength(1); y++)
                {
                    newB[x, y] = copy[x, y];
                }
            }
            return newB;
        }

        internal static Piece[,] GetDefault()
        {
            Piece[,] b = new Piece[8,8];

            // Big Pieces
            for (int i = 0; i < 4; i++)
            {
                b[((int)(i / 2)) * 7, ((int)(i % 2)) * 7] = new Rook((byte)(i % 2));
            }
            for (int i = 0; i < 4; i++)
            {
                b[((int)(i / 2)) * 5 + 1, ((int)(i % 2)) * 7] = new Knight((byte)(i % 2));
            }
            for (int i = 0; i < 4; i++)
            {
                b[((int)(i / 2)) * 3 + 2, ((int)(i % 2)) * 7] = new Bishop((byte)(i % 2));
            }

            for (byte i = 0; i < 2; i++)
            {
                b[3, (i * 7)] = new King(i);
                b[4, (i*7)] = new Queen(i);
            }

            // Pawns
            for (int i = 0; i < 16; i++)
            {
                b[i % 8, (((int)(i / 8)) * 5) + 1] = new Pawn((byte)((int)(i / 8)));
            }

            return b;
        }
    }
}

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
        readonly byte[] currSel;
        readonly byte[] moveSel;
        byte currTurn;

        byte promoteSel;
        Piece promotee;

        byte winner;

        GameState currState;

        public enum GameState
        {
            boardSel, boardMove, blank, prom
        }

        readonly List<Keys> accountedKeys;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            board = new Piece[8, 8];
            currSel = new byte[] { 0, 0, 0, 0 };
            moveSel = new byte[] { 0, 0, 0, 0 };
            currTurn = 0;
            currState = GameState.boardSel;

            promoteSel = 0;
            promotee = null;
            winner = 0;

            board[2, 6] = new Pawn(0);
            board[3, 5] = new Pawn(1);

            //board = GetDefault();

            accountedKeys = new List<Keys>();
        }
        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferWidth = (64 * 8);
            graphics.PreferredBackBufferHeight = (64 * 8);
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

            switch (currState)
            {
                case GameState.boardMove:
                    {
                        if (newKeys.Contains(Keys.A))
                        {
                            moveSel[0] = (byte)Math.Max(0, moveSel[0] - 1);
                        }
                        if (newKeys.Contains(Keys.D))
                        {
                            moveSel[0] = (byte)Math.Min(board.GetLength(0) - 1, moveSel[0] + 1);
                        }
                        if (newKeys.Contains(Keys.S))
                        {
                            moveSel[1] = (byte)Math.Min(board.GetLength(1) - 1, moveSel[1] + 1);
                        }
                        if (newKeys.Contains(Keys.W))
                        {
                            moveSel[1] = (byte)Math.Max(0, moveSel[1] - 1);
                        }
                        if (newKeys.Contains(Keys.Enter))
                        {
                            MoveToChoice();
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
                            moveSel[0] = (byte)Math.Max(0, currSel[0] - 1);
                            currSel[0] = (byte)Math.Max(0, currSel[0] - 1);
                        }
                        if (newKeys.Contains(Keys.D))
                        {
                            moveSel[0] = (byte)Math.Min(board.GetLength(0) - 1, currSel[0] + 1);
                            currSel[0] = (byte)Math.Min(board.GetLength(0) - 1, currSel[0] + 1);
                        }
                        if (newKeys.Contains(Keys.S))
                        {
                            moveSel[1] = (byte)Math.Min(board.GetLength(1) - 1, currSel[1] + 1);
                            currSel[1] = (byte)Math.Min(board.GetLength(1) - 1, currSel[1] + 1);
                        }
                        if (newKeys.Contains(Keys.W))
                        {
                            moveSel[1] = (byte)Math.Max(0, currSel[1] - 1);
                            currSel[1] = (byte)Math.Max(0, currSel[1] - 1);
                        }
                        if (newKeys.Contains(Keys.Enter))
                        {
                            if (board[currSel[0], currSel[1]] != null)
                            {
                                if (board[moveSel[0], moveSel[1]].team == currTurn)
                                {
                                    currState = GameState.boardMove;
                                }
                            }
                        }
                        break;
                    }
                case GameState.prom:
                    {
                        if (newKeys.Contains(Keys.A))
                        {
                            promoteSel = (byte)Math.Max(0, promoteSel - 1);
                        }
                        if (newKeys.Contains(Keys.D))
                        {
                            promoteSel = (byte)Math.Min(promoteSel + 1, promotee.prom.Count - 1);
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
            List<byte[][]> moves = board[currSel[0], currSel[1]].GetMove(board);
            for (int i = 0; i < moves.Count; i++)
            {
                byte[][] curr = moves[i];
                if (curr[curr.Length - 2][0] == moveSel[0] && curr[curr.Length - 2][1] == moveSel[1])
                {
                    for (int j = 0; j < curr.GetLength(0); j++)
                    {
                        if (curr[j][2] == byte.MaxValue && curr[j][3] == byte.MaxValue)
                        {
                            board[curr[j][0], curr[j][1]] = null;
                        }
                        else if (curr[j][2] == byte.MaxValue)
                        {
                            byte code = (byte)(byte.MaxValue - curr[j][3]);
                            board[curr[j][0], curr[j][1]] = null;
                            GameState tempState = board[curr[j - 1][0], curr[j - 1][1]].Move(board, code);
                            if (tempState == GameState.prom)
                            {
                                promotee = board[curr[j - 1][0], curr[j - 1][1]];
                                promoteSel = 0;
                                currState = tempState;
                            }
                        }
                        else
                        {
                            board[curr[j][0], curr[j][1]] = board[curr[j][2], curr[j][3]];
                        }
                    }
                    currSel[0] = moveSel[0];
                    currSel[1] = moveSel[1];
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


                    break;
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
                        PrintMovement(currSel[0], currSel[1]);
                        PrintSel("selected");
                        break;
                    }
                case GameState.boardMove:
                    {
                        PrintBoard();
                        PrintPieces();
                        PrintMovement(currSel[0], currSel[1]);
                        PrintSel("choice");
                        PrintMove("selected");
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

        void PrintMove(String file)
        { 
            Texture2D tex = Content.Load<Texture2D>(file);
        Rectangle rect = new Rectangle(new Point(moveSel[0] * 64, moveSel[1] * 64), new Point(64, 64));
        spriteBatch.Draw(tex, rect, Color.White);
        }

        void PrintSel(String file)
        {
            Texture2D tex = Content.Load<Texture2D>(file);
            Rectangle rect = new Rectangle(new Point(currSel[0] * 64, currSel[1] * 64), new Point(64, 64));
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
                    rect = new Rectangle(new Point(x * 64, y * 64), new Point(64, 64));
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
                        rect = new Rectangle(new Point(x * 64, y * 64), new Point(64, 64));
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
                            rect = new Rectangle(new Point(curr[j][0] * 64, curr[j][1] * 64), new Point(64, 64));
                            spriteBatch.Draw(tex, rect, Color.White);
                        }
                        else if (curr[j][2] == byte.MaxValue)
                        {
                            continue;
                        }
                        else
                        {
                            tex = Content.Load<Texture2D>("moveable");
                            rect = new Rectangle(new Point(curr[j][0] * 64, curr[j][1] * 64), new Point(64, 64));
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
                rect = new Rectangle(new Point(middleX - (prom.Count * 32) + (i * 64), middleY-32), new Point(64, 64));
                spriteBatch.Draw(tex, rect, Color.White);
            }
            tex = Content.Load<Texture2D>("selected");
            rect = new Rectangle(new Point(middleX - (prom.Count * 32) + (promoteSel * 64), middleY - 32), new Point(64, 64));
            spriteBatch.Draw(tex, rect, Color.White);
        }

        internal static byte[] GetPos(byte team, Type t, Piece[,] copy)
        {
            for (byte i = 0; i < copy.GetLength(0); i++)
            {
                for (byte j = 0; j < copy.GetLength(0); j++)
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
                            List<byte[][]> moveSet = copy[x, y].GetMove(copy);
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

        internal static bool[,] ORAll(bool[,] one, bool[,] two)
        {
            bool[,] three = new bool[one.GetLength(0), two.GetLength(1)];
            for (int i = 0; i < one.GetLength(0); i++)
            {
                for (int j = 0; j < one.GetLength(1); j++)
                {
                    three[i, j] = (one[i, j] || two[i, j]);
                }
            }
            return three;
        }

        internal static bool[,] AllMovesFromTeam(int te, Piece[,] copy)
        {
            bool[,] all = new bool[copy.GetLength(0), copy.GetLength(1)];

            return all;
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

            b[3, 0] = new King(0);
            b[4, 0] = new Queen(0);
            b[3, 7] = new King(1);
            b[4, 7] = new Queen(1);

            // Pawns
            for (int i = 0; i < 16; i++)
            {
                b[i % 8, (((int)(i / 8)) * 5) + 1] = new Pawn((byte)((int)(i / 8)));
            }

            return b;
        }
    }
}

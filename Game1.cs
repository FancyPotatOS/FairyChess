using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Chess
{

    // TODO: Castling
    // En Passant
    // Cannot move King into check move
    // King is in check/checkmate check
    // 


        // Assume 0 means headed +Y

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        readonly Piece[,] board;
        readonly byte[] currSel;
        byte currTurn;

        byte promoteSel;
        Piece promotee;

        byte winner;

        GameState currState;

        enum GameState
        {
            boardSel, boardMove, promote,
            won
        }

        readonly List<Keys> accountedKeys;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            board = new Piece[8, 8];
            currSel = new byte[] { 0, 0, 0, 0};
            currTurn = 0;
            currState = GameState.boardSel;

            promoteSel = 0;
            promotee = null;
            winner = 0;

            board[3, 0] = new King(0);
            board[0, 0] = new Rook(0);
            board[7, 0] = new Rook(0);

            board[1, 7] = new Rook(1);
            board[7, 7] = new King(1);

            //board = GetDefault();

            accountedKeys = new List<Keys>();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            graphics.PreferredBackBufferWidth = (64 * 8);
            graphics.PreferredBackBufferHeight = (64 * 8);
            //this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
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

            if (currState != GameState.won)
            {

                if (!CanGetKingOutOfCheck())
                {
                    currState = GameState.won;
                    winner = (byte)((currTurn + 1) % 2);
                }
            }

            switch (currState)
            {
                case GameState.boardMove:
                    {
                        if (newKeys.Contains(Keys.W))
                        {
                            currSel[3] = (byte)Math.Max(0, currSel[3] - 1);
                        }
                        if (newKeys.Contains(Keys.S))
                        {
                            currSel[3] = (byte)Math.Min((board.GetLength(1) - 1), currSel[3] + 1);
                        }
                        if (newKeys.Contains(Keys.A))
                        {
                            currSel[2] = (byte)Math.Max(0, currSel[2] - 1);
                        }
                        if (newKeys.Contains(Keys.D))
                        {
                            currSel[2] = (byte)Math.Min((board.GetLength(0) - 1), currSel[2] + 1);
                        }
                        if (newKeys.Contains(Keys.C))
                        {
                            Castle();
                        }

                        if (newKeys.Contains(Keys.Enter))
                        {
                            if (board[currSel[0], currSel[1]] != null)
                            {
                                if (board[currSel[0], currSel[1]].team == currTurn)
                                {
                                    MoveToChoice();
                                }
                            }
                        }
                        else if (newKeys.Contains(Keys.Escape))
                        {
                            currState = GameState.boardSel;
                            currSel[2] = currSel[0];
                            currSel[3] = currSel[1];
                        }
                        break;
                    }
                case GameState.boardSel:
                    {
                        if (newKeys.Contains(Keys.W))
                        {
                            currSel[1] = (byte)Math.Max(0, currSel[1] - 1);
                            currSel[3] = currSel[1];
                        }
                        if (newKeys.Contains(Keys.S))
                        {
                            currSel[1] = (byte)Math.Min((board.GetLength(1) - 1), currSel[1] + 1);
                            currSel[3] = currSel[1];
                        }
                        if (newKeys.Contains(Keys.A))
                        {
                            currSel[0] = (byte)Math.Max(0, currSel[0] - 1);
                            currSel[2] = currSel[0];
                        }
                        if (newKeys.Contains(Keys.D))
                        {
                            currSel[0] = (byte)Math.Min((board.GetLength(0) - 1), currSel[0] + 1);
                            currSel[2] = currSel[0];
                        }
                        if (newKeys.Contains(Keys.E))
                        {
                            { }
                        }
                        if (newKeys.Contains(Keys.Enter))
                        {
                            if (board[currSel[0], currSel[1]] != null)
                            {
                                if (board[currSel[0], currSel[1]].team == currTurn)
                                {
                                    currState = GameState.boardMove;
                                }
                            }
                        }
                        break;
                    }
                case GameState.promote:
                    {
                        if (newKeys.Contains(Keys.A))
                        {
                            promoteSel += (byte)(promotee.prom.Count - 1);
                            promoteSel %= (byte)promotee.prom.Count;
                        }
                        if (newKeys.Contains(Keys.D))
                        {
                            promoteSel += 1;
                            promoteSel %= (byte)promotee.prom.Count;
                        }
                        if (newKeys.Contains(Keys.Enter))
                        {
                            byte[] pos = GetPos((byte)((currTurn + 1) % 2), promotee.GetType(), board);
                            board[pos[0], pos[1]] = (Piece)promotee.prom[promoteSel].GetConstructor(new Type[] { typeof(byte) }).Invoke(new object[] { ((byte)((currTurn + 1) % 2)) });
                            promotee = null;
                            promoteSel = 0;
                            currState = GameState.boardSel;
                        }
                        break;
                    }
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        void Castle()
        {
            if (board[currSel[2], currSel[3]] != null)
            if (board[currSel[2], currSel[3]].GetType().Equals(typeof(Rook)))
            {
                Rook rook = (Rook)board[currSel[2], currSel[3]];
                byte[] castleMove = rook.GetCastle(board);
                if (castleMove != null)
                {
                    byte[] kingCoord = GetPos(currTurn, typeof(King), board);
                    Piece[,] copy = CopyBoard();
                    copy[castleMove[0], castleMove[1]] = copy[currSel[2], currSel[3]];
                    copy[currSel[2], currSel[3]] = null;
                    sbyte dX = (sbyte)((castleMove[0] - currSel[0]) / Math.Abs(castleMove[0] - currSel[0]));
                    dX *= -2;
                    copy[kingCoord[0] + dX, kingCoord[1]] = copy[kingCoord[0], kingCoord[1]];
                    copy[kingCoord[0], kingCoord[1]] = null;
                    if (!KingInCheck(currTurn, copy))
                    {
                        board[castleMove[0], castleMove[1]] = board[currSel[2], currSel[3]];
                        board[currSel[2], currSel[3]] = null;
                        board[kingCoord[0] + dX, kingCoord[1]] = board[kingCoord[0], kingCoord[1]];
                        board[kingCoord[0], kingCoord[1]] = null;
                        
                        currTurn += 1;
                        currTurn %= 2;
                    }
                }
            }
        }

        void MoveToChoice()
        {
            if (board[currSel[0], currSel[1]].GetMove(board)[currSel[2], currSel[3]])
            {
                // Move piece in dummy board
                Piece[,] copy = CopyBoard();
                copy[currSel[2], currSel[3]] = copy[currSel[0], currSel[1]];
                copy[currSel[0], currSel[1]] = null;
                if (!KingInCheck(currTurn, copy))
                {
                    board[currSel[0], currSel[1]].Move();

                    if (board[currSel[0], currSel[1]].GetType().Equals(typeof(Pawn)))
                    {

                        for (int i = 0; i < board.GetLength(0); i++)
                        {
                            for (int j = 0; j < board.GetLength(1); j++)
                            {
                                if (board[i, j] == null)
                                {
                                    continue;
                                }
                                else if (board[i, j].GetType() == typeof(Pawn))
                                {
                                    ((Pawn)board[i, j]).movedTwice = false;
                                }
                            }
                        }

                        if (Math.Abs(currSel[3] - currSel[1]) == 2)
                        {
                            ((Pawn)board[currSel[0], currSel[1]]).movedTwice = true;
                        }
                        // Is En Passant
                        else if (currSel[2] - currSel[0] != 0)
                        {
                            sbyte dirX = (sbyte)(currSel[2] - currSel[0]);
                            board[currSel[0] + dirX, currSel[1]] = null;
                        }
                        // Move Piece
                        {
                            board[currSel[2], currSel[3]] = board[currSel[0], currSel[1]];
                            board[currSel[0], currSel[1]] = null;
                            currTurn = (byte)((currTurn + 1) % 2);
                            currSel[0] = currSel[2];
                            currSel[1] = currSel[3];
                            currState = GameState.boardSel;
                        }
                        sbyte opposite = (sbyte)(((board[currSel[0], currSel[1]].team + 1) % 2) * 7);
                        if (currSel[1] == opposite)
                        {
                            promotee = board[currSel[0], currSel[1]];
                            currState = GameState.promote;
                        }
                    }
                    else
                    {
                        board[currSel[2], currSel[3]] = board[currSel[0], currSel[1]];
                        board[currSel[0], currSel[1]] = null;
                        currTurn = (byte)((currTurn + 1) % 2);
                        currSel[0] = currSel[2];
                        currSel[1] = currSel[3];
                        currState = GameState.boardSel;

                        for (int i = 0; i < board.GetLength(0); i++)
                        {
                            for (int j = 0; j < board.GetLength(1); j++)
                            {
                                if (board[i, j] == null)
                                {
                                    continue;
                                }
                                board[i, j].Move();
                            }
                        }
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
                        break;
                    }
                case GameState.boardMove:
                    {
                        PrintMove();
                        break;
                    }
                case GameState.promote:
                    {
                        PrintPromote();
                        break;
                    }
                case GameState.won:
                    {
                        PrintWon();
                        break;
                    }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        void PrintWon()
        {
            currSel[0] = (byte)board.GetLength(0);
            currSel[1] = (byte)board.GetLength(1);
            PrintBoard();
            Texture2D tex = Content.Load<Texture2D>("won_" + (currTurn + 1) % 2);
            Rectangle rect = new Rectangle(new Point((graphics.PreferredBackBufferHeight/2) - (tex.Width / 2), (graphics.PreferredBackBufferWidth/2) - (tex.Height / 2)), new Point(tex.Width, tex.Height));
            spriteBatch.Draw(tex, rect, Color.White);

            tex = Content.Load<Texture2D>("instruc");
            rect = new Rectangle(new Point((graphics.PreferredBackBufferHeight/2) - (tex.Width / 2), (graphics.PreferredBackBufferWidth/2) + (tex.Height / 2)), new Point(tex.Width, tex.Height));
            spriteBatch.Draw(tex, rect, Color.White);
        }

        void PrintMove()
        {
            PrintBoard();
            PrintSel("choice");
            PrintMove("selected");
        }

        void PrintSel(String file)
        {
            Texture2D tex;
            Rectangle rect;
            // Highlight currently selected
            tex = Content.Load<Texture2D>(file);
            rect = new Rectangle(new Point(currSel[0] * 64, currSel[1] * 64), new Point(64, 64));
            spriteBatch.Draw(tex, rect, Color.White);
        }

        void PrintMove(String file)
        {
            Texture2D tex;
            Rectangle rect;
            // Highlight currently selected
            tex = Content.Load<Texture2D>(file);
            rect = new Rectangle(new Point(currSel[2] * 64, currSel[3] * 64), new Point(64, 64));
            spriteBatch.Draw(tex, rect, Color.White);
        }

        void PrintBoard()
        {
            Texture2D tex;
            Rectangle rect;
            // Cycle through board
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    // No piece: print tile
                    if (board[i, j] == null)
                    {
                        tex = Content.Load<Texture2D>("empty_" + (i + j) % 2);
                        rect = new Rectangle(new Point(i * 64, j * 64), new Point(64, 64));
                        spriteBatch.Draw(tex, rect, Color.White);
                    }
                    // Piece: print tile then piece
                    else
                    {
                        tex = Content.Load<Texture2D>("empty_" + (i + j) % 2);
                        rect = new Rectangle(new Point(i * 64, j * 64), new Point(64, 64));
                        spriteBatch.Draw(tex, rect, Color.White);

                        tex = Content.Load<Texture2D>(board[i, j].tile);
                        rect = new Rectangle(new Point(i * 64, j * 64), new Point(64, 64));
                        spriteBatch.Draw(tex, rect, Color.White);
                    }
                }
            }

            PrintSel("selected");

            // Print all available positions
            if (currSel[0] >= board.GetLength(0) || currSel[1] >= board.GetLength(1))
            {
                return;
            }
            if (board[currSel[0], currSel[1]] != null)
            {
                if (board[currSel[0], currSel[1]].team == currTurn)
                {
                    // Get spots piece can move
                    bool[,] visible = board[currSel[0], currSel[1]].GetMove(board);
                    for (int x = 0; x < board.GetLength(0); x++)
                    {
                        for (int y = 0; y < board.GetLength(1); y++)
                        {
                            if (visible[x, y])
                            {
                                // Check if position is okay to move without endangering king
                                Piece[,] copy = CopyBoard();
                                copy[x, y] = copy[currSel[0], currSel[1]];
                                copy[currSel[0], currSel[1]] = null;

                                if (!KingInCheck(currTurn, copy))
                                {
                                    if (board[x, y] != null)
                                    {
                                        tex = Content.Load<Texture2D>("captureable");
                                        rect = new Rectangle(new Point(x * 64, y * 64), new Point(64, 64));
                                        spriteBatch.Draw(tex, rect, Color.White);
                                    }
                                    else
                                    {
                                        tex = Content.Load<Texture2D>("moveable");
                                        rect = new Rectangle(new Point(x * 64, y * 64), new Point(64, 64));
                                        spriteBatch.Draw(tex, rect, Color.White);
                                    }
                                }
                            }
                        }
                    }

                    // Castle Print
                    if (board[currSel[2], currSel[3]] != null)
                    if (board[currSel[2], currSel[3]].GetType().Equals(typeof(Rook)))
                    {
                        Rook rook = (Rook)board[currSel[2], currSel[3]];
                        byte[] castleMove = rook.GetCastle(board);
                        if (castleMove != null)
                        {
                            byte[] kingCoord = GetPos(currTurn, typeof(King), board);
                            Piece[,] copy = CopyBoard();
                            copy[castleMove[0], castleMove[1]] = copy[currSel[2], currSel[3]];
                            copy[currSel[2], currSel[3]] = null;
                            sbyte dX = (sbyte)((castleMove[0] - currSel[0]) / Math.Abs(castleMove[0] - currSel[0]));
                            dX *= -2;
                            copy[kingCoord[0] + dX, kingCoord[1]] = copy[kingCoord[0], kingCoord[1]];
                            copy[kingCoord[0], kingCoord[1]] = null;
                            if (!KingInCheck(currTurn, copy))
                            {
                                tex = Content.Load<Texture2D>("castle");
                                rect = new Rectangle(new Point(castleMove[0] * 64, castleMove[1] * 64), new Point(64, 64));
                                spriteBatch.Draw(tex, rect, Color.White);
                            }
                        }
                    }
                }
            }

            if (currTurn == 0)
            {
                tex = Content.Load<Texture2D>("castle");
                rect = new Rectangle(new Point(64, 64), new Point(64, 64));
                spriteBatch.Draw(tex, rect, Color.White);
            }
        }

        void PrintPromote()
        {

            Texture2D tex;
            Rectangle rect;

            // Cycle through board
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    // No piece: print tile
                    if (board[i, j] == null)
                    {
                        tex = Content.Load<Texture2D>("empty_" + (i + j) % 2);
                        rect = new Rectangle(new Point(i * 64, j * 64), new Point(64, 64));
                        spriteBatch.Draw(tex, rect, Color.Gray);
                    }
                    // Piece: print tile then piece
                    else
                    {
                        tex = Content.Load<Texture2D>("empty_" + (i + j) % 2);
                        rect = new Rectangle(new Point(i * 64, j * 64), new Point(64, 64));
                        spriteBatch.Draw(tex, rect, Color.Gray);
                    }
                }
            }

            int middleX = graphics.PreferredBackBufferWidth / 2;
            int middleY = graphics.PreferredBackBufferHeight / 2;
            List<Type> prom = promotee.prom;
            
            for (int i = 0; i < prom.Count; i++)
            {
                ConstructorInfo cI = prom[i].GetConstructor(new Type[] { typeof(byte) });
                Piece p = (Piece)cI.Invoke(new object[] { ((byte)((currTurn + 1) % 2)) });
                tex = Content.Load<Texture2D>(p.tile);
                rect = new Rectangle(new Point((middleX - (prom.Count*32) + (i*64)), middleY - 32), new Point(64, 64));
                spriteBatch.Draw(tex, rect, Color.White);
            }

            tex = Content.Load<Texture2D>("selected");
            rect = new Rectangle(new Point((middleX - (prom.Count * 32) + (promoteSel * 64)), middleY - 32), new Point(64, 64));
            spriteBatch.Draw(tex, rect, Color.White);
        }

        byte[] GetPos(byte team, Type t, Piece[,] copy)
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
        
        bool KingCanMove(byte te)
        {
            Piece[,] copy = CopyBoard();
            byte[] kingPos = GetPos(te, typeof(King), copy);
            if (kingPos[0] == byte.MaxValue || kingPos[1] == byte.MaxValue) { return false; }

            bool[,] kingMove = copy[kingPos[0], kingPos[1]].GetMove(copy);
            for(int i = 0; i < kingMove.GetLength(0); i++)
            {
                for (int j = 0; j < kingMove.GetLength(0); j++)
                {
                    if (kingMove[i,j])
                    {
                        Piece hold = copy[i, j];
                        copy[i, j] = copy[kingPos[0], kingPos[1]];
                        copy[kingPos[0], kingPos[1]] = null;
                        if (!KingInCheck(te, copy)) { return true; }
                    }
                }
            }
            return false;
        }

        bool KingInCheck(byte te, Piece[,] copy)
        {
            byte[] kingPos = GetPos(te, typeof(King), copy);
            if (kingPos[0] == byte.MaxValue || kingPos[1] == byte.MaxValue)
            {
                return false;
            }

            // Checks if position's move can attack king
            for (int cx = 0; cx < copy.GetLength(0); cx++)
            {
                for (int cy = 0; cy < copy.GetLength(1); cy++)
                {
                    if (copy[cx, cy] != null)
                    {
                        if (copy[cx, cy].team != te)
                        {
                            if (copy[cx, cy].GetMove(copy)[kingPos[0], kingPos[1]])
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        bool CanGetKingOutOfCheck()
        {
            Piece[,] copy = CopyBoard();
            for (int x = 0; x < copy.GetLength(0); x++)
            {
                for (int y = 0; y < copy.GetLength(0); y++)
                {
                    if (copy[x,y] != null)
                    {
                        if (copy[x,y].team == currTurn)
                        {
                            bool[,] moves = copy[x, y].GetMove(copy);
                            for (int i = 0; i < moves.GetLength(0); i++)
                            {
                                for (int j = 0; j < moves.GetLength(0); j++)
                                {
                                    if (moves[i,j])
                                    {
                                        Piece hold = copy[i, j];
                                        copy[i, j] = copy[x, y];
                                        copy[x, y] = null;
                                        if (!KingInCheck(currTurn, copy))
                                        {
                                            return true;
                                        }
                                        copy[x, y] = copy[i, j];
                                        copy[i, j] = hold;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        bool[,] ORAll(bool[,] one, bool[,] two)
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

        bool[,] AllMovesFromTeam(int te, Piece[,] copy)
        {
            bool[,] all = new bool[copy.GetLength(0), copy.GetLength(1)];
            for (int x = 0; x < copy.GetLength(0); x++)
            {
                for (int y = 0; y < copy.GetLength(1); y++)
                {
                    if (copy[x, y] != null)
                    {
                        if (copy[x, y].team == te)
                        {
                            all = ORAll(all, copy[x, y].GetMove(copy));
                        }
                    }
                }
            }
            return all;
        }

        Piece[,] CopyBoard()
        {
            Piece[,] newB = new Piece[board.GetLength(0), board.GetLength(1)];
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    newB[x, y] = board[x, y];
                }
            }
            return newB;
        }

        Piece[,] GetDefault()
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

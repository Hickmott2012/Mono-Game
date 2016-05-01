//Programmer: Benjamin Hickmott
//Progject Name: UnderDevolopment
//Start Date:
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace War_Game
{
    #region TODO's
    /// <summary>
    /// ----------------------------TODO's---------------------------------\\\
    /// Things to add to the game:                                         \\\
    ///     map that is zoomed in and scrlls to reveal more                \\\
    ///                                                                    \\\
    /// Things to make the game run better:                                \\\
    ///     scale size working                                             \\\
    ///                                                                    \\\
    /// Things to make the game look better                                \\\
    /// add shooting frame in player                                       \\\
    /// add diying frame                                                   \\\
    ///                                                                    \\\
    /// Move the bullet to its own class                                   \\\
    /// Create Enemy                                                       \\\
    ///                                                                    \\\
    ///--------------------------------------------------------------------\\\                                                         
    /// </summary>   
    #endregion

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        #region Declarations
        //Global Declaring
        private const int SCREEN_WIDTH = VIEW_PORT_WIDTH * TILE_SIZE + 2 * SPACEING;
        private const int SCREEN_LENGTH = (VIEW_PORT_LENGTH - 1) * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + 2 * SPACEING;
        private const int TILE_SIZE = 30;
        private const int SPACEING = 5;
        private const int VIEW_PORT_WIDTH = 30;
        private const int VIEW_PORT_LENGTH = 20;
        private const int GAME_BOARD_WIDTH = 300;
        private const int GAME_BOARD_LENGTH = 200;
        //Game Board TopLeft Location
        private Point GAMEBOARD_TOP_LEFT_POINT = new Point(SPACEING, GameStatusBar.STATUSBAR_LENGTH + SPACEING);
        //Amount the player can move before the tile changes
        private const int MOVEMENTAMOUNT = 30;
        private Point playerStepsMoved = new Point(0, 0);
        Rectangle rectangleSizeForTop, rectangleSizeForBottom, rectangleSizeForLeft, rectangleSizeForRight, rectangleSizeForTopLeft, rectangleSizeForTopRight, rectangleSizeForBottomLeft, rectangleSizeForBottomRight;
        Vector2 TopLeftTilePosition, TopRightTilePosition, BottomLeftTilePosition, bottomRightTilePosition;
        int counter = 0;

        //Game States
        private enum GameState
        {
            MainMenu,
            Gameplay,
            EndOfGame,
        }
        //Current Game State
        private GameState _state;
        //Mono Game Graphic and SpriteBatch loader
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        #region Mouse
        private MouseState mouseState;
        private MouseState previousMouseState;
        #endregion
        
        #region Main Menu
        private Texture2D startButton;
        private Texture2D exitButton;
        private Texture2D smallButton;
        private Texture2D mediumButton;
        private Texture2D largeButton;
        private Texture2D smallButtonClicked;
        private Texture2D mediumButtonClicked;
        private Texture2D largeButtonClicked;
        private Vector2 startButtonPosition;
        private Vector2 exitButtonPosition;
        private Vector2 smallButtonPosition;
        private Vector2 mediumButtonPosition;
        private Vector2 largeButtonPosition;
        private bool smallButtonShow = true;
        private bool mediumButtonShow = true;
        private bool largeButtonShow = true;
        private bool smallButtonClickedShow = false;
        private bool mediumButtonClickedShow = false;
        private bool largeButtonClickedShow = false;
        //Font Style
        private SpriteFont font;
        #endregion

        #region Game Status Bar
        private GameStatusBar statusBar;
        #endregion

        #region Sprite Location Strings
        //Game Board
        private const string FINISH_TILE_NAME = "Game Board/finishTile.png";
        private const string ROCK_TILE_NAME = "Game Board/rockTile.png";
        private const string START_TILE_NAME = "Game Board/startTile.png";
        private const string GRASS_TILE_NAME = "Game Board/smallGrassTile.png";
        private const string WATER_TILE_NAME = "Game Board/smallWaterTile.png";
        private const string SAND_TILE_NAME = "Game Board/smallSandTile.png";
        //Player
        public const string PLAYER_SPRITESHEET_NAME = "Player/skeletonSpriteSheet.png";
        private const string BULLET_SPRITESHEET_NAME = "Player/bullet.png";
        #endregion

        #region GroundTexture
        //Ground Texture Array
        private Texture2D[,] groundTexture;
        //Ground Texture Type Number Array
        private int[,] groundTextureType;
        //Ground Texture Collider Array
        private Rectangle[,] groundTextureCollider;
        //Ground Map Randomizer Array
        private int[,] randomMap;
        //Random Number Generator
        private Random rnd;
        //Ground Map Size
        private int mapSize = 0;
        #endregion

        #region view port
        private Vector2 viewPortColliderOriginPosition;
        private Point viewPortTopLeftIndex;
        #endregion

        #region Player
        Player player1;
        float timeShown;
        #endregion

        #region Bullet
        //amount of players killed
        private int killCount = 0;
        //bullets shot
        private int bulletsShot;
        //load bullet texture
        private Texture2D bulletTexture;
        //bullets current position array
        private Vector2[] bulletPosition;
        //bullets rectangle collider array
        private Rectangle[] bulletCollider;
        //Bullets Shown time array
        private float[] bulletTimeShown;
        //bullet speed
        private const int BULLET_SPEED = 3;
        //number of bullets to be shot
        private const int BULLET_AMOUNT = 10;
        //Bullet Fire Rate
        private const float fireRate = .2f;
        //last time bullet was shot
        private float lastBulletShotTime = 0;
        //Is Bullet Shot array
        private bool[] isBulletShot;
        // duration of time to show each frame
        private float bulletFramePlayTime = 0.1f;
        // an index of the current frame being shown array
        public int[] bulletFrameIndex;
        // the current frame row being shown array
        public int[] bulletFrameRow;
        // total number of frames in our spritesheet animation row
        private int bulletTotalFrames = 3;
        // define the size of our animation frame
        public int bulletFrameHeight = 25;
        public int bulletFrameWidth = 25;
        //Frame Row Name with Numbers
        private const int shootingLeftFrameRow = 3;
        private const int shootingRightFrameRow = 1;
        private const int shootingUpFrameRow = 0;
        private const int shootingDownFrameRow = 2;
        //animation stuff arrays
        public Rectangle[] bulletSource;
        public Vector2[] bulletOrigin;
        //Bullet Direction
        public enum bulletDirection
        {
            Left,
            Right,
            Up,
            Down,
        }
        //Bullet Direcion Array
        public bulletDirection[] _bulletDirection;
        #endregion

        //Scaleing
        float scale = 1f;
        Matrix scalematrix;

        Rectangle boxCollider;
        Texture2D boxTexture;
        Vector2 boxPosition;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";

            
            #region Map Size
            //Set Screen Size
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = SCREEN_LENGTH;   // set this value to the desired height of your window
            graphics.ApplyChanges();
            //Creats Ground Texture Arrays
            groundTexture = new Texture2D[GAME_BOARD_WIDTH, GAME_BOARD_LENGTH];
            groundTextureType = new int[GAME_BOARD_WIDTH, GAME_BOARD_LENGTH];
            groundTextureCollider = new Rectangle[GAME_BOARD_WIDTH, GAME_BOARD_LENGTH];
            #endregion
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
            //get the mouse state
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //creats the scale for the game
            scalematrix = Matrix.CreateScale(scale, scale, 1f);
            
            #region Menu
            //Create Menu Button Locations
            startButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 200);
            exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 250);
            smallButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2)-200, 20);
            mediumButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 20);
            largeButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) + 100, 20);

            //Load the munu page button textures
            startButton = Content.Load<Texture2D>("Menu/start");
            exitButton = Content.Load<Texture2D>("Menu/exit");
            smallButton = Content.Load<Texture2D>("Menu/small");
            mediumButton = Content.Load<Texture2D>("Menu/medium");
            largeButton = Content.Load<Texture2D>("Menu/large");
            smallButtonClicked = Content.Load<Texture2D>("Menu/smallClicked");
            mediumButtonClicked = Content.Load<Texture2D>("Menu/mediumClicked");
            largeButtonClicked = Content.Load<Texture2D>("Menu/largeClicked");
            //Load Text Font
            font = Content.Load<SpriteFont>("font");
            #endregion

            #region GameStatusBar
            //Load Game Bar Font
            statusBar = new GameStatusBar();
            statusBar.GameStatusFont = Content.Load<SpriteFont>("GameMenu/gameMenuFont");
            //load game menu background
            statusBar.StatusBarBackgroundTexture = Content.Load<Texture2D>(GameStatusBar.STATUSBAR_SPRITESHEET_NAME);
            //load health bar texutre
            statusBar.HealthBarTexture = Content.Load<Texture2D>(GameStatusBar.HEALTHBAR_SPRITESHEET_NAME);
            //load ammo texutre
            statusBar.AmmoAvailableTexture = Content.Load<Texture2D>(GameStatusBar.AMMO_SPRITESHEET_NAME);
            statusBar.loadGameStatusBar(GraphicsDevice.Viewport.Width);
            #endregion

            #region Build Game Board
            //Build Random Map
            //Random Map Number Spawner
            rnd = new Random();
            randomMap = new int[GAME_BOARD_WIDTH,GAME_BOARD_LENGTH];
            
            for (int y = 0; y < GAME_BOARD_LENGTH; y++)
            {
                for (int x = 0; x < GAME_BOARD_WIDTH; x++)
                {
                    //Set land type for map
                    randomMap[x,y] = rnd.Next(0,101);
                }
            }

            //Build Game Board
            //Add Level Finish location
            int xFinishLocation = rnd.Next(1, GAME_BOARD_WIDTH - 1);
            int yFinishLocation = rnd.Next(1, GAME_BOARD_LENGTH - 1);
            groundTexture[xFinishLocation, yFinishLocation] = Content.Load<Texture2D>(FINISH_TILE_NAME);
            groundTextureType[xFinishLocation, yFinishLocation] = 4;
            groundTexture[GAME_BOARD_WIDTH / 2, GAME_BOARD_LENGTH / 2] = Content.Load<Texture2D>(START_TILE_NAME);
            groundTextureType[GAME_BOARD_WIDTH / 2, GAME_BOARD_LENGTH / 2] = 1;

            int c;
             for (int y = 0; y < GAME_BOARD_LENGTH; y++)
            {
                 c = 0;
                groundTexture[c, y] = Content.Load<Texture2D>(ROCK_TILE_NAME);
                groundTextureType[c, y] = 5;
                 c = GAME_BOARD_WIDTH - 1;
                groundTexture[c, y] = Content.Load<Texture2D>(ROCK_TILE_NAME);
                groundTextureType[c, y] = 5;
             }
             int b;
             for (int a = 0; a < GAME_BOARD_WIDTH; a++)
             {
                 b = 0;
                 groundTexture[a, b] = Content.Load<Texture2D>(ROCK_TILE_NAME);
                 groundTextureType[a, b] = 5;
                 b = GAME_BOARD_LENGTH - 1;
                 groundTexture[a, b] = Content.Load<Texture2D>(ROCK_TILE_NAME);
                 groundTextureType[a, b] = 5;
             }


            for (int y = 0; y < GAME_BOARD_LENGTH; y++)
            {
                for (int x = 0; x < GAME_BOARD_WIDTH; x++)
                {
                    if (groundTexture[x, y] == null)
                    {
                        buildGameBoard(x, y);
                    }
                }
            }
            #endregion

            #region View Port
            viewPortColliderOriginPosition.X = VIEW_PORT_WIDTH / 2.0f;
            viewPortColliderOriginPosition.Y = VIEW_PORT_LENGTH / 2.0f;
            #endregion

            #region Position Player
            //Creats a new player
            player1 = new Player(GAME_BOARD_WIDTH, GAME_BOARD_LENGTH, VIEW_PORT_WIDTH, VIEW_PORT_LENGTH, viewPortTopLeftIndex, TILE_SIZE, groundTextureType, groundTextureCollider);
            //Load in Player Texture
            player1.playerSpriteSheet = Content.Load<Texture2D>(PLAYER_SPRITESHEET_NAME);
            //ViewPort Top Left Edge Tile Location
            viewPortTopLeftIndex.X = GAME_BOARD_WIDTH / 2 - VIEW_PORT_WIDTH / 2;
            viewPortTopLeftIndex.Y = GAME_BOARD_LENGTH / 2 - VIEW_PORT_LENGTH / 2;
            //Set the potition for the player
            player1.PlayerTilePosition.X = viewPortTopLeftIndex.X + VIEW_PORT_WIDTH / 2;
            player1.PlayerTilePosition.Y = viewPortTopLeftIndex.Y + VIEW_PORT_LENGTH / 2;
            player1.PlayerViewPortPosition.X = viewPortColliderOriginPosition.X * TILE_SIZE + SPACEING;
            player1.PlayerViewPortPosition.Y = viewPortColliderOriginPosition.Y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING;
            player1.PlayerGameBoardPosition.X = GAME_BOARD_WIDTH * TILE_SIZE;
            player1.PlayerGameBoardPosition.Y = GAME_BOARD_LENGTH * TILE_SIZE;
            player1.LoadPlayer();
            #endregion

            #region View Port Top Left Index
            viewPortTopLeftIndex.X = (int)(player1.PlayerTilePosition.X - VIEW_PORT_WIDTH / 2);
            viewPortTopLeftIndex.Y = (int)(player1.PlayerTilePosition.Y - VIEW_PORT_LENGTH / 2); ;
            #endregion

            #region bullet
            bulletTexture = Content.Load<Texture2D>(BULLET_SPRITESHEET_NAME);
            //create array sizes of amount of bullets
            bulletPosition = new Vector2[BULLET_AMOUNT];
            bulletCollider = new Rectangle[BULLET_AMOUNT];
            bulletTimeShown = new float[BULLET_AMOUNT];
            isBulletShot = new bool[BULLET_AMOUNT];
            bulletFrameIndex = new int[BULLET_AMOUNT];
            bulletFrameRow = new int[BULLET_AMOUNT];
            bulletSource = new Rectangle[BULLET_AMOUNT];
            bulletOrigin = new Vector2[BULLET_AMOUNT];
            _bulletDirection = new bulletDirection[BULLET_AMOUNT];

            //Set bullet arrays to default
            for (int x = 0; x < BULLET_AMOUNT; x++)
            {
                bulletTimeShown[x] = 0;
                isBulletShot[x] = false;
                bulletFrameIndex[x] = 0;
                bulletFrameRow[x] = 0;
            }
            #endregion

            //boxCollider = new Rectangle((int)player1.PlayerPosition.X, (int)player1.PlayerPosition.Y, 30, 30);
            boxTexture = Content.Load<Texture2D>(START_TILE_NAME);
            boxPosition = new Vector2((VIEW_PORT_WIDTH / 2) * TILE_SIZE + SPACEING, (VIEW_PORT_LENGTH / 2) * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING);
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
            //Code to exit the game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            switch (_state)
            {
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.Gameplay:
                    UpdateGameplay(gameTime);
                    break;
                case GameState.EndOfGame:
                    UpdateEndOfGame(gameTime);
                    break;
            }

            base.Update(gameTime);
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            //origional spriteBatch.Begin(); now changed it to where a scale can be applied
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, scalematrix);
       
            switch (_state)
            {
                case GameState.MainMenu:
                    DrawMainMenu(gameTime);
                    break;
                case GameState.Gameplay:
                    DrawGameplay(gameTime);
                    break;
                case GameState.EndOfGame:
                    DrawEndOfGame(gameTime);
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        void UpdateMainMenu(GameTime deltaTime)
        {
            // Respond to user input for menu selections, etc

            //Show the mouse pointer
            IsMouseVisible = true;
        }

        void UpdateGameplay(GameTime deltaTime)
        {
            // Respond to user actions in the game.
            // Update enemies
            // Handle collisions

            //Hide the mouse pointer
            IsMouseVisible = false;

            #region Amount of Bullets Shot
            //check to see how many bullets are shot
            bulletsShot = 0;
            for (int b = 0; b < BULLET_AMOUNT; b++)
            {
                if (isBulletShot[b] == true)
                {
                    bulletsShot += 1;
                }
            }
            #endregion

            #region Update Variables

            #endregion

            #region working on this spot gets the players position on the game board.
            //thinking i can use it to check if the player has moved viewerWidth / 2 - walkingCushion
            //then use that to char from moveing player to moveing the camera untill you get back to the edge of the board then back to player movement
            //int playerXIndex = 0, playerYIndex = 0;
            //float total = player1.PlayerPosition.X - GAMEBOARD_TOP_LEFT_POINT.X;
            //while(total - 30 > 0)
            //{
            //    total -= 30;
            //    playerXIndex += 1;
            //}
            //total = player1.PlayerPosition.Y - GAMEBOARD_TOP_LEFT_POINT.Y;
            //while (total - 30 > 0)
            //{
            //    total -= 30;
            //    playerYIndex += 1;
            //}
            #endregion
            
            #region ViewingPort

            #region Down
            //Down
            if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
            {
                player1._lastDirection = Player.lastDirection.Down;

                if (viewPortTopLeftIndex.Y + 1 <= GAME_BOARD_LENGTH - VIEW_PORT_LENGTH && player1.PlayerViewPortPosition.Y >= viewPortColliderOriginPosition.Y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING + 14)
                {
                    //move the player down
                    player1.PlayerGameBoardPosition.Y += player1.movingSpeed;
                    playerStepsMoved.Y += player1.movingSpeed;

                    if (playerStepsMoved.Y >= MOVEMENTAMOUNT)
                    {
                        viewPortTopLeftIndex.Y += 1;
                        player1.PlayerTilePosition.Y += 1;
                        playerStepsMoved.Y = 0;
                    }
                }
                else if (player1.PlayerViewPortPosition.Y + 31 < (VIEW_PORT_LENGTH - 1) * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING)
                {
                    //move the player on the screen down
                    player1.PlayerViewPortPosition.Y += player1.movingSpeed;
                    playerStepsMoved.Y = 0;
                    counter += 1;
                    if (counter >= MOVEMENTAMOUNT)
                    {
                        player1.PlayerTilePosition.Y += 1;
                        counter = 0;
                    }
                }
            }
            #endregion

            #region Up
            //Up
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
            {
                player1._lastDirection = Player.lastDirection.Up;

                if (viewPortTopLeftIndex.Y >= 2 && player1.PlayerViewPortPosition.Y <= viewPortColliderOriginPosition.Y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING + 14)
                {
                    //move the player Up
                    player1.PlayerGameBoardPosition.Y -= player1.movingSpeed;
                    playerStepsMoved.Y -= player1.movingSpeed;

                    if (playerStepsMoved.Y <= -MOVEMENTAMOUNT)
                    {
                        viewPortTopLeftIndex.Y -= 1;
                        player1.PlayerTilePosition.Y -= 1;
                        playerStepsMoved.Y = 0;
                    }
                }
                else if (player1.PlayerViewPortPosition.Y > TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING + 14)
                {
                    //move the player on the screen up
                    player1.PlayerViewPortPosition.Y -= player1.movingSpeed;
                    playerStepsMoved.Y = 0;
                    counter += 1;
                    if (counter >= MOVEMENTAMOUNT)
                    {
                        player1.PlayerTilePosition.Y -= 1;
                        counter = 0;
                    }
                }
            }
            #endregion

            #region Right
            //Right
            if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
            {
                player1._lastDirection = Player.lastDirection.Right;

                if (viewPortTopLeftIndex.X <= GAME_BOARD_WIDTH - VIEW_PORT_WIDTH - 1 && player1.PlayerViewPortPosition.X >= viewPortColliderOriginPosition.X * TILE_SIZE + SPACEING + 12)
                {
                    //move the player Right
                    player1.PlayerGameBoardPosition.X += player1.movingSpeed;
                    playerStepsMoved.X += player1.movingSpeed;

                    if (playerStepsMoved.X >= MOVEMENTAMOUNT)
                    {
                        viewPortTopLeftIndex.X += 1;
                        player1.PlayerTilePosition.X += 1;
                        playerStepsMoved.X = 0;
                    }
                }
                else if (player1.PlayerViewPortPosition.X < (VIEW_PORT_WIDTH - 1) * TILE_SIZE + SPACEING - 12)
                {
                    //move the player on the screen Right
                    player1.PlayerViewPortPosition.X += player1.movingSpeed;
                    playerStepsMoved.X = 0;
                    counter += 1;
                    if (counter >= MOVEMENTAMOUNT)
                    {
                        player1.PlayerTilePosition.X += 1;
                        counter = 0;
                    }
                }
            }
            #endregion

            #region Left
            //Left
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                player1._lastDirection = Player.lastDirection.Left;

                if (viewPortTopLeftIndex.X + 1 <= GAME_BOARD_LENGTH - VIEW_PORT_LENGTH && player1.PlayerViewPortPosition.Y >= viewPortColliderOriginPosition.Y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING + 14)
                {
                    //move the player down
                    player1.PlayerGameBoardPosition.Y += player1.movingSpeed;
                    playerStepsMoved.Y += player1.movingSpeed;

                    if (playerStepsMoved.Y >= MOVEMENTAMOUNT)
                    {
                        viewPortTopLeftIndex.Y += 1;
                        player1.PlayerTilePosition.Y += 1;
                        playerStepsMoved.Y = 0;
                    }
                }
                else if (player1.PlayerViewPortPosition.Y + 31 < VIEW_PORT_LENGTH * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING)
                {
                    //move the player on the screen down
                    player1.PlayerViewPortPosition.Y += player1.movingSpeed;
                    playerStepsMoved.Y = 0;
                    counter += 1;
                    if (counter >= MOVEMENTAMOUNT)
                    {
                        player1.PlayerTilePosition.Y += 1;
                        counter = 0;
                    }
                }
            }
            #endregion

            #endregion

            #region player
            //using the player class to update the player
            player1.UpdatePlayer();

            #region Player Finish Colider
            //checks to see if the player collides with the finish point
            if (player1.playerCollisionDectector(4))
            {
                _state = GameState.EndOfGame;
            }
            #endregion
            #endregion

            #region Status Bar
            statusBar.UpdateStatusBarAnimation(player1.PlayerHealth, (10 - bulletsShot), killCount);
            #endregion

            #region bullet
            lastBulletShotTime += (float)deltaTime.ElapsedGameTime.TotalSeconds;
            for (int z = 0; z < BULLET_AMOUNT; z++)
            {
                //checks to see if fireing
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    if (isBulletShot[z] == false && lastBulletShotTime > fireRate)
                    {
                        bulletPosition[z] = new Vector2(player1.PlayerViewPortPosition.X, player1.PlayerViewPortPosition.Y - 5);
                        isBulletShot[z] = true;
                        lastBulletShotTime = 0f;
                        //break out of loop
                        break;
                    }
                }
            }

            //loops thorugh the bullets
            for (int x = 0; x < BULLET_AMOUNT; x++)
            {
                if (isBulletShot[x] == false)
                {
                    //sets the current direction to shoot the bullet
                    if (player1._lastDirection == Player.lastDirection.Right)
                    {
                        _bulletDirection[x] = bulletDirection.Right;
                    }
                    else if (player1._lastDirection == Player.lastDirection.Left)
                    {
                        _bulletDirection[x] = bulletDirection.Left;
                    }
                    else if (player1._lastDirection == Player.lastDirection.Up)
                    {
                        _bulletDirection[x] = bulletDirection.Up;
                    }
                    else if (player1._lastDirection == Player.lastDirection.Down)
                    {
                        _bulletDirection[x] = bulletDirection.Down;
                    }
                }

                //Moves Bullet in the direction of fired
                if (isBulletShot[x] == true)
                {
                    if (_bulletDirection[x] == bulletDirection.Up)
                    {
                        bulletPosition[x].Y -= BULLET_SPEED;
                        bulletFrameRow[x] = shootingUpFrameRow;
                    }
                    else if (_bulletDirection[x] == bulletDirection.Down)
                    {
                        bulletPosition[x].Y += BULLET_SPEED;
                        bulletFrameRow[x] = shootingDownFrameRow;
                    }
                    else if (_bulletDirection[x] == bulletDirection.Left)
                    {
                        bulletPosition[x].X -= BULLET_SPEED;
                        bulletFrameRow[x] = shootingLeftFrameRow;
                    }
                    else if (_bulletDirection[x] == bulletDirection.Right)
                    {
                        bulletPosition[x].X += BULLET_SPEED;
                        bulletFrameRow[x] = shootingRightFrameRow;
                    }

                    //checks to see if bullet went outside of the view port
                    if (bulletPosition[x].X >= (VIEW_PORT_WIDTH * TILE_SIZE + SPACEING) || bulletPosition[x].X <= 0 + SPACEING)
                    {
                        isBulletShot[x] = false;
                    }
                    if (bulletPosition[x].Y >= (VIEW_PORT_LENGTH * TILE_SIZE) + GameStatusBar.STATUSBAR_LENGTH + SPACEING || bulletPosition[x].Y <= 0 + GameStatusBar.STATUSBAR_LENGTH + SPACEING)
                    {
                        isBulletShot[x] = false;
                    }

                    // Process elapsed time
                    bulletTimeShown[x] += (float)deltaTime.ElapsedGameTime.TotalSeconds;
                    while (bulletTimeShown[x] > bulletFramePlayTime)
                    {
                        // Play the next frame in the SpriteSheet
                        bulletFrameIndex[x]++;

                        // reset elapsed time
                        bulletTimeShown[x] = 0f;
                    }
                    //sets frame draw back to 0
                    if (bulletFrameIndex[x] > bulletTotalFrames)
                    {
                        bulletFrameIndex[x] = 0;
                    }
                }
            }
            #endregion

            #region Player Animation
            //Animation
            // Process elapsed time
            timeShown += (float)deltaTime.ElapsedGameTime.TotalSeconds;
            timeShown = player1.UpdatePlayerAnimation(timeShown);
            #endregion

            #region Is Player Alive
            if (!player1.IsPlayerAlive)
                _state = GameState.EndOfGame;
            #endregion
        }

        void UpdateEndOfGame(GameTime deltaTime)
        {
            // Update scores
            // Do any animations, effects, etc for getting a high score
            // Respond to user input to restart level, or go back to main menu

            //if (pushedMainMenuButton)
            //    _state = GameState.MainMenu;
            //else if (pushedRestartLevelButton)
            //{
            //    ResetLevel();
            //    _state = GameState.Gameplay;
            //}
        }

        void DrawMainMenu(GameTime deltaTime)
        {
            // Draw the main menu, any active selections, etc
            spriteBatch.Draw(startButton, startButtonPosition, Color.White);
            spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);
            if (smallButtonShow)
            spriteBatch.Draw(smallButton, smallButtonPosition, Color.White);
            if (mediumButtonShow)
            spriteBatch.Draw(mediumButton, mediumButtonPosition, Color.White);
            if (largeButtonShow)
            spriteBatch.Draw(largeButton, largeButtonPosition, Color.White);
            if (smallButtonClickedShow)
            spriteBatch.Draw(smallButtonClicked, smallButtonPosition, Color.White);
            if (mediumButtonClickedShow)
            spriteBatch.Draw(mediumButtonClicked, mediumButtonPosition, Color.White);
            if (largeButtonClickedShow)
            spriteBatch.Draw(largeButtonClicked, largeButtonPosition, Color.White);
            
            //wait for mouseclick
            mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(mouseState.X, mouseState.Y);
            }
            previousMouseState = mouseState;
        }

        void DrawGameplay(GameTime deltaTime)
        {
            // Draw the background the level
            // Draw enemies
            // Draw the player
            // Draw particle effects, etc
            #region gameBoard Drawing

            #region Down
            if (player1._lastDirection == Player.lastDirection.Down)
            {
                // Calculate the source rectangle of the current frame.
                rectangleSizeForTop = new Rectangle(0, Math.Abs(playerStepsMoved.Y), TILE_SIZE, TILE_SIZE - Math.Abs(playerStepsMoved.Y));
                rectangleSizeForBottom = new Rectangle(0, 0, TILE_SIZE, Math.Abs(playerStepsMoved.Y));
                rectangleSizeForLeft = new Rectangle(0, 0, TILE_SIZE - Math.Abs(playerStepsMoved.X), TILE_SIZE);
                rectangleSizeForRight = new Rectangle(0, 0, TILE_SIZE - Math.Abs(playerStepsMoved.X), TILE_SIZE);

                rectangleSizeForTopLeft = new Rectangle(Math.Abs(playerStepsMoved.X), Math.Abs(playerStepsMoved.Y), TILE_SIZE - Math.Abs(playerStepsMoved.X), TILE_SIZE - Math.Abs(playerStepsMoved.Y));
                rectangleSizeForTopRight = new Rectangle(Math.Abs(playerStepsMoved.X), Math.Abs(playerStepsMoved.Y), TILE_SIZE - Math.Abs(playerStepsMoved.X), TILE_SIZE - Math.Abs(playerStepsMoved.Y));
                rectangleSizeForBottomLeft = new Rectangle(0, 0, TILE_SIZE - Math.Abs(playerStepsMoved.X), Math.Abs(playerStepsMoved.Y));
                rectangleSizeForBottomRight = new Rectangle(0, 0, TILE_SIZE - Math.Abs(playerStepsMoved.X), Math.Abs(playerStepsMoved.Y));

                TopLeftTilePosition = new Vector2(SPACEING, GameStatusBar.STATUSBAR_LENGTH + SPACEING);
                TopRightTilePosition = new Vector2(VIEW_PORT_WIDTH * TILE_SIZE + SPACEING - TILE_SIZE, GameStatusBar.STATUSBAR_LENGTH + SPACEING);
                BottomLeftTilePosition = new Vector2(SPACEING - Math.Abs(playerStepsMoved.X), (VIEW_PORT_LENGTH - 1) * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - Math.Abs(playerStepsMoved.Y));
                bottomRightTilePosition = new Vector2((VIEW_PORT_WIDTH - 1) * TILE_SIZE + SPACEING - Math.Abs(playerStepsMoved.X), (VIEW_PORT_LENGTH - 1) * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - Math.Abs(playerStepsMoved.Y));

                //Draw Corners
                //Top Left            
                spriteBatch.Draw(groundTexture[viewPortTopLeftIndex.X, viewPortTopLeftIndex.Y], TopLeftTilePosition, rectangleSizeForTopLeft, Color.White);
                //Top Right
                spriteBatch.Draw(groundTexture[VIEW_PORT_WIDTH + viewPortTopLeftIndex.X - 1, viewPortTopLeftIndex.Y], TopRightTilePosition, rectangleSizeForTopRight, Color.White);
                //Bottom Left
                spriteBatch.Draw(groundTexture[viewPortTopLeftIndex.X, VIEW_PORT_LENGTH + viewPortTopLeftIndex.Y - 1], BottomLeftTilePosition, rectangleSizeForBottomLeft, Color.White);
                //Bottom Right           
                spriteBatch.Draw(groundTexture[VIEW_PORT_WIDTH + viewPortTopLeftIndex.X - 1, VIEW_PORT_LENGTH + viewPortTopLeftIndex.Y - 1], bottomRightTilePosition, rectangleSizeForBottomRight, Color.White);

                //Draw Top Edge of Game Board
                for (int x = 1; x < VIEW_PORT_WIDTH - 1; x++)
                {
                    int y = 0;
                    Vector2 groundTilePosition = new Vector2(x * TILE_SIZE + SPACEING + playerStepsMoved.X, y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING);
                    spriteBatch.Draw(groundTexture[x + viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y], groundTilePosition, rectangleSizeForTop, Color.White);
                }

                //Draw Bottom Edge of Game Board
                for (int x = 1; x < VIEW_PORT_WIDTH - 1; x++)
                {
                    int y = VIEW_PORT_LENGTH - 1;
                    Vector2 groundTilePosition = new Vector2(x * TILE_SIZE + SPACEING, y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - playerStepsMoved.Y);
                    spriteBatch.Draw(groundTexture[x + viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y], groundTilePosition, rectangleSizeForBottom, Color.White);
                }

                //Draw Left Edge of Game Board
                for (int y = 1; y <= VIEW_PORT_LENGTH - 2; y++)
                {
                    int x = 0;
                    Vector2 groundTilePosition = new Vector2(x * TILE_SIZE + SPACEING, y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - playerStepsMoved.Y);
                    spriteBatch.Draw(groundTexture[viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y], groundTilePosition, rectangleSizeForLeft, Color.White);
                }

                //Draw Right Edge of Game Board
                for (int y = 1; y <= VIEW_PORT_LENGTH - 2; y++)
                {
                    int x = VIEW_PORT_WIDTH - 1;
                    Vector2 groundTilePosition = new Vector2(x * TILE_SIZE + SPACEING + playerStepsMoved.X, y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - playerStepsMoved.Y);
                    spriteBatch.Draw(groundTexture[x + viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y], groundTilePosition, rectangleSizeForRight, Color.White);
                }

                //Draw Center of Game Board
                for (int x = 1; x < VIEW_PORT_WIDTH - 1; x++)
                {
                    for (int y = 1; y < VIEW_PORT_LENGTH - 1; y++)
                    {
                        Vector2 groundPosition = new Vector2(x * TILE_SIZE + SPACEING - playerStepsMoved.X, y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - playerStepsMoved.Y);
                        spriteBatch.Draw(groundTexture[x + viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y], groundPosition, Color.White);
                    }
                }
            }
            #endregion

            #region Up
            if (player1._lastDirection == Player.lastDirection.Up)
            {
                // Calculate the source rectangle of the current frame.
                rectangleSizeForTop = new Rectangle(0, TILE_SIZE - Math.Abs(playerStepsMoved.Y), TILE_SIZE, Math.Abs(playerStepsMoved.Y));
                rectangleSizeForBottom = new Rectangle(0, 0, TILE_SIZE, TILE_SIZE - Math.Abs(playerStepsMoved.Y));
                rectangleSizeForLeft = new Rectangle(0, 0, TILE_SIZE + Math.Abs(playerStepsMoved.X), TILE_SIZE);
                rectangleSizeForRight = new Rectangle(0, 0, TILE_SIZE + Math.Abs(playerStepsMoved.X), TILE_SIZE);

                rectangleSizeForTopLeft = new Rectangle(0, TILE_SIZE - Math.Abs(playerStepsMoved.Y), TILE_SIZE - Math.Abs(playerStepsMoved.X), Math.Abs(playerStepsMoved.Y));
                rectangleSizeForTopRight = new Rectangle(playerStepsMoved.X, TILE_SIZE - Math.Abs(playerStepsMoved.Y), TILE_SIZE - Math.Abs(playerStepsMoved.X), Math.Abs(playerStepsMoved.Y));
                rectangleSizeForBottomLeft = new Rectangle(0, 0, TILE_SIZE + Math.Abs(playerStepsMoved.X), TILE_SIZE - Math.Abs(playerStepsMoved.Y));
                rectangleSizeForBottomRight = new Rectangle(0, 0, TILE_SIZE + Math.Abs(playerStepsMoved.X), TILE_SIZE - Math.Abs(playerStepsMoved.Y));

                TopLeftTilePosition = new Vector2(SPACEING, GameStatusBar.STATUSBAR_LENGTH + SPACEING);
                TopRightTilePosition = new Vector2(VIEW_PORT_WIDTH * TILE_SIZE + SPACEING - TILE_SIZE, GameStatusBar.STATUSBAR_LENGTH + SPACEING);
                BottomLeftTilePosition = new Vector2(SPACEING + Math.Abs(playerStepsMoved.X), (VIEW_PORT_LENGTH - 2) * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING + Math.Abs(playerStepsMoved.Y));
                bottomRightTilePosition = new Vector2((VIEW_PORT_WIDTH - 1) * TILE_SIZE + SPACEING + Math.Abs(playerStepsMoved.X), (VIEW_PORT_LENGTH - 2) * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING + Math.Abs(playerStepsMoved.Y));

                //Draw Corners
                //Top Left            
                spriteBatch.Draw(groundTexture[viewPortTopLeftIndex.X, viewPortTopLeftIndex.Y - 1], TopLeftTilePosition, rectangleSizeForTopLeft, Color.White);
                //Top Right
                spriteBatch.Draw(groundTexture[VIEW_PORT_WIDTH + viewPortTopLeftIndex.X - 1, viewPortTopLeftIndex.Y - 1], TopRightTilePosition, rectangleSizeForTopRight, Color.White);
                //Bottom Left
                spriteBatch.Draw(groundTexture[viewPortTopLeftIndex.X, VIEW_PORT_LENGTH + viewPortTopLeftIndex.Y - 2], BottomLeftTilePosition, rectangleSizeForBottomLeft, Color.White);
                //Bottom Right           
                spriteBatch.Draw(groundTexture[VIEW_PORT_WIDTH + viewPortTopLeftIndex.X - 1, VIEW_PORT_LENGTH + viewPortTopLeftIndex.Y - 2], bottomRightTilePosition, rectangleSizeForBottomRight, Color.White);

                //Draw Top Edge of Game Board
                for (int x = 1; x < VIEW_PORT_WIDTH - 1; x++)
                {
                    int y = 0;
                    Vector2 groundTilePosition = new Vector2(x * TILE_SIZE + SPACEING + playerStepsMoved.X, y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING);
                    spriteBatch.Draw(groundTexture[x + viewPortTopLeftIndex.X, y - 1 + viewPortTopLeftIndex.Y], groundTilePosition, rectangleSizeForTop, Color.White);
                }

                //Draw Bottom Edge of Game Board
                for (int x = 1; x < VIEW_PORT_WIDTH - 1; x++)
                {
                    int y = VIEW_PORT_LENGTH - 2;
                    Vector2 groundTilePosition = new Vector2(x * TILE_SIZE + SPACEING, y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - playerStepsMoved.Y);
                    spriteBatch.Draw(groundTexture[x + viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y], groundTilePosition, rectangleSizeForBottom, Color.White);
                }

                //Draw Left Edge of Game Board
                for (int y = 1; y <= VIEW_PORT_LENGTH - 2; y++)
                {
                    int x = 0;
                    Vector2 groundTilePosition = new Vector2(x * TILE_SIZE + SPACEING, (y - 1) * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - playerStepsMoved.Y);
                    spriteBatch.Draw(groundTexture[viewPortTopLeftIndex.X, y - 1 + viewPortTopLeftIndex.Y], groundTilePosition, rectangleSizeForLeft, Color.White);
                }

                //Draw Right Edge of Game Board
                for (int y = 1; y <= VIEW_PORT_LENGTH - 2; y++)
                {
                    int x = VIEW_PORT_WIDTH - 1;
                    Vector2 groundTilePosition = new Vector2(x * TILE_SIZE + SPACEING + playerStepsMoved.X, (y - 1) * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - playerStepsMoved.Y);
                    spriteBatch.Draw(groundTexture[x + viewPortTopLeftIndex.X, (y - 1) + viewPortTopLeftIndex.Y], groundTilePosition, rectangleSizeForRight, Color.White);
                }

                //Draw Center of Game Board
                for (int x = 1; x < VIEW_PORT_WIDTH - 1; x++)
                {
                    for (int y = 0; y < VIEW_PORT_LENGTH - 2; y++)
                    {
                        Vector2 groundPosition = new Vector2(x * TILE_SIZE + SPACEING - playerStepsMoved.X, y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - playerStepsMoved.Y);
                        spriteBatch.Draw(groundTexture[x + viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y], groundPosition, Color.White);
                    }
                }
            }
            #endregion

            #region Left
            if (player1._lastDirection == Player.lastDirection.Left)
            {
                // Calculate the source rectangle of the current frame.
                rectangleSizeForTop = new Rectangle(0, Math.Abs(playerStepsMoved.Y), TILE_SIZE, TILE_SIZE - Math.Abs(playerStepsMoved.Y));
                rectangleSizeForBottom = new Rectangle(0, 0, TILE_SIZE, Math.Abs(playerStepsMoved.Y));
                rectangleSizeForLeft = new Rectangle(0, 0, TILE_SIZE - Math.Abs(playerStepsMoved.X), TILE_SIZE);
                rectangleSizeForRight = new Rectangle(0, 0, TILE_SIZE - Math.Abs(playerStepsMoved.X), TILE_SIZE);

                rectangleSizeForTopLeft = new Rectangle(Math.Abs(playerStepsMoved.X), Math.Abs(playerStepsMoved.Y), TILE_SIZE - Math.Abs(playerStepsMoved.X), TILE_SIZE - Math.Abs(playerStepsMoved.Y));
                rectangleSizeForTopRight = new Rectangle(Math.Abs(playerStepsMoved.X), Math.Abs(playerStepsMoved.Y), TILE_SIZE - Math.Abs(playerStepsMoved.X), TILE_SIZE - Math.Abs(playerStepsMoved.Y));
                rectangleSizeForBottomLeft = new Rectangle(0, 0, TILE_SIZE - Math.Abs(playerStepsMoved.X), Math.Abs(playerStepsMoved.Y));
                rectangleSizeForBottomRight = new Rectangle(0, 0, TILE_SIZE - Math.Abs(playerStepsMoved.X), Math.Abs(playerStepsMoved.Y));

                TopLeftTilePosition = new Vector2(SPACEING, GameStatusBar.STATUSBAR_LENGTH + SPACEING);
                TopRightTilePosition = new Vector2(VIEW_PORT_WIDTH * TILE_SIZE + SPACEING - TILE_SIZE, GameStatusBar.STATUSBAR_LENGTH + SPACEING);
                BottomLeftTilePosition = new Vector2(SPACEING - Math.Abs(playerStepsMoved.X), (VIEW_PORT_LENGTH - 1) * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - Math.Abs(playerStepsMoved.Y));
                bottomRightTilePosition = new Vector2((VIEW_PORT_WIDTH - 1) * TILE_SIZE + SPACEING - Math.Abs(playerStepsMoved.X), (VIEW_PORT_LENGTH - 1) * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - Math.Abs(playerStepsMoved.Y));

                //Draw Corners
                //Top Left            
                spriteBatch.Draw(groundTexture[viewPortTopLeftIndex.X, viewPortTopLeftIndex.Y], TopLeftTilePosition, rectangleSizeForTopLeft, Color.White);
                //Top Right
                spriteBatch.Draw(groundTexture[VIEW_PORT_WIDTH + viewPortTopLeftIndex.X - 1, viewPortTopLeftIndex.Y], TopRightTilePosition, rectangleSizeForTopRight, Color.White);
                //Bottom Left
                spriteBatch.Draw(groundTexture[viewPortTopLeftIndex.X, VIEW_PORT_LENGTH + viewPortTopLeftIndex.Y - 1], BottomLeftTilePosition, rectangleSizeForBottomLeft, Color.White);
                //Bottom Right           
                spriteBatch.Draw(groundTexture[VIEW_PORT_WIDTH + viewPortTopLeftIndex.X - 1, VIEW_PORT_LENGTH + viewPortTopLeftIndex.Y - 1], bottomRightTilePosition, rectangleSizeForBottomRight, Color.White);

                //Draw Top Edge of Game Board
                for (int x = 1; x < VIEW_PORT_WIDTH - 1; x++)
                {
                    int y = 0;
                    Vector2 groundTilePosition = new Vector2(x * TILE_SIZE + SPACEING + playerStepsMoved.X, y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING);
                    spriteBatch.Draw(groundTexture[x + viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y], groundTilePosition, rectangleSizeForTop, Color.White);
                }

                //Draw Bottom Edge of Game Board
                for (int x = 1; x < VIEW_PORT_WIDTH - 1; x++)
                {
                    int y = VIEW_PORT_LENGTH - 1;
                    Vector2 groundTilePosition = new Vector2(x * TILE_SIZE + SPACEING, y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - playerStepsMoved.Y);
                    spriteBatch.Draw(groundTexture[x + viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y], groundTilePosition, rectangleSizeForBottom, Color.White);
                }

                //Draw Left Edge of Game Board
                for (int y = 1; y <= VIEW_PORT_LENGTH - 2; y++)
                {
                    int x = 0;
                    Vector2 groundTilePosition = new Vector2(x * TILE_SIZE + SPACEING, y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - playerStepsMoved.Y);
                    spriteBatch.Draw(groundTexture[viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y], groundTilePosition, rectangleSizeForLeft, Color.White);
                }

                //Draw Right Edge of Game Board
                for (int y = 1; y <= VIEW_PORT_LENGTH - 2; y++)
                {
                    int x = VIEW_PORT_WIDTH - 1;
                    Vector2 groundTilePosition = new Vector2(x * TILE_SIZE + SPACEING + playerStepsMoved.X, y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - playerStepsMoved.Y);
                    spriteBatch.Draw(groundTexture[x + viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y], groundTilePosition, rectangleSizeForRight, Color.White);
                }

                //Draw Center of Game Board
                for (int x = 1; x < VIEW_PORT_WIDTH - 1; x++)
                {
                    for (int y = 1; y < VIEW_PORT_LENGTH - 1; y++)
                    {
                        Vector2 groundPosition = new Vector2(x * TILE_SIZE + SPACEING - playerStepsMoved.X, y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - playerStepsMoved.Y);
                        spriteBatch.Draw(groundTexture[x + viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y], groundPosition, Color.White);
                    }
                }
            }
            #endregion

            #region Right
            if (player1._lastDirection == Player.lastDirection.Right)
            {
                // Calculate the source rectangle of the current frame.
                rectangleSizeForTop = new Rectangle(0, TILE_SIZE - Math.Abs(playerStepsMoved.Y), TILE_SIZE, Math.Abs(playerStepsMoved.Y));
                rectangleSizeForBottom = new Rectangle(0, 0, TILE_SIZE, TILE_SIZE - Math.Abs(playerStepsMoved.Y));
                rectangleSizeForLeft = new Rectangle(0, 0, TILE_SIZE + Math.Abs(playerStepsMoved.X), TILE_SIZE);
                rectangleSizeForRight = new Rectangle(0, 0, TILE_SIZE + Math.Abs(playerStepsMoved.X), TILE_SIZE);

                rectangleSizeForTopLeft = new Rectangle(0, TILE_SIZE - Math.Abs(playerStepsMoved.Y), TILE_SIZE - Math.Abs(playerStepsMoved.X), Math.Abs(playerStepsMoved.Y));
                rectangleSizeForTopRight = new Rectangle(playerStepsMoved.X, TILE_SIZE - Math.Abs(playerStepsMoved.Y), TILE_SIZE - Math.Abs(playerStepsMoved.X), Math.Abs(playerStepsMoved.Y));
                rectangleSizeForBottomLeft = new Rectangle(0, 0, TILE_SIZE + Math.Abs(playerStepsMoved.X), TILE_SIZE - Math.Abs(playerStepsMoved.Y));
                rectangleSizeForBottomRight = new Rectangle(0, 0, TILE_SIZE + Math.Abs(playerStepsMoved.X), TILE_SIZE - Math.Abs(playerStepsMoved.Y));

                TopLeftTilePosition = new Vector2(SPACEING, GameStatusBar.STATUSBAR_LENGTH + SPACEING);
                TopRightTilePosition = new Vector2(VIEW_PORT_WIDTH * TILE_SIZE + SPACEING - TILE_SIZE, GameStatusBar.STATUSBAR_LENGTH + SPACEING);
                BottomLeftTilePosition = new Vector2(SPACEING + Math.Abs(playerStepsMoved.X), (VIEW_PORT_LENGTH - 2) * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING + Math.Abs(playerStepsMoved.Y));
                bottomRightTilePosition = new Vector2((VIEW_PORT_WIDTH - 1) * TILE_SIZE + SPACEING + Math.Abs(playerStepsMoved.X), (VIEW_PORT_LENGTH - 2) * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING + Math.Abs(playerStepsMoved.Y));

                //Draw Corners
                //Top Left            
                spriteBatch.Draw(groundTexture[viewPortTopLeftIndex.X, viewPortTopLeftIndex.Y - 1], TopLeftTilePosition, rectangleSizeForTopLeft, Color.White);
                //Top Right
                spriteBatch.Draw(groundTexture[VIEW_PORT_WIDTH + viewPortTopLeftIndex.X - 1, viewPortTopLeftIndex.Y - 1], TopRightTilePosition, rectangleSizeForTopRight, Color.White);
                //Bottom Left
                spriteBatch.Draw(groundTexture[viewPortTopLeftIndex.X, VIEW_PORT_LENGTH + viewPortTopLeftIndex.Y - 2], BottomLeftTilePosition, rectangleSizeForBottomLeft, Color.White);
                //Bottom Right           
                spriteBatch.Draw(groundTexture[VIEW_PORT_WIDTH + viewPortTopLeftIndex.X - 1, VIEW_PORT_LENGTH + viewPortTopLeftIndex.Y - 2], bottomRightTilePosition, rectangleSizeForBottomRight, Color.White);

                //Draw Top Edge of Game Board
                for (int x = 1; x < VIEW_PORT_WIDTH - 1; x++)
                {
                    int y = 0;
                    Vector2 groundTilePosition = new Vector2(x * TILE_SIZE + SPACEING + playerStepsMoved.X, y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING);
                    spriteBatch.Draw(groundTexture[x + viewPortTopLeftIndex.X, y - 1 + viewPortTopLeftIndex.Y], groundTilePosition, rectangleSizeForTop, Color.White);
                }

                //Draw Bottom Edge of Game Board
                for (int x = 1; x < VIEW_PORT_WIDTH - 1; x++)
                {
                    int y = VIEW_PORT_LENGTH - 2;
                    Vector2 groundTilePosition = new Vector2(x * TILE_SIZE + SPACEING, y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - playerStepsMoved.Y);
                    spriteBatch.Draw(groundTexture[x + viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y], groundTilePosition, rectangleSizeForBottom, Color.White);
                }

                //Draw Left Edge of Game Board
                for (int y = 1; y <= VIEW_PORT_LENGTH - 2; y++)
                {
                    int x = 0;
                    Vector2 groundTilePosition = new Vector2(x * TILE_SIZE + SPACEING, (y - 1) * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - playerStepsMoved.Y);
                    spriteBatch.Draw(groundTexture[viewPortTopLeftIndex.X, y - 1 + viewPortTopLeftIndex.Y], groundTilePosition, rectangleSizeForLeft, Color.White);
                }

                //Draw Right Edge of Game Board
                for (int y = 1; y <= VIEW_PORT_LENGTH - 2; y++)
                {
                    int x = VIEW_PORT_WIDTH - 1;
                    Vector2 groundTilePosition = new Vector2(x * TILE_SIZE + SPACEING + playerStepsMoved.X, (y - 1) * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - playerStepsMoved.Y);
                    spriteBatch.Draw(groundTexture[x + viewPortTopLeftIndex.X, (y - 1) + viewPortTopLeftIndex.Y], groundTilePosition, rectangleSizeForRight, Color.White);
                }

                //Draw Center of Game Board
                for (int x = 1; x < VIEW_PORT_WIDTH - 1; x++)
                {
                    for (int y = 0; y < VIEW_PORT_LENGTH - 2; y++)
                    {
                        Vector2 groundPosition = new Vector2(x * TILE_SIZE + SPACEING - playerStepsMoved.X, y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING - playerStepsMoved.Y);
                        spriteBatch.Draw(groundTexture[x + viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y], groundPosition, Color.White);
                    }
                }
            }
            #endregion
            



            

            //side the colliders on screen
            for (int y = 0; y < VIEW_PORT_LENGTH; y++)
            {
                for (int x = 0; x < VIEW_PORT_WIDTH; x++)
                {
                    groundTextureCollider[x + viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y] = new Rectangle(x * TILE_SIZE + SPACEING, y * TILE_SIZE + GameStatusBar.STATUSBAR_LENGTH + SPACEING, TILE_SIZE, TILE_SIZE);
                }
            }
            
            spriteBatch.Draw(boxTexture, boxPosition, Color.White);
            #endregion

            #region Status Bar
            statusBar.drawStatusBar();
            spriteBatch.Draw(statusBar.StatusBarBackgroundTexture, statusBar.StatusBarBackgroundPosition);
            spriteBatch.Draw(statusBar.HealthBarTexture, statusBar.HealthBarPosition, statusBar.healthBarSource, Color.White, 0.0f, statusBar.healthBarOrigin, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(statusBar.AmmoAvailableTexture, statusBar.AmmoAvailablePosition, statusBar.ammoSource, Color.White, 0.0f, statusBar.ammoOrigin, 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(statusBar.GameStatusFont, killCount.ToString(), statusBar.KillCountTextPosition, Color.Yellow);
            #endregion

            #region Draw Player Animation
            //Draw Player Animation
            player1.DrawPlayer();
            spriteBatch.Draw(player1.playerSpriteSheet, player1.PlayerViewPortPosition, player1.source, Color.White, 0.0f,
            player1.origin, 1.0f, SpriteEffects.None, 0.0f);
            #endregion

            #region draw the bullet
            //Draw Bullet
            for (int x = 0; x < BULLET_AMOUNT; x++)
            {
                if (isBulletShot[x] == true)
                {
                    //Draw Player Animation
                    // Calculate the source rectangle of the current frame.
                    bulletSource[x] = new Rectangle(bulletFrameIndex[x] * bulletFrameWidth, bulletFrameRow[x] * bulletFrameHeight, bulletFrameWidth, bulletFrameHeight);
                    // Calculate position and origin to draw in the center of the screen
                    bulletPosition[x] = new Vector2(bulletPosition[x].X, bulletPosition[x].Y);
                    bulletOrigin[x] = new Vector2(bulletFrameWidth / 2.0f, bulletFrameHeight);
                    // Draw the current frame.
                    spriteBatch.Draw(bulletTexture, bulletPosition[x], bulletSource[x], Color.White, 0.0f, bulletOrigin[x], 1.0f, SpriteEffects.None, 0.0f);
                }
            }
            #endregion

            #region testValues
            spriteBatch.DrawString(font, "Player Tile Position: " + player1.PlayerTilePosition.ToString(), new Vector2(100, 100), Color.Red);
            spriteBatch.DrawString(font, "View Port Tile Position: " + viewPortTopLeftIndex.ToString(), new Vector2(100, 150), Color.Red);
            spriteBatch.DrawString(font, "Colliding with: " + player1.playerCollisionLocator().ToString(), new Vector2(100, 200), Color.Red);
            #endregion
        }

        void DrawEndOfGame(GameTime deltaTime)
        {
            // Draw text and scores
            // Draw menu for restarting level or going back to main menu
        }

        public void buildGameBoard(int x, int y)
        {
            int chance;
            chance = randomMap[x, y];
            
            if (chance < 75)
            {
                groundTexture[x, y] = Content.Load<Texture2D>(GRASS_TILE_NAME);
                groundTextureType[x, y] = 1;
            }
            else if (chance <95)
            {
                groundTexture[x, y] = Content.Load<Texture2D>(SAND_TILE_NAME);
                groundTextureType[x, y] = 3;
            }
            else
            {
                groundTexture[x, y] = Content.Load<Texture2D>(SAND_TILE_NAME);
                groundTextureType[x, y] = 3;
            }
        }

        void MouseClicked(int x, int y)

        {
            Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 100, 20);
            Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 100, 20);
            Rectangle smallButtonRect = new Rectangle((int)smallButtonPosition.X, (int)smallButtonPosition.Y, 100, 20);
            Rectangle mediumButtonRect = new Rectangle((int)mediumButtonPosition.X, (int)mediumButtonPosition.Y, 100, 20);
            Rectangle largeButtonRect = new Rectangle((int)largeButtonPosition.X, (int)largeButtonPosition.Y, 100, 20);

            //creates a rectangle of 10x10 around the place where the mouse was clicked        
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);    
            
            if (mouseClickRect.Intersects(startButtonRect)) //player clicked start button    
            {      
                //change state to gameplay        
                _state = GameState.Gameplay;           
            }
            else if (mouseClickRect.Intersects(exitButtonRect)) //player clicked exit button
            {
                Exit();
            }
            else if (mouseClickRect.Intersects(smallButtonRect)) //player clicked small button
            {
                smallButtonShow = false;
                smallButtonClickedShow = true;
                mediumButtonShow = true;
                mediumButtonClickedShow = false;
                largeButtonShow = true;
                largeButtonClickedShow = false;
                mapSize = 1;
            }
            else if (mouseClickRect.Intersects(mediumButtonRect)) //player clicked medium button
            {
                smallButtonShow = true;
                smallButtonClickedShow = false;
                mediumButtonShow = false;
                mediumButtonClickedShow = true;
                largeButtonShow = true;
                largeButtonClickedShow = false;
                mapSize = 2;
            }
            else if (mouseClickRect.Intersects(largeButtonRect)) //player clicked large button
            {
                smallButtonShow = true;
                smallButtonClickedShow = false;
                mediumButtonShow = true;
                mediumButtonClickedShow = false;
                largeButtonShow = false;
                largeButtonClickedShow = true;
                mapSize = 3;
            }
        }
    }
}

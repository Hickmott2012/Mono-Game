using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace War_Game
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        #region Declarations
        //Global Declaring
        private const int TILE_SIZE = 30;
        private const int TROOP_BODY_SIZE_X = 24;
        private const int TROOP_BODY_SIZE_Y = 53;
        private const int TROOP_FEET_SIZE_X = 24;
        private const int TROOP_FEET_SIZE_Y = 10;
        private int GAME_BOARD_WIDTH = 30;
        private int GAME_BOARD_LENGTH = 20;
        private enum GameState
        {
            MainMenu,
            Gameplay,
            EndOfGame,
        }
        private GameState _state;
        private SpriteFont font;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

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
        private MouseState mouseState;
        private MouseState previousMouseState;
        private bool smallButtonShow = true;
        private bool mediumButtonShow = true;
        private bool largeButtonShow = true;
        private bool smallButtonClickedShow = false;
        private bool mediumButtonClickedShow = false;
        private bool largeButtonClickedShow = false;
        #endregion

        #region Moving Speeds
        private int movingSpeed = 2;
        private const int sandSpeed = 1;
        private const int grassSpeed = 2;
        #endregion

        #region Sprite Location Strings
        private const string TROOP_SPRITESHEET_NAME = "skeletonSpriteSheet.png";
        private const string FINISH_TILE_NAME = "finishTile.png";
        private const string GRASS_TILE_NAME = "smallGrassTile.png";
        private const string WATER_TILE_NAME = "smallWaterTile.png";
        private const string SAND_TILE_NAME = "smallSandTile.png";
        private Random rnd;
        #endregion

        #region GroundTexture
        //Ground Texture Array
        private Texture2D[,] groundTexture;
        //Ground Texture Type Number Array
        private int[,] groundTextureType;
        //Ground Texture Collider
        private Rectangle[,] groundTextureCollider;
        //Ground Map Randomizer
        private int[,] randomMap;
        //Ground Map Size
        private int mapSize = 0;
        #endregion

        #region Troop Animation
        //Troop Position
        private Vector2 TroopPosition;
        //Troop Feet Collider
        private Rectangle TroopFeetCollider;
        //Troop Bosy Collider
        private Rectangle TroopBodyCollider;
        // the spritesheet containing our animation frames
        private Texture2D troopSpriteSheet;
        // the elapsed amount of time the frame has been shown for
        private float timeShown;
        // duration of time to show each frame
        private float framePlayTime = 0.1f;
        //Normal frame play speed
        private const float normalPlaySpeed = 0.1f;
        //Sand frame play speed
        private const float sandPlaySpeed = 0.15f;
        // an index of the current frame being shown
        private int frameIndex = 0;
        // the current frame row being shown
        private int frameRow = 0;
        // total number of frames in our spritesheet animation row
        private int totalFrames = 8;
        // define the size of our animation frame
        private int frameHeight = 64;
        private int frameWidth = 64;
        //Frame Row Name with Numbers
        private const int walkingLeftFrameRow = 9;
        private const int walkingRightFrameRow = 11;
        private const int walkingUpFrameRow = 8;
        private const int walkingDownFrameRow = 10;
        //walking state selector
        private enum lastDirection
        {
            Left,
            Right,
            Up,
            Down,
        }
        private lastDirection _lastDirection;
        #endregion

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";

            
            #region Map Size
            graphics.PreferredBackBufferWidth = GAME_BOARD_WIDTH * TILE_SIZE;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = GAME_BOARD_LENGTH * TILE_SIZE;   // set this value to the desired height of your window
            graphics.ApplyChanges();
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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            #region Menu
            startButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 200);
            exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 250);
            smallButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2)-200, 20);
            mediumButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 20);
            largeButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) + 100, 20);

            startButton = Content.Load<Texture2D>("start");
            exitButton = Content.Load<Texture2D>("exit");
            smallButton = Content.Load<Texture2D>("small");
            mediumButton = Content.Load<Texture2D>("medium");
            largeButton = Content.Load<Texture2D>("large");
            smallButtonClicked = Content.Load<Texture2D>("smallClicked");
            mediumButtonClicked = Content.Load<Texture2D>("mediumClicked");
            largeButtonClicked = Content.Load<Texture2D>("largeClicked");
            #endregion

            #region Build Game Board
            //Build Random Map
            font = Content.Load<SpriteFont>("font");
            rnd = new Random();
            randomMap = new int[GAME_BOARD_WIDTH,GAME_BOARD_LENGTH];
            
            for (int y = 0; y < GAME_BOARD_LENGTH; y++)
            {
                for (int x = 0; x < GAME_BOARD_WIDTH; x++)
                {
                    randomMap[x,y] = rnd.Next(0,101);
                }
            }

            //Build Game Board
            //Add Level Finish
            int xFinishLocation = rnd.Next(0, GAME_BOARD_WIDTH - 1);
            int yFinishLocation = rnd.Next(0, GAME_BOARD_LENGTH - 1);
            groundTexture[xFinishLocation, yFinishLocation] = Content.Load<Texture2D>(FINISH_TILE_NAME);
            groundTextureType[xFinishLocation, yFinishLocation] = 4;

            for (int y = 0; y < GAME_BOARD_LENGTH; y++)
            {
                for (int x = 0; x < GAME_BOARD_WIDTH; x++)
                {
                    if (groundTexture[x, y] == null)
                    {
                        buildGameBoard(x, y);
                    }
                    groundTextureCollider[x,y] = new Rectangle(x * TILE_SIZE, y * TILE_SIZE, TILE_SIZE, TILE_SIZE);
                }
            }
            #endregion

            #region Position Troop
            //Position Troop
            bool found = false;
            for (int x = 0; x < GAME_BOARD_LENGTH; x++)
            {
                for (int y = 0; y < GAME_BOARD_WIDTH; y++)
                {
                    if (groundTextureType[x, y] == 1)
                    {
                        troopSpriteSheet = Content.Load<Texture2D>(TROOP_SPRITESHEET_NAME);
                        TroopPosition = new Vector2(x*TILE_SIZE+12, y*TILE_SIZE+53);
                        TroopBodyCollider = new Rectangle((int)TroopPosition.X - 12, (int)TroopPosition.Y-53, TROOP_BODY_SIZE_X, TROOP_BODY_SIZE_Y);
                        TroopFeetCollider = new Rectangle((int)TroopPosition.X-12, (int)TroopPosition.Y-14, TROOP_FEET_SIZE_X, TROOP_FEET_SIZE_Y);
                        found = true;
                        break;
                    }
                }
                if (found == true)
                {
                    break;
                }
            }
            #endregion
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
            spriteBatch.Begin();

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

            #region Moiveing The Troop
            #region Standing Still, Right, Left, Up, Down
            //Setsplayer to idle
            switch(_lastDirection)
            {
                case lastDirection.Left:
                    frameRow = walkingLeftFrameRow;
                    totalFrames = 0;
                    break;
                case lastDirection.Right:
                    frameRow = walkingRightFrameRow;
                    totalFrames = 0;
                    break;
                case lastDirection.Up:
                    frameRow = walkingUpFrameRow;
                    totalFrames = 0;
                    break;
                case lastDirection.Down:
                    frameRow = walkingDownFrameRow;
                    totalFrames = 0;
                    break;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
            {
                TroopPosition.X += movingSpeed;
                TroopBodyCollider.X += movingSpeed;
                TroopFeetCollider.X += movingSpeed;
                frameRow = walkingRightFrameRow;
                totalFrames = 8;
                _lastDirection = lastDirection.Right;

                //Get what the troop is intersecting with

                //if water stop moveing
                if (troopCollisionDectector(2))
                {
                    TroopPosition.X -= movingSpeed;
                    TroopBodyCollider.X -= movingSpeed;
                    TroopFeetCollider.X -= movingSpeed;
                }

                //if sand slow down
                if (troopCollisionDectector(3))
                {
                    movingSpeed = sandSpeed;
                    framePlayTime = sandPlaySpeed;
                }
                else
                {
                    movingSpeed = grassSpeed;
                    framePlayTime = normalPlaySpeed;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                TroopPosition.X -= movingSpeed;
                TroopBodyCollider.X -= movingSpeed;
                TroopFeetCollider.X -= movingSpeed;
                frameRow = walkingLeftFrameRow;
                totalFrames = 8;
                _lastDirection = lastDirection.Left;

                //Get what the troop is intersecting with

                //if water stop moveing
                if (troopCollisionDectector(2))
                {
                    TroopPosition.X += movingSpeed;
                    TroopBodyCollider.X += movingSpeed;
                    TroopFeetCollider.X += movingSpeed;
                }

                //if sand slow down
                if (troopCollisionDectector(3))
                {
                    movingSpeed = sandSpeed;
                    framePlayTime = sandPlaySpeed;
                }
                else
                {
                    movingSpeed = grassSpeed;
                    framePlayTime = normalPlaySpeed;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
            {
                TroopPosition.Y -= movingSpeed;
                TroopBodyCollider.Y -= movingSpeed;
                TroopFeetCollider.Y -= movingSpeed;
                frameRow = walkingUpFrameRow;
                totalFrames = 8;
                _lastDirection = lastDirection.Up;

                //Get what the troop is intersecting with

                //if water stop moveing
                if (troopCollisionDectector(2))
                {
                    TroopPosition.Y += movingSpeed;
                    TroopBodyCollider.Y += movingSpeed;
                    TroopFeetCollider.Y += movingSpeed;
                }

                //if sand slow down
                if (troopCollisionDectector(3))
                {
                    movingSpeed = sandSpeed;
                    framePlayTime = sandPlaySpeed;
                }
                else
                {
                    movingSpeed = grassSpeed;
                    framePlayTime = normalPlaySpeed;
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
            {
                TroopPosition.Y += movingSpeed;
                TroopBodyCollider.Y += movingSpeed;
                TroopFeetCollider.Y += movingSpeed;
                frameRow = walkingDownFrameRow;
                totalFrames = 8;
                _lastDirection = lastDirection.Down;

                //Get what the troop is intersecting with

                //if water stop moveing
                if (troopCollisionDectector(2))
                {
                    TroopPosition.Y -= movingSpeed;
                    TroopBodyCollider.Y -= movingSpeed;
                    TroopFeetCollider.Y -= movingSpeed;
                }

                //if sand slow down
                if (troopCollisionDectector(3))
                {
                    movingSpeed = sandSpeed;
                    framePlayTime = sandPlaySpeed;
                }
                else
                {
                    movingSpeed = grassSpeed;
                    framePlayTime = normalPlaySpeed;
                }
            }
            #endregion

            #region Troop Game Boarder Collider
            if (TroopBodyCollider.X + TROOP_BODY_SIZE_X > GAME_BOARD_WIDTH * TILE_SIZE)
            {
                TroopPosition.X -= movingSpeed;
                TroopBodyCollider.X -= movingSpeed;
                TroopFeetCollider.X -= movingSpeed;
            }
            if (TroopBodyCollider.X < 0)
            {
                TroopPosition.X += movingSpeed;
                TroopBodyCollider.X += movingSpeed;
                TroopFeetCollider.X += movingSpeed;
            }
            if (TroopBodyCollider.Y + TROOP_BODY_SIZE_Y > GAME_BOARD_LENGTH * TILE_SIZE)
            {
                TroopPosition.Y -= movingSpeed;
                TroopBodyCollider.Y -= movingSpeed;
                TroopFeetCollider.Y -= movingSpeed;
            }
            if (TroopBodyCollider.Y < 0)
            {
                TroopPosition.Y += movingSpeed;
                TroopBodyCollider.Y += movingSpeed;
                TroopFeetCollider.Y += movingSpeed;
            }
            #endregion

            #region Troop Finish Colider
            if (troopCollisionDectector(4))
            {
                _state = GameState.EndOfGame;
            }
            #endregion

            #region Troop Animation
            //Animation
            // Process elapsed time
            timeShown += (float)deltaTime.ElapsedGameTime.TotalSeconds;
            while (timeShown > framePlayTime)
            {
                // Play the next frame in the SpriteSheet
                frameIndex++;

                // reset elapsed time
                timeShown = 0f;
            }
            if (frameIndex > totalFrames) frameIndex = 0;
            #endregion
            #endregion

            //Set Collider Positions


            //if (playerDied)
            //    _state = GameState.EndOfGame;
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

            //Draw Game Board
            for (int x = 0; x < GAME_BOARD_WIDTH; x++)
            {
                for (int y = 0; y < GAME_BOARD_LENGTH; y++)
                {
                    Vector2 groundPosition = new Vector2(x * TILE_SIZE, y * TILE_SIZE);
                    spriteBatch.Draw(groundTexture[x, y], groundPosition, Color.White);
                }
            }

            //Draw Troop Animation
            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(frameIndex * frameWidth, frameRow * frameHeight, frameWidth, frameHeight);
            // Calculate position and origin to draw in the center of the screen
            Vector2 position = new Vector2(TroopPosition.X, TroopPosition.Y);
            Vector2 origin = new Vector2(frameWidth / 2.0f, frameHeight);
            // Draw the current frame.
            spriteBatch.Draw(troopSpriteSheet, position, source, Color.White, 0.0f,
              origin, 1.0f, SpriteEffects.None, 0.0f);

            //Draw Text
            spriteBatch.DrawString(font, "Position: " + TroopPosition.ToString(), new Vector2(0,0), Color.Red);
            spriteBatch.DrawString(font, "Body Collider Position: X: " + TroopBodyCollider.X + " Y: " + TroopBodyCollider.Y, new Vector2(0, 14), Color.Red);
            spriteBatch.DrawString(font, "Feet Collider Position: X: " + TroopFeetCollider.X + " Y: " + TroopFeetCollider.Y, new Vector2(0, 28), Color.Red);
            spriteBatch.DrawString(font, "Current Tile Type: Ground:" + troopCollisionDectector(1) + "Sand: " + troopCollisionDectector(3), new Vector2(0, 42), Color.Red);
        }

        void DrawEndOfGame(GameTime deltaTime)
        {
            // Draw text and scores
            // Draw menu for restarting level or going back to main menu
        }

        public bool troopCollisionDectector(int landTypeToCheck)
        {
            for (int x = 0; x < GAME_BOARD_WIDTH; x++)
            {
                for (int y = 0; y < GAME_BOARD_LENGTH; y++)
                {
                    if (TroopFeetCollider.Intersects(groundTextureCollider[x, y]))
                    {
                        if (groundTextureType[x,y] == landTypeToCheck)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
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
                groundTexture[x, y] = Content.Load<Texture2D>(WATER_TILE_NAME);
                groundTextureType[x, y] = 2;
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

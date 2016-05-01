using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace War_Game
{
    class PlayerOrigional
    {
        public const int PLAYER_BODY_SIZE_X = 24;
        public const int PLAYER_BODY_SIZE_Y = 53;
        public const int PLAYER_FEET_SIZE_X = 24;
        public const int PLAYER_FEET_SIZE_Y = 10;
        private int gameBoardWidth;
        private int gameBoardLength;
        private int viewPortWidth;
        private int viewPortLength;
        private Point viewPortTopLeftIndex;
        private int tileSize;
        private int[,] groundTextureType;
        private Rectangle[,] groundTextureCollider;

        #region Moving Speeds
        private int movingSpeed = 2;
        private const int sandSpeed = 1;
        private const int grassSpeed = 2;
        #endregion

        #region Player
        //player Health
        private int playerHealth = 100;
        //is player alive
        private bool isPlayerAlive = true;
        //Player Position
        public Vector2 PlayerPosition;
        //player Tile Loaction
        public Vector2 PlayerTilePosition;
        //Player Feet Collider
        public Rectangle PlayerFeetCollider;
        //Player Bosy Collider
        public Rectangle PlayerBodyCollider;
        // the spritesheet containing our animation frames
        public Texture2D playerSpriteSheet;
        // duration of time to show each frame
        private float framePlayTime = 0.1f;
        //Normal frame play speed
        private const float normalPlaySpeed = 0.1f;
        //Sand frame play speed
        private const float sandPlaySpeed = 0.15f;
        // an index of the current frame being shown
        public int frameIndex = 0;
        // the current frame row being shown
        public int frameRow = 0;
        // total number of frames in our spritesheet animation row
        private int totalFrames = 8;
        // define the size of our animation frame
        public int frameHeight = 64;
        public int frameWidth = 64;
        //Frame Row Name with Numbers
        private const int walkingLeftFrameRow = 9;
        private const int walkingRightFrameRow = 11;
        private const int walkingUpFrameRow = 8;
        private const int walkingDownFrameRow = 10;
        //animation stuff
        public Rectangle source;
        public Vector2 origin;
        //walking state selector
        public enum lastDirection
        {
            Left,
            Right,
            Up,
            Down,
        }
        public lastDirection _lastDirection;
        #endregion

        #region Public Get Sets
        public bool IsPlayerAlive
        {
            get
            {
                return isPlayerAlive;
            }
        }

        public int PlayerHealth
        {
            get
            {
                return playerHealth;
            }
            set
            {
                playerHealth = value;
                if(playerHealth < 0)
                {
                    isPlayerAlive = false;
                }
            }
        }
        public Point ViewPortTopLeftIndex
        {
            get
            {
                return viewPortTopLeftIndex;
            }
            set
            {
                viewPortTopLeftIndex.X = value.X;
                viewPortTopLeftIndex.Y = value.Y;
            }
        }
        #endregion

        public PlayerOrigional(int gameBoardWidthSent, int gameBoardLengthSent, int viewPortWidthSent, int viewPortLengthSent, Point viewPortTopLeftIndexSent, int tileSizeSent, int[,] groundTextureTypeArray, Rectangle[,] groundTextureColliderArray)
        {
            gameBoardWidth = gameBoardWidthSent;
            gameBoardLength = gameBoardLengthSent;
            viewPortWidth = viewPortWidthSent;
            viewPortLength = viewPortLengthSent;
            viewPortTopLeftIndex = viewPortTopLeftIndexSent;
            tileSize = tileSizeSent;
            groundTextureType = groundTextureTypeArray;
            groundTextureCollider = groundTextureColliderArray;
        }

        public void LoadPlayer()
        {
            PlayerPosition.X += 12;
            PlayerPosition.Y += 14;
            PlayerBodyCollider = new Rectangle((int)PlayerPosition.X - 12, (int)PlayerPosition.Y - 53, PLAYER_BODY_SIZE_X, PLAYER_BODY_SIZE_Y);
            PlayerFeetCollider = new Rectangle((int)PlayerPosition.X - 12, (int)PlayerPosition.Y - 14, PLAYER_FEET_SIZE_X, PLAYER_FEET_SIZE_Y);                      
        }

        public void UpdatePlayer()
        {
            #region Moiveing The Player
            #region Standing Still, Right, Left, Up, Down
            //Setsplayer to idle
            switch (_lastDirection)
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
                PlayerPosition.X += movingSpeed;
                PlayerBodyCollider.X += movingSpeed;
                PlayerFeetCollider.X += movingSpeed;
                frameRow = walkingRightFrameRow;
                totalFrames = 8;
                _lastDirection = lastDirection.Right;

                //Get what the player is intersecting with

                //if water stop moveing
                if (playerCollisionDectector(2))
                {
                    PlayerPosition.X -= movingSpeed;
                    PlayerBodyCollider.X -= movingSpeed;
                    PlayerFeetCollider.X -= movingSpeed;
                }

                //if sand slow down
                if (playerCollisionDectector(3))
                {
                    movingSpeed = sandSpeed;
                    framePlayTime = sandPlaySpeed;
                }
                else
                {
                    movingSpeed = grassSpeed;
                    framePlayTime = normalPlaySpeed;
                }
                PlayerTilePosition = playerCollisionLocator();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
            {
                PlayerPosition.X -= movingSpeed;
                PlayerBodyCollider.X -= movingSpeed;
                PlayerFeetCollider.X -= movingSpeed;
                frameRow = walkingLeftFrameRow;
                totalFrames = 8;
                _lastDirection = lastDirection.Left;

                //Get what the player is intersecting with

                //if water stop moveing
                if (playerCollisionDectector(2))
                {
                    PlayerPosition.X += movingSpeed;
                    PlayerBodyCollider.X += movingSpeed;
                    PlayerFeetCollider.X += movingSpeed;
                }

                //if sand slow down
                if (playerCollisionDectector(3))
                {
                    movingSpeed = sandSpeed;
                    framePlayTime = sandPlaySpeed;
                }
                else
                {
                    movingSpeed = grassSpeed;
                    framePlayTime = normalPlaySpeed;
                }
                PlayerTilePosition = playerCollisionLocator();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
            {
                PlayerPosition.Y -= movingSpeed;
                PlayerBodyCollider.Y -= movingSpeed;
                PlayerFeetCollider.Y -= movingSpeed;
                frameRow = walkingUpFrameRow;
                totalFrames = 8;
                _lastDirection = lastDirection.Up;

                //Get what the player is intersecting with

                //if water stop moveing
                if (playerCollisionDectector(2))
                {
                    PlayerPosition.Y += movingSpeed;
                    PlayerBodyCollider.Y += movingSpeed;
                    PlayerFeetCollider.Y += movingSpeed;
                }

                //if sand slow down
                if (playerCollisionDectector(3))
                {
                    movingSpeed = sandSpeed;
                    framePlayTime = sandPlaySpeed;
                }
                else
                {
                    movingSpeed = grassSpeed;
                    framePlayTime = normalPlaySpeed;
                }
                PlayerTilePosition = playerCollisionLocator();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
            {
                PlayerPosition.Y += movingSpeed;
                PlayerBodyCollider.Y += movingSpeed;
                PlayerFeetCollider.Y += movingSpeed;
                frameRow = walkingDownFrameRow;
                totalFrames = 8;
                _lastDirection = lastDirection.Down;

                //Get what the player is intersecting with

                //if water stop moveing
                if (playerCollisionDectector(2))
                {
                    PlayerPosition.Y -= movingSpeed;
                    PlayerBodyCollider.Y -= movingSpeed;
                    PlayerFeetCollider.Y -= movingSpeed;
                }

                //if sand slow down
                if (playerCollisionDectector(3))
                {
                    movingSpeed = sandSpeed;
                    framePlayTime = sandPlaySpeed;
                }
                else
                {
                    movingSpeed = grassSpeed;
                    framePlayTime = normalPlaySpeed;
                }
                PlayerTilePosition = playerCollisionLocator();
            }
            #endregion

            #region Player Game Boarder Collider
            if (PlayerBodyCollider.X + PLAYER_BODY_SIZE_X > gameBoardWidth * tileSize)
            {
                PlayerPosition.X -= movingSpeed;
                PlayerBodyCollider.X -= movingSpeed;
                PlayerFeetCollider.X -= movingSpeed;
            }
            if (PlayerBodyCollider.X < 0)
            {
                PlayerPosition.X += movingSpeed;
                PlayerBodyCollider.X += movingSpeed;
                PlayerFeetCollider.X += movingSpeed;
            }
            if (PlayerBodyCollider.Y + PLAYER_BODY_SIZE_Y > gameBoardLength * tileSize)
            {
                PlayerPosition.Y -= movingSpeed;
                PlayerBodyCollider.Y -= movingSpeed;
                PlayerFeetCollider.Y -= movingSpeed;
            }
            if (PlayerBodyCollider.Y < 0)
            {
                PlayerPosition.Y += movingSpeed;
                PlayerBodyCollider.Y += movingSpeed;
                PlayerFeetCollider.Y += movingSpeed;
            }
            #endregion
            #endregion
        }

        public float UpdatePlayerAnimation(float timeShown)
        {
            #region Player Animation
            while (timeShown > framePlayTime)
            {
                // Play the next frame in the SpriteSheet
                frameIndex++;

                // reset elapsed time
                timeShown = 0f;
            }
            if (frameIndex > totalFrames)
                frameIndex = 0;
            return timeShown;
            #endregion
        }

        public void DrawPlayer()
        {
            //Draw Player Animation
            // Calculate the source rectangle of the current frame.
            source = new Rectangle(frameIndex * frameWidth, frameRow * frameHeight, frameWidth, frameHeight);
            // Calculate origin to draw in the center of the screen
            origin = new Vector2(frameWidth / 2.0f, frameHeight);
        }

        public bool playerCollisionDectector(int landTypeToCheck)
        {
            for (int x = 0; x < viewPortWidth; x++)
            {
                for (int y = 0; y < viewPortLength; y++)
                {
                    if (PlayerFeetCollider.Intersects(groundTextureCollider[x + viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y]))
                    {
                        if (groundTextureType[x + viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y] == landTypeToCheck)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public Vector2 playerCollisionLocator()
        {
            Vector2 location;
            location.X = 0;
            location.Y = 0;

            for (int x = 0; x < viewPortWidth; x++)
            {
                for (int y = 0; y < viewPortLength; y++)
                {
                    if (PlayerFeetCollider.Intersects(groundTextureCollider[x + viewPortTopLeftIndex.X, y + viewPortTopLeftIndex.Y]))
                    {
                        location.X = x + viewPortTopLeftIndex.X;
                        location.Y = y + viewPortTopLeftIndex.Y;
                        return location;
                        break;
                    }
                }
            }
            return location;
        }
    }
}

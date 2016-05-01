using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace War_Game
{
    class GameStatusBar
    {
        #region Game StatusBar
        //Game Menu Constants
        public const string AMMO_SPRITESHEET_NAME = "GameMenu/ammoSpriteSheet.png";
        public const string HEALTHBAR_SPRITESHEET_NAME = "GameMenu/healthBarSpriteSheet.png";
        public const string STATUSBAR_SPRITESHEET_NAME = "GameMenu/menuHeaderBackground.png";
        public const int STATUSBAR_WIDTH = 800;
        public const int STATUSBAR_LENGTH = 60;

        //Font Style
        private SpriteFont gameStatusFont;
        //Menu Textures
        private Texture2D statusBarBackgroundTexture;
        private Texture2D healthBarTexture;
        private Texture2D ammoAvailableTexture;
        //Menu Positions
        private Vector2 statusBarBackgroundPosition;
        private Vector2 healthBarPosition;
        private Vector2 ammoAvailablePosition;
        private Vector2 killCountTextPosition;
        // ammo frameRow
        private int ammoFrameRow = 0;
        // healthBar frameRow
        private int healthBarFrameRow = 0;
        // ammo frameIndex
        private int ammoFrameIndex = 0;
        // healthBar frameIndex
        private int healthBarFrameIndex = 0;
        // define the size of our healthBar frame
        private int healthBarFrameHeight = 30;
        private int healthBarFrameWidth = 150;
        // define the size of our Ammo frame
        private int ammoFrameHeight = 60;
        private int ammoFrameWidth = 150;
        //health bar animation stuff
        public Rectangle healthBarSource;
        public Vector2 healthBarOrigin;
        //ammo animation stuff
        public Rectangle ammoSource;
        public Vector2 ammoOrigin;

        #region public properties
        //Public Properties
        public SpriteFont GameStatusFont
        {
            get
            {
                return gameStatusFont;
            }

            set
            {
                gameStatusFont = value;
            }
        }

        public Texture2D StatusBarBackgroundTexture
        {
            get
            {
                return statusBarBackgroundTexture;
            }

            set
            {
                statusBarBackgroundTexture = value;
            }
        }
        public Texture2D HealthBarTexture
        {
            get
            {
                return healthBarTexture;
            }

            set
            {
                healthBarTexture = value;
            }
        }
        public Texture2D AmmoAvailableTexture
        {
            get
            {
                return ammoAvailableTexture;
            }

            set
            {
                ammoAvailableTexture = value;
            }
        }
        public Vector2 StatusBarBackgroundPosition
        {
            get
            {
                return statusBarBackgroundPosition;
            }
            set
            {
                statusBarBackgroundPosition = value;
            }
        }
        public Vector2 HealthBarPosition
        {
            get
            {
                return healthBarPosition;
            }
            set
            {
                healthBarPosition = value;
            }
        }
        public Vector2 AmmoAvailablePosition
        {
            get
            {
                return ammoAvailablePosition;
            }
            set
            {
                ammoAvailablePosition = value;
            }
        }
        public Vector2 KillCountTextPosition
        {
            get
            {
                return killCountTextPosition;
            }
            set
            {
                killCountTextPosition = value;
            }
        }
        #endregion
        #endregion

        public GameStatusBar()
        {

        }

       public void loadGameStatusBar(int windowSize)
       {
           #region GameMenu
           statusBarBackgroundPosition = new Vector2((windowSize / 2)- 400, 2);
           healthBarPosition = new Vector2(statusBarBackgroundPosition.X + 225, statusBarBackgroundPosition.Y + 43);
           ammoAvailablePosition = new Vector2(statusBarBackgroundPosition.X + 727, statusBarBackgroundPosition.Y + 57);
           killCountTextPosition = new Vector2(statusBarBackgroundPosition.X + 450, StatusBarBackgroundPosition.Y + 6);
           #endregion
       }

        public void drawStatusBar()
       {
           // Calculate the source rectangle of the current frame.
           healthBarSource = new Rectangle(healthBarFrameIndex * healthBarFrameWidth, healthBarFrameRow * healthBarFrameHeight, healthBarFrameWidth, healthBarFrameHeight);
           // Calculate origin to draw in the center of the screen
           healthBarOrigin = new Vector2(healthBarFrameWidth / 2.0f, healthBarFrameHeight);

           // Calculate the source rectangle of the current frame.
           ammoSource = new Rectangle(ammoFrameIndex * ammoFrameWidth, ammoFrameRow * ammoFrameHeight, ammoFrameWidth, ammoFrameHeight);
           // Calculate origin to draw in the center of the screen
           ammoOrigin = new Vector2(ammoFrameWidth / 2.0f, ammoFrameHeight);
       }

        public void UpdateStatusBarAnimation(int playerHealth, int ammoAmount, int kills)
        {
            #region HealthBar Animation
            if (playerHealth > 90)
            {
                healthBarFrameRow = 0;
            }
            else if (playerHealth > 80)
            {
                healthBarFrameRow = 1;
            }
            else if (playerHealth > 70)
            {
                healthBarFrameRow = 2;
            }
            else if (playerHealth > 60)
            {
                healthBarFrameRow = 3;
            }
            else if (playerHealth > 50)
            {
                healthBarFrameRow = 4;
            }
            else if (playerHealth > 40)
            {
                healthBarFrameRow = 5;
            }
            else if (playerHealth > 30)
            {
                healthBarFrameRow = 6;
            }
            else if (playerHealth > 20)
            {
                healthBarFrameRow = 7;
            }
            else if (playerHealth > 10)
            {
                healthBarFrameRow = 8;
            }
            else if (playerHealth > 0)
            {
                healthBarFrameRow = 9;
            }
            else if (playerHealth < 0)
            {
                healthBarFrameRow = 10;
            }
            #endregion

            #region Ammo Amount Animation
            if (ammoAmount == 10)
            {
                ammoFrameRow = 0;
            }
            else if (ammoAmount == 9)
            {
                ammoFrameRow = 1;
            }
            else if (ammoAmount == 8)
            {
                ammoFrameRow = 2;
            }
            else if (ammoAmount == 7)
            {
                ammoFrameRow = 3;
            }
            else if (ammoAmount == 6)
            {
                ammoFrameRow = 4;
            }
            else if (ammoAmount == 5)
            {
                ammoFrameRow = 5;
            }
            else if (ammoAmount == 4)
            {
                ammoFrameRow = 6;
            }
            else if (ammoAmount == 3)
            {
                ammoFrameRow = 7;
            }
            else if (ammoAmount == 2)
            {
                ammoFrameRow = 8;
            }
            else if (ammoAmount == 1)
            {
                ammoFrameRow = 9;
            }
            else if (ammoAmount == 0)
            {
                ammoFrameRow = 10;
            }
            #endregion


        }
    }
}

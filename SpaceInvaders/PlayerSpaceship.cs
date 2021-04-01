using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class PlayerSpaceship : Spaceship
    {
        #region Fields
        #endregion

        #region Constructor
        /// <summary>
        /// Constructeur
        /// </summary>
        public PlayerSpaceship(Vecteur2D position, int lives, Bitmap image, Side side) : base(position, lives, image, side)
        {
        }
        #endregion

        #region Methods

        public override void Update(Game gameInstance, double deltaT)
        {
            if (gameInstance.keyPressed.Contains(Keys.Left))
            {
                Position.x -= speedPixelPerSecond * deltaT;
            }
            else if (gameInstance.keyPressed.Contains(Keys.Right))
            {
                Position.x += speedPixelPerSecond * deltaT;
            }
            if (Position.x < 0)
            {
                Position.x = 0;
            }
            else if (Position.x + Image.Width > gameInstance.gameSize.Width)
            {
                Position.x = gameInstance.gameSize.Width - Image.Width;
            }
            if (gameInstance.keyPressed.Contains(Keys.Space))
            {
                this.Shoot(gameInstance);
                gameInstance.ReleaseKey(Keys.Space);
            }
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            base.Draw(gameInstance, graphics);
            graphics.DrawString("Vie(s) : " + this.Lives.ToString(), new Font("Arial", 15), new SolidBrush(Color.Black), gameInstance.gameSize.Width - 100, 10);
        }
        #endregion
    }
}

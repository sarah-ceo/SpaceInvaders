using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;


namespace SpaceInvaders
{
    class Spaceship : SimpleObject
    {
        #region Fields

        /// <summary>
        /// Vitesse du vaisseau en pixel/seconde
        /// </summary>
        protected double speedPixelPerSecond = 200;

        public Missile missile = null;

        protected bool alive = true;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructeur
        /// </summary>
        public Spaceship(Vecteur2D position, int lives, Bitmap image, Side side) : base(position, lives, image, side)
        {
        }
        #endregion

        #region Methods
        
        public override void Update(Game gameInstance, double deltaT)
        {
        }
        
        public void Shoot(Game gameInstance)
        {
            if (missile == null || !missile.IsAlive())
            {
                if (this is PlayerSpaceship)
                {
                    this.missile = new Missile(new Vecteur2D(Position.x + (Image.Width / 2), Position.y - SpaceInvaders.Properties.Resources.shoot1.Height), 1, SpaceInvaders.Properties.Resources.shoot1, Side.Ally);
                }
                else
                {
                    this.missile = new Missile(new Vecteur2D(Position.x + (Image.Width / 2), Position.y + SpaceInvaders.Properties.Resources.shoot1.Height), 1, SpaceInvaders.Properties.Resources.shoot1, Side.Enemy);
                }
                gameInstance.AddNewGameObject(missile);
            }
        }

        protected override void OnCollision(Missile m, int numberOfPixelsInCollision)
        {
            if (m.Lives <= this.Lives)
            {
                this.Lives -= m.Lives;
                m.Lives -= m.Lives;
            }
            else
            {
                this.Lives -= this.Lives;
                m.Lives -= this.Lives;
            }
        }
        #endregion
    }
}

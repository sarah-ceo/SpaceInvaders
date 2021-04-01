using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SpaceInvaders
{
    class Missile : SimpleObject
    {
        #region Fields
        
        /// <summary>
        /// Vitesse du missile
        /// </summary>
        public int Vitesse { get; set; } = 600;
        
        #endregion

        #region Constructor
        /// <summary>
        /// Constructeur
        /// </summary>
        public Missile(Vecteur2D position, int lives, Bitmap image, Side side) : base(position, lives, image, side)
        {
        }
        #endregion

        #region Methods

        public override void Update(Game gameInstance, double deltaT)
        {
            if(this.Camp == Side.Ally)
            {
                Position.y -= Vitesse * deltaT;
            } else
            {
                Position.y += Vitesse * deltaT;
            }
            
            if (Position.y < 0 || Position.y >gameInstance.gameSize.Height)
                Lives = 0;
            foreach (GameObject obj in gameInstance.gameObjects)
            {
                if (obj != this)
                {
                    obj.Collision(this);
                }
            }
        }

        protected override void OnCollision(Missile m, int numberOfPixelsInCollision)
        {
            this.Lives = 0;
            m.Lives = 0;
        }

        #endregion
    }
}

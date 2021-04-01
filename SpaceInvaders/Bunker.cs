using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class Bunker : SimpleObject
    {
        #region Fields
        #endregion

        #region Constructor
        /// <summary>
        /// Constructeur
        /// </summary>
        public Bunker(Vecteur2D position, Side side) : base(position, SpaceInvaders.Properties.Resources.bunker.Width * SpaceInvaders.Properties.Resources.bunker.Height, SpaceInvaders.Properties.Resources.bunker, side)
        {
        }
        #endregion

        #region Methods

        public override void Update(Game gameInstance, double deltaT)
        {
        }

        protected override void OnCollision(Missile m, int numberOfPixelsInCollision)
        {
            this.Lives -= numberOfPixelsInCollision;
            m.Lives -= numberOfPixelsInCollision;
        }
        #endregion
    }

}

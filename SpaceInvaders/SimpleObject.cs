using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceInvaders
{
    abstract class SimpleObject : GameObject
    {
        #region Fields
        /// <summary>
        /// Position de l'objet
        /// </summary>
        public Vecteur2D Position { get; set; }

        /// <summary>
        /// Image de l'objet
        /// </summary>
        public Bitmap Image { get; set; }

        /// <summary>
        /// Nombre de vies de l'objet
        /// </summary>
        public int Lives { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Constructeurs
        /// </summary>
        public SimpleObject(Vecteur2D position, int lives, Bitmap image, Side side) : base(side)
        {
            this.Position = position;
            this.Lives = lives;
            this.Image = image;
        }
        #endregion

        #region Methods
        public override void Update(Game gameInstance, double deltaT)
        {
        }
        public override void Draw(Game gameInstance, Graphics graphics)
        {
            graphics.DrawImage(Image, (float)Position.x, (float)Position.y, Image.Width, Image.Height);
        }

        public override bool IsAlive()
        {
            if (Lives > 0)
            {
                return true;
            }
            return false;
        }

        public override void Collision(Missile m)
        {
            if (!(this.Camp.Equals(m.Camp)))
            {
                if (collisionRectangle(m))
                {
                    int nbPixelsInCollision = 0;
                    for (int i = 0; i < m.Image.Width; i++)
                    {
                        for (int j = 0; j < m.Image.Height; j++)
                        {
                            if (m.Image.GetPixel(i, j).Equals(Color.FromArgb(255, 0, 0, 0)))
                            {
                                Vecteur2D positionPixelMissileSurEcran = new Vecteur2D(m.Position.x + i, m.Position.y + j);
                                Vecteur2D positionPixelSurObjet = new Vecteur2D(positionPixelMissileSurEcran.x - this.Position.x, positionPixelMissileSurEcran.y - this.Position.y);
                                if (((positionPixelSurObjet.x >= 0) && (positionPixelSurObjet.x < this.Image.Width)) && ((positionPixelSurObjet.y >= 0) && (positionPixelSurObjet.y < this.Image.Height)))
                                {
                                    if (collisionPixel(positionPixelSurObjet))
                                    {
                                        this.Image.SetPixel((int)positionPixelSurObjet.x, (int)positionPixelSurObjet.y, Color.FromArgb(0, 255, 255, 255));
                                        nbPixelsInCollision += 1;
                                    }
                                }
                            }
                        }
                    }
                    OnCollision(m, nbPixelsInCollision);
                }
            }
        }
        public bool collisionRectangle(Missile m)
        {
            if (m.Position.x > this.Position.x + this.Image.Width || m.Position.y > this.Position.y + this.Image.Height
                || this.Position.x > m.Position.x + m.Image.Width || this.Position.y > m.Position.y + m.Image.Height)
            {
                return false;
            }
            return true;
        }

        public bool collisionPixel(Vecteur2D positionPixelSurObjet)
        {
            if (this.Image.GetPixel((int)positionPixelSurObjet.x, (int)positionPixelSurObjet.y).Equals(Color.FromArgb(255, 0, 0, 0)))
            {
                return true;
            }
            return false;
        }

        protected abstract void OnCollision(Missile m, int numberOfPixelsInCollision);
        #endregion
    }
}

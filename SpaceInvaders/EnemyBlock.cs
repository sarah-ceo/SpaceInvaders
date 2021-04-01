using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class EnemyBlock : GameObject
    {
        #region Fields

        private HashSet<Spaceship> enemyShips = new HashSet<Spaceship>();
        private int baseWidth;
        public Size Size { get; set; }
        public Vecteur2D Position { get; set; }
        private double speedPixelPerSecond;
        public string direction;
        private double randomShootProbability = 0.2;
        #endregion

        #region Constructor
        public EnemyBlock(int baseWidth, Vecteur2D Position, Side side, int Niveau) : base(side)
        {
            this.baseWidth = baseWidth;
            this.Position = Position;
            this.Size = this.Size + new Size(baseWidth, 0); ;
            this.direction = "droite";
            this.speedPixelPerSecond = 40 + (Niveau*10);
        }
        #endregion

        #region Methods
        public void AddLine(int nbShips, int nbLives, Bitmap shipImage)
        {
            int enemyBlock_zone = this.Size.Width / nbShips;
            for (int i=0; i<nbShips; i++)
            {
                this.enemyShips.Add(new Spaceship(new Vecteur2D(this.Position.x + (i * enemyBlock_zone + ((enemyBlock_zone - shipImage.Width) / 2)), this.Position.y+this.Size.Height), nbLives, new Bitmap (shipImage), Side.Enemy));
            }
            UpdateSize();
        }

        public void UpdateSize()
        {
            double minX = enemyShips.Min(enemyShip => enemyShip.Position.x);
            double maxX = enemyShips.Max(enemyShip => enemyShip.Position.x);
            double minY = enemyShips.Min(enemyShip => enemyShip.Position.y);
            double maxY = enemyShips.Max(enemyShip => enemyShip.Position.y);
            int width = (int)((maxX+40) - minX)+1;
            int height = (int)((maxY+30) - minY)+1;
            this.Size = new Size(width, height);
            this.Position.x = minX;
            this.Position.y = minY;
        }

        public override void Update(Game gameInstance, double deltaT)
        {
            switch (this.direction)
            {
                case "droite" :
                    this.Position.x += speedPixelPerSecond * deltaT;
                    foreach (Spaceship enemy in enemyShips)
                    {
                        enemy.Position.x += speedPixelPerSecond * deltaT;
                    }
                    break;
                case "gauche":
                    this.Position.x -= speedPixelPerSecond * deltaT;
                    foreach (Spaceship enemy in enemyShips)
                    {
                        enemy.Position.x -= speedPixelPerSecond * deltaT;
                    }
                    break;
            }
            if (this.Position.x < 0)
            {
                this.direction = "droite";
                this.Position.y += 30;
                foreach (Spaceship enemy in enemyShips)
                {
                    enemy.Position.y += 30;
                }
                this.speedPixelPerSecond += 5;
                this.randomShootProbability += 0.1;
            }
            else if (this.Position.x + this.Size.Width > gameInstance.gameSize.Width)
            {
                this.direction = "gauche";
                foreach (Spaceship enemy in enemyShips)
                {
                    enemy.Position.y += 30;
                }
                this.Position.y += 30;
                this.speedPixelPerSecond += 5;
                this.randomShootProbability += 0.1;
            }
            Random random = new Random();
            for (int i=0; i<enemyShips.Count; i++)
            {
                double r = random.NextDouble();
                Spaceship enemy = enemyShips.ElementAt(i);
                if (r <= randomShootProbability * deltaT)
                {
                    enemy.Shoot(gameInstance);
                }
            }
        }

        public override void Draw(Game gameInstance, Graphics graphics)
        {
            foreach(Spaceship enemy in this.enemyShips)
            {
                graphics.DrawImage(enemy.Image, (float)enemy.Position.x, (float)enemy.Position.y, enemy.Image.Width, enemy.Image.Height);
            }
        }

        public override bool IsAlive()
        {
            enemyShips.RemoveWhere(enemyShip => !enemyShip.IsAlive());
            if (enemyShips.Count > 0)
            {
                UpdateSize();
            }
            return enemyShips.Count>0;
        }

        public override void Collision(Missile m)
        {
            foreach (Spaceship enemy in enemyShips)
            {
                enemy.Collision(m);
            }
        }
        #endregion
    }
}

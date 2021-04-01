using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace SpaceInvaders
{
    class Game
    {

        #region GameObjects management
        /// <summary>
        /// Set of all game objects currently in the game
        /// </summary>
        public HashSet<GameObject> gameObjects = new HashSet<GameObject>();

        /// <summary>
        /// Set of new game objects scheduled for addition to the game
        /// </summary>
        private HashSet<GameObject> pendingNewGameObjects = new HashSet<GameObject>();

        /// <summary>
        /// Schedule a new object for addition in the game.
        /// The new object will be added at the beginning of the next update loop
        /// </summary>
        /// <param name="gameObject">object to add</param>
        public void AddNewGameObject(GameObject gameObject)
        {
            pendingNewGameObjects.Add(gameObject);
        }

        /// <summary>
        /// PlayerSpaceship
        /// </summary>
        public PlayerSpaceship playership;
        
        /// <summary>
        /// Bunkers
        /// </summary>
        public List<Bunker> bunkers = new List<Bunker>();
        int nb_bunkers = 3;

        /// <summary>
        /// Enemies
        /// </summary>
        private EnemyBlock enemies;
        
        #endregion

        #region game technical elements
        /// <summary>
        /// Size of the game area
        /// </summary>
        public Size gameSize;

        /// <summary>
        /// State of the keyboard
        /// </summary>
        public HashSet<Keys> keyPressed = new HashSet<Keys>();

        /// <summary>
        /// GameState enumeration
        /// </summary>
        private enum GameState
        {
            Play,
            Pause,
            Win,
            Lost
        }

        /// <summary>
        /// GameState variable
        /// </summary>
        private GameState state = GameState.Play;

        private int Niveau = 1;


        #endregion

        #region static fields (helpers)

        /// <summary>
        /// Singleton for easy access
        /// </summary>
        public static Game game { get; private set; }

        /// <summary>
        /// A shared black brush
        /// </summary>
        private static Brush blackBrush = new SolidBrush(Color.Black);

        /// <summary>
        /// A shared simple font
        /// </summary>
        private static Font defaultFont = new Font("Times New Roman", 24, FontStyle.Bold, GraphicsUnit.Pixel);
        #endregion


        #region constructors
        /// <summary>
        /// Singleton constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        /// 
        /// <returns></returns>
        public static Game CreateGame(Size gameSize)
        {
            if (game == null)
                game = new Game(gameSize);
            return game;
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="gameSize">Size of the game area</param>
        private Game(Size gameSize)
        {
            this.gameSize = gameSize;
            this.playership = new PlayerSpaceship(new Vecteur2D(gameSize.Width / 2 - 12, gameSize.Height-35), 3, SpaceInvaders.Properties.Resources.ship3, GameObject.Side.Ally);
            AddNewGameObject(playership);
            for (int i = 0; i<nb_bunkers; i++)
            {
                int bunker_zone = gameSize.Width/nb_bunkers;
                this.bunkers.Add(new Bunker(new Vecteur2D(i*bunker_zone + ((bunker_zone-87)/2), gameSize.Height - 100), GameObject.Side.Neutral));
                AddNewGameObject(this.bunkers[i]);
            }
            this.enemies = new EnemyBlock(350, new Vecteur2D(0, 25), GameObject.Side.Enemy, this.Niveau);
            this.enemies.AddLine(8, 3, SpaceInvaders.Properties.Resources.ship1);
            this.enemies.AddLine(7, 2, SpaceInvaders.Properties.Resources.ship2);
            this.enemies.AddLine(6, 1, SpaceInvaders.Properties.Resources.ship9);
            AddNewGameObject(this.enemies);
        }

        #endregion

        #region methods

        /// <summary>
        /// Force a given key to be ignored in following updates until the user
        /// explicitily retype it or the system autofires it again.
        /// </summary>
        /// <param name="key">key to ignore</param>
        public void ReleaseKey(Keys key)
        {
            keyPressed.Remove(key);
        }


        /// <summary>
        /// Draw the whole game
        /// </summary>
        /// <param name="g">Graphics to draw in</param>
        public void Draw(Graphics g)
        {
            if (state == GameState.Play)
            {
                foreach (GameObject gameObject in gameObjects)
                    gameObject.Draw(this, g);
                g.DrawString("Niveau : " + this.Niveau.ToString(), new Font("Arial", 15), new SolidBrush(Color.Black), 10, 10);
            } else if (state == GameState.Pause)
            {
                g.DrawString("Pause", new Font("Arial", 40), new SolidBrush(Color.Black), gameSize.Width/2-80, gameSize.Height/2-50);
            } else if (state == GameState.Win)
            {
                g.DrawString("Gagné !", new Font("Arial", 40), new SolidBrush(Color.Black), gameSize.Width / 2 - 80, gameSize.Height / 2 - 50);
                g.DrawString("Appuyez sur la touche 'Espace' pour rejouer !", new Font("Arial", 15), new SolidBrush(Color.Black), gameSize.Width / 2 - 200, gameSize.Height / 2 + 10);
            }
            else if (state == GameState.Lost)
            {
                g.DrawString("Perdu !", new Font("Arial", 40), new SolidBrush(Color.Black), gameSize.Width / 2 - 80, gameSize.Height / 2 - 50);
                g.DrawString("Appuyez sur la touche 'Espace' pour rejouer !", new Font("Arial", 15), new SolidBrush(Color.Black), gameSize.Width / 2 - 200, gameSize.Height / 2 + 10);
            }
        }

        /// <summary>
        /// Update game
        /// </summary>
        public void Update(double deltaT)
        {
            if (state == GameState.Play)
            {
                if (keyPressed.Contains(Keys.P))
                {
                    state = GameState.Pause;
                    ReleaseKey(Keys.P);
                }
                
                // add new game objects
                gameObjects.UnionWith(pendingNewGameObjects);
                pendingNewGameObjects.Clear();

                // update each game object
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.Update(this, deltaT);
                }

                // remove dead objects
                gameObjects.RemoveWhere(gameObject => !gameObject.IsAlive());
                
                if (!this.enemies.IsAlive())
                {
                    state = GameState.Win;
                }
                if ((this.enemies.Position.y + this.enemies.Size.Height) >= this.playership.Position.y)
                {
                    this.playership.Lives = 0;
                }
                if (!this.playership.IsAlive())
                {
                    state = GameState.Lost;
                }

            }
            else if (state == GameState.Pause && keyPressed.Contains(Keys.P))
            {
                state = GameState.Play;
                ReleaseKey(Keys.P);
            }
            else if ((state == GameState.Win || state == GameState.Lost) )
            {
                if(keyPressed.Contains(Keys.Space))
                {
                    ReleaseKey(Keys.Space);
                    ReinitializeGame();
                }
            }
        }

        private void ReinitializeGame()
        {
            this.gameObjects = new HashSet<GameObject>();
            this.pendingNewGameObjects = new HashSet<GameObject>();
            this.bunkers = new List<Bunker>();
            this.keyPressed = new HashSet<Keys>();
            if (this.state == GameState.Win){
                this.Niveau += 1;
            }
            else
            {
                this.Niveau = 1;
            }
            this.state = GameState.Play;
            this.playership = new PlayerSpaceship(new Vecteur2D(gameSize.Width / 2 - 12, gameSize.Height - 35), 3, SpaceInvaders.Properties.Resources.ship3, GameObject.Side.Ally);
            AddNewGameObject(playership);
            for (int i = 0; i < nb_bunkers; i++)
            {
                int bunker_zone = gameSize.Width / nb_bunkers;
                this.bunkers.Add(new Bunker(new Vecteur2D(i * bunker_zone + ((bunker_zone - 87) / 2), gameSize.Height - 100), GameObject.Side.Neutral));
                AddNewGameObject(this.bunkers[i]);
            }
            this.enemies = new EnemyBlock(350, new Vecteur2D(0, 25), GameObject.Side.Enemy, this.Niveau);
            this.enemies.AddLine(8, 3, SpaceInvaders.Properties.Resources.ship1);
            this.enemies.AddLine(7, 2, SpaceInvaders.Properties.Resources.ship2);
            this.enemies.AddLine(6, 1, SpaceInvaders.Properties.Resources.ship9);
            AddNewGameObject(this.enemies);
        }

    #endregion
    }
}

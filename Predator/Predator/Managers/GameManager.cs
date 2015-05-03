using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VoidEngine.VGUI;
using VoidEngine.VGame;
using VoidEngine.Helpers;
using Predator.Characters;
using VoidEngine.Particles;
using Predator.Game;

namespace Predator.Managers
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class GameManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		Game1 myGame;
		SpriteBatch spriteBatch;

		/// <summary>
		/// Used to control the game's screen.
		/// </summary>
		Camera Camera;

		/// <summary>
		/// Used to get random real numbers.
		/// </summary>
		Random Random = new Random();

		#region Textures
		/// <summary>
		/// Loads the line texture.
		/// </summary>
		public Texture2D LineTexture;
		/// <summary>
		/// Loads the air tile texture.
		/// </summary>
		public Texture2D AirTileTexture;
		/// <summary>
		/// Loads the sewer tile texture.
		/// </summary>
		public Texture2D SewerTileTexture;
		/// <summary>
		/// Loads the temp player texture.
		/// </summary>
		public Texture2D TempPlayerTexture;
		/// <summary>
		/// Loads the projectile's texture.
		/// </summary>
		public Texture2D ProjectileTexture;
		/// <summary>
		/// Loads the temp enemy texture.
		/// </summary>
		public Texture2D TempEnemyTexture;
		/// <summary>
		/// Loads the temp tile texture.
		/// </summary>
		public Texture2D TempTileTexture;
		/// <summary>
		/// Loads the main healthbar's background texture.
		/// </summary>
		public Texture2D HealthBackgroundTexture;
		/// <summary>
		/// Loads the main healthbar's foreground texture.
		/// </summary>
		public Texture2D HealthForegroundTexture;
		/// <summary>
		/// Loads the overhead healthbar's background texture.
		/// </summary>
		public Texture2D HealthOverheadBackgroundTexture;
		/// <summary>
		/// Loads the overhead healthbar's foreground texture.
		/// </summary>
		public Texture2D HealthOverheadForegroundTexture;
		/// <summary>
		/// Loads the particle's texture.
		/// </summary>
		public Texture2D ParticleTexture;
		#endregion

		#region Enemy Stuff
		/// <summary>
		/// The Enemy list.
		/// Enemy: UNKNOWN
		/// </summary>
		public List<Enemy> EnemyList = new List<Enemy>();
		#endregion

		#region Tile Stuff
		/// <summary>
		/// The tile objects
		/// </summary>
		public List<Tile> TilesList = new List<Tile>();
		#endregion

		#region Player Stuff
		/// <summary>
		/// The default player.
		/// </summary>
		public Player Player;
		/// <summary>
		/// The player's movement keys.
		/// </summary>
		public Keys[,] MovementKeys = new Keys[4, 15];
		/// <summary>
		/// The main healthbar.
		/// </summary>
		public HealthBar HealthBar;
		/// <summary>
		/// The overhead healthbar.
		/// </summary>
		public HealthBar OverheadHealthBar;
		#endregion

		#region Level & Transition Stuff
		/// <summary>
		/// The level of the game.
		/// </summary>
		public int CurrentLevel
		{
			get;
			set;
		}
		/// <summary>
		/// The map boarders list.
		/// </summary>
		public List<Rectangle> MapBoundries = new List<Rectangle>();
		/// <summary>
		/// Gets or sets if the level is loaded.
		/// DO NOT SET TO TRUE!!!
		/// </summary>
		public bool LevelLoaded
		{
			get;
			set;
		}
		#endregion

		#region Partical System
		/// <summary>
		/// The list of the particles.
		/// </summary>
		List<Particle> ParticleList = new List<Particle>();
		/// <summary>
		/// Gets or sets the blood's minimum radius.
		/// </summary>
		public float BloodMinRadius
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the blood's maximum radius.
		/// </summary>
		public float BloodMaxRadius
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// Creates the game manager
		/// </summary>
		/// <param name="game">The game that the manager is running off of.</param>
		public GameManager(Game1 game)
			: base(game)
		{
			myGame = game;

			// TODO: Construct any child components here
			Initialize();
		}

		/// <summary>
		/// Allows the game component to perform any initialization it needs to before starting
		/// to run.  This is where it can query for any required services and load content.
		/// </summary>
		public override void Initialize()
		{
			// TODO: Add your initialization code here

			base.Initialize();
		}

		/// <summary>
		/// Loads the game component's content.
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(myGame.GraphicsDevice);

			LoadTextures();

			#region Player Stuff
			MovementKeys[0, 0] = Keys.A;
			MovementKeys[0, 1] = Keys.W;
			MovementKeys[0, 2] = Keys.D;
			MovementKeys[0, 3] = Keys.S;
			MovementKeys[0, 4] = Keys.Space;
			MovementKeys[0, 5] = Keys.E;
			MovementKeys[1, 0] = Keys.Left;
			MovementKeys[1, 1] = Keys.Up;
			MovementKeys[1, 2] = Keys.Right;
			MovementKeys[1, 3] = Keys.Down;

			Player = new Player(TempPlayerTexture, new Vector2(100, 100), MovementKeys, 100f, Color.White, myGame);
			Player.ProjectileTexture = ProjectileTexture;

			HealthBar = new HealthBar(HealthForegroundTexture, new Vector2(20, 25), Color.White, Player);

			OverheadHealthBar = new HealthBar(HealthOverheadForegroundTexture, new Vector2(Player.Position.X - ((48 - 35) / 2), Player.Position.Y - 10), Color.White, Player);
			#endregion

			#region Game Stuff
			Camera = new Camera(myGame.GraphicsDevice.Viewport, new Point(1024, 768), 1.0f);

			MapBoundries.Add(new Rectangle(-5, -5, 5, Camera.Size.Y + 10));
			MapBoundries.Add(new Rectangle(-5, -5, Camera.Size.X + 10, 5));
			MapBoundries.Add(new Rectangle(Camera.Size.X, -5, 5, Camera.Size.Y + 10));
			MapBoundries.Add(new Rectangle(-5, Camera.Size.Y, Camera.Size.X + 10, 5));
			#endregion

			base.LoadContent();
		}

		/// <summary>
		/// Loads all of the textures.
		/// </summary>
		protected void LoadTextures()
		{
			LineTexture = Game.Content.Load<Texture2D>(@"images\other\line");
			AirTileTexture = Game.Content.Load<Texture2D>(@"images\tiles\airTiles");
			TempTileTexture = Game.Content.Load<Texture2D>(@"images\tiles\tempTiles");
			SewerTileTexture = Game.Content.Load<Texture2D>(@"images\tiles\sewerTiles");
			TempPlayerTexture = Game.Content.Load<Texture2D>(@"images\player\temp");
			TempEnemyTexture = Game.Content.Load<Texture2D>(@"images\enemy\tempEnemy");
			HealthForegroundTexture = Game.Content.Load<Texture2D>(@"images\gui\game\healthFG");
			HealthBackgroundTexture = Game.Content.Load<Texture2D>(@"images\gui\game\healthBG");
			HealthOverheadForegroundTexture = Game.Content.Load<Texture2D>(@"images\gui\game\healthOHFG");
			HealthOverheadBackgroundTexture = Game.Content.Load<Texture2D>(@"images\gui\game\healthOHBG");
			ProjectileTexture = Game.Content.Load<Texture2D>(@"images\player\attackTemp");
			ParticleTexture = Game.Content.Load<Texture2D>(@"images\other\testParticle");
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			#region Camera Controls
			#region Debug Camera Controls
			if (myGame.IsGameDebug)
			{
				// Rotate the camera to the left.
				if (myGame.KeyboardState.IsKeyDown(Keys.NumPad4))
				{
					Camera.RotationZ += 1 * (float)Math.PI / 180;
				}
				// Rotate the camera to the right.
				if (myGame.KeyboardState.IsKeyDown(Keys.NumPad6))
				{
					Camera.RotationZ -= 1 * (float)Math.PI / 180;
				}
				/// Zoom the camera in.
				if (myGame.KeyboardState.IsKeyDown(Keys.NumPad8))
				{
					Camera.Zoom += 0.01f;
				}
				/// Zome the camera out.
				if (myGame.KeyboardState.IsKeyDown(Keys.NumPad2))
				{
					Camera.Zoom -= 0.01f;
				}
				/// Reset the camera.
				if (myGame.KeyboardState.IsKeyDown(Keys.NumPad5))
				{
					Camera.Zoom = 1f;
					Camera.RotationZ = 0;
					Camera.Position = Player.Position;
				}
			}
			#endregion

			// Update the camera's position.
			if (Camera.IsInView(Player.Position, new Vector2(Player.CurrentAnimation.frameSize.X, Player.CurrentAnimation.frameSize.Y)))
			{
				Camera.Position = Player.Position;
			}
			#endregion

			#region Menu Controls
			// Open the map.
			if (myGame.CheckKey(Keys.M) && !myGame.mapScreenManager.isTransitioningIn)
			{
				myGame.mapScreenManager.isTransitioningIn = true;
				myGame.SetCurrentLevel(Game1.GameLevels.MAP);
			}
			// Open the stats screen.
			if (myGame.CheckKey(Keys.G))
			{
				myGame.SetCurrentLevel(Game1.GameLevels.STATS);
			}
			// Open the main menu
			if (myGame.CheckKey(Keys.Q))
			{
				myGame.SetCurrentLevel(Game1.GameLevels.MENU);
			}
			#endregion

			// Reset The level
			if (!LevelLoaded)
			{
				CurrentLevel = 0;
				RegenerateMap();
			}

			#region Update when level is loaded.
			if (LevelLoaded)
			{
				#region Update Player Stuff
				Player.UpdateKeyboardState(gameTime, myGame.KeyboardState);
				Player.Update(gameTime);
				//HealthBar.SetPlayer = Player;
				HealthBar.Update(gameTime);
				OverheadHealthBar.Update(gameTime);
				OverheadHealthBar.Position = new Vector2(Player.Position.X - ((44 - 35) / 2), Player.Position.Y - 7);
				#endregion

				#region Update Enemy Stuff
				for (int i = 0; i < EnemyList.Count; i++)
				{
					if (EnemyList[i].isDead)
					{
						EnemyList.RemoveAt(i);
						i--;
					}
					else
					{
						EnemyList[i].Update(gameTime);

						if (EnemyList[i].isHit)
						{
							ParticleSystem.CreateParticles(EnemyList[i].PositionCenter - new Vector2(0, 15), ParticleTexture, Random, ParticleList, 230, 255, 0, 0, 0, 0, 5, 10, (int)BloodMinRadius, (int)BloodMaxRadius, 100, 250, 3, 5, 200, 255);
							EnemyList[i].isHit = false;
						}
					}
				}
				#endregion

				#region Update Particle stuff
				for (int i = 0; i < ParticleList.Count; i++)
				{
					if (ParticleList[i].DeleteMe)
					{
						ParticleList.RemoveAt(i);
						i--;
					}
					else
					{
						ParticleList[i].Update(gameTime);
					}
				}
				#endregion

				#region Update Tile stuff
				for (int i = 0; i < TilesList.Count; i++)
				{
					TilesList[i].Update(gameTime);
				}
				#endregion
			}
			#endregion

			#region Debug Stuff
			if (myGame.IsGameDebug)
			{
				myGame.debugStrings[0] = "Player || JumpTime=" + Player.JumpTime + " isGrounded=" + Player.IsGrounded;
				myGame.debugStrings[1] = "       || Position=(" + Player.Position.X + "," + Player.Position.Y + ")";
			}
			#endregion

			base.Update(gameTime);
		}

		/// <summary>
		/// Draws the game component's content.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, Camera.GetTransformation());
			{
				foreach (Enemy e in EnemyList)
				{
					e.Draw(gameTime, spriteBatch);
				}

				foreach (Tile t in TilesList)
				{
					t.Draw(gameTime, spriteBatch);
				}

				spriteBatch.Draw(HealthOverheadBackgroundTexture, new Vector2(Player.Position.X - ((50 - 35) / 2), Player.Position.Y - 12), Color.White);

				OverheadHealthBar.Draw(gameTime, spriteBatch);

				Player.Draw(gameTime, spriteBatch);

				foreach (Particle p in ParticleList)
				{
					p.Draw(gameTime, spriteBatch);
				}
			}
			spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);
			{
				spriteBatch.Draw(HealthBackgroundTexture, new Vector2(15, 15), Color.White);

				HealthBar.Draw(gameTime, spriteBatch);

				myGame.debugLabel.Draw(gameTime, spriteBatch);

				//spriteBatch.DrawString(myGame.grimGhostRegular, "Hello World!", new Vector2((myGame.WindowSize.X - myGame.grimGhostRegular.MeasureString("Hello World!").X) / 2, (myGame.WindowSize.Y - myGame.grimGhostRegular.MeasureString("Hellow World!").Y) / 2), Color.Black);
			}
			spriteBatch.End();

			#region Debug Stuff
			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, Camera.GetTransformation());
			{
				if (myGame.IsGameDebug && LevelLoaded)
				{
					foreach (Tile t in TilesList)
					{
						if (t.TileType == Tile.TileCollisions.Impassable)
						{
							//t.DrawBoundingCollisions(LineTexture, Color.White, spriteBatch);
						}

						if (t.TileType == Tile.TileCollisions.Platform)
						{
							//t.DrawBoundingCollisions(LineTexture, Color.Blue, spriteBatch);
						}
					}
					foreach (Rectangle r in MapBoundries)
					{
						spriteBatch.Draw(LineTexture, new Rectangle(r.X, r.Y, r.Width, 1), Color.White);
						spriteBatch.Draw(LineTexture, new Rectangle(r.Right - 1, r.Y, 1, r.Height), Color.White);
						spriteBatch.Draw(LineTexture, new Rectangle(r.X, r.Bottom - 1, r.Width, 1), Color.White);
						spriteBatch.Draw(LineTexture, new Rectangle(r.X, r.Y, 1, r.Height), Color.White);
					}

					foreach (Enemy e in EnemyList)
					{
						e.DrawBoundingCollisions(LineTexture, Color.Magenta, spriteBatch);
					}

					Player.DrawBoundingCollisions(LineTexture, Color.Blue, spriteBatch);
				}
			}
			spriteBatch.End();
			#endregion

			base.Draw(gameTime);
		}

		/// <summary>
		/// Regenerates the map
		/// * Be sure to set level before this and set "LevelLoaded" to false *
		/// </summary>
		public void RegenerateMap()
		{
			TilesList.RemoveRange(0, TilesList.Count);
			EnemyList.RemoveRange(0, EnemyList.Count);
			Player.Position = Vector2.Zero;

			SpawnTiles(CurrentLevel);

			MapBoundries[0] = new Rectangle(-35, -35, 35, Camera.Size.Y + 70);
			MapBoundries[1] = new Rectangle(Camera.Size.X, -35, 35, Camera.Size.Y + 70);
			MapBoundries[2] = new Rectangle(-35, -35, Camera.Size.X + 70, 35);
			MapBoundries[3] = new Rectangle(-35, Camera.Size.Y, Camera.Size.X + 70, 35);

			LevelLoaded = true;
		}

		/// <summary>
		/// Spawns the tiles in the world
		/// </summary>
		/// <param name="level">The level to spawn.</param>
		public void SpawnTiles(int level)
		{
			Point Size = new Point(0, 0);
			int[,] tiles = new int[0, 0];

			switch (level)
			{
				case 0:
					tiles = Maps.TestLevel();
					Size = new Point(Maps.TestLevel().GetLength(1), Maps.TestLevel().GetLength(0));
					break;
			}

			Camera.Size = new Point(Size.X * 35, Size.Y * 35);

			for (int x = 0; x < Size.X; x++)
			{
				for (int y = 0; y < Size.Y; y++)
				{
					if (tiles[y, x] == 77)
					{
						Player.Position = new Vector2(x * 35, y * 35);
					}
					else if (tiles[y, x] == 78)
					{
						Enemy tempEnemy1 = new Enemy(TempEnemyTexture, new Vector2(x * 35, y * 35), Enemy.EnemyType.RAT, Color.White, myGame);
						EnemyList.Add(tempEnemy1);
					}

					if (tiles[y, x] > 69)
					{
						tiles[y, x] = 0;
					}

					if (tiles[y, x] == 0)
					{
						TilesList.Add(new Tile(AirTileTexture, new Vector2(x * 35, y * 35), Tile.TileCollisions.Passable, TilesList, 0, Color.White));
					}

					if (tiles[y, x] > 0)
					{
						TilesList.Add(new Tile(SewerTileTexture, new Vector2(x * 35, y * 35), Tile.TileCollisions.Impassable, TilesList, 1, Color.White));
					}
				}
			}

			foreach (Tile t in TilesList)
			{
				t.UpdateConnections();
			}

			Camera.Position = Player.Position;
		}
	}
}

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
using VoidEngine.Helpers;
using VoidEngine.Particles;
using VoidEngine.VGame;
using VoidEngine.VGUI;
using Predator.Characters;
using Predator.Game;
using Predator.Other;

namespace Predator.Managers
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class GameManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		/// <summary>
		/// The game that the game manager runs off of.
		/// </summary>
		private Game1 myGame;
		/// <summary>
		/// The sprite batch that the game manager uses.
		/// </summary>
		private SpriteBatch spriteBatch;

		/// <summary>
		/// Used to control the game's screen.
		/// </summary>
		protected Camera Camera;

		/// <summary>
		/// Used to get random real numbers.
		/// </summary>
		protected Random Random = new Random();

		#region Textures
		/// <summary>
		/// Loads the line texture.
		/// </summary>
		public Texture2D LineTexture;
		/// <summary>
		/// Loads the shadow tile texture.
		/// </summary>
		public Texture2D ShadowTileTexture;
		/// <summary>
		/// Loads the sewer tile texture.
		/// </summary>
		public Texture2D SewerTileTexture;
		/// <summary>
		/// Loads the temp player texture.
		/// </summary>
		public Texture2D TempPlayerTexture;
		/// <summary>
		/// Loads the projectile texture.
		/// </summary>
		public Texture2D ProjectileTexture;
		/// <summary>
		/// Loads the crawler enemy texture.
		/// </summary>
		public Texture2D CrawlerTexture;
		/// <summary>
		/// Loads the temp enemy texture.
		/// </summary>
		public Texture2D TempEnemyTexture;
		/// <summary>
		/// Loads the particle texture.
		/// </summary>
		public Texture2D ParticleTexture;
		/// <summary>
		/// Loads the health drop texture.
		/// </summary>
		public Texture2D HealthDropTexture;
		/// <summary>
		/// Loads the temp tile texture.
		/// </summary>
		public Texture2D TempTileTexture;
		/// <summary>
		/// Loads the main healthbar background texture.
		/// </summary>
		public Texture2D HealthBackgroundTexture;
		/// <summary>
		/// Loads the main healthbar foreground texture.
		/// </summary>
		public Texture2D HealthForegroundTexture;
		/// <summary>
		/// Loads the overhead healthbar background texture.
		/// </summary>
		public Texture2D HealthOverheadBackgroundTexture;
		/// <summary>
		/// Loads the overhead healthbar foreground texture.
		/// </summary>
		public Texture2D HealthOverheadForegroundTexture;
		/// <summary>
		/// Loads the sewer background spray texture.
		/// </summary>
		public Texture2D SewerBackgroundSprayTexture;
		/// <summary>
		/// Loads the sewer background texture.
		/// </summary>
		public Texture2D SewerBackgroundTexture;
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

		#region Background Stuff
		/// <summary>
		/// The sewer background parallax.
		/// </summary>
		public Parallax SewerBackgroundParallax;
		/// <summary>
		/// The sewer background spray parallax.
		/// </summary>
		public Parallax SewerBackgroundSprayParallax;
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
		/// <summary>
		/// The drop list for the pickups.
		/// </summary>
		public List<HealthPickUp> DropList = new List<HealthPickUp>();
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
		public List<Particle> ParticleList = new List<Particle>();
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
		/// Creates the game manager.
		/// </summary>
		/// <param name="game">The game that the game manager is running off of.</param>
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
		/// Loads the content for the game manager.
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

			Player = new Player(TempPlayerTexture, new Vector2(100, 100), MovementKeys, Color.White, myGame);
			Player.ProjectileTexture = ProjectileTexture;

			HealthBar = new HealthBar(HealthForegroundTexture, new Vector2(20, 25), Color.White);

			OverheadHealthBar = new HealthBar(HealthOverheadForegroundTexture, new Vector2(Player.Position.X - ((48 - 35) / 2), Player.Position.Y - 10), Color.White);
			#endregion

			#region Game Stuff
			Camera = new Camera(myGame.GraphicsDevice.Viewport, new Point(1024, 768), 1.0f);

			MapBoundries.Add(new Rectangle(-5, -105, 5, Camera.Size.Y + 10));
			MapBoundries.Add(new Rectangle(-5, -105, Camera.Size.X + 10, 5));
			MapBoundries.Add(new Rectangle(Camera.Size.X, -105, 5, Camera.Size.Y + 10));
			MapBoundries.Add(new Rectangle(-5, Camera.Size.Y, Camera.Size.X + 10, 5));

			SewerBackgroundParallax = new Parallax(SewerBackgroundTexture, new Vector2(Camera.Position.X, Camera.Position.Y), Color.White, new Vector2(0.50f, 0.00f), Camera);
			SewerBackgroundSprayParallax = new Parallax(SewerBackgroundSprayTexture, new Vector2(Camera.Position.X, Camera.Position.Y), Color.White, new Vector2(0.50f, 0.00f), Camera);
			#endregion

			base.LoadContent();
		}

		/// <summary>
		/// Loads the textures for the game manager.
		/// </summary>
		protected void LoadTextures()
		{
			// Debug
			LineTexture = Game.Content.Load<Texture2D>(@"images\other\line");

			// Tiles
			TempTileTexture = Game.Content.Load<Texture2D>(@"images\tiles\tempTiles");
			SewerTileTexture = Game.Content.Load<Texture2D>(@"images\tiles\sewerTiles");
			ShadowTileTexture = Game.Content.Load<Texture2D>(@"images\tiles\fadeTiles");

			// Player and entities
			TempPlayerTexture = Game.Content.Load<Texture2D>(@"images\player\temp");
			TempEnemyTexture = Game.Content.Load<Texture2D>(@"images\enemy\tempEnemy");
			CrawlerTexture = Game.Content.Load<Texture2D>(@"images\enemy\crawler1");

			// UI
			HealthForegroundTexture = Game.Content.Load<Texture2D>(@"images\gui\game\healthBarFore");
			HealthBackgroundTexture = Game.Content.Load<Texture2D>(@"images\gui\game\healthBarBack");
			HealthOverheadForegroundTexture = Game.Content.Load<Texture2D>(@"images\gui\game\healthBarOverFore");
			HealthOverheadBackgroundTexture = Game.Content.Load<Texture2D>(@"images\gui\game\healthBarOverback");

			// Effects
			ProjectileTexture = Game.Content.Load<Texture2D>(@"images\player\attackTemp");
			ParticleTexture = Game.Content.Load<Texture2D>(@"images\game\particles\tempParticle");

			// Drops
			HealthDropTexture = Game.Content.Load<Texture2D>(@"images\game\drops\healthDrop");

			// Backgrounds
			SewerBackgroundSprayTexture = Game.Content.Load<Texture2D>(@"images\game\backgrounds\funBackground");
			SewerBackgroundTexture = Game.Content.Load<Texture2D>(@"images\game\backgrounds\sewerBackground");
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
			if (myGame.CheckKey(Keys.M) && !myGame.mapScreenManager.IsTransitioningIn)
			{
				myGame.mapScreenManager.IsTransitioningIn = true;
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

			if (myGame.CheckKey(Keys.F6))
			{
				SaveFile tempSaveFile = new SaveFile();
				FileManagerTemplate.Game gameData = new FileManagerTemplate.Game();
				gameData.CurrentLevel = CurrentLevel;
				FileManagerTemplate.Options optionsData = new FileManagerTemplate.Options();
				optionsData.VSync = myGame.VSync;
				optionsData.WindowSize = myGame.WindowSize;
				FileManagerTemplate.PlayerData playerData = new FileManagerTemplate.PlayerData();
				playerData.Damage = Player.Damage;
				playerData.Defense = Player.Defense;
				playerData.Level = Player.Level;
				playerData.Lvl = Player.Lvl;
				playerData.MainHP = Player.MainHP;
				playerData.MaxHP = Player.MaxHP;
				playerData.MovementKeys = Player.MovementKeys;
				playerData.PAgility = Player.PAgility;
				playerData.PDefense = Player.PDefense;
				playerData.PExp = Player.PExp;
				playerData.Position = Player.Position;
				playerData.PStrength = Player.PStrength;
				playerData.StatPoints = Player.StatPoints;
				tempSaveFile.FileSaveVersion = "0.0.0.1";
				tempSaveFile.GameData = gameData;
				tempSaveFile.OptionsData = optionsData;
				tempSaveFile.PlayerData = playerData;
				SaveFileManager.SaveFile(tempSaveFile, "", "Save1.sav");
			}

			if (myGame.CheckKey(Keys.F5))
			{
				SaveFile tempSaveFile = SaveFileManager.LoadFile("", "Save1.sav");
				if (tempSaveFile.FileSaveVersion == "0.0.0.1")
				{
					CurrentLevel = tempSaveFile.GameData.CurrentLevel;
					myGame.VSync = tempSaveFile.OptionsData.VSync;
					myGame.WindowSize = tempSaveFile.OptionsData.WindowSize;
					myGame.ApplySettings = true;
					Player.Damage = tempSaveFile.PlayerData.Damage;
					Player.Defense = tempSaveFile.PlayerData.Defense;
					Player.Level = tempSaveFile.PlayerData.Level;
					Player.Lvl = tempSaveFile.PlayerData.Lvl;
					Player.MainHP = tempSaveFile.PlayerData.MainHP;
					Player.MaxHP = tempSaveFile.PlayerData.MaxHP;
					Player.MovementKeys = tempSaveFile.PlayerData.MovementKeys;
					Player.PAgility = tempSaveFile.PlayerData.PAgility;
					Player.PDefense = tempSaveFile.PlayerData.PDefense;
					Player.PExp = tempSaveFile.PlayerData.PExp;
					Player.Position = tempSaveFile.PlayerData.Position;
					Player.PStrength = tempSaveFile.PlayerData.PStrength;
					Player.StatPoints = tempSaveFile.PlayerData.StatPoints;
					Camera.Position = Player.Position;
				}
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

				if (myGame.CheckKey(Keys.Z))
				{
					Player.StatPoints += 500;
				}
				if (myGame.CheckKey(Keys.X))
				{
					Player.PExp += 500;
				}

				#region Update Enemy Stuff
				for (int i = 0; i < EnemyList.Count; i++)
				{
					if (EnemyList[i].isDead)
					{
						DropList.Add(new HealthPickUp(HealthDropTexture, EnemyList[i].Position, myGame));
						EnemyList.RemoveAt(i);
						i--;
					}
					else
					{
						EnemyList[i].Update(gameTime);

						if (EnemyList[i].IsHit)
						{
							ParticleSystem.CreateParticles(EnemyList[i].PositionCenter - new Vector2(0, 15), ParticleTexture, Random, ParticleList, 230, 255, 0, 0, 0, 0, 5, 10, (int)BloodMinRadius, (int)BloodMaxRadius, 100, 250, 3, 5, 200, 255);
							EnemyList[i].IsHit = false;
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

				#region Update Pickups
				for (int i = 0; i < DropList.Count; i++)
				{
					if (DropList[i].DeleteMe)
					{
						DropList.RemoveAt(i);
						Player.MainHP += 5;
						i--;
					}
					else
					{
						DropList[i].Update(gameTime);
					}
				}
				#endregion
			}
			#endregion

			#region Debug Stuff
			if (myGame.IsGameDebug)
			{
				DebugTable debugTable = new DebugTable();
				string[,] rawTable = {
										 { "Player", "Tings", "Stats" }
									 };
				debugTable.initalizeTable(rawTable);

				myGame.debugStrings[0] = debugTable.ReturnStringSegment(0, 0);
				myGame.debugStrings[1] = debugTable.ReturnStringSegment(0, 1) + "Postition=(" + Player.Position.X + "," + Player.Position.Y + ") Velocity=(" + Player.Velocity.X + "," + Player.Velocity.Y + ")";
				myGame.debugStrings[2] = debugTable.ReturnStringSegment(0, 2) + "Agility=" + Player.PAgility + " Strength=" + Player.PStrength + " Defense=" + Player.PDefense + " StatPoints=" + Player.StatPoints + "Stat Level=" + Player.Lvl;
			}
			#endregion

			base.Update(gameTime);
		}

		/// <summary>
		/// Draws the content of the game manager.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);
			{
				SewerBackgroundParallax.Draw(gameTime, spriteBatch);
				SewerBackgroundSprayParallax.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, Camera.GetTransformation());
			{
				foreach (Enemy e in EnemyList)
				{
					e.Draw(gameTime, spriteBatch);
				}

				foreach (Tile t in TilesList)
				{
					if (Camera.IsInView(t.Position, new Vector2(35, 35)))
					{
						t.Draw(gameTime, spriteBatch);
					}
				}

				spriteBatch.Draw(HealthOverheadBackgroundTexture, new Vector2(Player.Position.X - ((50 - 35) / 2), Player.Position.Y - 12), Color.White);

				OverheadHealthBar.Draw(gameTime, spriteBatch);

				Player.Draw(gameTime, spriteBatch);

				foreach (Particle p in ParticleList)
				{
					p.Draw(gameTime, spriteBatch);

				}
				foreach (HealthPickUp h in DropList)
				{
					h.Draw(gameTime, spriteBatch);
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

					spriteBatch.Draw(LineTexture, new Rectangle(Player.DebugBlock.X, Player.DebugBlock.Y, Player.DebugBlock.Width, 1), Color.White);
					spriteBatch.Draw(LineTexture, new Rectangle(Player.DebugBlock.Right - 1, Player.DebugBlock.Y, 1, Player.DebugBlock.Height), Color.White);
					spriteBatch.Draw(LineTexture, new Rectangle(Player.DebugBlock.X, Player.DebugBlock.Bottom - 1, Player.DebugBlock.Width, 1), Color.White);
					spriteBatch.Draw(LineTexture, new Rectangle(Player.DebugBlock.X, Player.DebugBlock.Y, 1, Player.DebugBlock.Height), Color.White);

					Player.DrawBoundingCollisions(LineTexture, Color.Blue, spriteBatch);
				}
			}
			spriteBatch.End();
			#endregion

			if (myGame.OptionsChanged > myGame.OldOptionsChanged)
			{
				Camera.viewportSize = new Vector2(myGame.WindowSize.X, myGame.WindowSize.Y);
			}

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

			MapBoundries[0] = new Rectangle(-35, -105, 35, Camera.Size.Y + 70);
			MapBoundries[1] = new Rectangle(Camera.Size.X, -105, 35, Camera.Size.Y + 70);
			MapBoundries[2] = new Rectangle(-35, -105, Camera.Size.X + 70, 35);
			MapBoundries[3] = new Rectangle(-35, Camera.Size.Y, Camera.Size.X + 70, 35);

			LevelLoaded = true;
		}

		/// <summary>
		/// Spawns the tiles in the world
		/// </summary>
		/// <param name="level">The level to spawn.</param>
		public void SpawnTiles(int level)
		{
			Point Size = new Point();
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
						Enemy tempCrawler = new Enemy(CrawlerTexture, new Vector2(x * 35, y * 35), Enemy.EnemyTypes.RAT, Color.DarkSeaGreen, myGame);
						tempCrawler.Scale = 0.70f;
						int width = (int)(tempCrawler.CurrentAnimation.frameSize.X * tempCrawler.Scale);
						int left = (int)(tempCrawler.CurrentAnimation.frameSize.X * tempCrawler.Scale - width);
						int height = (int)(tempCrawler.CurrentAnimation.frameSize.Y * tempCrawler.Scale);
						int top = (int)(tempCrawler.CurrentAnimation.frameSize.Y * tempCrawler.Scale - height);
						tempCrawler.Inbounds = new Rectangle(left, top, width, height);
						EnemyList.Add(tempCrawler);
					}

					if (tiles[y, x] > 69)
					{
						tiles[y, x] = 0;
					}

					if (tiles[y, x] > 0)
					{
						TilesList.Add(new Tile(SewerTileTexture, new Vector2(x * 35, y * 35), Tile.TileCollisions.Impassable, TilesList, 1, Color.White));
						TilesList.Add(new Tile(ShadowTileTexture, new Vector2(x * 35, y * 35), Tile.TileCollisions.Impassable, TilesList, 1, Color.White));
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
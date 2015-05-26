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
		public Texture2D SlimeTexture;
		public Texture2D SpitterTexture;
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
		/// Loads the exp drop texture.
		/// </summary>
		public Texture2D ExpDropTexture;
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
		/// <summary>
		/// Loads the checkpoint texture.
		/// </summary>
		public Texture2D CheckpointTexture;
		#endregion

		#region Enemy Stuff
		/// <summary>
		/// The Enemy list.
		/// Enemy: UNKNOWN
		/// </summary>
		public List<Enemy> EnemyList = new List<Enemy>();
        /// <summary>
        /// 
        /// </summary>
		public List<Sprite.AnimationSet> SpitterAnimationSet = new List<Sprite.AnimationSet>();
        /// <summary>
        /// 
        /// </summary>
		public List<Sprite.AnimationSet> JumperAnimationSet = new List<Sprite.AnimationSet>();
        /// <summary>
        /// 
        /// </summary>
		public List<Sprite.AnimationSet> RollerAnimationSet = new List<Sprite.AnimationSet>();
        /// <summary>
        /// 
        /// </summary>
        public List<Sprite.AnimationSet> CrawlerAnimationSet = new List<Sprite.AnimationSet>();
        /// <summary>
        /// 
        /// </summary>
        public int EnemyLevel;
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
		/// The drop list for the health pickups.
		/// </summary>
		public List<HealthPickUp> HpDropList = new List<HealthPickUp>();
		/// <summary>
		/// The drop list for the exp pickups.
		/// </summary>
        public List<ExpPickUp> ExpDropList = new List<ExpPickUp>();
		/// <summary>
		/// Gets or sets the amount of time to take the player to respawn.
		/// </summary>
		public float playerDeathTimer
		{
			get;
			set;
		}
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
		/// <summary>
		/// Sets the camera to the player if true.
		/// </summary>
		public bool CameraToPlayer
		{
			get;
			set;
		}
		protected Texture2D TestLevelTextureMap;
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

		#region Checkpoints
		/// <summary>
		/// 
		/// </summary>
		public int LastCheckpoint
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		public Vector2[] ListOfCheckpoints = new Vector2[5];
		/// <summary>
		/// 
		/// </summary>
		int checkpointNum = 1;
		#endregion

		#region Objects
		public List<PlaceableObject> PlaceableObjectsList = new List<PlaceableObject>();
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
			CameraToPlayer = true;

			HealthBar = new HealthBar(HealthForegroundTexture, new Vector2(20, 25), Color.White);

			OverheadHealthBar = new HealthBar(HealthOverheadForegroundTexture, new Vector2(Player.Position.X - ((48 - 35) / 2), Player.Position.Y - 10), Color.White);
			#endregion

			#region Game Stuff
			Camera = new Camera(myGame.GraphicsDevice.Viewport, new Point(1024, 768), 1.0f);

			MapBoundries.Add(new Rectangle(-5, -105, 5, Camera.Size.Y + 10));
			MapBoundries.Add(new Rectangle(-5, -105, Camera.Size.X + 10, 5));
			MapBoundries.Add(new Rectangle(Camera.Size.X, -105, 5, Camera.Size.Y + 10));
			MapBoundries.Add(new Rectangle(-5, Camera.Size.Y, Camera.Size.X + 10, 5));

			SewerBackgroundParallax = new Parallax(SewerBackgroundTexture, new Vector2(Camera.Position.X, Camera.Position.Y), Color.White, new Vector2(2.50f, 2.50f), Camera);
			SewerBackgroundSprayParallax = new Parallax(SewerBackgroundSprayTexture, new Vector2(Camera.Position.X, Camera.Position.Y), Color.White, new Vector2(2.50f, 2.50f), Camera);
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
			SlimeTexture = Game.Content.Load<Texture2D>(@"images\enemy\Spit_Glob");
			SpitterTexture = Game.Content.Load<Texture2D>(@"images\enemy\spitter1");

			// Effects
			ProjectileTexture = Game.Content.Load<Texture2D>(@"images\player\attackTemp");
			ParticleTexture = Game.Content.Load<Texture2D>(@"images\game\particles\tempParticle");

			// Drops
			HealthDropTexture = Game.Content.Load<Texture2D>(@"images\game\drops\healthDrop");
            ExpDropTexture = Game.Content.Load<Texture2D>(@"images\game\drops\XPSprite");

			// Backgrounds
			SewerBackgroundSprayTexture = Game.Content.Load<Texture2D>(@"images\game\backgrounds\funBackground");
			SewerBackgroundTexture = Game.Content.Load<Texture2D>(@"images\game\backgrounds\sewerBackground");

			// Objects
			CheckpointTexture = Game.Content.Load<Texture2D>(@"images\objects\checkpoint");

			// Levels
			TestLevelTextureMap = Game.Content.Load<Texture2D>(@"levels\City_1");

			// Sprite sheets
			CrawlerAnimationSet.Add(new Sprite.AnimationSet("IDLE", CrawlerTexture, new Point(122, 65), new Point(1, 1), new Point(0, 0), 16000, false));
			CrawlerAnimationSet.Add(new Sprite.AnimationSet("WALK", CrawlerTexture, new Point(120, 61), new Point(2, 2), new Point(0, 0), 80, true));
			CrawlerAnimationSet.Add(new Sprite.AnimationSet("DEATH", CrawlerTexture, new Point(122, 65), new Point(1, 1), new Point(0, 0), 16000, false));

			SpitterAnimationSet.Add(new Sprite.AnimationSet("IDLE", SpitterTexture, new Point(120, 240), new Point(1, 1), new Point(0, 0), 16000, false));
			SpitterAnimationSet.Add(new Sprite.AnimationSet("WALK", SpitterTexture, new Point(120, 240), new Point(6, 1), new Point(120, 0), 100, true));
			SpitterAnimationSet.Add(new Sprite.AnimationSet("SHOOT", SpitterTexture, new Point(120, 240), new Point(5, 1), new Point(120, 240), 130, true));

			RollerAnimationSet.Add(new Sprite.AnimationSet("ROLL", CrawlerTexture, new Point(122, 65), new Point(1, 1), new Point(0, 0), 16000, false));

			JumperAnimationSet.Add(new Sprite.AnimationSet("IDLE", CrawlerTexture, new Point(122, 65), new Point(1, 1), new Point(0, 0), 16000, false));
			JumperAnimationSet.Add(new Sprite.AnimationSet("WALK", CrawlerTexture, new Point(122, 65), new Point(1, 1), new Point(0, 0), 16000, false));
			JumperAnimationSet.Add(new Sprite.AnimationSet("DIVE", CrawlerTexture, new Point(122, 65), new Point(1, 1), new Point(0, 0), 16000, false));
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
			if (CameraToPlayer)
			{
				Camera.Position = Player.PositionCenter;
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

            if (myGame.CheckKey(Keys.X))
            {
                myGame.gameManager.Player.PExp += 500;
            }

			// Reset The level
			if (!LevelLoaded)
			{
				CurrentLevel = 0;
				RegenerateMap();
			}

			#region Saving & Loading
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
				playerData.LastCheckpoint = LastCheckpoint;
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
					LastCheckpoint = tempSaveFile.PlayerData.LastCheckpoint;
					Camera.Position = Player.Position;
				}
			}
			#endregion

			#region Update when level is loaded.
			if (LevelLoaded)
			{
				#region Update Player Stuff
				Player.UpdateKeyboardState(gameTime, myGame.KeyboardState);
				Player.Update(gameTime);
				//HealthBar.SetPlayer = Player;
				HealthBar.Update(gameTime, Player.HP, Player.MaxHP);
				OverheadHealthBar.Update(gameTime, Player.HP, Player.MaxHP);
				OverheadHealthBar.Position = new Vector2(Player.Position.X - ((44 - 35) / 2), Player.Position.Y - 7);
				if (Player.isDead)
				{
					playerDeathTimer -= gameTime.ElapsedGameTime.Milliseconds;

					if (playerDeathTimer <= 0)
					{
						if (LastCheckpoint > 0)
						{
							Player.Position = ListOfCheckpoints[LastCheckpoint - 1];
							Player.MainHP = Player.MaxHP * 0.75f;
							Player.isDead = false;
						}
						else
						{
							RegenerateMap();
							Player.MainHP += gameTime.ElapsedGameTime.Seconds;
							Player.MainHP = Player.MaxHP * 0.75f;
							Player.isDead = false;
						}
					}
				}
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
						HpDropList.Add(new HealthPickUp(HealthDropTexture, EnemyList[i].Position, myGame));
                        ExpDropList.Add(new ExpPickUp(ExpDropTexture,EnemyList[i].Position, myGame));
						EnemyList.RemoveAt(i);
						i--;
					}
					else
					{
						if (Camera.IsInView(EnemyList[i].Position, new Vector2(EnemyList[i].BoundingCollisions.Width, EnemyList[i].BoundingCollisions.Height)))
						{
							EnemyList[i].Update(gameTime);
						}

						if (EnemyList[i].IsHit)
						{
							ParticleSystem.CreateParticles(EnemyList[i].Position, ParticleTexture, Random, ParticleList, 230, 255, 0, 0, 0, 0, 5, 10, (int)BloodMinRadius, (int)BloodMaxRadius, 100, 250, 3, 5, 200, 255);
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
				for (int i = 0; i < HpDropList.Count; i++)
				{
					if (HpDropList[i].DeleteMe)
					{
						HpDropList.RemoveAt(i);
						Player.MainHP += 5;
						i--;
					}
					else
					{
						HpDropList[i].Update(gameTime);
					}
				}
                for (int i = 0; i < ExpDropList.Count; i++)
                {
                    if (ExpDropList[i].DeleteMe)
                    {
                        ExpDropList.RemoveAt(i);
                        Player.PExp += myGame.rng.Next(500, 1000);
                        i--;
                    }
                    else
                    {
                        ExpDropList[i].Update(gameTime);
                    }
                }
				#endregion

				#region Update Checkpoints
				foreach (PlaceableObject po in PlaceableObjectsList)
				{
					if (po is CheckPoint)
					{
						po.Update(gameTime);
					}
				}

				if (Player.isDead)
				{
					playerDeathTimer -= gameTime.ElapsedGameTime.Milliseconds;

					if (LastCheckpoint > 0)
					{
						if (playerDeathTimer < 0)
						{
							Player.Position = ListOfCheckpoints[LastCheckpoint];
						}
					}
					else
					{
						if (playerDeathTimer < 0)
						{
							RegenerateMap();
						}
					}
				}
				#endregion

			}
			#endregion

			#region Debug Stuff
			if (myGame.IsGameDebug && LevelLoaded)
			{
				DebugTable debugTable = new DebugTable();
				string[,] rawTable = {
										 { "Player", "Tings", "Stats" },
										 { "Enemy[0]", "Tings", "" },
										 { "CheckPoint", "Tings", "Stuff" }
									 };
				debugTable.initalizeTable(rawTable);

				myGame.debugStrings[0] = debugTable.ReturnStringSegment(0, 0);
				myGame.debugStrings[1] = debugTable.ReturnStringSegment(0, 1) + "Postition=(" + Player.Position.X + "," + Player.Position.Y + ") Velocity=(" + Player.Velocity.X + "," + Player.Velocity.Y + ")";
				myGame.debugStrings[2] = debugTable.ReturnStringSegment(0, 2) + "Agility=" + Player.PAgility + " Strength=" + Player.PStrength + " Defense=" + Player.PDefense + " StatPoints=" + Player.StatPoints + "Stat Level=" + Player.Lvl;
				myGame.debugStrings[3] = debugTable.ReturnStringSegment(2, 0);
				myGame.debugStrings[4] = debugTable.ReturnStringSegment(2, 1) + "Position[0]=(" + ListOfCheckpoints[0].X + "," + ListOfCheckpoints[0].Y + ") Position[1]=(" + ListOfCheckpoints[1].X + "," + ListOfCheckpoints[1].Y + ")";
				myGame.debugStrings[5] = debugTable.ReturnStringSegment(2, 2) + "CurrentCheckPoint=" + LastCheckpoint + " CheckPointIndex[0]=" + (PlaceableObjectsList[0] as CheckPoint).CheckpointIndex + " CheckPointIndex[1]=" + (PlaceableObjectsList[1] as CheckPoint).CheckpointIndex + " Index=" + checkpointNum;
				if (LastCheckpoint > 0)
				{
					myGame.debugStrings[5] = debugTable.ReturnStringSegment(2, 2) + "Cords=(" + ListOfCheckpoints[LastCheckpoint - 1].X + "," + ListOfCheckpoints[LastCheckpoint - 1].Y + ")";
				}
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
			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);
			{
				SewerBackgroundParallax.Draw(gameTime, spriteBatch);
				SewerBackgroundSprayParallax.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.GetTransformation());
			{
				foreach (PlaceableObject po in PlaceableObjectsList)
				{
					po.Draw(gameTime, spriteBatch);
				}

				spriteBatch.Draw(HealthOverheadBackgroundTexture, new Vector2(Player.Position.X - ((50 - 35) / 2), Player.Position.Y - 12), Color.White);
				OverheadHealthBar.Draw(gameTime, spriteBatch);

				foreach (Tile t in TilesList)
				{
					if (Camera.IsInView(t.Position, new Vector2(35, 35)))
					{
						t.Draw(gameTime, spriteBatch);
					}
				}

				foreach (HealthPickUp h in HpDropList)
				{
					h.Draw(gameTime, spriteBatch);
				}

				foreach (ExpPickUp e in ExpDropList)
				{
					e.Draw(gameTime, spriteBatch);
				}

				foreach (Enemy e in EnemyList)
				{
					e.Draw(gameTime, spriteBatch);
				}

				Player.Draw(gameTime, spriteBatch);

				foreach (Particle p in ParticleList)
				{
					p.Draw(gameTime, spriteBatch);

				}
			}
			spriteBatch.End();

			spriteBatch.Begin();
			{
				spriteBatch.Draw(HealthBackgroundTexture, new Vector2(15, 15), Color.White);

				HealthBar.Draw(gameTime, spriteBatch);

				myGame.debugLabel.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			#region Debug Stuff
			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, Camera.GetTransformation());
			{
				if (myGame.IsGameDebug && LevelLoaded)
				{
					foreach (Rectangle r in MapBoundries)
					{
						spriteBatch.Draw(LineTexture, new Rectangle(r.X, r.Y, r.Width, 1), Color.White);
						spriteBatch.Draw(LineTexture, new Rectangle(r.Right - 1, r.Y, 1, r.Height), Color.White);
						spriteBatch.Draw(LineTexture, new Rectangle(r.X, r.Bottom - 1, r.Width, 1), Color.White);
						spriteBatch.Draw(LineTexture, new Rectangle(r.X, r.Y, 1, r.Height), Color.White);
					}

					foreach (PlaceableObject po in PlaceableObjectsList)
					{
						Sprite.DrawBoundingCollisions(LineTexture, po.BoundingCollisions, Color.Red, spriteBatch);
					}

					foreach (Enemy e in EnemyList)
					{
						Sprite.DrawBoundingCollisions(LineTexture, e.BoundingCollisions, Color.Magenta, spriteBatch);
					}

					Sprite.DrawBoundingCollisions(LineTexture, Player.DebugBlock, Color.Lime, spriteBatch);
					Sprite.DrawBoundingCollisions(LineTexture, Player.BoundingCollisions, Color.Blue, spriteBatch);
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
					tiles = MapHelper.ImgToLevel(TestLevelTextureMap);
					Size = new Point(tiles.GetLength(0), tiles.GetLength(1));
					break;
			}

			Camera.Size = new Point(Size.X * 35, Size.Y * 35);

			for (int x = 0; x < Size.X; x++)
			{
				for (int y = 0; y < Size.Y; y++)
				{
					if (tiles[x, y] == 77)
					{
						Player.Position = new Vector2(x * 35, y * 35);
					}
					else if (tiles[x, y] == 78)
					{
						Enemy tempEnemy1 = new Enemy(SpitterAnimationSet, "IDLE", new Vector2(x * 35, y * 35), Enemy.EnemyTypes.ROLLER, new Color(myGame.gameManager.Random.Next(0, 255), myGame.gameManager.Random.Next(0, 255), myGame.gameManager.Random.Next(0, 255)), myGame);
						tempEnemy1.Scale = 0.5f;
						int width = (int)(tempEnemy1.CurrentAnimation.frameSize.X * tempEnemy1.Scale);
						int left = (int)(tempEnemy1.CurrentAnimation.frameSize.X * tempEnemy1.Scale - width);
						int height = (int)(tempEnemy1.CurrentAnimation.frameSize.Y * tempEnemy1.Scale);
						int top = (int)(tempEnemy1.CurrentAnimation.frameSize.Y * tempEnemy1.Scale - height);
						tempEnemy1.Inbounds = new Rectangle(left, top, width, height);
						EnemyList.Add(tempEnemy1);
					}
                    else if (tiles[x, y] == 79)
                    {
                        Enemy tempEnemy2 = new Enemy(SpitterAnimationSet, "IDLE", new Vector2(x * 35, y * 35), Enemy.EnemyTypes.SPITTER, new Color(myGame.gameManager.Random.Next(0, 255), myGame.gameManager.Random.Next(0, 255), myGame.gameManager.Random.Next(0, 255)), myGame);
                        tempEnemy2.Scale = 0.5f;
                        int width = (int)(tempEnemy2.CurrentAnimation.frameSize.X * tempEnemy2.Scale);
                        int left = (int)(tempEnemy2.CurrentAnimation.frameSize.X * tempEnemy2.Scale - width);
                        int height = (int)(tempEnemy2.CurrentAnimation.frameSize.Y * tempEnemy2.Scale);
                        int top = (int)(tempEnemy2.CurrentAnimation.frameSize.Y * tempEnemy2.Scale - height);
                        tempEnemy2.Inbounds = new Rectangle(left, top, width, height);
                        EnemyList.Add(tempEnemy2);
                    }
                    else if (tiles[x, y] == 80)
                    {
                        Enemy tempEnemy3 = new Enemy(SpitterAnimationSet, "IDLE", new Vector2(x * 35, y * 35), Enemy.EnemyTypes.JUMPER, new Color(myGame.gameManager.Random.Next(0, 255), myGame.gameManager.Random.Next(0, 255), myGame.gameManager.Random.Next(0, 255)), myGame);
                        tempEnemy3.Scale = 0.5f;
                        int width = (int)(tempEnemy3.CurrentAnimation.frameSize.X * tempEnemy3.Scale);
                        int left = (int)(tempEnemy3.CurrentAnimation.frameSize.X * tempEnemy3.Scale - width);
                        int height = (int)(tempEnemy3.CurrentAnimation.frameSize.Y * tempEnemy3.Scale);
                        int top = (int)(tempEnemy3.CurrentAnimation.frameSize.Y * tempEnemy3.Scale - height);
                        tempEnemy3.Inbounds = new Rectangle(left, top, width, height);
                        EnemyList.Add(tempEnemy3);
                    }
                    else if (tiles[x, y] == 81)
                    {
                        Enemy tempEnemy4 = new Enemy(CrawlerAnimationSet, "IDLE", new Vector2(x * 35, y * 35), Enemy.EnemyTypes.CHARGER, new Color(myGame.gameManager.Random.Next(0, 255), myGame.gameManager.Random.Next(0, 255), myGame.gameManager.Random.Next(0, 255)), myGame);
                        tempEnemy4.Scale = 0.5f;
                        int width = (int)(tempEnemy4.CurrentAnimation.frameSize.X * tempEnemy4.Scale);
                        int left = (int)(tempEnemy4.CurrentAnimation.frameSize.X * tempEnemy4.Scale - width);
                        int height = (int)(tempEnemy4.CurrentAnimation.frameSize.Y * tempEnemy4.Scale);
                        int top = (int)(tempEnemy4.CurrentAnimation.frameSize.Y * tempEnemy4.Scale - height);
                        tempEnemy4.Inbounds = new Rectangle(left, top, width, height);
                        EnemyList.Add(tempEnemy4);
					}
					if (tiles[x, y] == 25)
					{
						CheckPoint tempCheckPoint = new CheckPoint(CheckpointTexture, new Vector2(x * 35, y * 35), Color.White, checkpointNum, myGame);
						PlaceableObjectsList.Add(tempCheckPoint);
						ListOfCheckpoints[checkpointNum - 1] = new Vector2(x * 35, y * 35);
						checkpointNum += 1;
					}
					if (tiles[x, y] > 25)
					{
						tiles[x, y] = 0;
					}

					if (tiles[x, y] > 0 && tiles[x, y] < 25)
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
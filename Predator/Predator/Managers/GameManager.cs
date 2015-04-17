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
using VoidEngine;


namespace Predator
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class GameManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		Game1 myGame;
		SpriteBatch spriteBatch;

		Camera camera;

		Random rng = new Random();

		#region Textures
		/// <summary>
		/// Loads the line texture.
		/// </summary>
		public Texture2D line;
		/// <summary>
		/// Loads the temp player texture.
		/// </summary>
		public Texture2D playerTemp;
		/// <summary>
		///
		/// </summary>
		public Texture2D tempTileTexture;
		/// <summary>
		///
		/// </summary>
		public Texture2D healthBGTexture;
		/// <summary>
		///
		/// </summary>
		public Texture2D healthFGTexture;
		/// <summary>
		///
		/// </summary>
		public Texture2D healthOverheadBGTexture;
		/// <summary>
		///
		/// </summary>
		public Texture2D healthOverheadFGTexture;
		/// <summary>
		///
		/// </summary>
		public Texture2D projectileTexture;
		///
		public Texture2D particleTex;
		#endregion

		#region Lists & Objects
		/// <summary>
		/// Gets or sets the first enemy list.
		/// Enemy: UNKNOWN
		/// </summary>
		public List<Enemy> EnemyList1
		{
			get;
			set;
		}
		/// <summary>
		///
		/// </summary>
		public List<Tile> tileObjects
		{
			get;
			set;
		}
		/// <summary>
		///
		/// </summary>
		public List<Sprite.AnimationSet> tileAnimationSets
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the map boarders list.
		/// </summary>
		public List<Rectangle> MapBoundries
		{
			get;
			set;
		}
		/// <summary>
		/// The default player.
		/// </summary>
		public Player player;
		/// <summary>
		/// The player's animation set.
		/// </summary>
		public List<Sprite.AnimationSet> playerAnimationSet;
		/// <summary>
		///
		/// </summary>
		public List<Sprite.AnimationSet> projectileAnimationSet;
		/// <summary>
		/// The player's movement keys.
		/// </summary>
		public Keys[,] movementKeys = new Keys[4, 15];
		/// <summary>
		///
		/// </summary>
		public HealthBar healthBar;
		/// <summary>
		///
		/// </summary>
		public List<Sprite.AnimationSet> healthBarAnimationSetList;
		/// <summary>
		///
		/// </summary>
		public HealthBar playerHealthBar;
		/// <summary>
		///
		/// </summary>
		public List<Sprite.AnimationSet> playerHealthBarAnimationSetList;
		#endregion

		#region Level & Transition Stuff
		/// <summary>
		/// Gets or sets the level of the game.
		/// </summary>
		public int Level
		{
			get;
			set;
		}
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
		List<Particle> particleList = new List<Particle>();
		List<Sprite.AnimationSet> particalAnimationSet;
		public float bloodx;
		public float bloody;
		#endregion

		#region Debug Stuff
		Label debugLabel;
		string[] debugStrings = new string[10];
		#endregion

		/// <summary>
		/// Creates the game manager
		/// </summary>
		/// <param name="game">The game that the manager is running off of.</param>
		public GameManager(Game1 game)
			: base(game)
		{
			myGame = game;
			spriteBatch = new SpriteBatch(myGame.GraphicsDevice);

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
			MapBoundries = new List<Rectangle>();

			tileObjects = new List<Tile>();
			tileAnimationSets = new List<Sprite.AnimationSet>();

			playerAnimationSet = new List<Sprite.AnimationSet>();

			particalAnimationSet = new List<Sprite.AnimationSet>();

			healthBarAnimationSetList = new List<Sprite.AnimationSet>();
			playerHealthBarAnimationSetList = new List<Sprite.AnimationSet>();

			projectileAnimationSet = new List<Sprite.AnimationSet>();

			EnemyList1 = new List<Enemy>();

			base.Initialize();
		}

		/// <summary>
		/// Loads the game component's content.
		/// </summary>
		protected override void LoadContent()
		{
			line = Game.Content.Load<Texture2D>(@"images\other\line");
			playerTemp = Game.Content.Load<Texture2D>(@"images\player\temp");
			tempTileTexture = Game.Content.Load<Texture2D>(@"images\tiles\temp");
			healthFGTexture = Game.Content.Load<Texture2D>(@"images\gui\healthFG");
			healthBGTexture = Game.Content.Load<Texture2D>(@"images\gui\healthBG");
			healthOverheadFGTexture = Game.Content.Load<Texture2D>(@"images\gui\healthOHFG");
			healthOverheadBGTexture = Game.Content.Load<Texture2D>(@"images\gui\healthOHBG");
			projectileTexture = Game.Content.Load<Texture2D>(@"images\player\attackTemp");
			particleTex = Game.Content.Load<Texture2D>(@"images\other\testParticle");

			tileAnimationSets.Add(new Sprite.AnimationSet("0", tempTileTexture, new Point(0, 0), new Point(1, 1), new Point(0, 0), 1600, false));
			tileAnimationSets.Add(new Sprite.AnimationSet("1", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(0, 0), 1600, false));
			tileAnimationSets.Add(new Sprite.AnimationSet("2", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(35, 0), 1600, false));
			tileAnimationSets.Add(new Sprite.AnimationSet("3", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(70, 0), 1600, false));
			tileAnimationSets.Add(new Sprite.AnimationSet("4", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(105, 0), 1600, false));
			tileAnimationSets.Add(new Sprite.AnimationSet("5", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(0, 35), 1600, false));
			tileAnimationSets.Add(new Sprite.AnimationSet("6", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(35, 35), 1600, false));
			tileAnimationSets.Add(new Sprite.AnimationSet("7", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(70, 35), 1600, false));
			tileAnimationSets.Add(new Sprite.AnimationSet("8", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(105, 35), 1600, false));
			tileAnimationSets.Add(new Sprite.AnimationSet("9", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(0, 70), 1600, false));
			tileAnimationSets.Add(new Sprite.AnimationSet("10", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(35, 70), 1600, false));
			tileAnimationSets.Add(new Sprite.AnimationSet("11", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(70, 70), 1600, false));
			tileAnimationSets.Add(new Sprite.AnimationSet("12", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(105, 70), 1600, false));
			tileAnimationSets.Add(new Sprite.AnimationSet("13", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(0, 105), 1600, false));
			tileAnimationSets.Add(new Sprite.AnimationSet("14", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(35, 105), 1600, false));
			tileAnimationSets.Add(new Sprite.AnimationSet("15", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(70, 105), 1600, false));
			tileAnimationSets.Add(new Sprite.AnimationSet("16", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(105, 105), 1600, false));

			playerAnimationSet.Add(new Sprite.AnimationSet("IDLE1", playerTemp, new Point(35, 50), new Point(1, 1), new Point(000, 000), 1600, true));
			playerAnimationSet.Add(new Sprite.AnimationSet("WALK1", playerTemp, new Point(35, 50), new Point(1, 1), new Point(035, 000), 1600, true));
			playerAnimationSet.Add(new Sprite.AnimationSet("JUMP1", playerTemp, new Point(35, 50), new Point(1, 1), new Point(070, 000), 1600, true));
			playerAnimationSet.Add(new Sprite.AnimationSet("FALL1", playerTemp, new Point(35, 50), new Point(1, 1), new Point(105, 000), 1600, true));
			playerAnimationSet.Add(new Sprite.AnimationSet("HURT1", playerTemp, new Point(35, 50), new Point(1, 1), new Point(140, 000), 1600, true));
			playerAnimationSet.Add(new Sprite.AnimationSet("ATK-1", playerTemp, new Point(35, 50), new Point(1, 1), new Point(175, 000), 1600, true));
			playerAnimationSet.Add(new Sprite.AnimationSet("DIE-1", playerTemp, new Point(35, 50), new Point(1, 1), new Point(210, 000), 1600, true));
			playerAnimationSet.Add(new Sprite.AnimationSet("GAIN1", playerTemp, new Point(35, 50), new Point(1, 1), new Point(245, 000), 1600, true));
			playerAnimationSet.Add(new Sprite.AnimationSet("IDLE2", playerTemp, new Point(35, 50), new Point(1, 1), new Point(000, 050), 1600, true));
			playerAnimationSet.Add(new Sprite.AnimationSet("WALK2", playerTemp, new Point(35, 50), new Point(1, 1), new Point(035, 050), 1600, true));
			playerAnimationSet.Add(new Sprite.AnimationSet("JUMP2", playerTemp, new Point(35, 50), new Point(1, 1), new Point(070, 050), 1600, true));
			playerAnimationSet.Add(new Sprite.AnimationSet("FALL2", playerTemp, new Point(35, 50), new Point(1, 1), new Point(105, 050), 1600, true));
			playerAnimationSet.Add(new Sprite.AnimationSet("HURT2", playerTemp, new Point(35, 50), new Point(1, 1), new Point(140, 050), 1600, true));
			playerAnimationSet.Add(new Sprite.AnimationSet("ATK-2", playerTemp, new Point(35, 50), new Point(1, 1), new Point(175, 050), 1600, true));
			playerAnimationSet.Add(new Sprite.AnimationSet("DIE-2", playerTemp, new Point(35, 50), new Point(1, 1), new Point(210, 050), 1600, true));
			playerAnimationSet.Add(new Sprite.AnimationSet("GAIN2", playerTemp, new Point(35, 50), new Point(1, 1), new Point(245, 050), 1600, true));

			particalAnimationSet.Add(new Sprite.AnimationSet("IDLE", particleTex, new Point(particleTex.Width, particleTex.Height), Point.Zero, Point.Zero, 1600, false));

			projectileAnimationSet.Add(new Sprite.AnimationSet("IDLE", projectileTexture, new Point(40, 24), new Point(3, 1), new Point(0, 0), 50, false));

			healthBarAnimationSetList.Add(new Sprite.AnimationSet("IDLE", healthFGTexture, new Point(190, 16), new Point(1, 1), new Point(0, 0), 16000, false));
			playerHealthBarAnimationSetList.Add(new Sprite.AnimationSet("IDLE", healthOverheadFGTexture, new Point(44, 3), new Point(1, 1), new Point(0, 0), 16000, false));

			movementKeys[0, 0] = Keys.A;
			movementKeys[0, 1] = Keys.W;
			movementKeys[0, 2] = Keys.D;
			movementKeys[0, 3] = Keys.S;
			movementKeys[0, 4] = Keys.Space;
			movementKeys[0, 5] = Keys.E;
			movementKeys[1, 0] = Keys.Left;
			movementKeys[1, 1] = Keys.Up;
			movementKeys[1, 2] = Keys.Right;
			movementKeys[1, 3] = Keys.Down;

			player = new Player(new Vector2(100, 100), movementKeys, 100f, Color.White, playerAnimationSet, projectileAnimationSet);

			camera = new Camera(myGame.GraphicsDevice.Viewport, new Point(1024, 768), 0.9f);

			camera.Rotation = 45 * (float)Math.PI / 180;

			MapBoundries.Add(new Rectangle(-5, -5, 5, camera.Size.Y + 10));
			MapBoundries.Add(new Rectangle(-5, -5, camera.Size.X + 10, 5));
			MapBoundries.Add(new Rectangle(camera.Size.X, -5, 5, camera.Size.Y + 10));
			MapBoundries.Add(new Rectangle(-5, camera.Size.Y, camera.Size.X + 10, 5));

			healthBar = new HealthBar(new Vector2(20, 25), Color.White, healthBarAnimationSetList, player);

			playerHealthBar = new HealthBar(new Vector2(player.GetPosition.X - ((48 - 35) / 2), player.GetPosition.Y - 10), Color.White, playerHealthBarAnimationSetList, player);

			debugLabel = new Label(new Vector2(0, 60), myGame.segoeUIMonoDebug, 1f, Color.Black, "");

			base.LoadContent();
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			if (myGame.keyboardState.IsKeyDown(Keys.NumPad4))
			{
				camera.Rotation += 1 * (float)Math.PI / 180;
			}
			if (myGame.keyboardState.IsKeyDown(Keys.NumPad6))
			{
				camera.Rotation -= 1 * (float)Math.PI / 180;
			}
			if (myGame.keyboardState.IsKeyDown(Keys.NumPad8))
			{
				camera.Zoom += 0.01f;
			}
			if (myGame.keyboardState.IsKeyDown(Keys.NumPad2))
			{
				camera.Zoom -= 0.01f;
			}
			if (myGame.keyboardState.IsKeyDown(Keys.NumPad5))
			{
				camera.Zoom = 1f;
				camera.Rotation = 0;
			}
			if (myGame.CheckKey(Keys.M) && !myGame.mapScreenManager.isTransitioningIn)
			{
				myGame.mapScreenManager.isTransitioningIn = true;
				myGame.SetCurrentLevel(Game1.GameLevels.MAP);
			}
			if (myGame.CheckKey(Keys.G))
			{
				myGame.SetCurrentLevel(Game1.GameLevels.STATS);
			}

			if (myGame.CheckKey(Keys.Q))
			{
				myGame.SetCurrentLevel(Game1.GameLevels.MENU);
			}
			if (!LevelLoaded)
			{
				SpawnTiles(0);

				MapBoundries[0] = new Rectangle(-35, -35, 35, camera.Size.Y + 70);
				MapBoundries[1] = new Rectangle(camera.Size.X, -35, 35, camera.Size.Y + 70);
				MapBoundries[2] = new Rectangle(-35, -35, camera.Size.X + 70, 35);
				MapBoundries[3] = new Rectangle(-35, camera.Size.Y, camera.Size.X + 70, 35);

				LevelLoaded = true;
			}

			if (Keyboard.GetState().IsKeyDown(Keys.C))
			{
				//ParticleSystem.CreateParticles(player.GetPosition + player.RotationCenter, particleTex, rng, particleList, particalAnimationSet, rng, rng, rng, rng, rng, 50, 40, 800, 135, 585, 5000, 10000, 3, 5, 200, 255);
				ParticleSystem.CreateParticles(player.GetPosition + player.RotationCenter, particleTex, rng, particleList, particalAnimationSet, 250, 250, 0, 0, 0, 0, 40, 800, 135, 295, 5000, 6000, 3, 5, 200, 255);
			}

			player.UpdateKeyboardState(gameTime, myGame.keyboardState);
			if (LevelLoaded)
			{
				player.Update(gameTime, EnemyList1, tileObjects, MapBoundries);
			}
			if (camera.IsInView(player.GetPosition, new Vector2(player.CurrentAnimation.frameSize.X, player.CurrentAnimation.frameSize.Y)))
			{
				camera.Position = player.GetPosition;
			}

			healthBar.Update(gameTime);
			playerHealthBar.Update(gameTime);
			playerHealthBar.SetPosition = new Vector2(player.GetPosition.X - ((44 - 35) / 2), player.GetPosition.Y - 7);

			foreach (Enemy e in EnemyList1)
			{
				if (e.isHit)
				{
					ParticleSystem.CreateParticles(e.PositionCenter - new Vector2(0, 15), particleTex, rng, particleList, particalAnimationSet, 230, 255, 0, 0, 0, 0, 5, 10, (int)bloodx, (int)bloody, 100, 250, 3, 5, 200, 255);
					e.isHit = false;
				}
				if (LevelLoaded)
				{
					e.Update(gameTime, player, tileObjects, MapBoundries);
				}
			}
			for (int i = 0; i < particleList.Count; i++)
			{
				if (particleList[i].deleteMe)
				{
					particleList.RemoveAt(i);
					i--;
				}
				else
				{
					particleList[i].Update(gameTime);
				}
			}

			foreach (Tile t in tileObjects)
			{
				t.Update(gameTime);
			}

			ratio = (gameTime.ElapsedGameTime.Milliseconds + 1) / (gameTime.TotalGameTime.Milliseconds + 1);

			if (ratio <= 0.1)
			{
				washColor = Color.Lerp(new Color((int)(255 * ratio * 2), 255, 0, 0.5f), new Color(0, 255, (int)(255 * ratio * 2), 0.5f), 100f);
			}
			if (ratio >= 0.9)
			{
				washColor = Color.Lerp(new Color(0, 255, (int)(255 * ratio * 2), 0.5f), new Color((int)(255 * (1 - ratio) * 2), 255, 0, 0.5f), 100f);
			}

			debugStrings[0] = "Player || JumpTime=" + player.JumpTime + " isGrounded=" + player.isGrounded;
			debugStrings[1] = "       || Position=(" + player.GetPosition.X + "," + player.GetPosition.Y + ")";
			debugStrings[2] = "Color  || Color=(" + washColor.R + "," + washColor.G + "," + washColor.B + "," + washColor.A;

			if (myGame.keyboardState.IsKeyDown(Keys.Y))
			{
				EnemyList1[0].isJumping2 = true;
			}

			debugLabel.Text = debugStrings[0] + "\n" +
							  debugStrings[1] + "\n" +
							  debugStrings[2] + "\n" +
							  debugStrings[3] + "\n" +
							  debugStrings[4] + "\n" +
							  debugStrings[5] + "\n" +
							  debugStrings[6] + "\n" +
							  debugStrings[7] + "\n" +
							  debugStrings[8] + "\n" +
							  debugStrings[9];

			// TODO: Add your update code here

			base.Update(gameTime);
		}

		double ratio;

		Color washColor;

		/// <summary>
		/// Draws the game component's content.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, camera.GetTransformation());
			{
				foreach (Enemy e in EnemyList1)
				{
					e.Draw(gameTime, spriteBatch);
				}

				foreach (Tile t in tileObjects)
				{
					t.Draw(gameTime, spriteBatch);
				}

				spriteBatch.Draw(healthOverheadBGTexture, new Vector2(player.GetPosition.X - ((50 - 35) / 2), player.GetPosition.Y - 12), Color.White);

				playerHealthBar.Draw(gameTime, spriteBatch);

				player.Draw(gameTime, spriteBatch);

				foreach (Particle p in particleList)
				{
					p.Draw(gameTime, spriteBatch);
				}
			}
			spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);
			{
				spriteBatch.Draw(healthBGTexture, new Vector2(15, 15), Color.White);

				healthBar.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, camera.GetTransformation());
			{
				foreach (Tile t in tileObjects)
				{
					if (t.tileCollisions == Tile.TileCollisions.Impassable)
					{
						spriteBatch.Draw(line, new Rectangle(t.Collisions.X, t.Collisions.Y, t.Collisions.Width, 1), Color.White);
						spriteBatch.Draw(line, new Rectangle(t.Collisions.Right - 1, t.Collisions.Y, 1, t.Collisions.Height), Color.White);
						spriteBatch.Draw(line, new Rectangle(t.Collisions.X, t.Collisions.Bottom - 1, t.Collisions.Width, 1), Color.White);
						spriteBatch.Draw(line, new Rectangle(t.Collisions.X, t.Collisions.Y, 1, t.Collisions.Height), Color.White);
					}
					if (t.tileCollisions == Tile.TileCollisions.Platform)
					{
						spriteBatch.Draw(line, new Rectangle(t.Collisions.X, t.Collisions.Y, t.Collisions.Width, 1), Color.Blue);
					}
				}
				foreach (Rectangle r in MapBoundries)
				{
					spriteBatch.Draw(line, new Rectangle(r.X, r.Y, r.Width, 1), Color.White);
					spriteBatch.Draw(line, new Rectangle(r.Right - 1, r.Y, 1, r.Height), Color.White);
					spriteBatch.Draw(line, new Rectangle(r.X, r.Bottom - 1, r.Width, 1), Color.White);
					spriteBatch.Draw(line, new Rectangle(r.X, r.Y, 1, r.Height), Color.White);
				}

				foreach (Enemy e in EnemyList1)
				{
					spriteBatch.Draw(line, new Rectangle(e.test.X, e.test.Y, e.test.Width, 1), Color.Red);
					spriteBatch.Draw(line, new Rectangle(e.test.Right - 1, e.test.Y, 1, e.test.Height), Color.Red);
					spriteBatch.Draw(line, new Rectangle(e.test.X, e.test.Bottom - 1, e.test.Width, 1), Color.Red);
					spriteBatch.Draw(line, new Rectangle(e.test.X, e.test.Y, 1, e.test.Height), Color.Red);
					spriteBatch.Draw(line, new Rectangle(e.BoundingCollisions.X, e.BoundingCollisions.Y, e.BoundingCollisions.Width, 1), Color.Magenta);
					spriteBatch.Draw(line, new Rectangle(e.BoundingCollisions.Right - 1, e.BoundingCollisions.Y, 1, e.BoundingCollisions.Height), Color.Magenta);
					spriteBatch.Draw(line, new Rectangle(e.BoundingCollisions.X, e.BoundingCollisions.Bottom - 1, e.BoundingCollisions.Width, 1), Color.Magenta);
					spriteBatch.Draw(line, new Rectangle(e.BoundingCollisions.X, e.BoundingCollisions.Y, 1, e.BoundingCollisions.Height), Color.Magenta);
				}

				spriteBatch.Draw(line, new Rectangle(player.BoundingCollisions.X, player.BoundingCollisions.Y, player.BoundingCollisions.Width, 1), Color.Lime);
				spriteBatch.Draw(line, new Rectangle(player.BoundingCollisions.Right - 1, player.BoundingCollisions.Y, 1, player.BoundingCollisions.Height), Color.Lime);
				spriteBatch.Draw(line, new Rectangle(player.BoundingCollisions.X, player.BoundingCollisions.Bottom - 1, player.BoundingCollisions.Width, 1), Color.Lime);
				spriteBatch.Draw(line, new Rectangle(player.BoundingCollisions.X, player.BoundingCollisions.Y, 1, player.BoundingCollisions.Height), Color.Lime);
				spriteBatch.Draw(line, new Rectangle(player.test.X, player.test.Y, player.test.Width, 1), Color.Blue);
				spriteBatch.Draw(line, new Rectangle(player.test.Right - 1, player.test.Y, 1, player.test.Height), Color.Blue);
				spriteBatch.Draw(line, new Rectangle(player.test.X, player.test.Bottom - 1, player.test.Width, 1), Color.Blue);
				spriteBatch.Draw(line, new Rectangle(player.test.X, player.test.Y, 1, player.test.Height), Color.Blue);
			}
			spriteBatch.End();

			spriteBatch.Begin();
			{
				spriteBatch.Draw(line, new Rectangle(0, 0, myGame.WindowSize.X, myGame.WindowSize.Y), washColor);

				debugLabel.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}

		public void SpawnTiles(int level)
		{
			Point Size = new Point(0, 0);
			uint[,] tiles = new uint[0, 0];

			switch (level)
			{
				case 0:
					tiles = MapHelper.GetTileArray(Maps.TestLevel());
					Size = new Point(Maps.TestLevel()[0].Length, Maps.TestLevel().Count);
					break;
			}

			camera.Size = new Point(Size.X * 35, Size.Y * 35);

			for (int x = 0; x < Size.X; x++)
			{
				for (int y = 0; y < Size.Y; y++)
				{
					if (tiles[x, y] == 70)
					{
						player.SetPosition = new Vector2(x * 35, y * 35);
					}
					else if (tiles[x, y] == 71)
					{
						Enemy tempEnemy1 = new Enemy(new Vector2(x * 35, y * 35), Enemy.EnemyType.RAT, Color.White, playerAnimationSet, myGame);
						EnemyList1.Add(tempEnemy1);
					}

					if (tiles[x, y] > 69)
					{
						tiles[x, y] = 0;
					}

					if (tiles[x, y] == 0)
					{
						tileObjects.Add(new Tile(new Vector2(x * 35, y * 35), (int)tiles[x, y], Tile.TileCollisions.Passable, new Rectangle(x * 35, y * 35, 35, 35), Color.White, tileAnimationSets));
					}

					if (tiles[x, y] > 0)
					{
						tileObjects.Add(new Tile(new Vector2(x * 35, y * 35), (int)tiles[x, y], Tile.TileCollisions.Impassable, new Rectangle(x * 35, y * 35, 35, 35), Color.White, tileAnimationSets));
					}
				}
			}

			camera.Position = player.GetPosition;
		}
	}
}

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
		/// Gets or sets the map tiles list.
		/// </summary>
		public List<Rectangle> mapTiles
		{
			get;
			set;
		}
		/// <summary>
		/// 
		/// </summary>
		public List<Tiles> tileObjects
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
		public List<Rectangle> mapBoarders
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
		/// The player's movement keys.
		/// </summary>
		public List<Keys> movementKeys;
		/// <summary>
		/// The mana of the player.
		/// </summary>
		public Player.Mana mana;
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
		/// <summary>
		/// Gets or sets the tranition alpha variable.
		/// </summary>
		public float TransitionAlpha
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the level is transitioning.
		/// </summary>
		public bool isTransitioning
		{
			get;
			set;
		}
		#endregion

		#region Debug Stuff
		Label debugLabel;
		string[] debugStrings =  new string[10];
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
			mapTiles = new List<Rectangle>();
			mapBoarders = new List<Rectangle>();

			tileObjects = new List<Tiles>();
			tileAnimationSets = new List<Sprite.AnimationSet>();

			playerAnimationSet = new List<Sprite.AnimationSet>();
			movementKeys = new List<Keys>();

			healthBarAnimationSetList = new List<Sprite.AnimationSet>();
			playerHealthBarAnimationSetList = new List<Sprite.AnimationSet>();

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

			camera = new Camera(myGame.GraphicsDevice.Viewport, new Point(1024, 768), 1f);
			camera.Position = Vector2.Zero;

			mapBoarders.Add(new Rectangle(-5, 0, 5, 768));

			tileAnimationSets.Add(new Sprite.AnimationSet("1", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(0, 0), 16000));
			tileAnimationSets.Add(new Sprite.AnimationSet("2", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(35, 0), 16000));
			tileAnimationSets.Add(new Sprite.AnimationSet("3", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(70, 0), 16000));
			tileAnimationSets.Add(new Sprite.AnimationSet("4", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(105, 0), 16000));
			tileAnimationSets.Add(new Sprite.AnimationSet("5", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(0, 35), 16000));
			tileAnimationSets.Add(new Sprite.AnimationSet("6", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(35, 35), 16000));
			tileAnimationSets.Add(new Sprite.AnimationSet("7", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(70, 35), 16000));
			tileAnimationSets.Add(new Sprite.AnimationSet("8", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(105, 35), 16000));
			tileAnimationSets.Add(new Sprite.AnimationSet("9", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(0, 70), 16000));
			tileAnimationSets.Add(new Sprite.AnimationSet("10", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(35, 70), 16000));
			tileAnimationSets.Add(new Sprite.AnimationSet("11", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(70, 70), 16000));
			tileAnimationSets.Add(new Sprite.AnimationSet("12", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(105, 70), 16000));
			tileAnimationSets.Add(new Sprite.AnimationSet("13", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(0, 105), 16000));
			tileAnimationSets.Add(new Sprite.AnimationSet("14", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(35, 105), 16000));
			tileAnimationSets.Add(new Sprite.AnimationSet("15", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(70, 105), 16000));
			tileAnimationSets.Add(new Sprite.AnimationSet("16", tempTileTexture, new Point(35, 35), new Point(1, 1), new Point(105, 105), 16000));

			playerAnimationSet.Add(new Sprite.AnimationSet("IDLE1", playerTemp, new Point(35, 50), new Point(1, 1), new Point(000, 000), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("WALK1", playerTemp, new Point(35, 50), new Point(1, 1), new Point(035, 000), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("JUMP1", playerTemp, new Point(35, 50), new Point(1, 1), new Point(070, 000), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("FALL1", playerTemp, new Point(35, 50), new Point(1, 1), new Point(105, 000), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("HURT1", playerTemp, new Point(35, 50), new Point(1, 1), new Point(140, 000), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("ATK-1", playerTemp, new Point(35, 50), new Point(1, 1), new Point(175, 000), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("DIE-1", playerTemp, new Point(35, 50), new Point(1, 1), new Point(210, 000), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("GAIN1", playerTemp, new Point(35, 50), new Point(1, 1), new Point(245, 000), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("IDLE2", playerTemp, new Point(35, 50), new Point(1, 1), new Point(000, 050), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("WALK2", playerTemp, new Point(35, 50), new Point(1, 1), new Point(035, 050), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("JUMP2", playerTemp, new Point(35, 50), new Point(1, 1), new Point(070, 050), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("FALL2", playerTemp, new Point(35, 50), new Point(1, 1), new Point(105, 050), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("HURT2", playerTemp, new Point(35, 50), new Point(1, 1), new Point(140, 050), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("ATK-2", playerTemp, new Point(35, 50), new Point(1, 1), new Point(175, 050), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("DIE-2", playerTemp, new Point(35, 50), new Point(1, 1), new Point(210, 050), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("GAIN2", playerTemp, new Point(35, 50), new Point(1, 1), new Point(245, 050), 16000));

			healthBarAnimationSetList.Add(new Sprite.AnimationSet("IDLE", healthFGTexture, new Point(190, 16), new Point(1, 1), new Point(0, 0), 16000));
			playerHealthBarAnimationSetList.Add(new Sprite.AnimationSet("IDLE", healthOverheadFGTexture, new Point(44, 3), new Point(1, 1), new Point(0, 0), 16000));

			movementKeys.Add(Keys.A);
			movementKeys.Add(Keys.W);
			movementKeys.Add(Keys.D);
			movementKeys.Add(Keys.S);
			movementKeys.Add(Keys.Space);
			movementKeys.Add(Keys.E);

			mana = new Player.Mana(0, 0, 0, 0);

			player = new Player(new Vector2(200, 256), movementKeys, 1.75f, mana, Color.White, playerAnimationSet, myGame);

			healthBar = new HealthBar(new Vector2(20, 25), Color.White, healthBarAnimationSetList, player);

			playerHealthBar = new HealthBar(new Vector2(player.GetPosition.X - ((48 - 35) / 2), player.GetPosition.Y - 10), Color.White, playerHealthBarAnimationSetList, player);

			debugLabel = new Label(Vector2.Zero, myGame.segoeUIMonoDebug, 1f, Color.Black, "");
		
			base.LoadContent();
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			player.Update(gameTime);

			if (camera.IsInView(player.GetPosition, new Vector2(player.CurrentAnimation.frameSize.X, player.CurrentAnimation.frameSize.Y)))
			{
				camera.Position = player.GetPosition;
			}

			healthBar.Update(gameTime);
			playerHealthBar.Update(gameTime);
			playerHealthBar.SetPosition = new Vector2(player.GetPosition.X - ((44 - 35) / 2), player.GetPosition.Y - 7);

			foreach (Enemy e in EnemyList1)
			{
				e.Update(gameTime);
			}

			foreach (Tiles t in tileObjects)
			{
				t.Update(gameTime);
			}

			if (!LevelLoaded)
			{
				SpawnTiles(0);

				LevelLoaded = true;
			}

			debugStrings[0] = "Player || canFall=" + player.canFall + " isJumping=" + player.isJumping + " isFalling=" + player.isFalling + " GravityForce=" + player.GravityForce + "";
			debugStrings[1] = "Direction=(" + player.GetDirection.X + "," + player.GetDirection.Y + ") HP=" + player.HP + " PositionCenter=(" + player.PositionCenter.X + "," + player.PositionCenter.Y + ")";
			if (EnemyList1.Count > 0)
			{
				debugStrings[2] = "Enemy1 || PositionCenter=(" + EnemyList1[0].PositionCenter.X + "," + EnemyList1[0].PositionCenter.Y + ")";
			}

			debugLabel.Update(gameTime, debugStrings[0] + "\n" + 
										debugStrings[1] + "\n" + 
										debugStrings[2] + "\n" + 
										debugStrings[3] + "\n" + 
										debugStrings[4] + "\n" + 
										debugStrings[5] + "\n" + 
										debugStrings[6] + "\n" + 
										debugStrings[7] + "\n" + 
										debugStrings[8] + "\n" + 
										debugStrings[9]);

			// TODO: Add your update code here

			base.Update(gameTime);
		}

		/// <summary>
		/// Draws the game component's content.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, camera.GetTransformation());
			{
				player.Draw(gameTime, spriteBatch);
				spriteBatch.Draw(healthOverheadBGTexture, new Vector2(player.GetPosition.X - ((50 - 35) / 2), player.GetPosition.Y - 12), Color.White);

				playerHealthBar.Draw(gameTime, spriteBatch);

				foreach (Enemy e in EnemyList1)
				{
					e.Draw(gameTime, spriteBatch);
				}

				foreach (Tiles t in tileObjects)
				{
					t.Draw(gameTime, spriteBatch);
				}
			}
			spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);
			{
				spriteBatch.Draw(healthBGTexture, new Vector2(15, 15), Color.White);

				healthBar.Draw(gameTime, spriteBatch);

				spriteBatch.Draw(line, new Rectangle(0, 0, myGame.WindowSize.X, myGame.WindowSize.X), new Color(0, 0, 0, TransitionAlpha));
			}
			spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null, null, camera.GetTransformation());
			{
				for (int i = 0; i < mapTiles.Count; i++)
				{
					spriteBatch.Draw(line, new Rectangle(mapTiles[i].X, mapTiles[i].Y, mapTiles[i].Width, 1), Color.Red);
					spriteBatch.Draw(line, new Rectangle(mapTiles[i].X, mapTiles[i].Y, 1, 15), new Color(i % 0.999f, i % 0.999f, i % 0.999f));
				}

				debugLabel.Draw(gameTime, spriteBatch);

				spriteBatch.Draw(line, new Rectangle(player.GetPlayerRectangles().X, player.GetPlayerRectangles().Y, player.GetPlayerRectangles().Width, 1), Color.Lime);
				spriteBatch.Draw(line, new Rectangle(player.GetPlayerRectangles().X + player.GetPlayerRectangles().Width, player.GetPlayerRectangles().Y, 1, player.GetPlayerRectangles().Height), Color.Lime);
				spriteBatch.Draw(line, new Rectangle(player.GetPlayerRectangles().X, player.GetPlayerRectangles().Y + player.GetPlayerRectangles().Height, player.GetPlayerRectangles().Width, 1), Color.Lime);
				spriteBatch.Draw(line, new Rectangle(player.GetPlayerRectangles().X, player.GetPlayerRectangles().Y, 1, player.GetPlayerRectangles().Height), Color.Lime);
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
					if (tiles[x, y] > 0 && tiles[x, y] < 70)
					{
						mapTiles.Add(new Rectangle(x * 35, y * 35, 35, 35));
						tileObjects.Add(new Tiles(new Vector2(x * 35, y * 35), myGame, (int)tiles[x, y], tileAnimationSets));
					}
					else if (tiles[x, y] == 70)
					{
						player.SetPosition = new Vector2(x * 35, y * 35);
					}
					else if (tiles[x, y] == 71)
					{
						Enemy tempEnemy1 = new Enemy(new Vector2(x * 35, y * 35), 1.5f, Enemy.MovementType.RAT, Color.White, playerAnimationSet, player, mapTiles, mapBoarders);
						EnemyList1.Add(tempEnemy1);
					}
				}
			}
		}
	}
}

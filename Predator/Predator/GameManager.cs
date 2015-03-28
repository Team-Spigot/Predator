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

		#region Textures
		/// <summary>
		/// Loads the line texture.
		/// </summary>
		public Texture2D line;
		/// <summary>
		/// Loads the temp player texture.
		/// </summary>
		public Texture2D playerTemp;
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
		List<string> debugStrings;
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

			playerAnimationSet = new List<Sprite.AnimationSet>();
			movementKeys = new List<Keys>();

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

			playerAnimationSet.Add(new Sprite.AnimationSet("IDLE1", playerTemp, new Point(50, 100), new Point(1, 1), new Point(000, 000), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("WALK1", playerTemp, new Point(50, 100), new Point(1, 1), new Point(050, 000), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("JUMP1", playerTemp, new Point(50, 100), new Point(1, 1), new Point(100, 000), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("FALL1", playerTemp, new Point(50, 100), new Point(1, 1), new Point(150, 000), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("HURT1", playerTemp, new Point(50, 100), new Point(1, 1), new Point(200, 000), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("ATK-1", playerTemp, new Point(50, 100), new Point(1, 1), new Point(250, 000), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("DIE-1", playerTemp, new Point(50, 100), new Point(1, 1), new Point(300, 000), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("GAIN1", playerTemp, new Point(50, 100), new Point(1, 1), new Point(350, 000), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("IDLE2", playerTemp, new Point(50, 100), new Point(1, 1), new Point(000, 100), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("WALK2", playerTemp, new Point(50, 100), new Point(1, 1), new Point(050, 100), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("JUMP2", playerTemp, new Point(50, 100), new Point(1, 1), new Point(100, 100), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("FALL2", playerTemp, new Point(50, 100), new Point(1, 1), new Point(150, 100), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("HURT2", playerTemp, new Point(50, 100), new Point(1, 1), new Point(200, 100), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("ATK-2", playerTemp, new Point(50, 100), new Point(1, 1), new Point(250, 100), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("DIE-2", playerTemp, new Point(50, 100), new Point(1, 1), new Point(300, 100), 16000));
			playerAnimationSet.Add(new Sprite.AnimationSet("GAIN2", playerTemp, new Point(50, 100), new Point(1, 1), new Point(350, 100), 16000));

			movementKeys.Add(Keys.A);
			movementKeys.Add(Keys.W);
			movementKeys.Add(Keys.D);
			movementKeys.Add(Keys.S);
			movementKeys.Add(Keys.Space);
			movementKeys.Add(Keys.E);

			mana = new Player.Mana(0, 0, 0, 0);

			player = new Player(new Vector2(200, 256), movementKeys, 6f, mana, Color.White, playerAnimationSet, myGame);

			base.LoadContent();
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			player.Update(gameTime);

			if (!LevelLoaded)
			{
				SpawnTiles(0);

				LevelLoaded = true;
			}

			// TODO: Add your update code here

			base.Update(gameTime);
		}

		/// <summary>
		/// Draws the game component's content.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Draw(GameTime gameTime)
		{

			spriteBatch.Begin();
			{
				player.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);
			{
				spriteBatch.Draw(line, new Rectangle(0, 0, myGame.WindowSize.X, myGame.WindowSize.X), new Color(0, 0, 0, TransitionAlpha));
			}
			spriteBatch.End();

			spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointWrap, null, null);
			{
				for (int i = 0; i < mapTiles.Count; i++)
				{
					spriteBatch.Draw(line, new Rectangle(mapTiles[i].X, mapTiles[i].Y, mapTiles[i].Width, 1), Color.Red);
					spriteBatch.Draw(line, new Rectangle(mapTiles[i].X, mapTiles[i].Y, 1, 15), new Color(i % 0.999f, i % 0.999f, i % 0.999f));
				}
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

			for (int x = 0; x < Size.X; x++)
			{
				for (int y = 0; y < Size.Y; y++)
				{
					if (tiles[x, y] > 0 && tiles[x, y] < 70)
					{
						mapTiles.Add(new Rectangle(x * 50, y * 50, 50, 50));
					}
					else if (tiles[x, y] == 70)
					{
						player.SetPosition(new Vector2(x * 50, y * 50));
					}
				}
			}
		}
	}
}

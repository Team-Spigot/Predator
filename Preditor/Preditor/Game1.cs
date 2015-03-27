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

namespace Preditor
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		public enum GameLevels
		{
			SPLASH,
			MENU,
			OPTIONS,
			GAME,
			LOSE
		}
		public enum Ratio
		{
			WIDESCREEN,
			NORMAL
		}

		public GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public KeyboardState keyboardState, previousKeyboardState;

		#region Screen Properties
		public Point WindowSize;
		public Point Resolution = new Point(1024, 768);
		public Ratio ratio
		{
			get;
			set;
		}
		public bool Fullscreen
		{
			get
			{
				return graphics.IsFullScreen;
			}
			set
			{
				graphics.IsFullScreen = value;
			}
		}
		#endregion

		#region Game Levels
		public SplashScreenManager splashScreenManager;
		public GameManager gameManager;
		public MenuManager menuManager;
		//public OptionsManager optionsManager;
		public LoseManager loseManager;
		public GameLevels currentGameLevel;
		#endregion

		public float elapsedTime, previousElapsedTime;

		#region Fonts
		public SpriteFont segoeUIBold;
		public SpriteFont segoeUIItalic;
		public SpriteFont segoeUIMonoDebug;
		public SpriteFont segoeUIMono;
		public SpriteFont segoeUIRegular;
		#endregion

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			Window.Title = "Preditor";
			graphics.PreferredBackBufferWidth = (int)Resolution.X;
			graphics.PreferredBackBufferHeight = (int)Resolution.Y;
			WindowSize = new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
			Fullscreen = true;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			SetCurrentLevel(GameLevels.GAME);

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
			{
				this.Exit();
			}

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}

		/// <summary>
		/// This will check if the key specified is released.
		/// </summary>
		/// <param name="key">The key to check</param>
		public bool CheckKey(Keys key)
		{
			if (keyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key))
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// This sets the current scene or level that the game is at.
		/// </summary>
		/// <param name="level">The game level to change to.</param>
		public void SetCurrentLevel(GameLevels level)
		{
			if (currentGameLevel != level)
			{
				currentGameLevel = level;
				splashScreenManager.Enabled = false;
				splashScreenManager.Visible = false;
				menuManager.Enabled = false;
				menuManager.Visible = false;
				gameManager.Enabled = false;
				gameManager.Visible = false;
				//optionsManager.Enabled = false;
				//optionsManager.Visible = false;
				loseManager.Enabled = false;
				loseManager.Visible = false;
			}

			switch (currentGameLevel)
			{
				case GameLevels.SPLASH:
					splashScreenManager.Enabled = true;
					splashScreenManager.Visible = true;
					break;
				case GameLevels.MENU:
					menuManager.Enabled = true;
					menuManager.Visible = true;
					break;
				case GameLevels.OPTIONS:
					//optionsManager.Enabled = true;
					//optionsManager.Visible = true;
					break;
				case GameLevels.GAME:
					gameManager.Enabled = true;
					gameManager.Visible = true;
					break;
				case GameLevels.LOSE:
					loseManager.Enabled = true;
					loseManager.Visible = true;
					break;
				default:
					break;
			}
		}
	}
}

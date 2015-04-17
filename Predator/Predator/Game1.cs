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

namespace Predator
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		/// <summary>
		/// The levels of the game.
		/// </summary>
		public enum GameLevels
		{
			SPLASH,
			MENU,
			OPTIONS,
			GAME,
			MAP,
			STATS,
			LOSE,
			WIN,
			CREDITS
		}

		/// <summary>
		/// The GraphicsDeviceManager for the game.
		/// </summary>
		public GraphicsDeviceManager graphics;
		/// <summary>
		/// The main SpriteBatch of the game.
		/// </summary>
		SpriteBatch spriteBatch;

		/// <summary>
		/// The KeyboardState of the game.
		/// </summary>
		public KeyboardState keyboardState, previousKeyboardState;

		#region Screen Properties
		/// <summary>
		/// Gets or sets the window size.
		/// </summary>
		public Point WindowSize
		{
			get
			{
				return new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
			}
			set
			{
				graphics.PreferredBackBufferWidth = value.X;
				graphics.PreferredBackBufferHeight = value.Y;
			}
		}
		/// <summary>
		/// Gets or sets if the game IsFullScreen.
		/// </summary>
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
		/// <summary>
		/// The SplashScreenManager for the game.
		/// </summary>
		public SplashScreenManager splashScreenManager;
		/// <summary>
		/// The GameManager for the game.
		/// </summary>
		public GameManager gameManager;
		/// <summary>
		/// The MainMenuManager for the game.
		/// </summary>
        public MainMenuManager mainMenuManager;
		/// <summary>
		/// The OptionsManager for the game.
		/// </summary>
		//public OptionsManager optionsManager;
		/// <summary>
		/// The MapScreenManager for the game.
		/// </summary>
		public MapScreenManager mapScreenManager;
		/// <summary>
		/// The StatManager for the game.
		/// </summary>
		public StatManager statManager;
		/// <summary>
		/// The LoseManager for the game.
		/// </summary>
		public LoseManager loseManager;
		/// <summary>
		/// The GameLevels for the game.
		/// </summary>
		public GameLevels currentGameLevel;
		#endregion

		#region Fonts
		public SpriteFont segoeUIBold;
		public SpriteFont segoeUIItalic;
		public SpriteFont segoeUIMonoDebug;
		public SpriteFont segoeUIMono;
		public SpriteFont segoeUIRegular;
		#endregion

		/// <summary>
		/// Creates the game.
		/// </summary>
		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			Window.Title = "Preditor";
			WindowSize = new Point(1024, 768);
			Fullscreen = false;
			IsMouseVisible = true;
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

			segoeUIRegular = Content.Load<SpriteFont>(@"fonts\segoeuiregular");
			segoeUIMono = Content.Load<SpriteFont>(@"fonts\segoeuimono");
			segoeUIMonoDebug = Content.Load<SpriteFont>(@"fonts\segoeuimonodebug");
			segoeUIBold = Content.Load<SpriteFont>(@"fonts\segoeuibold");
			segoeUIItalic = Content.Load<SpriteFont>(@"fonts\segoeuiitalic");

			splashScreenManager = new SplashScreenManager(this);
			mainMenuManager = new MainMenuManager(this);
			gameManager = new GameManager(this);
			mapScreenManager = new MapScreenManager(this);
			statManager = new StatManager(this);
			loseManager = new LoseManager(this);

			Components.Add(splashScreenManager);
			Components.Add(mainMenuManager);
			Components.Add(gameManager);
			Components.Add(mapScreenManager);
			Components.Add(statManager);
			Components.Add(loseManager);

			mapScreenManager.Enabled = false;
			mapScreenManager.Visible = false;

			mainMenuManager.Enabled = false;
			mainMenuManager.Visible = false;

			statManager.Enabled = false;
			mainMenuManager.Visible = false;

			SetCurrentLevel(GameLevels.MENU);
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
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				this.Exit();
			}

			keyboardState = Keyboard.GetState();

			// TODO: Add your update logic here

			base.Update(gameTime);

			previousKeyboardState = keyboardState;
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
				mainMenuManager.Enabled = false;
				mainMenuManager.Visible = false;
				gameManager.Enabled = false;
				gameManager.Visible = false;
				//optionsManager.Enabled = false;
				//optionsManager.Visible = false;
				mapScreenManager.Enabled = false;
				mapScreenManager.Visible = false;
				statManager.Enabled = false;
				statManager.Visible = false;
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
					mainMenuManager.Enabled = true;
					mainMenuManager.Visible = true;
					break;
				case GameLevels.OPTIONS:
					//optionsManager.Enabled = true;
					//optionsManager.Visible = true;
					break;
				case GameLevels.GAME:
					gameManager.Enabled = true;
					gameManager.Visible = true;
					break;
				case GameLevels.MAP:
					gameManager.Visible = true;
					mapScreenManager.Enabled = true;
					mapScreenManager.Visible = true;
					break;
				case GameLevels.STATS:
					gameManager.Visible = true;
					statManager.Enabled = true;
					statManager.Visible = true;
					break;
				case GameLevels.LOSE:
					loseManager.Enabled = true;
					loseManager.Visible = true;
					break;
				case GameLevels.WIN:
					break;
				case GameLevels.CREDITS:
					break;
				default:
					break;
			}
		}
	}
}
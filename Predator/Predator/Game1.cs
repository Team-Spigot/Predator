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
using Predator.Managers;
using VoidEngine.VGUI;

// ====================================== //
// Possible Title: "The Curse of Chimera" //
// ====================================== //

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

		#region States
		/// <summary>
		/// The KeyboardState of the game.
		/// </summary>
		public KeyboardState KeyboardState, PreviousKeyboardState;
		/// <summary>
		/// The MouseState of the game.
		/// </summary>
		public MouseState MouseState, PreviousMouseState;
		#endregion

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
		/// 
		/// </summary>
		public OptionsManager optionsManager;
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
		public SpriteFont grimGhostRegular;
		#endregion

		#region Debug Stuff
		/// <summary>
		/// The value to debug the game.
		/// </summary>
		public bool IsGameDebug
		{
			get;
			set;
		}
		/// <summary>
		/// The label used for debuging.
		/// </summary>
		public Label debugLabel;
		/// <summary>
		/// The list of strings that are used for debuging.
		/// </summary>
		public string[] debugStrings = new string[25];
		#endregion

		#region Options Stuff
		/// <summary>
		/// Gets or sets if VSync is enabled.
		/// </summary>
		public bool VSync
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the game should apply the current settings.
		/// </summary>
		public bool ApplySettings
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the game should cancel the current settings.
		/// </summary>
		public bool CancelSettings
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the game finished applying/canceling the current settings.
		/// </summary>
		public bool FinishedSettings
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the amount of times the options changed.
		/// Used to check if they have against [OldOptionsChanged]
		/// to update the positions and other things.
		/// </summary>
		public int OptionsChanged
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the old amount of times the options changed.
		/// Used to check if they have against [OldOptionsChanged]
		/// to update the positions and other things.
		/// </summary>
		public int OldOptionsChanged
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the temporary window size.
		/// </summary>
		public Point TempWindowSize
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// Creates the game.
		/// </summary>
		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			Window.Title = "Predator";
			WindowSize = new Point(1024, 768);
			Fullscreen = false;
			IsMouseVisible = true;
			IsGameDebug = true;
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

			LoadTextures();

			splashScreenManager = new SplashScreenManager(this);
			mainMenuManager = new MainMenuManager(this);
			gameManager = new GameManager(this);
			mapScreenManager = new MapScreenManager(this);
			statManager = new StatManager(this);
			loseManager = new LoseManager(this);
			optionsManager = new OptionsManager(this);

			Components.Add(splashScreenManager);
			Components.Add(mainMenuManager);
			Components.Add(gameManager);
			Components.Add(mapScreenManager);
			Components.Add(statManager);
			Components.Add(loseManager);
			Components.Add(optionsManager);

			mapScreenManager.Enabled = false;
			mapScreenManager.Visible = false;

			gameManager.Enabled = false;
			gameManager.Visible = false;

			mainMenuManager.Enabled = false;
			mainMenuManager.Visible = false;

			statManager.Enabled = false;
			statManager.Visible = false;

			optionsManager.Enabled = false;
			optionsManager.Visible = false;

			debugLabel = new Label(new Vector2(0, 60), segoeUIMonoDebug, 1f, Color.White, "");

			TempWindowSize = WindowSize;
			ApplySettings = true;

			SetCurrentLevel(GameLevels.MENU);
		}

		/// <summary>
		/// Loads all the textures for the game.
		/// </summary>
		public void LoadTextures()
		{
			segoeUIRegular = Content.Load<SpriteFont>(@"fonts\segoeuiregular");
			segoeUIMono = Content.Load<SpriteFont>(@"fonts\segoeuimono");
			segoeUIMonoDebug = Content.Load<SpriteFont>(@"fonts\segoeuimonodebug");
			segoeUIBold = Content.Load<SpriteFont>(@"fonts\segoeuibold");
			segoeUIItalic = Content.Load<SpriteFont>(@"fonts\segoeuiitalic");
			grimGhostRegular = Content.Load<SpriteFont>(@"fonts\grimghostregular");
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
			KeyboardState = Keyboard.GetState();
			MouseState = Mouse.GetState();

			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || (KeyboardState.IsKeyDown(Keys.Escape) && KeyboardState.IsKeyDown(Keys.LeftShift)))
			{
				this.Exit();
			}

			if (CheckKey(Keys.OemPlus) && currentGameLevel != GameLevels.MENU)
			{
				SetCurrentLevel(GameLevels.MENU);
			}

			if (ApplySettings)
			{
				graphics.SynchronizeWithVerticalRetrace = VSync;

				if (new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight) != TempWindowSize)
				{
					graphics.PreferredBackBufferWidth = TempWindowSize.X;
					graphics.PreferredBackBufferHeight = TempWindowSize.Y;

					WindowSize = new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
				}

				graphics.ApplyChanges();
				ApplySettings = false;
				FinishedSettings = true;

				OldOptionsChanged = OptionsChanged;
				OptionsChanged += 1;
			}

			if (CancelSettings)
			{
				VSync = graphics.SynchronizeWithVerticalRetrace;

				WindowSize = new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

				CancelSettings = false;
				FinishedSettings = true;
			}

			base.Update(gameTime);

			#region Debug Stuff
			if (IsGameDebug)
			{
				debugLabel.Text = debugStrings[00] + "\n" +
								  debugStrings[01] + "\n" +
								  debugStrings[02] + "\n" +
								  debugStrings[03] + "\n" +
								  debugStrings[04] + "\n" +
								  debugStrings[05] + "\n" +
								  debugStrings[06] + "\n" +
								  debugStrings[07] + "\n" +
								  debugStrings[08] + "\n" +
								  debugStrings[09] + "\n" +
								  debugStrings[10] + "\n" +
								  debugStrings[11] + "\n" +
								  debugStrings[12] + "\n" +
								  debugStrings[13] + "\n" +
								  debugStrings[14] + "\n" +
								  debugStrings[15] + "\n" +
								  debugStrings[16] + "\n" +
								  debugStrings[17] + "\n" +
								  debugStrings[18] + "\n" +
								  debugStrings[19] + "\n" +
								  debugStrings[20] + "\n" +
								  debugStrings[21] + "\n" +
								  debugStrings[22] + "\n" +
								  debugStrings[23] + "\n" +
								  debugStrings[24];
			}
			#endregion

			PreviousKeyboardState = KeyboardState;
			PreviousMouseState = MouseState;
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
			if (KeyboardState.IsKeyUp(key) && PreviousKeyboardState.IsKeyDown(key))
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
				optionsManager.Enabled = false;
				optionsManager.Visible = false;
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
					optionsManager.Enabled = true;
					optionsManager.Visible = true;
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
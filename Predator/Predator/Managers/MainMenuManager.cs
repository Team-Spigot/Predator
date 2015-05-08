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


namespace Predator.Managers
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class MainMenuManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		/// <summary>
		/// The game that the main menu runs off of.
		/// </summary>
		Game1 myGame;
		/// <summary>
		/// The sprite batch that the main menu uses.
		/// </summary>
		SpriteBatch spriteBatch;

		/// <summary>
		/// The main menu Background.
		/// </summary>
		Texture2D menuBackground;

		/// <summary>
		/// The start button texture.
		/// <summary>
		Texture2D startButtonTexture;
		/// <summary>
		/// The quit button texture.
		/// </summary>
		Texture2D quitButtonTexture;

		/// <summary>
		/// The start button.
		/// </summary>
		Button startButton;
		/// <summary>
		/// The quit button.
		/// </summary>
		Button quitButton;

		Checkbox checkBoxTest;
		Texture2D checkBoxTexture;

		/// <summary>
		/// Creates the game manager.
		/// </summary>
		public MainMenuManager(Game1 game)
			: base(game)
		{
			myGame = game;
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
		/// Loads the main menu's content.
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(myGame.GraphicsDevice);

			LoadTextures();

			startButton = new Button(startButtonTexture, new Vector2((myGame.WindowSize.X - 100) / 2, 300), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White);
			quitButton = new Button(quitButtonTexture, new Vector2((myGame.WindowSize.X - 100) / 2, 550), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White);
			checkBoxTest = new Checkbox(checkBoxTexture, new Vector2(50, 50), Checkbox.ButtonTypes.Left, new Checkbox.SetButtonPositions(0, 1, 2, 3), Color.White);

			base.LoadContent();
		}

		public void LoadTextures()
		{
			menuBackground = Game.Content.Load<Texture2D>(@"images\gui\mainMenu\mainMenu");
			startButtonTexture = Game.Content.Load<Texture2D>(@"images\gui\mainMenu\startButton");
			quitButtonTexture = Game.Content.Load<Texture2D>(@"images\gui\mainMenu\quitButton");
			checkBoxTexture = Game.Content.Load<Texture2D>(@"images\gui\global\checkBox");
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			startButton.Update(gameTime);
			quitButton.Update(gameTime);

			if (startButton.Clicked())
			{
				myGame.SetCurrentLevel(Game1.GameLevels.GAME);
			}

			if (quitButton.Clicked())
			{
				Game.Exit();
			}

			checkBoxTest.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			{
				spriteBatch.Draw(menuBackground, Vector2.Zero, Color.White);
				startButton.Draw(gameTime, spriteBatch);
				quitButton.Draw(gameTime, spriteBatch);
				checkBoxTest.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
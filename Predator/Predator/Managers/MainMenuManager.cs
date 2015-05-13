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
		Texture2D menuBackgroundTexture;
		/// <summary>
		/// 
		/// </summary>
		Texture2D titleTexture;
		/// <summary>
		/// 
		/// </summary>
		Texture2D buttonTexture;

		/// <summary>
		/// The start button.
		/// </summary>
		Button startButton;
		/// <summary>
		/// The quit button.
		/// </summary>
		Button quitButton;
		/// <summary>
		/// 
		/// </summary>
		Button optionsButton;

		float TitleScale = 1.5f;
		float TitleMaxSca = 1.7f;
		float TitleMinSca = 1.3f;
		float TitleScaMov = -1;

		float TitleRot = 0;
		float TitleMaxRot = -10 * (float)Math.PI / 180;
		float TitleMinRot = 10 * (float)Math.PI / 180;
		float TitleMovement = -1;

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

			startButton   = new Button(buttonTexture, new Vector2((myGame.WindowSize.X - 121) / 2, 300), myGame.segoeUIRegular, 1.0f, new Color(155, 155, 155), "PLAY",    Color.White);
			optionsButton = new Button(buttonTexture, new Vector2((myGame.WindowSize.X - 121) / 2, 420), myGame.segoeUIRegular, 1.0f, new Color(155, 155, 155), "OPTIONS", Color.White);
			quitButton    = new Button(buttonTexture, new Vector2((myGame.WindowSize.X - 121) / 2, 550), myGame.segoeUIRegular, 1.0f, new Color(155, 155, 155), "QUIT",    Color.White);

			base.LoadContent();
		}

		public void LoadTextures()
		{
			menuBackgroundTexture = Game.Content.Load<Texture2D>(@"images\game\backgrounds\sewerBackgroundG");
			buttonTexture = Game.Content.Load<Texture2D>(@"images\gui\global\button1");
			titleTexture = Game.Content.Load<Texture2D>(@"images\gui\mainMenu\title");
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			startButton.Update(gameTime);
			quitButton.Update(gameTime);
			optionsButton.Update(gameTime);

			if (startButton.Clicked())
			{
				myGame.SetCurrentLevel(Game1.GameLevels.GAME);
			}

			if (quitButton.Clicked())
			{
				Game.Exit();
			}

			if (optionsButton.Clicked())
			{
				myGame.SetCurrentLevel(Game1.GameLevels.OPTIONS);
			}

			if (TitleScaMov == -1)
			{
				TitleScale -= 0.005f;
			}
			if (TitleScaMov == 1)
			{
				TitleScale += 0.005f;
			}
			if (TitleScale <= TitleMinSca)
			{
				TitleScaMov = 1;
			}
			if (TitleScale >= TitleMaxSca)
			{
				TitleScaMov = -1;
			}

			if (TitleMovement == -1)
			{
				TitleRot += 0.2f * (float)Math.PI / 180;
			}
			if (TitleMovement == 1)
			{
				TitleRot -= 0.2f * (float)Math.PI / 180;
			}
			if (TitleRot >= TitleMinRot)
			{
				TitleMovement = 1;
			}
			if (TitleRot <= TitleMaxRot)
			{
				TitleMovement = -1;
			}

			if (myGame.OptionsChanged > myGame.OldOptionsChanged)
			{
				startButton.Position = new Vector2((myGame.WindowSize.X - 121) / 2, 300);
				quitButton.Position = new Vector2((myGame.WindowSize.X - 121) / 2, 550);
				optionsButton.Position = new Vector2((myGame.WindowSize.X - 121) / 2, 420);
			}
		}

		public override void Draw(GameTime gameTime)
		{

			spriteBatch.Begin();
			{
				spriteBatch.Draw(menuBackgroundTexture, new Rectangle(0, 0, myGame.WindowSize.X, myGame.WindowSize.Y), Color.White);
				spriteBatch.Draw(titleTexture, new Vector2(myGame.WindowSize.X / 2, 100), new Rectangle(0, 0, titleTexture.Width, titleTexture.Height), Color.White, TitleRot, new Vector2(titleTexture.Width / 2, titleTexture.Height / 2), TitleScale, SpriteEffects.None, 0f);
				startButton.Draw(gameTime, spriteBatch);
				quitButton.Draw(gameTime, spriteBatch);
				optionsButton.Draw(gameTime, spriteBatch);
				spriteBatch.DrawString(myGame.segoeUIMonoDebug, TitleRot.ToString() + " Max=" + TitleMaxRot + " Min=" + TitleMinRot + " dir=" + TitleMovement, Vector2.Zero, Color.Lime);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
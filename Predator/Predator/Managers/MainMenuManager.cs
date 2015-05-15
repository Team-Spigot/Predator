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
		/// The game that the main menu manager runs off of.
		/// </summary>
		private Game1 myGame;
		/// <summary>
		/// The sprite batch that the main menu manager uses.
		/// </summary>
		private SpriteBatch spriteBatch;

		#region Textures
		/// <summary>
		/// Loads the texture for the menu.
		/// </summary>
		public Texture2D MenuBackgroundTexture;
		/// <summary>
		/// Loads the texture for the title.
		/// </summary>
		public Texture2D TitleTexture;
		/// <summary>
		/// Loads the texture for the button.
		/// </summary>
		public Texture2D ButtonTexture;
		#endregion

		#region Buttons
		/// <summary>
		/// The start button.
		/// </summary>
		public Button StartButton;
		/// <summary>
		/// The quit button.
		/// </summary>
		public Button QuitButton;
		/// <summary>
		/// The options button.
		/// </summary>
		public Button OptionsButton;
		#endregion

		#region Title Effect Variables
		/// <summary>
		/// The default title scale.
		/// </summary>
		protected float TitleScale = 1.5f;
		/// <summary>
		/// The title's max scale.
		/// </summary>
		protected float TitleMaxScale = 1.7f;
		/// <summary>
		/// The title's min scale.
		/// </summary>
		protected float TitleMinScale = 1.3f;
		/// <summary>
		/// The direction to move when scaling.
		/// </summary>
		protected float TitleScaleMovement = -1;

		/// <summary>
		/// The default title rotation.
		/// </summary>
		protected float TitleRotation = 0.0f;
		/// <summary>
		/// The title's max rotation.
		/// </summary>
		protected float TitleMaxRotation = -10.0f * (float)Math.PI / 180;
		/// <summary>
		/// The title's min rotation.
		/// </summary>
		protected float TitleMinRotation = 10.0f * (float)Math.PI / 180;
		/// <summary>
		/// The direction to move when rotating.
		/// </summary>
		protected float TitleRotationMovement = -1;
		#endregion

		/// <summary>
		/// Creates the main menu manager.
		/// </summary>
		/// <param name="game">The game that the main menu manager runs off of.</param>
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
		/// Loads the content of the main menu manager.
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(myGame.GraphicsDevice);

			LoadTextures();

			StartButton = new Button(ButtonTexture, new Vector2((myGame.WindowSize.X - 121) / 2, 300), myGame.segoeUIRegular, 1.0f, new Color(155, 155, 155), "PLAY", Color.White);
			OptionsButton = new Button(ButtonTexture, new Vector2((myGame.WindowSize.X - 121) / 2, 420), myGame.segoeUIRegular, 1.0f, new Color(155, 155, 155), "OPTIONS", Color.White);
			QuitButton = new Button(ButtonTexture, new Vector2((myGame.WindowSize.X - 121) / 2, 550), myGame.segoeUIRegular, 1.0f, new Color(155, 155, 155), "QUIT", Color.White);

			base.LoadContent();
		}

		/// <summary>
		/// Loads the textures of the main menu manager.
		/// </summary>
		public void LoadTextures()
		{
			MenuBackgroundTexture = Game.Content.Load<Texture2D>(@"images\game\backgrounds\sewerBackgroundG");
			ButtonTexture = Game.Content.Load<Texture2D>(@"images\gui\global\button1");
			TitleTexture = Game.Content.Load<Texture2D>(@"images\gui\mainMenu\title");
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			StartButton.Update(gameTime);
			QuitButton.Update(gameTime);
			OptionsButton.Update(gameTime);

			if (StartButton.Clicked())
			{
				myGame.SetCurrentLevel(Game1.GameLevels.GAME);
			}

			if (QuitButton.Clicked())
			{
				Game.Exit();
			}

			if (OptionsButton.Clicked())
			{
				myGame.SetCurrentLevel(Game1.GameLevels.OPTIONS);
			}

			#region Title Effects
			#region Title Scaling
			if (TitleScaleMovement == -1)
			{
				TitleScale -= 0.005f;
			}
			if (TitleScaleMovement == 1)
			{
				TitleScale += 0.005f;
			}
			if (TitleScale <= TitleMinScale)
			{
				TitleScaleMovement = 1;
			}
			if (TitleScale >= TitleMaxScale)
			{
				TitleScaleMovement = -1;
			}
			#endregion

			#region Title Rotation
			if (TitleRotationMovement == -1)
			{
				TitleRotation += 0.2f * (float)Math.PI / 180;
			}
			if (TitleRotationMovement == 1)
			{
				TitleRotation -= 0.2f * (float)Math.PI / 180;
			}
			if (TitleRotation >= TitleMinRotation)
			{
				TitleRotationMovement = 1;
			}
			if (TitleRotation <= TitleMaxRotation)
			{
				TitleRotationMovement = -1;
			}
			#endregion
			#endregion

			if (myGame.OptionsChanged > myGame.OldOptionsChanged)
			{
				StartButton.Position = new Vector2((myGame.WindowSize.X - 121) / 2, 300);
				QuitButton.Position = new Vector2((myGame.WindowSize.X - 121) / 2, 550);
				OptionsButton.Position = new Vector2((myGame.WindowSize.X - 121) / 2, 420);
			}
		}

		/// <summary>
		/// Draws the content of the main menu manager.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			{
				spriteBatch.Draw(MenuBackgroundTexture, new Rectangle(0, 0, myGame.WindowSize.X, myGame.WindowSize.Y), Color.White);
				spriteBatch.Draw(TitleTexture, new Vector2(myGame.WindowSize.X / 2, 100), new Rectangle(0, 0, TitleTexture.Width, TitleTexture.Height), Color.White, TitleRotation, new Vector2(TitleTexture.Width / 2, TitleTexture.Height / 2), TitleScale, SpriteEffects.None, 0f);
				StartButton.Draw(gameTime, spriteBatch);
				QuitButton.Draw(gameTime, spriteBatch);
				OptionsButton.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
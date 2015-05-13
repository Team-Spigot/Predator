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

namespace Predator.Managers
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class OptionsManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		Game1 myGame;
		SpriteBatch spriteBatch;

		Texture2D BackgroundTexture;
		Texture2D Button1Texture;
		Texture2D Button2Texture;
		Texture2D CheckboxTexture;
		Texture2D ArrowButton1Texture;
		Texture2D ArrowButton2Texture;

		Button ApplyButton;
		Button CancelButton;
		Button VSyncButton;
		Checkbox VSyncCheckbox;
		Button WindowSizeButton;
		Button DecWindowSizeButton;
		Button IncWindowSizeButton;

		string WindowSizeString;
		Point[] WindowSizes = new Point[2];
		int WindowIndex = 1;

		public OptionsManager(Game1 game)
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
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(myGame.GraphicsDevice);

			LoadTextures();

			ApplyButton = new Button(Button1Texture, new Vector2((myGame.WindowSize.X - (Button1Texture.Width / 3)) / 2 - 150, myGame.WindowSize.Y - 50), myGame.segoeUIBold, 1.0f, Color.White, "Apply", Color.White);
			CancelButton = new Button(Button1Texture, new Vector2((myGame.WindowSize.X - (Button1Texture.Width / 3)) / 2 + 150, myGame.WindowSize.Y - 50), myGame.segoeUIBold, 1.0f, Color.White, "Cancel", Color.White);

			VSyncButton = new Button(Button1Texture, new Vector2((myGame.WindowSize.X - (Button1Texture.Width / 3)) / 2 - 250, 450), myGame.segoeUIBold, 1.0f, Color.White, "VSync", Color.White);
			VSyncCheckbox = new Checkbox(CheckboxTexture, new Vector2(VSyncButton.Position.X + (Button1Texture.Width / 3) + 10, VSyncButton.Position.Y - ((CheckboxTexture.Height - Button1Texture.Height) / 2)), Checkbox.ButtonTypes.Left, Color.White);
			
			WindowSizeButton = new Button(Button2Texture, new Vector2((myGame.WindowSize.X - (Button1Texture.Width / 3)) / 2 + 100, 450), myGame.segoeUIBold, 1.0f, Color.White, "Window Size " + myGame.WindowSize.X + "x" + myGame.WindowSize.Y, Color.White);
			DecWindowSizeButton = new Button(ArrowButton1Texture, new Vector2(WindowSizeButton.Position.X - 33, 450), myGame.segoeUIRegular, 1.0f, Color.White, "", Color.White);
			IncWindowSizeButton = new Button(ArrowButton2Texture, new Vector2(WindowSizeButton.Position.X + (Button2Texture.Width / 3), 450), myGame.segoeUIRegular, 1.0f, Color.White, "", Color.White);

			WindowSizes[0] = new Point(768, 576);
			WindowSizes[1] = new Point(1024, 768);

			base.LoadContent();
		}

		public void LoadTextures()
		{
			BackgroundTexture = Game.Content.Load<Texture2D>(@"images\game\backgrounds\sewerBackgroundG");
			Button1Texture = Game.Content.Load<Texture2D>(@"images\gui\global\button1");
			Button2Texture = Game.Content.Load<Texture2D>(@"images\gui\global\button2");
			CheckboxTexture = Game.Content.Load<Texture2D>(@"images\gui\global\checkbox1");
			ArrowButton1Texture = Game.Content.Load<Texture2D>(@"images\gui\global\arrowButton1");
			ArrowButton2Texture = Game.Content.Load<Texture2D>(@"images\gui\global\arrowButton2");
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			ApplyButton.Update(gameTime);
			CancelButton.Update(gameTime);

			VSyncCheckbox.Update(gameTime);

			DecWindowSizeButton.Update(gameTime);
			IncWindowSizeButton.Update(gameTime);

			if (VSyncCheckbox.IsChecked)
			{
				myGame.VSync = true;
			}

			if (ApplyButton.Clicked())
			{
				myGame.ApplySettings = true;
			}
			if (CancelButton.Clicked())
			{
				myGame.CancelSettings = true;
			}

			if (myGame.FinishedSettings == true)
			{
				myGame.FinishedSettings = false;
				myGame.SetCurrentLevel(Game1.GameLevels.MENU);
			}

			WindowSizeButton.Update(gameTime);
			VSyncButton.Update(gameTime);

			if (DecWindowSizeButton.Clicked())
			{
				WindowIndex -= 1;

				if (WindowIndex < 0)
				{
					WindowIndex = 0;
				}

				myGame.TempWindowSize = WindowSizes[WindowIndex];
				WindowSizeString = "Window Size " + WindowSizes[WindowIndex].X + "x" + WindowSizes[WindowIndex].Y;
				WindowSizeButton.Text = WindowSizeString;
			}

			if (IncWindowSizeButton.Clicked())
			{
				WindowIndex += 1;

				if (WindowIndex > WindowSizes.Length - 1)
				{
					WindowIndex = WindowSizes.Length - 1;
				}

				WindowSizeString = "Window Size " + WindowSizes[WindowIndex].X + "x" + WindowSizes[WindowIndex].Y;

				myGame.TempWindowSize = WindowSizes[WindowIndex];
				WindowSizeButton.Text = WindowSizeString;
			}

			if (myGame.OptionsChanged > myGame.OldOptionsChanged)
			{
				ApplyButton.Position = new Vector2((myGame.WindowSize.X - (Button1Texture.Width / 3)) / 2 - 150, myGame.WindowSize.Y - 50);
				CancelButton.Position = new Vector2((myGame.WindowSize.X - (Button1Texture.Width / 3)) / 2 + 150, myGame.WindowSize.Y - 50);

				VSyncButton.Position = new Vector2((myGame.WindowSize.X - (Button1Texture.Width / 3)) / 2 - 250, 450);
				VSyncCheckbox.Position = new Vector2(VSyncButton.Position.X + (Button1Texture.Width / 3) + 10, VSyncButton.Position.Y - ((CheckboxTexture.Height - Button1Texture.Height) / 2));

				WindowSizeButton.Position = new Vector2((myGame.WindowSize.X - (Button1Texture.Width / 3)) / 2 + 100, 450);
				DecWindowSizeButton.Position = new Vector2(WindowSizeButton.Position.X - 33, 450);
				IncWindowSizeButton.Position = new Vector2(WindowSizeButton.Position.X + (Button2Texture.Width / 3), 450);
			}

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			{
				spriteBatch.Draw(BackgroundTexture, new Rectangle(0, 0, myGame.WindowSize.X, myGame.WindowSize.Y), Color.White);

				ApplyButton.Draw(gameTime, spriteBatch);
				CancelButton.Draw(gameTime, spriteBatch);

				VSyncCheckbox.Draw(gameTime, spriteBatch);
				VSyncButton.Draw(gameTime, spriteBatch);

				WindowSizeButton.Draw(gameTime, spriteBatch);
				DecWindowSizeButton.Draw(gameTime, spriteBatch);
				IncWindowSizeButton.Draw(gameTime, spriteBatch);

				spriteBatch.DrawString(myGame.segoeUIMonoDebug, "Index=" + WindowIndex, new Vector2(0, 0), Color.White);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}

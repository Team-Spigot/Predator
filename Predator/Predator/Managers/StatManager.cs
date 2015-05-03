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
using VoidEngine.VGame;
using VoidEngine.VGUI;
using Predator.Characters;
using Predator.Managers;


namespace Predator
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class StatManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		Game1 myGame;
		SpriteBatch spriteBatch;



		public int levelSelect = 0; //when zoomed in the hud is always the same(same start button) so when you first click on the level it will set this value so when you click on the start it will load the level using a switch
		// level 1 = 1, level 2 = 2... etc.

		Texture2D statMenuTexture;
		Texture2D plusButtonTexture;
		Texture2D exitButtonTexture;
		Texture2D backButtonTexture;

		Button plusButtonA;
		Button plusButtonS;
		Button plusButtonD;
		Button exitButton;
		Button backButton;


		public StatManager(Game1 game)
			: base(game)
		{
			myGame = game;
			spriteBatch = new SpriteBatch(myGame.GraphicsDevice);
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
			plusButtonTexture = Game.Content.Load<Texture2D>(@"images\gui\statsMenu\plusButton");
			statMenuTexture   = Game.Content.Load<Texture2D>(@"images\gui\statsMenu\menu");
			exitButtonTexture = Game.Content.Load<Texture2D>(@"images\gui\statsMenu\exitButton");
			backButtonTexture = Game.Content.Load<Texture2D>(@"images\gui\statsMenu\backButton");

			plusButtonS = new Button(plusButtonTexture, new Vector2(((myGame.WindowSize.X - statMenuTexture.Width) / 2) + 150, ((myGame.WindowSize.X - statMenuTexture.Height) / 2) + 075), myGame.segoeUIRegular, 1f, Color.Black, "",     Color.White);
			plusButtonA = new Button(plusButtonTexture, new Vector2(((myGame.WindowSize.X - statMenuTexture.Width) / 2) + 150, ((myGame.WindowSize.X - statMenuTexture.Height) / 2) + 150), myGame.segoeUIRegular, 1f, Color.Black, "",     Color.White);
			plusButtonD = new Button(plusButtonTexture, new Vector2(((myGame.WindowSize.X - statMenuTexture.Width) / 2) + 150, ((myGame.WindowSize.X - statMenuTexture.Height) / 2) + 225), myGame.segoeUIRegular, 1f, Color.Black, "",     Color.White);
			exitButton  = new Button(exitButtonTexture, new Vector2(((myGame.WindowSize.X - statMenuTexture.Width) / 2) + 000, ((myGame.WindowSize.X - statMenuTexture.Height) / 2) - 000), myGame.segoeUIRegular, 1f, Color.White, "Back", Color.White);
			backButton  = new Button(backButtonTexture, new Vector2(((myGame.WindowSize.X - statMenuTexture.Width) / 2) + 000, ((myGame.WindowSize.X - statMenuTexture.Height) / 2) - 000), myGame.segoeUIRegular, 1f, Color.White, "",     Color.White);

			base.LoadContent();
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			plusButtonA.Update(gameTime);
			plusButtonS.Update(gameTime);
			plusButtonD.Update(gameTime);

			if (plusButtonA.Clicked() && myGame.gameManager.Player.statPoints >= 1)
			{
				myGame.gameManager.Player.statPoints -= 1;
				myGame.gameManager.Player.PAgility -= 0.08f;

			}
			if (plusButtonS.Clicked() && myGame.gameManager.Player.statPoints >= 1)
			{
				myGame.gameManager.Player.statPoints -= 1;
				myGame.gameManager.Player.PStrength += 0.18f;

			}
			if (plusButtonD.Clicked() && myGame.gameManager.Player.statPoints >= 1)
			{
				myGame.gameManager.Player.statPoints -= 1;
				myGame.gameManager.Player.PDefense += 0.18f;

			}

			if (Keyboard.GetState().IsKeyDown(Keys.T))
			{
				myGame.SetCurrentLevel(Game1.GameLevels.GAME);
			}
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin();
			{
				spriteBatch.Draw(statMenuTexture, new Vector2((myGame.WindowSize.X - statMenuTexture.Width) / 2, (myGame.WindowSize.Y - statMenuTexture.Height) / 2), Color.White);
				plusButtonA.Draw(gameTime, spriteBatch);
				plusButtonS.Draw(gameTime, spriteBatch);
				plusButtonD.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}

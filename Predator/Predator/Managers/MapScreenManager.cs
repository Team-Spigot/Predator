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
using VoidEngine.Helpers;

namespace Predator
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class MapScreenManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		Game1 myGame;
		SpriteBatch spriteBatch;

		Camera camera;

		/// <summary>
		/// Loads the line texture.
		/// </summary>
		public Texture2D line;
		Texture2D mapBackground;
		Texture2D mapHudTexture;
		Texture2D buttonTexture;
		Texture2D backButtonTexture;
		Texture2D hudBackButtonTexture;

		float Speed = 10;
		int HudScroll = 1024;
		public int levelSelect = 0; //when zoomed in the hud is always the same(same start button) so when you first click on the level it will set this value so when you click on the start it will load the level using a switch
		// level 1 = 1, level 2 = 2... etc.

		Vector2 Direction;

		public bool levelPullUp = false; //becomes true when you select a level pulling up level info hud
		public bool isZoomingIn = false;
		public bool isZoomingOut = false;
		public bool isHudScrollingIn = false;
		public bool isHudScrollingOut = false;

		List<Sprite.AnimationSet> buttonAnimationSet;

		Button level1Button;
		Button level2Button;
		//Button level3Button;
		Button backButton;
		Button hudBackButton;

		/// <summary>
		/// Gets or sets the tranition alpha variable.
		/// </summary>
		public float TransitionAlpha
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the level is transitioning into the map screen.
		/// </summary>
		public bool isTransitioningIn
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the level is transitioning out of the map screen.
		/// </summary>
		public bool isTransitioningOut
		{
			get;
			set;
		}
		public bool SetAlpha
		{
			get;
			set;
		}

		Label debugLabel;
		string[] debugStrings = new string[10];

		public MapScreenManager(Game1 game)
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
			buttonAnimationSet = new List<Sprite.AnimationSet>();

			// TODO: Add your initialization code here

			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(myGame.GraphicsDevice);

			line = Game.Content.Load<Texture2D>(@"images\other\line");
			mapBackground = Game.Content.Load<Texture2D>(@"images\gui\map\mapScreen");
			buttonTexture = Game.Content.Load<Texture2D>(@"images\gui\map\mapButtonTest");
			hudBackButtonTexture = Game.Content.Load<Texture2D>(@"images\gui\map\mapButtonTest");
			backButtonTexture = Game.Content.Load<Texture2D>(@"images\gui\map\mapButtonTest");

			mapHudTexture = Game.Content.Load<Texture2D>(@"images\gui\map\mapHud");

			camera = new Camera(myGame.GraphicsDevice.Viewport, new Point(2048, 1536), 1f);
			camera.Position = new Vector2(512, 256);

			// The Map Buttons
			buttonAnimationSet.Add(new Sprite.AnimationSet("IDLE", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(0, 0), 16000, false));
			buttonAnimationSet.Add(new Sprite.AnimationSet("HOVER", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(20, 0), 16000, false));
			buttonAnimationSet.Add(new Sprite.AnimationSet("PRESSED", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(40, 0), 16000, false));
			// Hud Back Button
			buttonAnimationSet.Add(new Sprite.AnimationSet("IDLE", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(0, 0), 16000, false));
			buttonAnimationSet.Add(new Sprite.AnimationSet("HOVER", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(20, 0), 16000, false));
			buttonAnimationSet.Add(new Sprite.AnimationSet("PRESSED", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(40, 0), 16000, false));
			// Back Button
			buttonAnimationSet.Add(new Sprite.AnimationSet("IDLE", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(0, 0), 16000, false));
			buttonAnimationSet.Add(new Sprite.AnimationSet("HOVER", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(20, 0), 16000, false));
			buttonAnimationSet.Add(new Sprite.AnimationSet("PRESSED", buttonTexture, new Point(20, 20), new Point(1, 1), new Point(40, 0), 16000, false));

			level1Button = new Button(new Vector2(700, 400), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, buttonAnimationSet);
			level2Button = new Button(new Vector2(100, 120), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, buttonAnimationSet);

			backButton = new Button(new Vector2(50, 700), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, buttonAnimationSet);

			hudBackButton = new Button(new Vector2(100, 700), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, buttonAnimationSet);

			debugLabel = new Label(new Vector2(350, 15), myGame.segoeUIMonoDebug, 1.0f, Color.White, "");

			base.LoadContent();
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			// TODO: Add your update code here
			if (isTransitioningIn)
			{
				isTransitioningOut = false;

				TransitionAlpha += 1.0f / (float)gameTime.ElapsedGameTime.Milliseconds;

				if (TransitionAlpha > 1.0f)
				{
					isTransitioningIn = false;
					TransitionAlpha = 1.0f;
				}
			}

			if (isTransitioningOut)
			{
				isTransitioningIn = false;

				TransitionAlpha -= 1.0f / (float)gameTime.ElapsedGameTime.Milliseconds;

				if (TransitionAlpha <= 0.0f)
				{
					isTransitioningOut = false;
					TransitionAlpha = 0.0f;

					myGame.SetCurrentLevel(Game1.GameLevels.GAME);
				}
			}

			if (isZoomingIn == true)
			{
				camera.Zoom += 0.01f;
				camera.Position += Direction * Speed;

			}
			if (isZoomingOut == true)
			{
				camera.Zoom -= 0.01f;
				camera.Position -= Direction * Speed;

			}
			if (camera.Zoom <= 1.0f)
			{
				camera.Zoom = 1.0f;
				camera.Position = new Vector2(0, 0);
				isZoomingOut = false;
			}

			if (isHudScrollingIn)
			{
				HudScroll -= 10;
				if (HudScroll <= 527)
				{
					HudScroll = 527;
					isHudScrollingIn = false;
				}
			}
			if (isHudScrollingOut)
			{
				HudScroll += 10;
				if (HudScroll >= 1024)
				{
					HudScroll = 1024;
					isHudScrollingOut = false;
				}
			}
			switch (levelSelect)
			{
				case 1:
					if (camera.Position.X - level1Button.GetPosition.X >= 100 && camera.Position.Y - level1Button.GetPosition.Y <= 100)
					{
						Speed = 0.0f;
					}
					break;
				case 2:
					if (camera.Position.X - level2Button.GetPosition.X <= 100 && camera.Position.Y - level1Button.GetPosition.Y <= 100)
					{
						Speed = 0.0f;
					}
					break;
				/*
				case 3:
					if (camera.Position.X - level3Button.GetPosition.X >= 150 && camera.Position.Y - level1Button.GetPosition.Y <= 50)
					{
						Speed = 0.0f;
					}
					break;
				*/
			}
			if (levelPullUp == false)
			{
				Vector2 tempDirection;

				level1Button.Update(gameTime);
				level2Button.Update(gameTime);

				backButton.Update(gameTime);
				if (level1Button.Clicked()) // Zooming into zone 1
				{
					Speed = 10.0f;
					levelSelect = 1;
					isHudScrollingIn = true;
					levelPullUp = true;
					isZoomingIn = true;
					isZoomingOut = false;
					tempDirection = new Vector2(level1Button.GetPosition.X - camera.Position.X, level1Button.GetPosition.Y - camera.Position.Y);
					Direction = CollisionHelper.UnitVector(tempDirection);
				}
				if (level2Button.Clicked()) // Zooming into zone 2
				{
					Speed = 10.0f;
					levelSelect = 2;
					isHudScrollingIn = true;
					levelPullUp = true;
					isZoomingIn = true;
					isZoomingOut = false;
					tempDirection = new Vector2(level2Button.GetPosition.X - camera.Position.X, level2Button.GetPosition.Y - camera.Position.Y);
					Direction = CollisionHelper.UnitVector(tempDirection);
				}

				if (backButton.Clicked())
				{
					isTransitioningOut = true;
				}
			}
			if (levelPullUp == true)
			{
				Vector2 tempDirection;

				hudBackButton.Update(gameTime);

				if (hudBackButton.Clicked())
				{
					levelPullUp = false;
					isZoomingOut = true;
					isZoomingIn = false;
					isHudScrollingOut = true;

					switch (levelSelect) // Zooming out
					{
						case 1:
							Speed = 10.0f;
							tempDirection = new Vector2(camera.Position.X - level1Button.GetPosition.X, camera.Position.Y - level1Button.GetPosition.Y);
							Direction = CollisionHelper.UnitVector(tempDirection);
							break;
						case 2:
							Speed = 10.0f;
							tempDirection = new Vector2(camera.Position.X - level2Button.GetPosition.X, camera.Position.Y - level2Button.GetPosition.Y);
							Direction = CollisionHelper.UnitVector(tempDirection);
							break;
					}
				}
			}

			debugStrings[0] = "IsTransitioningIn=" + isTransitioningIn + " IsTransitioningOut=" + isTransitioningOut + " TranitionAlpha=" + TransitionAlpha;

			debugLabel.Text = debugStrings[0] + "\n" +
							  debugStrings[1] + "\n" +
							  debugStrings[2] + "\n" +
							  debugStrings[3] + "\n" +
							  debugStrings[4] + "\n" +
							  debugStrings[5] + "\n" +
							  debugStrings[6] + "\n" +
							  debugStrings[7] + "\n" +
							  debugStrings[8] + "\n" +
							  debugStrings[9];

			base.Update(gameTime);
		}



		public override void Draw(GameTime gameTime)
		{
			//zooming, buttons on map
			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetTransformation());
			{
				if (!isTransitioningIn && !isTransitioningOut)
				{
					spriteBatch.Draw(mapBackground, Vector2.Zero, Color.White);
					level1Button.Draw(gameTime, spriteBatch);
					level2Button.Draw(gameTime, spriteBatch);
				}
			}
			spriteBatch.End();

			spriteBatch.Begin();
			{
				if (isTransitioningIn || isTransitioningOut)
				{
					spriteBatch.Draw(line, new Rectangle(0, 0, myGame.WindowSize.X, myGame.WindowSize.X), new Color(0, 0, 0, TransitionAlpha));
				}
				if (!isTransitioningIn && !isTransitioningOut)
				{
					spriteBatch.Draw(mapHudTexture, new Vector2(HudScroll, 0), Color.White);

					if (levelPullUp == false)
					{
						backButton.Draw(gameTime, spriteBatch);
					}
					if (levelPullUp == true)
					{
						hudBackButton.Draw(gameTime, spriteBatch);
					}
				}

				debugLabel.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}

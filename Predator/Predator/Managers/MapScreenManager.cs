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

namespace Predator.Managers
{
	/// <summary>
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class MapScreenManager : Microsoft.Xna.Framework.DrawableGameComponent
	{
		/// <summary>
		/// The game that the map screen manager runs off of.
		/// </summary>
		private Game1 myGame;
		/// <summary>
		/// The sprite batch that the map screen manager uses.
		/// </summary>
		private SpriteBatch spriteBatch;

		/// <summary>
		/// The camrea used by the map screen.
		/// </summary>
		protected Camera Camera;

		#region Textures
		/// <summary>
		/// Loads the texture for the line.
		/// </summary>
		public Texture2D LineTexture;
		/// <summary>
		/// Loads the texture for the map.
		/// </summary>
		public Texture2D MapBackgroundTexture;
		/// <summary>
		/// Loads the texture for the map screen HUD.
		/// </summary>
		public Texture2D MapHUDTexture;
		/// <summary>
		/// Loads the texture for the button.
		/// </summary>
		public Texture2D ButtonTexture;
		/// <summary>
		/// Loads the texture for the back button.
		/// </summary>
		public Texture2D BackButtonTexture;
		/// <summary>
		/// Loads the texture for the HUD back button.
		/// </summary>
		public Texture2D HUDBackButtonTexture;
		#endregion

		#region Map Stuff
		/// <summary>
		/// The default speed to move the camera at.
		/// </summary>
		protected float Speed = 10;
		/// <summary>
		/// The default position to move the HUD at.
		/// </summary>
		protected int HudScroll = 1024;
		/// <summary>
		/// The current level to select.
		/// when zoomed in the hud is always the same(same start button) so when
		/// you first click on the level it will set this value so when you click
		/// on the start it will load the level using a switch
		/// level 1 = 1, level 2 = 2... etc.
		/// </summary>
		public int LevelSelect = 0;

		/// <summary>
		/// The direction that the camera is moving at.
		/// </summary>
		protected Vector2 Direction;

		/// <summary>
		/// Becomes true when you select a level pulling up level info HUD
		/// </summary>
		protected bool LevelPullUp = false;
		/// <summary>
		/// Tells if the the camera is zooming in.
		/// </summary>
		public bool IsZoomingIn = false;
		/// <summary>
		/// Tells if the camera is zomming out.
		/// </summary>
		public bool IsZoomingOut = false;
		/// <summary>
		/// Tells if the HUD should scroll in.
		/// </summary>
		public bool IsHUDScrollingIn = false;
		/// <summary>
		/// Tells if the HUD should scroll out.
		/// </summary>
		public bool IsHUDScrollingOut = false;
		#endregion

		#region Buttons
		/// <summary>
		/// The button for level 1.
		/// </summary>
		public Button Level1Button;
		/// <summary>
		/// The button for level 2.
		/// </summary>
		public Button Level2Button;
		/// <summary>
		/// The button for level 3.
		/// </summary>
		public Button Level3Button;
		/// <summary>
		/// The back button for exiting the map.
		/// </summary>
		public Button ExitButton;
		/// <summary>
		/// The HUD back button for zooming out.
		/// </summary>
		public Button HUDBackButton;
		#endregion

		#region Transitioning Stuff
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
		public bool IsTransitioningIn
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets if the level is transitioning out of the map screen.
		/// </summary>
		public bool IsTransitioningOut
		{
			get;
			set;
		}
		/// <summary>
		/// Gets or sets the alpha to transition in or out of the map screen.
		/// </summary>
		public bool SetAlpha
		{
			get;
			set;
		}
		#endregion

		/// <summary>
		/// Creates the map screen manager.
		/// </summary>
		/// <param name="game">The game that the map screen manager runs off of.</param>
		public MapScreenManager(Game1 game)
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

		/// <summary>
		/// Loads the content for the map screen manager.
		/// </summary>
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(myGame.GraphicsDevice);

			LoadTextures();

			Camera = new Camera(myGame.GraphicsDevice.Viewport, new Point(2048, 1536), 1f);
			Camera.Position = new Vector2(512, 256);
			Camera.Zoom = 0.5f;

			Level1Button = new Button(ButtonTexture, new Vector2(700, 400), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, Camera);
			Level2Button = new Button(ButtonTexture, new Vector2(100, 120), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White, Camera);
			ExitButton = new Button(ButtonTexture, new Vector2(50, 700), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White);
			HUDBackButton = new Button(ButtonTexture, new Vector2(100, 700), myGame.segoeUIRegular, 1f, Color.Black, "", Color.White);

			base.LoadContent();
		}

		/// <summary>
		/// Loads the textures for the map screen manager.
		/// </summary>
		public void LoadTextures()
		{
			MapBackgroundTexture = Game.Content.Load<Texture2D>(@"images\gui\map\tempMapScreen");
			ButtonTexture = Game.Content.Load<Texture2D>(@"images\gui\map\tempMapButton");
			HUDBackButtonTexture = Game.Content.Load<Texture2D>(@"images\gui\map\tempMapButton");
			BackButtonTexture = Game.Content.Load<Texture2D>(@"images\gui\map\tempMapButton");
			MapHUDTexture = Game.Content.Load<Texture2D>(@"images\gui\map\tempMapHud");
			LineTexture = Game.Content.Load<Texture2D>(@"images\other\line");
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{
			// TODO: Add your update code here
			if (IsTransitioningIn)
			{
				IsTransitioningOut = false;

				TransitionAlpha += 1.0f / (float)gameTime.ElapsedGameTime.Milliseconds;

				if (TransitionAlpha > 1.0f)
				{
					IsTransitioningIn = false;
					TransitionAlpha = 1.0f;
				}
			}

			if (IsTransitioningOut)
			{
				IsTransitioningIn = false;

				TransitionAlpha -= 1.0f / (float)gameTime.ElapsedGameTime.Milliseconds;

				if (TransitionAlpha <= 0.0f)
				{
					IsTransitioningOut = false;
					TransitionAlpha = 0.0f;

					myGame.SetCurrentLevel(Game1.GameLevels.GAME);
				}
			}

			if (IsZoomingIn == true)
			{
				Camera.Zoom += 0.01f;
				Camera.Position += Direction * Speed;

			}
			if (IsZoomingOut == true)
			{
				Camera.Zoom -= 0.01f;
				Camera.Position -= Direction * Speed;

			}
			if (Camera.Zoom <= 0.5f)
			{
				Camera.Zoom = 0.5f;
				Camera.Position = new Vector2(0, 0);
				IsZoomingOut = false;
			}

			if (IsHUDScrollingIn)
			{
				if (Camera.Position.X < myGame.WindowSize.X / 2)
				{
					HudScroll -= 10;
					if (HudScroll <= 527)
					{
						HudScroll = 527;
						IsHUDScrollingIn = false;
					}
				}
				else
				{
					HudScroll += 10;
					if (HudScroll >= 0)
					{
						HudScroll = 0;
						IsHUDScrollingIn = false;
					}
				}
			}
			if (IsHUDScrollingOut)
			{
				if (Camera.Position.X < myGame.WindowSize.X / 2)
				{
					HudScroll += 10;
					if (HudScroll >= 1024)
					{
						HudScroll = 1024;
						IsHUDScrollingOut = false;
					}
				}
				else
				{
					HudScroll -= 10;
					if (HudScroll <= -527)
					{
						HudScroll = -527;
						IsHUDScrollingIn = false;
					}
				}
			}
			switch (LevelSelect)
			{
				case 1:
					if (Camera.Position.X - Level1Button.Position.X >= 100 && Camera.Position.Y - Level1Button.Position.Y <= 100)
					{
						Speed = 0.0f;
					}
					break;
				case 2:
					if (Camera.Position.X - Level2Button.Position.X <= 100 && Camera.Position.Y - Level2Button.Position.Y <= 100)
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
			if (LevelPullUp == false)
			{
				Vector2 tempDirection;

				Level1Button.Update(gameTime);
				Level2Button.Update(gameTime);

				ExitButton.Update(gameTime);
				if (Level1Button.Clicked()) // Zooming into zone 1
				{
					Speed = 10.0f;
					LevelSelect = 1;
					IsHUDScrollingIn = true;
					LevelPullUp = true;
					IsZoomingIn = true;
					IsZoomingOut = false;
					tempDirection = new Vector2(Level1Button.Position.X - Camera.Position.X, Level1Button.Position.Y - Camera.Position.Y);
					Direction = CollisionHelper.UnitVector(tempDirection);
				}
				if (Level2Button.Clicked()) // Zooming into zone 2
				{
					Speed = 10.0f;
					LevelSelect = 2;
					IsHUDScrollingIn = true;
					LevelPullUp = true;
					IsZoomingIn = true;
					IsZoomingOut = false;
					tempDirection = new Vector2(Level2Button.Position.X - Camera.Position.X, Level2Button.Position.Y - Camera.Position.Y);
					Direction = CollisionHelper.UnitVector(tempDirection);
				}

				if (ExitButton.Clicked())
				{
					IsTransitioningOut = true;
				}
			}
			if (LevelPullUp == true)
			{
				Vector2 tempDirection;

				HUDBackButton.Update(gameTime);

				if (HUDBackButton.Clicked())
				{
					LevelPullUp = false;
					IsZoomingOut = true;
					IsZoomingIn = false;
					IsHUDScrollingOut = true;

					switch (LevelSelect) // Zooming out
					{
						case 1:
							Speed = 10.0f;
							tempDirection = new Vector2(Camera.Position.X - Level1Button.Position.X, Camera.Position.Y - Level1Button.Position.Y);
							Direction = CollisionHelper.UnitVector(tempDirection);
							break;
						case 2:
							Speed = 10.0f;
							tempDirection = new Vector2(Camera.Position.X - Level2Button.Position.X, Camera.Position.Y - Level2Button.Position.Y);
							Direction = CollisionHelper.UnitVector(tempDirection);
							break;
					}
				}
			}

			if (myGame.IsGameDebug)
			{
				DebugTable debugTable = new DebugTable();
				string[,] rawTable = {
										 { "Map Screen", "Fade" }
									 };
				debugTable.initalizeTable(rawTable);

				myGame.debugStrings[0] = debugTable.ReturnStringSegment(0, 0);
				myGame.debugStrings[1] = debugTable.ReturnStringSegment(0, 1) + "IsTransitioningIn=" + IsTransitioningIn + " IsTransitioningOut=" + IsTransitioningOut + " TranitionAlpha=" + TransitionAlpha;
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// Draws the content of the map screen manager.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Draw(GameTime gameTime)
		{
			//zooming, buttons on map
			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Camera.GetTransformation());
			{
				if (!IsTransitioningIn && !IsTransitioningOut)
				{
					spriteBatch.Draw(MapBackgroundTexture, Vector2.Zero, Color.White);
					Level1Button.Draw(gameTime, spriteBatch);
					Level2Button.Draw(gameTime, spriteBatch);
				}
			}
			spriteBatch.End();

			spriteBatch.Begin();
			{
				if (IsTransitioningIn || IsTransitioningOut)
				{
					//spriteBatch.Draw(line, new Rectangle(0, 0, myGame.WindowSize.X, myGame.WindowSize.X), new Color(0, 0, 0, TransitionAlpha));
				}
				if (!IsTransitioningIn && !IsTransitioningOut)
				{
					spriteBatch.Draw(MapHUDTexture, new Vector2(HudScroll, 0), Color.White);

					if (LevelPullUp == false)
					{
						ExitButton.Draw(gameTime, spriteBatch);
					}
					if (LevelPullUp == true)
					{
						HUDBackButton.Draw(gameTime, spriteBatch);
					}
				}

				spriteBatch.Draw(LineTexture, new Rectangle((int)Level1Button.RelitiveCenter.X, (int)Level1Button.RelitiveCenter.Y, 1000, 1), Color.Red);
				spriteBatch.Draw(LineTexture, new Rectangle((int)Level1Button.RelitiveCenter.X, (int)Level1Button.RelitiveCenter.Y, 1, 1000), Color.Red);
				spriteBatch.Draw(LineTexture, new Rectangle((int)Level2Button.RelitiveCenter.X, (int)Level2Button.RelitiveCenter.Y, 1000, 1), Color.Blue);
				spriteBatch.Draw(LineTexture, new Rectangle((int)Level2Button.RelitiveCenter.X, (int)Level2Button.RelitiveCenter.Y, 1, 1000), Color.Blue);

				myGame.debugLabel.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}

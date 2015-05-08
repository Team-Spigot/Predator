using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VoidEngine;

namespace VoidEngine.VGame
{
	public class Parallax
	{
		/// <summary>
		///
		/// </summary>
		private Texture2D texture
		{
			get;
			set;
		}
		/// <summary>
		///
		/// </summary>
		public Vector2 position
		{
			get;
			set;
		}
		/// <summary>
		///
		/// </summary>
		private Color color
		{
			get;
			set;
		}
		/// <summary>
		///
		/// </summary>
		public Vector2 multiplier
		{
			get;
			private set;
		}
		/// <summary>
		///
		/// </summary>
		private Camera camera
		{
			get;
			set;
		}
		/// <summary>
		///
		/// </summary>
		public Vector2 parallax
		{
			get;
			set;
		}
		/// <summary>
		///
		/// </summary>
		private Vector2 defaultPosition
		{
			get;
			set;
		}
		/// <summary>
		///
		/// </summary>
		public GraphicsDeviceManager graphics
		{
			get;
			set;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="position"></param>
		/// <param name="color"></param>
		/// <param name="multiplier"></param>
		/// <param name="camera"></param>
		public Parallax(Texture2D texture, Vector2 position, Color color, Vector2 multiplier, Camera camera)
		{
			this.texture = texture;
			this.position = position;
			this.color = color;
			this.multiplier = multiplier;
			this.camera = camera;
			defaultPosition = position;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			parallax = new Vector2((camera.Position.X) * multiplier.X, (camera.Position.Y) * multiplier.Y);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)camera.viewportSize.X, (int)camera.viewportSize.Y), new Rectangle((int)parallax.X, (int)parallax.Y, (int)texture.Width, (int)texture.Height), color);
		}
	}
}

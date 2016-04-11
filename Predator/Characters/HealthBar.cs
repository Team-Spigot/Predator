using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using VoidEngine.VGame;

namespace Predator.Characters
{
	public class HealthBar : Sprite
	{
		/// <summary>
		/// The color to wash the health bar with.
		/// </summary>
		protected Color washColor;
		/// <summary>
		/// Gets or sets the health bar's stored width.
		/// </summary>
		protected int barwidth
		{
			get;
			set;
		}

		/// <summary>
		/// Creates the health bar object class, with the default animation set.
		/// </summary>
		/// <param name="texture">The texture to use with the health bar.</param>
		/// <param name="position">The position to start the health bar at.</param>
		/// <param name="color">The color to mask the health bar with.</param>
		public HealthBar(Texture2D texture, Vector2 position, Color color)
			: base(position, color, texture)
		{
			AddAnimations(texture);
			SetAnimation("IDLE");
		}

		/// <summary>
		/// Creates the health bar object class, with a custom animation set.
		/// </summary>
		/// <param name="animationSetList">The animation set to use with the health bar.</param>
		/// <param name="defaultFrameName">The default frame to set the current animation to.</param>
		/// <param name="position">The position to start the health bar at.</param>
		/// <param name="color">The color to mask the health bar with.</param>
		public HealthBar(List<AnimationSet> animationSetList, string defaultFrameName, Vector2 position, Color color)
			: base(position, color, animationSetList)
		{
			AnimationSets = animationSetList;
			SetAnimation(defaultFrameName);
		}

		/// <summary>
		/// Updates the health bar with the hp of another object and it's max hp.
		/// </summary>
		/// <param name="gameTime">The game time that the game uses.</param>
		/// <param name="HP">The HP to use with the health bar.</param>
		/// <param name="MaxHP">The max HP to use with the health bar.</param>
		public void Update(GameTime gameTime, float HP, float MaxHP)
		{
			double ratio = ((double)HP / (double)MaxHP);
			barwidth = (int)(CurrentAnimation.frameSize.X * ratio);

			if (ratio <= .5)
			{
				washColor = new Color(255, (int)(255 * ratio * 2), 0);
			}
			if (ratio >= .5)
			{
				washColor = new Color((int)(255 * (1 - ratio) * 2), 255, 0);
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// Draws the health bar.
		/// </summary>
		/// <param name="gameTime">The game time that the game uses.</param>
		/// <param name="spriteBatch">The sprite batch that the game uses.</param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(CurrentAnimation.texture, Position - Offset, new Rectangle(0, 0, barwidth, CurrentAnimation.frameSize.Y), washColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
		}

		/// <summary>
		/// Adds animations to the health bar.
		/// </summary>
		/// <param name="texture">The texture to process.</param>
		protected override void AddAnimations(Texture2D texture)
		{
			AddAnimation("IDLE", texture, new Point(texture.Width, texture.Height), new Point(1, 1), new Point(0, 0), 1600, false);
			base.AddAnimations(texture);
		}
	}
}

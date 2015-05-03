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
using VoidEngine.VGame;
using Predator.Characters;

namespace Predator.Characters
{
	public class HealthBar : Sprite
	{
		Color washColor;
		int barwidth;
		Player player;
		public Player SetPlayer
		{
			set
			{
				player = value;
			}
		}

		public HealthBar(Texture2D texture, Vector2 position, Color color, Player player)
			: base(position, color, texture)
		{
			AddAnimations(texture);
			SetAnimation("IDLE");
			this.player = player;
		}

		public override void Update(GameTime gameTime)
		{
			double ratio = ((double)player.HP / (double)player.MaxHP);
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

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(CurrentAnimation.texture, Position - Offset, new Rectangle(0, 0, barwidth, CurrentAnimation.frameSize.Y), washColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
		}

		protected override void AddAnimations(Texture2D texture)
		{
			AddAnimation("IDLE", texture, new Point(texture.Width, texture.Height), new Point(1, 1), new Point(0, 0), 1600, false);
			base.AddAnimations(texture);
		}
	}
}

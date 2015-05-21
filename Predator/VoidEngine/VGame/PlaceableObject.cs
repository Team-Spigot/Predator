﻿using System;
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
using VoidEngine.Helpers;
using VoidEngine.Particles;
using VoidEngine.VGame;
using VoidEngine.VGUI;

namespace VoidEngine.VGame
{
	public class PlaceableObject : Sprite
	{
		public PlaceableObject(Texture2D texture, Vector2 position, Color color) : base(position, color, texture)
		{
			AddAnimations(texture);
			SetAnimation("IDLE");

			Position = position;
			Color = color;
			inbounds = new Rectangle(0, 0, texture.Width, texture.Height);
		}

		public PlaceableObject(List<AnimationSet> animationSets, string defaultFrameName, Vector2 position, Color color, Rectangle boundingBox)
			: base(position, color, animationSets)
		{
			AnimationSets = animationSets;
			SetAnimation(defaultFrameName);

			Position = position;
			Color = color;
			inbounds = boundingBox;
		}

		protected override void AddAnimations(Texture2D texture)
		{
			AddAnimation("IDLE", texture, new Point(texture.Width, texture.Height), new Point(1, 1), new Point(0, 0), 1600, false);

			base.AddAnimations(texture);
		}
	}
}

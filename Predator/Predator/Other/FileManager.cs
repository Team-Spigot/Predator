using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
using Predator.Characters;
using Predator.Game;
using Predator.Other;

namespace Predator.Other
{
	[Serializable]
	public class FileManagerTemplate
	{
		[Serializable]
		public struct PlayerData
		{
			public int Level;
			public float MainHP;
			public float MaxHP;
			public float Damage;
			public float Defense;
			public int StatPoints;
			public float PStrength;
			public float PAgility;
			public float PDefense;
			public int PExp;
			public int Lvl;
			public Vector2 Position;
			public Keys[,] MovementKeys;
		}

		[Serializable]
		public struct Options
		{
			public bool VSync;
			public Point WindowSize;
		}

		[Serializable]
		public struct Game
		{
			public int CurrentLevel;
		}
	}

	[Serializable]
	public class SaveFile
	{
		public string FileSaveVersion = "0.0.0.1";
		public FileManagerTemplate.Game GameData;
		public FileManagerTemplate.Options OptionsData;
		public FileManagerTemplate.PlayerData PlayerData;
	}

	public static class SaveFileManager
	{
		public static void SaveFile(SaveFile saveFile, string destination, string file)
		{
			Stream stream = new FileStream(destination + file, FileMode.Create, FileAccess.Write, FileShare.Write);
			BinaryFormatter formatter = new BinaryFormatter();

			formatter.Serialize(stream, saveFile);
			stream.Close();
		}

		public static SaveFile LoadFile(string destination, string file)
		{
			SaveFile tempSaveFile;
			Stream stream = new FileStream(destination + file, FileMode.Open, FileAccess.Read, FileShare.Read);
			BinaryFormatter formatter = new BinaryFormatter();

			tempSaveFile = (SaveFile)formatter.Deserialize(stream);

			stream.Close();

			return tempSaveFile;
		}
	}
}

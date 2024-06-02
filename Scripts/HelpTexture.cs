using Godot;
using System;
using System.Collections.Generic;

// Author : Sophia Solignac
namespace Com.IsartDigital.ProjectName {
	
	public class HelpTexture : TextureRect
	{
		int currentTexture = 0;
		[Export] List<Texture> allHelpTexture = new List<Texture>();

		public override void _Ready()
		{
			if (allHelpTexture.Count < 0) { return; }
			Texture = allHelpTexture[0];
		}

		public void NextTexture()
		{
			currentTexture++;
			if (currentTexture >= allHelpTexture.Count) currentTexture = 0;
			Texture = allHelpTexture[currentTexture];
		}
	}
}
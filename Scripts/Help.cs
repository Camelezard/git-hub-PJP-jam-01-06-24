using Godot;
using System;

// Author : Sophia Solignac
namespace Com.IsartDigital.ProjectName {

	public class Help : PanelContainer
	{
		[Export] NodePath pathTextRect;
        HelpTexture textRect => GetNode<HelpTexture>(pathTextRect);

        public override void _GuiInput(InputEvent @event)
        {
            if (@event is InputEventMouseButton mbe)
            {
                if (mbe.IsPressed())
                {
                    textRect.NextTexture();
                }
            }
            base._GuiInput(@event);
        }
    }
}
using Godot;
using System;

// Author : Sophia Solignac
namespace Com.IsartDigital.ProjectName {
	
	public class HelpOutside : Control
	{
        public override void _GuiInput(InputEvent @event)
        {
            if (@event is InputEventMouseButton mbe)
            {
                Visible = false;
            }
            base._GuiInput(@event);
        }
    }
}
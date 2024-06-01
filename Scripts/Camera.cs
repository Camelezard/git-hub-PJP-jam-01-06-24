using Com.IsartDigital.ProjectName.Utils;
using Godot;
using System;
using System.Drawing;

// Author : Sophia Solignac
namespace Com.IsartDigital.ProjectName {
	
	public class Camera : Camera2D
	{
		Vector2 screenSize;
        private float speed = 100f;
        float cameraMoveThreshold = 0.4f;

        public override void _Ready()
		{
			screenSize = GetViewportRect().Size;
			GetViewport().Connect(Signals.Ui.SCREEN_RESIZED, this, nameof(OnScreenSizeChanged));
		}

        private void OnScreenSizeChanged()
        {
            screenSize = GetViewportRect().Size;
        }

        public override void _Process(float pDelta)
		{
            Vector2 mouseDistanceRatio = GetLocalMousePosition() / screenSize;
            if (mouseDistanceRatio.y >  cameraMoveThreshold) Position += Vector2.Down * speed * pDelta;
            if (mouseDistanceRatio.y < -cameraMoveThreshold) Position += Vector2.Up * speed * pDelta;
            if (mouseDistanceRatio.x >  cameraMoveThreshold) Position += Vector2.Right * speed * pDelta;
            if (mouseDistanceRatio.x < -cameraMoveThreshold) Position += Vector2.Left * speed * pDelta;
        }


		protected override void Dispose(bool pDisposing)
		{

		}
	}
}
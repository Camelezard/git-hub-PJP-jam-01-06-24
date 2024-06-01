using Godot;
using System;

// Author : Sophia Solignac
namespace Com.IsartDigital.ProjectName.Utils
{
    public static class Signals
    {
        public static class Node
        {
            public static string TIMEOUT = "timeout";
            public static string OUT_OF_SCREEN = "screen_exited";
        }
        public static class Ui
        {
            public static string BUTTON_PRESSED = "pressed";
            public static string FOCUS_ENTERED = "focus_entered";
            public static string FOCUS_EXITED = "focus_exited";
            public static string MOUSE_ENTERED = "mouse_entered";
            public static string MOUSE_EXITED = "mouse_exited";
            public static string SCREEN_RESIZED = "size_changed";
            
        }

    }
}
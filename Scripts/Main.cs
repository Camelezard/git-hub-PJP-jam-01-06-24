using Com.IsartDigital.ProjectName.Utils;
using Godot;
using System;
using System.Drawing.Drawing2D;
using System.Threading;

// Author : Sophia Solignac
namespace Com.IsartDigital.ProjectName {

	public class Main : Node2D
    {
        private float cell_size = 20;
        private float half_cell_size;
        [Export(PropertyHint.Range,"2,100")] public float Cell_size { 
            get => cell_size; 
            set {
                cell_size = value;
                half_cell_size = cell_size / 2;
                Update();
            }
        }

        [Export] private Vector2 view_size;
        private Vector2 half_view_size;
        private Vector2 offset;


        public override void _Ready()
		{
            view_size = GetViewportRect().Size;
            half_view_size = view_size / 2;
            offset = half_view_size + (Vector2.One * half_cell_size);
            GetViewport().Connect(Signals.Ui.SCREEN_RESIZED, this, nameof(OnScreenSizeChanged));
        }

        protected override void Dispose(bool pDisposing)
		{

        }
        private void OnScreenSizeChanged()
        {
            view_size = GetViewportRect().Size;
            half_view_size = view_size / 2;
            offset = half_view_size + (Vector2.One * half_cell_size);
        }

        public override void _Draw()
        {
            base._Draw();
            //    # Setup the range: The grids center at the center of the screen
            //    # Start and end for the number of columns
            var x_start = (((int)-half_view_size.x) / Cell_size) - 1;
            var x_end = (((int)half_view_size.x) / Cell_size) + 1;

            //    # Start and end for the number of rows
            var y_start = (((int)-half_view_size.y) / Cell_size) - 1;
            var y_end = (((int)half_view_size.y) / Cell_size) + 1;

            for(int x = (int)x_start; x <= x_end; x++){
                for (int y = (int)y_start; y <= y_end; y++)
                {
                    var cell_pos = (new Vector2(x, y) * Cell_size) + offset;
                    // # Center
                    DrawRectOutline(cell_pos, Vector2.One * Cell_size);
                }
            }
        }

        private void DrawRectOutline(Vector2 pOrigin, Vector2 pRectSize)
        {
            DrawRect(new Rect2(pOrigin, pRectSize), Colors.Black,false,1,true);
        }
    }
}

//const cell_size = 12
//const half_cell_size = cell_size / 2
 
//onready var view_size = get_viewport_rect().size
//onready var half_view_size = view_size / 2
//onready var offset = half_view_size + (Vector2.ONE * half_cell_size)
 
 
//func _draw() -> void:
//    # Setup the range: The grids center at the center of the screen
//    # Start and end for the number of columns
//    var x_start = int((-half_view_size.x) / cell_size) - 1
//    var x_end = int((half_view_size.x) / cell_size) + 1
     
//    # Start and end for the number of rows
//    var y_start = int((-half_view_size.y) / cell_size) - 1
//    var y_end = int((half_view_size.y) / cell_size) + 1
 
//    for x in range(x_start, x_end):
//        for y in range(y_start, y_end):
//            # The offset must be added
//            # so we end up with a cell center in the middle of the screen
//            # One can adjust the offset based on what the engine considers [0,0]
//            var cell_pos = (Vector2(x, y) * cell_size) + offset

//            # Center
//            _draw_rect_outline(
//                cell_pos,
//                Vector2.ONE * cell_size
//            )

//# We wrapping the engines draw rect method to handle defaults
//func _draw_rect_outline(origin, rect_size):
//    # The engines rect drawing method positions the rect at the top left corner
//    # The bottom right corner position is found with rect.position + rect.size
//    draw_rect(
//        Rect2(origin, rect_size),
//        Color.black,
//        false,
//        .5,
//        true
//    )
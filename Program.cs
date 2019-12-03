using System;
using SFML.Window;
using SFML.System;
using SFML.Graphics;
using System.Collections.Generic;

namespace SmoothMovementDistance
{
    class Program
    {
        static RenderWindow window = new RenderWindow(new VideoMode(700, 700), "Test", Styles.Close, new ContextSettings() { AntialiasingLevel = 8 });
        static RectangleShape line = new RectangleShape() { Position = new Vector2f(50, window.Size.Y / 2), Size = new Vector2f(window.Size.X - 100, 1), FillColor = Color.Yellow};
        static Obj circle = new Obj(30, new Color(0, 255, 175));
        static Obj circle1 = new Obj(5, new Color(0, 175, 255));

        static List<CircleShape> trails = new List<CircleShape>();

        static bool drawC1 = true;
        static bool drawC2 = true;
        static bool drawLine;
        static bool drawTrail;

        static Toggle[] toggles =
        {
             new Toggle (0, new Vector2f(20,10)),
             new Toggle (1, new Vector2f(20,40)),
             new Toggle (2, new Vector2f(20,70)),
             new Toggle (3, new Vector2f(20,100)),
        };

        static void Main(string[] args)
        {
            window.SetFramerateLimit(60);
            window.Closed += WinClose;

            while(window.IsOpen)
            {
                circle.Update1(1, line.Position, line.Size.X, true);
                circle1.Update2(5, circle.pos, 50, true, true);

                if (drawTrail)
                    trails.Add(new CircleShape(3) { Position = circle1.pos, Origin = new Vector2f(3, 3), FillColor = new Color(200, 125, 0) });
                else
                    trails.Clear();


                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                    foreach (var t in toggles)
                        if (new FloatRect((Vector2f)Mouse.GetPosition(window), new Vector2f(1,1)).Intersects(t.text.GetGlobalBounds()))
                            t.Update();
                // Drawing
                window.Clear(new Color(0,0,0));

                if (drawLine) window.Draw(line);
                if (drawC1) window.Draw(circle.shape);
                if (drawC2) window.Draw(circle1.shape);
                if (drawTrail) 
                foreach (var t in trails)
                    window.Draw(t);

                foreach (var t in toggles)
                    window.Draw(t.text);

                window.DispatchEvents();
                window.Display();
            }
        }

        class Obj
        {
            public Vector2f pos;
            public CircleShape shape;
            private int tick;

            public Obj(float size, Color color)
            {
                shape = new CircleShape(size) { Origin = new Vector2f(size, size), FillColor = color };
            }

            public void Update1(float speed, Vector2f sPos, float Rad, bool rotX = false, bool rotY = false)
            {
                pos = new Vector2f(rotX ? sPos.X + Rad / 2 + -MathF.Cos(tick * speed / 100) * Rad / 2 : sPos.X, rotY ? sPos.Y + Rad / 2 + -MathF.Sin(tick * speed / 100) * Rad / 2 : sPos.Y);
                shape.Position = pos;
                tick++;
            }
            public void Update2(float speed, Vector2f sPos, float Rad, bool rotX = false, bool rotY = false)
            {
                pos = new Vector2f(rotX ? sPos.X + MathF.Cos(tick * speed / 100) * Rad : sPos.X, rotY ? sPos.Y + MathF.Sin(tick * speed / 100) * Rad: sPos.Y);
                shape.Position = pos;
                tick++;
            }
        }
        class Toggle
        {
            Font font = new Font(Properties.Resources.Landsans);

            public Text text;

            public int func;

            public Toggle(int _func, Vector2f pos)
            {
                func = _func;
                text = new Text("", font, 40) { FillColor = Color.White };
                switch (func)
                {
                    case 0:
                        text.DisplayedString = "Circle 1";
                        text.FillColor = drawC1 ? Color.Green : Color.Red;
                        break;
                    case 1:
                        text.DisplayedString = "Circle 2";
                        text.FillColor = drawC2 ? Color.Green : Color.Red;
                        break;
                    case 2:
                        text.DisplayedString = "Line";
                        text.FillColor = drawLine ? Color.Green : Color.Red;
                        break;
                    case 3:
                        text.DisplayedString = "Trail";
                        text.FillColor = drawTrail ? Color.Green : Color.Red;
                        break;
                }
                text.Position = pos;
            }

            public void Update()
            {
                while (Mouse.IsButtonPressed(Mouse.Button.Left))
                { }

                switch (func)
                {
                    case 0:
                        drawC1 = !drawC1;
                        text.FillColor = drawC1 ? Color.Green : Color.Red;
                        break;
                    case 1:
                        drawC2 = !drawC2;
                        text.FillColor = drawC2 ? Color.Green : Color.Red;
                        break;
                    case 2:
                        drawLine = !drawLine;
                        text.FillColor = drawLine ? Color.Green : Color.Red;
                        break;
                    case 3:
                        drawTrail = !drawTrail;
                        text.FillColor = drawTrail ? Color.Green : Color.Red;
                        break;
                }
            }
        }

        static void WinClose(object sender, EventArgs e)
        {
            window.Close();
        }
    }
}

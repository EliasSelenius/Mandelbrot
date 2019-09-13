using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Glow;

using OpenTK;
using OpenTK.Input;

using System.IO;

namespace Manedelbrot {
    class Program : Applet {
        public static Program applet;
        static void Main(string[] args) {
            applet = new Program();
            applet.Start();
        }

        static VertexArray vao;

        static ShaderProgram shader;

        protected override void Load() {

            shader = CreateShader(System.IO.File.ReadAllText("data/frag.glsl"), System.IO.File.ReadAllText("data/vert.glsl"));
            shader.Use();

            var vbo = new Buffer<float>();
            vbo.Initialize(new float[] {
                -1, -1,
                -1, 1,
                1, -1,
                1, 1
            }, OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);

            var ebo = new Buffer<uint>();
            ebo.Initialize(new uint[] {
                0, 1, 2,
                3, 2, 1
            }, OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);

            vao = new VertexArray();
            vao.SetBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, vbo);
            vao.SetBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ElementArrayBuffer, ebo);

            vao.AttribPointer(shader.GetAttribLocation("v_pos"), 2, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, false, sizeof(float) * 2, 0);


            Window.MouseWheel += Window_MouseWheel;
            Window.MouseMove += Window_MouseMove;
        }

        private void Window_MouseMove(object sender, OpenTK.Input.MouseMoveEventArgs e) {
            var state = Mouse.GetState();
            if (state.IsButtonDown(MouseButton.Left)) {
                viewpos += new Vector2(e.XDelta, -e.YDelta) * zoom * .003f;
                Console.WriteLine("viewpos: " + viewpos);
            }

        }


        private void Window_MouseWheel(object sender, OpenTK.Input.MouseWheelEventArgs e) {
            zoom -= zoom * e.DeltaPrecise/10f ;
            Console.WriteLine("zoom: " + zoom);
        }

        Vector2 viewpos = Vector2.Zero;
        float zoom = 1;
        float totalTime = 0;
        protected override void Render() {
            totalTime += (float)Window.RenderTime;
            shader.SetVec2("viewpos", viewpos.X, viewpos.Y);
            shader.SetFloat("zoom", zoom);
            shader.SetFloat("time", totalTime);
            vao.DrawElements(OpenTK.Graphics.OpenGL4.PrimitiveType.Triangles, 6, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt);
        }


        protected override void Update() {
            
        }

        protected override void OnWindowResize() {
            shader.SetVec2("resolution", Window.Width, Window.Height);
        }
    }
}

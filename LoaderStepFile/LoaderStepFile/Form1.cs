using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;


namespace LoaderStepFile
{
    public partial class Form1 : Form
    {
        bool loaded = false;
        int x = 0;
        float rotation = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            GL.ClearColor(Color.SkyBlue); // Yey! .NET Colors can be used directly!
            SetupViewport();
            //Application.Idle += Application_Idle;
        }

        void Application_Idle(object sender, EventArgs e)
        {
            // no guard needed -- we hooked into the event in Load handler
            while (glControl1.IsIdle)
            {
                rotation += 1;
                Render();
            }
        }

        private void Render()
        {
            if (!loaded) // Play nice
                return;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Translate(x, 0, 0); // position triangle according to our x variable
            if (glControl1.Focused) // Simple enough :)
                GL.Color3(Color.Yellow);
            else
                GL.Color3(Color.Blue);
            GL.Rotate(rotation, Vector3.UnitZ); // OpenTK has this nice Vector3 class!
            GL.Begin(BeginMode.Triangles); ;
            GL.Vertex3(10, 20, 0);
            GL.Vertex3(100, 20, 0);
            GL.Vertex3(100, 50, 1);
            GL.End();

            glControl1.SwapBuffers();
        }
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void SetupViewport()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0, w, 0, h, -1, 1); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            SetupViewport();
            glControl1.Invalidate();
            //if (!loaded)
            //    return;
            
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!loaded)
                return;
            if (e.KeyCode == Keys.Space)
            {
                x++;
                glControl1.Invalidate();
            }
        }
    }
}

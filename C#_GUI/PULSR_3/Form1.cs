using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;

namespace PULSR_3
{
    public partial class Form1 : Form
    {
        static private int screen_width = Screen.PrimaryScreen.Bounds.Width;
        static private int screen_height = Screen.PrimaryScreen.Bounds.Height;


        int levelStart = 0;
        int cycle = 0;
        int score = 0;
        

        /*
         * The following variables mimick the workspace on the GUI
         * center_x => the origin for the X-axis
         * center_y => the originn for the Y-axis
         * large_circle_radius => 
         * small_circle_radius =>
         */
        private int center_x;
        private int center_y;
        private int large_circle_radius;
        private int small_circle_radius = 10;
        private double target_point_angular_displacement = 270;
        private double angularSpeed = 3;
        private int no_cycles_completed = 0;

        
        private bool move_end_effector_right, move_end_effector_left, move_end_effector_up, move_end_effector_down; //Boolean variables for setting(mimicking) end_effector direction movement on GUI
        private int  end_effector_speed = 9;
        public Form1()
        {
            Debug.WriteLine("screen_width: "+screen_width.ToString());
            large_circle_radius = (int)(screen_width*0.1);
            small_circle_radius = (int)(screen_width * 0.01);
            Debug.WriteLine("path radius: " + small_circle_radius.ToString());
            this.Width = (int) 0.5*screen_width;
            this.Height = (int) 0.5*screen_height;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer.Start();

            this.KeyPreview = true;       // Enable keyevents for the move
            this.KeyDown += keyisdown;   // Register the KeyDown event handler
        }

        //Timer
        private void timer_Tick(object sender, EventArgs e)
        {
            target_point_angular_displacement -= angularSpeed;
            if (target_point_angular_displacement == -90)
            {
                timer.Stop();
                no_cycles_completed += 1;
                cycle_button.Text = no_cycles_completed.ToString();
                //target_point_angular_displacement = 0;
            }
            workspace.Invalidate(); // Trigger a redraw of the PictureBox

            if (move_end_effector_left == true && end_effector.Left > 196)
            {
                end_effector.Left -= end_effector_speed;
            }
            if (move_end_effector_right == true && end_effector.Left < 826)
            {
                end_effector.Left += end_effector_speed;
            }
            if (move_end_effector_up == true && end_effector.Top > 8)
            {
                end_effector.Top -= end_effector_speed;
            }
            if (move_end_effector_down == true && end_effector.Top < 768)
            {
                end_effector.Top += end_effector_speed;
            }

            // Display the coordinates in the terminal
            //Console.WriteLine("PictureBox Coordinates: X = {0}, Y = {1}", pictureBox3.Left, pictureBox3.Top);

            // Update the TextBox with the coordinates
            //textBox.Text = $"X: {pictureBox3.Left}, Y: {pictureBox3.Top}";
        }



        //Close button
        
        private void close_mouse(object sender, MouseEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Do you want to close this window", "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes) { Close(); }
            else if (dr == DialogResult.No) { }
            else { }
        }
        private void close_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }



        //LEVEl
        private void increase_mouse(object sender, MouseEventArgs e) //Increase level
        {
            level_button.Text = Convert.ToString(levelStart += 100);
        }
        private void increase_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }

        private void decrease_mouse(object sender, MouseEventArgs e) //Decrease level
        {
            if (levelStart > 100)
            {
                level_button.Text = Convert.ToString(levelStart -= 100);
            }
        }
        private void decrease_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }

        private void level_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }


        //Start Button
        private void start_mouse(object sender, MouseEventArgs e)
        {
            timer.Stop(); // Stop the timer if it is already running

            target_point_angular_displacement = 270;
            //no_cycles_completed += 1;
            //textBox1.Text = no_cycles_completed.ToString();
            workspace.Paint += new System.Windows.Forms.PaintEventHandler(drawCircularPath);
            timer.Start(); // Start the timer to begin the animation
        }

        private void start_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }
        //Reset Button
        private void reset_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }
        private void reset_mouse(object sender, MouseEventArgs e)
        {
            cycle_button.ResetText();  // Clear the textbox
            target_point_angular_displacement = 270;
            no_cycles_completed = 0;
            timer.Stop();
        }

        private void cyccle_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }

        //Display the score

        private void score_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.IsInputKey = true;
            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }



        // Circle Orbiting
        private void drawCircularPath(object sender, PaintEventArgs e)
        {
            {
                Graphics g = e.Graphics;
                Pen largeCirclePen = new Pen(Color.White, 3.0f);
                //Pen smallCirclePen = new Pen(Color.Red, 3.0f);
                Brush smallCirclePen = new SolidBrush(Color.Red);

                center_x = workspace.Width / 2;
                center_y = workspace.Height / 2;

                // Draw larger circle
                int largeCircleX = center_x - large_circle_radius;
                int largeCircleY = center_y - large_circle_radius;
                int largeCircleDiameter = 2 * large_circle_radius;
                g.DrawEllipse(largeCirclePen, largeCircleX, largeCircleY, largeCircleDiameter, largeCircleDiameter);

                // Draw small orbiting circle
                /*
                double radians = target_point_angular_displacement * Math.PI / 180;
                int smallCircleX = (int)(center_x + large_circle_radius * Math.Cos(radians));
                int smallCircleY = (int)(center_y + large_circle_radius * Math.Sin(radians));

                int smallCircleDiameter = 2 * small_circle_radius;

                int smallCircleXPos = smallCircleX - small_circle_radius;
                int smallCircleYPos = smallCircleY - small_circle_radius;

                g.FillEllipse(smallCirclePen, smallCircleXPos, smallCircleYPos, smallCircleDiameter, smallCircleDiameter);
                */

                // Update the TextBox with the coordinates of the smaller circle
                //textBox.Text = $"SmalCirc: X: {smallCircleXPos}, Y: {smallCircleYPos}";
                // Display the coordinates in the terminal
                //Console.WriteLine("Smaller Circle Coordinates: X = {0}, Y = {1}", smallCircleXPos, smallCircleYPos);
            }
        }

        ///// Movement Functions 
        private void keyisdown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                move_end_effector_left = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                move_end_effector_right = true;
            }
            if (e.KeyCode == Keys.Up)
            {
                move_end_effector_up = true;
            }
            if (e.KeyCode == Keys.Down)
            {
                move_end_effector_down = true;
            }
        }
        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                move_end_effector_left = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                move_end_effector_right = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                move_end_effector_up = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                move_end_effector_down = false;
            }
        }
    }
}

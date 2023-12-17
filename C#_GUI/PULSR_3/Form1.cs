using PULSR_3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;


namespace PULSR_3
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// INITIALIZE PULSR
        /// </summary>

        pulsr pulsr3 = new pulsr();

        //public int value { get; set; }
        public int value;

        public int old_y;
        public int old_x;
        public int new_y;
        private int current_x;
        private int current_y;
        public int new_x;
        public int yOffset = 38;
        public int xOffset = 195;

        int levelStart = 0;
        int cycle = 0;
        int score = 0;
        int distance;
        //
        //private float angle = 0;
        //private const float orbitRadius = 100;
        //private const float centerOffset = 150;
        //
        private int centerX;
        private int centerY;
        private int largeCircleRadius = 250;
        private int smallCircleRadius = 10;
        private double angle = 270;
        private double angularSpeed = 1;
        private int cycleCount = 0;

        public string fileName ;

        public int smallCircleX { get; private set; }
        public int smallCircleY { get; private set; }
        public int DEP { get; private set; }
        public int DEM { get; private set; }

        public Form1()
        {
            // COMMUNICATION INITIALIZATION AND DEFINITION //
            pulsr3.InitializeCommunication();
            //Thread.Sleep(500);

            // COMMUNICATION CHECK //
            if (pulsr3.CheckCommunication())
            {
                Console.WriteLine("Communication Port is Active"); /// change to dialog

                InitializeComponent();
            }
            else
            {
                //Console.WriteLine("Communication Port Not Active");
                //Close();

                Console.WriteLine("Communication check failed for the following components:");
                if (!pulsr3.load_cell_coms.IsOpen)
                {
                    Console.WriteLine(" - Load cell board not found");
                }
                if (!pulsr3.encoder_coms.IsOpen)
                {
                    Console.WriteLine(" - Encoder board not found");
                }
                if (!pulsr3.pulsr2_coms.IsOpen)
                {
                    Console.WriteLine(" - Pulsr communication check failed, restart system to reinitialize communication");
                }
                Close();
            }

            //// Game level selection ////
            //public int value { get; set;}   /////key metric
            if (InputBox("GAME LEVEL", "Select a Level!", ref value) == DialogResult.OK)
            {
                if (value == 0)
                {
                    MessageBox.Show("Passive Mode");
                }
                else if (value == 4)
                {
                    MessageBox.Show("Assistive Mode");
                }
                else if (value == 8)
                {
                    MessageBox.Show("Active Mode");
                }
                else
                {
                    MessageBox.Show("Please Select a Mode");

                    //Close();
                    //Application.Exit();

                }
            }

            // RESET ANGLE PARAMETERS AND DISABLE MOTORS //
            pulsr3.UpdateMotorSpeed(0, 0);
            pulsr3.ResetAngles();

            // PULSR KINEMATICS PARAMETERS DEFINITIION AND INITIALIZATION //
            pulsr3.DefineGeometry(26, 26, 26);
            pulsr3.SetOrigin(20);

            //int[] new_yx = pulsr3.ReturnXYCoordinate();
            //old_y = new_yx[0];
            //old_x = new_yx[1];



            //InitializeComponent();
            //panel12.Paint += panel12_Paint;
            //timer.Tick += timer_Tick;
            //timer.Start();
        }


        /// <summary>
        /// MAIN GUI
        /// </summary>

        // GUI INITIALIZATION //
        private void Form1_Load(object sender, EventArgs e)
        {
            //timer.Start();    //commentend not to start when the form loads, the start button controls it now

            //this.KeyPreview = true;       // Enable keyevents for the move
            //this.KeyDown += keyisdown;   // Register the KeyDown event handler
        }

        /////// Timer //////////////
        private void timer_Tick(object sender, EventArgs e)
        {
            angle -= angularSpeed;
            if (angle == -90)
            {
                timer.Stop();
                cycleCount += 1;
                button1.Text = cycleCount.ToString();
                //button1.ForeColor = Color.White;
                //angle = 0;


                //file.Close();  //Close the file after each cycle

            }
            orbitPanel.Invalidate(); // Trigger a redraw of the panel

            //// Calculate and display the coordinates
            //float centerX = panel12.Width / 2;
            //float centerY = panel12.Height / 2;
            //float orbitingX = centerX + (float)(orbitRadius * Math.Cos(angle)) - centerOffset;
            //float orbitingY = centerY + (float)(orbitRadius * Math.Sin(angle));

            //string coordinates = $"Orbiting Circle: ({orbitingX}, {orbitingY})";
            //Console.WriteLine(coordinates); // Print to the command line

        }

        ////  Close Maximize Minimize  ////
        private void closeButton(object sender, MouseEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Do you want to close this window", "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes) { Close(); }
            else if (dr == DialogResult.No) { }
            else { }
        }
        private void maximizeButton(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
        private void minimizeButton(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
        }

        //////// START Button //////
        private void startMouseEnter(object sender, EventArgs e)
        {
            panel8.BackgroundImage = global::PULSR_3.Properties.Resources.hoverButton;
        }

        private void startMouseLeave(object sender, EventArgs e)
        {
            panel8.BackgroundImage = global::PULSR_3.Properties.Resources.IdleButton;
        }

        public void startClick(object sender, MouseEventArgs e)
        {
            timer.Stop(); // Stop the timer if it is already running

            angle = 270;
            score = 0;
            //cycleCount += 1;
            //textBox1.Text = cycleCount.ToString();
            timer.Start(); // Start the timer to begin the animation


            // Initiate the filename to log parameters //
            fileName = "sessions_files/" + (cycleCount + 1) + ".csv";   ///Create the file for that session in session folder

            if (File.Exists(fileName))   // Check if it existed before
            {
                File.Delete(fileName);
            }

            using (StreamWriter file = new StreamWriter(fileName, true))
            {
                file.WriteLine("THRESHOLD, SCORE, UPPER ANGLE, UPPER THRESHOLD, UPPER_LOAD_CELL, LOWER ANGLE, LOWER THRESHOLD, LOWER_LOAD_CELL, DEP, DEM, TIMESTAMP, SAMPLE_NO");
                //Do not close the file yet, close after one cycle in ...//

                file.Close();
            }
        }

        ////////// RESET Button///
        private void resetMouseEnter(object sender, EventArgs e)
        {
            panel9.BackgroundImage = global::PULSR_3.Properties.Resources.hoverButton;
        }

        private void resetMouseLeave(object sender, EventArgs e)
        {
            panel9.BackgroundImage = global::PULSR_3.Properties.Resources.IdleButton;
        }

        private void resetClick(object sender, MouseEventArgs e)
        {
            button1.ResetText();  // Clear cycle textbox
            angle = 270;
            cycleCount = 0;

            score = 0;
            button3.ResetText();  // Clear score textbox
            timer.Stop();
        }

        ////// INCREASE Button ////
        private void increaseMouseEnter(object sender, EventArgs e)
        {
            panel10.BackgroundImage = global::PULSR_3.Properties.Resources.hoverButton;
        }

        private void increaseMouseLeave(object sender, EventArgs e)
        {
            panel10.BackgroundImage = global::PULSR_3.Properties.Resources.IdleButton;
        }

        private void increaseClick(object sender, MouseEventArgs e)
        {

            button2.Text = Convert.ToString(levelStart += 100);


        }

        ////// DECREASE Button /////
        private void decreaseMouseEnter(object sender, EventArgs e)
        {
            panel11.BackgroundImage = global::PULSR_3.Properties.Resources.hoverButton;
        }
        private void decreaseMouseLeave(object sender, EventArgs e)
        {
            panel11.BackgroundImage = global::PULSR_3.Properties.Resources.IdleButton;
        }
        private void decreaseClick(object sender, MouseEventArgs e)
        {
            if (levelStart > 100)
            {

                button2.Text = Convert.ToString(levelStart -= 100);

            }
        }
        ///////// Orbiting Circle /////////////
        public void orbitPanelPaint(object sender, PaintEventArgs e)
        {
            Pen largeCirclePen = new Pen(Color.FromArgb(0xB0, 0x80, 0x2E), 5.0f);
            float centerX = orbitPanel.Width / 2;
            float centerY = orbitPanel.Height / 2;
            //float orbitingX = centerX + (float)(orbitRadius * Math.Cos(angle)) - centerOffset;
            //float orbitingY = centerY + (float)(orbitRadius * Math.Sin(angle));

            /// Draw the larger circle
            float largeCircleX = centerX - largeCircleRadius;
            float largeCircleY = centerY - largeCircleRadius;
            float largeCircleDiameter = 2 * largeCircleRadius;

            e.Graphics.DrawEllipse(largeCirclePen, largeCircleX, largeCircleY, largeCircleDiameter, largeCircleDiameter);


            /// Draw Small Obiting Circle
            double radians = angle * Math.PI / 180;
             smallCircleX = (int)(centerX + largeCircleRadius * Math.Cos(radians));
             smallCircleY = (int)(centerY + largeCircleRadius * Math.Sin(radians));

            int smallCircleDiameter = 2 * smallCircleRadius;

            int smallCircleXPos = smallCircleX - smallCircleRadius;
            int smallCircleYPos = smallCircleY - smallCircleRadius;

            e.Graphics.FillEllipse(Brushes.Green, smallCircleXPos, smallCircleYPos, smallCircleDiameter, smallCircleDiameter);

            /// Display the coordinates in the terminal ///
            Console.WriteLine("Small Orbiting Circle Coordinates: X = {0}, Y = {1}", smallCircleXPos, smallCircleYPos);

            ///// Draw the small rectangle connected to end effector  /////
            float rectWidth = 20;
            float rectHeight = 20;
            float rectX = centerX - rectWidth / 2;
            //float rectY = centerY - orbitingCircleRadius - rectHeight;
            //e.Graphics.FillRectangle(Brushes.Blue, rectX, rectY, rectWidth, rectHeight);
            //e.Graphics.FillRectangle(Brushes.Blue, 57, 345, rectWidth, rectHeight);

            /// update effector coordinate to give new effector coordinates ///
            //old_x = new_x;
            //old_y = new_y;

            pulsr3.ReturnXYCoordinate();
            //pulsr3.ComputeXY();

            new_x = (xOffset - pulsr3.x);
            new_y = (pulsr3.y - yOffset);

            //new_x = pulsr3.x; // + 306;
            //new_y = pulsr3.y; // + 95;

            current_x = (int)(centerX - new_x);
            current_y = (int)(centerY + new_y);

            e.Graphics.FillRectangle(Brushes.Blue, current_y, current_x, rectWidth, rectHeight);

            //Console.WriteLine("End effector : X = {0}, Y = {1}", new_x, new_y);
            Console.WriteLine("End effector Coordinates: X = {0}, Y = {1}", current_y, current_x);


            /// Scoring Calculations ///
            int xDiff = current_y - smallCircleXPos;
            int yDiff = current_x - smallCircleYPos;

            distance = (int)Math.Sqrt((xDiff * xDiff) + (yDiff * yDiff));
            Console.WriteLine("Distance between .... :" + distance);

            if (distance <= 100)
            {
                if (distance <= 70)
                {
                    //change to green green


                    //update score
                    button3.Text = Convert.ToString(score += 1);
                }
                else
                {
                    // chnage to orange orange
                }
            }
            else
            {
                // change to green purple
            }


            /// Loggging parameter into CSV file ///
            parameterLogging();

            fileName = "sessions_files/" + (cycleCount + 1) + ".csv";
            using (StreamWriter file = new StreamWriter(fileName, true))
            {
                file.WriteLine(1 + "," + score + "," + pulsr3.upper.angle + "," + 2 + "," + pulsr3.upper.link_force + "," + pulsr3.lower.angle + "," + 3 + "," + pulsr3.lower.link_force + "," + DEP + "," + DEM + "," + DateTime.Now.ToString() + "," + 4);
                file.Close();

                //file.WriteLine($"{threshold},{score},{pulsr2.upper.angle},{threshold_upper},{pulsr2.upper.link_force},{pulsr2.lower.angle},{threshold_lower},{pulsr2.lower.link_force},{DEP},{DEM},{DateTime.Now.Ticks},{n}");
            }


        }


        /// Dialog ///

        public static DialogResult InputBox(string title, string promptText, ref int value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(60, 5, 100, 13);
            textBox.SetBounds(75, 23, 50, 20);
            buttonOk.SetBounds(25, 50, 70, 35);
            buttonCancel.SetBounds(100, 50, 70, 35);

            label.AutoSize = true;
            form.ClientSize = new Size(200, 100);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;

            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();

            value = Convert.ToInt32(textBox.Text);
            return dialogResult;



        }


        /// 
        void parameterLogging()
        {
            // compute DEP, TEP, DEM, TEM

            double TEP = (Math.Atan2((centerX - current_x), (current_y - centerY)) * (180 / Math.PI));
            double closest_x = largeCircleRadius * Math.Sin((TEP * (Math.PI / 180)));
            double closest_y = largeCircleRadius * Math.Cos((TEP * (Math.PI / 180)));
            double x = new_x ;
            double y = new_y ;
            TEP = TEP >= 0 ? TEP : 360 + TEP;
            double dx = (closest_x - x);
            double dy = (closest_y - y);
             DEP = ((int)Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)));
            dx = (smallCircleX - current_x) ;
            dy = (smallCircleY - current_y);
             DEM = (int)Math.Sqrt(Math.Pow(dy, 2) + Math.Pow(dx, 2));

            

            /*using (StreamWriter file = new StreamWriter(fileName, true))
            {
                file.WriteLine( 1 + "," + score + "," + pulsr3.upper.angle + "," + 2 + "," + pulsr3.upper.link_force + "," + pulsr3.lower.angle + "," + 3 + "," + pulsr3.lower.link_force + "," + DEP + "," + DEM + "," + DateTime.Now.ToString() + "," + 4);
                file.Close();

                //file.WriteLine($"{threshold},{score},{pulsr2.upper.angle},{threshold_upper},{pulsr2.upper.link_force},{pulsr2.lower.angle},{threshold_lower},{pulsr2.lower.link_force},{DEP},{DEM},{DateTime.Now.Ticks},{n}");
            }*/

        }
    }
}


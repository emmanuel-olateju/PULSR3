namespace PULSR_3
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.level_label = new System.Windows.Forms.Label();
            this.score_label = new System.Windows.Forms.Label();
            this.cycle_label = new System.Windows.Forms.Label();
            this.level_button = new System.Windows.Forms.Button();
            this.cycle_button = new System.Windows.Forms.Button();
            this.score_button = new System.Windows.Forms.Button();
            this.start_button = new System.Windows.Forms.Button();
            this.end_effector = new System.Windows.Forms.PictureBox();
            this.app_title = new System.Windows.Forms.Label();
            this.emergency_stop = new System.Windows.Forms.Button();
            this.workspace = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.end_effector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.workspace)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Interval = 50;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // level_label
            // 
            this.level_label.AutoSize = true;
            this.level_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.level_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.level_label.Location = new System.Drawing.Point(13, 58);
            this.level_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.level_label.Name = "level_label";
            this.level_label.Size = new System.Drawing.Size(74, 25);
            this.level_label.TabIndex = 0;
            this.level_label.Text = "LEVEL";
            // 
            // score_label
            // 
            this.score_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.score_label.AutoSize = true;
            this.score_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.score_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.score_label.Location = new System.Drawing.Point(13, 164);
            this.score_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.score_label.Name = "score_label";
            this.score_label.Size = new System.Drawing.Size(83, 25);
            this.score_label.TabIndex = 1;
            this.score_label.Text = "SCORE";
            // 
            // cycle_label
            // 
            this.cycle_label.AutoSize = true;
            this.cycle_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cycle_label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.cycle_label.Location = new System.Drawing.Point(13, 110);
            this.cycle_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.cycle_label.Name = "cycle_label";
            this.cycle_label.Size = new System.Drawing.Size(93, 25);
            this.cycle_label.TabIndex = 2;
            this.cycle_label.Text = "CYCLES";
            // 
            // level_button
            // 
            this.level_button.BackColor = System.Drawing.Color.Black;
            this.level_button.Enabled = false;
            this.level_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.level_button.ForeColor = System.Drawing.Color.White;
            this.level_button.Location = new System.Drawing.Point(118, 54);
            this.level_button.Margin = new System.Windows.Forms.Padding(4);
            this.level_button.Name = "level_button";
            this.level_button.Size = new System.Drawing.Size(100, 36);
            this.level_button.TabIndex = 3;
            this.level_button.Text = "0";
            this.level_button.UseVisualStyleBackColor = false;
            this.level_button.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.level_PreviewKeyDown);
            // 
            // cycle_button
            // 
            this.cycle_button.BackColor = System.Drawing.Color.Black;
            this.cycle_button.Enabled = false;
            this.cycle_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cycle_button.ForeColor = System.Drawing.Color.White;
            this.cycle_button.Location = new System.Drawing.Point(118, 107);
            this.cycle_button.Margin = new System.Windows.Forms.Padding(4);
            this.cycle_button.Name = "cycle_button";
            this.cycle_button.Size = new System.Drawing.Size(100, 34);
            this.cycle_button.TabIndex = 4;
            this.cycle_button.Text = "0";
            this.cycle_button.UseVisualStyleBackColor = false;
            this.cycle_button.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cyccle_PreviewKeyDown);
            // 
            // score_button
            // 
            this.score_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.score_button.BackColor = System.Drawing.Color.Black;
            this.score_button.Enabled = false;
            this.score_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.score_button.ForeColor = System.Drawing.Color.White;
            this.score_button.Location = new System.Drawing.Point(118, 161);
            this.score_button.Margin = new System.Windows.Forms.Padding(4);
            this.score_button.Name = "score_button";
            this.score_button.Size = new System.Drawing.Size(100, 34);
            this.score_button.TabIndex = 5;
            this.score_button.Text = "0";
            this.score_button.UseMnemonic = false;
            this.score_button.UseVisualStyleBackColor = false;
            this.score_button.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.score_PreviewKeyDown);
            // 
            // start_button
            // 
            this.start_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(41)))));
            this.start_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.start_button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
            this.start_button.Location = new System.Drawing.Point(18, 217);
            this.start_button.Margin = new System.Windows.Forms.Padding(4);
            this.start_button.Name = "start_button";
            this.start_button.Size = new System.Drawing.Size(88, 34);
            this.start_button.TabIndex = 9;
            this.start_button.Text = "Start";
            this.start_button.UseVisualStyleBackColor = false;
            this.start_button.MouseClick += new System.Windows.Forms.MouseEventHandler(this.start_mouse);
            this.start_button.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.start_PreviewKeyDown);
            // 
            // end_effector
            // 
            this.end_effector.BackColor = System.Drawing.Color.White;
            this.end_effector.Location = new System.Drawing.Point(465, 114);
            this.end_effector.Margin = new System.Windows.Forms.Padding(4);
            this.end_effector.Name = "end_effector";
            this.end_effector.Size = new System.Drawing.Size(17, 15);
            this.end_effector.TabIndex = 12;
            this.end_effector.TabStop = false;
            // 
            // app_title
            // 
            this.app_title.AutoSize = true;
            this.app_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.app_title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.app_title.Location = new System.Drawing.Point(422, 2);
            this.app_title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.app_title.Name = "app_title";
            this.app_title.Size = new System.Drawing.Size(120, 31);
            this.app_title.TabIndex = 0;
            this.app_title.Text = "PULSR3";
            // 
            // emergency_stop
            // 
            this.emergency_stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.emergency_stop.BackColor = System.Drawing.Color.DarkRed;
            this.emergency_stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emergency_stop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(249)))), ((int)(((byte)(224)))));
            this.emergency_stop.Location = new System.Drawing.Point(899, 2);
            this.emergency_stop.Margin = new System.Windows.Forms.Padding(4);
            this.emergency_stop.Name = "emergency_stop";
            this.emergency_stop.Size = new System.Drawing.Size(92, 39);
            this.emergency_stop.TabIndex = 2;
            this.emergency_stop.Text = "Kill";
            this.emergency_stop.UseVisualStyleBackColor = false;
            this.emergency_stop.MouseClick += new System.Windows.Forms.MouseEventHandler(this.close_mouse);
            this.emergency_stop.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.close_PreviewKeyDown);
            // 
            // workspace
            // 
            this.workspace.Location = new System.Drawing.Point(301, 58);
            this.workspace.Name = "workspace";
            this.workspace.Size = new System.Drawing.Size(415, 460);
            this.workspace.TabIndex = 13;
            this.workspace.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1006, 721);
            this.Controls.Add(this.app_title);
            this.Controls.Add(this.emergency_stop);
            this.Controls.Add(this.end_effector);
            this.Controls.Add(this.score_button);
            this.Controls.Add(this.start_button);
            this.Controls.Add(this.score_label);
            this.Controls.Add(this.cycle_label);
            this.Controls.Add(this.cycle_button);
            this.Controls.Add(this.level_label);
            this.Controls.Add(this.level_button);
            this.Controls.Add(this.workspace);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.keyisdown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.keyisup);
            ((System.ComponentModel.ISupportInitialize)(this.end_effector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.workspace)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label level_label;
        private System.Windows.Forms.Label score_label;
        private System.Windows.Forms.Label cycle_label;
        private System.Windows.Forms.Button level_button;
        private System.Windows.Forms.Button cycle_button;
        private System.Windows.Forms.Button score_button;
        private System.Windows.Forms.Button start_button;
        private System.Windows.Forms.PictureBox end_effector;
        private System.Windows.Forms.Label app_title;
        private System.Windows.Forms.Button emergency_stop;
        private System.Windows.Forms.PictureBox workspace;
    }
}


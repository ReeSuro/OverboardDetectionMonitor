
namespace DeviceMonitorGUI
{
    partial class MainForm
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
            this.DeviceGridBox = new System.Windows.Forms.DataGridView();
            this.LogBox = new System.Windows.Forms.TextBox();
            this.MainLabel = new System.Windows.Forms.Label();
            this.DeviceIDCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeviceIPCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PortCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RegisteredUserCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeviceStatusCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Battery = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastPingCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.DeviceGridBox)).BeginInit();
            this.SuspendLayout();
            // 
            // DeviceGridBox
            // 
            this.DeviceGridBox.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DeviceIDCol,
            this.DeviceIPCol,
            this.PortCol,
            this.RegisteredUserCol,
            this.DeviceStatusCol,
            this.Battery,
            this.LastPingCol});
            this.DeviceGridBox.Location = new System.Drawing.Point(12, 53);
            this.DeviceGridBox.Name = "DeviceGridBox";
            this.DeviceGridBox.ReadOnly = true;
            this.DeviceGridBox.Size = new System.Drawing.Size(748, 202);
            this.DeviceGridBox.TabIndex = 0;
            // 
            // LogBox
            // 
            this.LogBox.Location = new System.Drawing.Point(12, 274);
            this.LogBox.Multiline = true;
            this.LogBox.Name = "LogBox";
            this.LogBox.Size = new System.Drawing.Size(748, 133);
            this.LogBox.TabIndex = 1;
            // 
            // MainLabel
            // 
            this.MainLabel.AutoSize = true;
            this.MainLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainLabel.Location = new System.Drawing.Point(325, 9);
            this.MainLabel.Name = "MainLabel";
            this.MainLabel.Size = new System.Drawing.Size(138, 25);
            this.MainLabel.TabIndex = 2;
            this.MainLabel.Text = "MOB Monitor";
            // 
            // DeviceIDCol
            // 
            this.DeviceIDCol.HeaderText = "DeviceID";
            this.DeviceIDCol.Name = "DeviceIDCol";
            this.DeviceIDCol.ReadOnly = true;
            // 
            // DeviceIPCol
            // 
            this.DeviceIPCol.HeaderText = "DeviceIP";
            this.DeviceIPCol.Name = "DeviceIPCol";
            this.DeviceIPCol.ReadOnly = true;
            // 
            // PortCol
            // 
            this.PortCol.HeaderText = "Port";
            this.PortCol.Name = "PortCol";
            this.PortCol.ReadOnly = true;
            // 
            // RegisteredUserCol
            // 
            this.RegisteredUserCol.HeaderText = "Registered User";
            this.RegisteredUserCol.Name = "RegisteredUserCol";
            this.RegisteredUserCol.ReadOnly = true;
            // 
            // DeviceStatusCol
            // 
            this.DeviceStatusCol.HeaderText = "Device Status";
            this.DeviceStatusCol.Name = "DeviceStatusCol";
            this.DeviceStatusCol.ReadOnly = true;
            // 
            // Battery
            // 
            this.Battery.HeaderText = "Battery";
            this.Battery.Name = "Battery";
            this.Battery.ReadOnly = true;
            // 
            // LastPingCol
            // 
            this.LastPingCol.HeaderText = "Last Ping";
            this.LastPingCol.Name = "LastPingCol";
            this.LastPingCol.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 419);
            this.Controls.Add(this.MainLabel);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.DeviceGridBox);
            this.Name = "MainForm";
            this.Text = "MOBAL";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DeviceGridBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView DeviceGridBox;
        private System.Windows.Forms.TextBox LogBox;
        private System.Windows.Forms.Label MainLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceIDCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceIPCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PortCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn RegisteredUserCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceStatusCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Battery;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastPingCol;
    }
}


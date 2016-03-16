namespace AutoUpdaterView
{
    partial class UiMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UiMain));
            this.LblMessage = new System.Windows.Forms.Label();
            this.TmrMain = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // LblMessage
            // 
            this.LblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblMessage.BackColor = System.Drawing.Color.Transparent;
            this.LblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblMessage.Location = new System.Drawing.Point(12, 9);
            this.LblMessage.Name = "LblMessage";
            this.LblMessage.Size = new System.Drawing.Size(235, 238);
            this.LblMessage.TabIndex = 0;
            this.LblMessage.Text = "Atualizando...";
            this.LblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TmrMain
            // 
            this.TmrMain.Interval = 5000;
            this.TmrMain.Tick += new System.EventHandler(this.TmrMain_Tick);
            // 
            // UiMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::AutoUpdaterView.Properties.Resources.Update;
            this.ClientSize = new System.Drawing.Size(259, 256);
            this.Controls.Add(this.LblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "UiMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Atualizando";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.UiMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LblMessage;
        private System.Windows.Forms.Timer TmrMain;
    }
}


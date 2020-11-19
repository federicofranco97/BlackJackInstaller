namespace BlackJackInstaller.GUI
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
            this.txtProcess = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnInstallDepends = new System.Windows.Forms.Button();
            this.btnInstallCli = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // txtProcess
            // 
            this.txtProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProcess.Location = new System.Drawing.Point(408, 12);
            this.txtProcess.Multiline = true;
            this.txtProcess.Name = "txtProcess";
            this.txtProcess.Size = new System.Drawing.Size(380, 426);
            this.txtProcess.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(83, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Instalador de BlackJack";
            // 
            // btnInstallDepends
            // 
            this.btnInstallDepends.Location = new System.Drawing.Point(93, 120);
            this.btnInstallDepends.Name = "btnInstallDepends";
            this.btnInstallDepends.Size = new System.Drawing.Size(168, 43);
            this.btnInstallDepends.TabIndex = 2;
            this.btnInstallDepends.Text = "Instalar Dependencias";
            this.btnInstallDepends.UseVisualStyleBackColor = true;
            this.btnInstallDepends.Click += new System.EventHandler(this.btnInstallDepends_Click);
            // 
            // btnInstallCli
            // 
            this.btnInstallCli.Location = new System.Drawing.Point(93, 194);
            this.btnInstallCli.Name = "btnInstallCli";
            this.btnInstallCli.Size = new System.Drawing.Size(168, 43);
            this.btnInstallCli.TabIndex = 2;
            this.btnInstallCli.Text = "Instalar Cliente";
            this.btnInstallCli.UseVisualStyleBackColor = true;
            this.btnInstallCli.Click += new System.EventHandler(this.btnInstallCli_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnInstallCli);
            this.Controls.Add(this.btnInstallDepends);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtProcess);
            this.Name = "Form1";
            this.Text = "Instalador de BlackJack";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtProcess;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnInstallDepends;
        private System.Windows.Forms.Button btnInstallCli;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}


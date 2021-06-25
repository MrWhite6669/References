namespace TaskKiller
{
    partial class addForm
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
            this.curtaskList = new System.Windows.Forms.ListBox();
            this.addConfirmButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // curtaskList
            // 
            this.curtaskList.FormattingEnabled = true;
            this.curtaskList.Location = new System.Drawing.Point(7, 3);
            this.curtaskList.Name = "curtaskList";
            this.curtaskList.Size = new System.Drawing.Size(210, 290);
            this.curtaskList.TabIndex = 0;
            // 
            // addConfirmButton
            // 
            this.addConfirmButton.Location = new System.Drawing.Point(50, 299);
            this.addConfirmButton.Name = "addConfirmButton";
            this.addConfirmButton.Size = new System.Drawing.Size(123, 35);
            this.addConfirmButton.TabIndex = 1;
            this.addConfirmButton.Text = "Add";
            this.addConfirmButton.UseVisualStyleBackColor = true;
            this.addConfirmButton.Click += new System.EventHandler(this.addConfirmButton_Click);
            // 
            // addForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 341);
            this.Controls.Add(this.addConfirmButton);
            this.Controls.Add(this.curtaskList);
            this.MaximizeBox = false;
            this.Name = "addForm";
            this.Text = "Add Task";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.addForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox curtaskList;
        private System.Windows.Forms.Button addConfirmButton;
    }
}
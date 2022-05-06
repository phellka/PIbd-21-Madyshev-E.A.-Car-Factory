
namespace CarFactoryView
{
    partial class FormMessage
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
            this.labelSender = new System.Windows.Forms.Label();
            this.labelBody = new System.Windows.Forms.Label();
            this.labelSubject = new System.Windows.Forms.Label();
            this.labelDateDelivery = new System.Windows.Forms.Label();
            this.textBoxReplyText = new System.Windows.Forms.TextBox();
            this.buttonReply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelSender
            // 
            this.labelSender.AutoSize = true;
            this.labelSender.Location = new System.Drawing.Point(12, 9);
            this.labelSender.Name = "labelSender";
            this.labelSender.Size = new System.Drawing.Size(0, 15);
            this.labelSender.TabIndex = 0;
            // 
            // labelBody
            // 
            this.labelBody.AutoSize = true;
            this.labelBody.Location = new System.Drawing.Point(12, 115);
            this.labelBody.Name = "labelBody";
            this.labelBody.Size = new System.Drawing.Size(0, 15);
            this.labelBody.TabIndex = 1;
            // 
            // labelSubject
            // 
            this.labelSubject.AutoSize = true;
            this.labelSubject.Location = new System.Drawing.Point(12, 47);
            this.labelSubject.Name = "labelSubject";
            this.labelSubject.Size = new System.Drawing.Size(0, 15);
            this.labelSubject.TabIndex = 2;
            // 
            // labelDateDelivery
            // 
            this.labelDateDelivery.AutoSize = true;
            this.labelDateDelivery.Location = new System.Drawing.Point(12, 83);
            this.labelDateDelivery.Name = "labelDateDelivery";
            this.labelDateDelivery.Size = new System.Drawing.Size(0, 15);
            this.labelDateDelivery.TabIndex = 3;
            // 
            // textBoxReplyText
            // 
            this.textBoxReplyText.Font = new System.Drawing.Font("Segoe UI", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxReplyText.Location = new System.Drawing.Point(12, 148);
            this.textBoxReplyText.Name = "textBoxReplyText";
            this.textBoxReplyText.Size = new System.Drawing.Size(349, 96);
            this.textBoxReplyText.TabIndex = 4;
            // 
            // buttonReply
            // 
            this.buttonReply.Location = new System.Drawing.Point(12, 297);
            this.buttonReply.Name = "buttonReply";
            this.buttonReply.Size = new System.Drawing.Size(75, 23);
            this.buttonReply.TabIndex = 5;
            this.buttonReply.Text = "Ответить";
            this.buttonReply.UseVisualStyleBackColor = true;
            this.buttonReply.Click += new System.EventHandler(this.buttonReply_Click);
            // 
            // FormMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 332);
            this.Controls.Add(this.buttonReply);
            this.Controls.Add(this.textBoxReplyText);
            this.Controls.Add(this.labelDateDelivery);
            this.Controls.Add(this.labelSubject);
            this.Controls.Add(this.labelBody);
            this.Controls.Add(this.labelSender);
            this.Name = "FormMessage";
            this.Text = "Письмо";
            this.Load += new System.EventHandler(this.FormMessage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSender;
        private System.Windows.Forms.Label labelBody;
        private System.Windows.Forms.Label labelSubject;
        private System.Windows.Forms.Label labelDateDelivery;
        private System.Windows.Forms.TextBox textBoxReplyText;
        private System.Windows.Forms.Button buttonReply;
    }
}
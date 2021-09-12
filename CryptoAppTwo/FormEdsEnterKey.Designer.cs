namespace CryptoAppTwo
{
    partial class FormEdsEnterKey
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
            this.label_eds_entryKey = new System.Windows.Forms.Label();
            this.btn_confirm_entry = new System.Windows.Forms.Button();
            this.txt_key_file = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_generate_key = new System.Windows.Forms.Button();
            this.btn_loadKeyIV = new System.Windows.Forms.Button();
            this.label_or = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_eds_entryKey
            // 
            this.label_eds_entryKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_eds_entryKey.ForeColor = System.Drawing.Color.Maroon;
            this.label_eds_entryKey.Location = new System.Drawing.Point(5, 9);
            this.label_eds_entryKey.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label_eds_entryKey.Name = "label_eds_entryKey";
            this.label_eds_entryKey.Size = new System.Drawing.Size(834, 86);
            this.label_eds_entryKey.TabIndex = 79;
            this.label_eds_entryKey.Text = "N\r\nN\r\nN\r\nN";
            // 
            // btn_confirm_entry
            // 
            this.btn_confirm_entry.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_confirm_entry.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Green;
            this.btn_confirm_entry.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btn_confirm_entry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_confirm_entry.Location = new System.Drawing.Point(333, 248);
            this.btn_confirm_entry.Margin = new System.Windows.Forms.Padding(6);
            this.btn_confirm_entry.Name = "btn_confirm_entry";
            this.btn_confirm_entry.Size = new System.Drawing.Size(181, 42);
            this.btn_confirm_entry.TabIndex = 80;
            this.btn_confirm_entry.TabStop = false;
            this.btn_confirm_entry.Text = "Подтвердить";
            this.btn_confirm_entry.UseVisualStyleBackColor = true;
            this.btn_confirm_entry.Click += new System.EventHandler(this.btn_confirm_entry_Click);
            // 
            // txt_key_file
            // 
            this.txt_key_file.BackColor = System.Drawing.Color.White;
            this.txt_key_file.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txt_key_file.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txt_key_file.Location = new System.Drawing.Point(9, 128);
            this.txt_key_file.Name = "txt_key_file";
            this.txt_key_file.ReadOnly = true;
            this.txt_key_file.Size = new System.Drawing.Size(830, 26);
            this.txt_key_file.TabIndex = 32767;
            this.txt_key_file.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(5, 103);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(222, 20);
            this.label1.TabIndex = 84;
            this.label1.Text = "Выбранный файл с ключом:";
            // 
            // btn_generate_key
            // 
            this.btn_generate_key.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_generate_key.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_generate_key.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_generate_key.Location = new System.Drawing.Point(640, 163);
            this.btn_generate_key.Name = "btn_generate_key";
            this.btn_generate_key.Size = new System.Drawing.Size(199, 75);
            this.btn_generate_key.TabIndex = 86;
            this.btn_generate_key.TabStop = false;
            this.btn_generate_key.Text = "Сгенерировать\r\nпубличный и секретный\r\nключ";
            this.btn_generate_key.UseVisualStyleBackColor = true;
            this.btn_generate_key.Click += new System.EventHandler(this.btn_generate_key_Click);
            // 
            // btn_loadKeyIV
            // 
            this.btn_loadKeyIV.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_loadKeyIV.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Green;
            this.btn_loadKeyIV.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btn_loadKeyIV.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_loadKeyIV.Location = new System.Drawing.Point(15, 169);
            this.btn_loadKeyIV.Margin = new System.Windows.Forms.Padding(6);
            this.btn_loadKeyIV.Name = "btn_loadKeyIV";
            this.btn_loadKeyIV.Size = new System.Drawing.Size(173, 61);
            this.btn_loadKeyIV.TabIndex = 88;
            this.btn_loadKeyIV.TabStop = false;
            this.btn_loadKeyIV.Text = "Загрузить ключ\r\nиз файла";
            this.btn_loadKeyIV.UseVisualStyleBackColor = true;
            this.btn_loadKeyIV.Click += new System.EventHandler(this.btn_loadKeyIV_Click);
            // 
            // label_or
            // 
            this.label_or.AutoSize = true;
            this.label_or.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_or.ForeColor = System.Drawing.Color.Maroon;
            this.label_or.Location = new System.Drawing.Point(379, 190);
            this.label_or.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label_or.Name = "label_or";
            this.label_or.Size = new System.Drawing.Size(83, 20);
            this.label_or.TabIndex = 32768;
            this.label_or.Text = "← ИЛИ →";
            // 
            // FormEdsEnterKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 305);
            this.Controls.Add(this.label_or);
            this.Controls.Add(this.btn_loadKeyIV);
            this.Controls.Add(this.btn_generate_key);
            this.Controls.Add(this.txt_key_file);
            this.Controls.Add(this.btn_confirm_entry);
            this.Controls.Add(this.label_eds_entryKey);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FormEdsEnterKey";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ЭЦП: Ввод ключа (Key)";
            this.Load += new System.EventHandler(this.Form4_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label_eds_entryKey;
        private System.Windows.Forms.Button btn_confirm_entry;
        private System.Windows.Forms.TextBox txt_key_file;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_generate_key;
        private System.Windows.Forms.Button btn_loadKeyIV;
        private System.Windows.Forms.Label label_or;
    }
}
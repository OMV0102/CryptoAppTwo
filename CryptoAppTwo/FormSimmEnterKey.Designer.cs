namespace CryptoAppTwo
{
    partial class FormSimmEnterKey
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSimmEnterKey));
            this.label_simm_entryKeyIV = new System.Windows.Forms.Label();
            this.btn_confirm_entry = new System.Windows.Forms.Button();
            this.txt_key = new System.Windows.Forms.TextBox();
            this.txt_iv = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_generate_key = new System.Windows.Forms.Button();
            this.btn_generate_iv = new System.Windows.Forms.Button();
            this.btn_loadKeyIV = new System.Windows.Forms.Button();
            this.toolTip_LoadKeyIV = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label_simm_entryKeyIV
            // 
            this.label_simm_entryKeyIV.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_simm_entryKeyIV.ForeColor = System.Drawing.Color.Maroon;
            this.label_simm_entryKeyIV.Location = new System.Drawing.Point(5, 4);
            this.label_simm_entryKeyIV.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label_simm_entryKeyIV.Name = "label_simm_entryKeyIV";
            this.label_simm_entryKeyIV.Size = new System.Drawing.Size(693, 148);
            this.label_simm_entryKeyIV.TabIndex = 79;
            this.label_simm_entryKeyIV.Text = resources.GetString("label_simm_entryKeyIV.Text");
            // 
            // btn_confirm_entry
            // 
            this.btn_confirm_entry.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_confirm_entry.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Green;
            this.btn_confirm_entry.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btn_confirm_entry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_confirm_entry.Location = new System.Drawing.Point(263, 233);
            this.btn_confirm_entry.Margin = new System.Windows.Forms.Padding(6);
            this.btn_confirm_entry.Name = "btn_confirm_entry";
            this.btn_confirm_entry.Size = new System.Drawing.Size(181, 42);
            this.btn_confirm_entry.TabIndex = 80;
            this.btn_confirm_entry.TabStop = false;
            this.btn_confirm_entry.Text = "Подтвердить";
            this.btn_confirm_entry.UseVisualStyleBackColor = true;
            this.btn_confirm_entry.Click += new System.EventHandler(this.btn_confirm_entry_Click);
            // 
            // txt_key
            // 
            this.txt_key.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_key.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txt_key.Location = new System.Drawing.Point(78, 158);
            this.txt_key.MaxLength = 64;
            this.txt_key.Name = "txt_key";
            this.txt_key.Size = new System.Drawing.Size(970, 26);
            this.txt_key.TabIndex = 81;
            this.txt_key.TabStop = false;
            this.txt_key.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_key_KeyPress);
            // 
            // txt_iv
            // 
            this.txt_iv.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txt_iv.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txt_iv.Location = new System.Drawing.Point(246, 197);
            this.txt_iv.MaxLength = 32;
            this.txt_iv.Name = "txt_iv";
            this.txt_iv.Size = new System.Drawing.Size(490, 26);
            this.txt_iv.TabIndex = 82;
            this.txt_iv.TabStop = false;
            this.txt_iv.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_iv_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(24, 160);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 20);
            this.label1.TabIndex = 84;
            this.label1.Text = "Ключ:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(24, 200);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(219, 20);
            this.label2.TabIndex = 85;
            this.label2.Text = "Вектор инициализации (IV):";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btn_generate_key
            // 
            this.btn_generate_key.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_generate_key.BackgroundImage")));
            this.btn_generate_key.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_generate_key.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_generate_key.Location = new System.Drawing.Point(1054, 153);
            this.btn_generate_key.Name = "btn_generate_key";
            this.btn_generate_key.Size = new System.Drawing.Size(35, 35);
            this.btn_generate_key.TabIndex = 86;
            this.btn_generate_key.TabStop = false;
            this.btn_generate_key.UseVisualStyleBackColor = true;
            this.btn_generate_key.Click += new System.EventHandler(this.btn_generate_key_Click);
            // 
            // btn_generate_iv
            // 
            this.btn_generate_iv.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_generate_iv.BackgroundImage")));
            this.btn_generate_iv.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_generate_iv.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_generate_iv.Location = new System.Drawing.Point(743, 192);
            this.btn_generate_iv.Name = "btn_generate_iv";
            this.btn_generate_iv.Size = new System.Drawing.Size(35, 35);
            this.btn_generate_iv.TabIndex = 87;
            this.btn_generate_iv.TabStop = false;
            this.btn_generate_iv.UseVisualStyleBackColor = true;
            this.btn_generate_iv.Click += new System.EventHandler(this.btn_generate_iv_Click);
            // 
            // btn_loadKeyIV
            // 
            this.btn_loadKeyIV.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_loadKeyIV.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Green;
            this.btn_loadKeyIV.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btn_loadKeyIV.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_loadKeyIV.Location = new System.Drawing.Point(858, 200);
            this.btn_loadKeyIV.Margin = new System.Windows.Forms.Padding(6);
            this.btn_loadKeyIV.Name = "btn_loadKeyIV";
            this.btn_loadKeyIV.Size = new System.Drawing.Size(173, 51);
            this.btn_loadKeyIV.TabIndex = 88;
            this.btn_loadKeyIV.TabStop = false;
            this.btn_loadKeyIV.Text = "Загрузить ключ и IV из файла";
            this.btn_loadKeyIV.UseVisualStyleBackColor = true;
            this.btn_loadKeyIV.Click += new System.EventHandler(this.btn_loadKeyIV_Click);
            // 
            // toolTip_LoadKeyIV
            // 
            this.toolTip_LoadKeyIV.AutoPopDelay = 20000;
            this.toolTip_LoadKeyIV.InitialDelay = 300;
            this.toolTip_LoadKeyIV.ReshowDelay = 100;
            this.toolTip_LoadKeyIV.ShowAlways = true;
            // 
            // FormSimmEnterKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 288);
            this.Controls.Add(this.btn_loadKeyIV);
            this.Controls.Add(this.btn_generate_iv);
            this.Controls.Add(this.btn_generate_key);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_iv);
            this.Controls.Add(this.txt_key);
            this.Controls.Add(this.btn_confirm_entry);
            this.Controls.Add(this.label_simm_entryKeyIV);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FormSimmEnterKey";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ШИФРОВАНИЕ: Ввод ключа (Key) и вектора инициализации (IV)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label_simm_entryKeyIV;
        private System.Windows.Forms.Button btn_confirm_entry;
        private System.Windows.Forms.TextBox txt_key;
        private System.Windows.Forms.TextBox txt_iv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_generate_key;
        private System.Windows.Forms.Button btn_generate_iv;
        private System.Windows.Forms.Button btn_loadKeyIV;
        private System.Windows.Forms.ToolTip toolTip_LoadKeyIV;
    }
}
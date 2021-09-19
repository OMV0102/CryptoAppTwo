namespace CryptoAppTwo
{
    partial class FormGamEnterKey
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGamEnterKey));
            this.btnKeyConfirm = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnKeyGenerate = new System.Windows.Forms.Button();
            this.btnKeyLoad = new System.Windows.Forms.Button();
            this.toolTip_LoadKeyIV = new System.Windows.Forms.ToolTip(this.components);
            this.btnKeyCancelChanged = new System.Windows.Forms.Button();
            this.btnKeySaveChanged = new System.Windows.Forms.Button();
            this.checkBoxKeyEdit = new System.Windows.Forms.CheckBox();
            this.txtKey = new System.Windows.Forms.RichTextBox();
            this.btnKeyBinary = new System.Windows.Forms.Button();
            this.btnKeyHex = new System.Windows.Forms.Button();
            this.flagKeyIsEdited = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnKeyConfirm
            // 
            this.btnKeyConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKeyConfirm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Green;
            this.btnKeyConfirm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btnKeyConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyConfirm.Location = new System.Drawing.Point(232, 151);
            this.btnKeyConfirm.Margin = new System.Windows.Forms.Padding(6);
            this.btnKeyConfirm.Name = "btnKeyConfirm";
            this.btnKeyConfirm.Size = new System.Drawing.Size(215, 42);
            this.btnKeyConfirm.TabIndex = 80;
            this.btnKeyConfirm.TabStop = false;
            this.btnKeyConfirm.Text = "Подтвердить";
            this.btnKeyConfirm.UseVisualStyleBackColor = true;
            this.btnKeyConfirm.Click += new System.EventHandler(this.btnKeyConfirm_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 25);
            this.label1.TabIndex = 84;
            this.label1.Text = "Ключ:";
            // 
            // btnKeyGenerate
            // 
            this.btnKeyGenerate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnKeyGenerate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKeyGenerate.Location = new System.Drawing.Point(8, 148);
            this.btnKeyGenerate.Name = "btnKeyGenerate";
            this.btnKeyGenerate.Size = new System.Drawing.Size(215, 44);
            this.btnKeyGenerate.TabIndex = 86;
            this.btnKeyGenerate.TabStop = false;
            this.btnKeyGenerate.Text = "Сгенерировать ключ";
            this.btnKeyGenerate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnKeyGenerate.UseVisualStyleBackColor = true;
            this.btnKeyGenerate.Click += new System.EventHandler(this.btnKeyGenerate_Click);
            // 
            // btnKeyLoad
            // 
            this.btnKeyLoad.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKeyLoad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Green;
            this.btnKeyLoad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LimeGreen;
            this.btnKeyLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnKeyLoad.Location = new System.Drawing.Point(8, 149);
            this.btnKeyLoad.Margin = new System.Windows.Forms.Padding(6);
            this.btnKeyLoad.Name = "btnKeyLoad";
            this.btnKeyLoad.Size = new System.Drawing.Size(215, 44);
            this.btnKeyLoad.TabIndex = 88;
            this.btnKeyLoad.TabStop = false;
            this.btnKeyLoad.Text = "Загрузить ключ файла";
            this.btnKeyLoad.UseVisualStyleBackColor = true;
            this.btnKeyLoad.Click += new System.EventHandler(this.btnKeyLoad_Click);
            // 
            // toolTip_LoadKeyIV
            // 
            this.toolTip_LoadKeyIV.AutoPopDelay = 20000;
            this.toolTip_LoadKeyIV.InitialDelay = 300;
            this.toolTip_LoadKeyIV.ReshowDelay = 100;
            this.toolTip_LoadKeyIV.ShowAlways = true;
            // 
            // btnKeyCancelChanged
            // 
            this.btnKeyCancelChanged.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnKeyCancelChanged.BackgroundImage")));
            this.btnKeyCancelChanged.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnKeyCancelChanged.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKeyCancelChanged.FlatAppearance.BorderSize = 0;
            this.btnKeyCancelChanged.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeyCancelChanged.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnKeyCancelChanged.Location = new System.Drawing.Point(306, 14);
            this.btnKeyCancelChanged.Name = "btnKeyCancelChanged";
            this.btnKeyCancelChanged.Size = new System.Drawing.Size(35, 30);
            this.btnKeyCancelChanged.TabIndex = 104;
            this.btnKeyCancelChanged.TabStop = false;
            this.btnKeyCancelChanged.Tag = "";
            this.btnKeyCancelChanged.UseVisualStyleBackColor = true;
            this.btnKeyCancelChanged.Click += new System.EventHandler(this.btnKeyCancelChanged_Click);
            // 
            // btnKeySaveChanged
            // 
            this.btnKeySaveChanged.BackgroundImage = global::CryptoAppTwo.Properties.Resources.discet;
            this.btnKeySaveChanged.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnKeySaveChanged.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKeySaveChanged.FlatAppearance.BorderSize = 0;
            this.btnKeySaveChanged.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeySaveChanged.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnKeySaveChanged.Location = new System.Drawing.Point(346, 15);
            this.btnKeySaveChanged.Name = "btnKeySaveChanged";
            this.btnKeySaveChanged.Size = new System.Drawing.Size(35, 30);
            this.btnKeySaveChanged.TabIndex = 103;
            this.btnKeySaveChanged.TabStop = false;
            this.btnKeySaveChanged.Tag = "";
            this.btnKeySaveChanged.UseVisualStyleBackColor = true;
            this.btnKeySaveChanged.Click += new System.EventHandler(this.btnKeySaveChanged_Click);
            // 
            // checkBoxKeyEdit
            // 
            this.checkBoxKeyEdit.AutoSize = true;
            this.checkBoxKeyEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.checkBoxKeyEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBoxKeyEdit.Location = new System.Drawing.Point(387, 20);
            this.checkBoxKeyEdit.Name = "checkBoxKeyEdit";
            this.checkBoxKeyEdit.Size = new System.Drawing.Size(56, 24);
            this.checkBoxKeyEdit.TabIndex = 102;
            this.checkBoxKeyEdit.Text = "Edit";
            this.checkBoxKeyEdit.UseVisualStyleBackColor = true;
            this.checkBoxKeyEdit.CheckedChanged += new System.EventHandler(this.checkBoxEditKey_CheckedChanged);
            // 
            // txtKey
            // 
            this.txtKey.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtKey.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtKey.DetectUrls = false;
            this.txtKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtKey.Location = new System.Drawing.Point(8, 48);
            this.txtKey.Name = "txtKey";
            this.txtKey.ReadOnly = true;
            this.txtKey.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtKey.ShortcutsEnabled = false;
            this.txtKey.Size = new System.Drawing.Size(435, 92);
            this.txtKey.TabIndex = 105;
            this.txtKey.TabStop = false;
            this.txtKey.Text = "";
            // 
            // btnKeyBinary
            // 
            this.btnKeyBinary.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKeyBinary.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnKeyBinary.Location = new System.Drawing.Point(154, 12);
            this.btnKeyBinary.Name = "btnKeyBinary";
            this.btnKeyBinary.Size = new System.Drawing.Size(50, 30);
            this.btnKeyBinary.TabIndex = 108;
            this.btnKeyBinary.TabStop = false;
            this.btnKeyBinary.Tag = "";
            this.btnKeyBinary.Text = "Bin";
            this.btnKeyBinary.UseVisualStyleBackColor = true;
            this.btnKeyBinary.Click += new System.EventHandler(this.btnKeyBinary_Click);
            // 
            // btnKeyHex
            // 
            this.btnKeyHex.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnKeyHex.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnKeyHex.Location = new System.Drawing.Point(210, 12);
            this.btnKeyHex.Name = "btnKeyHex";
            this.btnKeyHex.Size = new System.Drawing.Size(50, 30);
            this.btnKeyHex.TabIndex = 107;
            this.btnKeyHex.TabStop = false;
            this.btnKeyHex.Tag = "";
            this.btnKeyHex.Text = "Hex";
            this.btnKeyHex.UseVisualStyleBackColor = true;
            this.btnKeyHex.Click += new System.EventHandler(this.btnKeyHex_Click);
            // 
            // flagKeyIsEdited
            // 
            this.flagKeyIsEdited.AutoSize = true;
            this.flagKeyIsEdited.Cursor = System.Windows.Forms.Cursors.Hand;
            this.flagKeyIsEdited.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.flagKeyIsEdited.Location = new System.Drawing.Point(9, -1);
            this.flagKeyIsEdited.Name = "flagKeyIsEdited";
            this.flagKeyIsEdited.Size = new System.Drawing.Size(139, 24);
            this.flagKeyIsEdited.TabIndex = 109;
            this.flagKeyIsEdited.Text = "flagKeyIsEdited";
            this.flagKeyIsEdited.UseVisualStyleBackColor = true;
            this.flagKeyIsEdited.Visible = false;
            this.flagKeyIsEdited.CheckedChanged += new System.EventHandler(this.flagKeyIsEdited_CheckedChanged);
            // 
            // FormGamEnterKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 207);
            this.Controls.Add(this.flagKeyIsEdited);
            this.Controls.Add(this.btnKeyBinary);
            this.Controls.Add(this.btnKeyHex);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.btnKeyCancelChanged);
            this.Controls.Add(this.btnKeySaveChanged);
            this.Controls.Add(this.checkBoxKeyEdit);
            this.Controls.Add(this.btnKeyLoad);
            this.Controls.Add(this.btnKeyGenerate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnKeyConfirm);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FormGamEnterKey";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ШИФРОВАНИЕ: Ввод ключа (Key)";
            this.Load += new System.EventHandler(this.FormGamEnterKey_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnKeyConfirm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnKeyGenerate;
        private System.Windows.Forms.Button btnKeyLoad;
        private System.Windows.Forms.ToolTip toolTip_LoadKeyIV;
        private System.Windows.Forms.Button btnKeyCancelChanged;
        private System.Windows.Forms.Button btnKeySaveChanged;
        private System.Windows.Forms.CheckBox checkBoxKeyEdit;
        private System.Windows.Forms.RichTextBox txtKey;
        private System.Windows.Forms.Button btnKeyBinary;
        private System.Windows.Forms.Button btnKeyHex;
        private System.Windows.Forms.CheckBox flagKeyIsEdited;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Collections;
using System.Text;
using System.Drawing;

namespace CryptoAppTwo
{
    public partial class FormGamEnterKey : Form
    {
        public FormGamEnterKey(ref Button btn, ref Gamirovanie gam)
        {
            InitializeComponent();
            this.btnGamEnterKey = btn;
            this.gamirovanie = gam;
        }

        private Button btnGamEnterKey;
        private Gamirovanie gamirovanie;

        // при ЗАГРУЗКЕ ФОРМЫ 
        private void FormGamEnterKey_Load(object sender, EventArgs e)
        {
            this.flagKeyIsEdited.Visible = false;

            // если раннее были введенны ключи то вывести их на форму
            if (gamirovanie.KeyIsEntry == true)
            {
                // ВЫВЕСТИ КЛЮЧ НА ФОРМУ
                this.txtKey.Text = Functions.ByteToHex(gamirovanie.KeyByte);
            }

            // Подсказка у кнопки загрузки ключа
            this.toolTip_LoadKeyIV.ToolTipTitle = this.btnKeyLoad.Text;
            this.toolTip_LoadKeyIV.ToolTipIcon = ToolTipIcon.Info;
            this.toolTip_LoadKeyIV.SetToolTip(this.btnKeyLoad, "В файле должно быть две строки в 16-ричном виде.\n1-ая строка: Ключ длинной 64 знака.\n2-ая строка: Вектор(IV) длиной 32 знакак.");

            if (Global.Simm_EncryptOrDecrypt) // если загрузили для ШИФРОВАНИЯ
            {
                this.Text = "ШИФРОВАНИЕ: Ввод ключа (Key)";
                // показать кнопки случайной генерации
                this.btnKeyGenerate.Visible = true;
                // скрыть кнопку загрузки ключа
                this.btnKeyLoad.Visible = false;
            }
            else  // если загрузили для РАСШИФРОВКИ
            {
                this.Text = "ДЕШИФРОВАНИЕ: Ввод ключа (Key)";
                // скрыть кнопки случайной генерации
                this.btnKeyGenerate.Visible = false;
                // показать кнопку загрузки ключа
                this.btnKeyLoad.Visible = true;
            }
        }

        // кнопка ПОДТВЕРДИТЬ
        private void btnKeyConfirm_Click(object sender, EventArgs e)
        {
            if(gamirovanie.KeyIsCorrect == true)
            {

            }


            if(txt_key.Text.Length == txt_key.MaxLength)
            {
                if (txt_iv.Text.Length == txt_iv.MaxLength)
                {
                    Global.Simm_byte_key = Functions.StringHexToByteArray(txt_key.Text); // Запомнили ключ
                    Global.Simm_byte_iv = Functions.StringHexToByteArray(txt_iv.Text); // Запомнили IV
                    Global.Simm_KeyIV_isEntry = true;

                    this.btnGamEnterKey.Text = "Изменить ключ (введен)"; // Изменили название кнопки на основной форме
                    this.btnGamEnterKey.ForeColor = Color.FromKnownColor(KnownColor.Green); // Цвет изменили

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Число символов в IV должно быть " + txt_iv.MaxLength.ToString() + "!\nОтредактируйте IV или сгенерируйте случайно.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
            }
            else
            {
                MessageBox.Show("Число символов в ключе должно быть " + txt_key.MaxLength.ToString() + "!\nОтредактируйте ключ или сгенерируйте случайно.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
        }

        // Генерировать ключ
        private void btnKeyGenerate_Click(object sender, EventArgs e)
        {
            if(gamirovanie.TextInByte.Length > 0)
            {
                gamirovanie.KeyByte = Functions.PRNGGenerateByteArray(gamirovanie.TextInByte.Length);
                this.btnKeyHex.PerformClick();
            }
            else
            {
                this.Enabled = false;
                MessageBox.Show("Исходные данные имеют нулевой размер!\nВвод ключа пока не возможен.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gamirovanie.KeyIsEdited = false;
                this.btnGamEnterKey.Text = "Ввести ключ (отсутствует)"; // Изменили название кнопки на основной форме
                this.btnGamEnterKey.ForeColor = Color.FromKnownColor(KnownColor.Black); // Цвет изменили
                this.Enabled = true;
                this.Close();
            }
        }

        // Ввод символа в поле ключа (только 16-ричные символы)
        private void txtKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.checkBoxKeyEdit.Checked == false)
                return;

            if (e.KeyChar == 8 || e.KeyChar == 127) // Backspace или Delete
            {
                e.Handled = false;
                this.flagKeyIsEdited.Checked = true;
            }
            else if (gamirovanie.KeyType == TypeDisplay.Hex && Functions.checkSymbolIsHex(e.KeyChar) == true)
            {
                e.Handled = false;
                if(Functions.checkSymbolaf(e.KeyChar) == true) // если ввели маленькие строчки a-f
                    e.KeyChar = (char)((int)e.KeyChar - 32); // то привести их к верхнему регистру
                this.flagKeyIsEdited.Checked = true;
            }
            else if (gamirovanie.KeyType == TypeDisplay.Binary && Functions.checkSymbolIsBinary(e.KeyChar) == true)
            {
                e.Handled = false;
                this.flagKeyIsEdited.Checked = true;
            }
            else
            {
                e.Handled = true;
            }
        }

        // кнопка ЗАГРУЗИТЬ ключ
        private void btnKeyLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выбрать файл с ключом..."; // Заголовок окна
            ofd.InitialDirectory = Application.StartupPath; // Папка откуда запустили exe

            if (ofd.ShowDialog() == DialogResult.OK) // Если выбрали файл
            {
                // читаем байты из файла
                if (ofd.FileName.Length > 0) // Если путь не нулевой
                {
                    if (File.Exists(ofd.FileName) == true) // Если указанный файл существует
                    {
                        gamirovanie.KeyByte = File.ReadAllBytes(ofd.FileName); // считали
                        this.btnKeyHex.PerformClick(); // вывели в Hex
                    }
                    else
                    {
                        MessageBox.Show("Файла {" + ofd.FileName + "} не существует!", " Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    this.Enabled = false;
                    MessageBox.Show("Указан неверный путь!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Enabled = true;
                    return;
                }
            }
            ofd.Dispose();
        }

        // БИНАРНЫЙ формат
        private void btnKeyBinary_Click(object sender, EventArgs e)
        {
            if (gamirovanie.KeyType == TypeDisplay.Binary) return;

            if (gamirovanie.KeyIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Ключ сейчас в режиме редактирования!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.btnKeyBinary.Focus();
                return;
            }

            if (gamirovanie.KeyByte.Length > 50000)
            {
                this.Enabled = false;
                MessageBox.Show("Количество байтов слишком велико!\n(Больше 50000 байт)\nОтображение в бинарном виде недоступно!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Enabled = true;
                return;
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                this.Enabled = false;
            }

            this.txtKey.Text = Functions.ByteToBinary(gamirovanie.KeyByte);

            gamirovanie.KeyType = TypeDisplay.Binary;
            this.btnKeyBinary.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            this.btnKeyHex.ForeColor = Color.FromKnownColor(KnownColor.Black);

            if (!(gamirovanie.KeyByte.Length > 50000))
            {
                this.Enabled = true;
                this.Cursor = Cursors.Arrow;
            }
        }

        // HEX формат
        private void btnKeyHex_Click(object sender, EventArgs e)
        {
            if (gamirovanie.KeyType == TypeDisplay.Hex) return;

            if (gamirovanie.KeyIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Ключ сейчас в режиме редактирования!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                this.btnKeyHex.Focus();
                return;
            }

            this.txtKey.Text = Functions.ByteToHex(gamirovanie.KeyByte);

            gamirovanie.KeyType = TypeDisplay.Hex;
            this.btnKeyBinary.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnKeyHex.ForeColor = Color.FromKnownColor(KnownColor.Blue);
        }

        // ВКЛ ВЫКЛ редактирование
        private void checkBoxEditKey_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxKeyEdit.Checked == true)
            {
                this.txtKey.ReadOnly = false;
            }
            else
            {
                if (gamirovanie.KeyIsEdited == true)
                {
                    this.Enabled = false;
                    MessageBox.Show("Ключ был изменен!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    this.checkBoxKeyEdit.Checked = true;
                }
                else
                {
                    this.txtKey.ReadOnly = true;
                }
            }
        }

        // ОТМЕНА редактирования
        private void btnKeyCancelChanged_Click(object sender, EventArgs e)
        {
            if (gamirovanie.KeyType == TypeDisplay.Binary)
                this.txtKey.Text = Functions.ByteToBinary(gamirovanie.KeyByte);
            else if (gamirovanie.KeyType == TypeDisplay.Hex)
                this.txtKey.Text = Functions.ByteToHex(gamirovanie.KeyByte);
            else if (gamirovanie.KeyType == TypeDisplay.Symbol)
                this.txtKey.Text = Functions.ByteToSymbol(gamirovanie.KeyByte);

            this.flagKeyIsEdited.Checked = false;
            this.checkBoxKeyEdit.Checked = false;
            if(gamirovanie.KeyByte.Length > 0)
                gamirovanie.KeyIsCorrect = true;
        }

        // СОХРАНИТЬ ИЗМЕНЕНИЯ
        private void btnKeySaveChanged_Click(object sender, EventArgs e)
        {
            if (gamirovanie.KeyIsEdited == true)
            {
                if (gamirovanie.KeyType == TypeDisplay.Binary)
                {
                    if (Functions.checkStringIsBinarySequence(this.txtKey.Text) == true)
                    {
                        gamirovanie.TextInByte = Functions.BinaryToByte(this.txtKey.Text);
                        this.flagKeyIsEdited.Checked = false;
                        this.checkBoxKeyEdit.Checked = false;
                        gamirovanie.KeyIsCorrect = true;
                    }
                    else
                    {
                        this.Enabled = false;
                        MessageBox.Show("Измененный ключ не соответствует бинарному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gamirovanie.KeyIsCorrect = false;
                        this.Enabled = true;
                        return;
                    }
                }
                else if (gamirovanie.KeyType == TypeDisplay.Hex)
                {
                    if (Functions.checkStringIsHexSequence(this.txtKey.Text) == true)
                    {
                        gamirovanie.TextInByte = Functions.HexToByte(this.txtKey.Text);
                        this.flagKeyIsEdited.Checked = false;
                        this.checkBoxKeyEdit.Checked = false;
                        gamirovanie.KeyIsCorrect = true;
                    }
                    else
                    {
                        this.Enabled = false;
                        MessageBox.Show("Измененный ключ не соответствует 16-ричному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gamirovanie.KeyIsCorrect = false;
                        this.Enabled = true;
                        return;
                    }
                }

                //MessageBox.Show("Изменения сохранены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Изменений не было!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btnKeySaveChanged.Visible = false;
                this.btnKeyCancelChanged.Visible = false;
                this.checkBoxKeyEdit.Checked = false;
            }
        }

        // ФЛАГ изменения ключа
        private void flagKeyIsEdited_CheckedChanged(object sender, EventArgs e)
        {
            if (flagKeyIsEdited.Checked == true)
            {
                gamirovanie.KeyIsEdited = true;
                this.btnKeySaveChanged.Visible = true;
                this.btnKeyCancelChanged.Visible = true;
            }
            else
            {
                gamirovanie.KeyIsEdited = false;
                this.btnKeySaveChanged.Visible = false;
                this.btnKeyCancelChanged.Visible = false;
            }
        }
    }
}

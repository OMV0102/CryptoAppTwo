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
    public partial class FormGamScremblerEnterKey : Form
    {
        public FormGamScremblerEnterKey(ref Button btn, ref Gamirovanie gam)
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
            this.flagKeyIsEdited.Checked = false;
            this.checkBoxKeyEdit.Checked = false;
            this.txtKey.ReadOnly = true;
            this.btnKeySaveChanged.Visible = false;
            this.btnKeyCancelChanged.Visible = false;

            this.txtKey.Text = "";

            // если раннее были введенны ключи то вывести их на форму
            if (gamirovanie.KeyIsEntry == true)
            {
                // ВЫВЕСТИ КЛЮЧ НА ФОРМУ
                this.txtKey.Text = Functions.ByteToHex(gamirovanie.KeyByte);
                this.btnKeyConfirm.Enabled = true;
            }
            else
            {
                gamirovanie.KeyByte = new byte[0];
                this.txtKey.Text = "";
                this.btnKeyConfirm.Enabled = false;
            }

            if (gamirovanie.EncryptOrDecrypt == true) // если загрузили для ШИФРОВАНИЯ
            {
                this.Text = "Скремблер: Генерация ключа";
                // показать кнопки случайной генерации
                this.btnKeyGenerate.Visible = true;
                // скрыть кнопку загрузки ключа
                this.btnKeyLoad.Visible = false;
                this.checkBoxKeyEdit.Visible = false;
            }
            else  // если загрузили для РАСШИФРОВКИ
            {
                this.Text = "Скремблер: Ввод ключа";
                // скрыть кнопки случайной генерации
                this.btnKeyGenerate.Visible = false;
                // показать кнопку загрузки ключа
                this.btnKeyLoad.Visible = true;
                this.checkBoxKeyEdit.Visible = true;
            }

            this.btnKeyBinary.PerformClick();
            this.radioBtnScrembler1.PerformClick();
        }

        // кнопка ПОДТВЕРДИТЬ
        private void btnKeyConfirm_Click(object sender, EventArgs e)
        {
            if(this.checkBoxKeyEdit.Checked == true)
            {
                this.Enabled = false;
                MessageBox.Show("Ключ сейчас в режиме редактирования!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (gamirovanie.KeyType == TypeDisplay.Binary)
                    this.btnKeyBinary.Focus();
                else if (gamirovanie.KeyType == TypeDisplay.Hex)
                    this.btnKeyHex.Focus();
                return;
            }

            if (gamirovanie.KeyIsCorrect == false || gamirovanie.KeyByte.Length < 1)
            {
                this.Enabled = false;
                MessageBox.Show("Формат ключа не корректный или ключ имеет нулевой размер!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = false;
                return;
            }
            else if(gamirovanie.KeyIsCorrect == true && gamirovanie.KeyByte.Length == gamirovanie.TextInByte.Length)
            {
                gamirovanie.KeyIsEntry = true;
                this.btnGamEnterKey.Text = "Изменить ключ (введен)"; // Изменили название кнопки на основной форме
                this.btnGamEnterKey.ForeColor = Color.FromKnownColor(KnownColor.Green); // Цвет изменили
                this.Close();
                return;
            }
            else if (gamirovanie.KeyIsCorrect == true && gamirovanie.KeyByte.Length < gamirovanie.TextInByte.Length)
            {
                if(gamirovanie.EncryptOrDecrypt == true)
                {
                    this.Enabled = false;
                    MessageBox.Show("Длина ключа меньше длины сообщений!\nСгенерируйте новый ключ!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Enabled = false;
                    return;
                }
                else
                {
                    this.Enabled = false;
                    DialogResult res = MessageBox.Show("Длина ключа меньше длины сообщений!\nДлина ключа: "+ gamirovanie.KeyByte.Length.ToString() +"\nДлина сообщения: " + gamirovanie.TextOutByte.Length.ToString() + "\n\nПродублировать ключ до длины сообщения?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    this.Enabled = false;

                    if(res == DialogResult.OK)
                    {
                        gamirovanie.KeyByte = Functions.DuplicateKeyToLength(gamirovanie.KeyByte, gamirovanie.TextOutByte.Length);
                        
                        gamirovanie.KeyIsEntry = true;
                        this.btnGamEnterKey.Text = "Изменить ключ (введен)"; // Изменили название кнопки на основной форме
                        this.btnGamEnterKey.ForeColor = Color.FromKnownColor(KnownColor.Green); // Цвет изменили
                        this.Close();
                    }
                    else
                        return;
                }
            }
            else if (gamirovanie.KeyIsCorrect == true && gamirovanie.KeyByte.Length > gamirovanie.TextInByte.Length)
            {
                if (gamirovanie.EncryptOrDecrypt == true)
                {
                    this.Enabled = false;
                    MessageBox.Show("Длина ключа больше длины сообщений!\nДлина ключа: " + gamirovanie.KeyByte.Length.ToString() + "\nДлина сообщения: " + gamirovanie.TextOutByte.Length.ToString() + "", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    return;
                }
                else
                {
                    gamirovanie.KeyIsEntry = true;
                    this.btnGamEnterKey.Text = "Изменить ключ (введен)"; // Изменили название кнопки на основной форме
                    this.btnGamEnterKey.ForeColor = Color.FromKnownColor(KnownColor.Green); // Цвет изменили
                    this.Close();
                }
            }
        }

        // Генерировать ключ
        private void btnKeyGenerate_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.Enabled = false;

            if(gamirovanie.TextInByte.Length > 0)
            {
                // начальное состояние запомнили
                int numberStart = (int) numericNumberStart.Value;
                
                // полином
                /*int[] pol = 
                {
                    Convert.ToInt32(numericX10.Value), 
                    Convert.ToInt32(numericX9.Value), 
                    Convert.ToInt32(numericX8.Value), 
                    Convert.ToInt32(numericX7.Value), 
                    Convert.ToInt32(numericX6.Value), 
                    Convert.ToInt32(numericX5.Value), 
                    Convert.ToInt32(numericX4.Value), 
                    Convert.ToInt32(numericX3.Value), 
                    Convert.ToInt32(numericX2.Value), 
                    Convert.ToInt32(numericX1.Value), 
                    Convert.ToInt32(numericX0.Value), 
                };*/

                int[] pol =
{
                    Convert.ToInt32(numericX0.Value),
                    Convert.ToInt32(numericX1.Value),
                    Convert.ToInt32(numericX2.Value),
                    Convert.ToInt32(numericX3.Value),
                    Convert.ToInt32(numericX4.Value),
                    Convert.ToInt32(numericX5.Value),
                    Convert.ToInt32(numericX6.Value),
                    Convert.ToInt32(numericX7.Value),
                    Convert.ToInt32(numericX8.Value),
                    Convert.ToInt32(numericX9.Value),
                    Convert.ToInt32(numericX10.Value),
                };
                int size = pol.Length;
                int index = size;
                // ищем наибольший разряд
                for (int i = size; i > 0; i--)
                {
                    if (pol[i - 1] == 1)
                    {
                        index = i;
                        break;
                    }
                }
                LFSR lf = new LFSR(pol, index);

                //gamirovanie.KeyByte = Functions.PRNGGenerateByteArray(gamirovanie.TextInByte.Length);
                gamirovanie.KeyType = TypeDisplay.None;
                gamirovanie.KeyIsCorrect = true;
                this.btnKeyHex.PerformClick();
                this.btnKeyConfirm.Enabled = true;
                //this.btnKeyConfirm_Click(null, null);
            }
            else
            {
                this.Enabled = false;
                MessageBox.Show("Исходные данные имеют нулевой размер!\nВвод ключа пока не возможен.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                gamirovanie.KeyIsEdited = false;
                this.btnGamEnterKey.Text = "Ввести ключ (отсутствует)"; // Изменили название кнопки на основной форме
                this.btnGamEnterKey.ForeColor = Color.FromKnownColor(KnownColor.Black); // Цвет изменили
                this.Enabled = true;
                gamirovanie.KeyIsCorrect = false;
                this.btnKeyConfirm.Enabled = false;
                this.Close();
            }
            this.Cursor = Cursors.Arrow;
            this.Enabled = true;
        }

        // Ввод символа в поле ключа 
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
            ofd.Filter = "Keys(*.key)|*.key"; // расширения файла ключа

            if (ofd.ShowDialog() == DialogResult.OK) // Если выбрали файл
            {
                // читаем байты из файла
                if (ofd.FileName.Length > 0) // Если путь не нулевой
                {
                    if (File.Exists(ofd.FileName) == true) // Если указанный файл существует
                    {
                        gamirovanie.KeyByte = File.ReadAllBytes(ofd.FileName); // считали
                        this.btnKeyHex.PerformClick(); // вывели в Hex
                        this.checkBoxKeyEdit.Checked = true;
                        this.flagKeyIsEdited.Checked = true;
                        this.btnKeySaveChanged_Click(null, null);
                        //this.btnKeyHex.PerformClick(); // вывели в Hex
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
                if (gamirovanie.KeyType == TypeDisplay.Binary)
                    this.btnKeyBinary.Focus();
                else if (gamirovanie.KeyType == TypeDisplay.Hex)
                    this.btnKeyHex.Focus();
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
                if(gamirovanie.KeyType == TypeDisplay.Binary)
                    this.btnKeyBinary.Focus();
                else if (gamirovanie.KeyType == TypeDisplay.Hex)
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
                this.btnKeyConfirm.Enabled = false;
            }
            else
            {
                if (gamirovanie.KeyIsEdited == true)
                {
                    this.Enabled = false;
                    MessageBox.Show("Ключ был изменен!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    this.checkBoxKeyEdit.Checked = true;
                    this.btnKeyConfirm.Enabled = false;
                }
                else
                {
                    this.txtKey.ReadOnly = true;
                    this.btnKeyConfirm.Enabled = true;
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
                this.btnKeyConfirm.Enabled = false;
            }
            else
            {
                gamirovanie.KeyIsEdited = false;
                this.btnKeySaveChanged.Visible = false;
                this.btnKeyCancelChanged.Visible = false;
                this.btnKeyConfirm.Enabled = true;
            }
        }

        // ВВОД скремблера 1 по варианту
        private void radioBtnScrembler1_Click(object sender, EventArgs e)
        {
            this.numericX10.Value = 1;
            this.numericX9.Value = 0;
            this.numericX8.Value = 0;
            this.numericX7.Value = 0;
            this.numericX6.Value = 0;
            this.numericX5.Value = 1;
            this.numericX4.Value = 1;
            this.numericX3.Value = 0;
            this.numericX2.Value = 1;
            this.numericX1.Value = 0;
            this.numericX0.Value = 1;
        }

        // ВВОД скремблера 2 по варианту
        private void radioBtnScrembler2_Click(object sender, EventArgs e)
        {
            this.numericX10.Value = 1;
            this.numericX9.Value = 0;
            this.numericX8.Value = 0;
            this.numericX7.Value = 1;
            this.numericX6.Value = 0;
            this.numericX5.Value = 0;
            this.numericX4.Value = 0;
            this.numericX3.Value = 0;
            this.numericX2.Value = 0;
            this.numericX1.Value = 0;
            this.numericX0.Value = 1;
        }

        // изменение начального значения
        private void numericNumberStart_ValueChanged(object sender, EventArgs e)
        {
            int num = (int) this.numericNumberStart.Value;
            string bin = Convert.ToString(num, 2).PadLeft(10, '0');
            labelNumberStartBin.Text = "= (" + bin + ")";
            Point point = labelNumberStartBin2.Location;
            point.X = labelNumberStartBin.Location.X + labelNumberStartBin.Size.Width;
            labelNumberStartBin2.Location = point;
        }
    }
}

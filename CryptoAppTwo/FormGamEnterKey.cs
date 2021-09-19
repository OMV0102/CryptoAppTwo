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



        // при ЗАГРУЗКЕ ФОРМЫ для ввода ключа и IV
        private void FormGamEnterKey_Load(object sender, EventArgs e)
        {

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

            // Инструкция сверху формы
            this.labelCaption.Text = "> Ключом могут быть только 16-ричные цифры (0-9, A-F).\n";
            this.labelCaption.Text += "> Длина ключа должна быть обязательно равна " + txt_key.MaxLength + " знакам!\n\n";
            this.labelCaption.Text += "> В векторе могут быть только 16-ричные цифры (0-9, A-F).\n";
            this.labelCaption.Text += "> Длина должна быть обязательно равна "+ txt_iv.MaxLength + " знакам!\n";
            

            if (Global.Simm_EncryptOrDecrypt) // если загрузили для ШИФРОВАНИЯ
            {
                this.Text = "ШИФРОВАНИЕ: Ввод ключа (Key)";
                // показать кнопки случайной генерации
                this.btnKeyGenerate.Visible = true;
                this.labelCaption.Text += "\n> 🔄 - случайное заполнение ключа и вектора (IV).";
            }
            else  // если загрузили для РАСШИФРОВКИ
            {
                this.Text = "ДЕШИФРОВАНИЕ: Ввод ключа (Key)";
                this.btnKeyGenerate.Visible = false;
            }
        }

        // кнопка ПОДТВЕРДИТЬ
        private void btnKeyConfirm_Click(object sender, EventArgs e)
        {
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
                this.txt
            }
            else
            {

            }
        }

        // Ввод символа в поле ключа (только 16-ричные символы)
        private void txtKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if((e.KeyChar >= 48 && e.KeyChar <= 57) || (e.KeyChar >= 65 && e.KeyChar <= 70) || (e.KeyChar >= 97 && e.KeyChar <= 102) || e.KeyChar == 8 || e.KeyChar == 127)
            {
                //8 - Backspace
                //127 - Delete
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        // кнопка ЗАГРУЗИТЬ ключ и IV из файла
        private void btnKeyLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выбрать файл с ключом и IV ..."; // Заголовок окна
            ofd.InitialDirectory = Application.StartupPath; // Папка проекта

            if (ofd.ShowDialog() == DialogResult.OK) // Если выбрали файл
            {
                // читаем байты из файла
                if (ofd.FileName.Length > 0) // Если путь не нулевой
                {
                    if (File.Exists(ofd.FileName) == true) // Если указанный файл существует
                    {
                        string temp1 = "";
                        string temp2 = "";
                        using (StreamReader sr = new StreamReader(ofd.FileName, Encoding.UTF8))
                        {
                            temp1 = sr.ReadLine();
                            temp2 = sr.ReadLine();
                            if(temp1 == null || temp2 == null || temp1.Length != txt_key.MaxLength || temp2.Length != txt_iv.MaxLength)
                            {
                                MessageBox.Show("Ошибка считывания данных!\nПосмотрите подсказку при наведении на кнопку загрузки.", " Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Выводим в форму считанные данные
                            this.txt_key.Text = temp1;
                            this.txt_iv.Text = temp2;

                        }
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

        }

        // HEX формат
        private void btnKeyHex_Click(object sender, EventArgs e)
        {

        }
    }
}

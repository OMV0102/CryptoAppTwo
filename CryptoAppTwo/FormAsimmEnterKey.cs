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
    public partial class FormAsimmEnterKey : Form
    {
        public FormAsimmEnterKey(string Alg)
        {
            InitializeComponent();
            AlgName = Alg;
        }

        public static string AlgName;
        public Button form1_btn_Asim_entryKey;
        byte[] key;


        // при ЗАГРУЗКЕ ФОРМЫ для ввода ключа Ассим шифрование
        private void Form3_Load(object sender, EventArgs e)
        {
            key = new byte[0];
            this.txt_key_file.Text = "";

            // если раннее были введенны ключи то вывести их на форму
            if (Global.Asim_Keys_isEntry == true)
            {
                this.txt_key_file.Text = Global.Asim_file_key;
                this.key = Global.Asim_byte_key;
            }

            if (Global.Asim_EncryptOrDecrypt) // если загрузили для ШИФРОВАНИЯ
            {
                // Инструкция сверху формы
                this.label_Asim_entryKey.Text = "> Шифровать можно как публичным, так и секретным ключом.\n";
                this.label_Asim_entryKey.Text += "> Выберите файл с одним из ключей и нажмите кнопку Подтвердить.\n";
                this.label_Asim_entryKey.Text += "> Если вы шифруете впервые, можете сгенерировать публичный и приватный ключи.\n";
                // заголовок формы
                this.Text = "ШИФРОВАНИЕ: Ввод ключа (Public/Private Key)";
                // показать кнопки случайно генерации
                this.btn_generate_key.Visible = true;
                this.label_or.Visible = true;
            }
            else  // если загрузили для РАСШИФРОВКИ
            {
                // Инструкция сверху формы
                this.label_Asim_entryKey.Text = "> Расшифровывать можно только секретным ключом.\n";
                this.label_Asim_entryKey.Text += "> Выберите секретный ключ и нажмите кнопку Подтвердить.\n";
                // заголовок формы
                this.Text = "РАСШИФРОВКА: Ввод секретного ключа (Private Key)";
                // скрыть кнопки случайно генерации
                this.btn_generate_key.Visible = false;
                this.label_or.Visible = false;
            }
        }

        // кнопка ПОДТВЕРДИТЬ
        private void btn_confirm_entry_Click(object sender, EventArgs e)
        {
            if(txt_key_file.Text.Length > 0 && (this.key != null || this.key.Length > 0))
            {

                Global.Asim_byte_key = this.key; // Запомнили ключ
                Global.Asim_file_key = this.txt_key_file.Text; // Запомнили путь к ключу
                Global.Asim_Keys_isEntry = true;

                form1_btn_Asim_entryKey.Text = "Изменить ключ (введенн)"; // Изменили название кнопки на основной форме
                form1_btn_Asim_entryKey.ForeColor = Color.FromKnownColor(KnownColor.Green); // Цвет изменили

                this.Close();

            }
            else
            {
                MessageBox.Show("Ключ не введен!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

        // Генерировать ключ
        private void btn_generate_key_Click(object sender, EventArgs e)
        {
            DialogResult res;

            // ждущий режим формы
            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            byte[] keyPrivate = new byte[0];
            byte[] keyPublic = new byte[0];

            if(AlgName == "RSA")
            {
                RSACryptoServiceProvider rsacrypto = new RSACryptoServiceProvider(Global.Asim_size_key_bit);
                keyPrivate = rsacrypto.ExportCspBlob(true); // запомнили приватный ключ
                keyPublic = rsacrypto.ExportCspBlob(false); // запомнили публичный ключ
            }

            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Выберите место и введите название файла (БЕЗ РАСШИРЕНИЯ) для сохранения сгенерированных ключей ...";
                sfd.InitialDirectory = Application.StartupPath;
                sfd.AddExtension = true;  //Добавлять расширение к имени если не указали
                sfd.Filter = "Keys(*.public;*.private)|*.public;*.private"; // Сохранять только c расширением public
                

                res = sfd.ShowDialog();
                if (res == DialogResult.OK)
                {
                    // получаем выбранный файл
                    string filename = sfd.FileName;
                    filename = filename.Remove(filename.LastIndexOf('.'));
                    // сохраняем байты в файл
                    System.IO.File.WriteAllBytes(filename+ ".public", keyPublic);
                    System.IO.File.WriteAllBytes(filename+ ".private", keyPrivate);
                    

                    this.Enabled = false;
                    string message = "Ключи сохранены.\nПубличный ключ записан в файл:\n" +
                        filename + ".public\n" +
                        "Приватный ключ записан в файл:\n" + 
                        filename + ".private\n" +
                        "\nВвести сгенерированный (публичный) ключ сейчас для шифрования?";
                    res = MessageBox.Show(message, "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    this.Enabled = true;
                    
                    if(res == DialogResult.Yes) // Если ответ да (ввести ключ)
                    {
                        this.key = keyPublic;
                        this.txt_key_file.Text = filename + ".public";
                    }
                }
                sfd.Dispose();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "НЕПРЕДВИДЕННАЯ ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // включаем форму
            this.Enabled = true;
            this.Cursor = Cursors.Arrow;
        }

        // кнопка ЗАГРУЗИТЬ ключ
        private void btn_loadKeyIV_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(Global.Asim_EncryptOrDecrypt == true) // Если шифруем
            {
                ofd.Title = "ШИФРОВАНИЕ: Выбрать файл c ключом ..."; // Заголовок окна
                ofd.InitialDirectory = Application.StartupPath; // Папка проекта
                ofd.Filter = "Keys(*.public;*.private)|*.public;*.private"; // расширения public/private
            }
            else
            {
                ofd.Title = "РАСШИФРОВКА: Выбрать файл c секретным ключом ..."; // Заголовок окна
                ofd.InitialDirectory = Application.StartupPath; // Папка проекта
                ofd.Filter = "Keys(*.private)|*.private"; // расширения public/private
            }
            if (ofd.ShowDialog() == DialogResult.OK) // Если выбрали файл
            {
                // читаем байты из файла
                if (ofd.FileName.Length > 0) // Если путь не нулевой
                {
                    if (File.Exists(ofd.FileName) == true) // Если указанный файл существует
                    {
                        byte[] tempKey = new byte[0];
                        tempKey = File.ReadAllBytes(ofd.FileName);
                        if(tempKey == null || tempKey.Length < Global.Asim_size_key_byte)
                        {
                            MessageBox.Show("Ошибка считывания ключа!\n", " Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Выводим в форму считанные данные
                        this.txt_key_file.Text = ofd.FileName;
                        this.key = tempKey;
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
    }
}

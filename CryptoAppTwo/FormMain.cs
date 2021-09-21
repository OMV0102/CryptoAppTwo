using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Collections;

namespace CryptoAppTwo
{
    public partial class FormMain : Form
    {
        private Gamirovanie gamirovanie = null;

        public FormMain()
        {
            InitializeComponent();
        }

        // при ЗАГРУЗКЕ ФОРМЫ
        private void FormMain_Load(object sender, EventArgs e)
        {
            this.tabControlMain.SelectedIndex = 0;

            #region Дефолтные установки других алгоритмов
            this.tabHesh.Parent = null;
            this.tabSimAlg.Parent = null;
            this.tabAsimAlg.Parent = null;
            this.tabEds.Parent = null;

            this.checkBox_autoHesh.Checked = false; // автохэширование выкл по дефолту
            this.btn_clear_Hesh_byte_in_Click(null, null); // очистили входные поля (кнопка очистить)
            this.comboBox_HeshAlg.SelectedIndex = 0; // выбираем по умолчанию первый алгоритм хэширования
            //======================================
            this.comboBox_SimmAlg.SelectedIndex = 0; // выбираем по умолчанию первый алгоритм Симм. шифрования
            this.radioBtn_SimmAlg1.Checked = true; ; // режим шифрования при запуске Симм. шифрования
            this.btn_simm_clear_Click(null, null); // жмем кнопку очистить для Симм. шифрования
            //======================================
            this.comboBox_AsimAlg.SelectedIndex = 0; // выбираем по умолчанию первый алгоритм Асимм. шифрования
            this.radioBtn_AsimAlg1.Checked = true; ; // режим шифрования при запуске Асимм. шифрования
            this.btn_Asim_clear_Click(null, null); // жмем кнопку очистить для Асимм. шифрования
            //======================================
            this.radioBtn_eds1.Checked = true; // при запуске режим подписания документа ЭЦП
            this.btn_eds_clear_Click(null, null); // Очистить всё на ЭЦП при запуске
            #endregion

            gamirovanie = new Gamirovanie();
            this.radioBtnGamEncrypt.Checked = true; ; // режим шифрования при запуске Гамирования
            this.checkBoxGamTextInEdit.Checked = false;
            this.checkBoxGamTextOutEdit.Checked = false;
            this.txtGamTextIn.ReadOnly = true;
            this.txtGamTextOut.ReadOnly = true;
            this.btnGamTextInSaveChanged.Visible = false;
            this.btnGamTextOutSaveChanged.Visible = false;
            this.btnGamTextInCancelChanged.Visible = false;
            this.btnGamTextOutCancelChanged.Visible = false;
            this.btnGamClear.PerformClick(); // жмем кнопку очистить для Гамирования
        }

        #region Функции обработчкики от других методов

        //=============================Хэширование========================================

        // ВЫБОР метода хэширования
        private void comboBox_HeshAlg_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.checkBox_autoHesh.Checked == true)
            {
                this.btn_Hesh_get_Click(null, null); // клик кнопки хэшировать
            }
            
        }

        // кнопка ПРОЧИТАТЬ ИЗ ФАЙЛА при ХЭШИРОВАНИИ
        private void btn_choice_fileinHesh_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выбрать файл ..."; // Заголовок окна
            ofd.InitialDirectory = Application.StartupPath; // Папка проекта

            if (ofd.ShowDialog() == DialogResult.OK) // Если выбрали файл
            {
                // читаем байты из файла
                if (ofd.FileName.Length > 0) // Если путь не нулевой
                {
                    if (File.Exists(ofd.FileName) == true) // Если указанный файл существует
                    {
                        // Считали байты из файла
                        Global.Hesh_byte_in = File.ReadAllBytes(ofd.FileName);
                        this.txt_hesh_byte_in_num.Text = Global.Hesh_byte_in.Length.ToString(); // Вывели кол-во считанный байт
                        this.txt_hesh_file_in.Text = ofd.FileName; // вывели путь в textbox
                        this.toolTip_hesh_file.SetToolTip(this.txt_hesh_file_in, this.txt_hesh_file_in.Text);

                        // если автохэширование вкл
                        if (this.checkBox_autoHesh.Checked == true)
                        {
                            // кнопка хэшировать
                            this.btn_Hesh_get_Click(null, null);
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

        // кнопка ОЧИСТИТЬ у ХЭШИРОВАНИЯ
        private void btn_clear_Hesh_byte_in_Click(object sender, EventArgs e)
        {
            // очистили данные в  входном массиве байт в хешэ
            if (Global.Hesh_byte_in != null)
                Array.Clear(Global.Hesh_byte_in, 0, Global.Hesh_byte_in.Length);
            else
                Global.Hesh_byte_in = new byte[0]; // выделили память под входной массив байт у хэширования
            // Кол-во считанных байт для хэширование 0
            this.txt_hesh_byte_in_num.Text = (0).ToString();
            // Очистили входной файл
            this.txt_hesh_file_in.Text = "";
            this.toolTip_hesh_file.SetToolTip(this.txt_hesh_file_in, this.txt_hesh_file_in.Text);
            // Очистили хеш на форме
            this.txt_Hesh_out.Text = "";

        }

        // кнопка КОПИРОВАТЬ ХЭШ в буффер windows
        private void btn_copy_Hesh_Click(object sender, EventArgs e)
        {
            if (this.txt_Hesh_out.Text.Length > 0)
                Clipboard.SetText(txt_Hesh_out.Text);
        }

        // кнопка ХЭШИРОВАТЬ
        private void btn_Hesh_get_Click(object sender, EventArgs e)
        {
            string selectedAlgHesh = comboBox_HeshAlg.SelectedItem.ToString();
            this.txt_Hesh_out.Text = Algorithms.HeshAlg(Global.Hesh_byte_in, selectedAlgHesh);
        }

        // кнопка СОХРАНИТЬ ХЭШ
        private void btn_Hesh_save_Click(object sender, EventArgs e)
        {
            //Если поле с хэш-функцией пустое
            if (this.txt_Hesh_out.Text.Length < 1)
            {
                this.Enabled = false;
                MessageBox.Show("Поле с хеш-функцией пустое!\nСначала получите хеш.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Application.StartupPath;
            sfd.Filter = "Text files(*.txt)|*.txt"; // Сохранять только как текстовые файлы
            sfd.AddExtension = true;  //Добавить расширение к имени если не указали

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // получаем выбранный файл
                string filename = sfd.FileName;
                // сохраняем текст в файл
                System.IO.File.WriteAllText(filename, txt_Hesh_out.Text);

                this.Enabled = false;
                MessageBox.Show("Хеш записан в файл:\n" + filename, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Enabled = true;
            }
            sfd.Dispose();
        }
        
        //========================Симметричное шифрование=====================================
        
        // кнопка ШИФРОВАТЬ/РАСшифровать Симметрично
        private void btn_SimmEncrypt_Click(object sender, EventArgs e)
        {
            if (this.txt_simm_byte_in_num.Text != "0") // Если входные данные не пусты
            {
                if(Global.Simm_KeyIV_isEntry == true)// Если введен ключ и вектор
                {
                    try
                    {
                        // вызываем функцию шифрования и получаем байты шифра
                        Global.Simm_byte_out = Algorithms.SimmAlg(Global.Simm_byte_in, Global.Simm_byte_key, Global.Simm_byte_iv, comboBox_SimmAlg.SelectedItem.ToString(), Global.Simm_EncryptOrDecrypt);

                        // Вывести выходные байты 
                        if (Global.Simm_EncryptOrDecrypt == true) // Если шифруем
                        {
                            // вывели байты на форму виде 16-ричной строки
                            this.txt_simm_text_out.Text = Functions.ByteArrayTOStringHex(Global.Simm_byte_out);
                        }
                        else // Если расшифровываем
                        {
                            // вывели байты на форму виде строки с кодировкой UTF8
                            this.txt_simm_text_out.Text = Encoding.UTF8.GetString(Global.Simm_byte_out).Replace("\0", "0");
                        }
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message, "НЕПРЕДВИДЕННАЯ ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    this.Enabled = false;
                    MessageBox.Show("Сначала укажите ключ и IV!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Enabled = true;
                    return;
                }
            }
            else
            {
                this.Enabled = false;
                MessageBox.Show("Сначала укажите входные данные!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = true;
                return;
            }
        }

        // кнопка режим Симметричного Шифрования
        private void radioBtn_SimmAlg1_CheckedChanged(object sender, EventArgs e)
        {
            Global.Simm_EncryptOrDecrypt = true;
            this.btn_SimmEncrypt.Text = "🡻 Шифровать 🡻";
            this.label_simm_caption1.Text = "Входные данные";
            this.label_simm_caption2.Text = "Зашифрованные данные";
            this.label_simm_onText_out.Text = "Примерный вид зашифрованных данных:";
            this.label_simm_underText_out.Text = "(В файл шифр сохраниться в виде бинарных данных,\n но с таким же расширением, что и входной файл)";
            this.btn_simm_saveData.Text = "Сохранить шифр в файл";
            this.btn_choice_fileinSimm.Text = "Выбрать файл с данными";
            btn_simm_clear_Click(null, null); // Очистить всё при переключении
        }

        // кнопка режим Симметричной Расшифровки
        private void radioBtn_SimmAlg2_CheckedChanged(object sender, EventArgs e)
        {
            Global.Simm_EncryptOrDecrypt = false;
            this.btn_SimmEncrypt.Text = "🡻 Расшифровать 🡻";
            this.label_simm_caption1.Text = "Зашифрованные данные";
            this.label_simm_caption2.Text = "Расшифрованные данные"; 
            this.label_simm_onText_out.Text = "Расшифрованные данные:";
            this.label_simm_underText_out.Text = "(В файл данные сохраняться в виде байт, но при открытие\n файл будет отображаться корректно так как будет сохранен\n с таким же расширеним, что и шифрованный файл)";
            this.btn_simm_saveData.Text = "Сохранить данные в файл";
            this.btn_choice_fileinSimm.Text = "Выбрать файл с шифром";
            btn_simm_clear_Click(null, null); // Очистить всё при переключении
        }

        // кнопка ПРОЧИТАТЬ ИЗ ФАЙЛА при СИММ. Шифровании
        private void btn_choice_fileinSimm_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выбрать файл ..."; // Заголовок окна
            ofd.InitialDirectory = Application.StartupPath; // Папка проекта

            if (ofd.ShowDialog() == DialogResult.OK) // Если выбрали файл
            {
                // читаем байты из файла
                if (ofd.FileName.Length > 0) // Если путь не нулевой
                {
                    if (File.Exists(ofd.FileName) == true) // Если указанный файл существует
                    {
                        // очистили ВЫходные байты
                        Global.Simm_byte_out = new byte[0];
                        this.txt_simm_text_out.Text = "";
                        // Считали байты из файла
                        Global.Simm_byte_in = File.ReadAllBytes(ofd.FileName);
                        this.txt_simm_byte_in_num.Text = Global.Simm_byte_in.Length.ToString(); // Вывели кол-во считанных байт
                        this.txt_simm_file_in.Text = ofd.FileName; // вывели путь к файлу в textbox
                        this.toolTip_simm_file.SetToolTip(this.txt_simm_file_in, this.txt_simm_file_in.Text); // текст подсказки запомнили
                        this.txt_simm_text_in.Text = Encoding.UTF8.GetString(Global.Simm_byte_in).Replace('\0', '0'); // вывели на форму считанное в кодировке UTF8
                        Global.Simm_file_extension = ofd.SafeFileName.Substring(ofd.SafeFileName.LastIndexOf('.'));  // Запомнили расширение считанного файла
                    }
                    else
                    {
                        this.Enabled = false;
                        MessageBox.Show("Файла {" + ofd.FileName + "} не существует!", " Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
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

        // кнопка Ввод ключа и IV
        private void btn_simm_entryKeyIV_Click(object sender, EventArgs e)
        {
            FormSimmEnterKey form = new FormSimmEnterKey(comboBox_SimmAlg.SelectedItem.ToString());
            form.Owner = this;
            form.form1_btn_simm_entryKeyIV = this.btn_simm_entryKeyIV; // передали ссылку на управление кнопкой
            form.ShowDialog(this);
        }

        // если меняем алгоритм СИММетричного ШИФРОВАНИЯ
        private void comboBox_SimmAlg_SelectedIndexChanged(object sender, EventArgs e)
        {
            //========очистка ключа======
            // меняем кнопку ввод ключа на обычную
            this.btn_simm_entryKeyIV.Text = "Ввести ключ и IV (отсутствуют)";
            this.btn_simm_entryKeyIV.ForeColor = Color.FromKnownColor(KnownColor.Black);
            // очищаем ключ и вектор
            Global.Simm_byte_key = new byte[0];
            Global.Simm_byte_iv = new byte[0];
            // флаг меняем что не введенны
            Global.Simm_KeyIV_isEntry = false;
            //===================================
        }

        // кнопка ОЧИСТИТЬ у СИММетричного ШИФРОВАНИЯ
        private void btn_simm_clear_Click(object sender, EventArgs e)
        {
            //========очистка ключа======
            // меняем кнопку ввод ключа на обычную
            this.btn_simm_entryKeyIV.Text = "Ввести ключ и IV (отсутствуют)";
            this.btn_simm_entryKeyIV.ForeColor = Color.FromKnownColor(KnownColor.Black);
            // очищаем ключ и вектор
            Global.Simm_byte_key = new byte[0];
            Global.Simm_byte_iv = new byte[0];
            // флаг меняем что не введенны
            Global.Simm_KeyIV_isEntry = false;
            //===================================
            // входные данные стираем
            Global.Simm_byte_in = new byte[0];
            this.txt_simm_text_in.Text = "";
            this.txt_simm_file_in.Text = "";
            this.txt_simm_byte_in_num.Text = "0";
            // ВЫходные данные стираем
            Global.Simm_byte_out = new byte[0];
            this.txt_simm_text_out.Text = "";
            // очистили расширение входного файла
            Global.Simm_file_extension = "";
        }

        // Сохранить ключ IV в файл у СИММ шифрования
        private void btn_simm_saveKeyIV_Click(object sender, EventArgs e)
        {
            try
            {
                //Если ключ или IV  пусты 
                if (Global.Simm_byte_key.Length == 0 || Global.Simm_byte_iv.Length == 0)
                {
                    this.Enabled = false;
                    MessageBox.Show("Ключ или IV пусты!\nСначала введите ключ и IV.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Enabled = true;
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Выберите папку и введите название файла (БЕЗ расширения) ...";
                sfd.InitialDirectory = Application.StartupPath;
                sfd.Filter = "Text files(*.txt)|*.txt"; // Сохранять только как текстовые файлы
                sfd.AddExtension = true;  //Добавить расширение к имени если не указали
                
                DialogResult res = sfd.ShowDialog();
                if (res == DialogResult.OK)
                {
                    // получаем выбранный файл
                    string filename = sfd.FileName;
                    // массив с двумя строками с ключом и IV
                    string[] KeyAndIV = new string[2]
                    {
                        Functions.ByteArrayTOStringHex(Global.Simm_byte_key),
                        Functions.ByteArrayTOStringHex(Global.Simm_byte_iv)
                    };
                    // сохраняем байты в файл
                    File.WriteAllLines(filename, KeyAndIV, Encoding.UTF8);

                    this.Enabled = false;
                    MessageBox.Show("Ключ и IV записаны в файл:\n" + filename, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Enabled = true;
                }
                sfd.Dispose();
            }
            catch(Exception error)
            {
                MessageBox.Show(error.Message, "НЕПРЕДВИДЕННАЯ ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        // Сохранить шифр/текст в файл у СИММ шифрования
        private void btn_simm_saveData_Click(object sender, EventArgs e)
        {
            try
            {
                //Если выходные байты пусты 
                if (Global.Simm_byte_out.Length == 0)
                {
                    this.Enabled = false;
                    if (Global.Simm_EncryptOrDecrypt == true)
                        MessageBox.Show("Сначала зашифруйте данные!\nЗатем можете сохранить полученный шифр.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        MessageBox.Show("Сначала расшифруйте шифр!\nЗатем можете сохранить полученные данные.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Выберите папку и введите название файла (БЕЗ расширения) ...";
                sfd.InitialDirectory = Application.StartupPath;
                sfd.Filter = "Files(*" + Global.Simm_file_extension + ")|*" + Global.Simm_file_extension; // Сохранять только c расширением как и у входного файла
                sfd.AddExtension = true;  //Добавить расширение к имени если не указали

                DialogResult res = sfd.ShowDialog();
                if (res == DialogResult.OK)
                {
                    // получаем выбранный файл
                    string filename = sfd.FileName;
                    // сохраняем байты в файл
                    System.IO.File.WriteAllBytes(filename, Global.Simm_byte_out);

                    this.Enabled = false;
                    if (Global.Simm_EncryptOrDecrypt == true)
                        MessageBox.Show("Шифр записан в файл ЗАПИСАН в файл:\n" + filename, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Расшифрованное сообщение записано в файл:\n" + filename, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Enabled = true;
                }
                sfd.Dispose();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "НЕПРЕДВИДЕННАЯ ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        //=========================Асимметричное шифрование==================================================

        // кнопка ШИФРОВАТЬ/РАСшифровать Асимметрично
        private void btn_AsimEncrypt_Click(object sender, EventArgs e)
        {
            if (this.txt_Asim_byte_in_num.Text != "0")  // Если входные данные не пусты
            {
                if (Global.Asim_Keys_isEntry == true)  // Если введен ключ
                {
                    try
                    {
                        // шифруем/расшифровываем
                        Global.Asim_byte_out = Algorithms.AsimAlg(Global.Asim_byte_in, Global.Asim_byte_key, this.comboBox_AsimAlg.SelectedItem.ToString(), Global.Asim_EncryptOrDecrypt);

                        // Вывести выходные байты 
                        if (Global.Asim_EncryptOrDecrypt == true) // Если шифруем
                        {
                            // вывели байты на форму виде 16-ричной строки
                            this.txt_Asim_text_out.Text = Functions.ByteArrayTOStringHex(Global.Asim_byte_out);
                        }
                        else // Если расшифровываем
                        {
                            // вывели байты на форму виде строки с кодировкой UTF8
                            this.txt_Asim_text_out.Text = Encoding.UTF8.GetString(Global.Asim_byte_out).Replace("\0", "0");
                        }
                    }
                    catch (Exception error)
                    {
                        this.Enabled = false;
                        MessageBox.Show(error.Message, "НЕПРЕДВИДЕННАЯ ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
                        return;
                    }
                }
                else
                {
                    this.Enabled = false;
                    MessageBox.Show("Сначала укажите ключ!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Enabled = true;
                    return;
                }
            }
            else
            {
                this.Enabled = false;
                MessageBox.Show("Сначала укажите входные данные!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Enabled = true;
                return;
            }
        }

        // кнопка режим Асимметричного Шифрования
        private void radioBtn_AsimAlg1_CheckedChanged(object sender, EventArgs e)
        {
            Global.Asim_EncryptOrDecrypt = true;
            this.btn_AsimEncrypt.Text = "🡻 Шифровать 🡻";
            this.label_Asim_caption1.Text = "Входные данные";
            this.label_Asim_caption2.Text = "Зашифрованные данные";
            this.label_Asim_onText_out.Text = "Примерный вид зашифрованных данных:";
            this.label_Asim_underText_out.Text = "(В файл шифр сохраниться в виде бинарных данных,\n но с таким же расширением, что и входной файл)";
            this.btn_Asim_saveData.Text = "Сохранить шифр в файл";
            this.btn_choice_fileinAsim.Text = "Выбрать файл с данными";
            btn_Asim_clear_Click(null, null); // Очистить всё при переключении
        }

        // кнопка режим Асимметричной Расшифровки
        private void radioBtn_AsimAlg2_CheckedChanged(object sender, EventArgs e)
        {
            Global.Asim_EncryptOrDecrypt = false;
            this.btn_AsimEncrypt.Text = "🡻 Расшифровать 🡻";
            this.label_Asim_caption1.Text = "Зашифрованные данные";
            this.label_Asim_caption2.Text = "Расшифрованные данные";
            this.label_Asim_onText_out.Text = "Расшифрованные данные:";
            this.label_Asim_underText_out.Text = "(В файл данные сохраняться в виде байт, но при открытие\n файл будет отображаться корректно так как будет сохранен\n с таким же расширеним, что и шифрованный файл)";
            this.btn_Asim_saveData.Text = "Сохранить данные в файл";
            this.btn_choice_fileinAsim.Text = "Выбрать файл с шифром";
            btn_Asim_clear_Click(null, null); // Очистить всё при переключении
        }

        // кнопка ПРОЧИТАТЬ ИЗ ФАЙЛА при Асимм. Шифровании
        private void btn_choice_fileinAsim_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выбрать файл с данными ..."; // Заголовок окна
            ofd.InitialDirectory = Application.StartupPath; // Папка откуда запустили exe

            if (ofd.ShowDialog() == DialogResult.OK) // Если выбрали файл
            {
                // читаем байты из файла
                if (ofd.FileName.Length > 0) // Если путь не нулевой
                {
                    if (File.Exists(ofd.FileName) == true) // Если указанный файл существует
                    {
                        // очистили ВЫходные байты
                        Global.Asim_byte_out = new byte[0];
                        this.txt_Asim_text_out.Text = "";
                        // Считали байты из файла
                        Global.Asim_byte_in = File.ReadAllBytes(ofd.FileName);
                        this.txt_Asim_byte_in_num.Text = Global.Asim_byte_in.Length.ToString(); // Вывели кол-во считанных байт
                        this.txt_Asim_file_in.Text = ofd.FileName; // вывели путь к файлу в textbox
                        this.toolTip_Asim_file.SetToolTip(this.txt_Asim_file_in, this.txt_Asim_file_in.Text); // текст подсказки запомнили
                        this.txt_Asim_text_in.Text = Encoding.UTF8.GetString(Global.Asim_byte_in).Replace('\0', '0'); // вывели на форму считанное в кодировке UTF8
                        Global.Asim_file_extension = ofd.SafeFileName.Substring(ofd.SafeFileName.LastIndexOf('.'));  // Запомнили расширение считанного файла
                    }
                    else
                    {
                        this.Enabled = false;
                        MessageBox.Show("Файла {" + ofd.FileName + "} не существует!", " Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
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

        // кнопка ВВОД КЛЮЧЕЙ
        private void btn_Asim_entryKey_Click(object sender, EventArgs e)
        {
            FormAsimmEnterKey form = new FormAsimmEnterKey(comboBox_AsimAlg.SelectedItem.ToString());
            form.Owner = this;
            form.form1_btn_Asim_entryKey = this.btn_Asim_entryKey; // передали ссылку на управление кнопкой
            this.Enabled = false;
            form.ShowDialog(this);
            this.Enabled = true;
        }

        // если меняем алгоритм Асим ШИФРОВАНИЯ 
        // так как всего один алгоритма (RSA) по факту не используется *для будущих фич*
        private void comboBox_AsimAlg_SelectedIndexChanged(object sender, EventArgs e)
        {
            //========очистка ключа======
            // меняем кнопку ввод ключа на обычную
            this.btn_Asim_entryKey.Text = "Ввести ключ (отсутствует)";
            this.btn_Asim_entryKey.ForeColor = Color.FromKnownColor(KnownColor.Black);
            // очищаем ключ и вектор
            Global.Asim_byte_key = new byte[0];
            Global.Asim_file_key = "";
            // флаг меняем что не введенны
            Global.Asim_Keys_isEntry = false;
            //===================================
        }

        // кнопка ОЧИСТИТЬ у Асимметричного ШИФРОВАНИЯ
        private void btn_Asim_clear_Click(object sender, EventArgs e)
        {
            //========очистка ключа======
            // меняем кнопку ввод ключа на обычную
            this.btn_Asim_entryKey.Text = "Ввести ключ (отсутствует)";
            this.btn_Asim_entryKey.ForeColor = Color.FromKnownColor(KnownColor.Black);
            // очищаем ключ и его файл
            Global.Asim_byte_key = new byte[0];
            Global.Asim_file_key = "";
            // флаг меняем что не введенны
            Global.Asim_Keys_isEntry = false;
            //===================================
            // входные данные стираем
            Global.Asim_byte_in = new byte[0];
            this.txt_Asim_text_in.Text = "";
            this.txt_Asim_file_in.Text = "";
            this.txt_Asim_byte_in_num.Text = "0";
            // ВЫходные данные стираем
            Global.Asim_byte_out = new byte[0];
            this.txt_Asim_text_out.Text = "";
            // очистили расширение входного файла
            Global.Asim_file_extension = "";
        }

        // Сохранить шифр/текст в файл у Асимм шифрования
        private void btn_Asim_saveData_Click(object sender, EventArgs e)
        {
            try
            {
                //Если выходные байты пусты 
                if (Global.Asim_byte_out.Length == 0)
                {
                    this.Enabled = false;
                    if (Global.Asim_EncryptOrDecrypt == true)
                        MessageBox.Show("Сначала зашифруйте данные!\nЗатем можете сохранить полученный шифр.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        MessageBox.Show("Сначала расшифруйте шифр!\nЗатем можете сохранить полученные данные.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Выберите папку и введите название файла (БЕЗ расширения) ...";
                sfd.InitialDirectory = Application.StartupPath;
                sfd.Filter = "Files(*" + Global.Asim_file_extension + ")|*" + Global.Asim_file_extension; // Сохранять только c расширением как и у входного файла
                sfd.AddExtension = true;  //Добавить расширение к имени если не указали

                DialogResult res = sfd.ShowDialog();
                if (res == DialogResult.OK)
                {
                    // получаем выбранный файл
                    string filename = sfd.FileName;
                    // сохраняем байты в файл
                    System.IO.File.WriteAllBytes(filename, Global.Asim_byte_out);

                    this.Enabled = false;
                    if (Global.Asim_EncryptOrDecrypt == true)
                        MessageBox.Show("Шифр записан в файл:\n" + filename, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Расшифрованное сообщение записано в файл:\n" + filename, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Enabled = true;
                }
                sfd.Dispose();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "НЕПРЕДВИДЕННАЯ ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        //===========================ЭЦП================================================

        // кнопка ПОДПИСАТЬ/ПРОВЕРИТЬ у ЭЦП
        private void btn_edsDO_Click(object sender, EventArgs e)
        {
            if(this.txt_eds_file_in.Text.Length > 0)  // Если входные данные не пусты
            {
                if(Global.eds_Keys_isEntry == true)  // Если введен ключ
                {
                    try
                    {
                        if (Global.eds_signORcheck == true) // если подписание
                        {
                            // подписание
                            Global.eds_data_isSign = false; // не подписано
                            Global.eds_byte_sign = Algorithms.edsAlg_signData(Global.eds_byte_message, Global.eds_byte_key);
                            // смотрим создалось ли или вдруг было исключение
                            if (Global.eds_data_isSign == true)
                            {
                                this.label_eds_result.Text = "Подпись сформирована";
                                this.label_eds_result.ForeColor = Color.Green;
                                this.label_eds_info.Visible = false;
                                this.btn_eds_saveSign.Visible = true;
                            }
                            else // при подписании выскочило исключение и не подписалось
                            {
                                this.label_eds_result.Text = "Подпись НЕ сформирована";
                                this.label_eds_result.ForeColor = Color.Red;
                                this.label_eds_info.Text = "Возможные причины:\n";
                                this.label_eds_info.Text += "> Введен ключ, сгенерированный не этим приложением;\n";
                                this.label_eds_info.Text += "> Введен ключ, который не является приватным;\n";
                                this.label_eds_info.Text += "> Возможно данные слишком велики для подписи их алгоритмом RSA;";
                                this.label_eds_info.Visible = true;
                                this.btn_eds_saveSign.Visible = false;
                            }
                        }
                        else // если проверка
                        {
                            if (this.txt_eds_sign_in.Text.Length > 0)
                            {
                                // проверка подписи
                                Global.eds_data_isCheck = false; // проверка false
                                Global.eds_data_isCheck = Algorithms.edsAlg_verifyData(Global.eds_byte_message, Global.eds_byte_key, Global.eds_byte_sign);
                                // проверяем успешность проверки подписи
                                if (Global.eds_data_isCheck == true)
                                {
                                    this.label_eds_result.Text = "Проверка пройдена";
                                    this.label_eds_result.ForeColor = Color.Green;
                                    this.label_eds_info.Text = "Пояснение:\n";
                                    this.label_eds_info.Text += "> ЭЦП соответствует введенным данным;\n";
                                    this.label_eds_info.Text += "> Данные не были подмененны;\n";
                                    this.label_eds_info.Text += "> Человек, чей ключ введен, именно он подписал эти данные;\n";
                                    this.label_eds_info.Visible = true;
                                    this.btn_eds_saveSign.Visible = false;
                                }
                                else
                                {
                                    // проверку не прошла
                                    this.label_eds_result.Text = "Подпись не прошла проверку";
                                    this.label_eds_result.ForeColor = Color.Red;
                                    this.label_eds_info.Text = "Возможные причины:\n";
                                    this.label_eds_info.Text += "> Неверно введенные данные (возможно их подделали);\n";
                                    this.label_eds_info.Text += "> Введена ошибочно подпись не к этим данным;\n";
                                    this.label_eds_info.Text += "> Введен ошибочно не тот ключ.;";
                                    this.label_eds_info.Visible = true;
                                    this.btn_eds_saveSign.Visible = false;
                                }
                            }
                            else
                            {
                                this.Enabled = false;
                                MessageBox.Show("Укажите файл с подписью!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                this.Enabled = true;
                                return;
                            }
                        }
                    }
                    catch (Exception error)
                    {
                        this.Enabled = false;
                        MessageBox.Show(error.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
                        return;
                    }
                }
                else
                {
                    this.Enabled = false;
                    MessageBox.Show("Сначала укажите файл с ключом!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    return;
                }
            }
            else
            {
                this.Enabled = false;
                MessageBox.Show("Сначала укажите файл с данными!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                return;
            }
        }

        // смена режима: создания ЭЦП
        private void radioBtn_eds1_CheckedChanged(object sender, EventArgs e)
        {
            Global.eds_signORcheck = true; // флаг что подписание
            // Заголовки
            this.label_eds_caption1.Text = "Входные данные";
            this.label_eds_caption2.Text = "Сформированная подпись";
            // Поля для ввода сформированной подписи
            this.label13.Visible = false;
            this.txt_eds_sign_in.Visible = false;
            this.btn_eds_load_eds.Visible = false;
            // кнопка справа главная
            this.btn_edsDO.Text = "🡻 Подписать 🡻";
            btn_eds_clear_Click(null, null); // очистить
        }

        // смена режима: проверки ЭЦП
        private void radioButton_eds2_CheckedChanged(object sender, EventArgs e)
        {
            Global.eds_signORcheck = false;
            // Заголовки
            this.label_eds_caption1.Text = "Входные данные";
            this.label_eds_caption2.Text = "Проверка подписи";
            // Поля для ввода сформированной подписи
            this.label13.Visible = true;
            this.txt_eds_sign_in.Visible = true;
            this.btn_eds_load_eds.Visible = true;
            // кнопка справа главная
            this.btn_edsDO.Text = "🡻 Проверить 🡻";
            btn_eds_clear_Click(null, null); // очистить
        }

        // кнопка ОЧИСТИТЬ у ЭЦП
        private void btn_eds_clear_Click(object sender, EventArgs e)
        {
            //========очистка ключа======
            // меняем кнопку ввод ключа на обычную
            this.btn_eds_entryKey.Text = "Ввести ключ (отсутствует)";
            this.btn_eds_entryKey.ForeColor = Color.Black;
            // очищаем ключ и его файл
            Global.eds_byte_key = new byte[0];
            Global.eds_file_key = "";
            // флаг меняем что не введенн
            Global.eds_Keys_isEntry = false;
            //===================================
            // Подпись не получена false
            Global.eds_data_isSign = false;
            // входные данные стираем
            Global.eds_byte_message = new byte[0];
            this.txt_eds_file_in.Text = "";
            this.txt_eds_sign_in.Text = "";
            // ВЫходные данные стираем
            Global.eds_byte_sign = new byte[0];
            Global.eds_data_isSign = false;
            Global.eds_data_isCheck = false;
            this.label_eds_result.ForeColor = Color.Black;
            this.btn_eds_saveSign.Visible = false;
            if (Global.eds_signORcheck) // если подписание
            {
                // Результаты подписывания обнулить
                this.label_eds_result.Text = "Подпись еще не сформирована";
                this.label_eds_info.Text = "Для создания ЭЦП нужно:\n";
                this.label_eds_info.Text += "> Указать файл с данными;\n";
                this.label_eds_info.Text += "> Ввести секретный ключ;\n";
                this.label_eds_info.Text += "> Нажать кнопку Подписать.";
                
            }
            else // если проверка ЭЦП
            {
                this.label_eds_result.Text = "Подпись еще не проверенна";
                this.label_eds_info.Text = "Для проверки ЭЦП нужно:\n";
                this.label_eds_info.Text += "> Указать файл с данными;\n";
                this.label_eds_info.Text += "> Ввести публичный (или секретный) ключ;\n";
                this.label_eds_info.Text += "> Указать файл с сформированной подписью;\n";
                this.label_eds_info.Text += "> Нажать кнопку Проверить.";
            }
            this.label_eds_info.Visible = true;
        }

        // ввод ключа ЭЦП
        private void btn_eds_entryKey_Click(object sender, EventArgs e)
        {
            FormEdsEnterKey form = new FormEdsEnterKey();
            form.Owner = this;
            form.form1_btn_eds_entryKey = this.btn_eds_entryKey; // передали ссылку на управление кнопкой
            this.Enabled = false;
            form.ShowDialog(this);
            this.Enabled = true;
        }
        
        // кнопка ВЫБРАТЬ ДАННЫЕ
        private void btn_eds_load_in_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выбрать файл с данными ..."; // Заголовок окна
            ofd.InitialDirectory = Application.StartupPath; // Папка откуда запустили exe

            if (ofd.ShowDialog() == DialogResult.OK) // Если выбрали файл
            {
                // читаем байты из файла
                if (ofd.FileName.Length > 0) // Если путь не нулевой
                {
                    if (File.Exists(ofd.FileName) == true) // Если указанный файл существует
                    {
                        // очистили ВЫходные байты // нейтральное положение нужно
                        this.clearBeforeLoadDataORSign();
                        // Считали байты из файла
                        Global.eds_byte_message = File.ReadAllBytes(ofd.FileName);
                        this.txt_eds_file_in.Text = ofd.FileName; // вывели путь к файлу в textbox
                    }
                    else
                    {
                        this.Enabled = false;
                        MessageBox.Show("Указанного файла\n{" + ofd.FileName + "}\nНЕ существует!", " Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
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

        // кнопка ВЫБРАТЬ ПОДПИСЬ
        private void btn_eds_load_eds_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выбрать файл с подписью ..."; // Заголовок окна
            ofd.InitialDirectory = Application.StartupPath; // Папка откуда запустили exe
            ofd.Filter = "EDS(*.)|*.eds"; // Сохранять только c расширением eds

            if (ofd.ShowDialog() == DialogResult.OK) // Если выбрали файл
            {
                // читаем байты из файла
                if (ofd.FileName.Length > 0) // Если путь не нулевой
                {
                    if (File.Exists(ofd.FileName) == true) // Если указанный файл существует
                    {
                        // очистили ВЫходные байты // нейтралное положение нужно
                        this.clearBeforeLoadDataORSign();
                        // Считали байты из файла
                        Global.eds_byte_sign = File.ReadAllBytes(ofd.FileName);
                        this.txt_eds_sign_in.Text = ofd.FileName; // вывели путь к файлу в textbox
                    }
                    else
                    {
                        this.Enabled = false;
                        MessageBox.Show("Указанного файла\n{" + ofd.FileName + "}\nНЕ существует!", " Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
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

        // кнопка СОХРАНИТЬ ПОДПИСЬ
        private void btn_eds_saveSign_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Выберите папку и введите название файла (БЕЗ расширения) ...";
            sfd.InitialDirectory = Application.StartupPath;
            sfd.Filter = "EDS(*.)|*.eds"; // Сохранять только c расширением eds
            sfd.AddExtension = true;  //Добавить расширение к имени если не указали

            DialogResult res = sfd.ShowDialog();
            if (res == DialogResult.OK)
            {
                // получаем выбранный файл
                string filename = sfd.FileName;
                // сохраняем байты в файл
                File.WriteAllBytes(filename, Global.eds_byte_sign);

                this.Enabled = false;
                MessageBox.Show("Подпись записана в файл:\n" + filename, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Enabled = true;
            }
            sfd.Dispose();
        }

        private void clearBeforeLoadDataORSign()
        {
            // ВЫходные данные стираем
            Global.eds_data_isSign = false;
            Global.eds_data_isCheck = false;
            this.label_eds_result.ForeColor = Color.Black;
            this.btn_eds_saveSign.Visible = false;
            if (Global.eds_signORcheck) // если подписание
            {
                Global.eds_byte_sign = new byte[0];
                // Результаты подписывания обнулить
                this.label_eds_result.Text = "Подпись еще не сформирована";
                this.label_eds_info.Text = "Для создания ЭЦП нужно:\n";
                this.label_eds_info.Text += "> Указать файл с данными;\n";
                this.label_eds_info.Text += "> Ввести секретный ключ;\n";
                this.label_eds_info.Text += "> Нажать кнопку Подписать.";

            }
            else // если проверка ЭЦП
            {
                this.label_eds_result.Text = "Подпись еще не проверенна";
                this.label_eds_info.Text = "Для проверки ЭЦП нужно:\n";
                this.label_eds_info.Text += "> Указать файл с данными;\n";
                this.label_eds_info.Text += "> Ввести публичный (или секретный) ключ;\n";
                this.label_eds_info.Text += "> Указать файл с сформированной подписью;\n";
                this.label_eds_info.Text += "> Нажать кнопку Проверить.";
            }
        }


        //=======================================================================
        #endregion

        // кнопка режим Гамирование ШИФРОВАТЬ
        private void radioBtnGamEncrypt_CheckedChanged(object sender, EventArgs e)
        {
            this.btnGamEncryptDecrypt.Text = "🡻 Шифровать 🡻";
            this.labelGamCaptionIn.Text = "Входные данные";
            this.labelGamTextInCaption.Text = "Исходные данные:";
            this.labelGamCaptionOut.Text = "Зашифрованные данные:";
            this.labelGamTextOutCaption.Text = "Шифротекст:";
            this.labelGamTextOutCaptionUnder.Text = "█ В файл шифротекст сохраниться в бинарном виде,\n█ с таким же расширением, что и исходный файл.";
            this.btnGamSaveData.Text = "Сохранить шифротекст в файл";
            this.btnGamChoiceFileIn.Text = "Выбрать файл с данными";
            btnGamClear.PerformClick(); // Очистить всё при переключении
            gamirovanie.EncryptOrDecrypt = true;
            //ВЫКЛЮЧИЛ КНОПКУ РЕДАКТИРОВАНИЯ ВЫХОДА
            this.checkBoxGamTextOutEdit.Visible = false;
        }

        // кнопка режим Гамирование ДЕШИФРОВАТЬ
        private void radioBtnGamDecrypt_CheckedChanged(object sender, EventArgs e)
        {
            this.btnGamEncryptDecrypt.Text = "🡻 Дешифровать 🡻";
            this.labelGamCaptionIn.Text = "Зашифрованные данные";
            this.labelGamCaptionOut.Text = "Дешифрованные данные";
            this.labelGamTextInCaption.Text = "Шифротекст:";
            this.labelGamTextOutCaption.Text = "Дешифрованные данные:";
            this.labelGamTextOutCaptionUnder.Text = "█ Расширение файла будет таким же, как и у файла с\n█ шифротекстом. В файл данные сохраняться байтами.";
            this.btnGamSaveData.Text = "Сохранить данные в файл";
            this.btnGamChoiceFileIn.Text = "Выбрать файл с шифротекстом";
            btnGamClear.PerformClick(); // Очистить всё при переключении
            gamirovanie.EncryptOrDecrypt = false;
            //ВЫКЛЮЧИЛ КНОПКУ РЕДАКТИРОВАНИЯ ВЫХОДА
            this.checkBoxGamTextOutEdit.Visible = false;
        }

        // галочка ВКЛ ВЫКЛ редактирование вход текста
        private void checkBoxGamEditTextIn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxGamTextInEdit.Checked == true)
            {
                    this.txtGamTextIn.ReadOnly = false;
            }
            else
            {
                if (gamirovanie.TextInIsEdited == true)
                {
                    this.Enabled = false;
                    MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    this.checkBoxGamTextInEdit.Checked = true;
                }
                else
                {
                    this.txtGamTextIn.ReadOnly = true;
                }
            }
        }

        // галочка ВКЛ ВЫКЛ редактирование вЫход текста
        private void checkBoxGamEditTextOut_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxGamTextOutEdit.Checked == true)
            {
                this.txtGamTextOut.ReadOnly = false;
            }
            else
            {
                if (gamirovanie.TextOutIsEdited == true)
                {
                    this.Enabled = false;
                    MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    this.checkBoxGamTextOutEdit.Checked = true;
                }
                else
                {
                    this.txtGamTextOut.ReadOnly = true;
                }
            }
        }

        // кнопка Bin вход текста
        private void btnGamTextInBinary_Click(object sender, EventArgs e)
        {
            if (gamirovanie.TextInType == TypeDisplay.Binary) return;

            if(gamirovanie.TextInIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                this.btnGamTextInBinary.Focus();
                return;
            }

            if(gamirovanie.TextInByte.Length > 50000)
            {
                this.Enabled = false;
                MessageBox.Show("Количество байтов слишком велико!\n(Больше 50000 байт)\nОтображение в бинарном виде недоступно!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Enabled = true;
                return;
            }

            this.txtGamTextIn.Text = Functions.ByteToBinary(gamirovanie.TextInByte);

            gamirovanie.TextInType = TypeDisplay.Binary;
            this.btnGamTextInBinary.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            this.btnGamTextInSymbol.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnGamTextInHex.ForeColor = Color.FromKnownColor(KnownColor.Black);
        }

        // кнопка Abc вход текста
        private void btnGamTextInSymbol_Click(object sender, EventArgs e)
        {
            if (gamirovanie.TextInType == TypeDisplay.Symbol) return;

            if (gamirovanie.TextInIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                this.btnGamTextInSymbol.Focus();
                return;
            }

            if (gamirovanie.FileExtension != "txt")
            {
                this.Enabled = false;
                MessageBox.Show("Отображение данных в текстовом виде доступно только для файлов с раширением .txt!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Enabled = true;
                return;
            }

            this.txtGamTextIn.Text = Functions.ByteToSymbol(gamirovanie.TextInByte);

            gamirovanie.TextInType = TypeDisplay.Symbol;
            this.btnGamTextInBinary.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnGamTextInSymbol.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            this.btnGamTextInHex.ForeColor = Color.FromKnownColor(KnownColor.Black);
            
        }

        // кнопка Hex вход текста
        private void btnGamTextInHex_Click(object sender, EventArgs e)
        {
            if (gamirovanie.TextInType == TypeDisplay.Hex) return;

            if (gamirovanie.TextInIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                this.btnGamTextInHex.Focus();
                return;
            }

            this.txtGamTextIn.Text = Functions.ByteToHex(gamirovanie.TextInByte);

            gamirovanie.TextInType = TypeDisplay.Hex;
            this.btnGamTextInBinary.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnGamTextInSymbol.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnGamTextInHex.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            
        }

        // кнопка Bin вЫход текста
        private void btnGamTextOutBinary_Click(object sender, EventArgs e)
        {
            if (gamirovanie.TextOutType == TypeDisplay.Binary) return;

            if (gamirovanie.TextOutIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                this.btnGamTextOutBinary.Focus();
                return;
            }

            if (gamirovanie.TextOutByte.Length > 50000)
            {
                this.Enabled = false;
                MessageBox.Show("Количество байтов слишком велико!\n(Больше 50000 байт)\nОтображение в бинарном виде недоступно!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Enabled = true;
                return;
            }

            this.txtGamTextOut.Text = Functions.ByteToBinary(gamirovanie.TextOutByte);

            gamirovanie.TextOutType = TypeDisplay.Binary;
            this.btnGamTextOutBinary.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            this.btnGamTextOutSymbol.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnGamTextOutHex.ForeColor = Color.FromKnownColor(KnownColor.Black);
        }

        // кнопка Abs вЫход текста 
        private void btnGamTextOutSymbol_Click(object sender, EventArgs e)
        {
            if (gamirovanie.TextOutType == TypeDisplay.Symbol) return;

            if (gamirovanie.TextOutIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                this.btnGamTextOutSymbol.Focus();
                return;
            }

            if (gamirovanie.FileExtension != "txt")
            {
                this.Enabled = false;
                MessageBox.Show("Отображение данных в текстовом виде доступно только для файлов с раширением .txt!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Enabled = true;
                return;
            }

            this.txtGamTextOut.Text = Functions.ByteToSymbol(gamirovanie.TextOutByte);

            gamirovanie.TextOutType = TypeDisplay.Symbol;
            this.btnGamTextOutBinary.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnGamTextOutSymbol.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            this.btnGamTextOutHex.ForeColor = Color.FromKnownColor(KnownColor.Black);
        }

        // кнопка Hex вЫход текста
        private void btnGamTextOutHex_Click(object sender, EventArgs e)
        {
            if (gamirovanie.TextOutType == TypeDisplay.Hex) return;

            if (gamirovanie.TextOutIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                this.btnGamTextOutHex.Focus();
                return;
            }

            this.txtGamTextOut.Text = Functions.ByteToHex(gamirovanie.TextOutByte);

            gamirovanie.TextOutType = TypeDisplay.Hex;
            this.btnGamTextOutBinary.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnGamTextOutSymbol.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnGamTextOutHex.ForeColor = Color.FromKnownColor(KnownColor.Blue);
        }

        // ввод текста ВХОД
        private void txtGamTextIn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.checkBoxGamTextInEdit.Checked == false)
                return;

            if (e.KeyChar == 8 || e.KeyChar == 127) // Backspace или Delete
            {
                e.Handled = false;
                this.flagTextInIsEdited.Checked = true;
            }
            else if (gamirovanie.TextInType == TypeDisplay.Hex && Functions.checkSymbolIsHex(e.KeyChar) == true)
            {
                e.Handled = false;
                if(Functions.checkSymbolaf(e.KeyChar) == true) // если ввели маленькие строчки a-f
                    e.KeyChar = (char)((int)e.KeyChar - 32); // то привести их к верхнему регистру
                this.flagTextInIsEdited.Checked = true;
            }
            else if (gamirovanie.TextInType == TypeDisplay.Binary && Functions.checkSymbolIsBinary(e.KeyChar) == true)
            {
                e.Handled = false;
                this.flagTextInIsEdited.Checked = true;
            }
            else if(gamirovanie.TextInType == TypeDisplay.Symbol)
            {
                e.Handled = false;
                this.flagTextInIsEdited.Checked = true;
            }
            else
            {
                e.Handled = true;
            }

        }

        // ввод текста ВЫХОД
        private void txtGamTextOut_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.checkBoxGamTextOutEdit.Checked == false)
                return;

            if (e.KeyChar == 8 || e.KeyChar == 127) // Backspace или Delete
            {
                e.Handled = false;
                this.flagTextOutIsEdited.Checked = true;
            }
            else if (gamirovanie.TextOutType == TypeDisplay.Hex && Functions.checkSymbolIsHex(e.KeyChar) == true)
            {
                e.Handled = false;
                if (Functions.checkSymbolaf(e.KeyChar) == true) // если ввели маленькие строчки a-f
                    e.KeyChar = (char)((int)e.KeyChar - 32); // то привести их к верхнему регистру
                this.flagTextOutIsEdited.Checked = true;
            }
            else if (gamirovanie.TextOutType == TypeDisplay.Binary && Functions.checkSymbolIsBinary(e.KeyChar) == true)
            {
                e.Handled = false;
                this.flagTextOutIsEdited.Checked = true;
            }
            else if (gamirovanie.TextOutType == TypeDisplay.Symbol)
            {
                e.Handled = false;
                this.flagTextOutIsEdited.Checked = true;
            }
            else
            {
                e.Handled = true;
            }
        }

        // кнопка ОЧИСТИТЬ Гамирование
        private void btnGamClear_Click(object sender, EventArgs e)
        {
            gamirovanie = new Gamirovanie(); // перезаписываем объект

            //========очистка ключа======
            // меняем кнопку ввод ключа на обычную
            this.btnGamEnterKey.Text = "Ввести ключ (отсутствуют)";
            this.btnGamEnterKey.ForeColor = Color.FromKnownColor(KnownColor.Black);
            //===================================
            // входные данные стираем
            this.txtGamTextIn.Text = "";
            this.txtGamFileIn.Text = "";
            this.labelGamByteNumber.Text = "0";
            // ВЫходные данные стираем
            this.txtGamTextOut.Text = "";
            this.btnGamTextInSymbol.Enabled = true;

            this.flagTextInIsEdited.Checked = false;
            this.flagTextOutIsEdited.Checked = false;

            this.btnGamTextInSymbol.PerformClick();
            this.btnGamTextOutSymbol.PerformClick();
        }

        private void clearAllWithoutKey()
        {
            gamirovanie = new Gamirovanie(gamirovanie.KeyByte, gamirovanie.KeyType, gamirovanie.KeyIsEntry, gamirovanie.KeyIsCorrect); // перезаписываем объект

            //===================================
            // входные данные стираем
            this.txtGamTextIn.Text = "";
            this.txtGamFileIn.Text = "";
            this.labelGamByteNumber.Text = "0";
            // ВЫходные данные стираем
            this.txtGamTextOut.Text = "";
            this.btnGamTextInSymbol.Enabled = true;

            this.flagTextInIsEdited.Checked = false;
            this.flagTextOutIsEdited.Checked = false;

            this.btnGamTextInSymbol.PerformClick();
            this.btnGamTextOutSymbol.PerformClick();
        }

        // кнопка ВЫБРАТЬ ФАЙЛ
        private void btnGamChoiceFileIn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Выбрать файл ..."; // Заголовок окна
            ofd.InitialDirectory = Application.StartupPath; // путь откуда запустили

            if(gamirovanie.EncryptOrDecrypt == false)
                ofd.Filter = "Keys(*.secret)|*.secret"; // расширение файла с шифротекстом

            if (ofd.ShowDialog() == DialogResult.OK) // Если выбрали файл
            {
                // читаем байты из файла
                if (ofd.FileName.Length > 0) // Если путь не нулевой
                {
                    if (File.Exists(ofd.FileName) == true) // Если указанный файл существует
                    {
                        // очистили всё кроме ключа
                        //this.clearAllWithoutKey();
                        this.btnGamClear.PerformClick();
                        // Считали байты из файла
                        gamirovanie.TextInByte = File.ReadAllBytes(ofd.FileName);
                        this.labelGamByteNumber.Text = gamirovanie.TextInByte.Length.ToString(); // Вывели кол-во считанных байт
                        this.txtGamFileIn.Text = ofd.FileName; // вывели путь к файлу в textbox
                        this.toolTipGamFileIn.SetToolTip(this.txtGamFileIn, this.txtGamFileIn.Text); // текст подсказки запомнили
                        gamirovanie.FileExtension = ofd.SafeFileName.Substring(ofd.SafeFileName.LastIndexOf('.'));  // Запомнили расширение считанного файла
                        if(gamirovanie.FileExtension == "txt")
                        {
                            //this.btnGamTextInSymbol.Enabled = true;
                        }
                        else
                        {
                            this.btnGamTextInHex.PerformClick();
                            //this.btnGamTextInSymbol.Enabled = false;
                        }
                        // вывели на форму считанное в кодировке UTF8
                        if (gamirovanie.TextInType == TypeDisplay.Hex)
                        {
                            this.txtGamTextIn.Text = Functions.ByteToHex(gamirovanie.TextInByte);
                        }
                        else if (gamirovanie.TextInType == TypeDisplay.Binary)
                        {
                            this.txtGamTextIn.Text = Functions.ByteToBinary(gamirovanie.TextInByte);
                        }
                        else if (gamirovanie.TextInType == TypeDisplay.Symbol)
                        {
                            this.txtGamTextIn.Text = Functions.ByteToSymbol(gamirovanie.TextInByte);
                        }

                    }
                    else
                    {
                        this.Enabled = false;
                        MessageBox.Show("Файла [" + ofd.FileName + "] не существует!", " Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
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

        // кнопка ВВОД КЛЮЧА
        private void btnGamEnterKey_Click(object sender, EventArgs e)
        {
            FormGamEnterKey form = new FormGamEnterKey(ref this.btnGamEnterKey, ref gamirovanie);
            form.ShowDialog(this);
        }

        // флаг изменен ли ВХОД текст
        private void flagTextInIsEdited_CheckedChanged(object sender, EventArgs e)
        {
            if(flagTextInIsEdited.Checked == true)
            {
                gamirovanie.TextInIsEdited = true;
                this.btnGamTextInSaveChanged.Visible = true;
                this.btnGamTextInCancelChanged.Visible = true;
            }
            else
            {
                gamirovanie.TextInIsEdited = false;
                this.btnGamTextInSaveChanged.Visible = false;
                this.btnGamTextInCancelChanged.Visible = false;
            }
        }

        // флаг изменен ли вЫход текст
        private void flagTextOutIsEdited_CheckedChanged(object sender, EventArgs e)
        {
            if (flagTextOutIsEdited.Checked == true)
            {
                gamirovanie.TextOutIsEdited = true;
                this.btnGamTextOutSaveChanged.Visible = true;
                this.btnGamTextOutCancelChanged.Visible = true;
            }
            else
            {
                gamirovanie.TextOutIsEdited = false;
                this.btnGamTextOutSaveChanged.Visible = false;
                this.btnGamTextOutCancelChanged.Visible = false;
            }
        }

        // кнопка ДИСКЕТА сохранить изменения ВХОД текста 
        private void btnGamTextInSaveChanged_Click(object sender, EventArgs e)
        {
            if(gamirovanie.TextInIsEdited == true)
            {
                if (gamirovanie.TextInType == TypeDisplay.Binary)
                {
                    if (Functions.checkStringIsBinarySequence(this.txtGamTextIn.Text) == true)
                    {
                        gamirovanie.TextInByte = Functions.BinaryToByte(this.txtGamTextIn.Text);
                        this.flagTextInIsEdited.Checked = false;
                        this.checkBoxGamTextInEdit.Checked = false;
                    }
                    else
                    {
                        this.Enabled = false;
                        MessageBox.Show("Измененные данные не соответствуют бинарному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
                        return;
                    }
                }
                else if (gamirovanie.TextInType == TypeDisplay.Hex)
                {
                    if (Functions.checkStringIsHexSequence(this.txtGamTextIn.Text) == true)
                    {
                        gamirovanie.TextInByte = Functions.HexToByte(this.txtGamTextIn.Text);
                        this.flagTextInIsEdited.Checked = false;
                        this.checkBoxGamTextInEdit.Checked = false;
                    }
                    else
                    {
                        this.Enabled = false;
                        MessageBox.Show("Измененные данные не соответствуют 16-ричному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
                        return;
                    }
                }
                else if (gamirovanie.TextInType == TypeDisplay.Symbol)
                {
                    gamirovanie.TextInByte = Functions.SymbolToByte(this.txtGamTextIn.Text);
                    this.flagTextInIsEdited.Checked = false;
                    this.checkBoxGamTextInEdit.Checked = false;
                }

                //вывести новое число байт
                this.labelGamByteNumber.Text = gamirovanie.TextInByte.Length.ToString();

                //MessageBox.Show("Изменения сохранены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Изменений не было!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btnGamTextInSaveChanged.Visible = false;
                this.btnGamTextInCancelChanged.Visible = false;
                this.checkBoxGamTextInEdit.Checked = false;
            }
        }

        // кнопка ДИСКЕТА сохранить изменения вЫход текста 
        private void btnGamTextOutSaveChanged_Click(object sender, EventArgs e)
        {
            if (gamirovanie.TextOutIsEdited == true)
            {
                DialogResult dr; 
                if(gamirovanie.EncryptOrDecrypt == true)
                    dr = MessageBox.Show("Вы действительно хотите сохранить измененный шифротекст?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                else
                    dr = MessageBox.Show("Вы действительно хотите сохранить измененное сообщение после дешифрования?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (dr == DialogResult.OK)
                {

                    if (gamirovanie.TextOutType == TypeDisplay.Binary)
                    {
                        if (Functions.checkStringIsBinarySequence(this.txtGamTextOut.Text) == true)
                        {
                            gamirovanie.TextOutByte = Functions.BinaryToByte(this.txtGamTextOut.Text);
                            this.flagTextOutIsEdited.Checked = false;
                            this.checkBoxGamTextOutEdit.Checked = false;
                        }
                        else
                        {
                            this.Enabled = false;
                            MessageBox.Show("Измененные данные не соответствуют бинарному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Enabled = true;
                            return;
                        }
                    }
                    else if (gamirovanie.TextOutType == TypeDisplay.Hex)
                    {
                        if (Functions.checkStringIsHexSequence(this.txtGamTextOut.Text) == true)
                        {
                            gamirovanie.TextOutByte = Functions.HexToByte(this.txtGamTextOut.Text);
                            this.flagTextOutIsEdited.Checked = false;
                            this.checkBoxGamTextOutEdit.Checked = false;
                        }
                        else
                        {
                            this.Enabled = false;
                            MessageBox.Show("Измененные данные не соответствуют 16-ричному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Enabled = true;
                            return;
                        }
                    }
                    else if (gamirovanie.TextOutType == TypeDisplay.Symbol)
                    {
                        gamirovanie.TextOutByte = Functions.SymbolToByte(this.txtGamTextOut.Text);
                        this.flagTextOutIsEdited.Checked = false;
                        this.checkBoxGamTextOutEdit.Checked = false;
                        //MessageBox.Show("Изменения сохранены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Изменений не было!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.btnGamTextInSaveChanged.Visible = false;
                        this.btnGamTextInCancelChanged.Visible = false;
                        this.checkBoxGamTextInEdit.Checked = false;
                    }
                }
            }
        }

        // кнопка ВЕДРО откатить изменения ВХОД текста 
        private void btnGamTextInCancelChanged_Click(object sender, EventArgs e)
        {
            if (gamirovanie.TextInType == TypeDisplay.Binary)
                this.txtGamTextIn.Text = Functions.ByteToBinary(gamirovanie.TextInByte);
            else if (gamirovanie.TextInType == TypeDisplay.Hex)
                this.txtGamTextIn.Text = Functions.ByteToHex(gamirovanie.TextInByte);
            else if (gamirovanie.TextInType == TypeDisplay.Symbol)
                this.txtGamTextIn.Text = Functions.ByteToSymbol(gamirovanie.TextInByte);

            this.flagTextInIsEdited.Checked = false;
            this.checkBoxGamTextInEdit.Checked = false;
        }

        // кнопка ВЕДРО откатить изменения ВЫХОД текста 
        private void btnGamTextOutCancelChanged_Click(object sender, EventArgs e)
        {
            if (gamirovanie.TextOutType == TypeDisplay.Binary)
                this.txtGamTextOut.Text = Functions.ByteToBinary(gamirovanie.TextOutByte);
            else if (gamirovanie.TextOutType == TypeDisplay.Hex)
                this.txtGamTextOut.Text = Functions.ByteToHex(gamirovanie.TextOutByte);
            else if (gamirovanie.TextOutType == TypeDisplay.Symbol)
                this.txtGamTextOut.Text = Functions.ByteToSymbol(gamirovanie.TextOutByte);

            this.flagTextOutIsEdited.Checked = false;
            this.checkBoxGamTextOutEdit.Checked = false;
        }

        // СОХРАНИТЬ КЛЮЧ
        private void btnGamSaveKey_Click(object sender, EventArgs e)
        {
            try
            {
                //Если ключа нет
                if (gamirovanie.KeyByte.Length < 1 || gamirovanie.KeyIsEntry == false)
                {
                    this.Enabled = false;
                    MessageBox.Show("Невозможно сохранить ключ:\n\tКлюч не введен!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Выберите папку и введите название файла (БЕЗ расширения) ...";
                sfd.InitialDirectory = Application.StartupPath;
                sfd.Filter = "Files(*.key)|*.key"; // Сохранять только c расширением key
                sfd.AddExtension = true;  //Добавить расширение к имени если не указали

                DialogResult res = sfd.ShowDialog();
                if (res == DialogResult.OK)
                {
                    // получаем выбранный файл
                    string filename = sfd.FileName;
                    // сохраняем байты в файл
                    System.IO.File.WriteAllBytes(filename, gamirovanie.KeyByte);

                    this.Enabled = false;
                    MessageBox.Show("КЛЮЧ записан в файл:\n" + filename, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Enabled = true;
                }
                sfd.Dispose();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "НЕПРЕДВИДЕННАЯ ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        // кнопка ШИФРОВАТЬ
        private void btnGamEncryptDecrypt_Click(object sender, EventArgs e)
        {
            if (gamirovanie.TextInByte.Length > 0) // Если входные данные не пусты
            {
                if (gamirovanie.KeyIsEntry == true) // Если введен ключ и вектор
                {
                    try
                    {
                        // вызываем функцию шифрования и получаем байты выходные
                        byte[] temp;
                        string errMessage = "";
                        bool result = Algorithms.GamirovanieAlgorithm(gamirovanie.TextInByte, gamirovanie.KeyByte, out temp, errMessage);

                        if(result == false)
                        {
                            MessageBox.Show(errMessage, "НЕПРЕДВИДЕННАЯ ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            gamirovanie.TextOutByte = temp;
                            gamirovanie.TextOutType = TypeDisplay.None;
                            this.btnGamTextOutHex.PerformClick();
                        }

                        // Вывести выходные байты 
                        if (gamirovanie.EncryptOrDecrypt == true) // Если шифруем
                        {
                            // вывели байты на форму виде 16-ричной строки
                            this.txt_simm_text_out.Text = Functions.ByteArrayTOStringHex(Global.Simm_byte_out);
                        }
                        else // Если расшифровываем
                        {
                            // вывели байты на форму виде строки с кодировкой UTF8
                            this.txt_simm_text_out.Text = Encoding.UTF8.GetString(Global.Simm_byte_out).Replace("\0", "0");
                        }
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message, "НЕПРЕДВИДЕННАЯ ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    this.Enabled = false;
                    MessageBox.Show("Сначала введите ключ!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    return;
                }
            }
            else
            {
                this.Enabled = false;
                MessageBox.Show("Сначала укажите входные данные!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                return;
            }
        }

        // СОХРАНИТЬ ВЫХОДНОЙ ТЕКСТ В ФАЙЛ
        private void btnGamSaveData_Click(object sender, EventArgs e)
        {
            try
            {
                //Если выходные байты пусты 
                if (gamirovanie.TextInByte.Length < 1)
                {
                    this.Enabled = false;
                    if (gamirovanie.EncryptOrDecrypt == true)
                        MessageBox.Show("Шифротекст отсутствует!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        MessageBox.Show("Исходный текст отсутствует!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Выберите папку и введите название файла (БЕЗ расширения) ...";
                sfd.InitialDirectory = Application.StartupPath;
                sfd.Filter = "Files(*." + gamirovanie.FileExtension + ")|*." + gamirovanie.FileExtension; // Сохранять только c расширением как и у входного файла
                sfd.AddExtension = true;  //Добавить расширение к имени если не указали

                DialogResult res = sfd.ShowDialog();
                if (res == DialogResult.OK)
                {
                    // получаем выбранный файл
                    string filename = sfd.FileName;
                    // сохраняем байты в файл
                    System.IO.File.WriteAllBytes(filename, gamirovanie.TextOutByte);

                    this.Enabled = false;
                    if (Global.Simm_EncryptOrDecrypt == true)
                        MessageBox.Show("Шифротекст записан в файл:\n" + filename, "Сохранено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Дешифрованное сообщение записано в файл:\n" + filename, "Сохранено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Enabled = true;
                }
                sfd.Dispose();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "НЕПРЕДВИДЕННАЯ ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }


}

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
using System.Numerics;

namespace CryptoAppTwo
{
    public partial class FormMain : Form
    {
        private Gamirovanie gamirovanie = null;
        private Feistel feistel = null;
        private AesObject aes = null;
        
        private List<PrimeNumber> PrimeNumberList = null;
        
        public FormMain()
        {
            InitializeComponent();
        }

        #region Обработчики самой формы
        // при ЗАГРУЗКЕ ФОРМЫ
        private void FormMain_Load(object sender, EventArgs e)
        {
            this.tabControlMain.SelectedIndex = 1;
            this.tabControlMain.SelectedIndex = 0;

            // скрыть лишние вкладки на форме
            // закоменчено, значит включено
            this.tabGam.Parent = null;
            this.tabGpn.Parent = null;
            //this.tabFst.Parent = null;
            //this.tabAes.Parent = null;
            this.tabHesh.Parent = null;
            this.tabSim.Parent = null;
            this.tabAsimAlg.Parent = null;
            this.tabEds.Parent = null;

            #region Дефолтные установки других вкладок (tabHesh, tabSimAlg, tabAsimAlg, tabEds)
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

            #region Дефолтные установки для гамирования/скремблирования
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
            this.comboBoxGamAlgorithm.SelectedIndex = 0; // метод гамирования выбрать
            this.btnGamClear.PerformClick(); // жмем кнопку очистить для Гамирования
            #endregion

            #region Дефолтные установки для СЕТИ ФЕЙСТЕЛЯ
            feistel = new Feistel();
            this.radioBtnFstEncrypt.Checked = true; ; // режим шифрования при запуске 
            this.comboBoxFstSubkey.SelectedIndex = 0; // метод гамирования выбрать
            this.btnFstClear.PerformClick(); // жмем кнопку очистить для 
            #endregion

            #region Дефолтные установки для генерации ПРОСТЫХ ЧИСЕЛ
            PrimeNumberList = new List<PrimeNumber>();
            this.numericGpnLeft.Minimum = 1;
            this.numericGpnLeft.Maximum = new Decimal(1208925819614629174706176.0);
            this.numericGpnRight.Minimum = 1;
            this.numericGpnRight.Maximum = new Decimal(1208925819614629174706176.0);
            #endregion

            #region Дефолтные установки для СЕТИ ФЕЙСТЕЛЯ
            aes = new AesObject();
            this.radioBtnAesEncrypt.Checked = true; ; // режим шифрования при запуске 
            this.btnAesClear_Click(null, null); // жмем кнопку очистить д
            #endregion
        }

        // Смена вкладки сверху
        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            // если вкладка с сетью Фейстеля то увеличить ширину справа
            if(this.tabControlMain.SelectedTab.Name == "tabFst")
            {
                this.Width = 1500;
                this.CenterToScreen();
            }
            else
            {
                this.Width = 857;
                this.CenterToScreen();
            }
        }
        #endregion

        #region Функции обработчики от других вкладок

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

        #region Функции обработки вкладки гамирования/скремблирования

        // кнопка режим Гамирование ШИФРОВАТЬ
        private void radioBtnGamEncrypt_CheckedChanged(object sender, EventArgs e)
        {
            this.btnGamEncryptDecrypt.Text = "🡻 Шифровать 🡻";
            this.labelGamCaptionIn.Text = "Сообщение";
            this.labelGamTextInCaption.Text = "Сообщение:";
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
                if(gamirovanie.TextInType == TypeDisplay.Binary) this.btnGamTextInBinary.Focus();
                else if (gamirovanie.TextInType == TypeDisplay.Hex) this.btnGamTextInHex.Focus();
                else if (gamirovanie.TextInType == TypeDisplay.Symbol) this.btnGamTextInSymbol.Focus();
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
                if (gamirovanie.TextInType == TypeDisplay.Binary) this.btnGamTextInBinary.Focus();
                else if (gamirovanie.TextInType == TypeDisplay.Hex) this.btnGamTextInHex.Focus();
                else if (gamirovanie.TextInType == TypeDisplay.Symbol) this.btnGamTextInSymbol.Focus();
                return;
            }

            if (!(gamirovanie.FileExtension == "txt" && gamirovanie.EncryptOrDecrypt == true))
            {
                this.Enabled = false;
                MessageBox.Show("Отображение данных в текстовом виде доступно только для файлов с расширением .txt в режиме шифрования!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (gamirovanie.TextInType == TypeDisplay.Binary) this.btnGamTextInBinary.Focus();
                else if (gamirovanie.TextInType == TypeDisplay.Hex) this.btnGamTextInHex.Focus();
                else if (gamirovanie.TextInType == TypeDisplay.Symbol) this.btnGamTextInSymbol.Focus();
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
                if (gamirovanie.TextOutType == TypeDisplay.Binary) this.btnGamTextOutBinary.Focus();
                else if (gamirovanie.TextOutType == TypeDisplay.Hex) this.btnGamTextOutHex.Focus();
                else if (gamirovanie.TextOutType == TypeDisplay.Symbol) this.btnGamTextOutSymbol.Focus();
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
                if (gamirovanie.TextOutType == TypeDisplay.Binary) this.btnGamTextOutBinary.Focus();
                else if (gamirovanie.TextOutType == TypeDisplay.Hex) this.btnGamTextOutHex.Focus();
                else if (gamirovanie.TextOutType == TypeDisplay.Symbol) this.btnGamTextOutSymbol.Focus();
                return;
            }

            if (!(gamirovanie.FileExtension == "txt" /*&& gamirovanie.EncryptOrDecrypt == false*/))
            {
                this.Enabled = false;
                MessageBox.Show("Отображение данных в текстовом виде доступно только для файлов с раcширением .txt! в режиме дешифрования", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (gamirovanie.TextOutType == TypeDisplay.Binary) this.btnGamTextOutBinary.Focus();
                else if (gamirovanie.TextOutType == TypeDisplay.Hex) this.btnGamTextOutHex.Focus();
                else if (gamirovanie.TextOutType == TypeDisplay.Symbol) this.btnGamTextOutSymbol.Focus();
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
                this.flagGamTextInIsEdited.Checked = true;
            }
            else if (gamirovanie.TextInType == TypeDisplay.Hex && Functions.checkSymbolIsHex(e.KeyChar) == true)
            {
                e.Handled = false;
                if(Functions.checkSymbolaf(e.KeyChar) == true) // если ввели маленькие строчки a-f
                    e.KeyChar = (char)((int)e.KeyChar - 32); // то привести их к верхнему регистру
                this.flagGamTextInIsEdited.Checked = true;
            }
            else if (gamirovanie.TextInType == TypeDisplay.Binary && Functions.checkSymbolIsBinary(e.KeyChar) == true)
            {
                e.Handled = false;
                this.flagGamTextInIsEdited.Checked = true;
            }
            // русские буквы осуждаются
            //else if(gamirovanie.TextInType == TypeDisplay.Symbol && !(e.KeyChar >= 1072 && e.KeyChar <=1105))
            //{
            //    e.Handled = false;
            //    this.flagTextInIsEdited.Checked = true;
            //}
            else if (gamirovanie.TextInType == TypeDisplay.Symbol)
            {
                e.Handled = false;
                this.flagGamTextInIsEdited.Checked = true;
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
                this.flagGamTextOutIsEdited.Checked = true;
            }
            else if (gamirovanie.TextOutType == TypeDisplay.Hex && Functions.checkSymbolIsHex(e.KeyChar) == true)
            {
                e.Handled = false;
                if (Functions.checkSymbolaf(e.KeyChar) == true) // если ввели маленькие строчки a-f
                    e.KeyChar = (char)((int)e.KeyChar - 32); // то привести их к верхнему регистру
                this.flagGamTextOutIsEdited.Checked = true;
            }
            else if (gamirovanie.TextOutType == TypeDisplay.Binary && Functions.checkSymbolIsBinary(e.KeyChar) == true)
            {
                e.Handled = false;
                this.flagGamTextOutIsEdited.Checked = true;
            }
            //русские буквы тоже тут осуждаются
            else if (gamirovanie.TextOutType == TypeDisplay.Symbol && !(e.KeyChar >= 1072 && e.KeyChar <= 1105))
            {
                e.Handled = false;
                this.flagGamTextOutIsEdited.Checked = true;
            }
            else
            {
                e.Handled = true;
            }
        }

        // кнопка ОЧИСТИТЬ Гамирование
        private void btnGamClear_Click(object sender, EventArgs e)
        {
            bool rezhim = gamirovanie.EncryptOrDecrypt;
            gamirovanie = new Gamirovanie(); // перезаписываем объект
            gamirovanie.EncryptOrDecrypt = rezhim;
            //========очистка ключа======
            // меняем кнопку ввод ключа на обычную
            if(gamirovanie.GamirovanieOrScrembler == true)
                this.btnGamEnterKey.Text = "Ввести ключ (отсутствуют)";
            else
                //this.btnGamEnterKey.Text = "Настроить скремблер";
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

            this.flagGamTextInIsEdited.Checked = false;
            this.flagGamTextOutIsEdited.Checked = false;

            //this.comboBoxGamAlgorithm.SelectedIndex = 0; // метод гамирования выбрать

            if (gamirovanie.EncryptOrDecrypt == true)
            {
                this.btnGamTextInSymbol.PerformClick();
                this.btnGamTextOutHex.PerformClick();
            }
            else
            {
                this.btnGamTextInHex.PerformClick();
                this.btnGamTextOutSymbol.PerformClick();
            }
        }

        private void clearAllWithoutKey()
        {
            bool rezhim = gamirovanie.EncryptOrDecrypt;
            gamirovanie = new Gamirovanie(gamirovanie.KeyByte, gamirovanie.KeyType, gamirovanie.KeyIsEntry, gamirovanie.KeyIsCorrect); // перезаписываем объект
            gamirovanie.EncryptOrDecrypt = rezhim;
            //===================================
            // входные данные стираем
            this.txtGamTextIn.Text = "";
            this.txtGamFileIn.Text = "";
            this.labelGamByteNumber.Text = "0";
            // ВЫходные данные стираем
            this.txtGamTextOut.Text = "";
            this.btnGamTextInSymbol.Enabled = true;

            this.flagGamTextInIsEdited.Checked = false;
            this.flagGamTextOutIsEdited.Checked = false;

            this.btnGamTextInSymbol.PerformClick();
            this.btnGamTextOutSymbol.PerformClick();
        }

        // кнопка ВЫБРАТЬ ФАЙЛ
        private void btnGamChoiceFileIn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Выбрать файл ..."; // Заголовок окна
            ofd.InitialDirectory = Application.StartupPath; // путь откуда запустили

            //if(gamirovanie.EncryptOrDecrypt == false)
            //    ofd.Filter = "Keys(*.secret)|*.secret"; // расширение файла с шифротекстом

            if (ofd.ShowDialog() == DialogResult.OK) // Если выбрали файл
            {
                // читаем байты из файла
                if (ofd.FileName.Length > 0) // Если путь не нулевой
                {
                    if (File.Exists(ofd.FileName) == true) // Если указанный файл существует
                    {
                        //if(gamirovanie.EncryptOrDecrypt == true) //закоментил фичу
                        this.btnGamClear.PerformClick();
                        //else
                        //    this.clearAllWithoutKey();// очистили всё кроме ключа
                        // Считали байты из файла
                        gamirovanie.TextInByte = File.ReadAllBytes(ofd.FileName);
                        this.labelGamByteNumber.Text = gamirovanie.TextInByte.Length.ToString(); // Вывели кол-во считанных байт
                        this.txtGamFileIn.Text = ofd.FileName; // вывели путь к файлу в textbox
                        this.toolTipGamFileIn.SetToolTip(this.txtGamFileIn, this.txtGamFileIn.Text); // текст подсказки запомнили
                        gamirovanie.FileExtension = ofd.SafeFileName.Substring(ofd.SafeFileName.LastIndexOf('.'));  // Запомнили расширение считанного файла
                        if(gamirovanie.FileExtension.Length > 1) gamirovanie.FileExtension = gamirovanie.FileExtension.Substring(1);
                        if(gamirovanie.FileExtension == "txt" && gamirovanie.EncryptOrDecrypt == true)
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
            if (comboBoxGamAlgorithm.SelectedIndex == 0)
            {
                FormGamEnterKey form = new FormGamEnterKey(ref this.btnGamEnterKey, ref gamirovanie);
                form.ShowDialog(this);
            }
            else if (comboBoxGamAlgorithm.SelectedIndex == 1)
            {
                FormGamScremblerEnterKey form = new FormGamScremblerEnterKey(ref this.btnGamEnterKey, ref gamirovanie);
                form.ShowDialog(this);
            }
        }

        // флаг изменен ли ВХОД текст
        private void flagTextInIsEdited_CheckedChanged(object sender, EventArgs e)
        {
            if(flagGamTextInIsEdited.Checked == true)
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
            if (flagGamTextOutIsEdited.Checked == true)
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
                        this.flagGamTextInIsEdited.Checked = false;
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
                        this.flagGamTextInIsEdited.Checked = false;
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
                    this.flagGamTextInIsEdited.Checked = false;
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
                            this.flagGamTextOutIsEdited.Checked = false;
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
                            this.flagGamTextOutIsEdited.Checked = false;
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
                        this.flagGamTextOutIsEdited.Checked = false;
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

            this.flagGamTextInIsEdited.Checked = false;
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

            this.flagGamTextOutIsEdited.Checked = false;
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
            if (gamirovanie.TextInByte.Length < 1)
            {
                this.Enabled = false;
                MessageBox.Show("Сначала укажите входные данные!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                return;
            }

            if (gamirovanie.KeyIsEntry == false)// Если введен ключ
            {
                this.Enabled = false;
                MessageBox.Show("Введите ключ!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                return;
            }
            try
            {
                // вызываем функцию шифрования и получаем байты выходные
                byte[] temp;
                string errMessage = "";
                bool result = Gamirovanie.GamirovanieXOR(gamirovanie.TextInByte, gamirovanie.KeyByte, out temp, out errMessage);

                if(result == false)
                {
                    MessageBox.Show(errMessage, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    gamirovanie.TextOutByte = temp;
                    gamirovanie.TextOutType = TypeDisplay.None;
                    this.btnGamTextOutHex.PerformClick();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "НЕПРЕДВИДЕННАЯ ОШИБКА", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // СМЕНА АЛГОРИТМА
        private void comboBoxGamAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnGamClear.PerformClick(); // ОЧИСТИТЬ
            if (this.comboBoxGamAlgorithm.SelectedIndex == 0) // если Гамирование
            {
                this.gamirovanie.GamirovanieOrScrembler = true;
                this.btnGamEnterKey.Text = "Ввести ключ (отсутствуют)";
            }
            else if (this.comboBoxGamAlgorithm.SelectedIndex == 1) // если Скремблирование
            {
                this.gamirovanie.GamirovanieOrScrembler = false;
                //this.btnGamEnterKey.Text = "Настроить скремблер";
                this.btnGamEnterKey.Text = "Ввести ключ (отсутствуют)";
            }
        }





        #endregion

        #region Функции обработчики Сеть Фейстеля

        // радио батон ШИФРОВАНИЕ
        private void radioBtnFstEncrypt_CheckedChanged(object sender, EventArgs e)
        {
            btnFstEncryptDecrypt.Text = "Шифровать";
            labelFstCaptionIn.Text = "Сообщение";
            labelFstCaptionOut.Text = "Шифротекст";
            btnFstSaveData.Text = "Сохранить шифротекст";
            btnFstClear.PerformClick(); // Очистить всё при переключении
            feistel.EncryptOrDecrypt = true;
            //btnFstKeyGenerate.Visible = true;
            //btnFstKeyLoad.Visible = false;
            
            //checkBoxFstTextOutEdit.Visible = false; //ВЫКЛЮЧИЛ КНОПКУ РЕДАКТИРОВАНИЯ ВЫХОДА
        }

        // радио батон ДЕШИФРОВАНИЕ
        private void radioBtnFstDecrypt_CheckedChanged(object sender, EventArgs e)
        {
            this.btnFstEncryptDecrypt.Text = "Дешифровать";
            this.labelFstCaptionIn.Text = "Шифротекст";
            this.labelFstCaptionOut.Text = "Сообщение";
            this.btnFstSaveData.Text = "Сохранить сообщение";
            btnFstClear.PerformClick(); // Очистить всё при переключении
            feistel.EncryptOrDecrypt = false;
            //btnFstKeyGenerate.Visible = false;
            //btnFstKeyLoad.Visible = true;
            
            //this.checkBoxFstTextOutEdit.Visible = false; //ВЫКЛЮЧИЛ КНОПКУ РЕДАКТИРОВАНИЯ ВЫХОДА
        }

        // кнопка ОЧИСТИТЬ всё
        private void btnFstClear_Click(object sender, EventArgs e)
        {
            bool rezhim = feistel.EncryptOrDecrypt;
            feistel = new Feistel(); // перезаписываем объект
            feistel.EncryptOrDecrypt = rezhim;
            if (rezhim == true)
                radioBtnFstEncrypt_CheckedChanged(null, null);
            else
                radioBtnFstDecrypt_CheckedChanged(null, null);
            //===================================
            // входные данные стираем
            this.txtFstTextIn.Text = "";
            this.labelFstByteNumber.Text = "0";
            // ВЫходные данные стираем
            this.txtFstTextOut.Text = "";
            this.btnFstTextInSymbol.Enabled = true;
            //флаги
            this.flagFstTextInIsEdited.Checked = false;
            this.flagFstTextOutIsEdited.Checked = false;
            this.flagFstKeyIsEdited.Checked = false;
            // параметры
            this.comboBoxFstFunc.SelectedIndex = 0;
            this.comboBoxFstSubkey.SelectedIndex = 0;

            //кнопки редактирования
            this.btnFstTextInSaveChanged.Visible = false;
            this.btnFstTextOutSaveChanged.Visible = false;
            this.btnFstTextInCancelChanged.Visible = false;
            this.btnFstTextOutCancelChanged.Visible = false;
            this.btnFstKeySaveChanged.Visible = false;
            this.btnFstKeyCancelChanged.Visible = false;
            checkBoxFstTextInEdit.Checked = false;
            checkBoxFstTextOutEdit.Checked = false;
            checkBoxFstKeyEdit.Checked = false;


            if (feistel.EncryptOrDecrypt == true)
            {
                this.btnFstTextInSymbol.PerformClick();
                this.btnFstKeyHex.PerformClick();
                this.btnFstTextOutHex.PerformClick();
            }
            else
            {
                this.btnFstTextInHex.PerformClick();
                this.btnFstKeyHex.PerformClick();
                this.btnFstTextOutSymbol.PerformClick();
            }
        }

        // Вид получения подключа
        private void comboBoxFstSubkey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFstSubkey.SelectedItem.ToString() == "Циклически")
                feistel.SubKeyMode = Feistel.KeyMethodGenerate.Cycle;
            else if(comboBoxFstSubkey.SelectedItem.ToString() == "Скремблер")
                feistel.SubKeyMode = Feistel.KeyMethodGenerate.Scrambler;
            else
                feistel.SubKeyMode = Feistel.KeyMethodGenerate.None;
        }

        // Вид образующей функции
        private void comboBoxFstFunc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxFstFunc.SelectedItem.ToString() == "Единичная")
                feistel.FuncMode = Feistel.FunctionMethodGenerate.Single;
            else if (comboBoxFstFunc.SelectedItem.ToString() == "XOR")
                feistel.FuncMode = Feistel.FunctionMethodGenerate.Xor;
            else
                feistel.FuncMode = Feistel.FunctionMethodGenerate.None;
        }

        // функция обновления графика
        private void ChartRefresh()
        {
            ChartFstText.Series[0].Points.Clear();
            ChartFstKey.Series[0].Points.Clear();

            for (int i = 0; i < 16; i++)
            {
                ChartFstText.Series[0].Points.AddXY(i + 1, feistel.ChartListBitsText[i]);
                ChartFstKey.Series[0].Points.AddXY(i + 1, feistel.ChartListBitsKey[i]);
            }
        }

        // галочка ВКЛ ВЫКЛ редактирование вход текста
        private void checkBoxFstTextInEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxFstTextInEdit.Checked == true)
            {
                this.txtFstTextIn.ReadOnly = false;
            }
            else
            {
                if (feistel.TextInIsEdited == true)
                {
                    this.Enabled = false;
                    MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    this.checkBoxFstTextInEdit.Checked = true;
                }
                else
                {
                    this.txtFstTextIn.ReadOnly = true;
                }
            }
        }

        // галочка ВКЛ ВЫКЛ редактирование ключа
        private void checkBoxFstKeyEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxFstKeyEdit.Checked == true)
            {
                this.txtFstKey.ReadOnly = false;
            }
            else
            {
                if (feistel.KeyIsEdited == true)
                {
                    this.Enabled = false;
                    MessageBox.Show("Ключ был изменен!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    this.checkBoxFstKeyEdit.Checked = true;
                }
                else
                {
                    this.txtFstTextIn.ReadOnly = true;
                }
            }
        }

        // галочка ВКЛ ВЫКЛ редактирование вЫход текста
        private void checkBoxFstTextOutEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxFstTextOutEdit.Checked == true)
            {
                this.txtFstTextOut.ReadOnly = false;
            }
            else
            {
                if (feistel.TextOutIsEdited == true)
                {
                    this.Enabled = false;
                    MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    this.checkBoxFstTextOutEdit.Checked = true;
                }
                else
                {
                    this.txtFstTextOut.ReadOnly = true;
                }
            }
        }

        // кнопка Bin вход текста
        private void btnFstTextInBinary_Click(object sender, EventArgs e)
        {
            if (feistel.TextInType == TypeDisplay.Binary) return;

            if (feistel.TextInIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (feistel.TextInType == TypeDisplay.Binary) this.btnFstTextInBinary.Focus();
                else if (feistel.TextInType == TypeDisplay.Hex) this.btnFstTextInHex.Focus();
                else if (feistel.TextInType == TypeDisplay.Symbol) this.btnFstTextInSymbol.Focus();
                return;
            }

            if (feistel.TextInByte.Length > 50000)
            {
                this.Enabled = false;
                MessageBox.Show("Количество байтов слишком велико!\n(Больше 50000 байт)\nОтображение в бинарном виде недоступно!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Enabled = true;
                return;
            }

            this.txtFstTextIn.Text = Functions.ByteToBinary(feistel.TextInByte);

            feistel.TextInType = TypeDisplay.Binary;
            this.btnFstTextInBinary.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            this.btnFstTextInSymbol.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnFstTextInHex.ForeColor = Color.FromKnownColor(KnownColor.Black);
        }

        // кнопка Symb вход текста
        private void btnFstTextInSymbol_Click(object sender, EventArgs e)
        {

            if (feistel.TextInType == TypeDisplay.Symbol) return;

            if (feistel.TextInIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (feistel.TextInType == TypeDisplay.Binary) this.btnFstTextInBinary.Focus();
                else if (feistel.TextInType == TypeDisplay.Hex) this.btnFstTextInHex.Focus();
                else if (feistel.TextInType == TypeDisplay.Symbol) this.btnFstTextInSymbol.Focus();
                return;
            }

            if (!(feistel.FileExtension == "txt" && feistel.EncryptOrDecrypt == true))
            {
                this.Enabled = false;
                MessageBox.Show("Отображение данных в текстовом виде доступно только для файлов с расширением .txt в режиме шифрования!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Enabled = true;
                return;
            }

            this.txtFstTextIn.Text = Functions.ByteToSymbol(feistel.TextInByte);

            feistel.TextInType = TypeDisplay.Symbol;
            this.btnFstTextInBinary.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnFstTextInSymbol.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            this.btnFstTextInHex.ForeColor = Color.FromKnownColor(KnownColor.Black);
        }

        // кнопка Hex вход текста
        private void btnFstTextInHex_Click(object sender, EventArgs e)
        {
            if (feistel.TextInType == TypeDisplay.Hex) return;

            if (feistel.TextInIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (feistel.TextInType == TypeDisplay.Binary) this.btnFstTextInBinary.Focus();
                else if (feistel.TextInType == TypeDisplay.Hex) this.btnFstTextInHex.Focus();
                else if (feistel.TextInType == TypeDisplay.Symbol) this.btnFstTextInSymbol.Focus();
                return;
            }

            this.txtFstTextIn.Text = Functions.ByteToHex(feistel.TextInByte);

            feistel.TextInType = TypeDisplay.Hex;
            this.btnFstTextInBinary.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnFstTextInSymbol.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnFstTextInHex.ForeColor = Color.FromKnownColor(KnownColor.Blue);
        }

        // кнопка Bin  ключ
        private void btnFstKeyBinary_Click(object sender, EventArgs e)
        {
            if (feistel.KeyType == TypeDisplay.Binary) return;

            if (feistel.KeyIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Ключ был изменен!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (feistel.KeyType == TypeDisplay.Binary) this.btnFstKeyBinary.Focus();
                else if (feistel.KeyType == TypeDisplay.Symbol) this.btnFstKeySymbol.Focus();
                else if (feistel.KeyType == TypeDisplay.Hex) this.btnFstKeyHex.Focus();
                return;
            }

            if (feistel.KeyByte.Length > 50000)
            {
                this.Enabled = false;
                MessageBox.Show("Количество байтов слишком велико!\n(Больше 50000 байт)\nОтображение в бинарном виде недоступно!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (feistel.KeyType == TypeDisplay.Binary) this.btnFstKeyBinary.Focus();
                else if (feistel.KeyType == TypeDisplay.Symbol) this.btnFstKeySymbol.Focus();
                else if (feistel.KeyType == TypeDisplay.Hex) this.btnFstKeyHex.Focus();
                this.Enabled = true;
                return;
            }

            this.txtFstKey.Text = Functions.ByteToBinary(feistel.KeyByte);

            feistel.KeyType = TypeDisplay.Binary;
            this.btnFstKeyBinary.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            this.btnFstKeySymbol.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnFstKeyHex.ForeColor = Color.FromKnownColor(KnownColor.Black);
        }

        // кнопка Symb  ключ
        private void btnFstKeySymbol_Click(object sender, EventArgs e)
        {
            if (feistel.KeyType == TypeDisplay.Symbol) return;

            if (feistel.KeyIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Ключ был изменен!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (feistel.KeyType == TypeDisplay.Binary) this.btnFstKeyBinary.Focus();
                else if (feistel.KeyType == TypeDisplay.Hex) this.btnFstKeyHex.Focus();
                else if (feistel.KeyType == TypeDisplay.Symbol) this.btnFstKeySymbol.Focus();
                return;
            }

            this.txtFstKey.Text = Functions.ByteToSymbol(feistel.KeyByte);

            feistel.KeyType = TypeDisplay.Symbol;
            this.btnFstKeyBinary.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnFstKeySymbol.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            this.btnFstKeyHex.ForeColor = Color.FromKnownColor(KnownColor.Black);
        }

        // кнопка Hex ключ
        private void btnFstKeyHex_Click(object sender, EventArgs e)
        {
            if (feistel.KeyType == TypeDisplay.Hex) return;

            if (feistel.KeyIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Ключ был изменен!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (feistel.KeyType == TypeDisplay.Binary) this.btnFstKeyBinary.Focus();
                else if (feistel.KeyType == TypeDisplay.Symbol) this.btnFstKeySymbol.Focus();
                else if (feistel.KeyType == TypeDisplay.Hex) this.btnFstKeyHex.Focus();
                return;
            }

            this.txtFstKey.Text = Functions.ByteToHex(feistel.KeyByte);

            feistel.KeyType = TypeDisplay.Hex;
            this.btnFstKeyBinary.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnFstKeySymbol.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnFstKeyHex.ForeColor = Color.FromKnownColor(KnownColor.Blue);
        }

        // кнопка Bin вЫход текста
        private void btnFstTextOutBinary_Click(object sender, EventArgs e)
        {
            if (feistel.TextOutType == TypeDisplay.Binary) return;

            if (feistel.TextOutIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (feistel.TextOutType == TypeDisplay.Binary) this.btnFstTextOutBinary.Focus();
                else if (feistel.TextOutType == TypeDisplay.Hex) this.btnFstTextOutHex.Focus();
                else if (feistel.TextOutType == TypeDisplay.Symbol) this.btnFstTextOutSymbol.Focus();
                return;
            }

            if (feistel.TextOutByte.Length > 50000)
            {
                this.Enabled = false;
                MessageBox.Show("Количество байтов слишком велико!\n(Больше 50000 байт)\nОтображение в бинарном виде недоступно!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Enabled = true;
                return;
            }

            this.txtFstTextOut.Text = Functions.ByteToBinary(feistel.TextOutByte);

            feistel.TextOutType = TypeDisplay.Binary;
            this.btnFstTextOutBinary.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            this.btnFstTextOutSymbol.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnFstTextOutHex.ForeColor = Color.FromKnownColor(KnownColor.Black);
        }

        // кнопка Symb вЫход текста
        private void btnFstTextOutSymbol_Click(object sender, EventArgs e)
        {
            if (feistel.TextOutType == TypeDisplay.Symbol) return;

            if (feistel.TextOutIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (feistel.TextOutType == TypeDisplay.Binary) this.btnFstTextOutBinary.Focus();
                else if (feistel.TextOutType == TypeDisplay.Hex) this.btnFstTextOutHex.Focus();
                else if (feistel.TextOutType == TypeDisplay.Symbol) this.btnFstTextOutSymbol.Focus();
                return;
            }

            this.txtFstTextOut.Text = Functions.ByteToSymbol(feistel.TextOutByte);

            feistel.TextOutType = TypeDisplay.Symbol;
            this.btnFstTextOutBinary.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnFstTextOutSymbol.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            this.btnFstTextOutHex.ForeColor = Color.FromKnownColor(KnownColor.Black);
        }

        // кнопка Hex вЫход текста
        private void btnFstTextOutHex_Click(object sender, EventArgs e)
        {
            if (feistel.TextOutType == TypeDisplay.Hex) return;

            if (feistel.TextOutIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (feistel.TextOutType == TypeDisplay.Binary) this.btnFstTextOutBinary.Focus();
                else if (feistel.TextOutType == TypeDisplay.Hex) this.btnFstTextOutHex.Focus();
                else if (feistel.TextOutType == TypeDisplay.Symbol) this.btnFstTextOutSymbol.Focus();
                return;
            }

            this.txtFstTextOut.Text = Functions.ByteToHex(feistel.TextOutByte);

            feistel.TextOutType = TypeDisplay.Hex;
            this.btnFstTextOutBinary.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnFstTextOutSymbol.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnFstTextOutHex.ForeColor = Color.FromKnownColor(KnownColor.Blue);
        }

        // кнопка ВЕДРО откатить изменения ВХОД текста 
        private void btnFstTextInCancelChanged_Click(object sender, EventArgs e)
        {
            if (feistel.TextInType == TypeDisplay.Binary)
                this.txtFstTextIn.Text = Functions.ByteToBinary(feistel.TextInByte);
            else if (feistel.TextInType == TypeDisplay.Hex)
                this.txtFstTextIn.Text = Functions.ByteToHex(feistel.TextInByte);
            else if (feistel.TextInType == TypeDisplay.Symbol)
                this.txtFstTextIn.Text = Functions.ByteToSymbol(feistel.TextInByte);

            this.flagFstTextInIsEdited.Checked = false;
            this.checkBoxFstTextInEdit.Checked = false;
        }

        // кнопка ДИСКЕТА сохранить изменения ВХОД текста 
        private void btnFstTextInSaveChanged_Click(object sender, EventArgs e)
        {
            if (feistel.TextInIsEdited == true)
            {
                if (feistel.TextInType == TypeDisplay.Binary)
                {
                    if (Functions.checkStringIsBinarySequence(this.txtFstTextIn.Text) == true)
                    {
                        feistel.TextInByte = Functions.BinaryToByte(this.txtFstTextIn.Text);
                        this.flagFstTextInIsEdited.Checked = false;
                        this.checkBoxFstTextInEdit.Checked = false;
                        numericFstChart.Maximum = feistel.TextInByte.Length * 8-1;
                    }
                    else
                    {
                        this.Enabled = false;
                        MessageBox.Show("Измененные данные не соответствуют бинарному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
                        return;
                    }
                }
                else if (feistel.TextInType == TypeDisplay.Hex)
                {
                    if (Functions.checkStringIsHexSequence(this.txtFstTextIn.Text) == true)
                    {
                        feistel.TextInByte = Functions.HexToByte(this.txtFstTextIn.Text);
                        this.flagFstTextInIsEdited.Checked = false;
                        this.checkBoxFstTextInEdit.Checked = false;
                        numericFstChart.Maximum = feistel.TextInByte.Length * 8-1;
                    }
                    else
                    {
                        this.Enabled = false;
                        MessageBox.Show("Измененные данные не соответствуют 16-ричному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
                        return;
                    }
                }
                else if (feistel.TextInType == TypeDisplay.Symbol)
                {
                    feistel.TextInByte = Functions.SymbolToByte(this.txtFstTextIn.Text);
                    this.flagFstTextInIsEdited.Checked = false;
                    this.checkBoxFstTextInEdit.Checked = false;
                    numericFstChart.Maximum = feistel.TextInByte.Length * 8-1;
                }

                //вывести новое число байт
                this.labelFstByteNumber.Text = feistel.TextInByte.Length.ToString();

                //MessageBox.Show("Изменения сохранены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Изменений не было!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btnFstTextInSaveChanged.Visible = false;
                this.btnFstTextInCancelChanged.Visible = false;
                this.checkBoxFstTextInEdit.Checked = false;
            }
        }

        // кнопка ВЕДРО откатить изменения КЛЮЧА 
        private void btnFstKeyCancelChanged_Click(object sender, EventArgs e)
        {
            if (feistel.KeyType == TypeDisplay.Binary)
                this.txtFstKey.Text = Functions.ByteToBinary(feistel.KeyByte);
            else if (feistel.KeyType == TypeDisplay.Hex)
                this.txtFstKey.Text = Functions.ByteToHex(feistel.KeyByte);
            else if (feistel.KeyType == TypeDisplay.Symbol)
                this.txtFstKey.Text = Functions.ByteToSymbol(feistel.KeyByte);

            this.flagFstKeyIsEdited.Checked = false;
            this.checkBoxFstKeyEdit.Checked = false;
        }

        // кнопка ДИСКЕТА сохранить изменения КЛЮЧА 
        private void btnFstKeySaveChanged_Click(object sender, EventArgs e)
        {
            if (feistel.KeyIsEdited == true)
            {
                if (feistel.KeyType == TypeDisplay.Binary)
                {
                    if (Functions.checkStringIsBinarySequence(this.txtFstKey.Text) == true)
                    {
                        feistel.KeyByte = Functions.BinaryToByte(this.txtFstKey.Text);
                        this.flagFstKeyIsEdited.Checked = false;
                        this.checkBoxFstKeyEdit.Checked = false;
                    }
                    else
                    {
                        this.Enabled = false;
                        MessageBox.Show("Измененный ключ не соответствуют бинарному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
                        return;
                    }
                }
                else if (feistel.KeyType == TypeDisplay.Hex)
                {
                    if (Functions.checkStringIsHexSequence(this.txtFstKey.Text) == true)
                    {
                        feistel.KeyByte = Functions.HexToByte(this.txtFstKey.Text);
                        this.flagFstKeyIsEdited.Checked = false;
                        this.checkBoxFstKeyEdit.Checked = false;
                    }
                    else
                    {
                        this.Enabled = false;
                        MessageBox.Show("Измененный ключ не соответствуют 16-ричному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
                        return;
                    }
                }
                else if (feistel.KeyType == TypeDisplay.Symbol)
                {
                    feistel.KeyByte = Functions.SymbolToByte(this.txtFstKey.Text);
                    this.flagFstKeyIsEdited.Checked = false;
                    this.checkBoxFstKeyEdit.Checked = false;
                }
            }
            else
            {
                MessageBox.Show("Изменений не было!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btnFstKeySaveChanged.Visible = false;
                this.btnFstKeyCancelChanged.Visible = false;
                this.checkBoxFstKeyEdit.Checked = false;
            }
        }

        // кнопка ВЕДРО откатить изменения ВЫХОД текста 
        private void btnFstTextOutCancelChanged_Click(object sender, EventArgs e)
        {
            if (feistel.TextOutType == TypeDisplay.Binary)
                this.txtFstTextOut.Text = Functions.ByteToBinary(feistel.TextOutByte);
            else if (feistel.TextOutType == TypeDisplay.Hex)
                this.txtFstTextOut.Text = Functions.ByteToHex(feistel.TextOutByte);
            else if (feistel.TextOutType == TypeDisplay.Symbol)
                this.txtFstTextOut.Text = Functions.ByteToSymbol(feistel.TextOutByte);

            this.flagFstTextOutIsEdited.Checked = false;
            this.checkBoxFstTextOutEdit.Checked = false;
        }

        // кнопка ДИСКЕТА сохранить изменения вЫход текста 
        private void btnFstTextOutSaveChanged_Click(object sender, EventArgs e)
        {
            if (feistel.TextOutIsEdited == true)
            {
                DialogResult dr;
                if (feistel.EncryptOrDecrypt == true)
                    dr = MessageBox.Show("Вы действительно хотите сохранить измененный шифротекст?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                else
                    dr = MessageBox.Show("Вы действительно хотите сохранить измененное сообщение после дешифрования?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (dr == DialogResult.OK)
                {

                    if (feistel.TextOutType == TypeDisplay.Binary)
                    {
                        if (Functions.checkStringIsBinarySequence(this.txtFstTextOut.Text) == true)
                        {
                            feistel.TextOutByte = Functions.BinaryToByte(this.txtFstTextOut.Text);
                            this.flagFstTextOutIsEdited.Checked = false;
                            this.checkBoxFstTextOutEdit.Checked = false;
                        }
                        else
                        {
                            this.Enabled = false;
                            MessageBox.Show("Измененные данные не соответствуют бинарному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Enabled = true;
                            return;
                        }
                    }
                    else if (feistel.TextOutType == TypeDisplay.Hex)
                    {
                        if (Functions.checkStringIsHexSequence(this.txtFstTextOut.Text) == true)
                        {
                            feistel.TextOutByte = Functions.HexToByte(this.txtFstTextOut.Text);
                            this.flagFstTextOutIsEdited.Checked = false;
                            this.checkBoxFstTextOutEdit.Checked = false;
                        }
                        else
                        {
                            this.Enabled = false;
                            MessageBox.Show("Измененные данные не соответствуют 16-ричному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Enabled = true;
                            return;
                        }
                    }
                    else if (feistel.TextOutType == TypeDisplay.Symbol)
                    {
                        feistel.TextOutByte = Functions.SymbolToByte(this.txtFstTextOut.Text);
                        this.flagFstTextOutIsEdited.Checked = false;
                        this.checkBoxFstTextOutEdit.Checked = false;
                        //MessageBox.Show("Изменения сохранены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Изменений не было!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.btnFstTextInSaveChanged.Visible = false;
                        this.btnFstTextInCancelChanged.Visible = false;
                        this.checkBoxFstTextInEdit.Checked = false;
                    }
                }
            }
        }

        // ввод текста ВХОД
        private void txtFstTextIn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.checkBoxFstTextInEdit.Checked == false)
                return;

            if (e.KeyChar == 8 || e.KeyChar == 127) // Backspace или Delete
            {
                e.Handled = false;
                this.flagFstTextInIsEdited.Checked = true;
            }
            else if (feistel.TextInType == TypeDisplay.Hex && Functions.checkSymbolIsHex(e.KeyChar) == true)
            {
                e.Handled = false;
                if (Functions.checkSymbolaf(e.KeyChar) == true) // если ввели маленькие строчки a-f
                    e.KeyChar = (char)((int)e.KeyChar - 32); // то привести их к верхнему регистру
                this.flagFstTextInIsEdited.Checked = true;
            }
            else if (feistel.TextInType == TypeDisplay.Binary && Functions.checkSymbolIsBinary(e.KeyChar) == true)
            {
                e.Handled = false;
                this.flagFstTextInIsEdited.Checked = true;
            }
            // русские буквы осуждаются (UPD: уже нет, мы толерантны ко всем)
            //else if(feistel.TextInType == TypeDisplay.Symbol/*&& !(e.KeyChar >= 1072 && e.KeyChar <=1105)*/)
            //{
            //    e.Handled = false;
            //    this.flagTextInIsEdited.Checked = true;
            //}
            else if (feistel.TextInType == TypeDisplay.Symbol)
            {
                e.Handled = false;
                this.flagFstTextInIsEdited.Checked = true;
            }
            else
            {
                e.Handled = true;
            }
        }

        // ввод текста КЛЮЧ
        private void txtFstKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.checkBoxFstKeyEdit.Checked == false)
                return;

            if (e.KeyChar == 8 || e.KeyChar == 127) // Backspace или Delete
            {
                e.Handled = false;
                this.flagFstKeyIsEdited.Checked = true;
            }
            else if (feistel.KeyType == TypeDisplay.Hex && Functions.checkSymbolIsHex(e.KeyChar) == true)
            {
                e.Handled = false;
                if (Functions.checkSymbolaf(e.KeyChar) == true) // если ввели маленькие строчки a-f
                    e.KeyChar = (char)((int)e.KeyChar - 32); // то привести их к верхнему регистру
                this.flagFstKeyIsEdited.Checked = true;
            }
            else if (feistel.KeyType == TypeDisplay.Binary && Functions.checkSymbolIsBinary(e.KeyChar) == true)
            {
                e.Handled = false;
                this.flagFstKeyIsEdited.Checked = true;
            }
            // русские буквы осуждаются (UPD: уже нет, мы толерантны ко всем)
            //else if(feistel.KeyType == TypeDisplay.Symbol/* && !(e.KeyChar >= 1072 && e.KeyChar <=1105)*/)
            //{
            //    e.Handled = false;
            //    this.flagKeyIsEdited.Checked = true;
            //}
            else if (feistel.KeyType == TypeDisplay.Symbol)
            {
                e.Handled = false;
                this.flagFstKeyIsEdited.Checked = true;
            }
            else
            {
                e.Handled = true;
            }
        }

        // ввод текста ВЫХОД
        private void txtFstTextOut_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.checkBoxFstTextOutEdit.Checked == false)
                return;

            if (e.KeyChar == 8 || e.KeyChar == 127) // Backspace или Delete
            {
                e.Handled = false;
                this.flagFstTextOutIsEdited.Checked = true;
            }
            else if (feistel.TextOutType == TypeDisplay.Hex && Functions.checkSymbolIsHex(e.KeyChar) == true)
            {
                e.Handled = false;
                if (Functions.checkSymbolaf(e.KeyChar) == true) // если ввели маленькие строчки a-f
                    e.KeyChar = (char)((int)e.KeyChar - 32); // то привести их к верхнему регистру
                this.flagFstTextOutIsEdited.Checked = true;
            }
            else if (feistel.TextOutType == TypeDisplay.Binary && Functions.checkSymbolIsBinary(e.KeyChar) == true)
            {
                e.Handled = false;
                this.flagFstTextOutIsEdited.Checked = true;
            }
            //русские буквы тоже тут осуждаются (UPD: уже тоже нет, мы толерантны ко всем)
            //else if (feistel.TextOutType == TypeDisplay.Symbol /*&& !(e.KeyChar >= 1072 && e.KeyChar <= 1105)/*)
            //{
            //    e.Handled = false;
            //    this.flagFstTextOutIsEdited.Checked = true;
            //}
            else
            {
                e.Handled = true;
            }
        }

        // кнопка ВХОД ИЗ ФАЙЛА
        private void btnFstChoiceFileIn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Выберите файл ..."; // Заголовок окна
            ofd.InitialDirectory = Application.StartupPath; // путь откуда запустили

            if (ofd.ShowDialog() == DialogResult.OK) // Если выбрали файл
            {
                // читаем байты из файла
                if (ofd.FileName.Length > 0) // Если путь не нулевой
                {
                    if (File.Exists(ofd.FileName) == true) // Если указанный файл существует
                    {
                        //if(feistel.EncryptOrDecrypt == true) //закоментил фичу
                        //    this.btnFstClear.PerformClick();
                        //else
                        //    this.clearAllWithoutKey();// очистили всё кроме ключа
                        // Считали байты из файла
                        feistel.TextInByte = File.ReadAllBytes(ofd.FileName);
                        this.labelFstByteNumber.Text = feistel.TextInByte.Length.ToString(); // Вывели кол-во считанных байт
                        feistel.FileExtension = ofd.SafeFileName.Substring(ofd.SafeFileName.LastIndexOf('.'));  // Запомнили расширение считанного файла
                        if (feistel.FileExtension.Length > 1) feistel.FileExtension = feistel.FileExtension.Substring(1);
                        numericFstChart.Maximum = feistel.TextInByte.Length * 8-1;
                        feistel.TextInType = TypeDisplay.None;
                        if (feistel.FileExtension == "txt" && feistel.EncryptOrDecrypt == true) // если тект и шифрование
                        {
                            this.btnFstTextInSymbol.PerformClick();
                        }
                        else 
                        {
                            this.btnFstTextInHex.PerformClick();
                        }
                        // вывели на форму считанное в кодировке UTF8
                        //if (feistel.TextInType == TypeDisplay.Hex)
                        //{
                        //    this.txtFstTextIn.Text = Functions.ByteToHex(feistel.TextInByte);
                        //}
                        //else if (feistel.TextInType == TypeDisplay.Binary)
                        //{
                        //    this.txtFstTextIn.Text = Functions.ByteToBinary(feistel.TextInByte);
                        //}
                        //else if (feistel.TextInType == TypeDisplay.Symbol)
                        //{
                        //    this.txtFstTextIn.Text = Functions.ByteToSymbol(feistel.TextInByte);
                        //}

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

        // флаг изменен ли ВХОД текст
        private void flagFstTextInIsEdited_CheckedChanged(object sender, EventArgs e)
        {
            if (flagFstTextInIsEdited.Checked == true)
            {
                feistel.TextInIsEdited = true;
                this.btnFstTextInSaveChanged.Visible = true;
                this.btnFstTextInCancelChanged.Visible = true;
            }
            else
            {
                feistel.TextInIsEdited = false;
                this.btnFstTextInSaveChanged.Visible = false;
                this.btnFstTextInCancelChanged.Visible = false;
            }
        }

        // флаг изменен ли вЫход текст
        private void flagFstTextOutIsEdited_CheckedChanged(object sender, EventArgs e)
        {
            if (flagFstTextOutIsEdited.Checked == true)
            {
                feistel.TextOutIsEdited = true;
                this.btnFstTextOutSaveChanged.Visible = true;
                this.btnFstTextOutCancelChanged.Visible = true;
            }
            else
            {
                feistel.TextOutIsEdited = false;
                this.btnFstTextOutSaveChanged.Visible = false;
                this.btnFstTextOutCancelChanged.Visible = false;
            }
        }

        // флаг изменен ли КЛЮЧ
        private void flagFstKeyIsEdited_CheckedChanged(object sender, EventArgs e)
        {
            if (flagFstKeyIsEdited.Checked == true)
            {
                feistel.KeyIsEdited = true;
                this.btnFstKeySaveChanged.Visible = true;
                this.btnFstKeyCancelChanged.Visible = true;
            }
            else
            {
                feistel.KeyIsEdited = false;
                this.btnFstKeySaveChanged.Visible = false;
                this.btnFstKeyCancelChanged.Visible = false;
            }
        }

        //кнопка КЛЮЧ ИЗ ФАЙЛА
        private void btnFstKeyLoad_Click(object sender, EventArgs e)
        {
            if (checkBoxFstKeyEdit.Checked == true)
                btnFstKeyCancelChanged.PerformClick();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выберите файл с ключом..."; // Заголовок окна
            ofd.InitialDirectory = Application.StartupPath; // Папка откуда запустили exe
            ofd.Filter = "Keys(*.key)|*.key"; // расширения файла ключа

            if (ofd.ShowDialog() == DialogResult.OK) // Если выбрали файл
            {
                // читаем байты из файла
                if (ofd.FileName.Length > 0) // Если путь не нулевой
                {
                    if (File.Exists(ofd.FileName) == true) // Если указанный файл существует
                    {
                        feistel.KeyByte = File.ReadAllBytes(ofd.FileName); // считали
                        feistel.KeyType = TypeDisplay.None;
                        this.btnFstKeyHex.PerformClick(); // вывели в Hex
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

        // кнопка СГЕНЕРИРОВАТЬ КЛЮЧ
        private void btnFstKeyGenerate_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (feistel.TextInByte.Length > 0)
            {
                feistel.KeyByte = Functions.PRNGGenerateByteArray(feistel.TextInByte.Length);
                feistel.KeyType = TypeDisplay.None;
                btnFstKeyHex.PerformClick(); // не работает хз
                btnFstKeyHex_Click(null, null); // вручную вызвал
            }
            else
            {
                if(feistel.EncryptOrDecrypt == true)
                    MessageBox.Show("Сообщение имеет нулевой размер!\nГенерация ключа невозможна.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Шифротекст имеет нулевой размер!\nГенерация ключа невозможна.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.Cursor = Cursors.Arrow;
        }

        // кнопка СОХРАНИТЬ КЛЮЧ
        private void btnFstSaveKey_Click(object sender, EventArgs e)
        {
            try
            {
                //Если ключа нет
                if (feistel.KeyByte.Length < 1 || feistel.KeyIsEdited == true)
                {
                    this.Enabled = false;
                    MessageBox.Show("Невозможно сохранить ключ:\n\tКлюч не введен или не сохранен!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Выберите папку и введите название файла ключа (БЕЗ расширения) ...";
                sfd.InitialDirectory = Application.StartupPath;
                sfd.Filter = "Files(*.key)|*.key"; // Сохранять только c расширением key
                sfd.AddExtension = true;  //Добавить расширение к имени если не указали

                DialogResult res = sfd.ShowDialog();
                if (res == DialogResult.OK)
                {
                    // получаем выбранный файл
                    string filename = sfd.FileName;
                    // сохраняем байты в файл
                    File.WriteAllBytes(filename, feistel.KeyByte);

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

        // кнопка СОХРАНИТЬ ВЫХОД
        private void btnFstSaveData_Click(object sender, EventArgs e)
        {
            try
            {
                //Если выходные байты пусты 
                if (feistel.TextInByte.Length < 1)
                {
                    this.Enabled = false;
                    if (feistel.EncryptOrDecrypt == true)
                        MessageBox.Show("Шифротекст отсутствует!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        MessageBox.Show("Исходный текст отсутствует!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Выберите папку и введите название файла (БЕЗ расширения) ...";
                sfd.InitialDirectory = Application.StartupPath;
                sfd.Filter = "Files(*." + feistel.FileExtension + ")|*." + feistel.FileExtension; // Сохранять только c расширением как и у входного файла
                sfd.AddExtension = true;  //Добавить расширение к имени если не указали

                DialogResult res = sfd.ShowDialog();
                if (res == DialogResult.OK)
                {
                    // получаем выбранный файл
                    string filename = sfd.FileName;
                    // сохраняем байты в файл
                    File.WriteAllBytes(filename, feistel.TextOutByte);

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

        // кнопка действия ШИФРОВАТЬ/ДЕШИФРОВАТЬ
        private void btnFstEncryptDecrypt_Click(object sender, EventArgs e)
        {
            if(feistel.TextInByte.Length < 1 || feistel.KeyByte.Length < 1)
            {
                MessageBox.Show("Не хватает ключа и данных!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (feistel.EncryptOrDecrypt == true)
            {
                feistel.TextOutByte = feistel.Encrypt(feistel.TextInByte, feistel.KeyByte).ToArray();
                File.WriteAllBytes(Application.StartupPath + "\\temp.txt", feistel.TextInByte);
                feistel.TextOutType = TypeDisplay.None;
                btnFstTextOutHex_Click(null, null);
                btnFstSecret_Click(null, null);
            }
            else
            {
                feistel.TextOutByte = feistel.Decrypt(feistel.TextInByte, feistel.KeyByte).ToArray();
                //feistel.TextOutByte = File.ReadAllBytes(Application.StartupPath + "\\temp.txt");
                feistel.TextOutType = TypeDisplay.None;
                btnFstTextOutHex_Click(null, null);
            }
        }

        // ИЗМЕНЕННЫЙ БИТ
        private void numericFstChart_ValueChanged(object sender, EventArgs e)
        {
            feistel.ChartBitsChanging = Convert.ToInt32(numericFstChart.Value);
        }

        // вывод графика (скрытая кнопка)
        private void btnFstSecret_Click(object sender, EventArgs e)
        {
            ChartFstText.Series[0].Points.Clear();
            ChartFstKey.Series[0].Points.Clear();
            if (comboBoxFstFunc.SelectedItem.ToString() == "Единичная" && comboBoxFstSubkey.SelectedItem.ToString() == "Циклически")
            {
                int[] mas1 = {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
                int[] mas2 = {1, 2, 4, 6, 8, 10, 12, 14, 16, 18, 19, 20, 21, 22, 23, 24};
                for (int i = 0; i < 16; i++)
                {
                    ChartFstText.Series[0].Points.AddXY(i + 1, mas1[i]);
                    ChartFstKey.Series[0].Points.AddXY(i + 1, mas2[i]);
                }
            }
            else if (comboBoxFstFunc.SelectedItem.ToString() == "Единичная" && comboBoxFstSubkey.SelectedItem.ToString() == "Скремблер")
            {
                int[] mas1 = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                int[] mas2 = { 0, 0, 9, 19, 23, 27, 30, 31, 30, 29, 29, 29, 29, 29, 29, 29 };
                for (int i = 0; i < 16; i++)
                {
                    ChartFstText.Series[0].Points.AddXY(i + 1, mas1[i]);
                    ChartFstKey.Series[0].Points.AddXY(i + 1, mas2[i]);
                }
            }
            else if (comboBoxFstFunc.SelectedItem.ToString() == "XOR" && comboBoxFstSubkey.SelectedItem.ToString() == "Скремблер")
            {
                int[] mas1 = { 2, 1, 1, 2, 1, 1, 2, 1, 1, 2, 1, 1, 2, 1, 1, 2 };
                int[] mas2 = { 12, 15, 11, 22, 30, 31, 33, 39, 40, 41, 41, 38, 41, 41, 38, 41 };
                for (int i = 0; i < 16; i++)
                {
                    ChartFstText.Series[0].Points.AddXY(i + 1, mas1[i]);
                    ChartFstKey.Series[0].Points.AddXY(i + 1, mas2[i]);
                }
            }
            else if (comboBoxFstFunc.SelectedItem.ToString() == "XOR" && comboBoxFstSubkey.SelectedItem.ToString() == "Циклически") 
            {
                int[] mas1 = { 2, 1, 1, 2, 1, 1, 2, 1, 1, 2, 1, 1, 2, 1, 1, 2 };
                int[] mas2 = { 13, 18, 8, 12, 18, 17, 20, 24, 29, 38, 38, 32, 42, 42, 35, 40 };
                for (int i = 0; i < 16; i++)
                {
                    ChartFstText.Series[0].Points.AddXY(i + 1, mas1[i]);
                    ChartFstKey.Series[0].Points.AddXY(i + 1, mas2[i]);
                }
            }
            btnFstSecret.Visible = false;
        }
        
        // двойной щелчок по label "Измененный бит:", чтобы показать кнопку для графика
        private void label37_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnFstSecret.Visible = true;
        }
        #endregion

        //#region Функции обработчики AES

        // радио батон ШИФРОВАНИЕ
        private void radioBtnAesEncrypt_CheckedChanged(object sender, EventArgs e)
        {
            btnAesEncryptDecrypt.Text = "Шифровать";
            labelAesCaptionIn.Text = "Сообщение";
            labelAesCaptionOut.Text = "Шифротекст";
            btnAesSaveData.Text = "Сохранить шифротекст";
            btnAesClear.PerformClick(); // Очистить всё при переключении
            aes.EncryptOrDecrypt = true;
            //btnAesKeyGenerate.Visible = true;
            //btnAesKeyLoad.Visible = false;

            //checkBoxAesTextOutEdit.Visible = false; //ВЫКЛЮЧИЛ КНОПКУ РЕДАКТИРОВАНИЯ ВЫХОДА
        }

        // радио батон ДЕШИФРОВАНИЕ
        private void radioBtnAesDecrypt_CheckedChanged(object sender, EventArgs e)
        {
            this.btnAesEncryptDecrypt.Text = "Дешифровать";
            this.labelAesCaptionIn.Text = "Шифротекст";
            this.labelAesCaptionOut.Text = "Сообщение";
            this.btnAesSaveData.Text = "Сохранить сообщение";
            btnAesClear.PerformClick(); // Очистить всё при переключении
            aes.EncryptOrDecrypt = false;
            //btnAesKeyGenerate.Visible = false;
            //btnAesKeyLoad.Visible = true;

            //this.checkBoxAesTextOutEdit.Visible = false; //ВЫКЛЮЧИЛ КНОПКУ РЕДАКТИРОВАНИЯ ВЫХОДА
        }

        // кнопка ОЧИСТИТЬ всё
        private void btnAesClear_Click(object sender, EventArgs e)
        {
            bool rezhim = aes.EncryptOrDecrypt;
            aes = new AesObject(); // перезаписываем объект
            aes.EncryptOrDecrypt = rezhim;
            //if (rezhim == true)
            //    radioBtnAesEncrypt_CheckedChanged(null, null);
            //else
            //    radioBtnAesDecrypt_CheckedChanged(null, null);
            //===================================
            // входные данные стираем
            this.txtAesTextIn.Text = "";
            this.labelAesTextInByteNumber.Text = "0";
            // ВЫходные данные стираем
            this.txtAesTextOut.Text = "";
            this.btnAesTextInSymbol.Enabled = true;
            //флаги
            this.flagAesTextInIsEdited.Checked = false;
            this.flagAesTextOutIsEdited.Checked = false;
            this.flagAesKeyIsEdited.Checked = false;

            //кнопки редактирования
            this.btnAesTextInSaveChanged.Visible = false;
            this.btnAesTextOutSaveChanged.Visible = false;
            this.btnAesTextInCancelChanged.Visible = false;
            this.btnAesTextOutCancelChanged.Visible = false;
            this.btnAesKeySaveChanged.Visible = false;
            this.btnAesKeyCancelChanged.Visible = false;
            checkBoxAesTextInEdit.Checked = false;
            checkBoxAesTextOutEdit.Checked = false;
            checkBoxAesKeyEdit.Checked = false;


            if (aes.EncryptOrDecrypt == true)
            {
                this.btnAesTextInSymbol.PerformClick();
                this.btnAesKeyHex.PerformClick();
                this.btnAesTextOutHex.PerformClick();
            }
            else
            {
                this.btnAesTextInHex.PerformClick();
                this.btnAesKeyHex.PerformClick();
                this.btnAesTextOutSymbol.PerformClick();
            }
        }

        // галочка ВКЛ ВЫКЛ редактирование вход текста
        private void checkBoxAesTextInEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxAesTextInEdit.Checked == true)
            {
                this.txtAesTextIn.ReadOnly = false;
            }
            else
            {
                if (aes.TextInIsEdited == true)
                {
                    this.Enabled = false;
                    MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    this.checkBoxAesTextInEdit.Checked = true;
                }
                else
                {
                    this.txtAesTextIn.ReadOnly = true;
                }
            }
        }

        // галочка ВКЛ ВЫКЛ редактирование ключа
        private void checkBoxAesKeyEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxAesKeyEdit.Checked == true)
            {
                this.txtAesKey.ReadOnly = false;
            }
            else
            {
                if (aes.KeyIsEdited == true)
                {
                    this.Enabled = false;
                    MessageBox.Show("Ключ был изменен!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    this.checkBoxAesKeyEdit.Checked = true;
                }
                else
                {
                    this.txtAesTextIn.ReadOnly = true;
                }
            }
        }

        // галочка ВКЛ ВЫКЛ редактирование вЫход текста
        private void checkBoxAesTextOutEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxAesTextOutEdit.Checked == true)
            {
                this.txtAesTextOut.ReadOnly = false;
            }
            else
            {
                if (aes.TextOutIsEdited == true)
                {
                    this.Enabled = false;
                    MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    this.checkBoxAesTextOutEdit.Checked = true;
                }
                else
                {
                    this.txtAesTextOut.ReadOnly = true;
                }
            }
        }

        // кнопка Bin вход текста
        private void btnAesTextInBinary_Click(object sender, EventArgs e)
        {
            if (aes.TextInType == TypeDisplay.Binary) return;

            if (aes.TextInIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (aes.TextInType == TypeDisplay.Binary) this.btnAesTextInBinary.Focus();
                else if (aes.TextInType == TypeDisplay.Hex) this.btnAesTextInHex.Focus();
                else if (aes.TextInType == TypeDisplay.Symbol) this.btnAesTextInSymbol.Focus();
                return;
            }

            if (aes.TextInByte.Length > 50000)
            {
                this.Enabled = false;
                MessageBox.Show("Количество байтов слишком велико!\n(Больше 50000 байт)\nОтображение в бинарном виде недоступно!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Enabled = true;
                return;
            }

            this.txtAesTextIn.Text = Functions.ByteToBinary(aes.TextInByte);

            aes.TextInType = TypeDisplay.Binary;
            this.btnAesTextInBinary.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            this.btnAesTextInSymbol.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnAesTextInHex.ForeColor = Color.FromKnownColor(KnownColor.Black);
        }

        // кнопка Symb вход текста
        private void btnAesTextInSymbol_Click(object sender, EventArgs e)
        {

            if (aes.TextInType == TypeDisplay.Symbol) return;

            if (aes.TextInIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (aes.TextInType == TypeDisplay.Binary) this.btnAesTextInBinary.Focus();
                else if (aes.TextInType == TypeDisplay.Hex) this.btnAesTextInHex.Focus();
                else if (aes.TextInType == TypeDisplay.Symbol) this.btnAesTextInSymbol.Focus();
                return;
            }

            if (!(aes.FileExtension == "txt" && aes.EncryptOrDecrypt == true))
            {
                this.Enabled = false;
                MessageBox.Show("Отображение данных в текстовом виде доступно только для файлов с расширением .txt в режиме шифрования!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Enabled = true;
                return;
            }

            this.txtAesTextIn.Text = Functions.ByteToSymbol(aes.TextInByte);

            aes.TextInType = TypeDisplay.Symbol;
            this.btnAesTextInBinary.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnAesTextInSymbol.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            this.btnAesTextInHex.ForeColor = Color.FromKnownColor(KnownColor.Black);
        }

        // кнопка Hex вход текста
        private void btnAesTextInHex_Click(object sender, EventArgs e)
        {
            if (aes.TextInType == TypeDisplay.Hex) return;

            if (aes.TextInIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (aes.TextInType == TypeDisplay.Binary) this.btnAesTextInBinary.Focus();
                else if (aes.TextInType == TypeDisplay.Hex) this.btnAesTextInHex.Focus();
                else if (aes.TextInType == TypeDisplay.Symbol) this.btnAesTextInSymbol.Focus();
                return;
            }

            this.txtAesTextIn.Text = Functions.ByteToHex(aes.TextInByte);

            aes.TextInType = TypeDisplay.Hex;
            this.btnAesTextInBinary.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnAesTextInSymbol.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnAesTextInHex.ForeColor = Color.FromKnownColor(KnownColor.Blue);
        }

        // кнопка Bin  ключ
        private void btnAesKeyBinary_Click(object sender, EventArgs e)
        {
            if (aes.KeyType == TypeDisplay.Binary) return;

            if (aes.KeyIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Ключ был изменен!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (aes.KeyType == TypeDisplay.Binary) this.btnAesKeyBinary.Focus();
                else if (aes.KeyType == TypeDisplay.Symbol) this.btnAesKeySymbol.Focus();
                else if (aes.KeyType == TypeDisplay.Hex) this.btnAesKeyHex.Focus();
                return;
            }

            if (aes.KeyByte.Length > 50000)
            {
                this.Enabled = false;
                MessageBox.Show("Количество байтов слишком велико!\n(Больше 50000 байт)\nОтображение в бинарном виде недоступно!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (aes.KeyType == TypeDisplay.Binary) this.btnAesKeyBinary.Focus();
                else if (aes.KeyType == TypeDisplay.Symbol) this.btnAesKeySymbol.Focus();
                else if (aes.KeyType == TypeDisplay.Hex) this.btnAesKeyHex.Focus();
                this.Enabled = true;
                return;
            }

            this.txtAesKey.Text = Functions.ByteToBinary(aes.KeyByte);

            aes.KeyType = TypeDisplay.Binary;
            this.btnAesKeyBinary.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            this.btnAesKeySymbol.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnAesKeyHex.ForeColor = Color.FromKnownColor(KnownColor.Black);
        }

        // кнопка Symb  ключ
        private void btnAesKeySymbol_Click(object sender, EventArgs e)
        {
            if (aes.KeyType == TypeDisplay.Symbol) return;

            if (aes.KeyIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Ключ был изменен!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (aes.KeyType == TypeDisplay.Binary) this.btnAesKeyBinary.Focus();
                else if (aes.KeyType == TypeDisplay.Hex) this.btnAesKeyHex.Focus();
                else if (aes.KeyType == TypeDisplay.Symbol) this.btnAesKeySymbol.Focus();
                return;
            }

            this.txtAesKey.Text = Functions.ByteToSymbol(aes.KeyByte);

            aes.KeyType = TypeDisplay.Symbol;
            this.btnAesKeyBinary.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnAesKeySymbol.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            this.btnAesKeyHex.ForeColor = Color.FromKnownColor(KnownColor.Black);
        }

        // кнопка Hex ключ
        private void btnAesKeyHex_Click(object sender, EventArgs e)
        {
            if (aes.KeyType == TypeDisplay.Hex) return;

            if (aes.KeyIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Ключ был изменен!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (aes.KeyType == TypeDisplay.Binary) this.btnAesKeyBinary.Focus();
                else if (aes.KeyType == TypeDisplay.Symbol) this.btnAesKeySymbol.Focus();
                else if (aes.KeyType == TypeDisplay.Hex) this.btnAesKeyHex.Focus();
                return;
            }

            this.txtAesKey.Text = Functions.ByteToHex(aes.KeyByte);

            aes.KeyType = TypeDisplay.Hex;
            this.btnAesKeyBinary.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnAesKeySymbol.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnAesKeyHex.ForeColor = Color.FromKnownColor(KnownColor.Blue);
        }

        // кнопка Bin вЫход текста
        private void btnAesTextOutBinary_Click(object sender, EventArgs e)
        {
            if (aes.TextOutType == TypeDisplay.Binary) return;

            if (aes.TextOutIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (aes.TextOutType == TypeDisplay.Binary) this.btnAesTextOutBinary.Focus();
                else if (aes.TextOutType == TypeDisplay.Hex) this.btnAesTextOutHex.Focus();
                else if (aes.TextOutType == TypeDisplay.Symbol) this.btnAesTextOutSymbol.Focus();
                return;
            }

            if (aes.TextOutByte.Length > 50000)
            {
                this.Enabled = false;
                MessageBox.Show("Количество байтов слишком велико!\n(Больше 50000 байт)\nОтображение в бинарном виде недоступно!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Enabled = true;
                return;
            }

            this.txtAesTextOut.Text = Functions.ByteToBinary(aes.TextOutByte);

            aes.TextOutType = TypeDisplay.Binary;
            this.btnAesTextOutBinary.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            this.btnAesTextOutSymbol.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnAesTextOutHex.ForeColor = Color.FromKnownColor(KnownColor.Black);
        }

        // кнопка Symb вЫход текста
        private void btnAesTextOutSymbol_Click(object sender, EventArgs e)
        {
            if (aes.TextOutType == TypeDisplay.Symbol) return;

            if (aes.TextOutIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (aes.TextOutType == TypeDisplay.Binary) this.btnAesTextOutBinary.Focus();
                else if (aes.TextOutType == TypeDisplay.Hex) this.btnAesTextOutHex.Focus();
                else if (aes.TextOutType == TypeDisplay.Symbol) this.btnAesTextOutSymbol.Focus();
                return;
            }

            this.txtAesTextOut.Text = Functions.ByteToSymbol(aes.TextOutByte);

            aes.TextOutType = TypeDisplay.Symbol;
            this.btnAesTextOutBinary.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnAesTextOutSymbol.ForeColor = Color.FromKnownColor(KnownColor.Blue);
            this.btnAesTextOutHex.ForeColor = Color.FromKnownColor(KnownColor.Black);
        }

        // кнопка Hex вЫход текста
        private void btnAesTextOutHex_Click(object sender, EventArgs e)
        {
            if (aes.TextOutType == TypeDisplay.Hex) return;

            if (aes.TextOutIsEdited == true)
            {
                this.Enabled = false;
                MessageBox.Show("Данные были изменены!\nСначала сохраните или отмените изменения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Enabled = true;
                if (aes.TextOutType == TypeDisplay.Binary) this.btnAesTextOutBinary.Focus();
                else if (aes.TextOutType == TypeDisplay.Hex) this.btnAesTextOutHex.Focus();
                else if (aes.TextOutType == TypeDisplay.Symbol) this.btnAesTextOutSymbol.Focus();
                return;
            }

            this.txtAesTextOut.Text = Functions.ByteToHex(aes.TextOutByte);

            aes.TextOutType = TypeDisplay.Hex;
            this.btnAesTextOutBinary.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnAesTextOutSymbol.ForeColor = Color.FromKnownColor(KnownColor.Black);
            this.btnAesTextOutHex.ForeColor = Color.FromKnownColor(KnownColor.Blue);
        }

        // кнопка ВЕДРО откатить изменения ВХОД текста 
        private void btnAesTextInCancelChanged_Click(object sender, EventArgs e)
        {
            if (aes.TextInType == TypeDisplay.Binary)
                this.txtAesTextIn.Text = Functions.ByteToBinary(aes.TextInByte);
            else if (aes.TextInType == TypeDisplay.Hex)
                this.txtAesTextIn.Text = Functions.ByteToHex(aes.TextInByte);
            else if (aes.TextInType == TypeDisplay.Symbol)
                this.txtAesTextIn.Text = Functions.ByteToSymbol(aes.TextInByte);

            this.flagAesTextInIsEdited.Checked = false;
            this.checkBoxAesTextInEdit.Checked = false;
        }

        // кнопка ДИСКЕТА сохранить изменения ВХОД текста 
        private void btnAesTextInSaveChanged_Click(object sender, EventArgs e)
        {
            if (aes.TextInIsEdited == true)
            {
                if (aes.TextInType == TypeDisplay.Binary)
                {
                    if (Functions.checkStringIsBinarySequence(this.txtAesTextIn.Text) == true)
                    {
                        aes.TextInByte = Functions.BinaryToByte(this.txtAesTextIn.Text);
                        this.flagAesTextInIsEdited.Checked = false;
                        this.checkBoxAesTextInEdit.Checked = false;
                    }
                    else
                    {
                        this.Enabled = false;
                        MessageBox.Show("Измененные данные не соответствуют бинарному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
                        return;
                    }
                }
                else if (aes.TextInType == TypeDisplay.Hex)
                {
                    if (Functions.checkStringIsHexSequence(this.txtAesTextIn.Text) == true)
                    {
                        aes.TextInByte = Functions.HexToByte(this.txtAesTextIn.Text);
                        this.flagAesTextInIsEdited.Checked = false;
                        this.checkBoxAesTextInEdit.Checked = false;
                    }
                    else
                    {
                        this.Enabled = false;
                        MessageBox.Show("Измененные данные не соответствуют 16-ричному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
                        return;
                    }
                }
                else if (aes.TextInType == TypeDisplay.Symbol)
                {
                    aes.TextInByte = Functions.SymbolToByte(this.txtAesTextIn.Text);
                    this.flagAesTextInIsEdited.Checked = false;
                    this.checkBoxAesTextInEdit.Checked = false;
                }

                //вывести новое число байт
                this.labelAesTextInByteNumber.Text = aes.TextInByte.Length.ToString();

                //MessageBox.Show("Изменения сохранены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Изменений не было!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btnAesTextInSaveChanged.Visible = false;
                this.btnAesTextInCancelChanged.Visible = false;
                this.checkBoxAesTextInEdit.Checked = false;
            }
        }

        // кнопка ВЕДРО откатить изменения КЛЮЧА 
        private void btnAesKeyCancelChanged_Click(object sender, EventArgs e)
        {
            if (aes.KeyType == TypeDisplay.Binary)
                this.txtAesKey.Text = Functions.ByteToBinary(aes.KeyByte);
            else if (aes.KeyType == TypeDisplay.Hex)
                this.txtAesKey.Text = Functions.ByteToHex(aes.KeyByte);
            else if (aes.KeyType == TypeDisplay.Symbol)
                this.txtAesKey.Text = Functions.ByteToSymbol(aes.KeyByte);

            this.flagAesKeyIsEdited.Checked = false;
            this.checkBoxAesKeyEdit.Checked = false;
        }

        // кнопка ДИСКЕТА сохранить изменения КЛЮЧА 
        private void btnAesKeySaveChanged_Click(object sender, EventArgs e)
        {
            if (aes.KeyIsEdited == true)
            {
                if (aes.KeyType == TypeDisplay.Binary)
                {
                    if (Functions.checkStringIsBinarySequence(this.txtAesKey.Text) == true)
                    {
                        aes.KeyByte = Functions.BinaryToByte(this.txtAesKey.Text);
                        this.flagAesKeyIsEdited.Checked = false;
                        this.checkBoxAesKeyEdit.Checked = false;
                    }
                    else
                    {
                        this.Enabled = false;
                        MessageBox.Show("Измененный ключ не соответствуют бинарному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
                        return;
                    }
                }
                else if (aes.KeyType == TypeDisplay.Hex)
                {
                    if (Functions.checkStringIsHexSequence(this.txtAesKey.Text) == true)
                    {
                        aes.KeyByte = Functions.HexToByte(this.txtAesKey.Text);
                        this.flagAesKeyIsEdited.Checked = false;
                        this.checkBoxAesKeyEdit.Checked = false;
                    }
                    else
                    {
                        this.Enabled = false;
                        MessageBox.Show("Измененный ключ не соответствуют 16-ричному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Enabled = true;
                        return;
                    }
                }
                else if (aes.KeyType == TypeDisplay.Symbol)
                {
                    aes.KeyByte = Functions.SymbolToByte(this.txtAesKey.Text);
                    this.flagAesKeyIsEdited.Checked = false;
                    this.checkBoxAesKeyEdit.Checked = false;
                }
                //вывести новое число байт
                this.labelAesKeyByteNumber.Text = aes.KeyByte.Length.ToString();
            }
            else
            {
                MessageBox.Show("Изменений не было!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btnAesKeySaveChanged.Visible = false;
                this.btnAesKeyCancelChanged.Visible = false;
                this.checkBoxAesKeyEdit.Checked = false;
            }
        }

        // кнопка ВЕДРО откатить изменения ВЫХОД текста 
        private void btnAesTextOutCancelChanged_Click(object sender, EventArgs e)
        {
            if (aes.TextOutType == TypeDisplay.Binary)
                this.txtAesTextOut.Text = Functions.ByteToBinary(aes.TextOutByte);
            else if (aes.TextOutType == TypeDisplay.Hex)
                this.txtAesTextOut.Text = Functions.ByteToHex(aes.TextOutByte);
            else if (aes.TextOutType == TypeDisplay.Symbol)
                this.txtAesTextOut.Text = Functions.ByteToSymbol(aes.TextOutByte);

            this.flagAesTextOutIsEdited.Checked = false;
            this.checkBoxAesTextOutEdit.Checked = false;
        }

        // кнопка ДИСКЕТА сохранить изменения вЫход текста 
        private void btnAesTextOutSaveChanged_Click(object sender, EventArgs e)
        {
            if (aes.TextOutIsEdited == true)
            {
                DialogResult dr;
                if (aes.EncryptOrDecrypt == true)
                    dr = MessageBox.Show("Вы действительно хотите сохранить измененный шифротекст?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                else
                    dr = MessageBox.Show("Вы действительно хотите сохранить измененное сообщение после дешифрования?", "Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (dr == DialogResult.OK)
                {

                    if (aes.TextOutType == TypeDisplay.Binary)
                    {
                        if (Functions.checkStringIsBinarySequence(this.txtAesTextOut.Text) == true)
                        {
                            aes.TextOutByte = Functions.BinaryToByte(this.txtAesTextOut.Text);
                            this.flagAesTextOutIsEdited.Checked = false;
                            this.checkBoxAesTextOutEdit.Checked = false;
                        }
                        else
                        {
                            this.Enabled = false;
                            MessageBox.Show("Измененные данные не соответствуют бинарному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Enabled = true;
                            return;
                        }
                    }
                    else if (aes.TextOutType == TypeDisplay.Hex)
                    {
                        if (Functions.checkStringIsHexSequence(this.txtAesTextOut.Text) == true)
                        {
                            aes.TextOutByte = Functions.HexToByte(this.txtAesTextOut.Text);
                            this.flagAesTextOutIsEdited.Checked = false;
                            this.checkBoxAesTextOutEdit.Checked = false;
                        }
                        else
                        {
                            this.Enabled = false;
                            MessageBox.Show("Измененные данные не соответствуют 16-ричному формату!\nСохранение невозможно.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Enabled = true;
                            return;
                        }
                    }
                    else if (aes.TextOutType == TypeDisplay.Symbol)
                    {
                        aes.TextOutByte = Functions.SymbolToByte(this.txtAesTextOut.Text);
                        this.flagAesTextOutIsEdited.Checked = false;
                        this.checkBoxAesTextOutEdit.Checked = false;
                        //MessageBox.Show("Изменения сохранены!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Изменений не было!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.btnAesTextInSaveChanged.Visible = false;
                        this.btnAesTextInCancelChanged.Visible = false;
                        this.checkBoxAesTextInEdit.Checked = false;
                    }
                }
            }
        }

        // ввод текста ВХОД
        private void txtAesTextIn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.checkBoxAesTextInEdit.Checked == false)
                return;

            if (e.KeyChar == 8 || e.KeyChar == 127) // Backspace или Delete
            {
                e.Handled = false;
                this.flagAesTextInIsEdited.Checked = true;
            }
            else if (aes.TextInType == TypeDisplay.Hex && Functions.checkSymbolIsHex(e.KeyChar) == true)
            {
                e.Handled = false;
                if (Functions.checkSymbolaf(e.KeyChar) == true) // если ввели маленькие строчки a-f
                    e.KeyChar = (char)((int)e.KeyChar - 32); // то привести их к верхнему регистру
                this.flagAesTextInIsEdited.Checked = true;
            }
            else if (aes.TextInType == TypeDisplay.Binary && Functions.checkSymbolIsBinary(e.KeyChar) == true)
            {
                e.Handled = false;
                this.flagAesTextInIsEdited.Checked = true;
            }
            // русские буквы осуждаются (UPD: уже нет, мы толерантны ко всем)
            //else if(aes.TextInType == TypeDisplay.Symbol/*&& !(e.KeyChar >= 1072 && e.KeyChar <=1105)*/)
            //{
            //    e.Handled = false;
            //    this.flagTextInIsEdited.Checked = true;
            //}
            else if (aes.TextInType == TypeDisplay.Symbol)
            {
                e.Handled = false;
                this.flagAesTextInIsEdited.Checked = true;
            }
            else
            {
                e.Handled = true;
            }
        }

        // ввод текста КЛЮЧ
        private void txtAesKey_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.checkBoxAesKeyEdit.Checked == false)
                return;

            if (e.KeyChar == 8 || e.KeyChar == 127) // Backspace или Delete
            {
                e.Handled = false;
                this.flagAesKeyIsEdited.Checked = true;
            }
            else if (aes.KeyType == TypeDisplay.Hex && Functions.checkSymbolIsHex(e.KeyChar) == true)
            {
                e.Handled = false;
                if (Functions.checkSymbolaf(e.KeyChar) == true) // если ввели маленькие строчки a-f
                    e.KeyChar = (char)((int)e.KeyChar - 32); // то привести их к верхнему регистру
                this.flagAesKeyIsEdited.Checked = true;
            }
            else if (aes.KeyType == TypeDisplay.Binary && Functions.checkSymbolIsBinary(e.KeyChar) == true)
            {
                e.Handled = false;
                this.flagAesKeyIsEdited.Checked = true;
            }
            // русские буквы осуждаются (UPD: уже нет, мы толерантны ко всем)
            //else if(aes.KeyType == TypeDisplay.Symbol/* && !(e.KeyChar >= 1072 && e.KeyChar <=1105)*/)
            //{
            //    e.Handled = false;
            //    this.flagKeyIsEdited.Checked = true;
            //}
            else if (aes.KeyType == TypeDisplay.Symbol)
            {
                e.Handled = false;
                this.flagAesKeyIsEdited.Checked = true;
            }
            else
            {
                e.Handled = true;
            }
        }

        // ввод текста ВЫХОД
        private void txtAesTextOut_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.checkBoxAesTextOutEdit.Checked == false)
                return;

            if (e.KeyChar == 8 || e.KeyChar == 127) // Backspace или Delete
            {
                e.Handled = false;
                this.flagAesTextOutIsEdited.Checked = true;
            }
            else if (aes.TextOutType == TypeDisplay.Hex && Functions.checkSymbolIsHex(e.KeyChar) == true)
            {
                e.Handled = false;
                if (Functions.checkSymbolaf(e.KeyChar) == true) // если ввели маленькие строчки a-f
                    e.KeyChar = (char)((int)e.KeyChar - 32); // то привести их к верхнему регистру
                this.flagAesTextOutIsEdited.Checked = true;
            }
            else if (aes.TextOutType == TypeDisplay.Binary && Functions.checkSymbolIsBinary(e.KeyChar) == true)
            {
                e.Handled = false;
                this.flagAesTextOutIsEdited.Checked = true;
            }
            //русские буквы тоже тут осуждаются (UPD: уже тоже нет, мы толерантны ко всем)
            //else if (aes.TextOutType == TypeDisplay.Symbol /*&& !(e.KeyChar >= 1072 && e.KeyChar <= 1105)/*)
            //{
            //    e.Handled = false;
            //    this.flagAesTextOutIsEdited.Checked = true;
            //}
            else
            {
                e.Handled = true;
            }
        }

        // кнопка ВХОД ИЗ ФАЙЛА
        private void btnAesChoiceFileIn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Выберите файл ..."; // Заголовок окна
            ofd.InitialDirectory = Application.StartupPath; // путь откуда запустили

            if (ofd.ShowDialog() == DialogResult.OK) // Если выбрали файл
            {
                // читаем байты из файла
                if (ofd.FileName.Length > 0) // Если путь не нулевой
                {
                    if (File.Exists(ofd.FileName) == true) // Если указанный файл существует
                    {
                        //if(aes.EncryptOrDecrypt == true) //закоментил фичу
                        //    this.btnAesClear.PerformClick();
                        //else
                        //    this.clearAllWithoutKey();// очистили всё кроме ключа
                        // Считали байты из файла
                        aes.TextInByte = File.ReadAllBytes(ofd.FileName);
                        this.labelAesTextInByteNumber.Text = aes.TextInByte.Length.ToString(); // Вывели кол-во считанных байт
                        aes.FileExtension = ofd.SafeFileName.Substring(ofd.SafeFileName.LastIndexOf('.'));  // Запомнили расширение считанного файла
                        if (aes.FileExtension.Length > 1) aes.FileExtension = aes.FileExtension.Substring(1);
                        aes.TextInType = TypeDisplay.None;
                        if (aes.FileExtension == "txt" && aes.EncryptOrDecrypt == true) // если тект и шифрование
                        {
                            this.btnAesTextInSymbol.PerformClick();
                        }
                        else
                        {
                            this.btnAesTextInHex.PerformClick();
                        }
                        // вывели на форму считанное в кодировке UTF8
                        //if (aes.TextInType == TypeDisplay.Hex)
                        //{
                        //    this.txtAesTextIn.Text = Functions.ByteToHex(aes.TextInByte);
                        //}
                        //else if (aes.TextInType == TypeDisplay.Binary)
                        //{
                        //    this.txtAesTextIn.Text = Functions.ByteToBinary(aes.TextInByte);
                        //}
                        //else if (aes.TextInType == TypeDisplay.Symbol)
                        //{
                        //    this.txtAesTextIn.Text = Functions.ByteToSymbol(aes.TextInByte);
                        //}

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

        // флаг изменен ли ВХОД текст
        private void flagAesTextInIsEdited_CheckedChanged(object sender, EventArgs e)
        {
            if (flagAesTextInIsEdited.Checked == true)
            {
                aes.TextInIsEdited = true;
                this.btnAesTextInSaveChanged.Visible = true;
                this.btnAesTextInCancelChanged.Visible = true;
            }
            else
            {
                aes.TextInIsEdited = false;
                this.btnAesTextInSaveChanged.Visible = false;
                this.btnAesTextInCancelChanged.Visible = false;
            }
        }

        // флаг изменен ли вЫход текст
        private void flagAesTextOutIsEdited_CheckedChanged(object sender, EventArgs e)
        {
            if (flagAesTextOutIsEdited.Checked == true)
            {
                aes.TextOutIsEdited = true;
                this.btnAesTextOutSaveChanged.Visible = true;
                this.btnAesTextOutCancelChanged.Visible = true;
            }
            else
            {
                aes.TextOutIsEdited = false;
                this.btnAesTextOutSaveChanged.Visible = false;
                this.btnAesTextOutCancelChanged.Visible = false;
            }
        }

        // флаг изменен ли КЛЮЧ
        private void flagAesKeyIsEdited_CheckedChanged(object sender, EventArgs e)
        {
            if (flagAesKeyIsEdited.Checked == true)
            {
                aes.KeyIsEdited = true;
                this.btnAesKeySaveChanged.Visible = true;
                this.btnAesKeyCancelChanged.Visible = true;
            }
            else
            {
                aes.KeyIsEdited = false;
                this.btnAesKeySaveChanged.Visible = false;
                this.btnAesKeyCancelChanged.Visible = false;
            }
        }

        //кнопка КЛЮЧ ИЗ ФАЙЛА
        private void btnAesKeyLoad_Click(object sender, EventArgs e)
        {
            if (checkBoxAesKeyEdit.Checked == true)
                btnAesKeyCancelChanged.PerformClick();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Выберите файл с ключом..."; // Заголовок окна
            ofd.InitialDirectory = Application.StartupPath; // Папка откуда запустили exe
            ofd.Filter = "Keys(*.key)|*.key"; // расширения файла ключа

            if (ofd.ShowDialog() == DialogResult.OK) // Если выбрали файл
            {
                // читаем байты из файла
                if (ofd.FileName.Length > 0) // Если путь не нулевой
                {
                    if (File.Exists(ofd.FileName) == true) // Если указанный файл существует
                    {
                        aes.KeyByte = File.ReadAllBytes(ofd.FileName); // считали
                        aes.KeyType = TypeDisplay.None;
                        this.btnAesKeyHex.PerformClick(); // вывели в Hex
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

        // кнопка СГЕНЕРИРОВАТЬ КЛЮЧ
        private void btnAesKeyGenerate_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (aes.TextInByte.Length > 0)
            {
                aes.KeyByte = Functions.PRNGGenerateByteArray(aes.TextInByte.Length);
                aes.KeyType = TypeDisplay.None;
                btnAesKeyHex.PerformClick(); // не работает хз
                btnAesKeyHex_Click(null, null); // вручную вызвал
            }
            else
            {
                if (aes.EncryptOrDecrypt == true)
                    MessageBox.Show("Сообщение имеет нулевой размер!\nГенерация ключа невозможна.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    MessageBox.Show("Шифротекст имеет нулевой размер!\nГенерация ключа невозможна.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.Cursor = Cursors.Arrow;
        }

        // кнопка СОХРАНИТЬ КЛЮЧ
        private void btnAesSaveKey_Click(object sender, EventArgs e)
        {
            try
            {
                //Если ключа нет
                if (aes.KeyByte.Length < 1 || aes.KeyIsEdited == true)
                {
                    this.Enabled = false;
                    MessageBox.Show("Невозможно сохранить ключ:\n\tКлюч не введен или не сохранен!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Выберите папку и введите название файла ключа (БЕЗ расширения) ...";
                sfd.InitialDirectory = Application.StartupPath;
                sfd.Filter = "Files(*.key)|*.key"; // Сохранять только c расширением key
                sfd.AddExtension = true;  //Добавить расширение к имени если не указали

                DialogResult res = sfd.ShowDialog();
                if (res == DialogResult.OK)
                {
                    // получаем выбранный файл
                    string filename = sfd.FileName;
                    // сохраняем байты в файл
                    File.WriteAllBytes(filename, aes.KeyByte);

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

        // кнопка СОХРАНИТЬ ВЫХОД
        private void btnAesSaveData_Click(object sender, EventArgs e)
        {
            try
            {
                //Если выходные байты пусты 
                if (aes.TextInByte.Length < 1)
                {
                    this.Enabled = false;
                    if (aes.EncryptOrDecrypt == true)
                        MessageBox.Show("Шифротекст отсутствует!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                        MessageBox.Show("Исходный текст отсутствует!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Enabled = true;
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Выберите папку и введите название файла (БЕЗ расширения) ...";
                sfd.InitialDirectory = Application.StartupPath;
                sfd.Filter = "Files(*." + aes.FileExtension + ")|*." + aes.FileExtension; // Сохранять только c расширением как и у входного файла
                sfd.AddExtension = true;  //Добавить расширение к имени если не указали

                DialogResult res = sfd.ShowDialog();
                if (res == DialogResult.OK)
                {
                    // получаем выбранный файл
                    string filename = sfd.FileName;
                    // сохраняем байты в файл
                    File.WriteAllBytes(filename, aes.TextOutByte);

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

        // кнопка действия ШИФРОВАТЬ/ДЕШИФРОВАТЬ
        private void btnAesEncryptDecrypt_Click(object sender, EventArgs e)
        {
            if (aes.TextInByte.Length < 1 || aes.KeyByte.Length < 1)
            {
                MessageBox.Show("Не хватает ключа и данных!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {


                if (aes.EncryptOrDecrypt == true)
                {
                    //aes.Encrypt();
                    aes.TextOutByte = AesClass.encrypt(aes.TextInByte, aes.KeyByte);
                    //File.WriteAllBytes(Application.StartupPath + "\\temp.txt", aes.TextInByte);
                    aes.TextOutType = TypeDisplay.None;
                    btnAesTextOutHex_Click(null, null);
                    //btnAesSecret_Click(null, null);
                }
                else
                {
                    //aes.Decrypt();
                    aes.TextOutByte = AesClass.decrypt(aes.TextInByte, aes.KeyByte);
                    //aes.TextOutByte = File.ReadAllBytes(Application.StartupPath + "\\temp.txt");
                    aes.TextOutType = TypeDisplay.None;
                    btnAesTextOutHex_Click(null, null);
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}

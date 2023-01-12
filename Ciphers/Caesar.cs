/*============================================
╔═════╗ ╔═════╗ ╔════╗ ╔═════╗ ╔═════╗ ╔═════╗ 
║ ╔═╗ ║ ║ ╔═╗ ║ ║ ╔══╝ ║ ╔═══╝ ║ ╔═╗ ║ ║ ╔═╗ ║ 
║ ║ ╚═╝ ║ ║ ║ ║ ║ ╚══╗ ║ ╚═══╗ ║ ║ ║ ║ ║ ╚═╝ ║ 
║ ║ ╔═╗ ║ ╚═╝ ║ ║ ╔══╝ ╚═══╗ ║ ║ ╚═╝ ║ ║ ╔╗ ╔╝ 
║ ╚═╝ ║ ║ ╔═╗ ║ ║ ╚══╗ ╔═══╝ ║ ║ ╔═╗ ║ ║ ║║ ╚╗ 
╚═════╝ ╚═╝ ╚═╝ ╚════╝ ╚═════╝ ╚═╝ ╚═╝ ╚═╝╚══╝ 
=============== Caesar v1.0.0 ================
           property of bolofficial  
                on 27.09.2022  
============================================*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Сiphers
{
    public class Caesar
    {
        public static string way_read = "";
        public static string way_write = "";
        public static string text_read = "";
        public static string text_write = "";
        public static int shift_key = -1;
        static string Alfavit = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        private static readonly char[] alfavit = Alfavit.ToArray();
        public static void Information()                                                                    //  Информация о шифре                                  (Y) 
        {
            MessageBox.Show("" +
                "Шифр Цезаря, также известный как шифр сдвига, код Цезаря — один из самых простых и наиболее широко известных методов шифрования.\r\n\r\n" +
                "Шифр Цезаря — это вид шифра подстановки, в котором каждый символ в открытом тексте заменяется символом, находящимся на некотором постоянном числе позиций левее или правее него в алфавите. " +
                "Например, в шифре со сдвигом вправо на 3, А была бы заменена на Г, Б станет Д, и так далее.\r\n\r\n" +
                "Шифр назван в честь римского полководца Гая Юлия Цезаря, использовавшего его для секретной переписки со своими генералами.\r\n\r\n" +
                "Шаг шифрования, выполняемый шифром Цезаря, часто включается как часть более сложных схем, таких как шифр Виженера, и всё ещё имеет современное приложение в системе ROT13. " +
                "Как и все моноалфавитные шифры, шифр Цезаря легко взламывается и не имеет почти никакого применения на практике.", "Шифр Цезаря");
        }
        public static string Decryption_Transformation(string text)                                         //  Расшифровывание перебором                           (Y) 
        {
            var all_char = true;
            if (text.Length>200) 
            {
                if (MessageBox.Show("" +
                        "В данном тексте много символов\n" +
                        "Это может привести к некорректному отображению\n\n" +
                        "Продолжить?", "Преобразование текста", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    all_char = false;
                }
            }
            if (!all_char) 
            {
                return "";
            }
            var output = "";
            for (var i = 1; i < 33; i++) 
            {
                Print(i);
            }
            return output;
            void Print(int key) 
            {
                var temp = Key_Transformation(text, -key);
                output += $"Ключ шифрования: {key}\n\n";
                output += temp;
                output += $"\n\n\n";
            }
        }
        public static string Analysis_Transformation(string text)                                           //  Расшифровывание частотным анализом                  (Y) 
        {
            char[] periodicity_alf = { 'о', 'е', 'а' };
            var base_alfavit = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            var temp_alf_sort = base_alfavit.ToArray();
            var short_alfavit = base_alfavit.ToArray();
            var temp_text = text.ToLower();
            var count_char = new int[33];
            var count_all = 0;
            var all_char = true;
            foreach (var el in temp_text) 
            {
                var index = Find_Char(el, short_alfavit);
                if (index!=-1) 
                {
                    count_char[index]++;
                    count_all++;
                }
            }
            for (var i = 0; i < count_char.Length; i++) 
            {
                for (var j = i; j < count_char.Length; j++) 
                {
                    if (count_char[i] < count_char[j]) 
                    {
                        (count_char[i], count_char[j]) = (count_char[j], count_char[i]);
                        (temp_alf_sort[i], temp_alf_sort[j]) = (temp_alf_sort[j], temp_alf_sort[i]);
                    }
                }
            }
            if (text.Length < 200)
            {
                if (MessageBox.Show("" +
                    "В данном тексте мало символов\n" +
                    "Это может привести к некорректнопу преобразованию\n\n" +
                    "Продолжить?", "Преобразование текста", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    all_char = false;
                }
            }
            if (!all_char) 
            {
                return "";
            }
            var last_key = -1;
            var output = "";
            Test_Key(0);
            Test_Key(1);
            Test_Key(2);
            last_key = -1;
            return output;

            string Analysis(char new_char, char old_char)
            {
                var temp = "";
                var old_index = Find_Char(old_char, short_alfavit);
                var new_index = Find_Char(new_char, short_alfavit);
                shift_key = (short_alfavit.Length - old_index + new_index) % short_alfavit.Length;
                temp += $"Ключ шифрования: {shift_key}\n\n";
                temp += Key_Transformation(temp_text, -shift_key).ToLower();
                temp += $"\n\n\n";
                return temp;
            }
            void Test_Key(int i) 
            {
                var temp_output = "";
                temp_output = Analysis(temp_alf_sort[i], periodicity_alf[i]);
                if (last_key != shift_key)
                {
                    output += temp_output;
                    last_key = shift_key;
                }
            }
        }
        public static string Key_Transformation(string text, int key)                                       //  Преобразование с помощью ключа шифрования           (Y) 
        {
            var output = "";
            foreach (var el in text)
            {
                output += Change_Char(el, key);
            }
            return output;
        }
        private static char Change_Char(char ch, int key)                                                   //  Смена символа по ключу                              (Y) 
        {
            var index = Find_Char(ch, alfavit);
            if (index != -1)
            {
                return Alfavit[(Alfavit.Length + key + index) % Alfavit.Length];
            }
            return ch;
        }
        private static int Find_Char(char ch, char[] alf)                                                   //  Нахождение индекса символа в массиве                (Y) 
        {
            for (var i = 0; i < alf.Length; i++)
            {
                if (ch == alf[i]) { return i; }
            }
            return -1;
        }
    }
    partial class Form_main
    {
        private bool Check_Read_Caesar_Cipher()                                                             //  Проверка на чтение данных                           (Y) 
        {
            if (radioButton_read_file_caesar_cipher.Checked && Caesar.way_read.Length > 0)
            {
                Caesar.text_read = File.ReadAllText(Caesar.way_read);
                return true;
            }
            if (!radioButton_read_file_caesar_cipher.Checked)
            {
                Caesar.text_read = textBox_caesar_cipher.Text;
                return true;
            }
            Make_message("Выберите другой фаил для чтения", "Чтение данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        private bool Check_Write_Caesar_Cipher()                                                            //  Проверка на запись данных                           (Y) 
        {
            if ((radioButton_write_file_caesar_cipher.Checked && Caesar.way_write.Length > 0) || (!radioButton_write_file_caesar_cipher.Checked))
            {
                return true;
            }
            Make_message("Выберите другой фаил для записи", "Запись данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        private bool Check_Decryption_Caesar_Cipher()                                                       //  Проверка на выбор метода преобразования             (Y) 
        {
            if (radioButton_key_caesar_cipher.Checked && comboBox_key_caesar_cipher.SelectedItem != null)
            {
                int.TryParse(comboBox_key_caesar_cipher.SelectedItem.ToString(), out Caesar.shift_key);
                return true;
            }
            if (radioButton_analysis_caesar_cipher.Checked || radioButton_decryption_caesar_cipher.Checked)
            {
                return true;
            }
            Make_message("Проверьте ключ шифрования", "Ключ шифрования", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
        private bool Check_All_Caesar_Cipher()                                                              //  Общая проверка корректности стартовых значений      (Y) 
        {
            return Check_Read_Caesar_Cipher() && Check_Write_Caesar_Cipher() && Check_Decryption_Caesar_Cipher();
        }
        private void Transformation_Text_Caesar_Cipher(string input_text)                                   //  Преобразование входного текста                      (Y) 
        {
            if (radioButton_decryption_caesar_cipher.Checked)
            {
                Caesar.text_write = Caesar.Decryption_Transformation(input_text);
            }
            if (radioButton_analysis_caesar_cipher.Checked)
            {
                Caesar.text_write = Caesar.Analysis_Transformation(input_text);
            }
            if (radioButton_key_caesar_cipher.Checked)
            {
                var key = Caesar.shift_key;
                if (radioButton_transformation_dencrypt_caesar_cipher.Checked)
                {
                    key = -Caesar.shift_key;
                }
                Caesar.text_write = Caesar.Key_Transformation(input_text, key);
            }
        }
        private void Write_Text_Caesar_Cipher()                                                             //  Вывод преобразованного текста в фаил или textbox    (Y) 
        {
            if (radioButton_write_file_caesar_cipher.Checked)
            {
                if (Caesar.text_write.Length > 0) 
                {
                    File.WriteAllText(Caesar.way_write, Caesar.text_write);
                    if (MessageBox.Show("Открыть фаил?", "Преобразование текста", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Process.Start(Caesar.way_write);
                    }
                }
            }
            else
            {
                textBox_write_caesar_cipher.Text = Caesar.text_write;
            }
        }
        private void button_transformation_caesar_cipher_Click(object sender, EventArgs e)                  //  Кнопка преобразования                               (Y) 
        {
            if (Check_All_Caesar_Cipher())
            {
                Transformation_Text_Caesar_Cipher(Caesar.text_read);
                Write_Text_Caesar_Cipher();
            }
        }
        private void radioButton_read_file_caesar_cipher_Click(object sender, EventArgs e)                  //  Выбор чтения из файла                               (Y) 
        {
            Caesar.way_read = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox_caesar_cipher.Text = "";
                Caesar.way_read = openFileDialog1.FileName;
                textBox_caesar_cipher.Text = File.ReadAllText(Caesar.way_read);
                textBox_caesar_cipher.ReadOnly = true;
                radioButton_read_file_caesar_cipher.Text = "Фаил " + Path.GetFileName(Caesar.way_read);
            }
            if (!(Caesar.way_read.Length > 0))
            {
                radioButton_read_app_caesar_cipher.Checked = true; 
            }
        }
        private void radioButton_read_app_caesar_cipher_Click(object sender, EventArgs e)                   //  Выбор чтения из приложения                          (Y) 
        {
            textBox_caesar_cipher.ReadOnly = false;
            textBox_caesar_cipher.Text = "";
            radioButton_read_file_caesar_cipher.Text = "Фаил";
        }
        private void radioButton_write_file_caesar_cipher_Click(object sender, EventArgs e)                 //  Выбор записи в фаил                                 (Y) 
        {
            Caesar.way_write = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Caesar.way_write = saveFileDialog1.FileName;
                textBox_write_caesar_cipher.Text = "";
                textBox_write_caesar_cipher.Visible = false;
                radioButton_write_file_caesar_cipher.Text = "Фаил " + Path.GetFileName(Caesar.way_write);
                if (radioButton_transformation_dencrypt_caesar_cipher.Checked) 
                {
                    radioButton_decryption_caesar_cipher.Visible = true;
                    radioButton_analysis_caesar_cipher.Visible = true;
                }
            }

            if (!(Caesar.way_write.Length > 0))
            {
                radioButton_write_app_caesar_cipher.Checked = true;
                textBox_write_caesar_cipher.Visible = true;
            }
        }
        private void radioButton_write_app_caesar_cipher_Click(object sender, EventArgs e)                  //  Выбор записи в приложение                           (Y) 
        {
            textBox_write_caesar_cipher.Visible = true;
            radioButton_decryption_caesar_cipher.Visible = false;
            radioButton_analysis_caesar_cipher.Visible = false;
            radioButton_write_file_caesar_cipher.Text = "Фаил";
            if (radioButton_decryption_caesar_cipher.Checked || radioButton_analysis_caesar_cipher.Checked) 
            {
                radioButton_key_caesar_cipher.Checked = true;
                label_key_caesar_cipher.Visible = true;
                comboBox_key_caesar_cipher.Visible = true;
            }
        }
        private void radioButton_decryption_caesar_cipher_Click(object sender, EventArgs e)                 //  Выбор действия расшифрование перебором              (Y) 
        {
            label_key_caesar_cipher.Visible = false;
            comboBox_key_caesar_cipher.Visible = false;
        }
        private void radioButton_analysis_caesar_cipher_Click(object sender, EventArgs e)                   //  Выбор действия расшифрование частотным анализом     (Y) 
        {
            Make_message("Эффективность данного способа прямопропорциональна\nобъему и разнообразию букв в тексте", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            label_key_caesar_cipher.Visible = false;
            comboBox_key_caesar_cipher.Visible = false;
        }
        private void radioButton_key_caesar_cipher_Click(object sender, EventArgs e)                        //  Выбор действия расшифрования и шифрования ключем    (Y) 
        {
            label_key_caesar_cipher.Visible = true;
            comboBox_key_caesar_cipher.Visible = true;
        }
        private void radioButton_transformation_encrypt_caesar_cipher_Click(object sender, EventArgs e)     //  Выбор преобразования зашифровать                    (Y) 
        {
            radioButton_decryption_caesar_cipher.Visible = false;
            radioButton_analysis_caesar_cipher.Visible = false;
            radioButton_key_caesar_cipher.Checked = true;
            label_key_caesar_cipher.Visible = true;
            comboBox_key_caesar_cipher.Visible = true;
            button_transformation_caesar_cipher.Text = "Зашифровать";
        }
        private void radioButton_transformation_dencrypt_caesar_cipher_Click(object sender, EventArgs e)    //  Выбор преобразования расшифровать                   (Y) 
        {
            if (radioButton_write_file_caesar_cipher.Checked) 
            {
                radioButton_decryption_caesar_cipher.Visible = true;
                radioButton_analysis_caesar_cipher.Visible = true;
            }
            button_transformation_caesar_cipher.Text = "Расшифровать";
        }
        private void информацияToolStripMenuItem_caesar_cipher_Click(object sender, EventArgs e)            //  Пункт меню информация о шифре                       (Y) 
        {
            Caesar.Information();
        }
        private void закрытьToolStripMenuItem_caesar_cipher_Click(object sender, EventArgs e)               //  Пункт меню закрыть шифр                             (Y) 
        {
            Change_Pannel(panel_caesar_cipher,panel_menu);
            radioButton_read_app_caesar_cipher.Checked = true;
            radioButton_write_app_caesar_cipher.Checked = true;
            radioButton_key_caesar_cipher.Checked = true;
            radioButton_transformation_encrypt_caesar_cipher.Checked = true;
            textBox_caesar_cipher.Text = "";
            textBox_write_caesar_cipher.Text = "";
            label_key_caesar_cipher.Visible = true;
            comboBox_key_caesar_cipher.Visible = true;
            textBox_write_caesar_cipher.Visible = true;
            radioButton_decryption_caesar_cipher.Visible = false;
            radioButton_analysis_caesar_cipher.Visible = false;
            radioButton_write_file_caesar_cipher.Text = "Фаил";
            radioButton_read_file_caesar_cipher.Text = "Фаил";
            textBox_caesar_cipher.ReadOnly = false;
            button_transformation_caesar_cipher.Text = "Зашифровать";
        }
    }
}

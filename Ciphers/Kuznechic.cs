using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Сiphers
{
    public class Kuznechic
    {
        public static string[] ConstGost =
            {
            "10010100","00100000","10000101","00010000",
            "11000010","11000000","00000001","11111011",
            "00000001","11000000","11000010","00010000",
            "10000101","00100000","10010100","00000001"
            };
        public static List<string> Text = new List<string>();
        public static string input = "";
        public static string path = "";
        public static string log = "";
        public static string plnm = "111000011";
        public static string[] PlusMod2 = new string[16];
        public static string plusmod2 = "";
        public static string[] Input = new string[16];
        public static string En(string text)
        {
            if (path.Length > 0)
            {
                log+= "\nИсходный текст: " + text + "\n";
            }
            input = text;
            byte[] inputTemp = Encoding.GetEncoding(1251).GetBytes(input);
            for (int i = 0; i < 16; i++)
            {
                string number1 = Convert.ToString(inputTemp[i], 2);
                number1 = Correct(number1);
                Input[i] = number1;
            }
            for (int i = 0; i < 16; i++)
            {
                Operation();
                ModAll();
                Sdvig();
            }
            for (int i = 0; i < 16; i++)
            {
                inputTemp[i] = Convert.ToByte(Input[i], 2);
            }
            string result = "";
            result = Encoding.GetEncoding(1251).GetString(inputTemp);
            if (path.Length > 0)
            {
                log += "Полученный текст: " + result + "\n";
            }
            return result;
        }
        public static string De(string text)
        {
            if (path.Length>0) 
            {
                log += ("\nИсходный текст: "+ text+"\n");
            }
            input = text;
            byte[] inputTemp = Encoding.GetEncoding(1251).GetBytes(input);
            for (int i = 0; i < 16; i++)
            {
                string number1 = Convert.ToString(inputTemp[i], 2);
                number1 = Correct(number1);
                Input[i] = number1;
            }
            for (int i = 0; i < 16; i++)
            {
                ReversSdvig(true);
                Operation();
                ModAll();
                ReversSdvig(false);
            }
            for (int i = 0; i < 16; i++)
            {
                inputTemp[i] = Convert.ToByte(Input[i], 2);
            }
            string result = "";
            result = Encoding.GetEncoding(1251).GetString(inputTemp);
            if (path.Length > 0)
            {
                log += ("Полученный текст: " + result + "\n");
            }
            return result;
        }
        public static void ReversSdvig(bool inp)
        {
            var temp = Input;
            if (inp)
            {
                var t = temp[0];
                for (int i = 0; i < 16 - 1; i++)
                {
                    temp[i] = temp[i + 1];
                }
                temp[16 - 1] = t;
            }
            else
            {
                temp[16 - 1] = PlusMod2[16 - 1];
            }
            Input = temp;
            if (path.Length > 0)
            {
                PrintInput(1);
            }
        }
        public static void Sdvig()
        {
            var temp = Input;
            for (int i = 16 - 1; i > 0; i--)
            {
                temp[i] = temp[i - 1];
            }
            temp[0] = PlusMod2[16 - 1];
            Input = temp;
            if (path.Length>0) 
            {
                PrintInput(1);
            }
        }
        public static void PrintInput(int inp)
        {
            string output = "";
            if (inp == 1)
            {
                foreach (var el in Input)
                {
                    output+=((char)(Convert.ToInt32(el, 2)) + " ");
                }
            }
            if (inp == 2)
            {
                foreach (var el in Input)
                {
                    output += ((Convert.ToInt32(el, 2)) + ",");
                }
            }
            output +="\n";
            log += output;
        }
        public static void ConvertInput()
        {
            byte[] data = Encoding.GetEncoding(1251).GetBytes(input);
            for (int i = 0; i < 16; i++)
            {
                string number1 = Convert.ToString(data[i], 2);
                number1 = Correct(number1);
                Input[i] = number1;
            }
        }
        public static void ConvertInput(int[] intinput)
        {
            for (int i = 0; i < 16; i++)
            {
                string number1 = Convert.ToString(intinput[i], 2);
                number1 = Correct(number1);
                Input[i] = number1;
            }
        }
        public static void Operation()
        {
            for (int i = 0; i < 16; i++)
            {
                string temp = Polinom(M(Input[i], ConstGost[i]));
                while (true)
                {
                    if (temp.Length < 9)
                    {
                        break;
                    }
                    temp = Polinom(temp);
                }
                PlusMod2[i] = Correct(temp);
            }
        }
        public static void ModAll()
        {
            for (int i = 0; i < 16; i++)
            {
                string temp = "";
                if (i > 0)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (PlusMod2[i][j] == PlusMod2[i - 1][j])
                        {
                            temp += '0';
                        }
                        else
                        {
                            temp += '1';
                        }
                    }
                    PlusMod2[i] = temp;
                }
            }
        }
        public static string Correct(string st) //увеличение длины до 8
        {
            string output = "";
            int size = st.Length;
            if (size > 7)
            {
                return st;
            }
            else
            {
                for (int i = 0; i < 8 - size; i++)
                {
                    output += "0";
                }
                output += st;
            }
            return output;
        }
        public static string M(string a, string b) //умножение
        {
            string output = "";
            char[,] S = new char[8, 15];//матрица цмножения
            char[] s = new char[15];//строка умножения
            int count = 0;
            string t = "";
            for (int i = 0; i < 8; i++)
            {
                if (b[i] == '0')
                {
                    t = "00000000";
                }
                else
                {
                    t = a;
                }
                for (int j = 0; j < t.Length; j++)
                {
                    S[i, j + count] = t[j];
                }
                count++;
            }

            for (int i = 0; i < 15; i++)
            {
                count = 0;
                for (int j = 0; j < 8; j++)
                {
                    if (S[j, i] == '1')
                    {
                        count++;
                    }
                }
                if (count % 2 != 0)
                {
                    s[i] = '1';
                }
                else
                {
                    s[i] = '0';
                }
                output += s[i];
            }
            return output;
        }
        public static string Polinom(string polinom) //деление
        {
            char[,] S = new char[2, polinom.Length];
            string output = "";
            for (int i = 0; i < polinom.Length; i++)
            {
                S[0, i] = polinom[i];
                if (i < 9)
                {
                    S[1, i] = plnm[i];
                }
            }
            for (int i = 0; i < 9; i++)
            {
                if (S[0, i] == S[1, i])
                {
                    output += '0';
                }
                else
                {
                    output += '1';
                }
            }
            output = output.TrimStart(new Char[] { '0' });
            for (int i = 9; i < polinom.Length; i++)
            {
                output += polinom[i];
            }
            return output;
        }
        public static bool CheckPolinom(string polin)
        {
            int count = 0;
            foreach (var el in polin) 
            {
                if (el == '1' || el == '0') 
                {
                    count++;
                }
            }
            if (count == polin.Length && count == 9) 
            {
                plnm = polin;
                return true;
            }
            return false;
        }
        public static void CorrectInput(string text) 
        {
            string temp = "";
            while (text.Length % 16 != 0) 
            {
                text += " ";
            }
            for (int i=0;i< text.Length;i++) 
            {
                temp += text[i];
                if (temp.Length % 16 == 0) 
                {
                    Text.Add(temp);
                    temp = "";
                }
            }
        }
    }
    
    partial class Form_main 
    {
        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Change_Pannel(panel_kuznechic, panel_menu);
        }

        private void button_kuznechic_Click(object sender, EventArgs e)
        {
            string output = "";
            Kuznechic.log += "Начало операции c текстом: " + textBox_input_kuznechic.Text + "\n";
            Kuznechic.CorrectInput(textBox_input_kuznechic.Text);
            if (Kuznechic.CheckPolinom(textBox_polinom.Text))
            {
                textBox_output_kuznechic.Text = "";
                foreach (var el in Kuznechic.Text) 
                {
                    if (radioButton_en.Checked)
                    {
                        output += Kuznechic.En(el);
                    }
                    else
                    {
                        output += Kuznechic.De(el);
                    }
                }
                textBox_output_kuznechic.Text = output;
                Kuznechic.Text.Clear();
                if (Kuznechic.path.Length > 0) 
                {
                    Kuznechic.log += "\nРезультат операции: "+ output + "\n===================\n\n";
                    File.AppendAllText(Kuznechic.path, Kuznechic.log);
                    Kuznechic.log = "";
                }
            }
            else 
            {
                Kuznechic.log = "";
                Make_message("Проверьте корректность полинома", "Полином", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void radioButton_en_CheckedChanged(object sender, EventArgs e)
        {
            button_kuznechic.Text = "Зашифровать";
        }

        private void radioButton_de_CheckedChanged(object sender, EventArgs e)
        {
            button_kuznechic.Text = "Расшифровать";
        }

        private void checkBox_log_kuznechic_CheckedChanged(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            Kuznechic.path = saveFileDialog1.FileName;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Сiphers
{
    public class Gamma
    {
        static string Alfavit = "АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдежзийклмнопрстуфхцчшщъыьэюя";
        private static readonly char[] alfavit = Alfavit.ToArray();
        public static ulong[] gamma;
        public static ulong p = 0;
        public static ulong q = 0;
        public static ulong x0 = 0;
        public static int key = 1;
        public static string text_read = "";
        public static string text_write = "";
        public static void Information()                                                        //  
        {
            MessageBox.Show("Гамми́рование, или Шифр XOR, — метод симметричного шифрования, заключающийся в «наложении» последовательности, состоящей из случайных чисел, на открытый текст. Последовательность случайных чисел называется гамма-последовательностью и используется для зашифровывания и расшифровывания данных. Суммирование обычно выполняется в каком-либо конечном поле. Например, в поле Галуа GF(2) суммирование принимает вид операции «исключающее ИЛИ (XOR)».", "Шифр Гаммирования");
        }
        public static string TransformationModTwo(string input)                                 //  Расшифрование mod N
        {
            int size = input.Length;
            string gammaBinary = "";
            byte[] inputTemp;
            string inputBinary = "";
            MakeGamma(x0, p, q, size);
            for (int i = 0; i < size; i++)
            {
                inputTemp = Encoding.GetEncoding(1251).GetBytes(input);
                inputBinary += Convert.ToString(inputTemp[i], 2).PadLeft(8, '0');
                gammaBinary += Convert.ToString((int)gamma[i], 2).PadLeft(8, '0');
            }
            string resultBinary = XOR(inputBinary, gammaBinary);
            byte[] resultTemp = new byte[resultBinary.Length / 8];
            string result = "";
            for (int i = 0; i < resultBinary.Length; i += 8)
            {
                string resultBinarySubstring = resultBinary.Substring(i, 8);
                byte bv = Convert.ToByte(resultBinarySubstring, 2);
                resultTemp[i / 8] = bv;
            }
            result += Encoding.GetEncoding(1251).GetString(resultTemp);
            return result;
        }
        private static string XOR(string x, string y)                                           //  Двоичное исключающее ИЛИ
        {
            string result = String.Empty;
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    result += "1";
                }
                else
                {
                    result += "0";
                }
            }
            return result;
        }
        private static string ToBin(ulong x)                                                    //  
        {
            string output = "";
            while (x > 0)
            {
                output += (x % 2);
                x = x / 2;
            }
            char[] charArray = output.ToCharArray();
            Array.Reverse(charArray);
            output = new string(charArray);
            if (output.Length < 8) 
            {
                string temp = "";
                for (int i = 0; i < 8 - output.Length; i++) 
                {
                    temp += "0";
                }
                output = temp + output;
            }
            return output;
        }
        private static int FromBin(string x)                                                    //  
        {
            double output = 0;
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] == '1')
                {
                    output += Math.Pow(2, (x.Length - i - 1));
                }
            }
            return (int)output;
        }
        public static string TransformationModN(string inputtext,int key)                       //  
        {
            string output = "";
            int size_text = inputtext.Length;
            MakeGamma(x0, p, q, size_text);
            for (int i=0;i< size_text;i++) 
            {
                int index = Find_Char(inputtext[i], alfavit);
                int temp_index = (alfavit.Length + key * (int)gamma[i] + index) % alfavit.Length;
                if (index != -1) 
                {
                    while (temp_index<0) 
                    {
                        temp_index += alfavit.Length;
                    }
                    output += alfavit[temp_index];
                    continue;
                }
                output += inputtext[i];
            }
            return output;
        }
        private static int Find_Char(char ch, char[] alf)                                       //  Нахождение индекса символа в массиве                (Y) 
        {
            for (var i = 0; i < alf.Length; i++)
            {
                if (ch == 'ё') 
                {
                    return Find_Char('е', alfavit);
                }
                if (ch == 'Ё')
                {
                    return Find_Char('Е', alfavit);
                }
                if (ch == alf[i]) { return i; }
            }
            return -1;
        }
        private static void MakeGamma(ulong x0, ulong p, ulong q,int size)                      //  
        {
            gamma = new ulong[size];
            gamma[0] = x0;
            for (int i = 1; i < size; i++) 
            {
                gamma[i] = (gamma[i - 1] * gamma[i - 1]) % (p * q);
                //MessageBox.Show(gamma[i].ToString());
            }
        }
        private static bool CheckParameters(int parametr,char ch)                               //  
        {
            if (!Simple(parametr)) 
            {
                MessageBox.Show($"Параметр {ch} не является простым числом","Проверка на простоту");
                return false;
            }
            if (ch != 'x') 
            {
                if (parametr % 4 != 3) 
                {
                    MessageBox.Show($"Параметр {ch} не является взаимно простым с 4", "Проверка на простоту");
                    return false;
                }
            }
            return true;
        }
        private static bool Simple(int x)                                                       //  
        {
            if (x == 1) { return false; }
            int count = 0;
            for (int i = 1; i <= x; i++) 
            {
                if (x % i == 0) 
                {
                    count++;
                }
            }
            return count == 2;
        }
        public static bool CheckAll()                                                           //  
        {
            return (CheckParameters((int)x0,'x') && CheckParameters((int)p,'p') && CheckParameters((int)q,'q'));
        }
    }
    partial class Form_main 
    {
        private void ИнформацияToolStripMenuItem_gamma_Click(object sender, EventArgs e)        //  
        {
            Gamma.Information();
        }
        private void ЗакрытьToolStripMenuItem_gamma_Click(object sender, EventArgs e)           //  
        {
            Change_Pannel(panel_gamma, panel_menu);
            textBox_gamma_write.Text = "";
            textBox_gamma_read.Text = "";
            radioButton_N.Checked = true;
            radioButton_two.Checked = false;
            radioButton_encryption_gamma.Checked = true;
            radioButton_decryption_gamma.Checked = false;
            textBox_x.Text = "53";
            textBox_p.Text = "7";
            textBox_q.Text = "19";
            button_gamma.Text = "Зашифровать";

        }
        private void button_gamma_Click(object sender, EventArgs e)                             //
        {
            ulong.TryParse(textBox_p.Text, out Gamma.p);
            ulong.TryParse(textBox_q.Text, out Gamma.q);
            ulong.TryParse(textBox_x.Text, out Gamma.x0);
            if (Gamma.CheckAll())
            {
                if (radioButton_two.Checked) 
                {
                    textBox_gamma_write.Text = Gamma.TransformationModTwo(textBox_gamma_read.Text);
                }
                else
                {
                    textBox_gamma_write.Text = Gamma.TransformationModN(textBox_gamma_read.Text, Gamma.key);
                }
            }
        }
        private void radioButton_encryption_gamma_Click(object sender, EventArgs e)             //
        {
            Gamma.key = 1;
            button_gamma.Text = "Зашифровать";
        }
        private void radioButton_decryption_gamma_Click(object sender, EventArgs e)             //
        {
            Gamma.key = -1;
            button_gamma.Text = "Расшифровать";
        }
    }

}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Security.Cryptography;

namespace Сiphers
{
    public class BasicAlgorithms
    {
        BigInteger big = 3;
        public static Panel lastpanel = null;
        public static int panel = 0;
        public static BigInteger MyRand(BigInteger min, BigInteger max) 
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] _rand = new byte[max.ToByteArray().Length];
            BigInteger rand;
            do
            {
                rng.GetBytes(_rand);
                rand = new BigInteger(_rand);
            }
            while (rand<min|| rand>max);
            return rand;
        }
        public static void Information()                                                        //  
        {
            if (lastpanel != null) 
            {
                string info = "";
                string title = "";
                switch (panel)
                {
                    case 1:
                        info = "1";
                        title = "Возведение в степень по модулю";
                        break;
                    case 2:
                        info = "2";
                        title = "Вычисление НОД";
                        break;
                    case 3:
                        info = "3";
                        title = "Вычисление мультипликативной инверсии";
                        break;
                    case 4:
                        info = "4";
                        title = "Проверка простого числа";
                        break;
                    case 5:
                        info = "5";
                        title = "Генерация большого простого числа";
                        break;
                }
                MessageBox.Show(info, title);
            }
        }
        public static void MovePanel(Panel new_panel,int panel) 
        {
            if (BasicAlgorithms.lastpanel == null)
            {
                new_panel.Visible = true;
            }
            else
            {
                lastpanel.Visible = false;
                new_panel.Visible = true;
            }
            BasicAlgorithms.panel = panel;
            BasicAlgorithms.lastpanel = new_panel;
        }
        public static BigInteger Ex(BigInteger a, BigInteger n, BigInteger m) 
        {
            BigInteger c = 1;
            for (BigInteger i = 0; i < n; i++) 
            {
                c *= a;
            }
            return c % m;
        }
        public static BigInteger Cgcd(BigInteger a, BigInteger b) 
        { 
            while (a != b) 
            {
                if (a > b)
                {
                    a = a - b;
                }
                else 
                {
                    b = b - a;
                }
            }
            return a;
        }
        public static BigInteger Cmi(BigInteger a, BigInteger b, BigInteger n, ref int key) 
        {
            bool flag = true;
            if (key == -1)
            {
                key = (a * b) % n == 1 ? 4 : 5;
                return 0;
            }
            if (key == 1)
            {
                a = 1;
                while (flag)
                {
                    if ((a * b) % n == 1)
                    {
                        flag = false;
                        return a;
                    }
                    a++;
                }
            }
            if (key == 2)
            {
                b = 1;
                while (flag)
                {
                    if ((a * b) % n == 1)
                    {
                        flag = false;
                        return b;
                    }
                    b++;
                }
            }
            if (key == 3)
            {
                n = 1;
                while (flag)
                {
                    if ((a * b) % n == 1)
                    {
                        flag = false;
                        return n;
                    }
                    n++;
                }
            }
            return 0;

        }
        public static bool Cns_Ferma(BigInteger a) 
        {
            if (a == 2)
            {
                return true;
            }
            if (a % 2 == 0) 
            {
                return false;
            }
            BigInteger count = (int)Math.Log((double)a, 2);
            Random rnd = new Random();
            for (BigInteger i = 0; i < count; i++)
            {
                //BigInteger x = rnd.Next(1,(((int)a)-2));
                BigInteger x = MyRand(1, a - 2);
                if (Ex(x, a - 1, a) != 1) 
                {  
                    return false;
                }
            }
            return true;
        }
        public static bool Cns_Mr(BigInteger n)
        {
            if (n == 2 || n == 3) 
            {
                return true;
            }
            if (n < 2 || n % 2 == 0) 
            {
                return false;
            }
            int k = (int)Math.Log((double)n, 2);
            BigInteger d = n - 1;
            int s = 0;
            while (d % 2 == 0)
            {
                d /= 2;
                s++;
            }
            for (int i = 0; i < k; i++) 
            {
                RNGCryptoServiceProvider rNG = new RNGCryptoServiceProvider();
                byte[] _a = new byte[n.ToByteArray().LongLength];
                BigInteger a;
                do
                {
                    rNG.GetBytes(_a);
                    a = new BigInteger(_a);
                }
                while (a < 2 || a > n - 2);
                BigInteger x = Ex(a, d, n);
                if (x == 1 || x == n - 1) 
                {
                    continue;
                }
                for (int j = 1; j < s; j++)
                {
                    x = Ex(x, 2, n);
                    if (x == 1) 
                    {
                        return false;
                    }
                    if (x == n - 1) 
                    {
                        break;
                    }
                    return false;
                }
                if (x != n - 1)
                {
                    return false;
                }
            }
            return true;
        }
        public static BigInteger Glp(int size)
        {
            BigInteger output = 1;
            int iteration = 1;
            while (true) 
            {
                for (int i = 0; i < iteration; i++) 
                {
                    output *= 2;
                }
                output -= 1;
                string temp = output.ToString();
                if (temp.Length >= size) 
                {
                    return output;
                }
                iteration++;
            }
        }
    }
    partial class Form_main
    {
        private void ИнформацияToolStripMenuItem_algorithm_Click(object sender, EventArgs e)
        {
            BasicAlgorithms.Information();
        }

        private void ЗакрытьToolStripMenuItem_algorithm_Click(object sender, EventArgs e)
        {
            BasicAlgorithms.lastpanel.Visible = false;
            radioButton_ex.Checked = false;
            radioButton_cgcd.Checked = false;
            radioButton_cmi.Checked = false;
            radioButton_cns.Checked = false;
            radioButton_glp.Checked = false;
            textBox_a_ex.Text = "";
            textBox_n_ex.Text = "";
            textBox_m_ex.Text = "";
            textBox_c_ex.Text = "";
            textBox_a_cgcd.Text = "";
            textBox_b_cgcd.Text = "";
            textBox_c_cgcd.Text = "";
            radioButton_ferma_cns.Checked = true;
            radioButton_mr_cns.Checked = false;
            Change_Pannel(panel_algorithm, panel_menu);
        }

        private void radioButton_cgcd_MouseClick(object sender, MouseEventArgs e)
        {
            BasicAlgorithms.MovePanel(panel_cgcd,2);
        }

        private void radioButton_ex_MouseClick(object sender, MouseEventArgs e)
        {
            BasicAlgorithms.MovePanel(panel_ex,1);
        }

        private void radioButton_cmi_MouseClick(object sender, MouseEventArgs e)
        {
            BasicAlgorithms.MovePanel(panel_cmi,3);
        }

        private void radioButton_cns_MouseClick(object sender, MouseEventArgs e)
        {
            BasicAlgorithms.MovePanel(panel_cns,4);
        }

        private void radioButton_glp_MouseClick(object sender, MouseEventArgs e)
        {
            BasicAlgorithms.MovePanel(panel_glp,5);
        }
        
        private void button_ex_Click(object sender, EventArgs e)
        {
            ulong.TryParse(textBox_a_ex.Text, out ulong a);
            ulong.TryParse(textBox_n_ex.Text, out ulong n);
            ulong.TryParse(textBox_m_ex.Text, out ulong m);
            textBox_c_ex.Text = BasicAlgorithms.Ex(a,n,m).ToString();
        }
        private void button_cgcd_Click(object sender, EventArgs e)
        {
            ulong.TryParse(textBox_a_cgcd.Text, out ulong a);
            ulong.TryParse(textBox_b_cgcd.Text, out ulong b);
            textBox_c_cgcd.Text = BasicAlgorithms.Cgcd(a,b).ToString();
        }
        private void button_cmi_Click(object sender, EventArgs e)
        {
            BigInteger res =0;
            int key = -1;
            ulong.TryParse(textBox_a_cmi.Text, out ulong a);
            ulong.TryParse(textBox_b_cmi.Text, out ulong b);
            ulong.TryParse(textBox_n_cmi.Text, out ulong n);
            if (textBox_a_cmi.Text.Length == 0&& textBox_b_cmi.Text.Length != 0&& textBox_n_cmi.Text.Length != 0) 
            {
                key = 1;
            }
            if (textBox_a_cmi.Text.Length != 0 && textBox_b_cmi.Text.Length == 0 && textBox_n_cmi.Text.Length != 0)
            {
                key = 2;
            }
            if (textBox_a_cmi.Text.Length != 0 && textBox_b_cmi.Text.Length != 0 && textBox_n_cmi.Text.Length == 0)
            {
                key = 3;
            }
            res = BasicAlgorithms.Cmi(a,b,n,ref key);
            if (key == 4) 
            {
                textBox_r_cmi.Text = "Числа мультипликативно инверстные";
            }
            if (key == 5)
            {
                textBox_r_cmi.Text = "Числа мультипликативно неинверстные";
            }
            if (key == -1 || key == 1 || key == 2 || key == 3)
            {
                textBox_r_cmi.Text = res.ToString();
            }
        }
        private void button_cns_Click(object sender, EventArgs e)
        {
            ulong.TryParse(textBox_cns.Text,out ulong a);
            bool status = false;
            if (radioButton_ferma_cns.Checked) 
            {
                status = BasicAlgorithms.Cns_Ferma(a);
            }
            if (radioButton_mr_cns.Checked)
            {
                status = BasicAlgorithms.Cns_Mr(a);
            }
            if (status)
            {
                label_cns_status.Text = "Число вероятно простое";
            }
            else 
            {
                label_cns_status.Text = "Число не простое";
            }
        }
        private void button_glp_Click(object sender, EventArgs e)
        {
            int.TryParse(textBox_length_glp.Text,out int size);
            textBox_glp.Text = BasicAlgorithms.Glp(size).ToString();
        }
    }

}

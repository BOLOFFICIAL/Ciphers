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
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.CompilerServices;

namespace Ciphers
{
    public class ElGamal
    {
        private long p = 0;
        private long x = 0;
        private long y = 0;
        private long g = 0;
        private long[] a;
        private long[] b;
        private long[] long_text;
        private char[] char_text;
        public string Encryption(string text, long p, long x)
        {
            string output = "";
            this.p = p;
            this.x = x;
            long size = text.Length;
            a = new long[size];
            b = new long[size];
            Tolong(text);
            Primitive(p);
            y = MultiplicationModulo(g, x, p);
            for (long i = 0; i < size; i++)
            {
                a[i] = MultiplicationModulo(g, MakeRand(), p);
                b[i] = (long_text[i] * MultiplicationModulo(y, MakeRand(), p)) % p;
                output+=(a[i] + ";" + b[i] + ";");
            }
            return output;
        }
        private int MakeRand() 
        {
            Random random = new Random();
            return random.Next(250, (int)p - 2);
        }
        private long Multiplication(long a, long b, long n) 
        {
            long sum = 0;
            for (long i = 0; i < b; i++)
            {
                sum += a;
                if (sum >= n)
                {
                    sum -= n;
                }
            }
            return sum;
        }
        public string Decryption(string text, long p, long x)
        {
            string output = "";
            this.p = p;
            this.x = x;
            Primitive(p);
            y = MultiplicationModulo(g, x, p);
            string[] array_string = text.Split(';');
            int size = array_string.Length / 2;
            long[] array = new long[size * 2];
            for (int i = 0; i < size * 2; i++)
            {
                array[i] = long.Parse(array_string[i]);
            }
            a = new long[size];
            b = new long[size];
            char_text = new char[size];
            for (int i = 0, j = 1, k = 0; j < size * 2; i += 2, j += 2, k++)
            {
                a[k] = array[i];
                b[k] = array[j];
            }
            for (int i = 0; i < size; i++)
            {
                char_text[i] = (char)((b[i] * MultiplicationModulo(a[i], p - 1 - x, p)) % p);
                output+=(char_text[i]);
            }
            return output;
        }
        private void Tolong(string text)
        {
            long[] numbers = new long[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                numbers[i] = (long)text[i];
            }
            long_text = numbers;
        }
        public bool IsSimple(long p)
        {
            if (p == 1)
            {
                return false;
            }
            long i = 2;
            while (i * i <= p)
            {
                if (p % i == 0)
                {
                    return false;
                }
                i++;
            }
            return true;
        }
        private void Primitive(long p)
        {
            g = 2;
            bool flag = true;
            while (true)
            {
                if (MultiplicationModulo(g, p - 1, p) == 1)
                {
                    for (long i = 1; i < p - 1; i++)
                    {

                        if (MultiplicationModulo(g, i, p) == 1)
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        break;
                    }
                }
                flag = true;
                g++;
            }
        }
        private long MultiplicationModulo(long g, long f, long p)
        {
            long temp = g;
            long G = g;
            string fstring = Convert.ToString(f, 2);
            for (int i = 1; i < fstring.Length; i++)
            {
                if (fstring[i] == '1')
                {
                    temp = (temp * temp * G) % p;
                }
                else
                {
                    temp = (temp * temp) % p;
                }
            }
            return temp;
        }
    }
}

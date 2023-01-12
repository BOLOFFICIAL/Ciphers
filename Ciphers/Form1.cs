/*==================================================
╔═════╗ ╔═══╗ ╔═════╗ ╔═╗ ╔═╗ ╔════╗ ╔═════╗ ╔═════╗ 
║ ╔═╗ ║ ╚╣ ╠╝ ║ ╔═╗ ║ ║ ║ ║ ║ ║ ╔══╝ ║ ╔═╗ ║ ║ ╔═══╝ 
║ ║ ╚═╝  ║ ║  ║ ╚═╝ ║ ║ ╚═╝ ║ ║ ╚══╗ ║ ╚═╝ ║ ║ ╚═══╗ 
║ ║ ╔═╗  ║ ║  ║ ╔═══╝ ║ ╔═╗ ║ ║ ╔══╝ ║ ╔╗ ╔╝ ╚═══╗ ║ 
║ ╚═╝ ║ ╔╣ ╠╗ ║ ║     ║ ║ ║ ║ ║ ╚══╗ ║ ║║ ╚╗ ╔═══╝ ║ 
╚═════╝ ╚═══╝ ╚═╝     ╚═╝ ╚═╝ ╚════╝ ╚═╝╚══╝ ╚═════╝ 
================== Ciphers v1.0.0 ==================
              property of bolofficial 
                   on 27.09.2022 
==================================================*/


using Ciphers;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Сiphers
{
    public partial class Form_main : Form
    {
        public Form_main()
        {
            InitializeComponent();
            Fill();
        }
        private void Fill()                                                                                                                                     //  Перевод всех панелей в полноэкранный режим          (Y)  
        {
            panel_menu.Dock = DockStyle.Fill;
            panel_caesar_cipher.Dock = DockStyle.Fill;
        }
        public static void Change_Pannel(Panel old_panel,Panel new_panel)                                                                                            //  Смена панели                                        (Y)  
        {
            old_panel.Visible = false;
            new_panel.Visible = true;
        }
        public void Make_message(string text, string title = "", MessageBoxButtons button = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)    //  Создание сообщений                                  (Y)  
        {
            MessageBox.Show(text, title, button, icon);
        }
        private void button_menu_Click(object sender, EventArgs e)                                                                                              //  Кнопка открытия выбранного шифра                    (Y)  
        {
            if (comboBox_menu.SelectedItem == null) 
            {
                return;
            }
            switch (comboBox_menu.SelectedItem)
            {
                case "Шифр Цезаря":
                    Change_Pannel(panel_menu,panel_caesar_cipher);
                    break;
                case "Шифры Гаммирования":
                    Change_Pannel(panel_menu,panel_gamma);
                    break;
                case "Базовые Алгоритмы":
                    Change_Pannel(panel_menu,panel_algorithm);
                    break;
                case "Кузнечик":
                    Change_Pannel(panel_menu, panel_kuznechic);
                    break;
                case "Эль Гамаль":
                    Change_Pannel(panel_menu, panel_elgamal);
                    break;
                default:
                    Make_message("Данный шифр пока недоступен", "Выбор шифра", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
        private void закрытьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Change_Pannel(panel_elgamal, panel_menu);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button_elgamal.Text = "Зашифровать";
        }
        private bool CheckNumber(long n,string N) 
        {
            return n.ToString().Length == N.Length;
        }
        private void button_elgamal_Click(object sender, EventArgs e)
        {
            ElGamal elGamal = new ElGamal();
            long.TryParse(textBox_p_elgamal.Text, out long p);
            long.TryParse(textBox_x_elgamal.Text, out long x);
            if (CheckNumber(p, textBox_p_elgamal.Text))
            {
                if (elGamal.IsSimple(p) && p >= 1500)
                {
                    if (CheckNumber(x, textBox_x_elgamal.Text)) 
                    {
                        if (radioButton_en_elgamal.Checked)
                        {
                            textBox_elgamal.Text = elGamal.Encryption(textBox_elgamal.Text, p, x);
                        }
                        else
                        {
                            textBox_elgamal.Text = elGamal.Decryption(textBox_elgamal.Text, p, x);
                        }
                    }
                    else
                    {
                        Make_message("Параметр X не является числом", "Значение параметра X", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }    
                }
                else 
                {
                    Make_message("Параметр Р должно быть простым числом больше 1500", "Значение параметра Р", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else 
            {
                Make_message("Параметр Р не является числом", "Значение параметра Р", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void radioButton_de_elgamal_CheckedChanged(object sender, EventArgs e)
        {
            button_elgamal.Text = "Расшифровать";
        }

        private void информацияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Make_message("" +
                "Лаборторная работа 5 - Заключительная\n" +
                "По дисциплине Криптографические методы защиты инофрмации\n" +
                "Посвящена Схеме Эль Гамаля\n" +
                "Выполнена студентом ЯГТУ ЭИСБ-34\n" +
                "Болониным Михаилом" +
                "", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void подписиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}
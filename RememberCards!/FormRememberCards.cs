using RememberLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;


namespace RememberCards_
{
    public partial class Form1 : Form, IPlayable
    {
        //int count = 0;
        static Random rand = new Random();
        int var = rand.Next(1, 5);

        LogicRemember logic;
        public Form1(string data)
        {
            InitializeComponent();
            logic = new LogicRemember(this);
            label_count.Text = "0";//count.ToString();
            logic.CreateNewGame();
            label1.Text = data;
        }
        
        
        private void menu_game_exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menu_help_rules_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"Цель игры - открыть все карточки за минимальное количество ходов.
Перед вами 16 карточек - 8 пар разных картинок.
Необходимо найти парные карточки.

Ваша задача открывать по две карточки.
Если выбранная пара картинок совпадает, то карточки остаются раскрытыми.
Иначе - карточки снова закрываются.", "Правила", MessageBoxButtons.OK);
        }

        private void menu_help_about_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"Игра «Запомни карточки!» создана в рамках курсовой работы.


©ТУСУР, ФБ, Васильева Кюннэй Викторовна, группа 717-2", "О программе", MessageBoxButtons.OK);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int nr = int.Parse(((PictureBox)sender).Tag.ToString());//получение номера картинки
            logic.ClickPicture(nr);
            return;
            
        }
      
        
        private void LoadPicture(int picture, int image)
        {
            getPictureBox(picture).Image = getImage(image);
        }
         

        private PictureBox getPictureBox(int picture)
        {
            
            switch (picture)
            {
                case 0: return pictureBox0;
                case 1: return pictureBox1;
                case 2: return pictureBox2;
                case 3: return pictureBox3;
                case 4: return pictureBox4;
                case 5: return pictureBox5;
                case 6: return pictureBox6;
                case 7: return pictureBox7;
                case 8: return pictureBox8;
                case 9: return pictureBox9;
                case 10: return pictureBox10;
                case 11: return pictureBox11;
                case 12: return pictureBox12;
                case 13: return pictureBox13;
                case 14: return pictureBox14;
                case 15: return pictureBox15;
                default: return null;
            }
        
        }

        private Image getImage (int image)
        {
            if (var == 1)
            {
                switch (image)
                {
                    case 0: return Properties.Resources._0;
                    case 1: return Properties.Resources._1;
                    case 2: return Properties.Resources._2;
                    case 3: return Properties.Resources._3;
                    case 4: return Properties.Resources._4;
                    case 5: return Properties.Resources._5;
                    case 6: return Properties.Resources._6;
                    case 7: return Properties.Resources._7;
                    case 8: return Properties.Resources._8;
                    default: return null;
                }
            }
            else if (var == 2)
            {
                switch (image)
                {
                    case 0: return Properties.Resources._0;
                    case 1: return Properties.Resources.f1;
                    case 2: return Properties.Resources.f2;
                    case 3: return Properties.Resources.f3;
                    case 4: return Properties.Resources.f4;
                    case 5: return Properties.Resources.f5;
                    case 6: return Properties.Resources.f6;
                    case 7: return Properties.Resources.f7;
                    case 8: return Properties.Resources.f8;
                    default: return null;
                }
            }
            else if (var == 3)
            {
                switch (image)
                {
                    case 0: return Properties.Resources._0;
                    case 1: return Properties.Resources.b1;
                    case 2: return Properties.Resources.b2;
                    case 3: return Properties.Resources.b3;
                    case 4: return Properties.Resources.b4;
                    case 5: return Properties.Resources.b5;
                    case 6: return Properties.Resources.b6;
                    case 7: return Properties.Resources.b7;
                    case 8: return Properties.Resources.b8;
                    default: return null;
                }
            }
            else if (var == 4)
            {
                switch (image)
                {
                    case 0: return Properties.Resources._0;
                    case 1: return Properties.Resources.p1;
                    case 2: return Properties.Resources.p2;
                    case 3: return Properties.Resources.p3;
                    case 4: return Properties.Resources.p4;
                    case 5: return Properties.Resources.p5;
                    case 6: return Properties.Resources.p6;
                    case 7: return Properties.Resources.p7;
                    case 8: return Properties.Resources.p8;
                    default: return null;
                }
            }
            else return null;
        }

        private void menu_game_new_Click(object sender, EventArgs e)
        {
            var = rand.Next(1, 5);
            //string varr = Convert.ToString(var);
            //MessageBox.Show(varr);
            logic.CreateNewGame();
            //count = 0;
            label_count.Text = "0";//count.ToString();
        }

        public void ShowCard (int nr, int card, int count)
        {
            {
                LoadPicture(nr, card);
                getPictureBox(nr).Cursor = Cursors.No;
                //count++;
                label_count.Text = count.ToString();
            }
        }

        public void HideCard (int picture)
        {
            LoadPicture(picture, 0);
            getPictureBox(picture).Cursor = Cursors.Hand;
        }

        public void ShowWinner()
        {
            MessageBox.Show("Вы победили :)", "Поздравляем!");
            string score = label_count.Text;
            string login = label1.Text;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://h923416t.beget.tech/insert_score.php");
            request.UserAgent = "Mozilla/5.0";
            request.Method = "POST";

            string query = "login=" + login + "&score=" + score;
            byte[] byteMsg = Encoding.UTF8.GetBytes(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteMsg.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(byteMsg, 0, byteMsg.Length);
            }

            WebResponse response = request.GetResponse();
            string answer = null;

            using (Stream s = response.GetResponseStream())
            {
                using (StreamReader sR = new StreamReader(s))
                {
                    answer = sR.ReadToEnd();
                    //MessageBox.Show(answer);
                }
            }
        }

        public void ShowLoser()
        {
            MessageBox.Show("Вы проиграли :(", "Не огорчайтесь!");
        }
        
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void menu_game_new_usual_Click(object sender, EventArgs e)
        {
           
            
        }

        private void menu_game_new_hard_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
           

        }

        private void ToolStripMenuItem_Rayting_Click(object sender, EventArgs e)
        {
            
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://h923416t.beget.tech/rayting.php");
            request.UserAgent = "Mozilla/5.0";
            request.Method = "POST";
            string login = label1.Text;
            string query = "login=" + login;
            byte[] byteMsg = Encoding.UTF8.GetBytes(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteMsg.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(byteMsg, 0, byteMsg.Length);
            }

            WebResponse response = request.GetResponse();
            string answer = null;

            using (Stream s = response.GetResponseStream())
            {
                using (StreamReader sR = new StreamReader(s))
                {
                    answer = sR.ReadToEnd();
                    
                }
            }
            var ray = new FormRating(answer);
            ray.ShowDialog();
            
        }
    }
}

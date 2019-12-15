using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using xNet;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace RememberCards_
{
    public partial class FormAuth : Form
    {
        public FormAuth()
        {
            InitializeComponent();
            ToolTip l = new ToolTip();
            //ToolTip p = new ToolTip();
            l.SetToolTip(textBox_login, @"Поле должно обязательно включать в себя: 
- от 8 до 15 символов
- одну цифру
- одну строчную букву
- одну прописную букву");
            l.SetToolTip(textBox_password, @"Поле должно обязательно включать в себя: 
- от 8 до 15 символов
- одну цифру
- одну строчную букву
- одну прописную букву");

        }
        private string Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
        public static bool StringIsValid(string str)
        {
            return !Regex.IsMatch(str, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$");
        }
        private void button_reg_Click(object sender, EventArgs e)
        {
            if ((textBox_login.Text.Length==0)|| (textBox_password.Text.Length == 0))
            {
                MessageBox.Show("Заполните оба поля!");
                return;
            }
            if (StringIsValid(textBox_login.Text)||StringIsValid(textBox_password.Text))
            {
                MessageBox.Show("Недопустимое значение полей!");
                return;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://h923416t.beget.tech/logger.php");
            request.UserAgent = "Mozilla/5.0";//исключили ошибку
            request.Method = "POST";
            string login = textBox_login.Text;
            string password = Hash(textBox_password.Text);
            string query = "login=" + login + "&pass=" + password;
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
            response.Close();
            MessageBox.Show(answer);
            textBox_login.Clear();
            textBox_password.Clear();
        }

        private void button_enter_Click(object sender, EventArgs e)
        {
            if ((textBox_login.Text.Length == 0) || (textBox_password.Text.Length == 0))
            {
                MessageBox.Show("Заполните оба поля!");
                return;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://h923416t.beget.tech/auth.php");
            request.UserAgent = "Mozilla/5.0";
            request.Method = "POST";
            string login = textBox_login.Text;
            string password = Hash(textBox_password.Text);
            string query = "login=" + login + "&pass=" + password;
            byte[] byteMsg = Encoding.UTF8.GetBytes(query);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteMsg.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(byteMsg, 0, byteMsg.Length);
            }

            WebResponse response = request.GetResponse();
            string answer = null;
            bool test=false;
            using (Stream s = response.GetResponseStream())
            {
                using (StreamReader sR = new StreamReader(s))
                {
                    answer = sR.ReadToEnd();
                    if (answer == login)
                        test = true;
                }
            }
            response.Close();
            
            
            if (test)
            {
                MessageBox.Show("Приятной игры, "+ answer +" :)");
                this.Hide();
                var f1= new Form1(answer);
                f1.FormClosed+=(s, args)=>this.Close();
                f1.Show();
            }
            else
            {
                MessageBox.Show(answer);
                textBox_login.Clear();
                textBox_password.Clear();
            }
        }
    }
}

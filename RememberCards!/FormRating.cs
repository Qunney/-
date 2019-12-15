using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RememberCards_
{
    
    public partial class FormRating : Form
    {
        
        public FormRating(string rayting)
        {
            InitializeComponent();
            label1.Text = rayting;
        }

    }
    
}

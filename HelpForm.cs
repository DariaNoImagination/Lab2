using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laboratory1
{
    public partial class HelpForm : Form
    {
        public HelpForm(string form)
        {
            InitializeComponent();
            switch (form)
            {
               
                case "aboutProgramm":
                    this.Text = "О программе";
                    textBox1.Text = "Программа выполнена студенткой 3-его курса АВТФ НГТУ группы АП-327 Осинцевой Дарьей Александровной " +
                        "в рамках программы дисциплины 'Теория формальных языков и компиляторов'";
                    break;
            }

        }
    }
}

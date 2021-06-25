using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskKiller
{
    public partial class addForm : Form
    {
        public addForm()
        {
            InitializeComponent();
        }

        private void addForm_Load(object sender, EventArgs e)
        {
            foreach(string s in TaskManager.GetTasks())
            {
                curtaskList.Items.Add(s);
            }
        }

        private void addConfirmButton_Click(object sender, EventArgs e)
        {
            string selected = curtaskList.SelectedItem.ToString();
            TaskManager.current.AddTask(selected);
            this.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TaskKiller
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            TaskManager.current = this;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            addForm addForm = new addForm();
            addForm.ShowDialog();
        }

        public void AddTask(string taskName)
        {
            taskList.Items.Add(taskName);
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(taskList.SelectedItem.ToString())){
                taskList.Items.RemoveAt(taskList.SelectedIndex);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                using(StreamWriter sw = new StreamWriter("KillList.tk"))
                {
                    foreach(string s in taskList.Items)
                    {
                        sw.WriteLine(s);
                    }
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void openButton_Click(object sender, EventArgs e)
        {

            try
            {
                using(StreamReader sr = new StreamReader("KillList.tk"))
                {
                    string line;
                    while(!string.IsNullOrEmpty(line = sr.ReadLine()))
                    {
                        taskList.Items.Add(line);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
    }
}

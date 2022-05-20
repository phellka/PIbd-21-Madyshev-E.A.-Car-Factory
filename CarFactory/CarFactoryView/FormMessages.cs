using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using CarFactoryContracts.BusinessLogicsContracts;
using CarFactoryContracts.BindingModels;

namespace CarFactoryView
{
    public partial class FormMessages : Form
    {
        private readonly IMessageInfoLogic logic;
        private bool hasNext = false;
        private readonly int mailsOnPage = 4;
        private int currentPage = 0;
        public FormMessages(IMessageInfoLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }
        private void LoadData()
        {
            var list = logic.Read(new MessageInfoBindingModel
            {
                ToSkip = currentPage * mailsOnPage,
                ToTake = mailsOnPage + 1
            });
            hasNext = !(list.Count() <= mailsOnPage);
            if (hasNext)
            {
                buttonNext.Enabled = true;
            }
            else
            {
                buttonNext.Enabled = false;
            }
            if (list != null)
            {
                dataGridViewMessages.DataSource = list.Take(mailsOnPage).ToList();
            }
        }
        private void FormMessages_Load(object sender, EventArgs e)
        {
            LoadData();
            labelPage.Text = "1";
            dataGridViewMessages.Columns[0].Visible = false;
            dataGridViewMessages.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if ((currentPage - 1) >= 0)
            {
                currentPage--;
                labelPage.Text = (currentPage + 1).ToString();
                buttonNext.Enabled = true;
                if (currentPage == 0)
                {
                    buttonPrev.Enabled = false;
                }
                LoadData();
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (hasNext)
            {
                currentPage++;
                labelPage.Text = (currentPage + 1).ToString();
                buttonPrev.Enabled = true;
                LoadData();
            }
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (dataGridViewMessages.SelectedRows.Count == 1)
            {
                var form = Program.Container.Resolve<FormMessage>();
                form.MessageId = dataGridViewMessages.SelectedRows[0].Cells[0].Value.ToString();
                form.ShowDialog();
                LoadData();
            }
        }
    }
}

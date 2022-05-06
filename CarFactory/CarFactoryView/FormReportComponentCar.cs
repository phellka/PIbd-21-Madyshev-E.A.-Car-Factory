using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CarFactoryContracts.BindingModels;
using CarFactoryContracts.BusinessLogicsContracts;

namespace CarFactoryView
{
    public partial class FormReportComponentCar : Form
    {
        private readonly IReportLogic logic;

        public FormReportComponentCar(IReportLogic logic)
        {
            InitializeComponent();
            this.logic = logic;
        }

        private void FormComponentCar_Load(object sender, EventArgs e)
        {
            try
            {
                var dict = logic.GetCarComponent();
                if (dict != null)
                {
                    dataGridViewComponents.Rows.Clear();
                    foreach (var elem in dict)
                    {
                        dataGridViewComponents.Rows.Add(new object[] { elem.CarName, "", ""});
                        foreach (var listElem in elem.Components)
                        {
                            dataGridViewComponents.Rows.Add(new object[] { "", listElem.Item1, listElem.Item2 });
                        }
                        dataGridViewComponents.Rows.Add(new object[] { "Итого", "", elem.TotalCount });
                        dataGridViewComponents.Rows.Add(Array.Empty<object>());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }

        }

        private void buttonSaveExcel_Click(object sender, EventArgs e)
        {
            using var dialog = new SaveFileDialog { Filter = "xlsx|*.xlsx" };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    logic.SaveCarComponentToExcelFile(new ReportBindingModel
                    {
                        FileName = dialog.FileName
                    });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    }
}

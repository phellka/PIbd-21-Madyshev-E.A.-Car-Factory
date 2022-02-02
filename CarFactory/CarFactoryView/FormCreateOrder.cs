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
using CarFactoryContracts.ViewModels;

namespace CarFactoryView
{
    public partial class FormCreateOrder : Form
    {
        private readonly ICarLogic logicCar;
        private readonly IOrderLogic logicOrder;
        public FormCreateOrder(ICarLogic logicP, IOrderLogic logicO)
        {
            InitializeComponent();
            logicCar = logicP;
            logicOrder = logicO;
        }
        private void CalcSum()
        {
            if (comboBoxCars.SelectedValue != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = (comboBoxCars.SelectedValue as CarViewModel).Id;
                    CarViewModel car = logicCar.Read(new CarBindingModel { Id = id })?[0];
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = (count * car?.Price ?? 0).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxCars.SelectedValue == null)
            {
                MessageBox.Show("Выберите изделие", "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            try
            {
                logicOrder.CreateOrder(new CreateOrderBindingModel
                {
                    CarId = (comboBoxCars.SelectedValue as CarViewModel).Id,
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToDecimal(textBoxSum.Text)
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FormCreateOrder_Load(object sender, EventArgs e)
        {
            try
            {
                List<CarViewModel> list = logicCar.Read(null);
                if (list != null)
                {
                    comboBoxCars.DisplayMember = "CarName";
                    comboBoxCars.DisplayMember = "Id";
                    comboBoxCars.DataSource = list;
                    comboBoxCars.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void textBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void comboBoxCars_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }
    }
}

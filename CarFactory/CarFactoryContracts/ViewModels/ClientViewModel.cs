using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using CarFactoryContracts.Attributes;

namespace CarFactoryContracts.ViewModels
{
    public class ClientViewModel
    {
        public int Id { get; set; }
        [Column(title: "ФИО клиента", gridViewAutoSize: GridViewAutoSize.Fill)]
        public string FCs { get; set; }
        [Column(title: "Логин клиента", width: 100)]
        public string Login { get; set; }
        [Column(title: "Пароль клиента", width: 100)]
        public string Password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using CarFactoryContracts.Enums;
using CarFactoryContracts.Attributes;

namespace CarFactoryContracts.ViewModels
{
    public class OrderViewModel
    {
        [Column(title:"Номер", width:50)]
        public int Id { get; set; }
        public int CarId { get; set; }
        public int ClientId { get; set; }
        public int? ImplementerId { get; set; }
        [Column(title: "ФИО исполнителя", width: 150)]
        public string ImplementerFCs { get; set; }
        [Column(title: "ФИО клиента", width: 150)]
        public string ClientFCs { get; set; }
        [Column(title: "Машина", gridViewAutoSize: GridViewAutoSize.Fill)]
        public string CarName { get; set; }
        [Column(title: "Количество", width: 80)]
        public int Count { get; set; }
        [Column(title: "Сумма", width: 50)]
        public decimal Sum { get; set; }
        [Column(title: "Статус", width: 100)]
        public OrderStatus Status { get; set; }
        [Column(title: "Дата создания", width: 150)]
        public DateTime DateCreate { get; set; }
        [Column(title: "Дата выполнения", width: 150)]
        public DateTime? DateImplement { get; set; }
    }
}

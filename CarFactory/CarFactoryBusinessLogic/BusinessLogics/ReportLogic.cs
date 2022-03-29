using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarFactoryBusinessLogic.OfficePackage;
using CarFactoryBusinessLogic.OfficePackage.HelperModels;
using CarFactoryContracts.BindingModels;
using CarFactoryContracts.BusinessLogicsContracts;
using CarFactoryContracts.StoragesContracts;
using CarFactoryContracts.ViewModels;
using CarFactoryContracts.Enums;

namespace CarFactoryBusinessLogic.BusinessLogics
{
    public class ReportLogic : IReportLogic
    {
        private readonly IComponentStorage componentStorage;
        private readonly ICarStorage carStorage;
        private readonly IOrderStorage orderStorage;
        private readonly AbstractSaveToExcel saveToExcel;
        private readonly AbstractSaveToWord saveToWord;
        private readonly AbstractSaveToPdf saveToPdf;
        public ReportLogic(ICarStorage carStorage, IComponentStorage
            componentStorage, IOrderStorage orderStorage,
            AbstractSaveToExcel saveToExcel, AbstractSaveToWord saveToWord,
            AbstractSaveToPdf saveToPdf)
        {
            this.carStorage = carStorage;
            this.componentStorage = componentStorage;
            this.orderStorage = orderStorage;
            this.saveToExcel = saveToExcel;
            this.saveToWord = saveToWord;
            this.saveToPdf = saveToPdf;
        }
        public List<ReportCarComponentViewModel> GetCarComponent()
        {
            var cars = carStorage.GetFullList();
            var list = new List<ReportCarComponentViewModel>();
            foreach (var car in cars)
            {
                var record = new ReportCarComponentViewModel
                {
                    CarName = car.CarName,
                    Components = new List<Tuple<string, int>>(),
                    TotalCount = 0
                };
                foreach (var component in car.CarComponents)
                {
                    record.Components.Add(new Tuple<string, int>(component.Value.Item1, component.Value.Item2));
                    record.TotalCount += component.Value.Item2;
                }
                list.Add(record);
            }
            return list;
        }
        public List<ReportOrdersViewModel> GetOrders(ReportBindingModel model)
        {
            return orderStorage.GetFilteredList(new OrderBindingModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            })
            .Select(x => new ReportOrdersViewModel
            {
                DateCreate = x.DateCreate,
                CarName = x.CarName,
                Count = x.Count,
                Sum = x.Sum,
                Status = x.Status.ToString(),
                ClientFCs = x.ClientFCs
            })
           .ToList();
        }
        public void SaveCarsToWordFile(ReportBindingModel model)
        {
            saveToWord.CreateDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список машин",
                Cars = carStorage.GetFullList()
            });
        }
        public void SaveCarComponentToExcelFile(ReportBindingModel model)
        {
            saveToExcel.CreateReport(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список компонент",
                CarComponents = GetCarComponent()
            });
        }
        public void SaveOrdersToPdfFile(ReportBindingModel model)
        {
            saveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Список заказов",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                Orders = GetOrders(model)
            });
        }

    }
}

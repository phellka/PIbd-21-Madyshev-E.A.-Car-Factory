using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarFactoryContracts.BindingModels;
using CarFactoryContracts.ViewModels;

namespace CarFactoryContracts.BusinessLogicsContracts
{
    public interface IReportLogic
    {
        List<ReportCarComponentViewModel> GetCarComponent();
        List<ReportWarehouseComponentViewModel> GetWarehouseComponent();
        List<ReportOrdersViewModel> GetOrders(ReportBindingModel model);
        List<ReportOrdersByDateViewModel> GetOrdersByDate();
        void SaveCarsToWordFile(ReportBindingModel model);
        void SaveWarehousesToWordFile(ReportBindingModel model);
        void SaveCarComponentToExcelFile(ReportBindingModel model);
        void SaveWarehouseComponentToExcelFile(ReportBindingModel model);
        void SaveOrdersToPdfFile(ReportBindingModel model);
        void SaveOrdersByDateToPdfFile(ReportBindingModel model);
    }
}

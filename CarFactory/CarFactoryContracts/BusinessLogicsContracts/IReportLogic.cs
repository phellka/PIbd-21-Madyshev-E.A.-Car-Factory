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
        List<ReportOrdersViewModel> GetOrders(ReportBindingModel model);
        void SaveComponentsToWordFile(ReportBindingModel model);
        void SaveCarComponentToExcelFile(ReportBindingModel model);
        void SaveOrdersToPdfFile(ReportBindingModel model);
    }
}

using CarFactoryContracts.BindingModels;
using CarFactoryContracts.BusinessLogicsContracts;
using CarFactoryContracts.Enums;
using CarFactoryContracts.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;


namespace CarFactoryBusinessLogic.BusinessLogics
{
    public class WorkModeling : IWorkProcess
    {
        private IOrderLogic orderLogic;
        private readonly Random rnd;
        public WorkModeling()
        {
            rnd = new Random(1000);
        }
        public void DoWork(IImplementerLogic implementerLogic, IOrderLogic orderLogic)
        {
            this.orderLogic = orderLogic;
            var implementers = implementerLogic.Read(null);
            ConcurrentBag<OrderViewModel> orders = new(this.orderLogic.Read(new OrderBindingModel
                { 
                    SearchStatus = OrderStatus.Принят 
                }));
            foreach (var implementer in implementers)
            {
                Task.Run(async () => await WorkerWorkAsync(implementer, orders));
            }
        }
        private async Task WorkerWorkAsync(ImplementerViewModel implementer, ConcurrentBag<OrderViewModel> orders)
        {
            var runOrders = await Task.Run(() => orderLogic.Read(new OrderBindingModel
                {
                    ImplementerId = implementer.Id,
                    Status = OrderStatus.Выполняется
                }));
            foreach (var order in runOrders)
            {
                Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                orderLogic.FinishOrder(new ChangeStatusBindingModel
                    {
                        OrderId = order.Id
                    });
                Thread.Sleep(implementer.PauseTime);
            }
            var requiredOrders = await Task.Run(() => orderLogic.Read(new OrderBindingModel
            {
                ImplementerId = implementer.Id,
                Status = OrderStatus.ТребуютсяМатериалы
            }));
            foreach (var order in requiredOrders)
            {
                orderLogic.TakeOrder(new ChangeStatusBindingModel
                {
                    OrderId = order.Id,
                    ImplementerId = implementer.Id
                });
                OrderViewModel tempOrder = orderLogic.Read(new OrderBindingModel { Id = order.Id })?[0];
                if (tempOrder.Status == OrderStatus.ТребуютсяМатериалы)
                {
                    continue;
                }
                Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                orderLogic.FinishOrder(new ChangeStatusBindingModel
                {
                    OrderId = order.Id,
                    ImplementerId = implementer.Id
                });
                Thread.Sleep(implementer.PauseTime);
            }
            await Task.Run(() =>
            {
                while (!orders.IsEmpty)
                {
                    if (orders.TryTake(out OrderViewModel order))
                    {
                        orderLogic.TakeOrder(new ChangeStatusBindingModel
                            { 
                                OrderId = order.Id, 
                                ImplementerId = implementer.Id 
                            });
                        OrderViewModel tempOrder = orderLogic.Read(new OrderBindingModel { Id = order.Id })?[0];
                        if (tempOrder.Status == OrderStatus.ТребуютсяМатериалы)
                        {
                            continue;
                        }
                        Thread.Sleep(implementer.WorkingTime * rnd.Next(1, 5) * order.Count);
                        orderLogic.FinishOrder(new ChangeStatusBindingModel
                            { 
                                OrderId = order.Id,
                                ImplementerId = implementer.Id
                        });
                        Thread.Sleep(implementer.PauseTime);
                    }
                }
            });
        }
    }
}

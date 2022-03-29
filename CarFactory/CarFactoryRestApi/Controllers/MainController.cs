using CarFactoryContracts.BindingModels;
using CarFactoryContracts.BusinessLogicsContracts;
using CarFactoryContracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CarFactoryRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IOrderLogic order;
        private readonly ICarLogic car;
        public MainController(IOrderLogic order, ICarLogic car)
        {
            this.order = order;
            this.car = car;
        }
        [HttpGet]
        public List<CarViewModel> GetCarList() => car.Read(null)?.ToList();
        [HttpGet]
        public CarViewModel GetCar(int carId) => car.Read(new CarBindingModel { Id = carId })?[0];
        [HttpGet]
        public List<OrderViewModel> GetOrders(int clientId) => order.Read(new OrderBindingModel { ClientId = clientId });
        [HttpPost]
        public void CreateOrder(CreateOrderBindingModel model) =>
        order.CreateOrder(model);
    }

}

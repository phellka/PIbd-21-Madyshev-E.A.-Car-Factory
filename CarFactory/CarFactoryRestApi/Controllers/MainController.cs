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
        private readonly IMessageInfoLogic message;
        public MainController(IOrderLogic order, ICarLogic car, IMessageInfoLogic message)
        {
            this.order = order;
            this.car = car;
            this.message = message;
        }
        [HttpGet]
        public List<CarViewModel> GetCarList() => car.Read(null)?.ToList();
        [HttpGet]
        public CarViewModel GetCar(int carId) => car.Read(new CarBindingModel { Id = carId })?[0];
        [HttpGet]
        public List<OrderViewModel> GetOrders(int clientId) => order.Read(new OrderBindingModel { ClientId = clientId });
        [HttpGet]
        public List<MessageInfoViewModel> GetMessages(int clientId) => message.Read(new MessageInfoBindingModel { ClientId = clientId });
        [HttpPost]
        public void CreateOrder(CreateOrderBindingModel model) =>
        order.CreateOrder(model);
    }

}

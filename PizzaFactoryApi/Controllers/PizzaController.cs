using Microsoft.AspNetCore.Mvc;
using PizzaFactoryApi.Factories;

namespace PizzaFactoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzaController : ControllerBase
    {
        private readonly PizzaFactory _factory;

        public PizzaController(PizzaFactory factory)
        {
            _factory = factory;
        }

        [HttpGet("{pizzaType}")]
        public IActionResult GetPizza(string pizzaType)
        {
            var pizza = _factory.Create(pizzaType);

            var result = pizza.Prepare();

            return Ok(result);
        }
    }
}

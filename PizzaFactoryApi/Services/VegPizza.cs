using PizzaFactoryApi.Interfaces;

namespace PizzaFactoryApi.Services
{
    public class VegPizza : IPizza
    {
        public string Type => "veg";

        public string Prepare()
        {
            return "Veg Pizza Prepared with Onion, Capsicum and Corn";
        }
    }
}

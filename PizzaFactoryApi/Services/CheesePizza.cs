using PizzaFactoryApi.Interfaces;

namespace PizzaFactoryApi.Services
{
    public class CheesePizza : IPizza
    {
        public string Type => "cheese";

        public string Prepare()
        {
            return "Extra Cheese Pizza Prepared";
        }
    }
}

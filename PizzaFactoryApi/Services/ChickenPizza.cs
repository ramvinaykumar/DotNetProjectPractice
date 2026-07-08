using PizzaFactoryApi.Interfaces;

namespace PizzaFactoryApi.Services
{
    public class ChickenPizza : IPizza
    {
        public string Type => "chicken";

        public string Prepare()
        {
            return "Chicken Pizza Prepared with Chicken and Cheese";
        }
    }
}

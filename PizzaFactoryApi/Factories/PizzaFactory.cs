using PizzaFactoryApi.Interfaces;

namespace PizzaFactoryApi.Factories
{
    public class PizzaFactory
    {
        private readonly Dictionary<string, IPizza> _pizzaDictionary;

        public PizzaFactory(IEnumerable<IPizza> pizzas)
        {
            _pizzaDictionary = pizzas.ToDictionary(x => x.Type.Trim().ToLowerInvariant());

            Console.WriteLine("Available Pizzas:");

            foreach (var key in _pizzaDictionary.Keys)
            {
                Console.WriteLine(key);
            }
        }

        public IPizza Create(string pizzaType)
        {
            if (string.IsNullOrWhiteSpace(pizzaType))
                throw new ArgumentException("Pizza type is required.");

            var key = pizzaType.Trim().ToLowerInvariant();

            if (_pizzaDictionary.TryGetValue(key, out var pizza))
                return pizza;

            throw new Exception($"Pizza '{pizzaType}' not found. Available pizzas: {string.Join(", ", _pizzaDictionary.Keys)}");
        }
    }
}

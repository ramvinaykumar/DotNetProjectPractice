namespace PizzaFactoryApi.Interfaces
{
    public interface IPizza
    {
        string Type { get; }

        string Prepare();
    }
}

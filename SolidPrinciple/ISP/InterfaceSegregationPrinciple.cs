// Interface Segregation Principle (ISP) in C#. The ISP states that clients should not be forced to depend on interfaces they do not use.
// In this example, we have a multi-function device interface that includes methods for printing, scanning, and faxing.
// A basic printer that only needs to print would be forced to implement methods it doesn't need, which violates the ISP.
// The correct design is to create smaller, more specific interfaces for each functionality, allowing clients to implement only what they need.

namespace SolidPrinciple.ISP
{
    public class InterfaceSegregationPrinciple
    {
        public void ISPExample()
        {
            Console.WriteLine("Interface Segregation Principle (ISP) Example:");
            BasicPrinter basicPrinter = new BasicPrinter();
            basicPrinter.Print();
            Console.WriteLine("---");
            Console.WriteLine("Multi-function printer:");
            MultiFunctionPrinter multiFunctionPrinter = new MultiFunctionPrinter();
            multiFunctionPrinter.Print();
            multiFunctionPrinter.Scan();
            multiFunctionPrinter.Fax();
            Console.WriteLine("---");
            Console.WriteLine("Super office machine:");
            SuperOfficeMachine superOfficeMachine = new SuperOfficeMachine();
            superOfficeMachine.Print();
            superOfficeMachine.Scan();
        }
    }

    // The Issue with the Current Design which violates ISP:
    public interface IMultiFunctionDevice
    {
        void Print();
        void Scan();
        void Fax();
    }

    public class BasicPrinter_Voilating : IMultiFunctionDevice
    {
        public void Print() => Console.WriteLine("Printing document...");

        // Violation! Basic printers can't scan or fax.
        public void Scan() => throw new NotImplementedException();
        public void Fax() => throw new NotImplementedException();
    }

    // The Correct Design adhering to ISP:
    public interface IPrinter
    {
        void Print();
    }

    public interface IScanner
    {
        void Scan();
    }

    public interface IFax
    {
        void Fax();
    }

    public class MultiFunctionPrinter : IPrinter, IScanner, IFax
    {
        public void Print() => Console.WriteLine("Printing document...");
        public void Scan() => Console.WriteLine("Scanning document...");
        public void Fax() => Console.WriteLine("Faxing document...");
    }

    // The basic printer only cares about printing
    public class BasicPrinter : IPrinter
    {
        public void Print() => Console.WriteLine("Printing...");
    }

    // An advanced office machine can implement multiple, small interfaces
    public class SuperOfficeMachine : IPrinter, IScanner
    {
        public void Print() => Console.WriteLine("Printing...");
        public void Scan() => Console.WriteLine("Scanning...");
    }

}

using SolidPrinciple.DIP;
using SolidPrinciple.ISP;
using SolidPrinciple.LSP;
using SolidPrinciple.OCP;
using SolidPrinciple.SRP;

namespace SolidPrinciple
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of the SingleResponsibilityPrinciple class and call the example method
            Console.WriteLine("Start Of Single Responsibility Principle Example:");
            SingleResponsibility srpExample = new SingleResponsibility();
            srpExample.SRPExample();
            Console.WriteLine("End Of Single Responsibility Principle Example.\n");

            // Create an instance of the OpenClosedPrinciple class and call the example method
            Console.WriteLine("Start Of Open Closed Principle Example:");
            OpenClosedPrinciple ocpExample = new OpenClosedPrinciple();
            ocpExample.OCPExample();
            Console.WriteLine("End Of Open Closed Principle Example.\n");

            // Create an instance of the LiskovSubstitution class and call the example method
            Console.WriteLine("Start Of Liskov Substitution Principle Example:");
            LiskovSubstitution lspExample = new LiskovSubstitution();
            lspExample.LSPExample();
            Console.WriteLine("End Of Liskov Substitution Principle Example.\n");                     

            // Create an instance of the InterfaceSegregationPrinciple class and call the example method
            Console.WriteLine("Start Of Interface Segregation Principle Example:");
            InterfaceSegregationPrinciple ispExample = new InterfaceSegregationPrinciple();
            ispExample.ISPExample();
            Console.WriteLine("End Of Interface Segregation Principle Example.\n");

            // Create an instance of the DependencyInversionPrinciple class and call the example method
            Console.WriteLine("Start Of Dependency Inversion Principle Example:");
            DependencyInversion dipExample = new DependencyInversion();
            dipExample.DIPExample();
            Console.WriteLine("End Of Dependency Inversion Principle Example.\n");
        }
    }
}

// Liskov Substitution Principle (LSP) states that objects of a superclass should be replaceable with objects of a subclass
// without affecting the correctness of the program. In other words, if class B is a subclass of class A, then we should
// be able to replace A with B without breaking the functionality of the program.

namespace SolidPrinciple.LSP
{
    public class LiskovSubstitution
    {
        public void LSPExample()
        {
            Console.WriteLine("Liskov Substitution Principle (LSP) Example:");
            IColor colorProvider = new Red();            
            Console.WriteLine(colorProvider.GetColor()); // Output: Red
            Console.WriteLine("Now substituting with Blue:");
            colorProvider = new Blue();
            Console.WriteLine(colorProvider.GetColor()); // Output: Blue
            Console.WriteLine("This demonstrates that both Red and Blue can be used interchangeably without affecting the correctness of the program, adhering to LSP.");
        }
    }

    // The Correct Design adhering to LSP:
    public class Red : IColor
    {
        public string GetColor()
        {
            return "Red";
        }
    }

    public class Blue : IColor
    {
        public string GetColor()
        {
            return "Blue";
        }
    }

    public interface IColor
    {
        string GetColor();
    }

    // The Issue with the Current Design which violates LSP:

    // Implicitly violating LSP through poor conceptual inheritance
    //public class Red
    //{
    //    public virtual string GetColor() => "Red";
    //}

    //public class Blue : Red
    //{
    //    public override string GetColor() => "Blue";
    //}
}

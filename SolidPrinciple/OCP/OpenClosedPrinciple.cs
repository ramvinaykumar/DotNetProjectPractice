// The Open-Closed Principle (OCP) is one of the SOLID principles of object-oriented design. It states that software entities (classes, modules, functions, etc.)
// should be open for extension but closed for modification. This means that you should be able to add new functionality to a system without changing existing code,
// which helps to prevent bugs and maintain the integrity of the existing codebase.

namespace SolidPrinciple.OCP
{
    public class OpenClosedPrinciple
    {
        InvoiceCalculator _calculator = new InvoiceCalculator();
        public void OCPExample()
        {
            Console.WriteLine("Open-Closed Principle (OCP) Example:");
            decimal subtotal = 100m;
            // Using Regular Discount
            decimal totalRegular = _calculator.CalculateTotal(subtotal, new RegularDiscount());
            Console.WriteLine($"Total with Regular Discount: {totalRegular}");
            // Using VIP Discount
            decimal totalVip = _calculator.CalculateTotal(subtotal, new VipDiscount());
            Console.WriteLine($"Total with VIP Discount: {totalVip}");
            // Using Student Discount
            decimal totalStudent = _calculator.CalculateTotal(subtotal, new StudentDiscount());
            Console.WriteLine($"Total with Student Discount: {totalStudent}");
        }
    }

    // The Correct Design adhering to OCP:
    public interface IDiscountStrategy
    {
        decimal ApplyDiscount(decimal subtotal);
    }

    public class RegularDiscount : IDiscountStrategy
    {
        public decimal ApplyDiscount(decimal subtotal) => subtotal * 0.9m;
    }

    public class VipDiscount : IDiscountStrategy
    {
        public decimal ApplyDiscount(decimal subtotal) => subtotal * 0.8m;
    }

    public class StudentDiscount : IDiscountStrategy
    {
        public decimal ApplyDiscount(decimal subtotal) => subtotal * 0.7m;
    }

    // This class is now CLOSED for modification. 
    // It can support 100 new discount types without ever changing this code.
    public class InvoiceCalculator
    {
        public decimal CalculateTotal(decimal subtotal, IDiscountStrategy discountStrategy)
        {
            return discountStrategy.ApplyDiscount(subtotal);
        }
    }

    // The Issue with the Current Design which violates OCP:
    public class InvoiceCalculator_VoilatingOCP
    {
        public decimal CalculateTotal(decimal subtotal, string customerType)
        {
            if (customerType == "Regular") return subtotal * 0.9m;
            else if (customerType == "VIP") return subtotal * 0.8m;
            // Every new customer type forces us to modify this existing method!
            return subtotal;
        }
    }
}
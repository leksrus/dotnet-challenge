using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;

public class Program
{
    public static void Main()
    {
        using (var ctx = new DatabaseContext())
        {
            InitializeDB(ctx);

            var purchases = GetPurchases(ctx);

            var info = CreateConsoleString(purchases);

            Console.WriteLine(info);


            //Ejemplo de impresion
            // foreach (var purchase in ctx.Purchases)
            // {
            //     Console.WriteLine(purchase.PurchaseId.ToString());
            // }
        }
    }

    public static ICollection<Purchase> GetPurchases(DatabaseContext ctx)
    {
        return ctx.Purchases.ToList()
            .Where(p => p.Customer.DateOfBirth < new DateTime(2000, 01, 01))
            .OrderBy(p => p.CustomerId).ThenByDescending(p => p.PurchaseDateUTC.Date)
            .ThenByDescending(p => p.Total)
            .GroupBy(c => c.Customer.CustomerId).SelectMany(p => p).ToList();
    }


    public static string CreateConsoleString(ICollection<Purchase> purchases)
    {
        var sb = new StringBuilder();
        var customersDictionary = new Dictionary<int, string>();
        var partialTotal = 0.00m;

        foreach (var purchase in purchases)
        {
            if (!customersDictionary.TryGetValue(purchase.CustomerId, out _))
            {
                if (partialTotal > 0)
                {
                    sb.AppendFormat("====================================={0}", Environment.NewLine);
                    sb.AppendFormat("TOTAL: $ {0} {1}", partialTotal, Environment.NewLine);
                    partialTotal = 0.00m;
                }
                var age = CalculateClientAge(purchase.Customer);
                customersDictionary.Add(purchase.CustomerId, $"{purchase.Customer.FullName} (Edad: {age} años)");
                
                sb.AppendFormat("{0}", Environment.NewLine);
                sb.AppendFormat($"{purchase.Customer.FullName, -21} (Edad: {age} años) {Environment.NewLine}");
                sb.AppendFormat("====================================={0}", Environment.NewLine);
            }

            sb.AppendFormat("{0:dd/MM/yyyy} -------- $      {1, 10}{2}", purchase.PurchaseDateUTC.AddHours(-3), purchase.Total, Environment.NewLine);
            partialTotal += purchase.Total;
        }
        
        sb.AppendFormat("====================================={0}", Environment.NewLine);
        sb.AppendFormat("TOTAL: $ {0} {1}", partialTotal, Environment.NewLine);


        return sb.ToString();
    }

    public static int CalculateClientAge(Customer customer)
    {
        var today = DateTime.Today;
        var age = today.Year - customer.DateOfBirth.Year;

        if (customer.DateOfBirth.Date > today.AddYears(-age)) age--;

        return age;
    }

    public static void InitializeDB(DatabaseContext ctx)
    {
        if (ctx.Customers.Count() == 0)
        {
            ctx.Customers.Add(new Customer()
                { CustomerId = 1, FullName = "Sanchez Mario", DateOfBirth = new DateTime(1985, 10, 18) });
            ctx.Customers.Add(new Customer()
                { CustomerId = 2, FullName = "Gimenez Pedro", DateOfBirth = new DateTime(2010, 01, 10) });
            ctx.Customers.Add(new Customer()
                { CustomerId = 3, FullName = "Gomez Ricardo", DateOfBirth = new DateTime(1993, 11, 25) });
            ctx.Customers.Add(new Customer()
                { CustomerId = 4, FullName = "Araujo María", DateOfBirth = new DateTime(2009, 12, 2) });

            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1001, PurchaseDateUTC = new DateTime(2021, 2, 2, 15, 22, 35), Total = 255m, CustomerId = 1
            });
            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1002, PurchaseDateUTC = new DateTime(2021, 2, 7, 12, 07, 45), Total = 888m, CustomerId = 3
            });
            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1003, PurchaseDateUTC = new DateTime(2021, 2, 9, 9, 00, 10), Total = 672m, CustomerId = 1
            });
            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1004, PurchaseDateUTC = new DateTime(2021, 1, 2, 10, 12, 32), Total = 1000m, CustomerId = 1
            });
            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1005, PurchaseDateUTC = new DateTime(2021, 1, 4, 2, 25, 55), Total = 56m, CustomerId = 2
            });
            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1006, PurchaseDateUTC = new DateTime(2021, 1, 7, 3, 12, 57), Total = 75m, CustomerId = 2
            });
            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1007, PurchaseDateUTC = new DateTime(2021, 1, 12, 1, 17, 12), Total = 987m, CustomerId = 3
            });
            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1008, PurchaseDateUTC = new DateTime(2021, 1, 15, 8, 55, 00), Total = 12000m,
                CustomerId = 3
            });
            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1009, PurchaseDateUTC = new DateTime(2021, 1, 25, 10, 43, 10), Total = 1m, CustomerId = 3
            });
            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1010, PurchaseDateUTC = new DateTime(2021, 2, 2, 17, 32, 22), Total = 100m, CustomerId = 4
            });
            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1011, PurchaseDateUTC = new DateTime(2021, 2, 2, 15, 22, 35), Total = 256m, CustomerId = 1
            });
            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1012, PurchaseDateUTC = new DateTime(2021, 2, 7, 12, 07, 45), Total = 887m, CustomerId = 3
            });
            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1013, PurchaseDateUTC = new DateTime(2021, 2, 9, 9, 00, 10), Total = 673m, CustomerId = 1
            });
            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1014, PurchaseDateUTC = new DateTime(2021, 1, 12, 1, 17, 12), Total = 987m, CustomerId = 3
            });
            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1015, PurchaseDateUTC = new DateTime(2021, 1, 15, 8, 55, 00), Total = 12000m,
                CustomerId = 3
            });
            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1016, PurchaseDateUTC = new DateTime(2021, 1, 25, 10, 43, 10), Total = 1m, CustomerId = 3
            });
            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1017, PurchaseDateUTC = new DateTime(2021, 1, 25, 12, 43, 10), Total = 111m, CustomerId = 3
            });
            ctx.Purchases.Add(new Purchase()
            {
                PurchaseId = 1018, PurchaseDateUTC = new DateTime(2021, 1, 25, 16, 43, 10), Total = 10m, CustomerId = 3
            });
            ctx.SaveChanges();
        }
    }
}

public class Purchase
{
    public int PurchaseId { get; set; }
    public DateTime PurchaseDateUTC { get; set; }
    public Decimal Total { get; set; }
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
}

public class Customer
{
    public int CustomerId { get; set; }
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
}

public class DatabaseContext : DbContext
{
    public DatabaseContext() : base()
    {
    }

    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<Customer> Customers { get; set; }
}
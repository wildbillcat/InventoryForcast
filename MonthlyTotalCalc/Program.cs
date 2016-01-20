using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using CsvHelper;
using InventoryForcast.Models.Calculations;
using InventoryForcast.Models;

namespace MonthlyTotalCalc
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string filepath in System.IO.Directory.EnumerateFiles(@"C:\Users\wildbillcat\Sync\WebSpecs\SKUs", "*.csv"))
            {
                using (var csv = new CsvReader(System.IO.File.OpenText(filepath)))
                {
                    string[] datePieces = filepath.Split('\\').Last().Split('.')[0].Split('-');
                    DateTime SalesDate = new DateTime(int.Parse(datePieces[2]), int.Parse(datePieces[0]), int.Parse(datePieces[1]));
                    SalesDate.AddDays(-1 * (SalesDate.Day - 1));//Ensures Day of the first of the month
                    int MonthID = (12 * SalesDate.Year) + SalesDate.Month;
                    while (csv.Read())
                    {
                        using(var ctx = new InventoryForcast.Models.ApplicationDbContext())
                        {
                            int _sku = csv.GetField<int>(0);
                            System.Console.WriteLine(string.Concat("SKU: ", _sku, " Date: ", SalesDate));
                            int _qty_sold = csv.GetField<int>(1);
                            ctx.MonthlyTotals.Add(new MonthlyTotal()
                            {
                                SKU = _sku,
                                Quantity_Sold = _qty_sold,
                                Absolute_Quantity_Sold = MonthlyTotal.RemoveSeasonality(_qty_sold, SalesDate.Month),
                                Date = SalesDate,
                                Month_Id = MonthID
                            }
                            );
                            ctx.SaveChanges();
                        }                         
                    }                        
                }
            }
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ordering;

namespace EDI.Tests
{
    [TestClass]
    public class MakeTheseTestsPass
    {
        [TestMethod]
        public void can_generate_850_edi_file_from_purchase_order()
        {

            PurchaseOrder order = new PurchaseOrder("10000001",
                                    new Customer("123", "John Doe", "John Doe",
                                    new Address("1001","1 Main st.", "San Francisco", "California","CA", "USA","US", "90210"),
                                    new Address("1002","1 Main st.", "San Francisco", "California","CA", "USA","US", "90210")),
                new List<LineItem>
                {
                    new LineItem("1", "CTGTN04B", (decimal) 80.01,32, "Brother HL-2700CN; MFC-9420 - Toner Cartridge, Black"),
                    new LineItem("2", "CTGTN04C", (decimal) 80.01,23, "Brother HL-2700CN; MFC-9420 - Toner Cartridge, Cyan"),
                    new LineItem("3", "CTGTN04M", (decimal) 80.01, 11, "Brother HL-2700CN; MFC-9420 - Toner Cartridge, Magenta")

                });

            string fileName=@"c:\abhi\";
            PurchaseOrderGenerator generator = new PurchaseOrderGenerator();
            generator.GeneratePurchaseOrderFile(order, fileName);

            //IEnumerable<string> lines = File.ReadLines(fileName);
            //byte[] bytes = File.ReadAllBytes(fileName);

            //Assert.IsTrue(File.Exists(fileName));
            //Assert.IsTrue(bytes.Length > 0);
            //Assert.AreNotEqual(0, lines.Count());
            //TODO:Add more asserts to verify that the file is the proper EDI format
        }

        [TestMethod]
        public void can_read_810_invoice_file_into_invoice()
        {
            string fileName = @"c:\810file.txt";

            var reader = new InvoiceReader();
            Invoice invoice = reader.ReadInvoiceFile(fileName);

            Assert.IsNotNull(invoice);
            Assert.IsNotNull(invoice.Customer);
            Assert.IsNotNull(invoice.Items);
            Assert.IsNotNull(invoice.PurchaseOrderNumber);
            Assert.IsNotNull(invoice.Total);
            Assert.IsNotNull(invoice.InvoiceDate);

            //TODO:Add more asserts based on data in sample file that is not yet created
            //for example:
            Assert.AreEqual("123", invoice.Customer.Id);
            Assert.AreEqual("PO3445", invoice.PurchaseOrderNumber);
        }
    }
}

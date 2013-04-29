using System;
using System.Collections.Generic;

namespace Ordering
{
    public class Invoice
    {
        public Customer Customer { get; set; }
        public IEnumerable<LineItem> Items { get; set; }
        public string Status { get; protected set; }
        public string PurchaseOrderNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public DateTime ShipDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal Total { get; set; }

        public Invoice(string invoiceNumber, string purchaseOrderNumber, decimal total,
                       DateTime shipDate, DateTime invoiceDate, Customer customer, 
                        IEnumerable<LineItem> items)
        {
            Total = total;
            ShipDate = shipDate;
            InvoiceDate = invoiceDate;
            InvoiceNumber = invoiceNumber;
            PurchaseOrderNumber = purchaseOrderNumber;
            Customer = customer;
            Items = items;
        }
    }
}
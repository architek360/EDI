using System;
using System.Collections.Generic;

namespace Ordering
{
    public class PurchaseOrder
    {
        public Customer Customer { get; protected set; }
        public IEnumerable<LineItem> Items { get; protected set; }
        public string Status { get; protected set; }
        public string Number { get; protected set; }
        public DateTime Date { get; protected set; }
        public PurchaseOrder(string number, Customer customer, IEnumerable<LineItem> items)
        {
            Number = number;
            Customer = customer;
            Items = items;
        }
    }

    public class Customer
    {
        public string Id { get; protected set; }
        public string BillingName { get; protected set; }
        public string ShippingName { get; protected set; }
        public Address BillingAddress { get; protected set; }
        public Address ShippingAddress { get; protected set; }

        public Customer(string id, string billingName, string shippingName, Address billingAddress, Address shippingAddress)
        {
            Id = id;
            BillingName = billingName;
            ShippingName = shippingName;
            BillingAddress = billingAddress;
            ShippingAddress = shippingAddress;
        }
    }

    public class Address
    {
        public string Id { get; protected set; }
        public string Street { get; protected set; }
        public string City { get; protected set; }
        public string State { get; protected set; }
        public string ShortState { get; protected set; }
        public string Country { get; protected set; }
        public string ShortCountry { get; protected set; }
        public string PostalCode { get; protected set; }

        public Address(string id, string street, string city, string state,string shortState, string country,string shortCountry, string postalCode)
        {
            Id = id;
            Street = street;
            City = city;
            State = state;
            Country = country;
            PostalCode = postalCode;
            ShortState = shortState;
            ShortCountry = shortCountry;
        }
    }

    public class LineItem
    {
        public string Number { get; protected set; }
        public string SupplierItemNumber { get; protected set; }
        public decimal Price { get; protected set; }
        public int Quantity { get; protected set; }
        public string Name { get; protected set; }

        public LineItem(string lineNumber, string supplierItemNumber, decimal price, int quantity, string name)
        {
            SupplierItemNumber = supplierItemNumber;
            Number = lineNumber;
            Price = price;
            Quantity = quantity;
            Name = name;
        }
    }
}

using System;
using OopFactory.X12.Parsing.Model;

namespace Ordering
{
    public class PurchaseOrderGenerator : IGenerateEDIPurchaseOrders
    {
        /// <summary>
        /// Creates a EDI 850 Purchase order file that adheres to the EDI specification
        /// </summary>
        /// <param name="purchaseOrder">Input parameters for the file</param>
        /// <param name="fileName">Output file name</param>
        /// 

        private String InterchangeSenderId = "828513080"; // need a mutually agreed up ID
        private String InterchangeReceiverId = "001903202U"; // will be provided by the supplier
        
        //TODO:Pass in sender and receiver id
        
        public void GeneratePurchaseOrderFile(PurchaseOrder purchaseOrder, string fileName)
        {
            CreatePurchaseOrder850(purchaseOrder,fileName);
        }

        public void CreatePurchaseOrder850(PurchaseOrder purchaseOrder,string fileName)
        {

            DateTime purcaseOrderDate = DateTime.Now;
            Interchange interchange = new Interchange(purcaseOrderDate, Convert.ToInt32(purchaseOrder.Number), true)
            {
                InterchangeSenderIdQualifier = "ZZ",
                InterchangeSenderId = InterchangeSenderId,  
                InterchangeReceiverIdQualifier = "ZZ",
                InterchangeReceiverId = InterchangeReceiverId, 
                InterchangeDate = purcaseOrderDate,
            };

            interchange.SetElement(14, "0"); //No Aknowlegement is 0

            FunctionGroup group = interchange.AddFunctionGroup("PO", purcaseOrderDate, Convert.ToInt32(purchaseOrder.Number), "004010");
            group.ApplicationSendersCode = InterchangeSenderId;
            group.ApplicationReceiversCode = InterchangeReceiverId;
           // group.Date = purcaseOrderDate;
           // group.ControlNumber = Convert.ToInt32(purchaseOrder.Number);

            Transaction transaction = group.AddTransaction("850", "0001");

            Segment bhtSegment = transaction.AddSegment("BEG");
            bhtSegment.SetElement(1, "00");
            bhtSegment.SetElement(2, "SA");
            bhtSegment.SetElement(3, purchaseOrder.Number);
            bhtSegment.SetElement(5, purcaseOrderDate.ToString("yyyyMMdd"));

            //bhtSegment = transaction.AddSegment("CUR");
            //bhtSegment.SetElement(1, "BY");
            //bhtSegment.SetElement(2, "USD");

            //bhtSegment = transaction.AddSegment("PER");
            //bhtSegment.SetElement(1, "IC");
            //bhtSegment.SetElement(2, "Doe,Jane");
            //bhtSegment.SetElement(8, "Doe,Jane");

            //bill to section 
            Loop loop = transaction.AddLoop("N1");
            loop.SetElement(1, "BT");
            loop.SetElement(2, purchaseOrder.Customer.BillingName);
            loop.SetElement(3,92);
            loop.SetElement(4,purchaseOrder.Customer.BillingAddress.Id);

            Segment segment = loop.AddSegment("N3");
            segment.SetElement(1, purchaseOrder.Customer.BillingAddress.Street);
            segment.SetElement(2, purchaseOrder.Customer.BillingAddress.City + " " + purchaseOrder.Customer.BillingAddress.State + " " + purchaseOrder.Customer.BillingAddress.PostalCode + " " + purchaseOrder.Customer.BillingAddress.Country);
            segment = loop.AddSegment("N4");
            segment.SetElement(1, purchaseOrder.Customer.BillingAddress.City);
            segment.SetElement(2, purchaseOrder.Customer.BillingAddress.ShortState); // short state
            segment.SetElement(3, purchaseOrder.Customer.BillingAddress.PostalCode);
            segment.SetElement(4, purchaseOrder.Customer.BillingAddress.ShortCountry); //short country

            //ship to to section 
            loop = transaction.AddLoop("N1");
            loop.SetElement(1, "ST");
            loop.SetElement(2, purchaseOrder.Customer.ShippingName);
            loop.SetElement(3, 92);
            loop.SetElement(4, purchaseOrder.Customer.ShippingAddress.Id);

            segment = loop.AddSegment("N3");
            segment.SetElement(1, purchaseOrder.Customer.ShippingAddress.Street);
            segment.SetElement(2, purchaseOrder.Customer.ShippingAddress.City + " " + purchaseOrder.Customer.ShippingAddress.State + " " + purchaseOrder.Customer.ShippingAddress.PostalCode + " " + purchaseOrder.Customer.ShippingAddress.Country);
            segment = loop.AddSegment("N4");
            segment.SetElement(1, purchaseOrder.Customer.ShippingAddress.City);
            segment.SetElement(2, purchaseOrder.Customer.BillingAddress.ShortState); // short state
            segment.SetElement(3, purchaseOrder.Customer.BillingAddress.PostalCode);
            segment.SetElement(4, purchaseOrder.Customer.BillingAddress.ShortCountry); //short country

            int count = 1;
            foreach(LineItem lineItem in purchaseOrder.Items)
            {
                loop = transaction.AddLoop("PO1");
                loop.SetElement(1, count++);
                loop.SetElement(2, lineItem.Quantity);
                loop.SetElement(3, "EA");
                loop.SetElement(4, lineItem.Price);
                loop.SetElement(6, "CGT");
                loop.SetElement(7, lineItem.SupplierItemNumber);
            }

            loop = transaction.AddLoop("CTT");
            loop.SetElement(1, --count);



            var x12 = interchange.SerializeToX12(true);
            Console.WriteLine(x12);
            // Write the string to a file.
            fileName += purchaseOrder.Number + ".850";
            System.IO.StreamWriter file = new System.IO.StreamWriter(fileName);
            file.WriteLine(x12);

            file.Close();
        }

        #region Sample Code
        public void CreatePurchaseOrder850()
        {
            DateTime purcaseOrderDate = DateTime.Now;
            Interchange interchange = new Interchange(purcaseOrderDate, 245, true)
            {
                InterchangeSenderIdQualifier = "01",
                InterchangeSenderId = "828513080",
                InterchangeReceiverIdQualifier = "01",
                InterchangeReceiverId = "001903202U",
                InterchangeDate = purcaseOrderDate,
            };

            interchange.SetElement(14, "0"); //No Aknowlegement is 0

            FunctionGroup group = interchange.AddFunctionGroup("PO", purcaseOrderDate, 1, "005010X222");
            group.ApplicationSendersCode = "828513080";
            group.ApplicationReceiversCode = "001903202U";
            group.Date = purcaseOrderDate;
            group.ControlNumber = 245;

            Transaction transaction = group.AddTransaction("850", "0001");

            Segment bhtSegment = transaction.AddSegment("BEG");
            bhtSegment.SetElement(1, "00");
            bhtSegment.SetElement(2, "SA");
            bhtSegment.SetElement(3, "S41000439");
            bhtSegment.SetElement(5, "20100810");

            bhtSegment = transaction.AddSegment("CUR");
            bhtSegment.SetElement(1, "BY");
            bhtSegment.SetElement(2, "USD");

            bhtSegment = transaction.AddSegment("PER");
            bhtSegment.SetElement(1, "IC");
            bhtSegment.SetElement(2, "Doe,Jane");
            bhtSegment.SetElement(8, "Doe,Jane");


            // Bill to address
            Loop loop= transaction.AddLoop("N1");
            loop.SetElement(1, "BT");
            loop.SetElement(2,"Doe Jane");
            loop.SetElement(3, 92);
            loop.SetElement(4, 10001);
            Segment segment=loop.AddSegment("N3");
            segment.SetElement(1, "Street");
            segment.SetElement(2, "City State Pin Country");
            segment = loop.AddSegment("N4");
            segment.SetElement(1,"City");
            segment.SetElement(2,"SS");
            segment.SetElement(3,"PIN");
            segment.SetElement(4,"SC");



            // ship to address
            loop = transaction.AddLoop("N1");
            loop.SetElement(1, "ST");
            loop.SetElement(2, "Doe Jane");
            loop.SetElement(3, 92);
            loop.SetElement(4, 10001);

            segment = loop.AddSegment("N3");
            segment.SetElement(1, "Street");
            segment.SetElement(2, "City State Pin Country");
            segment = loop.AddSegment("N4");
            segment.SetElement(1, "City");
            segment.SetElement(2, "SS");
            segment.SetElement(3, "PIN");
            segment.SetElement(4, "SC");

            // add products ordered

            loop = transaction.AddLoop("PO1");
            loop.SetElement(1,"1");
            loop.SetElement(2, 10);
            loop.SetElement(3, "EA");
            loop.SetElement(4, "10.2");
            loop.SetElement(6, "IB");
            loop.SetElement(7,"100001");
            
            
            

            var x12 = interchange.SerializeToX12(true);
            Console.WriteLine(x12);
        }

        #endregion

    }
}

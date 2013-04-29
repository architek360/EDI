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
        public void GeneratePurchaseOrderFile(PurchaseOrder purchaseOrder, string fileName)
        {
            throw new NotImplementedException();
        }

        #region Sample Code
        public void CreatePurchaseOrder850()
        {
            DateTime purcaseOrderDate = new DateTime(2010, 8, 17, 08, 50, 0);
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
            bhtSegment.SetElement(1, "05");
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

            var x12 = interchange.SerializeToX12(false);
            Console.WriteLine(x12);
        }

        #endregion

    }
}

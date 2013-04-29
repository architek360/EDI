using System;
using OopFactory.X12.Parsing.Model;
using OopFactory.X12.Parsing.Model.Typed;

namespace Ordering
{
    public class InvoiceReader : IReadEDIInvoices
    {
        /// <summary>
        /// Reads an EDI 810 Invoice and translates it to a Invoice object
        /// </summary>
        /// <param name="fileName">File name of the EDI 810 invoice file</param>
        /// <returns></returns>
        public Invoice ReadInvoiceFile(string fileName)
        {
            throw new NotImplementedException();
        }

        #region Sample Code

        public void Create810_4010Version()
        {
            var message = new Interchange(Convert.ToDateTime("1/4/99 15:32"), 35, false, '~', '*', '>')
                {
                    SecurityInfoQualifier = "00",
                    InterchangeSenderIdQualifier = "30",
                    InterchangeSenderId = "943274043",
                    InterchangeReceiverIdQualifier = "16",
                    InterchangeReceiverId = "0069088189999"
                };

            var fg = message.AddFunctionGroup("IN", Convert.ToDateTime("1/4/1999 15:32"), 1);
            fg.ApplicationSendersCode = "943274043TO";
            fg.ApplicationReceiversCode = "0069088189999";
            fg.ResponsibleAgencyCode = "X";
            fg.VersionIdentifierCode = "004010";

            var trans = fg.AddTransaction("810", "0001");

            var big = trans.AddSegment(new TypedSegmentBIG());
            big.BIG01_InvoiceDate = Convert.ToDateTime("10/14/1998");
            big.BIG02_InvoiceNumber = "3662";
            big.BIG07_TransactionTypeCode = "N6";

            var billTo = trans.AddLoop(new TypedLoopN1());
            billTo.N101_EntityIdentifierCodeEnum = EntityIdentifierCode.BillToParty;
            billTo.N102_Name = "The Scheduling Coordinator, Inc";

            var billToAddress = billTo.AddSegment(new TypedSegmentN3());
            billToAddress.N301_AddressInformation = "53241 Hamilton Dr";

            var billToLocale = billTo.AddSegment(new TypedSegmentN4());
            billToLocale.N401_CityName = "Palo Alto";
            billToLocale.N402_StateOrProvinceCode = "CA";
            billToLocale.N403_PostalCode = "95622";
            billToLocale.N404_CountryCode = "US";

            var remitTo = trans.AddLoop(new TypedLoopN1());
            remitTo.N101_EntityIdentifierCodeEnum = EntityIdentifierCode.PartyToReceiveCommercialInvoiceRemittance;
            remitTo.N102_Name = "Bank of America- (Mkt and GMC)";

            var remitToAddress = remitTo.AddSegment(new TypedSegmentN3());
            remitToAddress.N301_AddressInformation = "1850 Gateway Boulevard";

            var remitToLocale = remitTo.AddSegment(new TypedSegmentN4());
            remitToLocale.N401_CityName = "Concord";
            remitToLocale.N402_StateOrProvinceCode = "CA";
            remitToLocale.N403_PostalCode = "94520";
            remitToLocale.N404_CountryCode = "US";

            var remitToRef1 = remitTo.AddSegment(new TypedSegmentREF());
            remitToRef1.REF01_ReferenceIdQualifier = "11";
            remitToRef1.REF02_ReferenceId = "1233626208";

            var remitToRef2 = remitTo.AddSegment(new TypedSegmentREF());
            remitToRef2.REF01_ReferenceIdQualifier = "01";
            remitToRef2.REF02_ReferenceId = "026009593";

            var itd = trans.AddSegment(new TypedSegmentITD());
            itd.ITD01_TermsTypeCode = "03";
            itd.ITD06_TermsNetDueDate = Convert.ToDateTime("10/20/1998");

            var it1 = trans.AddLoop(new TypedLoopIT1());
            it1.IT101_AssignedIdentification = "1";
            it1.IT102_QuantityInvoiced = 1;
            it1.IT103_UnitOrBasisForMeasurementCode = UnitOrBasisOfMeasurementCode.Each;
            it1.IT104_UnitPrice = 2896035.3m;

            var pid = it1.AddLoop(new TypedLoopPID());
            pid.PID01_ItemDescriptionType = "X";
            pid.PID05_Description = "RMR Scheduling Coordinator - Estimated RMR";

            var tds = trans.AddSegment(new TypedSegmentTDS());
            tds.TDS01_AmountN2 = 289603530;

            var ctt = trans.AddSegment(new TypedSegmentCTT());
            ctt.CTT01_NumberOfLineItems = 1;

            var x12 = message.SerializeToX12(false);
            Console.WriteLine(x12);
        }

        #endregion
    }
}
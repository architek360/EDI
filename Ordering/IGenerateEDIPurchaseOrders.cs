namespace Ordering
{
    public interface IGenerateEDIPurchaseOrders
    {
        void GeneratePurchaseOrderFile(PurchaseOrder purchaseOrder, string fileName);

    }
}
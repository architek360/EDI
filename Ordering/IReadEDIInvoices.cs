namespace Ordering
{
    public interface IReadEDIInvoices
    {
        Invoice ReadInvoiceFile(string fileName);
    }
}
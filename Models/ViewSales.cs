namespace Asp.NetProject.Models
{
    public class ViewSales
    {
        // Products properties
        public Sale? objSale { get; set; }
        public IList<SaleLine>? ListSaleLine { get; set; }
    }
}

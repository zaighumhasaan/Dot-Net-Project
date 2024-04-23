namespace Asp.NetProject.Models
{
    public class SaleDetailViewModel
    {

        public int SaleId { get; set; } 
        public DateTime SaleDate { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public List<SaleLine>? SaleLines { get; set; }

    }
}

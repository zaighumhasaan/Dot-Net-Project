using System.ComponentModel.DataAnnotations.Schema;

namespace Asp.NetProject.Models
{
    public class ViewProduct
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = null!;

        public string? Formula { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public DateOnly? MfgDate { get; set; }

        public DateOnly? ExpDate { get; set; }

        public int? StockQuantity { get; set; }

        public string? Sku { get; set; }

        public string? Barcode { get; set; }

        public string? ImagePath { get; set; }

        public int? CategoryId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public decimal? PurchasePrice { get; set; }

        public int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
        public virtual ProductCategory? Category { get; set; }

        //Product Category

        public string CategoryName { get; set; } = null!;

        public string? CatCode { get; set; }

        public string? CatSerial { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifyBy { get; set; }

    }
}

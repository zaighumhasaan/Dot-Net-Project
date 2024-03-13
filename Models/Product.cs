using System;
using System.Collections.Generic;

namespace Asp.NetProject.Models;

public partial class Product
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

    public virtual ProductCategory? Category { get; set; }

    public virtual ICollection<SaleLine> SaleLines { get; set; } = new List<SaleLine>();
}

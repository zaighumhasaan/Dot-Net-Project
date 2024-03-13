using System;
using System.Collections.Generic;

namespace Asp.NetProject.Models;

public partial class ProductCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public string? CatCode { get; set; }

    public string? CatSerial { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifyBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

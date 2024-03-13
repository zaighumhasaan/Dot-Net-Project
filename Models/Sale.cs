using System;
using System.Collections.Generic;

namespace Asp.NetProject.Models;

public partial class Sale
{
    public int SaleId { get; set; }

    public decimal TotalAmount { get; set; }

    public int? EmployeeId { get; set; }

    public decimal? Discount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<SaleLine> SaleLines { get; set; } = new List<SaleLine>();
}

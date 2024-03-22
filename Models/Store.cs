using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Asp.NetProject.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string StoreName { get; set; } = null!;

    public string? Description { get; set; }

    public string? Location { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Website { get; set; }

    public string? OpeningHours { get; set; }

//    public byte[]? Logo { get; set; }

    public int? OwnerId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[]? Profit { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Owner? Owner { get; set; }
    [NotMapped]
    public IFormFile? LogoFile { get; set; }//ImageFile to get image as an input from the user
    public String? Logo { get; set; }//Image
}

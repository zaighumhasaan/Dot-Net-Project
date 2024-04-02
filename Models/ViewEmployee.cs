using System.ComponentModel.DataAnnotations.Schema;

namespace Asp.NetProject.Models
{
    public class ViewEmployee
    {
        public int EmployeeId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateOnly? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public int? StoreId { get; set; }



        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public decimal? Salary { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? RoleId { get; set; }

        public virtual Store? Store { get; set; }


        public String? Image { get; set; }//Image

        //Role Properties 

        public string RoleName { get; set; } = null!;


    }
}

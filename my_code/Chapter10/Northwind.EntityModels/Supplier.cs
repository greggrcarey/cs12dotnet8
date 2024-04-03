using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.EntityModels
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }
        [StringLength(40)]
        public string CompanyName { get; set; } = null!;

        [StringLength(30)]
        public string? ContactName { get; set; }

        [StringLength(30)]
        public string? ContactTitle { get; set; }
        [StringLength(60)]
        public string? Address { get; set; }
        [StringLength(24)]
        public string? Phone { get; set; }
        [Column("HomePage", TypeName = "ntext")]
        public string? Website { get; set; }
    }
}

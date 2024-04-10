﻿using System.ComponentModel.DataAnnotations; //[Required] [StringLength]
using System.ComponentModel.DataAnnotations.Schema; // [Column]

namespace Northwind.EntityModels;

public class Product
{
    [Required]
    [StringLength(40)]
    public string ProductName { get; set; } = null!;

    public int? SupplierId { get; set; }
    public int? CategoryId { get; set; }

    [StringLength(20)]
    public string? QuantityPerUnit { get; set; }

    //Required for SQL Server provider
    [Column(TypeName = "money")]
    public decimal? UnitPrice { get; set; }
    public short? UnitsInStock { get; set; }
    public short? UnitsOnOrder { get; set; }
    public short? ReorderLevel { get; set; }
    public bool Discontinued { get; set; }
}


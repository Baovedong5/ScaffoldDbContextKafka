using System;
using System.Collections.Generic;

namespace ProductServices.Models;

public partial class TableProduct
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int Quantity { get; set; }
}

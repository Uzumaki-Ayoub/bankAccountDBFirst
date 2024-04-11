using System;
using System.Collections.Generic;

namespace DBFIRST.Models;

public partial class BankAccount
{
    public int Id { get; set; }

    public string AccountNumber { get; set; } = null!;

    public string ClientName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public decimal? AccountBalance { get; set; }

    public virtual Transaction? Transaction { get; set; }
}

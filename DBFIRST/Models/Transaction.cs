using System;
using System.Collections.Generic;

namespace DBFIRST.Models;

public partial class Transaction
{
    public string AccountNumber { get; set; } = null!;

    public int? Deposit { get; set; }

    public int? Withdraw { get; set; }

    public virtual BankAccount AccountNumberNavigation { get; set; } = null!;
}

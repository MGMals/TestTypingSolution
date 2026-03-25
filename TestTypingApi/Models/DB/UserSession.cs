using System;
using System.Collections.Generic;

namespace TestTypingApi.Models.DB;

public partial class UserSession
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Token { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public virtual TestTypeUser User { get; set; } = null!;
}

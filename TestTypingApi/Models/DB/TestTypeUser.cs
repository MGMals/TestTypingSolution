using System;
using System.Collections.Generic;

namespace TestTypingApi.Models.DB;

public partial class TestTypeUser
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<TypingTest> TypingTests { get; set; } = new List<TypingTest>();

    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
}

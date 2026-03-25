using System;
using System.Collections.Generic;

namespace TestTypingApi.Models.DB;

public partial class TypingTest
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int Wpm { get; set; }

    public decimal? Accuracy { get; set; }

    public int? CharactersTyped { get; set; }

    public int? Errors { get; set; }

    public int? TestDuration { get; set; }

    public string? Difficulty { get; set; }

    public DateTime? TestDate { get; set; }

    public virtual TestTypeUser User { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace studak.spbrtk.API.Models;

public partial class Involvementstatus
{
    public int Id { get; set; }

    public string? InvolvementName { get; set; }

    public virtual ICollection<Involvement> Involvements { get; set; } = new List<Involvement>();
}

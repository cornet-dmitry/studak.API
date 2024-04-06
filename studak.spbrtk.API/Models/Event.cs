using System;
using System.Collections.Generic;

namespace studak.spbrtk.API.Models;

public partial class Event
{
    public int Id { get; set; }

    public int? Responsible { get; set; }

    public int? Direction { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? Rate { get; set; }

    public virtual Direction? DirectionNavigation { get; set; }

    public virtual ICollection<Involvement> Involvements { get; set; } = new List<Involvement>();

    public virtual User? ResponsibleNavigation { get; set; }
}

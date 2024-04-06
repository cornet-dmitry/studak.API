using System;
using System.Collections.Generic;

namespace studak.spbrtk.API.Models;

public class User
{
    public int Id { get; set; }

    public string? Surname { get; set; }

    public string? Name { get; set; }

    public string? Patronymic { get; set; }

    public string? Group { get; set; }

    public DateTime? DateBirth { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? VkLink { get; set; }

    public string? TgLink { get; set; }

    public int? Kpi { get; set; }

    public int? Status { get; set; }

    public string? OrderNumber { get; set; }

    public DateTime? StartDate { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Involvement> Involvements { get; set; } = new List<Involvement>();

    public virtual UserStatus? StatusNavigation { get; set; }
}

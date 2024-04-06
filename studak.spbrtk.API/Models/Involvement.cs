using System;
using System.Collections.Generic;

namespace studak.spbrtk.API.Models;

public partial class Involvement
{
    public int Eventid { get; set; }

    public int Userid { get; set; }

    public int Status { get; set; }

    public DateTime Createtime { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Involvementstatus StatusNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using studak.spbrtk.API.Models;

namespace studak.spbrtk.API.Models
{
    
    public partial class Involvementstatus
    {
        public int Id { get; set; }

        public string? InvolvementName { get; set; }

        public virtual ICollection<Involvement> Involvements { get; } = new List<Involvement>();
    }

}

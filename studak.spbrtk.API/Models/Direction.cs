using System;
using System.Collections.Generic;

namespace studak.spbrtk.API.Models
{
    
    public partial class Direction
    {
        public int Id { get; set; }

        public string? DirectionShortName { get; set; }

        public string? DirectionLongName { get; set; }

        public virtual ICollection<Event> Events { get; } = new List<Event>();
    }

}

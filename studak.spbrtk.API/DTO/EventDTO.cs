using System;

namespace studak.spbrtk.API.DTO
{
    
    public class EventDTO
    {
        public int Id { get; set; }

        public string? Responsible { get; set; }

        public string? Direction { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
        
        public string? Place { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? Rate { get; set; }
        
        public bool? Isactice { get; set; }
    }
}

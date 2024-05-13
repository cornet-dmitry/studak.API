using System;
using System.Collections.Generic;
using studak.spbrtk.API.Models;

namespace studak.spbrtk.API.Models
{
    
    public partial class UserStatus
    {
        public int Id { get; set; }

        public string? StatusName { get; set; }

        public virtual ICollection<User> Users { get; } = new List<User>();
    }

}

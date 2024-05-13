using System;
using System.Collections.Generic;

namespace studak.spbrtk.API.Models
{
    
    public partial class Admin
    {
        public int Userid { get; set; }

        public string Userlogin { get; set; } = null!;

        public byte[] Userpasswordhash { get; set; } = null!;

        public byte[] Userpasswordsalt { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }

}

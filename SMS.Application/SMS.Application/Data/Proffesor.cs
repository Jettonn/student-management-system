using System;
using System.Collections.Generic;

#nullable disable

namespace SMS.Application.Data
{
    public partial class Proffesor
    {
        public int ProffesorId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

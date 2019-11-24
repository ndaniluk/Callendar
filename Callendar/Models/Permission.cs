using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Callendar
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PermissionLevel { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
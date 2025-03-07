using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using projetTicket.Models;

namespace projetTicket.Data
{
    public class projetTicketContext : DbContext
    {
        public projetTicketContext (DbContextOptions<projetTicketContext> options)
            : base(options)
        {
        }

        public DbSet<projetTicket.Models.Ticket> Ticket { get; set; } = default!;
        public DbSet<projetTicket.Models.Utilisateur> Utilisateur { get; set; } = default!;
    }
}

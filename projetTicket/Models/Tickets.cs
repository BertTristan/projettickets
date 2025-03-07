using System.ComponentModel.DataAnnotations;

namespace projetTicket.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public TicketStatus Status { get; set; } = TicketStatus.Low;

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }

        public bool IsResolved { get; set; }

        [StringLength(100)] 
        public string AssignedTo { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ResolutionDate { get; set; }
    }

    public enum TicketStatus
    {
        Low,
        Medium,
        High,
        Urgent
    }


}

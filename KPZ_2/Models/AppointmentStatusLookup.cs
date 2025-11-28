using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedTeleHelp.WPF.Models
{
    public class AppointmentStatusLookup
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; } 
        public string StatusName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Admin : BaseEntity
    {
        [Key]
        public Guid AdminId { get; set; }

        [Required]
        [StringLength(30)]
        public string AdminUserName { get; set; }

        [Required]
        [StringLength(30)]
        public string AdminFirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string AdminLastName { get; set; }

        [Required]
        [StringLength(60)]
        public string AdminPassword { get; set; }

        [Required]
        [StringLength(30)]
        public string AdminEmail { get; set; }

        [Required]
        [StringLength(30)]
        public string AdminPhoneNumber { get; set; }

        [Required]
        [StringLength(80)]
        public string AdminAddress { get; set; }

        [JsonIgnore]
        public ICollection<Jersey> Jerseys { get; set; }
    }
}

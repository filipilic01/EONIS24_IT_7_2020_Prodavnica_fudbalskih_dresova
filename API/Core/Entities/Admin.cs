﻿using System;
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
        public string AdminKorisnickoIme { get; set; }

        [Required]
        [StringLength(30)]
        public string AdminIme { get; set; }

        [Required]
        [StringLength(30)]
        public string AdminPrezime { get; set; }

        [Required]
        [StringLength(60)]
        public string AdminLozinka { get; set; }

        [Required]
        [StringLength(30)]
        public string AdminEmail { get; set; }

        [Required]
        [StringLength(30)]
        public string AdminBrojTelefona { get; set; }

        [Required]
        [StringLength(80)]
        public string AdminAdresa { get; set; }

        [JsonIgnore]
        public ICollection<Dres> Dress { get; set; }
    }
}

﻿using Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Dtos.Porudzbina
{
    public class PorudzbinaDto
    {
        public Guid PorudzbinaId { get; set; }
        public double UkupanIznos { get; set; }
        public DateTime DatumAzuriranja { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public bool Placena { get; set; }
        public string Kupac { get; set; }

        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }

    }
}

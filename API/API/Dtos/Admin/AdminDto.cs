using System.ComponentModel.DataAnnotations;

namespace API.Dtos.Admin
{
    public class AdminDto
    {
        public Guid AdminId { get; set; }

        public string AdminKorisnickoIme { get; set; }

     
        public string AdminIme { get; set; }

      
        public string AdminPrezime { get; set; }


        public string AdminEmail { get; set; }

        public string AdminBrojTelefona { get; set; }

        public string AdminAdresa { get; set; }
    }
}

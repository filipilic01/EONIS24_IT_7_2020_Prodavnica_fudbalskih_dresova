using Core.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class DresWithAdminSpecification : BaseSpecification<Dres>
    {
        public DresWithAdminSpecification(DresSpecParams specParams)
            : base(x =>
            (specParams.Search == null || x.ImeIgraca.ToLower().Contains(specParams.Search) ||
             x.Tip.ToLower().Contains(specParams.Search) || x.Obrisan.ToString().ToLower().Contains(specParams.Search) || x.Sezona.ToLower().Contains(specParams.Search) ||
             x.Takmicenje.ToLower().Contains(specParams.Search) || x.Tim.ToLower().Contains(specParams.Search)) &&
            
                (specParams.Tip == null || x.Tip == specParams.Tip) && (specParams.Sezona == null || x.Sezona == specParams.Sezona) &&
                    (specParams.Takmicenje == null || x.Takmicenje == specParams.Takmicenje) && (specParams.Player == null || x.ImeIgraca == specParams.Player)
                        && (specParams.Tim == null || x.Tim == specParams.Tim) && (!specParams.Obrisan.HasValue || x.Obrisan == specParams.Obrisan))

        {
            AddInclude(x => x.Admin);
            AddOrderBy(x => x.ImeIgraca);
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "CenaAsc":
                        AddOrderBy(p => p.Cena);
                        break;

                    case "CenaDesc":
                        AddOrderByDescending(p => p.Cena);
                        break;

                    default:
                        AddOrderBy(x => x.ImeIgraca);
                        break;
                }
            }
        }

        public DresWithAdminSpecification(Guid id) : base(x => x.DresId == id)
        {
            AddInclude(x => x.Admin);
        }
    }
}

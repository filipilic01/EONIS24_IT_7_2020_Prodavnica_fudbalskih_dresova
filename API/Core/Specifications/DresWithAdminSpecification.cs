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
            (specParams.Search == null || x.ImeIgraca.ToLower().Contains(specParams.Search)) &&
                (specParams.Tim == null || x.Tim == specParams.Tim) && (specParams.Brend == null || x.Brend == specParams.Brend) &&
                    (specParams.Takmicenje == null || x.Takmicenje == specParams.Takmicenje) && (specParams.Player == null || x.ImeIgraca == specParams.Player))

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

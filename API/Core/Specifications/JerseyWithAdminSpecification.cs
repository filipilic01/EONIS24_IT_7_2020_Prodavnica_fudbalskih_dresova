using Core.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class JerseysWithAdminSpecification : BaseSpecification<Jersey>
    {
        public JerseysWithAdminSpecification(JerseySpecParams specParams)
            : base(x =>
            (specParams.Search == null || x.PlayerName.ToLower().Contains(specParams.Search)) &&
                (specParams.Team == null || x.Team == specParams.Team) && (specParams.Brand == null || x.Brand == specParams.Brand) &&
                    (specParams.Competition == null || x.Competition == specParams.Competition) && (specParams.Player == null || x.PlayerName == specParams.Player))

        {
            AddInclude(x => x.Admin);
            AddOrderBy(x => x.PlayerName);
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;

                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;

                    default:
                        AddOrderBy(x => x.PlayerName);
                        break;
                }
            }
        }

        public JerseysWithAdminSpecification(Guid id) : base(x => x.JerseyId == id)
        {
            AddInclude(x => x.Admin);
        }
    }
}

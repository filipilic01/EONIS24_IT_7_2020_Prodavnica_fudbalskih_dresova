using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class JerseyWithFiltersForCountSpecification : BaseSpecification<Jersey>
    {
        public JerseyWithFiltersForCountSpecification(JerseySpecParams specParams)
             : base(x =>
             (specParams.Search == null || x.PlayerName.ToLower().Contains(specParams.Search)) &&
                (specParams.Team == null || x.Team == specParams.Team) && (specParams.Brand == null || x.Brand == specParams.Brand) &&
                    (specParams.Competition == null || x.Competition == specParams.Competition) &&
                    (specParams.Player == null || x.PlayerName == specParams.Player))
        {

        }
    }
}

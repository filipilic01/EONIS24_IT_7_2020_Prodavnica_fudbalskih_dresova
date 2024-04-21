using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class DresWithFiltersForCountSpecification : BaseSpecification<Dres>
    {
        public DresWithFiltersForCountSpecification(DresSpecParams specParams)
             : base(x =>
             (specParams.Search == null || x.ImeIgraca.ToLower().Contains(specParams.Search) ||
             x.Tim.ToLower().Contains(specParams.Search) || x.Brend.ToLower().Contains(specParams.Search) ||
             x.Takmicenje.ToLower().Contains(specParams.Search)) &&
                (specParams.Tim == null || x.Tim == specParams.Tim) && (specParams.Brend == null || x.Brend == specParams.Brend) &&
                    (specParams.Takmicenje == null || x.Takmicenje == specParams.Takmicenje) &&
                    (specParams.Player == null || x.ImeIgraca == specParams.Player))
        {

        }
    }
}

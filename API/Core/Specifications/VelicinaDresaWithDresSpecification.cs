using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class VelicinaDresaWithDresSpecification: BaseSpecification<VelicinaDresa>
    {
        public VelicinaDresaWithDresSpecification()
        {
            AddInclude(x => x.Dres);
        }

        public VelicinaDresaWithDresSpecification(Guid id) : base(x => x.VelicinaDresaId == id)
        {
            AddInclude(x => x.Dres);
        }
    }
}

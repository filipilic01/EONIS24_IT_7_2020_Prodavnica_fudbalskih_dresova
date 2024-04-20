using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class PorudzbinaWithKupacSpecification : BaseSpecification<Porudzbina>
    {
        public PorudzbinaWithKupacSpecification()
        {
            AddInclude(x => x.Kupac);
        }

        public PorudzbinaWithKupacSpecification(Guid id) : base(x => x.PorudzbinaId == id)
        {
            AddInclude(x => x.Kupac);
        }
    }
}

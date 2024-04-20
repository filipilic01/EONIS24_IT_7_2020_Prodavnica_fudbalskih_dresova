using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class StavkaWithPorudzbinaAndVelicinaDresaSpecification:  BaseSpecification<StavkaPorudzbine>
    {
        public StavkaWithPorudzbinaAndVelicinaDresaSpecification()
        {
            AddInclude(x => x.Porudzbina);
            AddInclude(x => x.VelicinaDresa);
        }

        public StavkaWithPorudzbinaAndVelicinaDresaSpecification(Guid id) : base(x => x.StavkaPorudzbineId == id)
        {
            AddInclude(x => x.Porudzbina);
            AddInclude(x => x.VelicinaDresa);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace EntregasCorreio.Models
{
using Swashbuckle.AspNetCore.Filters;

public class FreteRequestExample : IExamplesProvider<FreteRequest>
{
    public FreteRequest GetExamples()
    {
        return new FreteRequest
        {
            CepOrigem = "28625720",
            CepDestino = "05271000",
            Peso = 450,
            Modalidade = "PAC"
        };
    }
}


}
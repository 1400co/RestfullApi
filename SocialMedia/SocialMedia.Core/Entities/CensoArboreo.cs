using System;
using System.Collections.Generic;

namespace SocialMedia.Core.Entities
{
    public class CensoArboreo : BaseEntity
    {        

        public decimal AlturaTotal { get; set; }
        public decimal AlturaComercial { get; set; }
        public decimal DiametroCopa { get; set; }
        public string FormaCopa { get; set; }

        public decimal DAP { get; set; }
        public decimal NumeroFustes { get; set; }
        public string NombreComun { get; set; }
        public string NombreCientifico { get; set; }

        public string Orden { get; set; }
        public string Familia { get; set; }
        public string Genero { get; set; }
        public string Especie { get; set; }
        public Coordenada CoordenadasGeograficasYPlanas { get; set; }
        public decimal AltitudASNM { get; set; }
        public string EstadoFitosanitario { get; set; }

        public string EstadoMadurez { get; set; }

        public string TipoIndividuo { get; set; }

        public IList<RegistroFotografico> Fotos { get; set; }
        public string Barrio { get; set; }
        public string Comuna { get; set; }
        public string TipoEmplazamiento { get; set; }

        public string InfraestructuraAfectada { get; set; }

        public bool ApendiceCites { get; set; }
        public bool CategoriaUicn { get; set; }
        public bool CategoriaMinisterioResolucion0192de2014 { get; set; }
        public bool EspecieEndemica { get; set; }
        public string Origen { get; set; }

        public string Observacion { get; set; }
        public DateTime Fecha { get; set; }

        public string RecomendacionesParaManejoSilvicultural { get; set; }

        public string Ninguna { get; set; }
    }

    
}

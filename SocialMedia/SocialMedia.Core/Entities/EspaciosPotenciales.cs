using System;
using System.Collections.Generic;

namespace SocialMedia.Core.Entities
{
    public class EspaciosPotenciales
    {
        public Guid Id { get; set; }
        public decimal AreasDisponibles { get; set; }
        public string TipoEmplazamiento { get; set; }

        public string EspeciesPotencialesParaSiembraAlMenosTresOpciones { get; set; }

        public decimal AlturasPotencialesArboles { get; set; }

        public decimal CaracteristicasEdaficasODelSuelo { get; set; }

        public bool TresBolillo { get; set; }
        public bool Cuadrado { get; set; }
        public bool Rectangular { get; set; }
        public bool IndividualOUno { get; set; }

        public decimal DistanciamientoSiembra { get; set; }

        public string InfraestructuraAfectada { get; set; }

        public decimal CoordenadasGeograficasYPlanas { get; set; }
        public string PendienteDelTerreno { get; set; }

        public IList<RegistroFotografico> Fotos { get; set; }
        public string Barrio { get; set; }
        public string Comuna { get; set; }
        public decimal AltitudASNM { get; set; }
    }
}

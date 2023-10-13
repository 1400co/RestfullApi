using System;

namespace SocialMedia.Core.Entities
{
    public class ActividadesSilviculturales
    {
        public Guid Id { get; set; }

        public virtual Guid IdCensoArboreo { get; set; }
        public virtual CensoArboreo CensoArboreo { get; set; }
        public bool PodaRealceR { get; set; }
        public bool PodaEstabilidadE { get; set; }
        public bool PodaMantenimientoM { get; set; }

        public bool CortesNuevos { get; set; }
        public bool CortesViejos { get; set; }
        public bool CortesEnfermos { get; set; }
        public bool PodaRaices { get; set; }
        public string EstructurasCercanasTipoEmplazamiento { get; set; }

        public string Limpieza { get; set; }

        public string PodaSanitaria { get; set; }

        public bool InmediataI { get; set; }
        public bool CortoPlazoC { get; set; }
        public bool LargoPlazoL { get; set; }
        public bool Trasplante { get; set; }
        public string Observacion { get; set; }
    }
}

using System;

namespace SocialMedia.Core.CustomEntities
{
    public class AuditLog
    {
        public int AuditLogId { get; set; }
        public string TableName { get; set; }
        public string ActionType { get; set; } // Ejemplo: "Add", "Update", "Delete"
        public DateTime Timestamp { get; set; }
        public string KeyValues { get; set; } // JSON con las claves primarias
        public string OldValues { get; set; } // JSON con los valores antiguos
        public string NewValues { get; set; } // JSON con los valores nuevos
    }
}

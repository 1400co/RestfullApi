using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Template.Core.CustomEntities
{
    public class AuditEntry
    {
        public EntityEntry Entry { get; }
        public string TableName { get; set; } = string.Empty;
        public string ActionType { get; set; } = string.Empty;
        public Dictionary<string, object?> KeyValues { get; } = [];
        public Dictionary<string, object?> OldValues { get; } = [];
        public Dictionary<string, object?> NewValues { get; } = [];
        public List<PropertyEntry> TempProperties { get; } = [];

        public bool HasTemporaryProperties => TempProperties.Any();

        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public AuditLog ToAudit()
        {
            var audit = new AuditLog
            {
                TableName = TableName,
                ActionType = ActionType,
                Timestamp = DateTime.UtcNow,
                KeyValues = JsonConvert.SerializeObject(KeyValues),
                OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues),
                NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues)
            };
            return audit;
        }
    }
}

using Edi.Practice.RequestResponseModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moonglade.Auditing
{
    public interface IMoongladeAudit
    {
        Task<Response> AddAuditEntry(EventType eventType, AuditEventId auditEventId, string message);

        Task<Response<(IReadOnlyList<AuditEntry> Entries, int Count)>> GetAuditEntries(
            int skip, int take, EventType? eventType = null, AuditEventId? eventId = null);

        Task<Response> ClearAuditLog();
    }
}
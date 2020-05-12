using Moonglade.Data.Entities;
using Moonglade.Data.Infrastructure;
using System;

namespace Moonglade.Data.Spec
{
    public class PingbackHistorySpec : BaseSpecification<PingbackHistoryEntity>
    {
        public PingbackHistorySpec(Guid id) : base(p => p.Id == id)
        {

        }
    }
}

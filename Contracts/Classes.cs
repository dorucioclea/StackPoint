using System;

namespace Contracts
{
    public class Contract
    {
        public long ContractId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Number { get; set; }
        public decimal? Price { get; set; }
        public long ConsumerId { get; set; }
        public long ContractorId { get; set; }
    }

    public class Organization
    {
        public long OrganizationId { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
    }

    public class OperationResult<T>
    {
        public T Data;
    }
}

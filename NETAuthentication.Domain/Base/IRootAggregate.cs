using System;
namespace NETAuthentication.Domain.Base
{
    /// <summary>
    /// Represents a default Root Aggregate
    /// </summary>
    public interface IRootAggregate
    {
        /// <summary>
        /// Identifies the primary key of the Root Aggregate
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Identifies if the Aggregate is valid or not
        /// </summary>
        bool IsValid { get; }
    }
}
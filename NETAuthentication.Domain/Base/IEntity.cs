using System;

namespace NETAuthentication.Domain.Base
{
    public interface IEntity
    {
        /// <summary>
        /// Identifies the primary key of the Entity
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Identifies if the Entity is valid or not
        /// </summary>
        bool IsValid { get; }        
    }
}
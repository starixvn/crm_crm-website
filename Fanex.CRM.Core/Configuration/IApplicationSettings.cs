// --------------------------------------------------------------------------
// <copyright file="IApplicationSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Configuration
{
    public interface IApplicationSettings
    {
        int NumberOfRecordsPerPage { get; }
    }
}

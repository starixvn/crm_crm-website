// --------------------------------------------------------------------------
// <copyright file="ICustomerInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Entities
{
    public interface ICustomerInfo
    {
        int Id { get; set; }

        string Username { get; set; }

        string Title { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string Phone { get; set; }

        string MobilePhone { get; set; }

        string Fax { get; set; }

        string Email { get; set; }

        Setting Country { get; set; }
    }
}

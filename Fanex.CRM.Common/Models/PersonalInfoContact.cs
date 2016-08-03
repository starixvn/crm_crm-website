// --------------------------------------------------------------------------
// <copyright file="PersonalInfoContact.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.11.18">Created</change>
// </history>

namespace Fanex.CRM.Common.Models
{
    using System;

    /// <summary>
    /// Personal info contact.
    /// </summary>
    [Serializable]
    public class PersonalInfoContact
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Phone))
            {
                return Name + Phone;    
            }

            return Name + " - " + Phone;
        }
    }
}

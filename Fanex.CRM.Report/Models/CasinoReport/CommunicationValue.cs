// --------------------------------------------------------------------------
// <copyright file="CommunicationValue.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.08.04">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.CasinoReport
{
    using System;

    /// <summary>
    /// Communication Value.
    /// </summary>
    [Serializable]
    public class CommunicationValue
    {
        public CommunicationValue(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public int Value { get; set; }
    }
}

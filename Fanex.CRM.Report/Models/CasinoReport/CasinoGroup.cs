// --------------------------------------------------------------------------
// <copyright file="CasinoGroup.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.07.31">Created</change>
// </history>

namespace Fanex.CRM.Report.Models.CasinoReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Resources;
    using Fanex.CRM.Report.Resources;

    /// <summary>
    /// Casino Group.
    /// </summary>
    [Serializable]
    public class CasinoGroup
    {
        public CasinoGroup(string casinoName)
        {
            Name = casinoName;
            Communications = new List<CommunicationValue>();

            var resourceMgmt = new ResourceManager(typeof(Labels));
            ResourceName = resourceMgmt.GetString(casinoName);
        }

        public string Name { get; set; }

        public string ResourceName { get; set; }

        public IList<CommunicationValue> Communications { get; set; }

        public int GrandTotal
        {
            get
            {
                return Communications.Sum(c => c.Value);
            }
        }
    }
}

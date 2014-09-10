// --------------------------------------------------------------------------
// <copyright file="RelatedCustomerAutoSuggestionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Infrastructure.Helper;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Related Customer Auto Suggestion Criteria.
    /// </summary>
    public class RelatedCustomerAutoSuggestionCriteria : CriteriaBase
    {
        [SpParamAttribute(SqlParamName = "username")]
        public string CustomerNamePharse { get; set; }

        public override string GetSettingKey()
        {
            return "AutoSuggestionCustomer";
        }
    }
}

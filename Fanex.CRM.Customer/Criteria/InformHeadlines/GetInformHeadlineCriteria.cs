// --------------------------------------------------------------------------
// <copyright file="GetInformHeadlineCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.09">Created</change>
// </history>

namespace Fanex.CRM.Customer.Criteria.InformHeadlines
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// GetInformHeadline criteria.
    /// </summary>
    public class GetInformHeadlineCriteria : CriteriaBase
    {
        private string _status;

        public int TitleId { get; set; }

        public string Status 
        { 
            get
            {
                if (_status == null)
                {
                    _status = string.Empty;
                }

                return _status;
            }

            set
            {
                _status = value;
            }
        }

        public int PageIndex { get; set; }

        public int ItemsPerPage { get; set; }

        public int TotalItem { get; set; }

        public override string GetSettingKey()
        {
            return "Customer_InformHeadline_GetList";
        }

        public override bool IsValid()
        {
            return TitleId > 0;
        }
    }
}

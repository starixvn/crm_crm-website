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
    using Fanex.Data.Repository;

    /// <summary>
    /// GetInformHeadline criteria.
    /// </summary>
    public class GetInformHeadlineCriteria : CriteriaBase
    {
        private string _status;

        private string _position;

        private string _site;

        private string _currencyId;

        public int TitleId { get; set; }

        public string Username { get; set; }

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

        public string Position
        {
            get
            {
                if (_position == null)
                {
                    _position = string.Empty;
                }

                return _position;
            }

            set
            {
                _position = value;
            }
        }

        public string Site
        {
            get
            {
                if (_site == null)
                {
                    _site = string.Empty;
                }

                return _site;
            }

            set
            {
                _site = value;
            }
        }

        public string CurrencyId
        {
            get
            {
                if (_currencyId == null)
                {
                    _currencyId = string.Empty;
                }

                return _currencyId;
            }

            set
            {
                _currencyId = value;
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

// --------------------------------------------------------------------------
// <copyright file="InformHeadlineRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.05.09">Created</change>
// </history>

namespace Fanex.CRM.Customer.Repositories.Implements
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Customer.Criteria.InformHeadlines;
    using Fanex.CRM.Customer.Models.InformHeadlines;
    using Fanex.CRM.Customer.Repositories.Interfaces;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// InformHeadline Repository.
    /// </summary>
    public class InformHeadlineRepository : IInformHeadlineRepository
    {
        /// <summary>
        /// Gets the inform headline.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The list of InformHeadlineStatus object.
        /// </returns>
        public IList<InformHeadlineStatus> GetInformStatistic(ICriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    IList<InformHeadlineStatus> informs = new List<InformHeadlineStatus>();
                    while (reader.Read())
                    {
                        informs.Add(ParseInformHeadlineStatus(reader));   
                    }

                    return informs;
                }
            }
        }

        public void CreateTitle(ICriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                int effectedRows = db.ExecuteNonQuery(criteria.GetSpParams());
            }
        }

        /// <summary>
        /// Gets the informs.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of Inform Headline.</returns>
        public IList<InformHeadline> GetInforms(GetInformHeadlineCriteria criteria)
        {
            IList<InformHeadline> informs = new List<InformHeadline>();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                var rawdata = db.Query<InformHeadline>(criteria.GetSpParams());
                foreach (var item in rawdata)
                {
                    item.Remark = HtmlHelper.DecodeXml(item.Remark);
                    informs.Add(item);
                }
                
                var outputParam = db.GetOuputParams();
                criteria.TotalItem = outputParam == null ? 0 : Converter.ToInt(outputParam["totalitem"]);
                return informs;
            }
        }

        public Result UpdateInforms(ICriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                int effectedRows = db.ExecuteNonQuery(criteria.GetSpParams());
                return new Result { Code = 0, Message = string.Empty };
            }   
        }

        /// <summary>
        /// Parses the inform headline status.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The InformHeadlineStatus object.</returns>
        private InformHeadlineStatus ParseInformHeadlineStatus(IDataReader reader)
        {
            return new InformHeadlineStatus
            {
                TitleId = Converter.ToInt(reader["titleid"]),
                Title = Converter.ToString2(reader["title"]),
                CreatedTime = Converter.ToDateTime(reader["createdtime"]),
                Total = Converter.ToInt(reader["total"]),
                Done = Converter.ToInt(reader["done"]),
                RequestToCallback = Converter.ToInt(reader["request_to_call_back"]),
                NoNeedToCall = Converter.ToInt(reader["no_need_to_call"]),
                NoReply = Converter.ToInt(reader["no_reply"]),
                Uncontactable = Converter.ToInt(reader["uncontactable"]),
                InternalAccount = Converter.ToInt(reader["internal_account"]),
                NeverTouch = Converter.ToInt(reader["never_touch"]),
            };
        }
    }
}

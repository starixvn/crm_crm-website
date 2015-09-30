// --------------------------------------------------------------------------
// <copyright file="FeedbackSubjectManagementRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Repositories.FeedbackSubjectManagement
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Models.FeedbackSubjectManagement;
    using Fanex.CRM.Common.Repositories;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;

    /// <summary>
    /// Feedback Subject Management Repository.
    /// </summary>
    public class FeedbackSubjectManagementRepository : BaseRepository, IFeedbackSubjectManagementRepository
    {
        public IEnumerable<FeedbackSubjectTemplate> SearchFeedbackSubjectTemplate(SearchFeedbackSubjectTemplateCriteria criteria)
        {
            var result = new List<FeedbackSubjectTemplate>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                var reader = objectDB.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new FeedbackSubjectTemplate
                    {
                        Id = Converter.ToInt(reader["templateID"]),
                        TemplateContent = Converter.ToString2(reader["subject"])
                    });
                }
            }

            return result.Where(template => IsCorrectTemplate(template, criteria)).ToList();
        }

        public Result DeleteTemplate(DeleteFeedbackSubjectTemplateCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        /// <summary>
        /// Gets the feedback subject template by identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The feedback subject template by identifier.</returns>
        public FeedbackSubjectTemplate GetFeedbackSubjectTemplate(FeedbackSubjectTemplateByIdCriteria criteria)
        {
            var result = new List<FeedbackSubjectTemplate>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var reader = objectDB.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new FeedbackSubjectTemplate
                    {
                        Id = Converter.ToInt(reader["templateID"]),
                        TemplateContent = Converter.ToString2(reader["subject"])
                    });
                }
            }

            return result.FirstOrDefault();
        }

        public Result EditTemplate(EditFeedbackSubjectTemplateCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public Result InsertTemplate(InsertFeedbackSubjectTemplateCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        private bool IsCorrectTemplate(FeedbackSubjectTemplate template, SearchFeedbackSubjectTemplateCriteria criteria)
        {
            return string.IsNullOrEmpty(criteria.Subject) ||
                 template.TemplateContent.ToLower().Contains(criteria.Subject.Trim().ToLower());
        }
    }
}

// --------------------------------------------------------------------------
// <copyright file="FeedbackSubjectManagementRepository.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.06.03">Created</change>
// </history>
// --------------------------------------------------------------------------

namespace Fanex.CRM.AdminTool.Repositories.FeedbackSubjectManagement
{
    using System.Collections.Generic;
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
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var result = new List<FeedbackSubjectTemplate>();
                    while (reader.Read())
                    {
                        var templateContent = Converter.ToString2(reader["subject"]);
                        if (IsCorrectTemplate(templateContent, criteria.Subject))
                        {
                            result.Add(new FeedbackSubjectTemplate
                            {
                                Id = Converter.ToInt(reader["templateID"]),
                                TemplateContent = templateContent,
                                IsActive = Converter.ToBool(reader["isactive"])
                            });
                        }
                    }

                    return result;
                }
            }
        }

        public Result ChangeTemplateState(ChangeSubjectTemplateStateCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public FeedbackSubjectTemplate GetFeedbackSubjectTemplate(FeedbackSubjectTemplateByIdCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        return new FeedbackSubjectTemplate
                        {
                            Id = Converter.ToInt(reader["templateID"]),
                            TemplateContent = Converter.ToString2(reader["subject"])
                        };
                    }

                    return null;
                }
            }
        }

        public Result EditTemplate(EditFeedbackSubjectTemplateCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public Result InsertTemplate(InsertFeedbackSubjectTemplateCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        private bool IsCorrectTemplate(string templateContent, string subject)
        {
            return string.IsNullOrEmpty(subject) ||
                 templateContent.ToLower().Contains(subject.Trim().ToLower());
        }
    }
}

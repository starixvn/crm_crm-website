namespace Fanex.CRM.AdminTool.Repositories.FeedbackSubjectManagement
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.CRM.AdminTool.Criteria.FeedbackSubjectManagement;
    using Fanex.CRM.AdminTool.Models.FeedbackSubjectManagement;
    using Fanex.Data;

    public class FeedbackSubjectManagementRepository : IFeedbackSubjectManagementRepository
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

        private static bool IsCorrectTemplate(string templateContent, string subject)
        {
            return string.IsNullOrEmpty(subject) ||
                 templateContent.ToLower().Contains(subject.Trim().ToLower());
        }
    }
}
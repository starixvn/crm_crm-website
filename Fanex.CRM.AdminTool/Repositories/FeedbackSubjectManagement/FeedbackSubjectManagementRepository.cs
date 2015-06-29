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
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;

    /// <summary>
    /// Feedback Subject Management Repository.
    /// </summary>
    public class FeedbackSubjectManagementRepository : IFeedbackSubjectManagementRepository
    {
        /// <summary>
        /// Searches the feedback subject template.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The feedback subject templates.</returns>
        public List<FeedbackSubjectTemplateInfo> SearchFeedbackSubjectTemplate(SearchFeedbackSubjectTemplateCriteria criteria)
        {
            List<FeedbackSubjectTemplateInfo> result = new List<FeedbackSubjectTemplateInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                var reader = objectDB.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new FeedbackSubjectTemplateInfo
                    {
                        Id = Converter.ToInt(reader["templateID"]),
                        TemplateContent = Converter.ToString2(reader["subject"])
                    });
                }
            }

            return result.Where(template => IsCorrectTemplate(template, criteria)).ToList();
        }

        /// <summary>
        /// Deletes the template.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Delete result.</returns>
        public Result DeleteTemplate(DeleteFeedbackSubjectTemplateCriteria criteria)
        {
            Result deleteResult = null;
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();
                deleteResult = new Result
                {
                    Code = Converter.ToInt(outputParams["errcode"]),
                    Message = Converter.ToString2(outputParams["errmsg"])
                };
            }

            return deleteResult;
        }

        /// <summary>
        /// Gets the feedback subject template by identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The feedback subject template by identifier.</returns>
        public FeedbackSubjectTemplateInfo FeedbackSubjectTemplateById(FeedbackSubjectTemplateByIdCriteria criteria)
        {
            List<FeedbackSubjectTemplateInfo> result = new List<FeedbackSubjectTemplateInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var reader = objectDB.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(new FeedbackSubjectTemplateInfo
                    {
                        Id = Converter.ToInt(reader["templateID"]),
                        TemplateContent = Converter.ToString2(reader["subject"])
                    });
                }
            }

            return result.FirstOrDefault();
        }

        /// <summary>
        /// Edits the template.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Edit result.</returns>
        public Result EditTemplate(EditFeedbackSubjectTemplateCriteria criteria)
        {
            Result editResult = null;
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();
                editResult = new Result
                {
                    Code = Converter.ToInt(outputParams["errcode"]),
                    Message = Converter.ToString2(outputParams["errmsg"])
                };
            }

            return editResult;
        }

        /// <summary>
        /// Inserts the template.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Insert result.</returns>
        public Result InsertTemplate(InsertFeedbackSubjectTemplateCriteria criteria)
        {
            Result insertResult = null;
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                objectDB.ExecuteNonQuery();
                var outputParams = objectDB.GetOuputParams();
                insertResult = new Result
                {
                    Code = Converter.ToInt(outputParams["errcode"]),
                    Message = Converter.ToString2(outputParams["errmsg"])
                };
            }

            return insertResult;
        }

        private bool IsCorrectTemplate(FeedbackSubjectTemplateInfo template, SearchFeedbackSubjectTemplateCriteria criteria)
        {
            return string.IsNullOrEmpty(criteria.Subject) ||
                 template.TemplateContent.ToLower().Contains(criteria.Subject.Trim().ToLower());
        }
    }
}

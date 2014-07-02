// --------------------------------------------------------------------------
// <copyright file="SettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Repositories.Implements
{
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Extension;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Repositories.Interfaces;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Setting Repository.
    /// </summary>
    public class SettingRepository : ISettingRepository
    {
        /// <summary>
        /// Gets all setting.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>All setting.</returns>
        public List<SettingInfo> GetAllSetting(GetAllSettingCriteria criteria)
        {
            var allsetting = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            allsetting.Add(new SettingInfo()
                            {
                                Id = reader.GetInt32NotNull("SettingId"),
                                Type = reader.GetStringNotNull("SettingName"),
                                Value = reader.GetStringNotNull("SettingValue"),
                                DisplayOrder = reader.GetInt32NotNull("DisplayOrder")
                            });
                        }
                    }
                }
            }

            return allsetting;
        }

        /// <summary>
        /// Gets the setting relation by name.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The setting relation by name.</returns>
        public List<SettingInfo> GetSettingRelationByName(GetSettingRelationByNameCriteria criteria)
        {
            var allSetting = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var setting = new SettingInfo()
                            {
                                Id = reader.GetInt32NotNull("SettingId"),
                                Value = reader.GetStringNotNull("SettingValue"),
                                IsSubSetting = reader.GetInt32NotNull("IsSub") == 1,
                                DisplayOrder = reader.GetInt32NotNull("DisplayOrder")
                            };

                            allSetting.Add(setting);
                        }
                    }
                }
            }

            return allSetting;
        }

        /// <summary>
        /// Gets all country.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>All country.</returns>
        public List<CountryInfo> GetAllCountry(GetAllCountryCriteria criteria)
        {
            var allSetting = new List<CountryInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var setting = new CountryInfo()
                            {
                                Id = reader.GetInt32NotNull("CountryID"),
                                Name = reader.GetStringNotNull("CountryName"),
                                CountryCode = reader.GetInt16NotNull("CountryCode"),
                                DisplayOrder = reader.GetInt16NotNull("DisplayOrder"),
                                IsActive = reader.GetBooleanNotNull("isActive")
                            };

                            allSetting.Add(setting);
                        }
                    }
                }
            }

            return allSetting;
        }

        /// <summary>
        /// Gets all setting relation.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>All setting relation.</returns>
        public List<SettingInfo> GetAllSettingRelation(GetAllSettingRelationCriteria criteria)
        {
            var settingRelations = new List<SettingInfo>();
            using (var obj = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var settingInfo = new SettingInfo()
                            {
                                Id = reader.GetInt32NotNull("SettingID"),
                                ParentId = reader.GetInt32NotNull("ParentId"),
                                DisplayOrder = reader.GetInt32NotNull("DisplayOrder")
                            };

                            settingRelations.Add(settingInfo);
                        }
                    }
                }
            }

            return settingRelations;
        }

        /// <summary>
        /// Gets all language.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>All language.</returns>
        public List<SettingInfo> GetAllLanguage(GetAllLanguageCriteria criteria)
        {
            var allLanguage = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var setting = new SettingInfo()
                            {
                                Id = reader.GetInt32NotNull("LanguageId"),
                                Name = reader.GetStringNotNull("Name"),
                                DisplayOrder = reader.GetInt32NotNull("DisplayOrder"),
                                IsActive = reader.GetBooleanNotNull("IsActive")
                            };

                            allLanguage.Add(setting);
                        }
                    }
                }
            }

            return allLanguage;
        }

        /// <summary>
        /// Gets all communication tool.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>All communication tool.</returns>
        public List<SettingInfo> GetAllCommunicationTool(GetAllCommunicationToolCriteria criteria)
        {
            var allCommunicationTool = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var setting = new SettingInfo()
                            {
                                Id = reader.GetInt32NotNull("CommunicationId"),
                                Name = reader.GetStringNotNull("Name"),
                                DisplayOrder = reader.GetInt32NotNull("DisplayOrder"),
                                IsActive = reader.GetBooleanNotNull("IsActive")
                            };

                            allCommunicationTool.Add(setting);
                        }
                    }
                }
            }

            return allCommunicationTool;
        }

        /// <summary>
        /// Gets all main category.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>All main category.</returns>
        public List<SettingInfo> GetAllMainCategory(GetAllMainCategoryCriteria criteria)
        {
            var allMainCategory = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var setting = new SettingInfo()
                            {
                                Id = reader.GetInt32NotNull("MainCategoryId"),
                                Name = reader.GetStringNotNull("Name"),
                                DisplayOrder = reader.GetInt32NotNull("DisplayOrder"),
                                IsActive = reader.GetBooleanNotNull("IsActive")
                            };

                            allMainCategory.Add(setting);
                        }
                    }
                }
            }

            return allMainCategory;
        }

        /// <summary>
        /// Gets all sub category.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>All sub category.</returns>
        public List<SettingInfo> GetAllSubCategory(GetAllSubCategoryCriteria criteria)
        {
            var allSubCategory = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var setting = new SettingInfo()
                            {
                                Id = reader.GetInt32NotNull("SubCategoryId"),
                                Name = reader.GetStringNotNull("Name"),
                                DisplayOrder = reader.GetInt32NotNull("DisplayOrder"),
                                IsActive = reader.GetBooleanNotNull("IsActive")
                            };

                            allSubCategory.Add(setting);
                        }
                    }
                }
            }

            return allSubCategory;
        }

        /// <summary>
        /// Gets all status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>All sub Status.</returns>
        public List<SettingInfo> GetAllStatus(GetAllStatusCriteria criteria)
        {
            var allStatus = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var setting = new SettingInfo()
                            {
                                Id = reader.GetInt32NotNull("StatusId"),
                                Name = reader.GetStringNotNull("Name")
                            };

                            allStatus.Add(setting);
                        }
                    }
                }
            }

            return allStatus;
        }

        /// <summary>
        /// Gets the feedback subject template by identifier.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The feedback subject template by identifier.</returns>
        public List<FeedbackTemplate> GetFeedbackSubjectTemplateById(GetFeedbackSubjectTemplateByIdCriteria criteria)
        {
            List<FeedbackTemplate> feedbackSubjectTemplate = new List<FeedbackTemplate>();
            using (var obj = new ObjectDb(criteria.GetSettingKey(), 500))
            {
                obj.AssignParamValues(criteria.GetSpParams());
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var feedbackTemplate = new FeedbackTemplate()
                            {
                                TemplateId = reader.GetInt32NotNull("templateID"),
                                Subject = reader.GetStringNotNull("subject")
                            };

                            feedbackSubjectTemplate.Add(feedbackTemplate);
                        }
                    }
                }
            }

            return feedbackSubjectTemplate;
        }

        /// <summary>
        /// Gets all log actions.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The log actions list.</returns>
        public List<SettingInfo> GetAllLogAction(ICriteria criteria)
        {
            var actions = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            actions.Add(new SettingInfo()
                            {
                                Id = reader.GetInt32NotNull("ActionID"),
                                Name = reader.GetStringNotNull("ActionName")
                            });
                        }
                    }
                }
            }

            return actions;
        }

        /// <summary>
        /// Gets all status inform headline.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The SettingInfo list.</returns>
        public List<SettingInfo> GetAllStatusInformHeadline(GetAllStatusInformHeadlineCriteria criteria)
        {
            var status = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var setting = new SettingInfo
                            {
                                Id = reader.GetInt32NotNull("InformStatusId"),
                                Name = reader.GetStringNotNull("Name")
                            };

                            status.Add(setting);
                        }
                    }
                }
            }

            return status;
        }

        /// <summary>
        /// Gets all bet type.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>All bet type.</returns>
        public List<SettingInfo> GetAllBetType(GetAllBetTypeCriteria criteria)
        {
            var allBetType = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var setting = new SettingInfo()
                            {
                                Id = reader.GetInt32NotNull("BetTypeId"),
                                Name = reader.GetStringNotNull("Name"),
                                DisplayOrder = reader.GetInt32NotNull("DisplayOrder"),
                                IsActive = reader.GetBooleanNotNull("IsActive")
                            };

                            allBetType.Add(setting);
                        }
                    }
                }
            }

            return allBetType;
        }
    }
}

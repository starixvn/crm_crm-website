// --------------------------------------------------------------------------
// <copyright file="SettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Setting.Repositories
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Repositories;
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// Setting Repository.
    /// </summary>
    public class SettingRepository : ISettingRepository
    {
        public IEnumerable<SettingInfo> GetAllSettings(GetAllSettingCriteria criteria)
        {
            var settings = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        settings.Add(new SettingInfo
                        {
                            Id = Converter.ToInt(reader["settingid"], 0),
                            SettingType = Converter.ToString2(reader["settingname"]),
                            Value = Converter.ToString2(reader["settingvalue"]),
                            DisplayOrder = Converter.ToInt(reader["displayorder"], 0)
                        });
                    }
                }
            }

            return settings;
        }

        public IEnumerable<SettingInfo> GetSettingRelations(GetSettingRelationByNameCriteria criteria)
        {
            var settings = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var setting = new SettingInfo()
                        {
                            Id = Converter.ToInt(reader["SettingId"]),
                            Value = Converter.ToString2(reader["SettingValue"]),
                            IsSubSetting = Converter.ToInt(reader["IsSub"]) == 1,
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"])
                        };

                        settings.Add(setting);
                    }
                }
            }

            return settings;
        }

        public IEnumerable<CountryInfo> GetAllCountries(GetAllCountryCriteria criteria)
        {
            var settings = new List<CountryInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var setting = new CountryInfo()
                        {
                            Id = Converter.ToInt(reader["CountryID"]),
                            Name = Converter.ToString2(reader["CountryName"]),
                            CountryCode = Converter.ToInt(reader["CountryCode"]),
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"]),
                            IsActive = Converter.ToBool(reader["isActive"])
                        };

                        settings.Add(setting);
                    }
                }
            }

            return settings;
        }

        /// <summary>
        /// Gets all setting relation.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>All setting relation.</returns>
        public IEnumerable<SettingInfo> GetAllSettingRelation(GetAllSettingRelationCriteria criteria)
        {
            var settingRelations = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var settingInfo = new SettingInfo()
                        {
                            Id = Converter.ToInt(reader["SettingID"]),
                            ParentId = Converter.ToInt(reader["ParentId"]),
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"])
                        };

                        settingRelations.Add(settingInfo);
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
        public IEnumerable<SettingInfo> GetAllLanguage(GetAllLanguageCriteria criteria)
        {
            var allLanguage = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var setting = new SettingInfo()
                        {
                            Id = Converter.ToInt(reader["LanguageId"]),
                            Name = Converter.ToString2(reader["Name"]),
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"]),
                            IsActive = Converter.ToBool(reader["IsActive"])
                        };

                        allLanguage.Add(setting);
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
        public IEnumerable<SettingInfo> GetAllCommunicationTool(GetAllCommunicationToolCriteria criteria)
        {
            var allCommunicationTool = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var setting = new SettingInfo()
                        {
                            Id = Converter.ToInt(reader["CommunicationId"]),
                            Name = Converter.ToString2(reader["Name"]),
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"]),
                            IsActive = Converter.ToBool(reader["IsActive"])
                        };

                        allCommunicationTool.Add(setting);
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
        public IEnumerable<SettingInfo> GetAllMainCategory(GetAllMainCategoryCriteria criteria)
        {
            var allMainCategory = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var setting = new SettingInfo()
                        {
                            Id = Converter.ToInt(reader["MainCategoryId"]),
                            Name = Converter.ToString2(reader["Name"]),
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"]),
                            IsActive = Converter.ToBool(reader["IsActive"])
                        };

                        allMainCategory.Add(setting);
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
        public IEnumerable<SettingInfo> GetAllSubCategory(GetAllSubCategoryCriteria criteria)
        {
            var allSubCategory = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var setting = new SettingInfo()
                        {
                            Id = Converter.ToInt(reader["SubCategoryId"]),
                            Name = Converter.ToString2(reader["Name"]),
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"]),
                            IsActive = Converter.ToBool(reader["IsActive"])
                        };

                        allSubCategory.Add(setting);
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
        public IEnumerable<SettingInfo> GetAllStatus(GetAllStatusCriteria criteria)
        {
            var allStatus = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        var setting = new SettingInfo()
                        {
                            Id = Converter.ToInt(reader["StatusId"]),
                            Name = Converter.ToString2(reader["Name"])
                        };

                        allStatus.Add(setting);
                    }
                }
            }

            return allStatus;
        }

        public IEnumerable<Status> GetStatuses(GetAllStatusCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<Status>(criteria.GetSpParams());
            }
        }

        public IEnumerable<FeedbackTemplate> GetFeedbackSubjectTemplateById(GetFeedbackSubjectTemplateByIdCriteria criteria)
        {
            using (var obj = new ObjectDb(criteria.GetSettingKey(), 500))
            {
                return obj.Query<FeedbackTemplate>(criteria.GetSpParams());
            }
        }

        public IEnumerable<BaseModel> GetAllLogActions(GetActionCriteria criteria)
        {
            var actions = new List<BaseModel>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        actions.Add(new BaseModel()
                        {
                            Id = Converter.ToInt(reader["ActionID"]),
                            Name = Converter.ToString2(reader["ActionName"])
                        });
                    }
                }
            }

            return actions;
        }

        /// <summary>
        /// Gets all status inform headline.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The BaseModel list.</returns>
        public IEnumerable<BaseModel> GetAllStatusInformHeadline(GetAllStatusInformHeadlineCriteria criteria)
        {
            var status = new List<BaseModel>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var setting = new BaseModel
                        {
                            Id = Converter.ToInt(reader["InformStatusId"]),
                            Name = Converter.ToString2(reader["Name"])
                        };

                        status.Add(setting);
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
        public IEnumerable<SettingInfo> GetAllBetType(GetAllBetTypeCriteria criteria)
        {
            var allBetType = new List<SettingInfo>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var setting = new SettingInfo()
                        {
                            Id = Converter.ToInt(reader["BetTypeId"]),
                            Name = Converter.ToString2(reader["Name"]),
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"]),
                            IsActive = Converter.ToBool(reader["IsActive"])
                        };

                        allBetType.Add(setting);
                    }
                }
            }

            return allBetType;
        }

        public IEnumerable<BaseModel> GetAllCurrencyRedemptions(GetAllCurrencyRedemptionCriteria criteria)
        {
            var allCurrency = new List<BaseModel>();
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var setting = new BaseModel()
                        {
                            Id = Converter.ToInt(reader["CurrencyId"]),
                            Name = Converter.ToString2(reader["Currency"])
                        };

                        allCurrency.Add(setting);
                    }
                }
            }

            return allCurrency;
        }

        /// <summary>
        /// Gets the type of all licensee.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The Licensee Type list.
        /// </returns>
        public IEnumerable<BaseModel> GetAllLicenseeTypes(GetAllLicenseeTypeCriteria criteria)
        {
            var licenseeTypes = new List<BaseModel>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        licenseeTypes.Add(new BaseModel
                        {
                            Id = Converter.ToInt(reader["LicenseeType"]),
                            Name = Converter.ToString2(reader["Licensee"])
                        });
                    }
                }
            }

            return licenseeTypes;
        }

        public IEnumerable<BaseModel> GetAllCurrencies(GetAllCurrencyCriteria criteria)
        {
            var currencies = new List<BaseModel>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        currencies.Add(new BaseModel
                        {
                            Id = Converter.ToInt(reader["CurrencyId"]),
                            Name = Converter.ToString2(reader["Currency"])
                        });
                    }
                }
            }

            return currencies;
        }

        public IEnumerable<Station> GetStations(GetStationsCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<Station>(criteria.GetSpParams());
            }
        }
    }
}

namespace Fanex.CRM.Setting.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Enumerations;
    using Fanex.Common;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Setting.Criteria;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class SettingRepository : ISettingRepository
    {
        public List<Setting> GetAllSettings(GetAllSettingCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var settings = new List<Setting>();
                    while (reader.Read())
                    {
                        settings.Add(new Setting
                        {
                            Id = Converter.ToInt(reader["settingid"]),
                            SettingType = Converter.ToString2(reader["settingname"]),
                            Value = Converter.ToString2(reader["settingvalue"]),
                            DisplayOrder = Converter.ToInt(reader["displayorder"]),
                            IsActive = Converter.ToBool(reader["isactive"])
                        });
                    }

                    return settings;
                }
            }
        }

        public IList<T> GetSettingRelations<T>(GetSettingRelationByNameCriteria criteria) where T : SettingRelation, new()
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var settings = new List<T>();
                    while (reader.Read())
                    {
                        var setting = new T
                        {
                            Id = Converter.ToInt(reader["SettingId"]),
                            Value = Converter.ToString2(reader["SettingValue"]),
                            IsChild = Converter.ToInt(reader["IsSub"]) == 1,
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"])
                        };

                        settings.Add(setting);
                    }

                    return settings;
                }
            }
        }

        public IList<Country> GetAllCountries(GetAllCountryCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var settings = new List<Country>();
                    while (reader.Read())
                    {
                        var setting = new Country
                        {
                            Id = Converter.ToInt(reader["CountryID"]),
                            Name = Converter.ToString2(reader["CountryName"]),
                            CountryCode = Converter.ToInt(reader["CountryCode"]),
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"]),
                            IsActive = Converter.ToBool(reader["isActive"])
                        };

                        settings.Add(setting);
                    }

                    return settings;
                }
            }
        }

        public IList<Province> GetAllProvinces(GetAllProvinceCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var settings = new List<Province>();
                    while (reader.Read())
                    {
                        var setting = new Province
                        {
                            Id = Converter.ToInt(reader["ProvinceID"]),
                            Name = Converter.ToString2(reader["ProvinceName"]),
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"]),
                            CountryId = Converter.ToInt(reader["CountryID"])
                        };

                        settings.Add(setting);
                    }

                    return settings;
                }
            }
        }

        public IEnumerable<Setting> GetAllSettingRelations(GetAllSettingRelationCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader())
                {
                    var settingRelations = new List<Setting>();
                    while (reader.Read())
                    {
                        var settingInfo = new Setting
                        {
                            Id = Converter.ToInt(reader["SettingID"]),
                            ParentId = Converter.ToInt(reader["ParentId"]),
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"])
                        };

                        settingRelations.Add(settingInfo);
                    }

                    return settingRelations;
                }
            }
        }

        public IList<Setting> GetAllLanguages(GetAllLanguageCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var allLanguage = new List<Setting>();
                    while (reader.Read())
                    {
                        var setting = new Setting
                        {
                            Id = Converter.ToInt(reader["LanguageId"]),
                            Name = Converter.ToString2(reader["Name"]),
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"]),
                            IsActive = Converter.ToBool(reader["IsActive"])
                        };

                        allLanguage.Add(setting);
                    }

                    return allLanguage;
                }
            }
        }

        public IList<Setting> GetAllCommunicationTools(GetAllCommunicationToolCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var allCommunicationTool = new List<Setting>();
                    while (reader.Read())
                    {
                        var setting = new Setting
                        {
                            Id = Converter.ToInt(reader["CommunicationId"]),
                            Name = Converter.ToString2(reader["Name"]),
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"]),
                            IsActive = Converter.ToBool(reader["IsActive"])
                        };

                        allCommunicationTool.Add(setting);
                    }

                    return allCommunicationTool;
                }
            }
        }

        public IList<Setting> GetAllMainCategory(GetAllMainCategoryCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var allMainCategory = new List<Setting>();
                    while (reader.Read())
                    {
                        var setting = new Setting
                        {
                            Id = Converter.ToInt(reader["MainCategoryId"]),
                            Name = Converter.ToString2(reader["Name"]),
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"]),
                            IsActive = Converter.ToBool(reader["IsActive"])
                        };

                        allMainCategory.Add(setting);
                    }

                    return allMainCategory;
                }
            }
        }

        public IList<Setting> GetAllSubCategory(GetAllSubCategoryCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var allSubCategory = new List<Setting>();
                    while (reader.Read())
                    {
                        var setting = new Setting
                        {
                            ParentId = Converter.ToInt(reader["MainCategoryID"]),
                            Id = Converter.ToInt(reader["SubCategoryId"]),
                            Name = Converter.ToString2(reader["Name"]),
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"]),
                            IsActive = Converter.ToBool(reader["IsActive"])
                        };

                        allSubCategory.Add(setting);
                    }

                    return allSubCategory;
                }
            }
        }

        public IList<BaseModel> GetAllStatus(GetAllStatusCriteria criteria)
        {
            var data = ReadBaseModels(criteria, "StatusId", "Name");
            var closedStatus = data.FirstOrDefault(d => d.Id == (int)CRMStatus.Closed);
            if (closedStatus != null)
            {
                data.Remove(closedStatus);
                data.Insert(0, closedStatus);
            }

            return data;
        }

        public IList<BaseModel> GetAllLogActions(GetActionCriteria criteria)
        {
            return ReadBaseModels(criteria, "ActionID", "ActionName");
        }

        public IEnumerable<BaseModel> GetAllStatusInformHeadline(GetAllStatusInformHeadlineCriteria criteria)
        {
            return ReadBaseModels(criteria, "InformStatusId", "Name");
        }

        public IEnumerable<Product> GetAllBetType(GetAllBetTypeCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSpParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    var allBetType = new List<Product>();
                    while (reader.Read())
                    {
                        var setting = new Product
                        {
                            Id = Converter.ToInt(reader["BetTypeId"]),
                            Name = Converter.ToString2(reader["Name"]),
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"]),
                            IsActive = Converter.ToBool(reader["IsActive"]),
                            ParentId = Converter.ToInt(reader["ProductID"])
                        };

                        allBetType.Add(setting);
                    }

                    return allBetType;
                }
            }
        }

        public IEnumerable<Product> GetSubProducts(SubProductCriteria criteria)
        {
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDB.ExecuteReader(criteria.GetSpParams()))
                {
                    var subProducts = new List<Product>();
                    while (reader.Read())
                    {
                        subProducts.Add(new Product
                        {
                            Id = Converter.ToInt(reader["SubProductID"]),
                            Value = Converter.ToString2(reader["Name"]),
                            DisplayOrder = Converter.ToInt(reader["DisplayOrder"]),
                            IsActive = Converter.ToBool(reader["IsActive"]),
                            ParentId = Converter.ToInt(reader["ProductID"])
                        });
                    }

                    return subProducts;
                }
            }
        }

        public IList<BaseModel> GetAllCurrencyRedemptions(GetAllCurrencyRedemptionCriteria criteria)
        {
            return ReadBaseModels(criteria, "CurrencyId", "Currency");
        }

        public IList<BaseModel> GetAllLicenseeTypes(GetAllLicenseeTypeCriteria criteria)
        {
            return ReadBaseModels(criteria, "LicenseeType", "Licensee");
        }

        public IList<BaseModel> GetAllCurrencies(GetAllCurrencyCriteria criteria)
        {
            return ReadBaseModels(criteria, "CurrencyId", "Currency");
        }

        private static IList<BaseModel> ReadBaseModels(ICriteria criteria, string keyColumn, string nameColumn, bool hasParams = true)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(hasParams ? criteria.GetSpParams() : null))
                {
                    var list = new List<BaseModel>();
                    while (reader.Read())
                    {
                        list.Add(new BaseModel
                        {
                            Id = Converter.ToInt(reader[keyColumn]),
                            Name = Converter.ToString2(reader[nameColumn])
                        });
                    }

                    return list;
                }
            }
        }
    }
}
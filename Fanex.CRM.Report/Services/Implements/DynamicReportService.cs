// --------------------------------------------------------------------------
// <copyright file="DynamicReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Report.Services.Implements
{
    using System;
    using System.Collections.Generic;
    using Fanex.CRM.Infrastructure.Logging;
    using Fanex.CRM.Report.Resources;
    using Fanex.CRM.Report.Services.Interfaces;
    using Fanex.CRM.Setting.Constants;
    using Fanex.CRM.Setting.Models;
    using Fanex.CRM.Setting.Services.Implements;
    using Fanex.CRM.Setting.Services.Interfaces;

    /// <summary>
    /// Dynamic Report Service.
    /// </summary>
    public class DynamicReportService : IDynamicReportService
    {
        private ISettingService _serviceSetting;

        public DynamicReportService()
            : this(null)
        {
        }

        public DynamicReportService(
            ISettingService serviceSetting)
        {
            _serviceSetting = serviceSetting ?? new SettingService();
        }

        /// <summary>
        /// Gets all period type.
        /// </summary>
        /// <returns>All period type.</returns>
        public List<SettingInfo> GetAllPeriodType()
        {
            var allPeriodType = new List<SettingInfo>();
            try
            {
                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Daily,
                    Name = Labels.Daily
                });
                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Weekly,
                    Name = Labels.Weekly
                });
                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Monthly,
                    Name = Labels.Monthly
                });
                allPeriodType.Add(new SettingInfo
                {
                    Id = PeriodTypeConstants.Yearly,
                    Name = Labels.Yearly
                });
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allPeriodType;
        }

        /// <summary>
        /// Gets all product.
        /// </summary>
        /// <returns>All product.</returns>      
        public List<SettingInfo> GetAllProduct()
        {
            var allProduct = new List<SettingInfo>();
            try
            {
                var rawData = _serviceSetting.GetAllProduct();
                foreach (var rawProduct in rawData)
                {
                    var product = rawProduct;
                    if (rawProduct.Id == SettingConstants.NotApply)
                    {
                        product.Value = Labels.All;
                    }

                    allProduct.Add(product);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allProduct;
        }

        /// <summary>
        /// Gets all site.
        /// </summary>
        /// <returns>All site value.</returns>
        public List<SiteInfo> GetAllSite()
        {
            var allSite = new List<SiteInfo>();
            try
            {
                allSite = _serviceSetting.GetAllSite();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allSite;
        }

        /// <summary>
        /// Gets all status.
        /// </summary>
        /// <returns>All status.</returns>
        public List<SettingInfo> GetAllStatus()
        {
            var allStatus = new List<SettingInfo>();
            try
            {
                var rawData = _serviceSetting.GetAllStatus();
                allStatus.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.All
                });

                foreach (var rawStatus in rawData)
                {
                    allStatus.Add(rawStatus);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allStatus;
        }

        /// <summary>
        /// Gets all response.
        /// </summary>
        /// <returns>All response.</returns>
        public List<SettingInfo> GetAllResponse()
        {
            var allResponse = new List<SettingInfo>();
            try
            {
                var rawData = _serviceSetting.GetAllResponse();
                allResponse.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Value = Labels.All
                });

                foreach (var rawResponse in rawData)
                {
                    allResponse.Add(rawResponse);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allResponse;
        }

        /// <summary>
        /// Gets all role.
        /// </summary>
        /// <returns>All role value.</returns>
        public List<SettingInfo> GetAllRole()
        {
            var allRole = new List<SettingInfo>();
            try
            {
                var rawData = _serviceSetting.GetAllResponse();
                allRole.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Value = Labels.All
                });

                foreach (var rawRole in rawData)
                {
                    allRole.Add(rawRole);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allRole;
        }

        /// <summary>
        /// Gets all communication tool.
        /// </summary>
        /// <returns>All communication tool.</returns>
        public List<SettingInfo> GetAllCommunicationTool()
        {
            var allCommunicationTool = new List<SettingInfo>();
            try
            {
                var rawData = _serviceSetting.GetAllResponse();
                allCommunicationTool.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Value = Labels.All
                });

                foreach (var rawCommunicationTool in rawData)
                {
                    allCommunicationTool.Add(rawCommunicationTool);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allCommunicationTool;
        }

        /// <summary>
        /// Gets all main category.
        /// </summary>
        /// <returns>All main category.</returns>
        public List<SettingInfo> GetAllMainCategory()
        {
            var allMainCategory = new List<SettingInfo>();
            try
            {
                var rawData = _serviceSetting.GetAllMainCategory();
                allMainCategory.Add(new SettingInfo
                {
                    Id = SettingConstants.NotApply,
                    Name = Labels.All,
                    ChildSettings = new List<SettingInfo>
                    {
                        new SettingInfo {
                            Id = SettingConstants.NotApply,
                            Name = Labels.All,
                        }
                    }
                });

                foreach (var rawMainCategory in rawData)
                {
                    allMainCategory.Add(rawMainCategory);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return allMainCategory;
        }
    }
}

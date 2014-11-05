// --------------------------------------------------------------------------
// <copyright file="UpdateCustomerFeedbackRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.10.20">Created</change>
// </history>

namespace Fanex.CRM.Feedback.Repositories
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.CRM.Common.Helpers;
    using Fanex.CRM.Common.Models;
    using Fanex.CRM.Feedback.Criteria;
    using Fanex.CRM.Feedback.Models;
    using Fanex.CRM.Infrastructure.Extensions;
    using Fanex.CRM.Infrastructure.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// Update customer feedback repository.
    /// </summary>
    public class UpdateCustomerFeedbackRepository : IUpdateCustomerFeedbackRepository
    {
        /// <summary>
        /// Gets the feedback by id.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The feedback of customer.</returns>
        public CustomerFeedback GetFeedbackById(ICriteria criteria)
        {
            CustomerFeedback feedback = null;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader != null && reader.Read())
                    {
                        feedback = GetCustomerFeedback(reader);
                        feedback.CustomerInfo = GetCustomerInfo(reader);
                    }
                }
            }

            return feedback;
        }

        /// <summary>
        /// Updates the feedback.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void UpdateFeedback(UpdateCustomerFeedbackCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();
                if (outputParams != null && outputParams.Count > 0)
                {
                    criteria.ErrorCode = (int)outputParams["errcode"];
                    criteria.ErrorMessage = (string)outputParams["errmsg"];
                }
            }
        }

        /// <summary>
        /// Gets the customer info.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The customer information.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private CustomerInfo GetCustomerInfo(IDataReader reader)
        {
            var birthday = Converter.ToDateTime(reader["Birthday"]);
            CustomerInfo customer = new CustomerInfo
            {
                Id = Converter.ToInt(reader["CustId"]),
                Name = Converter.ToString2(reader["Username"]),
                FirstName = Converter.ToString2(reader["FirstName"]),
                LastName = Converter.ToString2(reader["LastName"]),
                FaxInfo = new FaxInfo 
                {
                    CountryCode = Converter.ToString2(reader["FaxCountryCode"]),
                    AreaCode = Converter.ToString2(reader["FaxAreaCode"]),
                    FaxNumber = Converter.ToString2(reader["Fax"])
                },
                MobilePhone = new PhoneInfo 
                {
                    CountryCode = Converter.ToString2(reader["MobilePhoneCountryCode"]),
                    AreaCode = Converter.ToString2(reader["MobilePhoneAreaCode"]),
                    PhoneNumber = Converter.ToString2(reader["MobilePhone"])
                },
                Skype = Converter.ToString2(reader["Skype"]),
                Email = Converter.ToString2(reader["Email"]),
                MSN = Converter.ToString2(reader["MSN"]),
                Passport = new BaseModel { Name = Converter.ToString2(reader["Passport"]) },
                BirthDay = birthday == DateTime.MinValue ? string.Empty : birthday.ToShortFormatedDate(),
                IsVIP = Converter.ToBool(reader["IsVip"])
            };

            customer.BirthDayString = customer.BirthDay;
            customer.Title.Id = Converter.ToInt(reader["TitleId"]);
            customer.PhoneInfo.Add(new PhoneInfo 
            { 
                CountryCode = Converter.ToString2(reader["PhoneCountryCode"]),
                AreaCode = Converter.ToString2(reader["PhoneAreaCode"]),
                PhoneNumber = Converter.ToString2(reader["Phone"])
            });
            customer.PhoneInfo.Add(new PhoneInfo 
            { 
                CountryCode = Converter.ToString2(reader["Phone2CountryCode"]),
                AreaCode = Converter.ToString2(reader["Phone2AreaCode"]),
                PhoneNumber = Converter.ToString2(reader["Phone2"])
            });
            customer.PhoneInfo.Add(new PhoneInfo 
            { 
                CountryCode = Converter.ToString2(reader["Phone3CountryCode"]),
                AreaCode = Converter.ToString2(reader["Phone3AreaCode"]),
                PhoneNumber = Converter.ToString2(reader["Phone3"])
            });
            customer.SiteSetting.Id = Converter.ToInt(reader["SiteId"]);
            customer.BettingCode.Add(Converter.ToString2(reader["RefCode1"]));
            customer.BettingCode.Add(Converter.ToString2(reader["RefCode2"]));
            customer.PersonalInfoContact.Add(Converter.ToString2(reader["Pic1"]));
            customer.PersonalInfoContact.Add(Converter.ToString2(reader["Pic2"]));
            customer.PersonalInfoContact.Add(Converter.ToString2(reader["Pic3"]));
            customer.PersonalInfoContact.Add(Converter.ToString2(reader["Pic4"]));

            return customer;
        }

        /// <summary>
        /// Gets the customer feedback.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The feedback of customer.</returns>
        private CustomerFeedback GetCustomerFeedback(IDataReader reader)
        {
            CustomerFeedback feedback = new CustomerFeedback 
            {
                Id = Converter.ToInt(reader["FeedbackId"]),
                FeedbackTimeString = DateTimeUtilize.ChangeTimeZoneToClient(Converter.ToDateTime(reader["FeedbackDate"])),
                ResolutionTimeString = DateTimeUtilize.ChangeTimeZoneToClient(Converter.ToDateTime(reader["ResolutionDate"])),
                SolveTimeString = DateTimeUtilize.ChangeTimeZoneToClient(Converter.ToDateTime(reader["SolveDate"])),
                Extension = new BaseModel { Id = Converter.ToInt(reader["StationId"]) },
                Communication = new BaseModel { Id = Converter.ToInt(reader["CommunicationId"]) },
                Category = new BaseModel { Id = Converter.ToInt(reader["MainCategoryId"]) },
                SubCategory = new BaseModel { Id = Converter.ToInt(reader["SubcategoryId"]) },
                Product = new BaseModel { Id = Converter.ToInt(reader["ProductId"]) },
                BetType = new BaseModel { Id = Converter.ToInt(reader["BettypeId"]) },
                Country = new BaseModel { Id = Converter.ToInt(reader["CountryId"]) },
                Language = new BaseModel { Id = Converter.ToInt(reader["LanguageId"]) },
                Status = new BaseModel { Id = Converter.ToInt(reader["StatusId"]) },
                Role = new BaseModel { Id = Converter.ToInt(reader["RoleId"]) },
                Response = new BaseModel { Id = Converter.ToInt(reader["ResponseId"]) },
                RelatedCustomer = Converter.ToString2(reader["RelatedCustomer"]),
                Subject = Converter.ToString2(reader["Subject"]),
                Description = Converter.ToString2(reader["Description"]),
                Solution = Converter.ToString2(reader["Solution"]),
                IsReviewed = Converter.ToBool(reader["IsReviewed"]),
                ReviewerId = Converter.ToInt(reader["ReviewerId"]),
                ReviewerName = Converter.ToString2(reader["ReviewerName"]),
                Review = Converter.ToString2(reader["Review"]),
            };

            feedback.HasRelated = !string.IsNullOrWhiteSpace(feedback.RelatedCustomer);
            return feedback;
        }
    }
}

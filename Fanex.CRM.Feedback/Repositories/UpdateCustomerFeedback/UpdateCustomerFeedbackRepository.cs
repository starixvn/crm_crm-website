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
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
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

                        if (reader.NextResult())
                        {
                            var relatedCustomers = new List<BaseModel>();
                            while (reader.Read())
                            {
                                relatedCustomers.Add(new BaseModel
                                {
                                    Id = Converter.ToInt(reader["CustID"]),
                                    Name = Converter.ToString2(reader["UserName"])
                                });
                            }

                            feedback.RelatedCustomers = relatedCustomers.OrderBy(c => c.Name);
                            feedback.HasRelated = relatedCustomers.Count > 0;
                        }
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
                FaxInfo = new Fax
                {
                    CountryCode = Converter.ToString2(reader["FaxCountryCode"]),
                    AreaCode = Converter.ToString2(reader["FaxAreaCode"]),
                    FaxNumber = Converter.ToString2(reader["Fax"])
                },
                MobilePhone = PhoneInfo(reader, "MobilePhone"),
                Skype = Converter.ToString2(reader["Skype"]),
                Email = Converter.ToString2(reader["Email"]),
                MSN = Converter.ToString2(reader["MSN"]),
                Passport = new BaseModel { Name = Converter.ToString2(reader["Passport"]) },
                BirthDay = birthday == DateTime.MinValue ? string.Empty : birthday.ToShortFormatedDate(),
                IsVIP = Converter.ToBool(reader["IsVip"]),
                WeChat = Converter.ToString2(reader["WeChat"]),
                WhatsApp = Converter.ToString2(reader["WhatsApp"]),
                Line = Converter.ToString2(reader["Line"]),
                Yahoo = Converter.ToString2(reader["Yahoo"]),
                QQ = Converter.ToString2(reader["QQ"]),
                BBM = Converter.ToString2(reader["BBM"]),
                LicenseeType = new BaseModel
                {
                    Id = Converter.ToInt(reader["LicenseeType"])
                }
            };

            customer.BirthDayString = customer.BirthDay;
            customer.Title.Id = Converter.ToInt(reader["TitleId"]);
            customer.PhoneInfo.Add(PhoneInfo(reader, "Phone"));
            customer.PhoneInfo.Add(PhoneInfo(reader, "Phone2"));
            customer.PhoneInfo.Add(PhoneInfo(reader, "Phone3"));
            customer.SiteSetting.Id = Converter.ToInt(reader["SiteId"]);
            customer.BettingCode.Add(Converter.ToString2(reader["RefCode1"]));
            customer.BettingCode.Add(Converter.ToString2(reader["RefCode2"]));
            PersonalInfoContact pic = new PersonalInfoContact 
            {
                Name = Converter.ToString2(reader["Pic1Name"]),
                Phone = Converter.ToString2(reader["Pic1"])
            };
            customer.PersonalInfoContacts.Add(pic);

            pic = new PersonalInfoContact
            {
                Name = Converter.ToString2(reader["Pic2Name"]),
                Phone = Converter.ToString2(reader["Pic2"])
            };
            customer.PersonalInfoContacts.Add(pic);

            pic = new PersonalInfoContact
            {
                Name = Converter.ToString2(reader["Pic3Name"]),
                Phone = Converter.ToString2(reader["Pic3"])
            };
            customer.PersonalInfoContacts.Add(pic);

            pic = new PersonalInfoContact
            {
                Name = Converter.ToString2(reader["Pic4Name"]),
                Phone = Converter.ToString2(reader["Pic4"])
            };
            customer.PersonalInfoContacts.Add(pic);

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
                SubProduct = new BaseModel { Id = Converter.ToInt(reader["SubProductId"]) },
                BetType = new BaseModel { Id = Converter.ToInt(reader["BettypeId"]) },
                Country = new BaseModel { Id = Converter.ToInt(reader["CountryId"]) },
                Language = new BaseModel { Id = Converter.ToInt(reader["LanguageId"]) },
                Status = new BaseModel { Id = Converter.ToInt(reader["StatusId"]) },
                Role = new BaseModel { Id = Converter.ToInt(reader["RoleId"]) },
                Response = new BaseModel { Id = Converter.ToInt(reader["ResponseId"]) },
                Subject = Converter.ToString2(reader["Subject"]),
                Description = Converter.ToString2(reader["Description"]),
                Solution = Converter.ToString2(reader["Solution"]),
                IsReviewed = Converter.ToBool(reader["IsReviewed"]),
                ReviewerId = Converter.ToInt(reader["ReviewerId"]),
                ReviewerName = Converter.ToString2(reader["ReviewerName"]),
                Review = Converter.ToString2(reader["Review"])
            };

            return feedback;
        }

        private Phone PhoneInfo(IDataReader reader, string fieldName)
        {
            return new Phone
            {
                CountryCode = Converter.ToString2(reader[fieldName + "CountryCode"]),
                AreaCode = Converter.ToString2(reader[fieldName + "AreaCode"]),
                PhoneNumber = Converter.ToString2(reader[fieldName])
            };
        }
    }
}

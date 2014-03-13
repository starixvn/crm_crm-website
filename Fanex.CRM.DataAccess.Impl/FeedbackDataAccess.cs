// --------------------------------------------------------------------------
// <copyright file="FeedbackDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.DataAccess.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using DNAide.Core;
    using Fanex.Common;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.CRM.Entities.ViewModel;
    using Fanex.CRM.Infrastructure.Common;
    using Fanex.CRM.Infrastructure.Core;
    using Fanex.Data;

    /// <summary>
    /// Feedback Data Access.
    /// </summary>
    public class FeedbackDataAccess : IFeedbackDataAccess
    {
        public bool Save(Feedback entity)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int tId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Get result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public Feedback GetById(int id)
        {
            Feedback feedback = null;
            using (var obj = new ObjectDb("FeedbackSelByID", 500))
            {
                obj.AddInParameter("feedbackid", DbType.Int32, id);
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null && reader.Read())
                    {
                        feedback = GetFeedbackInformation(reader);
                        feedback.CustomerInfo = GetCustomerformation(reader);
                        feedback.RelatedCustomer = GetRealtedCustomer(reader);

                        if (reader.NextResult())
                        {
                            feedback.CustomerInfo.Languages = GetCustomerLanguages(reader);
                        }
                    }
                }
            }

            return feedback;
        }

        /// <summary>
        /// Adds the guest feedback.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="result">The result.</param>
        /// <returns>Add result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public bool AddGuestFeedback(Feedback entity, string ip, ref Result result)
        {
            var numberOfRecordsAffected = -1;
            try
            {
                var customerInfo = entity.CustomerInfo;
                var contactNo1 = customerInfo.ContactNumber1;
                var contactNo2 = customerInfo.ContactNumber2;
                var contactNo3 = customerInfo.ContactNumber3;
                var mobile = customerInfo.Mobile;
                var fax = customerInfo.Fax;
                using (var obj = new ObjectDb("Acc_CRM_GuestFeedbackIns", 500))
                {
                    var param = new Hashtable();
                    param.Add("userid", entity.Creator.Id);
                    param.Add("creatorid", entity.Creator.Id);
                    param.Add("username", entity.Customer.UserName);
                    param.Add("firstname", customerInfo.FirstName);
                    param.Add("lastname", customerInfo.LastName);
                    param.Add("titleid", customerInfo.Title.Id);
                    param.Add("birthday", customerInfo.BirthDay);
                    param.Add("phone", contactNo1.PhoneNumber);
                    param.Add("phonecountrycode", contactNo1.CountryCode);
                    param.Add("phoneareacode", contactNo1.AreaCode);
                    param.Add("phone2", contactNo2.PhoneNumber);
                    param.Add("phone2countrycode", contactNo2.CountryCode);
                    param.Add("phone2areacode", contactNo2.AreaCode);
                    param.Add("phone3", contactNo3.PhoneNumber);
                    param.Add("phone3countrycode", contactNo3.CountryCode);
                    param.Add("phone3areacode", contactNo3.AreaCode);
                    param.Add("passport", customerInfo.PassportNumber);
                    param.Add("mobilephone", mobile.PhoneNumber);
                    param.Add("mobilephonecountrycode", mobile.CountryCode);
                    param.Add("mobilephoneareacode", mobile.AreaCode);
                    param.Add("fax", fax.FaxNumber);
                    param.Add("faxcountrycode", fax.CountryCode);
                    param.Add("faxareacode", fax.AreaCode);
                    param.Add("email", customerInfo.Email);
                    param.Add("skype", customerInfo.Skype);
                    param.Add("msn", customerInfo.MSN);
                    param.Add("bettingcode", customerInfo.BettingCode);
                    param.Add("refcode2", customerInfo.RefCodes2);
                    param.Add("pic1", customerInfo.PIC1);
                    param.Add("pic2", customerInfo.PIC2);
                    param.Add("pic3", customerInfo.PIC3);
                    param.Add("pic4", customerInfo.PIC4);
                    param.Add("siteid", customerInfo.Site);
                    param.Add("countryid", customerInfo.Country);
                    param.Add("languageid", string.Join(",", customerInfo.Languages));
                    param.Add("isvip", customerInfo.IsVIP);
                    
                    param.Add("feedbackdate", DateTimeUtilize.ChangeTimeZoneToServer(entity.FeedbackDate));
                    param.Add("resolutiondate", DateTimeUtilize.ChangeTimeZoneToServer(entity.ResolutionTime));
                    param.Add("stationid", entity.CsExtension.Id);
                    param.Add("communicationid", entity.Communication.Id);
                    param.Add("maincategoryid", entity.Category.Id);
                    param.Add("subcategoryid", entity.SubCategory.Id);
                    param.Add("productid", entity.Product.Id);
                    param.Add("bettypeid", entity.BetType.Id);
                    param.Add("statusid", entity.Status.Id);
                    param.Add("roleid", entity.Role.Id);
                    param.Add("responseid", entity.Response.Id);
                    param.Add("relatedcustid", entity.RelatedCustomer.UserName);
                    param.Add("subject", entity.Subject);
                    param.Add("description", entity.Description);
                    param.Add("solution", entity.Solution);
                    param.Add("isreviewed", entity.IsReviewed);
                    param.Add("review", entity.Review);
                    if (entity.Reviewer.Id > 0)
                    {
                        param.Add("reviewerid", entity.Reviewer.Id);
                    }

                    param.Add("ip", ip);
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);

                    obj.AssignParamValues(param);
                    numberOfRecordsAffected = obj.ExecuteNonQuery();

                    result.ErrCode = Convert.ToInt32(obj.GetParameterValue("errcode"));
                    result.ErrMsg = Convert.ToString(obj.GetParameterValue("errmsg"));
                }
            }
            catch (Exception ex)
            {
                result.ErrCode = -1;
                result.ErrMsg = ex.Message;
            }

            return numberOfRecordsAffected > 0;
        }

        /// <summary>
        /// Adds the feedback.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="result">The result.</param>
        /// <returns>Add result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public bool AddFeedback(Feedback entity, string ip, ref Result result)
        {
            int numberOfRecordsAffected = -1;
            try
            {
                var customerInfo = entity.CustomerInfo;
                var contactNo1 = customerInfo.ContactNumber1;
                var contactNo2 = customerInfo.ContactNumber2;
                var contactNo3 = customerInfo.ContactNumber3;
                var mobile = customerInfo.Mobile;
                var fax = customerInfo.Fax;
                using (var obj = new ObjectDb("CustomerFeedbackIns", 500))
                {
                    var param = new Hashtable();
                    param.Add("userid", entity.Creator.Id);
                    param.Add("custid", entity.Customer.Id);
                    param.Add("username", entity.Customer.UserName);
                    param.Add("creatorid", entity.Creator.Id);
                    //// Personal information.
                    param.Add("firstname", customerInfo.FirstName);
                    param.Add("lastname", customerInfo.LastName);
                    param.Add("titleid", customerInfo.Title.Id);
                    param.Add("birthday", customerInfo.BirthDay);
                    param.Add("phone", contactNo1.PhoneNumber);
                    param.Add("phonecountrycode", contactNo1.CountryCode);
                    param.Add("phoneareacode", contactNo1.AreaCode);
                    param.Add("phone2", contactNo2.PhoneNumber);
                    param.Add("phone2countrycode", contactNo2.CountryCode);
                    param.Add("phone2areacode", contactNo2.AreaCode);
                    param.Add("phone3", contactNo3.PhoneNumber);
                    param.Add("phone3countrycode", contactNo3.CountryCode);
                    param.Add("phone3areacode", contactNo3.AreaCode);
                    param.Add("passport", customerInfo.PassportNumber);
                    param.Add("mobilephone", mobile.PhoneNumber);
                    param.Add("mobilephonecountrycode", mobile.CountryCode);
                    param.Add("mobilephoneareacode", mobile.AreaCode);
                    param.Add("fax", fax.FaxNumber);
                    param.Add("faxcountrycode", fax.CountryCode);
                    param.Add("faxareacode", fax.AreaCode);
                    param.Add("email", customerInfo.Email);
                    param.Add("skype", customerInfo.Skype);
                    param.Add("msn", customerInfo.MSN);
                    param.Add("bettingcode", customerInfo.BettingCode);
                    param.Add("refcode2", customerInfo.RefCodes2);
                    param.Add("pic1", customerInfo.PIC1);
                    param.Add("pic2", customerInfo.PIC2);
                    param.Add("pic3", customerInfo.PIC3);
                    param.Add("pic4", customerInfo.PIC4);
                    param.Add("siteid", customerInfo.Site);
                    param.Add("countryid", customerInfo.Country);
                    param.Add("languageid", string.Join(",", customerInfo.Languages));
                    param.Add("isvip", customerInfo.IsVIP);

                    // Detail tab.
                    param.Add("feedbackdate", DateTimeUtilize.ChangeTimeZoneToServer(entity.FeedbackDate));
                    param.Add("resolutiondate", DateTimeUtilize.ChangeTimeZoneToServer(entity.ResolutionTime));

                    param.Add("communicationid", entity.Communication.Id);
                    param.Add("stationid", entity.CsExtension.Id);
                    param.Add("maincategoryid", entity.Category.Id);
                    param.Add("subcategoryid", entity.SubCategory.Id);
                    param.Add("roleid", entity.Role.Id);
                    param.Add("productid", entity.Product.Id);
                    param.Add("bettypeid", entity.BetType.Id);
                    param.Add("responseid", entity.Response.Id);
                    param.Add("statusid", entity.Status.Id);

                    if (entity.RelatedCustomer != null && !string.IsNullOrWhiteSpace(entity.RelatedCustomer.UserName))
                    {
                        param.Add("relatedcustid", entity.RelatedCustomer.UserName);
                    }

                    param.Add("subject", entity.Subject);
                    param.Add("description", entity.Description);
                    param.Add("solution", entity.Solution);
                    param.Add("isreviewed", entity.IsReviewed);
                    if (entity.IsReviewed)
                    {
                        param.Add("review", entity.Review);
                        param.Add("reviewerid", entity.Reviewer.Id);
                    }

                    param.Add("isoverwrite", entity.IsOverrideCrmInfo);
                    param.Add("ip", ip);
                    param.Add("errcode", 0);
                    param.Add("errmsg", string.Empty);

                    obj.AssignParamValues(param);
                    numberOfRecordsAffected = obj.ExecuteNonQuery();

                    result.ErrCode = Converter.ToInt(obj.GetParameterValue("errcode"));
                    result.ErrMsg = Converter.ToString2(obj.GetParameterValue("errmsg"));
                }
            }
            catch (Exception ex)
            {
                result.ErrCode = -1;
                result.ErrMsg = ex.Message;
            }

            return numberOfRecordsAffected > 0;
        }

        /// <summary>
        /// Edits the feedback.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="ip">The IP address.</param>
        /// <param name="result">The result.</param>
        /// <returns>Edit result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public bool EditFeedback(Feedback entity, int userId, string ip, ref Result result)
        {
            var numberOfRecordsAffected = -1;

            var customerInfo = entity.CustomerInfo;
            var contactNo1 = customerInfo.ContactNumber1;
            var contactNo2 = customerInfo.ContactNumber2;
            var contactNo3 = customerInfo.ContactNumber3;
            var mobile = customerInfo.Mobile;
            var fax = customerInfo.Fax;
            using (var obj = new ObjectDb("FeedbackInfoUpd", 500))
            {
                var param = new Hashtable();
                param.Add("feedbackid", entity.Id);
                param.Add("userid", entity.Creator.Id);
                param.Add("custid", entity.Customer.Id);
                //// Personal information.
                param.Add("firstname", customerInfo.FirstName);
                param.Add("lastname", customerInfo.LastName);
                param.Add("titleid", customerInfo.Title.Id);
                param.Add("birthday", customerInfo.BirthDay);
                param.Add("phone", contactNo1.PhoneNumber);
                param.Add("phonecountrycode", contactNo1.CountryCode);
                param.Add("phoneareacode", contactNo1.AreaCode);
                param.Add("phone2", contactNo2.PhoneNumber);
                param.Add("phone2countrycode", contactNo2.CountryCode);
                param.Add("phone2areacode", contactNo2.AreaCode);
                param.Add("phone3", contactNo3.PhoneNumber);
                param.Add("phone3countrycode", contactNo3.CountryCode);
                param.Add("phone3areacode", contactNo3.AreaCode);
                param.Add("passport", customerInfo.PassportNumber);
                param.Add("mobilephone", mobile.PhoneNumber);
                param.Add("mobilephonecountrycode", mobile.CountryCode);
                param.Add("mobilephoneareacode", mobile.AreaCode);
                param.Add("fax", fax.FaxNumber);
                param.Add("faxcountrycode", fax.CountryCode);
                param.Add("faxareacode", fax.AreaCode);
                param.Add("email", customerInfo.Email);
                param.Add("skype", customerInfo.Skype);
                param.Add("msn", customerInfo.MSN);
                param.Add("bettingcode", customerInfo.BettingCode);
                param.Add("refcode2", customerInfo.RefCodes2);
                param.Add("pic1", customerInfo.PIC1);
                param.Add("pic2", customerInfo.PIC2);
                param.Add("pic3", customerInfo.PIC3);
                param.Add("pic4", customerInfo.PIC4);
                param.Add("siteid", customerInfo.Site);
                param.Add("countryid", customerInfo.Country);
                if (customerInfo.Languages != null)
                {
                    param.Add("languageid", string.Join(",", customerInfo.Languages));
                }
                param.Add("isvip", customerInfo.IsVIP);

                // Detail tab.
                param.Add("feedbackdate", DateTimeUtilize.ChangeTimeZoneToServer(entity.FeedbackDate));
                param.Add("resolutiondate", DateTimeUtilize.ChangeTimeZoneToServer(entity.ResolutionTime));
                param.Add("communicationid", entity.Communication.Id);
                param.Add("stationid", entity.CsExtension.Id);
                param.Add("maincategoryid", entity.Category.Id);
                param.Add("subcategoryid", entity.SubCategory.Id);
                param.Add("roleid", entity.Role.Id);
                param.Add("productid", entity.Product.Id);
                param.Add("bettypeid", entity.BetType.Id);
                param.Add("responseid", entity.Response.Id);
                param.Add("statusid", entity.Status.Id);

                if (entity.RelatedCustomer != null && !string.IsNullOrWhiteSpace(entity.RelatedCustomer.UserName))
                {
                    var relatedUsername = !string.IsNullOrEmpty(entity.RelatedCustomer.UserName) ?
                        (entity.RelatedCustomer.UserName.Equals("_") ?
                        string.Empty :
                        entity.RelatedCustomer.UserName) :
                        null;
                    param.Add("relatedcustid", relatedUsername);
                }

                param.Add("subject", entity.Subject);
                param.Add("description", entity.Description);
                param.Add("solution", entity.Solution);

                param.Add("updatereviewedmode", entity.UpdateReviewMode);
                param.Add("isreviewed", entity.IsReviewed);
                if (entity.UpdateReviewMode == 1)
                {
                    param.Add("review", entity.Review);
                    param.Add("reviewerid", entity.Reviewer.Id);
                }
                else
                {
                    param.Add("review", string.Empty);
                }

                param.Add("isoverwrite", entity.IsOverrideCrmInfo);
                param.Add("ip", ip);
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                obj.AssignParamValues(param);
                numberOfRecordsAffected = obj.ExecuteNonQuery();

                result.ErrCode = Convert.ToInt32(obj.GetParameterValue("errcode"));
                result.ErrMsg = Convert.ToString(obj.GetParameterValue("errmsg"));
            }

            return numberOfRecordsAffected > 0;
        }

        /// <summary>
        /// Gets the feedback by customer identifier.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <returns>The feedback by customer identifier.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public IEnumerable<Feedback> GetFeedbackByCustId(int custId, DateTime fromDate, DateTime toDate)
        {
            var lstFeedbacks = new List<Feedback>();
            using (var obj = new ObjectDb("FeedbackSelByDate", 500))
            {
                var param = new Hashtable
                {
                    { "custid", custId },
                    { "fromdate", DateTimeUtilize.ChangeTimeToServerForReportBegin(fromDate) },
                    { "todate", DateTimeUtilize.ChangeTimeToServerForReportEnd(toDate) }
                };

                obj.AssignParamValues(param);
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var feedback = new Feedback();
                            feedback.Id = reader.GetInt32(reader.GetOrdinal("feedbackid"));

                            if (!reader.IsDBNull(reader.GetOrdinal("custid")))
                            {
                                feedback.Customer = new Customer
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("custid")),
                                    UserName = reader.GetString(reader.GetOrdinal("username")),
                                };
                            }

                            feedback.CustomerInfo = new CustomerInfo
                            {
                                Title = new Setting
                                {
                                    Id = reader.IsDBNull(reader.GetOrdinal("titleid"))
                                    ? 0
                                    : reader.GetInt32(reader.GetOrdinal("titleid"))
                                },
                                Email = reader.IsDBNull(reader.GetOrdinal("email")) ? string.Empty : reader.GetString(reader.GetOrdinal("email")),
                                //Fax = reader.IsDBNull(reader.GetOrdinal("fax")) ? string.Empty : reader.GetString(reader.GetOrdinal("fax")),
                                FirstName = reader.IsDBNull(reader.GetOrdinal("firstname")) ? string.Empty : reader.GetString(reader.GetOrdinal("firstname")),
                                LastName = reader.IsDBNull(reader.GetOrdinal("lastname")) ? string.Empty : reader.GetString(reader.GetOrdinal("lastname")),
                                //MobilePhone = reader.IsDBNull(reader.GetOrdinal("mobilephone")) ? string.Empty : reader.GetString(reader.GetOrdinal("mobilephone")),
                                //Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? string.Empty : reader.GetString(reader.GetOrdinal("phone"))
                            };

                            feedback.Language = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("languageid"))
                            };

                            feedback.Country = new Country
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("countryid"))
                            };

                            feedback.CreateDate = DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("createdate")));

                            feedback.Creator = new User
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("creatorid")),
                                Username = reader.GetString(reader.GetOrdinal("creatorname"))
                            };

                            feedback.FeedbackDate = DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("feedbackdate")));

                            feedback.Communication = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("communicationid"))
                            };

                            feedback.CsExtension = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("StationID"))
                            };

                            feedback.Category = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("maincategoryid"))
                            };

                            feedback.SubCategory = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("subcategoryid"))
                            };

                            feedback.Site = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("subcategoryid"))
                            };

                            feedback.Role = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("roleid"))
                            };

                            feedback.Role = new Setting
                            {
                                Id = reader.GetOrdinal("productid")
                            };

                            feedback.BetType = new Setting
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("bettypeid"))
                                ? 0
                                : reader.GetInt32(reader.GetOrdinal("bettypeid"))
                            };

                            var responseId = reader.GetInt32(reader.GetOrdinal("responseid"));
                            feedback.Response = new Setting { Id = responseId };

                            feedback.Status = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("statusid"))
                            };

                            feedback.Subject = reader.IsDBNull(reader.GetOrdinal("subject"))
                                 ? string.Empty
                                 : reader.GetString(reader.GetOrdinal("subject"));

                            feedback.Description = reader.IsDBNull(reader.GetOrdinal("description"))
                                ? string.Empty
                                : reader.GetString(reader.GetOrdinal("description"));

                            feedback.Solution = reader.IsDBNull(reader.GetOrdinal("solution"))
                                ? string.Empty
                                : reader.GetString(reader.GetOrdinal("solution"));

                            feedback.Review = reader.IsDBNull(reader.GetOrdinal("review"))
                                ? string.Empty
                                : reader.GetString(reader.GetOrdinal("review"));

                            var reviewerId = reader.IsDBNull(reader.GetOrdinal("reviewerid"))
                                ? 0
                                : reader.GetInt32(reader.GetOrdinal("reviewerid"));

                            if (reviewerId > 0)
                            {
                                feedback.Reviewer = new User
                                {
                                    Id = reviewerId,
                                    Username = reader.IsDBNull(reader.GetOrdinal("reviewername"))
                                                   ? string.Empty
                                                   : reader.GetString(reader.GetOrdinal("reviewername")),
                                };
                                feedback.SolvedDate = DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("solvedate")));
                            }
                            else
                            {
                                feedback.Reviewer = new User
                                {
                                    Id = 0,
                                    Username = string.Empty
                                };
                            }

                            feedback.RelatedCustomer = new Customer
                            {
                                Id = 0,
                                UserName = reader.GetBoolean(reader.GetOrdinal("isRelated")) ?
                                    (reader.IsDBNull(reader.GetOrdinal("username")) ? string.Empty : reader.GetString(reader.GetOrdinal("username"))) :
                                    (reader.IsDBNull(reader.GetOrdinal("relatedCustomer")) ? string.Empty : reader.GetString(reader.GetOrdinal("relatedCustomer")))
                            };
                            lstFeedbacks.Add(feedback);
                        }
                    }
                }
            }

            return lstFeedbacks;
        }

        /// <summary>
        /// Gets the feedback.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="isGuest">The is guest.</param>
        /// <param name="communicationID">The communication identifier.</param>
        /// <param name="mainCategoryID">The main category identifier.</param>
        /// <param name="subCategoryID">The sub category identifier.</param>
        /// <param name="statusID">The status identifier.</param>
        /// <param name="isReviewed">The is reviewed.</param>
        /// <param name="responseId">The response identifier.</param>
        /// <returns>The feedback.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public IEnumerable<Feedback> GetFeedback(
            DateTime fromDate,
            DateTime toDate,
            string userName,
            int isGuest,
            int communicationID,
            int mainCategoryID,
            int subCategoryID,
            int statusID,
            int isReviewed,
            int responseId)
        {
            var lstFeedbacks = new List<Feedback>();
            using (var obj = new ObjectDb("FeedbackGetLastest", 500))
            {
                var param = new Hashtable();
                param.Add("fromdate", DateTimeUtilize.ChangeTimeToServerForReportBegin(fromDate));
                param.Add("todate", DateTimeUtilize.ChangeTimeToServerForReportEnd(toDate));
                param.Add("username", userName);
                if (isGuest >= 0)
                {
                    param.Add("isguest", isGuest == 1);
                }

                if (communicationID > 0)
                {
                    param.Add("communicationid", communicationID);
                }

                if (mainCategoryID > 0)
                {
                    param.Add("maincategoryid", mainCategoryID);
                }

                if (subCategoryID > 0)
                {
                    param.Add("subcategoryid", subCategoryID);
                }

                if (statusID > 0)
                {
                    param.Add("statusid", statusID);
                }

                if (responseId > 0)
                {
                    param.Add("responseid", responseId);
                }

                param.Add("isreviewed", isReviewed == 1 ? true : false);
                obj.AssignParamValues(param);
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        Feedback feedback;
                        while (reader.Read())
                        {
                            feedback = GetFeedbackInformation(reader);
                            feedback.IsReviewed = Converter.ToBool(reader["isreviewed"]);
                            feedback.CustomerInfo = GetCustomerInformation4Report(reader);
                            feedback.RelatedCustomer = GetRealtedCustomer4Report(reader);

                            lstFeedbacks.Add(feedback);
                        }
                    }
                }
            }

            return lstFeedbacks;
        }

        /// <summary>
        /// Gets the feedback statistic.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns>The feedback statistic.</returns>
        public DataTableCollection GetFeedbackStatistic(Params param)
        {
            DataTableCollection dataTables = null;

            using (var obj = new ObjectDb("FeedbackStatistics", 500))
            {
                var p = new Hashtable();
                p.Add("type", param.Type);
                p.Add("fromdate", DateTimeUtilize.ChangeTimeToServerForReportBegin(param.FromDate));
                p.Add("todate", DateTimeUtilize.ChangeTimeToServerForReportEnd(param.ToDate));
                var fromDuration = string.Format(
                    "{0}:{1}:{2}:{3}",
                    CastAide.AsInt32(param.FromDuration.Day),
                    CastAide.AsInt32(param.FromDuration.Hour),
                    CastAide.AsInt32(param.FromDuration.Minute),
                    CastAide.AsInt32(param.FromDuration.Second));
                p.Add("fromduration", fromDuration);
                var toDuration = string.Format(
                    "{0}:{1}:{2}:{3}",
                    CastAide.AsInt32(param.ToDuration.Day),
                    CastAide.AsInt32(param.ToDuration.Hour),
                    CastAide.AsInt32(param.ToDuration.Minute),
                    CastAide.AsInt32(param.ToDuration.Second));
                p.Add("toduration", toDuration);
                obj.AssignParamValues(p);
                var ds = obj.ExecuteDataSet();
                if (ds.Tables.Count > 0)
                {
                    dataTables = ds.Tables;
                }
            }

            return dataTables;
        }

        /// <summary>
        /// Gets the feedback statistic detail.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <param name="totalRows">The total rows.</param>
        /// <returns>The feedback statistic detail.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public IEnumerable<Feedback> GetFeedbackStatisticDetail(Params param, ref int totalRows)
        {
            var lstFeedbacks = new List<Feedback>();
            using (var obj = new ObjectDb("Rpt_FeedbackStatisticDetails", 1200))
            {
                var p = new Hashtable
                    {
                        { "type", param.Type },
                        { "valueid", param.ValueId },
                        { "maincategoryid", param.MainId },
                        { "subcategoryid", param.SubId },
                        { "fromdate", DateTimeUtilize.ChangeTimeToServerForReportBegin(param.FromDate) },
                        { "todate", DateTimeUtilize.ChangeTimeToServerForReportEnd(param.ToDate) },
                        {
                            "fromduration",
                            string.Format(
                                "{0}:{1}:{2}:{3}",
                                param.FromDuration.Day,
                                param.FromDuration.Hour,
                                param.FromDuration.Minute,
                                param.FromDuration.Second)
                            },
                        {
                            "toduration",
                            string.Format(
                                "{0}:{1}:{2}:{3}",
                                param.ToDuration.Day,
                                param.ToDuration.Hour,
                                param.ToDuration.Minute,
                                param.ToDuration.Second)
                            }
                    };
                obj.AssignParamValues(p);

                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var feedback = new Feedback();
                            feedback.Id = reader.GetInt32(reader.GetOrdinal("feedbackid"));
                            if (!reader.IsDBNull(reader.GetOrdinal("custid")))
                            {
                                feedback.Customer = new Customer
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("custid")),
                                    UserName = reader.GetString(reader.GetOrdinal("username")),
                                };
                            }

                            feedback.CustomerInfo = new CustomerInfo
                                {
                                    Title =
                                        new Setting
                                            {
                                                Id =
                                                    reader.IsDBNull(reader.GetOrdinal("titleid"))
                                                        ? 0
                                                        : reader.GetInt32(reader.GetOrdinal("titleid"))
                                            },
                                    Email =
                                        reader.IsDBNull(reader.GetOrdinal("email"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("email")),
                                    /*Fax =
                                        reader.IsDBNull(reader.GetOrdinal("fax"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("fax")),*/
                                    FirstName =
                                        reader.IsDBNull(reader.GetOrdinal("firstname"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("firstname")),
                                    LastName =
                                        reader.IsDBNull(reader.GetOrdinal("lastname"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("lastname"))/*,
                                    MobilePhone =
                                        reader.IsDBNull(reader.GetOrdinal("mobilephone"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("mobilephone")),
                                    Phone =
                                        reader.IsDBNull(reader.GetOrdinal("phone"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("phone"))*/
                                };

                            feedback.Language = new Setting
                            {
                                Id = 
                                    reader.IsDBNull(reader.GetOrdinal("languageid")) 
                                        ? 0
                                        : reader.GetInt32(reader.GetOrdinal("languageid"))
                            };

                            feedback.Country = new Country
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("countryid"))
                                        ? 0
                                        : reader.GetInt32(reader.GetOrdinal("countryid"))
                            };

                            feedback.CreateDate = DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("createdate")));

                            feedback.Creator = new User
                                {
                                    Id =
                                        reader.IsDBNull(reader.GetOrdinal("creatorid"))
                                            ? 0
                                            : reader.GetInt32(reader.GetOrdinal("creatorid")),
                                    Username =
                                        reader.IsDBNull(reader.GetOrdinal("creatorname"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("creatorname"))
                                };

                            feedback.FeedbackDate = DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("feedbackdate")));
                            feedback.ResolutionTime = !reader.IsDBNull(reader.GetOrdinal("resolutiondate"))
                                                          ? DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("resolutiondate")))
                                                          : null;

                            feedback.Communication = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("communicationid"))
                            };

                            feedback.CsExtension = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("StationID"))
                            };

                            feedback.Category = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("maincategoryid"))
                            };

                            feedback.SubCategory = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("subcategoryid"))
                            };

                            feedback.Site = new Setting
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("siteid"))
                                ? 0
                                : reader.GetInt32(reader.GetOrdinal("siteid"))
                            };

                            feedback.Role = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("roleid"))
                            };

                            feedback.Role = new Setting
                            {
                                Id = reader.GetOrdinal("productid")
                            };

                            feedback.BetType = new Setting
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("bettypeid"))
                                ? 0
                                : reader.GetInt32(reader.GetOrdinal("bettypeid"))
                            };

                            var responseId = reader.GetInt32(reader.GetOrdinal("responseid"));
                            feedback.Response = new Setting { Id = responseId };

                            feedback.Status = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("statusid"))
                            };

                            feedback.Subject = reader.IsDBNull(reader.GetOrdinal("subject"))
                                 ? string.Empty
                                 : reader.GetString(reader.GetOrdinal("subject"));

                            feedback.Description = reader.IsDBNull(reader.GetOrdinal("description"))
                                ? string.Empty
                                : reader.GetString(reader.GetOrdinal("description"));

                            feedback.Solution = reader.IsDBNull(reader.GetOrdinal("solution"))
                                ? string.Empty
                                : reader.GetString(reader.GetOrdinal("solution"));

                            feedback.Review = reader.IsDBNull(reader.GetOrdinal("rerview"))
                                ? string.Empty
                                : reader.GetString(reader.GetOrdinal("review"));

                            var reviewerId = reader.IsDBNull(reader.GetOrdinal("reviewerid"))
                                ? 0
                                : reader.GetInt32(reader.GetOrdinal("reviewerid"));

                            if (reviewerId > 0)
                            {
                                feedback.Reviewer = new User
                                {
                                    Id = reviewerId,
                                    Username = reader.IsDBNull(reader.GetOrdinal("reviewername"))
                                                   ? string.Empty
                                                   : reader.GetString(reader.GetOrdinal("reviewername"))
                                };

                                feedback.SolvedDate = !reader.IsDBNull(reader.GetOrdinal("solvedate")) ?
                                    DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("solvedate"))) :
                                    null;
                            }
                            else
                            {
                                feedback.Reviewer = new User
                                {
                                    Id = 0,
                                    Username = string.Empty
                                };
                            }

                            feedback.RelatedCustomer = new Customer
                                {
                                    Id = 0,
                                    UserName =
                                        reader.GetBoolean(reader.GetOrdinal("isRelated"))
                                            ? (reader.IsDBNull(reader.GetOrdinal("username"))
                                                   ? string.Empty
                                                   : reader.GetString(reader.GetOrdinal("username")))
                                            : (reader.IsDBNull(reader.GetOrdinal("relatedCustomer"))
                                                   ? string.Empty
                                                   : reader.GetString(reader.GetOrdinal("relatedCustomer")))
                                };
                            lstFeedbacks.Add(feedback);
                        }

                        totalRows = lstFeedbacks.Count;
                    }
                }
            }

            return lstFeedbacks;
        }

        /// <summary>
        /// Gets the deleted feedback.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="isGuest">The is guest.</param>
        /// <param name="communicationID">The communication identifier.</param>
        /// <param name="mainCategoryID">The main category identifier.</param>
        /// <param name="subCategoryID">The sub category identifier.</param>
        /// <param name="isReviewed">The is reviewed.</param>
        /// <param name="responseId">The response identifier.</param>
        /// <returns>The deleted feedback.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public IEnumerable<Feedback> GetDeletedFeedback(
            DateTime fromDate,
            DateTime toDate,
            string userName,
            int isGuest,
            int communicationID,
            int mainCategoryID,
            int subCategoryID,
            int isReviewed,
            int responseId)
        {
            var lstFeedbacks = new List<Feedback>();

            using (var obj = new ObjectDb("FeedbackGetDeleted", 500))
            {
                var param = new Hashtable();
                param.Add("fromdate", DateTimeUtilize.ChangeTimeToServerForReportBegin(fromDate));
                param.Add("todate", DateTimeUtilize.ChangeTimeToServerForReportEnd(toDate));
                param.Add("username", userName);
                if (isGuest >= 0)
                {
                    param.Add("isguest", isGuest == 1);
                }

                if (communicationID > 0)
                {
                    param.Add("communicationid", communicationID);
                }

                if (mainCategoryID > 0)
                {
                    param.Add("maincategoryid", mainCategoryID);
                }

                if (subCategoryID > 0)
                {
                    param.Add("subcategoryid", subCategoryID);
                }

                if (responseId > 0)
                {
                    param.Add("responseid", responseId);
                }

                if (isReviewed == 1)
                {
                    param.Add("isreviewed", true);
                }
                else if (isReviewed == -1)
                {
                    param.Add("isreviewed", false);
                }

                obj.AssignParamValues(param);
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var feedback = new Feedback();
                            feedback.Id = reader.GetInt32(reader.GetOrdinal("feedbackid"));

                            if (!reader.IsDBNull(reader.GetOrdinal("custid")))
                            {
                                feedback.Customer = new Customer
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("custid")),
                                    UserName = reader.GetString(reader.GetOrdinal("username")),
                                };
                            }

                            feedback.CustomerInfo = new CustomerInfo
                            {
                                Title =
                                    new Setting
                                    {
                                        Id =
                                            reader.IsDBNull(reader.GetOrdinal("titleid"))
                                                ? 0
                                                : reader.GetInt32(reader.GetOrdinal("titleid"))
                                    },
                                Email =
                                    reader.IsDBNull(reader.GetOrdinal("email"))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("email")),
                                /*Fax =
                                    reader.IsDBNull(reader.GetOrdinal("fax"))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("fax")),*/
                                FirstName =
                                    reader.IsDBNull(reader.GetOrdinal("firstname"))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("firstname")),
                                LastName =
                                    reader.IsDBNull(reader.GetOrdinal("lastname"))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("lastname"))/*,
                                MobilePhone =
                                    reader.IsDBNull(reader.GetOrdinal("mobilephone"))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("mobilephone")),
                                Phone =
                                    reader.IsDBNull(reader.GetOrdinal("phone"))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("phone"))*/
                            };

                            feedback.Language = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("languageid"))
                            };

                            feedback.Country = new Country
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("languageid"))
                            };

                            feedback.CreateDate = DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("createdate")));

                            feedback.Creator = new User
                            {
                                Id =
                                    reader.IsDBNull(reader.GetOrdinal("creatorid"))
                                        ? 0
                                        : reader.GetInt32(reader.GetOrdinal("creatorid")),
                                Username =
                                    reader.IsDBNull(reader.GetOrdinal("creatorname"))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("creatorname"))
                            };

                            feedback.FeedbackDate =
                                DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("feedbackdate")));
                            feedback.Communication = new Setting { Id = reader.GetInt32(reader.GetOrdinal("communicationid")) };
                            feedback.CsExtension = new Setting { Id = reader.GetInt32(reader.GetOrdinal("stationid")) };
                            feedback.Category = new Setting { Id = reader.GetInt32(reader.GetOrdinal("maincategoryid")) };
                            feedback.SubCategory = new Setting { Id = reader.GetInt32(reader.GetOrdinal("subcategoryid")) };
                            feedback.Site = new Setting { Id = reader.GetInt32(reader.GetOrdinal("subcategoryid")) };
                            feedback.Role = new Setting { Id = reader.GetInt32(reader.GetOrdinal("roleid")) };
                            feedback.Role = new Setting { Id = reader.GetOrdinal("productid") };
                            feedback.BetType = new Setting
                            {
                                Id =
                                    reader.IsDBNull(reader.GetOrdinal("bettypeid"))
                                        ? 0
                                        : reader.GetInt32(reader.GetOrdinal("bettypeid"))
                            };

                            feedback.Response = new Setting { Id = reader.GetInt32(reader.GetOrdinal("responseid")) };
                            feedback.Status = new Setting { Id = reader.GetInt32(reader.GetOrdinal("statusid")) };
                            feedback.Subject = reader.IsDBNull(reader.GetOrdinal("subject"))
                                                   ? string.Empty
                                                   : reader.GetString(reader.GetOrdinal("subject"));
                            feedback.Description = reader.IsDBNull(reader.GetOrdinal("description"))
                                                       ? string.Empty
                                                       : reader.GetString(reader.GetOrdinal("description"));
                            feedback.Solution = reader.IsDBNull(reader.GetOrdinal("solution"))
                                                    ? string.Empty
                                                    : reader.GetString(reader.GetOrdinal("solution"));
                            feedback.Review = reader.IsDBNull(reader.GetOrdinal("review"))
                                                  ? string.Empty
                                                  : reader.GetString(reader.GetOrdinal("review"));

                            var reviewerId = reader.IsDBNull(reader.GetOrdinal("reviewerid"))
                                                 ? 0
                                                 : reader.GetInt32(reader.GetOrdinal("reviewerid"));
                            if (reviewerId > 0)
                            {
                                feedback.Reviewer = new User
                                {
                                    Id = reviewerId,
                                    Username =
                                        reader.IsDBNull(reader.GetOrdinal("reviewername"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("reviewername"))
                                };

                                feedback.SolvedDate = DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("solvedate")));
                            }
                            else
                            {
                                feedback.Reviewer = new User
                                {
                                    Id = 0,
                                    Username = string.Empty
                                };
                            }

                            feedback.RelatedCustomer = new Customer
                            {
                                Id = 0,
                                UserName =
                                    reader.IsDBNull(reader.GetOrdinal("relatedCustomer"))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("relatedCustomer"))
                            };

                            lstFeedbacks.Add(feedback);
                        }
                    }
                }
            }

            return lstFeedbacks;
        }

        /// <summary>
        /// Selects the top feedback by customer identifier.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="selectTop">The select top.</param>
        /// <returns>
        /// The top feedback by customer identifier.
        /// </returns>
        public IEnumerable<Feedback> SelTopFeedbackByCustId(int custId, int selectTop)
        {
            var lstFeedbacks = new List<Feedback>();
            using (var obj = new ObjectDb("FeedbackSelTopFeedbackByCustId", 500))
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("top", selectTop);
                obj.AssignParamValues(param);
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        Feedback feedback;
                        while (reader.Read())
                        {
                            feedback = GetFeedbackInformation(reader);
                            feedback.IsReviewed = Converter.ToBool(reader["isreviewed"]);
                            feedback.CustomerInfo = GetCustomerInformation4Report(reader);
                            feedback.RelatedCustomer = GetRealtedCustomer4Report(reader);

                            lstFeedbacks.Add(feedback);
                        }
                    }
                }
            }

            return lstFeedbacks;
        }

        /// <summary>
        /// Selects the top feedback.
        /// </summary>
        /// <param name="selectTop">The select top.</param>
        /// <returns>
        /// The top feedback.
        /// </returns>
        public IEnumerable<Feedback> SelTopFeedback(int selectTop)
        {
            var lstFeedbacks = new List<Feedback>();
            using (var obj = new ObjectDb("FeedbackSelTopFeedback", 500))
            {
                obj.AddInParameter("top", DbType.Int32, selectTop);
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        Feedback feedback;
                        while (reader.Read())
                        {
                            feedback = GetFeedbackInformation(reader);
                            feedback.IsReviewed = Converter.ToBool(reader["isreviewed"]);
                            feedback.CustomerInfo = GetCustomerInformation4Report(reader);
                            feedback.RelatedCustomer = GetRealtedCustomer4Report(reader);

                            lstFeedbacks.Add(feedback);
                        }
                    }
                }
            }

            return lstFeedbacks;
        }

        /// <summary>
        /// Gets the IBC daily summary report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="siteType">Type of the site.</param>
        /// <returns>The IBC daily summary report.</returns>
        public DataSet GetIBCDailySummaryReport(DateTime fromDate, DateTime toDate, Enumerations.ReportTypes siteType)
        {
            var result = new DataSet();

            using (var obj = new ObjectDb("dailysummaryreport", 500))
            {
                var param = new Hashtable
                    {
                        { "fromdate", DateTimeUtilize.ChangeTimeToServerForReportBegin(fromDate) },
                        { "todate", DateTimeUtilize.ChangeTimeToServerForReportEnd(toDate) },
                        { "sitetype", (int)siteType }
                    };
                obj.AssignParamValues(param);
                result = obj.ExecuteDataSet();
            }

            return result;
        }

        /// <summary>
        /// Gets the IBC daily detail report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="siteType">Type of the site.</param>
        /// <returns>The IBC daily detail report.</returns>
        public DataSet GetIBCDailyDetailReport(DateTime fromDate, DateTime toDate, Enumerations.ReportTypes siteType)
        {
            var result = new DataSet();
            using (var obj = new ObjectDb("dailydetailreport", 1200))
            {
                var param = new Hashtable
                    {
                        { "fromdate", DateTimeUtilize.ChangeTimeToServerForReportBegin(fromDate) },
                        { "todate", DateTimeUtilize.ChangeTimeToServerForReportEnd(toDate) },
                        { "sitetype", (int)siteType }
                    };
                obj.AssignParamValues(param);
                result = obj.ExecuteDataSet();
            }

            return result;
        }

        /// <summary>
        /// Gets the licensee weekly summary report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <returns>The licensee weekly summary report.</returns>
        public DataSet GetLicenseeWeeklySummaryReport(DateTime fromDate, DateTime toDate)
        {
            var result = new DataSet();
            using (var obj = new ObjectDb("dailylicenseesummaryreport", 500))
            {
                var param = new Hashtable
                    {
                        { "fromdate", DateTimeUtilize.ChangeTimeToServerForReportBegin(fromDate) },
                        { "todate", DateTimeUtilize.ChangeTimeToServerForReportEnd(toDate) },
                    };
                obj.AssignParamValues(param);
                result = obj.ExecuteDataSet();
            }

            return result;
        }

        /// <summary>
        /// Gets the licensee weekly detail report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <returns>The licensee weekly detail report.</returns>
        public DataSet GetLicenseeWeeklyDetailReport(DateTime fromDate, DateTime toDate)
        {
            var result = new DataSet();
            using (var obj = new ObjectDb("dailylicenseedetailreport", 1200))
            {
                var param = new Hashtable
                    {
                        { "fromdate", DateTimeUtilize.ChangeTimeToServerForReportBegin(fromDate) },
                        { "todate", DateTimeUtilize.ChangeTimeToServerForReportEnd(toDate) },
                    };
                obj.AssignParamValues(param);
                result = obj.ExecuteDataSet();
            }

            return result;
        }

        /// <summary>
        /// Gets the casino daily summary report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <returns>The casino daily summary report.</returns>
        public DataSet GetCasinoDailySummaryReport(DateTime fromDate, DateTime toDate, int reportType)
        {
            var result = new DataSet();
            using (var obj = new ObjectDb("casinodailysummary", 1000))
            {
                var param = new Hashtable
                    {
                        { "fromdate", DateTimeUtilize.ChangeTimeToServerForReportBegin(fromDate) },
                        { "todate", DateTimeUtilize.ChangeTimeToServerForReportEnd(toDate) },
                        { "type", reportType }
                    };
                obj.AssignParamValues(param);
                result = obj.ExecuteDataSet();
            }

            return result;
        }

        /// <summary>
        /// Gets the casino daily detail report.
        /// </summary>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date value.</param>
        /// <returns>The casino daily detail report.</returns>
        public DataSet GetCasinoDailyDetailReport(DateTime fromDate, DateTime toDate)
        {
            var result = new DataSet();
            using (var obj = new ObjectDb("casinodailydetail", 1200))
            {
                var param = new Hashtable
                    {
                        { "fromdate", DateTimeUtilize.ChangeTimeToServerForReportBegin(fromDate) },
                        { "todate", DateTimeUtilize.ChangeTimeToServerForReportEnd(toDate) },
                    };
                obj.AssignParamValues(param);
                result = obj.ExecuteDataSet();
            }

            return result;
        }

        /// <summary>
        /// Gets the feedback subject template items.
        /// </summary>
        /// <param name="templateId">The template identifier.</param>
        /// <returns>The feedback subject template items.</returns>
        public DataSet GetFeedbackSubjectTemplateItems(int templateId = 0)
        {
            var datasetResut = new DataSet();

            using (var obj = new ObjectDb("Setting_FeedbackTemplateSubject_Get", 500))
            {
                var param = new Hashtable { { "templateid", templateId } };
                obj.AssignParamValues(param);
                datasetResut = obj.ExecuteDataSet();
            }

            return datasetResut;
        }

        /// <summary>
        /// Adds the subject feedback template.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns>Add result.</returns>
        public Result AddSubjectFeedbackTemplate(string subject)
        {
            var result = new Result();
            try
            {
                using (var obj = new ObjectDb("FeedSubjectTemplateInsert", 500))
                {
                    var param = new Hashtable { { "subject", subject }, { "errcode", 0 }, { "errmsg", string.Empty } };
                    obj.AssignParamValues(param);
                    obj.ExecuteNonQuery();

                    result.ErrCode = Convert.ToInt32(obj.GetParameterValue("errcode"));
                    result.ErrMsg = Convert.ToString(obj.GetParameterValue("errmsg"));
                }
            }
            catch (Exception ex)
            {
                result.ErrCode = 300;
                result.ErrMsg = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Edits the subject feedback template.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="newSubject">The new subject.</param>
        /// <returns>Edit result.</returns>
        public Result EditSubjectFeedbackTemplate(int id, string newSubject)
        {
            var result = new Result();
            try
            {
                using (var obj = new ObjectDb("FeedSubjectTemplateUpdate", 500))
                {
                    var param = new Hashtable { { "templateid", id }, { "subject", newSubject }, { "errcode", 0 }, { "errmsg", string.Empty } };
                    obj.AssignParamValues(param);
                    obj.ExecuteNonQuery();

                    result.ErrCode = Convert.ToInt32(obj.GetParameterValue("errcode"));
                    result.ErrMsg = Convert.ToString(obj.GetParameterValue("errmsg"));
                }
            }
            catch (Exception ex)
            {
                result.ErrCode = 300;
                result.ErrMsg = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Deletes the subject feedback template.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Delete result.</returns>
        public Result DeleteSubjectFeedbackTemplate(int id)
        {
            var result = new Result();
            try
            {
                using (var obj = new ObjectDb("FeedSubjectTemplateDelete", 500))
                {
                    var param = new Hashtable { { "templateid", id }, { "errcode", 0 }, { "errmsg", string.Empty } };
                    obj.AssignParamValues(param);
                    obj.ExecuteNonQuery();

                    result.ErrCode = Convert.ToInt32(obj.GetParameterValue("errcode"));
                    result.ErrMsg = Convert.ToString(obj.GetParameterValue("errmsg"));
                }
            }
            catch (Exception ex)
            {
                result.ErrCode = 300;
                result.ErrMsg = ex.Message;
            }

            return result;
        }

        public DataSet AutoSuggestionCustomer(string userName)
        {
            using (var obj = new ObjectDb("AutoSuggestionCustomer", 500))
            {
                var param = new Hashtable();
                param.Add("username", userName);
                obj.AssignParamValues(param);
                return obj.ExecuteDataSet();
            }
        }

        #region Helper methods

        /// <summary>
        /// Gets the feedback information.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The feedback object.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private Feedback GetFeedbackInformation(IDataReader reader)
        {
            var feedback = new Feedback
            {
                Id = Converter.ToInt(reader["feedbackid"]),
                Customer = new Customer
                {
                    Id = Converter.ToInt(reader["custid"]),
                    UserName = Converter.ToString2(reader["username"])
                },
                Creator = new User
                {
                    Id = Converter.ToInt(reader["creatorid"]),
                    Username = Converter.ToString2(reader["creatorname"]),
                },
                FeedbackDate = DateTimeUtilize.ChangeTimeZoneToClient(Converter.ToDateTime(reader["feedbackdate"])),
                ResolutionTime = DateTimeUtilize.ChangeTimeZoneToClient(Converter.ToDateTime(reader["resolutiondate"])),
                CreateDate = DateTimeUtilize.ChangeTimeZoneToClient(Converter.ToDateTime(reader["createdate"])),
                SolvedDate = DateTimeUtilize.ChangeTimeZoneToClient(Converter.ToDateTime(reader["solvedate"])),
                Communication = new Setting
                {
                    Id = Converter.ToInt(reader["communicationid"])
                },
                CsExtension = new Setting
                {
                    Id = Converter.ToInt(reader["stationid"])
                },
                Category = new Setting
                {
                    Id = Converter.ToInt(reader["maincategoryid"])
                },
                SubCategory = new Setting
                {
                    Id = Converter.ToInt(reader["subcategoryid"])
                },
                Site = new Setting
                {
                    Id = Converter.ToInt(reader["siteid"])
                },
                Role = new Setting
                {
                    Id = Converter.ToInt(reader["roleid"])
                },
                Product = new Setting
                {
                    Id = Converter.ToInt(reader["productid"])
                },
                BetType = new Setting
                {
                    Id = Converter.ToInt(reader["bettypeid"])
                },
                Response = new Setting
                {
                    Id = Converter.ToInt(reader["responseid"])
                },
                Status = new Setting
                {
                    Id = Converter.ToInt(reader["statusid"])
                },
                Description = Converter.ToString2(reader["description"]),
                Solution = Converter.ToString2(reader["solution"]),
                Review = Converter.ToString2(reader["review"]),
                Reviewer = new User
                {
                    Id = Converter.ToInt(reader["reviewerid"]),
                    Username = Converter.ToString2(reader["reviewername"])
                },
                Subject = Converter.ToString2(reader["subject"]),
                Country = new Country 
                {
                    Id = Converter.ToInt(reader["countryid"])
                }
            };

            //feedback.IsReviewed = feedback.Reviewer.Id > 0;
            //feedback.IsReviewed = Converter.ToBool(reader["isreviewed"]);
            return feedback;
        }

        private Customer GetRealtedCustomer(IDataReader reader)
        {
            return new Customer
            {
                UserName = Converter.ToString2(reader["relatedcustomer"])
            };
        }

        private Customer GetRealtedCustomer4Report(IDataReader reader)
        {
            return new Customer
            {
                UserName = Converter.ToBool(reader["isrelated"])
                    ? Converter.ToString2(reader["username"])
                    : Converter.ToString2(reader["relatedcustomer"])
            };
        }

        /// <summary>
        /// Gets the customer information.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The customer info object.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        private CustomerInfo GetCustomerformation(IDataReader reader)
        {
            var customerInfo = GetCustomerInformation4Report(reader);
            customerInfo.ContactNumber2 = new PhoneInfo
            {
                PhoneNumber = Converter.ToString2(reader["Phone2"]),
                CountryCode = Converter.ToString2(reader["Phone2CountryCode"]),
                AreaCode = Converter.ToString2(reader["Phone2AreaCode"])
            };
            customerInfo.ContactNumber3 = new PhoneInfo
            {
                PhoneNumber = Converter.ToString2(reader["Phone3"]),
                CountryCode = Converter.ToString2(reader["Phone3CountryCode"]),
                AreaCode = Converter.ToString2(reader["Phone3AreaCode"])
            };

            customerInfo.PassportNumber = Converter.ToString2(reader["Passport"]);
            customerInfo.BirthDay = Converter.ToDateTime(reader["Birthday"]);
            customerInfo.Site = Converter.ToInt(reader["SiteId"]);
            customerInfo.BettingCode = Converter.ToString2(reader["RefCode1"]);
            customerInfo.RefCodes2 = Converter.ToString2(reader["RefCode2"]);
            customerInfo.IsVIP = Converter.ToBool(reader["IsVIP"]);
            customerInfo.PIC1 = Converter.ToString2(reader["PIC1"]);
            customerInfo.PIC2 = Converter.ToString2(reader["PIC2"]);
            customerInfo.PIC3 = Converter.ToString2(reader["PIC3"]);
            customerInfo.PIC4 = Converter.ToString2(reader["PIC4"]);

            return customerInfo;
        }

        /// <summary>
        /// Gets the customer information.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The customer info object.</returns>
        private CustomerInfo GetCustomerInformation4Report(IDataReader reader)
        {
            return new CustomerInfo
            {
                Title = new Setting
                {
                    Id = Converter.ToInt(reader["titleid"]),
                },
                FirstName = Converter.ToString2(reader["firstname"]),
                LastName = Converter.ToString2(reader["lastname"]),
                ContactNumber1 = new PhoneInfo
                {
                    AreaCode = Converter.ToString2(reader["phoneareacode"]),
                    CountryCode = Converter.ToString2(reader["phonecountrycode"]),
                    PhoneNumber = Converter.ToString2(reader["phone"])
                },
                Mobile = new PhoneInfo
                {
                    AreaCode = Converter.ToString2(reader["mobilephoneareacode"]),
                    CountryCode = Converter.ToString2(reader["mobilephonecountrycode"]),
                    PhoneNumber = Converter.ToString2(reader["mobilephone"])
                },
                Fax = new FaxInfo
                {
                    AreaCode = Converter.ToString2(reader["faxareacode"]),
                    CountryCode = Converter.ToString2(reader["faxcountrycode"]),
                    FaxNumber = Converter.ToString2(reader["fax"])
                },
                Email = Converter.ToString2(reader["email"]),
                Skype = Converter.ToString2(reader["skype"]),
                MSN = Converter.ToString2(reader["msn"]),
                Country = Converter.ToInt(reader["countryid"])
            };
        }

        /// <summary>
        /// Gets the customer language.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The language list.</returns>
        private IList<int> GetCustomerLanguages(IDataReader reader)
        {
            IList<int> languages = new List<int>();
            while (reader.Read())
            {
                languages.Add(Converter.ToInt(reader["LanguageId"]));
            }

            return languages;
        }

        #endregion
    }
}

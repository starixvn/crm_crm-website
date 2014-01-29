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
            var feedback = new Feedback();
            using (var obj = new ObjectDb("FeedbackSelByID", 500))
            {
                obj.AddInParameter("feedbackid", DbType.Int32, id);
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            feedback.Id = reader.GetInt32(reader.GetOrdinal("feedbackid"));

                            if (!reader.IsDBNull(reader.GetOrdinal("custid")))
                            {
                                feedback.Customer = new Customer
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("custid")),
                                    UserName = reader.GetString(reader.GetOrdinal("username")),
                                    Pic1 = reader.GetString(reader.GetOrdinal("Pic1")),
                                    Pic2 = reader.GetString(reader.GetOrdinal("Pic2")),
                                    Pic3 = reader.GetString(reader.GetOrdinal("Pic3"))
                                };
                            }

                            feedback.CustomerInfo = new CustomerInfo
                            {
                                Title = new Setting
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("titleid"))
                                },
                                Email = reader.IsDBNull(reader.GetOrdinal("email")) ? string.Empty : reader.GetString(reader.GetOrdinal("email")),
                                Fax = reader.IsDBNull(reader.GetOrdinal("fax")) ? string.Empty : reader.GetString(reader.GetOrdinal("fax")),
                                FirstName = reader.IsDBNull(reader.GetOrdinal("firstname")) ? string.Empty : reader.GetString(reader.GetOrdinal("firstname")),
                                LastName = reader.IsDBNull(reader.GetOrdinal("lastname")) ? string.Empty : reader.GetString(reader.GetOrdinal("lastname")),
                                MobilePhone = reader.IsDBNull(reader.GetOrdinal("mobilephone")) ? string.Empty : reader.GetString(reader.GetOrdinal("mobilephone")),
                                Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? string.Empty : reader.GetString(reader.GetOrdinal("phone")),
                                Skype = reader["Skype"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Skype")) : string.Empty,
                                MSN = reader["msn"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("msn")) : string.Empty
                            };

                            feedback.Language = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("languageid"))
                            };

                            feedback.Country = new Country
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("countryid"))
                            };

                            feedback.Creator = new User
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("creatorid")),
                                Username = reader.GetString(reader.GetOrdinal("creatorname"))
                            };

                            if (reader["FeedbackDate"] != DBNull.Value)
                            {
                                feedback.FeedbackDate = DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("feedbackdate")));
                            }

                            feedback.ResolutionTime = DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("resolutiondate")));

                            if (reader["SolveDate"] != DBNull.Value)
                            {
                                feedback.SolvedDate = DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("solvedate")));
                            }

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
                                Id = reader.GetInt32(reader.GetOrdinal("SiteID"))
                            };

                            feedback.Role = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("roleid"))
                            };

                            feedback.Product = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("productid"))
                            };

                            feedback.BetType = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("bettypeid"))
                            };

                            feedback.Response = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("responseid"))
                            };

                            feedback.Status = new Setting
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("statusid"))
                            };

                            feedback.Description = reader.IsDBNull(reader.GetOrdinal("description"))
                                ? string.Empty
                                : reader.GetString(reader.GetOrdinal("description"));

                            feedback.Subject = reader.IsDBNull(reader.GetOrdinal("subject"))
                                  ? string.Empty
                                  : reader.GetString(reader.GetOrdinal("subject"));

                            feedback.Solution = reader.IsDBNull(reader.GetOrdinal("solution"))
                                ? string.Empty
                                : reader.GetString(reader.GetOrdinal("solution"));

                            feedback.Review = reader.IsDBNull(reader.GetOrdinal("review"))
                                ? string.Empty
                                : reader.GetString(reader.GetOrdinal("review"));

                            var reviewerId = reader.IsDBNull(reader.GetOrdinal("reviewerid"))
                                ? 0
                                : Convert.ToInt32(reader["ReviewerID"]);

                            if (reviewerId > 0)
                            {
                                feedback.IsReviewed = true;
                                feedback.Reviewer = new User
                                {
                                    Id = reviewerId,
                                    Username = reader["ReviewerName"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("reviewername")) : string.Empty
                                };
                            }

                            feedback.RelatedCustomer = new Customer
                            {
                                Id = 0,
                                UserName = reader.GetString(reader.GetOrdinal("relatedCustomer"))
                            };
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
                using (var obj = new ObjectDb("GuestFeedbackIns", 500))
                {
                    var param = new Hashtable();
                    param.Add("userid", entity.Creator.Id);
                    param.Add("username", entity.Customer.UserName);
                    param.Add("creatorid", entity.Creator.Id);
                    param.Add("titleid", entity.CustomerInfo.Title.Id);
                    param.Add("firstname", !string.IsNullOrEmpty(entity.CustomerInfo.FirstName) ? entity.CustomerInfo.FirstName : string.Empty);
                    param.Add("lastname", !string.IsNullOrEmpty(entity.CustomerInfo.LastName) ? entity.CustomerInfo.LastName : string.Empty);
                    param.Add("phone", !string.IsNullOrEmpty(entity.CustomerInfo.Phone) ? entity.CustomerInfo.Phone : string.Empty);
                    param.Add("mobilephone", !string.IsNullOrEmpty(entity.CustomerInfo.MobilePhone) ? entity.CustomerInfo.MobilePhone : string.Empty);
                    param.Add("fax", !string.IsNullOrEmpty(entity.CustomerInfo.Fax) ? entity.CustomerInfo.Fax : string.Empty);

                    param.Add("email", !string.IsNullOrEmpty(entity.CustomerInfo.Email) ? entity.CustomerInfo.Email : string.Empty);
                    param.Add("skype", !string.IsNullOrEmpty(entity.CustomerInfo.Skype) ? entity.CustomerInfo.Skype : string.Empty);
                    param.Add("msn", !string.IsNullOrEmpty(entity.CustomerInfo.MSN) ? entity.CustomerInfo.MSN : string.Empty);

                    param.Add("languageid", entity.Language.Id);
                    param.Add("countryid", entity.Country.Id);
                    param.Add("feedbackdate", DateTimeUtilize.ChangeTimeZoneToServer(entity.FeedbackDate));
                    param.Add("resolutiondate", DateTimeUtilize.ChangeTimeZoneToServer(entity.ResolutionTime));

                    param.Add("communicationid", entity.Communication.Id);
                    param.Add("stationid", entity.CsExtension.Id);
                    param.Add("maincategoryid", entity.Category.Id);
                    param.Add("subcategoryid", entity.SubCategory.Id);
                    param.Add("siteid", entity.Site.Id);
                    param.Add("roleid", entity.Role.Id);
                    param.Add("productid", entity.Product.Id);
                    param.Add("bettypeid", entity.BetType.Id);
                    param.Add("responseid", entity.Response.Id);
                    param.Add("statusid", entity.Status.Id);

                    param.Add("relatedcustid", entity.RelatedCustomer.UserName);

                    param.Add("subject", entity.Subject);
                    param.Add("description", entity.Description);
                    param.Add("solution", entity.Solution);
                    param.Add("isreviewed", entity.IsReviewed);
                    param.Add("review", entity.Review);
                    param.Add("pic1", entity.Customer.Pic1);
                    param.Add("pic2", entity.Customer.Pic2);
                    param.Add("pic3", entity.Customer.Pic3);
                    if (entity.Reviewer.Id > 0)
                    {
                        param.Add("reviewerid", entity.Reviewer.Id);
                    }

                    param.Add("ip", ip);
                    param.Add("errcode", 4);
                    param.Add("errmsg", 4000);

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
                using (var obj = new ObjectDb("CustomerFeedbackIns", 500))
                {
                    var param = new Hashtable();
                    param.Add("userid", entity.Creator.Id);
                    param.Add("custid", entity.Customer.Id);
                    param.Add("username", entity.Customer.UserName);
                    param.Add("creatorid", entity.Creator.Id);
                    param.Add("titleid", entity.CustomerInfo.Title.Id);
                    param.Add("firstname", !string.IsNullOrEmpty(entity.CustomerInfo.FirstName) ? entity.CustomerInfo.FirstName : string.Empty);
                    param.Add("lastname", !string.IsNullOrEmpty(entity.CustomerInfo.LastName) ? entity.CustomerInfo.LastName : string.Empty);
                    param.Add("phone", !string.IsNullOrEmpty(entity.CustomerInfo.Phone) ? entity.CustomerInfo.Phone : string.Empty);
                    param.Add("mobilephone", !string.IsNullOrEmpty(entity.CustomerInfo.MobilePhone) ? entity.CustomerInfo.MobilePhone : string.Empty);
                    param.Add("fax", !string.IsNullOrEmpty(entity.CustomerInfo.Fax) ? entity.CustomerInfo.Fax : string.Empty);

                    param.Add("email", !string.IsNullOrEmpty(entity.CustomerInfo.Email) ? entity.CustomerInfo.Email : string.Empty);
                    param.Add("skype", !string.IsNullOrEmpty(entity.CustomerInfo.Skype) ? entity.CustomerInfo.Skype : string.Empty);
                    param.Add("msn", !string.IsNullOrEmpty(entity.CustomerInfo.MSN) ? entity.CustomerInfo.MSN : string.Empty);

                    param.Add("languageid", entity.Language.Id);
                    param.Add("countryid", entity.Country.Id);
                    param.Add("feedbackdate", DateTimeUtilize.ChangeTimeZoneToServer(entity.FeedbackDate));
                    param.Add("resolutiondate", DateTimeUtilize.ChangeTimeZoneToServer(entity.ResolutionTime));

                    param.Add("communicationid", entity.Communication.Id);
                    param.Add("stationid", entity.CsExtension.Id);
                    param.Add("maincategoryid", entity.Category.Id);
                    param.Add("subcategoryid", entity.SubCategory.Id);
                    param.Add("siteid", entity.Site.Id);
                    param.Add("roleid", entity.Role.Id);
                    param.Add("productid", entity.Product.Id);
                    param.Add("bettypeid", entity.BetType.Id);
                    param.Add("responseid", entity.Response.Id);
                    param.Add("statusid", entity.Status.Id);

                    param.Add("relatedcustid", entity.RelatedCustomer.UserName);

                    param.Add("subject", entity.Subject);
                    param.Add("description", entity.Description);
                    param.Add("solution", entity.Solution);
                    param.Add("isreviewed", entity.IsReviewed);
                    param.Add("review", entity.Review);
                    param.Add("pic1", entity.Customer.Pic1);
                    param.Add("pic2", entity.Customer.Pic2);
                    param.Add("pic3", entity.Customer.Pic3);
                    if (entity.Reviewer.Id > 0)
                    {
                        param.Add("reviewerid", entity.Reviewer.Id);
                    }

                    param.Add("isoverwrite", entity.IsOverrideCrmInfo);
                    param.Add("ip", ip);
                    param.Add("errcode", 4);
                    param.Add("errmsg", 4000);

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
            using (var obj = new ObjectDb("FeedbackInfoUpd", 500))
            {
                var param = new Hashtable();
                param.Add("feedbackid", entity.Id);
                param.Add("userid", userId);
                param.Add("custid", entity.Customer.Id);
                param.Add("pic1", entity.Customer.Pic1);
                param.Add("pic2", entity.Customer.Pic2);
                param.Add("pic3", entity.Customer.Pic3);
                param.Add("titleid", entity.CustomerInfo.Title.Id);
                param.Add("firstname", !string.IsNullOrEmpty(entity.CustomerInfo.FirstName) ? entity.CustomerInfo.FirstName : string.Empty);
                param.Add("lastname", !string.IsNullOrEmpty(entity.CustomerInfo.LastName) ? entity.CustomerInfo.LastName : string.Empty);
                param.Add("phone", !string.IsNullOrEmpty(entity.CustomerInfo.Phone) ? entity.CustomerInfo.Phone : string.Empty);
                param.Add("mobilephone", !string.IsNullOrEmpty(entity.CustomerInfo.MobilePhone) ? entity.CustomerInfo.MobilePhone : string.Empty);
                param.Add("fax", !string.IsNullOrEmpty(entity.CustomerInfo.Fax) ? entity.CustomerInfo.Fax : string.Empty);

                param.Add("email", !string.IsNullOrEmpty(entity.CustomerInfo.Email) ? entity.CustomerInfo.Email : string.Empty);
                param.Add("skype", !string.IsNullOrEmpty(entity.CustomerInfo.Skype) ? entity.CustomerInfo.Skype : string.Empty);
                param.Add("msn", !string.IsNullOrEmpty(entity.CustomerInfo.MSN) ? entity.CustomerInfo.MSN : string.Empty);

                param.Add("languageid", entity.Language.Id);
                param.Add("countryid", entity.Country.Id);
                param.Add("feedbackdate", DateTimeUtilize.ChangeTimeZoneToServer(entity.FeedbackDate));
                param.Add("resolutiondate", DateTimeUtilize.ChangeTimeZoneToServer(entity.ResolutionTime));
                param.Add("communicationid", entity.Communication.Id);
                param.Add("stationid", entity.CsExtension.Id);
                param.Add("maincategoryid", entity.Category.Id);
                param.Add("subcategoryid", entity.SubCategory.Id);
                param.Add("siteid", entity.Site.Id);
                param.Add("roleid", entity.Role.Id);
                param.Add("productid", entity.Product.Id);
                param.Add("bettypeid", entity.BetType.Id);
                param.Add("responseid", entity.Response.Id);
                param.Add("statusid", entity.Status.Id);

                var relatedUsername = !string.IsNullOrEmpty(entity.RelatedCustomer.UserName) ?
                        (entity.RelatedCustomer.UserName.Equals("_") ?
                        string.Empty :
                        entity.RelatedCustomer.UserName) :
                        null;
                param.Add("relatedcustid", relatedUsername);

                param.Add("subject", entity.Subject);
                param.Add("description", entity.Description);
                param.Add("solution", !string.IsNullOrEmpty(entity.Solution) ? entity.Solution : string.Empty);

                param.Add("updatereviewedmode", entity.UpdateReviewMode);
                param.Add("review", !string.IsNullOrEmpty(entity.Review) ? entity.Review : string.Empty);
                if (entity.Reviewer.Id > 0)
                {
                    param.Add("reviewerid", entity.Reviewer.Id);
                    param.Add("isreviewed", true);
                }
                else
                {
                    param.Add("isreviewed", false);
                }

                param.Add("isoverwrite", entity.IsOverrideCrmInfo);
                param.Add("ip", ip);
                param.Add("errcode", 4);
                param.Add("errmsg", 4000);
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
                                Fax = reader.IsDBNull(reader.GetOrdinal("fax")) ? string.Empty : reader.GetString(reader.GetOrdinal("fax")),
                                FirstName = reader.IsDBNull(reader.GetOrdinal("firstname")) ? string.Empty : reader.GetString(reader.GetOrdinal("firstname")),
                                LastName = reader.IsDBNull(reader.GetOrdinal("lastname")) ? string.Empty : reader.GetString(reader.GetOrdinal("lastname")),
                                MobilePhone = reader.IsDBNull(reader.GetOrdinal("mobilephone")) ? string.Empty : reader.GetString(reader.GetOrdinal("mobilephone")),
                                Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? string.Empty : reader.GetString(reader.GetOrdinal("phone"))
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
                                    Fax =
                                        reader.IsDBNull(reader.GetOrdinal("fax"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("fax")),
                                    FirstName =
                                        reader.IsDBNull(reader.GetOrdinal("firstname"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("firstname")),
                                    LastName =
                                        reader.IsDBNull(reader.GetOrdinal("lastname"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("lastname")),
                                    MobilePhone =
                                        reader.IsDBNull(reader.GetOrdinal("mobilephone"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("mobilephone")),
                                    Phone =
                                        reader.IsDBNull(reader.GetOrdinal("phone"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("phone"))
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
                                Id = reader.IsDBNull(reader.GetOrdinal("creatorid")) ? 0 : reader.GetInt32(reader.GetOrdinal("creatorid")),
                                Username = reader.IsDBNull(reader.GetOrdinal("creatorname")) ? string.Empty : reader.GetString(reader.GetOrdinal("creatorname"))
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

                            feedback.Response = new Setting { Id = reader.GetInt32(reader.GetOrdinal("responseid")) };

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
                                    Fax =
                                        reader.IsDBNull(reader.GetOrdinal("fax"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("fax")),
                                    FirstName =
                                        reader.IsDBNull(reader.GetOrdinal("firstname"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("firstname")),
                                    LastName =
                                        reader.IsDBNull(reader.GetOrdinal("lastname"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("lastname")),
                                    MobilePhone =
                                        reader.IsDBNull(reader.GetOrdinal("mobilephone"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("mobilephone")),
                                    Phone =
                                        reader.IsDBNull(reader.GetOrdinal("phone"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("phone"))
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
                                Fax =
                                    reader.IsDBNull(reader.GetOrdinal("fax"))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("fax")),
                                FirstName =
                                    reader.IsDBNull(reader.GetOrdinal("firstname"))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("firstname")),
                                LastName =
                                    reader.IsDBNull(reader.GetOrdinal("lastname"))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("lastname")),
                                MobilePhone =
                                    reader.IsDBNull(reader.GetOrdinal("mobilephone"))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("mobilephone")),
                                Phone =
                                    reader.IsDBNull(reader.GetOrdinal("phone"))
                                        ? string.Empty
                                        : reader.GetString(reader.GetOrdinal("phone"))
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
        /// <returns>The top feedback by customer identifier.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
             "SP2101:MethodMustNotContainMoreLinesThan",
             Justification = "Reviewed. Suppression is OK here.")]
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
                                    Fax =
                                        reader.IsDBNull(reader.GetOrdinal("fax"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("fax")),
                                    FirstName =
                                        reader.IsDBNull(reader.GetOrdinal("firstname"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("firstname")),
                                    LastName =
                                        reader.IsDBNull(reader.GetOrdinal("lastname"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("lastname")),
                                    MobilePhone =
                                        reader.IsDBNull(reader.GetOrdinal("mobilephone"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("mobilephone")),
                                    Phone =
                                        reader.IsDBNull(reader.GetOrdinal("phone"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("phone"))
                                };

                            feedback.Language = new Setting { Id = reader.GetInt32(reader.GetOrdinal("languageid")) };
                            feedback.Country = new Country { Id = reader.GetInt32(reader.GetOrdinal("countryid")) };

                            feedback.CreateDate = DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("createdate")));

                            feedback.Creator = new User
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("creatorid")),
                                    Username = reader.GetString(reader.GetOrdinal("creatorname"))
                                };
                            feedback.FeedbackDate =
                                DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("feedbackdate")));
                            feedback.ResolutionTime = !reader.IsDBNull(reader.GetOrdinal("resolutiondate"))
                                                          ? DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("resolutiondate")))
                                                          : null;

                            feedback.Communication = new Setting { Id = reader.GetInt32(reader.GetOrdinal("communicationid")) };
                            feedback.CsExtension = new Setting { Id = reader.GetInt32(reader.GetOrdinal("StationID")) };
                            feedback.Category = new Setting { Id = reader.GetInt32(reader.GetOrdinal("maincategoryid")), };
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

                            var responseId = reader.GetInt32(reader.GetOrdinal("responseid"));
                            feedback.Response = new Setting { Id = responseId };
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
                                                : reader.GetString(reader.GetOrdinal("reviewername")),
                                    };
                                feedback.SolvedDate =
                                 DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("solvedate")));
                            }
                            else
                            {
                                feedback.Reviewer = new User { Id = 0, Username = string.Empty };
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
                    }
                }
            }

            return lstFeedbacks;
        }

        /// <summary>
        /// Selects the top feedback.
        /// </summary>
        /// <param name="selectTop">The select top.</param>
        /// <returns>The top feedback.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
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
                                    Fax =
                                        reader.IsDBNull(reader.GetOrdinal("fax"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("fax")),
                                    FirstName =
                                        reader.IsDBNull(reader.GetOrdinal("firstname"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("firstname")),
                                    LastName =
                                        reader.IsDBNull(reader.GetOrdinal("lastname"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("lastname")),
                                    MobilePhone =
                                        reader.IsDBNull(reader.GetOrdinal("mobilephone"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("mobilephone")),
                                    Phone =
                                        reader.IsDBNull(reader.GetOrdinal("phone"))
                                            ? string.Empty
                                            : reader.GetString(reader.GetOrdinal("phone"))
                                };

                            feedback.Language = new Setting { Id = reader.GetInt32(reader.GetOrdinal("languageid")) };
                            feedback.Country = new Country { Id = reader.GetInt32(reader.GetOrdinal("countryid")) };
                            feedback.CreateDate = DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("createdate")));
                            feedback.Creator = new User
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("creatorid")),
                                    Username = reader.GetString(reader.GetOrdinal("creatorname"))
                                };

                            feedback.FeedbackDate =
                                DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("feedbackdate")));
                            feedback.ResolutionTime = !reader.IsDBNull(reader.GetOrdinal("resolutiondate"))
                                                          ? DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("resolutiondate")))
                                                          : null;
                            feedback.Communication = new Setting { Id = reader.GetInt32(reader.GetOrdinal("communicationid")) };
                            feedback.CsExtension = new Setting { Id = reader.GetInt32(reader.GetOrdinal("StationID")) };
                            feedback.Category = new Setting { Id = reader.GetInt32(reader.GetOrdinal("maincategoryid")), };
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

                            var responseId = reader.GetInt32(reader.GetOrdinal("responseid"));
                            feedback.Response = new Setting { Id = responseId };

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
                                                : reader.GetString(reader.GetOrdinal("reviewername")),
                                    };
                                feedback.SolvedDate =
                                  DateTimeUtilize.ChangeTimeZoneToClient(reader.GetDateTime(reader.GetOrdinal("solvedate")));
                            }
                            else
                            {
                                feedback.Reviewer = new User { Id = 0, Username = string.Empty };
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
                    var param = new Hashtable { { "subject", subject }, { "errcode", 4 }, { "errmsg", 4000 } };
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
                    var param = new Hashtable { { "templateid", id }, { "subject", newSubject }, { "errcode", 4 }, { "errmsg", 4000 } };
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
                    var param = new Hashtable { { "templateid", id }, { "errcode", 4 }, { "errmsg", 4000 } };
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
    }
}

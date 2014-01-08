// --------------------------------------------------------------------------
// <copyright file="ICustomerService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Service.Interface
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.CRM.Entities;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public interface ICustomerService
    {
        Customer GetCustomerInfoById(int custId);

        Customer GetCustomerInfoByUserName(string userName, bool isGuest);

        List<Customer> GetListUpline(int custId);

        List<RelatedCustomer> VerifyUserName(string userName);

        Result UpdateCustomerInfo(Customer customer, bool isUpdateUserName, string ip);

        DataTable GetAutoSearch(int userID, string userName, bool isGuest);

        Result UpdateAutoCustomerSearch(int userID, int custID, string userName);

        MongoCursor<BsonDocument> CustomerAutoSuggest(string customerName, bool isGuest);

        bool UpdateCustomerSearchCountValue(int custId, string customerName);
    }
}

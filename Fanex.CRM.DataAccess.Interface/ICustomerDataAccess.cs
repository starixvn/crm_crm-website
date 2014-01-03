// --------------------------------------------------------------------------
// <copyright file="ICustomerDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.DataAccess.Interface
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.CRM.Entities;
    using MongoDB.Driver;

    public interface ICustomerDataAccess
    {
        Customer FindBy(int custId);

        Customer FindBy(string userName, bool isGuest);

        List<RelatedCustomer> VerifyUserName(string userName);

        List<Customer> FindAllUpline(int custId);

        bool UpdateCustomerInfo(Customer customer, bool isUpdateUserName, string ip, ref Result result);

        DataSet QueryUserName(int userID, string userName, bool isGuest);

        bool UpdateCustomerSearch(int userID, int custID, string userName, ref Result result);

        MongoCursor<MongoDB.Bson.BsonDocument> CustomerAutoSuggest(string customerName, bool isGuest);

        bool UpdateCustomerSearchCountValue(int custId, string customerName);

        void AddNewCustomer(int customerId, string customerName);
    }
}

// --------------------------------------------------------------------------
// <copyright file="CustomerRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Customer.Repositories.Implements
{
    using Fanex.CRM.Customer.Criteria;
    using Fanex.CRM.Customer.Models;
    using Fanex.CRM.Customer.Repositories.Interfaces;
    using Fanex.CRM.Infrastructure.Entities;
    using Fanex.Data;
    using System;

    /// <summary>
    /// Customer Repository.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        public CustomerInfo GetCustomerInfoByUsername(GetCustomerInfoByUsernameCriteria criteria)
        {
            CustomerInfo customer = null;
            using (var objectDB = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDB.AssignParamValues(criteria.GetSPParams());
                using (var reader = objectDB.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            customer = new CustomerInfo
                            {
                                Title = new BaseModel
                                {
                                    Id = reader["TitleID"] != DBNull.Value ?
                                        reader.GetInt32(reader.GetOrdinal("TitleID")) :
                                        0,
                                    Name = string.Empty
                                },
                                

                            };
                        }
                    }
                }
            }

            return customer;
        }
    }
}

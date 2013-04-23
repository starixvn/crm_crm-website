namespace Fanex.CRM.DataAccess.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.CRM.DataAccess.Interface;
    using Fanex.CRM.Entities;
    using Fanex.Data;

    public class SecurityDataAccess: ISecurityDataAccess
    {
        public List<Security> GetCustomerRoles(int custId)
        {
            var listCustRole = new List<Security>();
            using (var obj = new ObjectDb("FileNameSelByCustID"))
            {
                obj.AddInParameter("custid", DbType.Int32, custId);
                using (var reader = obj.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var security = new Security
                                {
                                    FileNames =
                                        reader["filenames"] != DBNull.Value
                                            ? reader.GetString(reader.GetOrdinal("filenames"))
                                            : string.Empty,
                                    FuncName =
                                        reader["funcname"] != DBNull.Value
                                            ? reader.GetString(reader.GetOrdinal("funcname"))
                                            : string.Empty,
                                    ActualPolicy =
                                        reader["actual_policy"] != DBNull.Value
                                            ? reader.GetByte(reader.GetOrdinal("actual_policy"))
                                            : 0
                                };

                            listCustRole.Add(security);
                        }
                    }
                }
            }
            return listCustRole;
        }
    }
}

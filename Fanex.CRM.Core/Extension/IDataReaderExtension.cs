// --------------------------------------------------------------------------
// <copyright file="IDataReaderExtension.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.CRM.Infrastructure.Extension
{
    using System;
    using System.Data;

    /// <summary>
    /// Data Reader Extension.
    /// </summary>
    public static class IDataReaderExtension
    {
        public static string GetStringNotNull(this IDataReader reader, string columnName)
        {
            return reader[columnName] != DBNull.Value && reader[columnName] != null ?
                reader.GetString(reader.GetOrdinal(columnName)) :
                string.Empty;
        }

        public static int GetInt32NotNull(this IDataReader reader, string columnName)
        {
            return reader[columnName] != DBNull.Value ?
                reader.GetInt32(reader.GetOrdinal(columnName)) :
                0;
        }

        public static int GetInt16NotNull(this IDataReader reader, string columnName)
        {
            return reader[columnName] != DBNull.Value ?
                reader.GetInt16(reader.GetOrdinal(columnName)) :
                0;
        }

        public static DateTime GetDateTimeNotNull(this IDataReader reader, string columnName)
        {
            return reader[columnName] != DBNull.Value ?
                reader.GetDateTime(reader.GetOrdinal(columnName)) :
                DateTime.Now;
        }

        public static bool GetBooleanNotNull(this IDataReader reader, string columnName)
        {
            return reader[columnName] != DBNull.Value ?
                reader.GetBoolean(reader.GetOrdinal(columnName)) :
                false;
        }

        public static byte GetByteNotNull(this IDataReader reader, string columnName)
        {
            return reader[columnName] != DBNull.Value ?
                reader.GetByte(reader.GetOrdinal(columnName)) :
                (byte)0;
        }
    }
}

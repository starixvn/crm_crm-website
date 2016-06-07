// --------------------------------------------------------------------------
// <copyright file="CasinoReportRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kan.Nguyen" date="2014.07.30">Created</change>
// </history>

namespace Fanex.CRM.Report.Repositories.CasinoReport
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common;
    using Fanex.CRM.Report.Criteria.CasinoReport;
    using Fanex.CRM.Report.Entities.CasinoReport;
    using Fanex.CRM.Report.Models.CasinoReport;
    using Fanex.CRM.Report.Resources;
    using Fanex.Data;

    /// <summary>
    /// Casino Report Repository.
    /// </summary>
    public class CasinoReportRepository : ICasinoReportRepository
    {
        public CasinoDailySummary GetCasinoDailySummary(GetCasinoSummaryCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    IList<CategoryCasinoGroup> categoryCasinoGroups = new List<CategoryCasinoGroup>();
                    IEnumerable<CasinoGroup> casinoGroups = null;
                    int fieldCount = 0;

                    while (reader.Read())
                    {
                        if (fieldCount == 0)
                        {
                            fieldCount = reader.FieldCount;
                            casinoGroups = ReadCasinoGroups(reader, fieldCount);
                        }

                        var categoryCasinoGroup = new CategoryCasinoGroup(Converter.ToString2(reader["Maincategory"]));
                        categoryCasinoGroups.Add(categoryCasinoGroup);
                        foreach (var casino in casinoGroups)
                        {
                            var casinoGroup = new CasinoGroup(casino.Name);
                            categoryCasinoGroup.CasinoGroups.Add(casinoGroup);
                            foreach (var comm in casino.Communications)
                            {
                                var value = Converter.ToInt(reader[casino.Name + "_" + comm.Name]);
                                casinoGroup.Communications.Add(new CommunicationValue(comm.Name, value));

                                // Calculate total.
                                comm.Value += value;
                            }
                        }
                    }

                    return new CasinoDailySummary
                    {
                        CategoryCasinoGroups = categoryCasinoGroups,
                        Total = new CategoryCasinoGroup(Labels.Grand_Total)
                        {
                            CasinoGroups = casinoGroups.ToList() ?? new List<CasinoGroup>()
                        }
                    };
                }
            }
        }

        public IEnumerable<CasinoDetailEntity> GetCasinoDetails(GetCasinoDetailCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<CasinoDetailEntity>(criteria.GetSpParams()).ToList();
            }
        }

        public CasinoWeeklySummary GetCasinoWeeklySummary(GetCasinoSummaryCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    return ReadCasinoWeeklySummary(reader);
                }
            }
        }

        private static IEnumerable<CasinoGroup> ReadCasinoGroups(IDataReader reader, int fieldCount)
        {
            var casinoGroupHash = new Dictionary<string, CasinoGroup>();
            for (int i = 0; i < fieldCount; i++)
            {
                var columns = reader.GetName(i).Split('_');
                if (columns.Length != 2)
                {
                    continue;
                }

                CasinoGroup casinoGroup;
                if (!casinoGroupHash.TryGetValue(columns[0], out casinoGroup))
                {
                    casinoGroup = new CasinoGroup(columns[0]);
                    casinoGroupHash.Add(columns[0], casinoGroup);
                }

                casinoGroup.Communications.Add(new CommunicationValue(columns[1], 0));
            }

            return casinoGroupHash.Values.ToList();
        }

        private static CasinoWeeklySummary ReadCasinoWeeklySummary(IDataReader reader)
        {
            var weeklySummary = new CasinoWeeklySummary
            {
                GameTypes = new List<string>(),
                WeeklyCategories = new List<CasinoWeeklyCategory>(),
                GrandTotal = new CasinoWeeklyCategory
                {
                    Name = Labels.Grand_Total,
                    GameTypeValues = new List<int>()
                }
            };

            for (int i = 2; i < reader.FieldCount; i++)
            {
                weeklySummary.GameTypes.Add(reader.GetName(i));
            }

            var gameTypeCount = weeklySummary.GameTypes.Count;
            var firstRow = true;
            while (reader.Read())
            {                
                var gameTypeValues = new List<int>();
                for (int i = 0; i < gameTypeCount; i++)
                {
                    var value = Converter.ToInt(reader[weeklySummary.GameTypes[i]]);
                    gameTypeValues.Add(value);

                    // Grand Total.
                    if (firstRow)
                    {                      
                        weeklySummary.GrandTotal.GameTypeValues.Add(value);
                    }
                    else
                    {
                        weeklySummary.GrandTotal.GameTypeValues[i] += value;
                    }
                }

                firstRow = false;
                weeklySummary.WeeklyCategories.Add(new CasinoWeeklyCategory
                {
                    Name = Converter.ToString2(reader["MainCategory"]),
                    GameTypeValues = gameTypeValues
                });
            }

            return weeklySummary;
        }
    }
}

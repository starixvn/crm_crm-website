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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common;
    using Fanex.CRM.Report.Entities.CasinoReport;
    using Fanex.CRM.Report.Models.CasinoReport;
    using Fanex.CRM.Report.Resources;
    using Fanex.Data;
    using Fanex.Data.Repository;

    /// <summary>
    /// Casino Report Repository.
    /// </summary>
    public class CasinoReportRepository : ICasinoReportRepository
    {
        /// <summary>
        /// Gets the casino daily summaries.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The list of Category Casino Group object.</returns>
        public CasinoDailySummary GetCasinoDailySummary(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    var categoryCasinoGroups = new List<CategoryCasinoGroup>();
                    var casinoGroups = new List<CasinoGroup>();
                    int fieldCount = 0, value = 0;
                    CategoryCasinoGroup categoryCasinoGroup;
                    CasinoGroup casinoGroup;

                    while (reader.Read())
                    {
                        if (fieldCount == 0)
                        {
                            fieldCount = reader.FieldCount;
                            casinoGroups = ParseCasinoGroups(reader, fieldCount);
                        }

                        categoryCasinoGroup = new CategoryCasinoGroup(Converter.ToString2(reader["Maincategory"]));
                        categoryCasinoGroups.Add(categoryCasinoGroup);
                        foreach (var casino in casinoGroups)
                        {
                            casinoGroup = new CasinoGroup(casino.Name);
                            categoryCasinoGroup.CasinoGroups.Add(casinoGroup);
                            foreach (var comm in casino.Communications)
                            {
                                value = Converter.ToInt(reader[casino.Name + "_" + comm.Name]);
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
                            CasinoGroups = casinoGroups
                        }
                    };
                }
            }
        }

        public IList<CasinoDetailEntity> GetCasinoDetails(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<CasinoDetailEntity>(criteria.GetSpParams()).ToList();
            }
        }

        /// <summary>
        /// Gets the casino weekly summary.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The Casino Weekly Summary object.</returns>
        public CasinoWeeklySummary GetCasinoWeeklySummary(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader == null)
                    {
                        return null;
                    }

                    return ParseCasinoWeeklySummary(reader);
                }
            }
        }

        /// <summary>
        /// Parses the casino groups.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="fieldCount">The field count.</param>
        /// <returns>The list of Casino Group.</returns>
        private IList<CasinoGroup> ParseCasinoGroups(IDataReader reader, int fieldCount)
        {
            var casinoGroupHash = new Dictionary<string, CasinoGroup>();
            for (int i = 0; i < fieldCount; i++)
            {
                string[] columnArray = reader.GetName(i).Split('_');
                if (columnArray.Length != 2)
                {
                    continue;
                }

                CasinoGroup casinoGroup;
                if (!casinoGroupHash.TryGetValue(columnArray[0], out casinoGroup))
                {
                    casinoGroup = new CasinoGroup(columnArray[0]);
                    casinoGroupHash.Add(columnArray[0], casinoGroup);
                }

                casinoGroup.Communications.Add(new CommunicationValue(columnArray[1], 0));
            }

            return casinoGroupHash.Values.ToList();
        }

        /// <summary>
        /// Parses the casino weekly summary.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>The Casino Weekly Summary object.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private CasinoWeeklySummary ParseCasinoWeeklySummary(IDataReader reader)
        {
            CasinoWeeklySummary weeklySummary = new CasinoWeeklySummary
            {
                GameTypes = new List<string>(),
                WeeklyCategories = new List<CasinoWeeklyCategory>(),
                GrandTotal = new CasinoWeeklyCategory
                {
                    Name = Labels.Grand_Total,
                    GameTypeValues = new List<int>()
                }
            };

            int fieldCount = reader.FieldCount;
            for (int i = 2; i < fieldCount; i++)
            {
                weeklySummary.GameTypes.Add(reader.GetName(i));
            }

            int value = 0,
                gameTypeCount = weeklySummary.GameTypes.Count;
            bool firstRow = true;
            CasinoWeeklyCategory weeklyCategory;
            while (reader.Read())
            {
                weeklyCategory = new CasinoWeeklyCategory
                {
                    Name = Converter.ToString2(reader["MainCategory"]),
                    GameTypeValues = new List<int>()
                };

                for (int i = 0; i < gameTypeCount; i++)
                {
                    value = Converter.ToInt(reader[weeklySummary.GameTypes[i]]);
                    weeklyCategory.GameTypeValues.Add(value);

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
                weeklySummary.WeeklyCategories.Add(weeklyCategory);
            }

            return weeklySummary;
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="LogStatisticViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Models.LogStatistics
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Infrastructure.Models;

    public class LogStatisticsViewModel : BaseModel
    {
        public LogStatisticsViewModel()
        {
            MainCategories = new List<LogStatisticsMainCategory>();
        }

        public IEnumerable<LogStatisticsMainCategory> MainCategories { get; set; }

        public IEnumerable<BaseModel> ProductColumns { get; set; }

        public IEnumerable<BaseModel> CommunicationColumns { get; set; }

        public IEnumerable<int> GrandTotalOfCommunicationCount
        {
            get
            {
                List<int> sumOfCommunications =
                        Enumerable.Repeat(0, ProductColumns.Count() * CommunicationColumns.Count()).ToList();
                foreach (var mainCategory in MainCategories)
                {
                    var totalListOfCommunications = mainCategory.ListOfCommunicationCount.Count();
                    var listOfCommunicationCount = mainCategory.ListOfCommunicationCount;
                    for (int i = 0; i < totalListOfCommunications; i++)
                    {
                        sumOfCommunications[i] += listOfCommunicationCount.ElementAt(i);
                    }
                }

                return sumOfCommunications;
            }
        }

        public IEnumerable<int> GrandTotalOfCommunicationSumByProduct
        {
            get
            {
                List<int> sumOfCommunicationsByProduct =
                        Enumerable.Repeat(0, ProductColumns.Count()).ToList();
                foreach (var mainCategory in MainCategories)
                {
                    var totalListOfCommunicationSumByProduct = mainCategory.ListOfCommunicationSumByProduct.Count();
                    var listOfCommunicationSumByProduct = mainCategory.ListOfCommunicationSumByProduct;
                    for (int i = 0; i < totalListOfCommunicationSumByProduct; i++)
                    {
                        sumOfCommunicationsByProduct[i] += listOfCommunicationSumByProduct.ElementAt(i);
                    }
                }

                return sumOfCommunicationsByProduct;
            }
        }
    }
}

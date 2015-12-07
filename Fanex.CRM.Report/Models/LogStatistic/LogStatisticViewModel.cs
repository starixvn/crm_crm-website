// -----------------------------------------------------------------------
// <copyright file="LogStatisticViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Report.Models.LogStatistic
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CRM.Infrastructure.Models;

    public class LogStatisticViewModel : BaseModel
    {
        public LogStatisticViewModel()
        {
            MainCategories = new List<LogStatisticMainCategory>();
        }

        public IEnumerable<LogStatisticMainCategory> MainCategories { get; set; }

        public IEnumerable<BaseModel> ProductColumns { get; set; }

        public IEnumerable<BaseModel> CommunicationColumns { get; set; }

        public List<int> GrandTotalOfCommunicationCount
        {
            get
            {
                List<int> sumOfCommunications =
                        Enumerable.Repeat(0, ProductColumns.Count() * CommunicationColumns.Count()).ToList();
                foreach (var mainCategory in MainCategories)
                {
                    for (int i = 0; i < mainCategory.ListOfCommunicationCount.Count; i++)
                    {
                        sumOfCommunications[i] += mainCategory.ListOfCommunicationCount[i];
                    }
                }

                return sumOfCommunications;
            }
        }

        public List<int> GrandTotalOfCommunicationSumByProduct
        {
            get
            {
                List<int> sumOfCommunicationsByProduct =
                        Enumerable.Repeat(0, ProductColumns.Count()).ToList();
                foreach (var mainCategory in MainCategories)
                {
                    for (int i = 0; i < mainCategory.ListOfCommunicationSumByProduct.Count; i++)
                    {
                        sumOfCommunicationsByProduct[i] += mainCategory.ListOfCommunicationSumByProduct[i];
                    }
                }

                return sumOfCommunicationsByProduct;
            }
        }

        public int GrandTotalAllCommunications
        {
            get { return MainCategories.Sum(mainCategory => mainCategory.TotalCommunications); }
        }
    }
}

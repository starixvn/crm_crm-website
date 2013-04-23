// -----------------------------------------------------------------------
// <copyright file="CustomerDTO.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.CRM.Entities.ViewModel
{
    using System.Collections.Specialized;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomerDTO
    {
    }

    public class CustomersSuggestion
    {        
        public string query { get; set; }
        public StringCollection suggestions{get; set;}
        public string[] data = new string[] { "" };
    }

    public class RelatedCustomerSuggestion
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}

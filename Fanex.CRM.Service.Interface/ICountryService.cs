
namespace Fanex.CRM.Service.Interface
{
    using System.Collections.Generic;
    using Fanex.CRM.Entities;

    public interface ICountryService
    {
        IEnumerable<Country> GetAll();
        Country GetById(int countryId);
        
    }
}

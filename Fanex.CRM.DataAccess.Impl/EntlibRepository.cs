namespace Fanex.CRM.DataAccess.Impl
{
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using Fanex.CRM.Infrastructure.Repository;

    public class EntlibRepository<T, TId> : RepositoryBase<T, TId> where T : IAggregateRoot
    {
        private Database _db;

        protected EntlibRepository(IUnitOfWork uow)
            : base(uow)
        {
            
        }

        /// <summary>
        /// Get CRM instance
        /// </summary>
        protected Database Database
        {
            get
            {
                _db = EnterpriseLibraryContainer.Current.GetInstance<Database>(DbNameConstants.CRM);
                return _db;
            }
        }

        /// <summary>
        /// Get Databse by connection string
        /// </summary>
        /// <param name="dataBaseName"></param>
        /// <returns></returns>
        protected Database GetDatabase(string dataBaseName)
        {
            if (string.IsNullOrEmpty(dataBaseName))
            {
                return this.Database;
            }

            return EnterpriseLibraryContainer.Current.GetInstance<Database>(dataBaseName);
        }

        /// <summary>
        /// Get CRM database
        /// </summary>
        /// <returns></returns>
        protected Database GetDatabase()
        {
            return GetDatabase(string.Empty);
        }


    }
}

namespace Sunplus.Accounting.CRM.Repository.Entlib
{
    using System.Collections.Generic;
    using System.Transactions;
    using Infrastructure.Domain;
    using Sunplus.Accounting.CRM.Core.UnitOfWork;

    public class EntlibUnitOfWork : IUnitOfWork
    {
        private Dictionary<IAggregateRoot, IUnitOfWorkRepository> addedEntities;
        private Dictionary<IAggregateRoot, IUnitOfWorkRepository> changedEntities;
        private Dictionary<IAggregateRoot, IUnitOfWorkRepository> deletedEntities;

        public EntlibUnitOfWork()
        {
            addedEntities =
                 new Dictionary<IAggregateRoot, IUnitOfWorkRepository>();
            changedEntities =
                 new Dictionary<IAggregateRoot, IUnitOfWorkRepository>();
            deletedEntities =
                 new Dictionary<IAggregateRoot, IUnitOfWorkRepository>();
        }

        #region IUnitOfWork Members

        public void RegisterAmended(IAggregateRoot entity, IUnitOfWorkRepository unitofWorkRepository)
        {
            if (!changedEntities.ContainsKey(entity))
            {
                changedEntities.Add(entity, unitofWorkRepository);
            }
        }

        public void RegisterNew(IAggregateRoot entity, IUnitOfWorkRepository unitofWorkRepository)
        {
            if (!addedEntities.ContainsKey(entity))
            {
                addedEntities.Add(entity, unitofWorkRepository);
            }
        }

        public void RegisterRemoved(IAggregateRoot entity, IUnitOfWorkRepository unitofWorkRepository)
        {
            if (!deletedEntities.ContainsKey(entity))
            {
                deletedEntities.Add(entity, unitofWorkRepository);
            }
        }

        public void Commit()
        {
          
            using (var transScope = new TransactionScope())
            {
                foreach (IAggregateRoot entity in this.addedEntities.Keys)
                {
                    addedEntities[entity].PersistCreationOf(entity);
                }

                foreach (IAggregateRoot entity in this.changedEntities.Keys)
                {
                    changedEntities[entity].PersistUpdateOf(entity);
                }

                foreach (IAggregateRoot entity in this.deletedEntities.Keys)
                {
                    deletedEntities[entity].PersistDeletionOf(entity);
                }

                transScope.Complete(); 
            }
        }



        #endregion
    }
}

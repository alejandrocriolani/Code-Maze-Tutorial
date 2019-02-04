using Contracts;
using Entities;

namespace Repository
{
    public class RepositoryWrapper: IRepositoryWrapper
    {
        private RepositoryContext _repContext;
        private IOwnerRepository _owner;
        private IAccountRepository _account;

        public IOwnerRepository Owner
        {
            get{
                if(_owner == null)
                {
                    _owner = new OwnerRepository(_repContext);
                }
                return _owner;
            }
        }

        public IAccountRepository Account
        {
            get {
                if(_account == null)
                {
                    _account = new AccountRepository(_repContext);
                }
                return _account;
            }
        }

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repContext = repositoryContext;
        }
    }
}
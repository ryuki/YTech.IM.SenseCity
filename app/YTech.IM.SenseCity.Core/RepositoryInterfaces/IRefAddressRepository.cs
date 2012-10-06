using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.SenseCity.Core.Master;


namespace YTech.IM.SenseCity.Core.RepositoryInterfaces
{
   public interface IRefAddressRepository : INHibernateRepositoryWithTypedId<RefAddress, string>
    {
    }
}

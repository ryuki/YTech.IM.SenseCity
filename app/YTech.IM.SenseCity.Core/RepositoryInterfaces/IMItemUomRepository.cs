using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.SenseCity.Core.Master;

namespace YTech.IM.SenseCity.Core.RepositoryInterfaces
{
   public interface IMItemUomRepository : INHibernateRepositoryWithTypedId<MItemUom, string>
   {
       MItemUom GetByItem(MItem item);
   }
}

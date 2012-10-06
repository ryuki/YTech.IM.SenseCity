using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.SenseCity.Core.Master;

namespace YTech.IM.SenseCity.Core.RepositoryInterfaces
{
    public interface IMPacketItemCatRepository : INHibernateRepositoryWithTypedId<MPacketItemCat, string>
    {
        IEnumerable<MPacketItemCat> GetPagedItemList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows,string packetId);
        IList<MPacketItemCat> GetByPacketId(string packetId);
    }
}

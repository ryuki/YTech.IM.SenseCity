using System.Collections.Generic;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.IM.SenseCity.Core.Master;
using YTech.IM.SenseCity.Core.Transaction;
using YTech.IM.SenseCity.Core.Transaction.Inventory;

namespace YTech.IM.SenseCity.Core.RepositoryInterfaces
{
    public interface ITTransRoomRepository : INHibernateRepositoryWithTypedId<TTransRoom, string>
    {

        TTransRoom GetByRoom(MRoom room);

        IList<TTransRoom> GetListByTransDate(System.DateTime? dateFrom, System.DateTime? dateTo);

        IEnumerable<TTransRoom> GetPagedTransRoomList(string orderCol, string orderBy, int pageIndex, int maxRows, ref int totalRows, string searchBy, string searchText);
    }
}

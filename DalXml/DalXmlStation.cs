using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal
{
    partial class DalXml : DalApi.IDal
    {
        public void AddStation(Station newStation)
        {
            throw new NotImplementedException();
        }

        public Station GetStation(int stationId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Station> GetStations(Predicate<Station> stationPredicate)
        {
            throw new NotImplementedException();
        }

        public void UpdateStation(Station station)
        {
            throw new NotImplementedException();
        }
    }
}

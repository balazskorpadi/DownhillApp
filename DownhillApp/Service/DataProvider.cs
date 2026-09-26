using DownhillApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillApp.Service
{
    public class DataProvider : IDataProvider
    {
        private ILocalDataBase db;
        public DataProvider(ILocalDataBase dataBase)
        {
            this.db = dataBase;
        }
        public void Create(object obj)
        {
            this.db.Insert(obj);
        }
        public void Delete(object obj)
        {
            this.db.Delete(obj);
        }
        public void Update(object obj)
        {
            this.db.Ubdate(obj);
        }
        public ObservableCollection<string> GetAllGroupName()
        {
            ObservableCollection<string> result = new ObservableCollection<string>();
            foreach (Group g in db.AllGroups().Result)
            {
                result.Add(g.Name);
            }
            return result;
        }
        public List<RideData> GetAllRides()
        {
            List<RideData> result = this.db.AllRides().Result;
            return result;
        }

        public List<RideData> GetRidesWithName(string name)
        {
            List<RideData> result = this.db.AllRides().Result;
            foreach (RideData rd in result)
            {
                if (rd.GrpupName != name)
                {
                    result.Remove(rd);
                }
            }
            return result;
        }
    }
}

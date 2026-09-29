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
        public void UpdateRideData(RideData rd)
        {
            this.db.UpdateRide(rd);
        }
        public void Update(Group add, Group remove)
        {
            this.db.UpdateGroup(add, remove);
        }
        public ObservableCollection<Group> GetAllGroupName()
        {
            return this.db.AllGroups();
        }
        public ObservableCollection<RideData> GetAllRides()
        {
            return this.db.AllRides();
        }
        public ObservableCollection<RideData> GetRidesWithName(string name)
        {
            return new ObservableCollection<RideData>(this.db.AllRides().Where(x => x.GroupName == name));
        }
    }
}

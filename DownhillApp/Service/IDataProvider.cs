using DownhillApp.Model;
using System.Collections.ObjectModel;

namespace DownhillApp.Service
{
    public interface IDataProvider
    {
        void Create(object obj);
        void Delete(object obj);
        ObservableCollection<Group> GetAllGroupName();
        ObservableCollection<RideData> GetAllRides();
        ObservableCollection<RideData> GetRidesWithName(string name);
        void Update(Group add, Group remove);
        void UpdateRideData(RideData rd);
    }
}
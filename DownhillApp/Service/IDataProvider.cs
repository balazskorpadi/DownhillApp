using DownhillApp.Model;
using System.Collections.ObjectModel;

namespace DownhillApp.Service
{
    public interface IDataProvider
    {
        void Create(object obj);
        void Delete(object obj);
        ObservableCollection<string> GetAllGroupName();
        List<RideData> GetAllRides();
        List<RideData> GetRidesWithName(string name);
        void Update(object obj);
    }
}
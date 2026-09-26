using DownhillApp.Model;

namespace DownhillApp.Service
{
    public interface IDataProvider
    {
        void Create(object obj);
        void Delete(object obj);
        List<string> GetAllGroupName();
        List<RideData> GetAllRides();
        List<RideData> GetRidesWithName(string name);
        void Update(object obj);
    }
}
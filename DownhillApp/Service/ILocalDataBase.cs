using DownhillApp.Model;

namespace DownhillApp.Service
{
    public interface ILocalDataBase
    {
        Task Delete(object obj);
        Task<List<Group>> AllGroups();
        Task<List<RideData>> AllRides();
        Task Insert(object obj);
        Task Ubdate(object obj);
    }
}
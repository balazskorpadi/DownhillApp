using DownhillApp.Model;
using System.Collections.ObjectModel;

namespace DownhillApp.Service
{
    public interface ILocalDataBase
    {
        ObservableCollection<Group> AllGroups();
        ObservableCollection<RideData> AllRides();
        Task Delete(object obj);
        Task Insert(object obj);
        void Save(string tablename);
        Task UpdateGroup(Group add, Group remove);
        Task UpdateRide(RideData obj);
    }
}
using DownhillApp.Model;
using System.Collections.ObjectModel;

namespace DownhillApp.Service
{
    public interface ILocalDataBase
    {
        //Task Inicializedatabase();
        Task Delete(object obj);
        Task<ObservableCollection<string>> AllGroups();
        Task<List<RideData>> AllRides();
        Task Insert(object obj);
        Task Ubdate(object obj);
    }
}
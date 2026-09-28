using DownhillApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillApp.Service
{
    public class LocalDataBase : ILocalDataBase
    {
        private string dataBasePath = Path.Combine(FileSystem.Current.AppDataDirectory, "downhill.db3");
        private SQLiteAsyncConnection dataBase;

        public LocalDataBase()
        {
            dataBase = new SQLiteAsyncConnection(dataBasePath);
            dataBase.CreateTableAsync<Group>().Wait();
            dataBase.CreateTableAsync<RideData>().Wait();
        }

        /*public async Task Inicializedatabase()
        {

            dataBase.CreateTableAsync<RideData>().Wait();

            dataBase.CreateTableAsync<Group>().Wait();

        }*/


        public async Task Insert(object obj)
        {
            if (obj != null)
            {
                if (obj is RideData)
                {
                    await dataBase.InsertAsync(obj as RideData);
                }
                if (obj is Group)
                {
                    await dataBase.InsertAsync(obj as Group);
                }
            }
        }

        public async Task Ubdate(object obj)
        {
            if (obj != null)
            {
                if (obj is RideData)
                {
                    await dataBase.UpdateAsync(obj as RideData);
                }
                if (obj is Group g)
                {
                    await dataBase.UpdateAsync(obj as Group);
                    List<RideData> all = await dataBase.Table<RideData>().ToListAsync();
                    foreach (RideData item in all)
                    {
                        if(item.GrpupName == g.Name)
                        {
                            item.GrpupName = g.Name;
                            await dataBase.UpdateAsync(item);
                        }
                    }
                }
            }
        }

        public async Task Delete(object obj)
        {
            if (obj != null)
            {
                if (obj is RideData)
                {
                    await dataBase.DeleteAsync(obj as RideData);
                }
                if (obj is Group)
                {
                    await dataBase.DeleteAsync(obj as Group);
                }
            }
        }

        public async Task<List<RideData>> AllRides()
        {
            List<RideData> result = await dataBase.Table<RideData>().ToListAsync();
            return result;
        }

        public async Task<ObservableCollection<string>> AllGroups()
        {

            //await dataBase.CreateTableAsync<Group>();


            var valami = dataBase.Table<Group>();
            var temp = valami.ToListAsync().Result;
            ;

            //var temp = await dataBase.Table<Group>().ToListAsync();
            ObservableCollection<string> result = new ObservableCollection<string>();
            foreach(Group g in temp)
            {
                result.Add(g.Name);
            }
            return result;
        }
    }
}

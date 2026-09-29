using AudioToolbox;
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

        ObservableCollection<Group> Groups { get; set; }
        ObservableCollection<RideData> RideDatas { get; set; }

        public LocalDataBase()
        {
            Groups = new ObservableCollection<Group>();
            RideDatas = new ObservableCollection<RideData>();

            dataBase = new SQLiteAsyncConnection(dataBasePath);
            dataBase.CreateTableAsync<Group>().Wait();
            dataBase.CreateTableAsync<RideData>().Wait();

            LoadGroups();
            LoadRides();
        }

        public async Task Insert(object obj)
        {
            if (obj != null)
            {
                if (obj is RideData)
                {
                    await dataBase.InsertAsync(obj as RideData);
                    this.Save("ridedata");
                }
                if (obj is Group)
                {
                    await dataBase.InsertAsync(obj as Group);
                    this.Save("group");
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
                    this.Save("ridedata");
                }
                if (obj is Group)
                {
                    await dataBase.DeleteAsync(obj as Group);
                    this.Save("group");
                }
            }
        }

        public async Task UpdateRide(RideData obj)
        {
            if (obj != null)
            {
                await dataBase.UpdateAsync(obj);
                this.Save("ridedata");
            }
        }

        public async Task UpdateGroup(Group add, Group remove)
        {
            if (add != null && remove != null)
            {
                List<RideData> all = await dataBase.Table<RideData>().Where(x => x.GroupName == remove.Name).ToListAsync();
                foreach (RideData item in all)
                {
                    item.GroupName = add.Name;
                    await dataBase.UpdateAsync(item);
                }
                remove.Name = add.Name;
                await dataBase.UpdateAsync(remove);
                this.Save("group");
                this.Save("ridedata");
            }
        }

        public async void Save(string tablename)
        {
            if (tablename == "group")
            {
                var all = await dataBase.Table<Group>().ToListAsync();
                this.Groups.Clear();
                all.ForEach(x => Groups.Add(x));
            }
            if (tablename == "ridedata")
            {
                var all = await dataBase.Table<RideData>().ToListAsync();
                this.RideDatas.Clear();
                all.ForEach(x => this.RideDatas.Add(x));
            }
        }


        public ObservableCollection<RideData> AllRides()
        {
            return this.RideDatas;
        }

        public ObservableCollection<Group> AllGroups()
        {
            return this.Groups;
        }

        private async void LoadGroups()
        {
            List<Group> all = await dataBase.Table<Group>().ToListAsync();
            foreach (Group g in all)
            {
                Groups.Add(g);
            }
        }

        private async void LoadRides()
        {
            List<RideData> all = await dataBase.Table<RideData>().ToListAsync();
            foreach (RideData r in all)
            {
                RideDatas.Add(r);
            }
        }
    }
}

using DownhillApp.Model;
using DownhillApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DownhillApp.ViewModel
{
    public class AllGroupViewModel : BaseViewModel
    {
        IDataProvider dataProvider;
        public ObservableCollection<Group> AllGroupName {  get; set; }
        public Command C_BacktoMenu { get; private set; }
        public Command C_Add { get; private set; }
        public Command C_Delete { get; private set; }
        public Command C_Update { get; private set; }

        private Group selectedName;
        private Group editName;
        private bool IsModified;
        public AllGroupViewModel(IDataProvider dp)
        {
            this.dataProvider = dp;

            this.AllGroupName = dataProvider.GetAllGroupName();

            this.C_BacktoMenu = new Command(BackToMenu);
            this.C_Add = new Command(Add);
            this.C_Delete = new Command(() => Delete());
            this.C_Update = new Command(Update);
            selectedName = new Group();
            editName = new Group();
            IsModified = false;
        }

        public Group SelectedName
        {
            get { return selectedName; }
            set
            {
                this.selectedName = value;
                OnPropertyChange();
            }
        }

        public Group EditName
        {
            get { return editName; }
            set
            {
                this.editName = value;
                OnPropertyChange();
            }
        }

        private async void BackToMenu()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        private void Delete()
        {
            Group temp = AllGroupName.First(x => x.Name == SelectedName.Name);
            AllGroupName.Remove(temp);
            dataProvider.Delete(temp);
            SelectedName = new Group();
        }

        private void Update()
        {
            EditName = SelectedName;
            IsModified = true;
        }

        private void Add()
        {
            if(IsModified)
            {
                //Update
                Group temp = AllGroupName.First(x => x.Name == SelectedName.Name);
                dataProvider.UpdateGroup(EditName, temp);
            }
            else
            {
                //New Group
                dataProvider.Create(EditName);
            }
            AllGroupName = dataProvider.GetAllGroupName();
            IsModified = false;
            EditName = new Group();
            SelectedName = new Group();
        }

        public void SetDatas()
        {
            this.AllGroupName = dataProvider.GetAllGroupName();

            EditName = new Group();
            SelectedName = new Group();
            IsModified = false;
        }
    }
}

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
        public ObservableCollection<string> AllGroupName {  get; set; }
        public Command C_BacktoMenu { get; private set; }
        public Command C_Add { get; private set; }
        public Command C_Delete { get; private set; }
        public Command C_Update { get; private set; }

        private string selectedName;
        private string editName;
        private bool IsModified;
        public AllGroupViewModel(IDataProvider dp)
        {
            this.dataProvider = dp;

            this.AllGroupName = dataProvider.GetAllGroupName();
            this.C_BacktoMenu = new Command(BackToMenu);
            this.C_Add = new Command(Add);
            this.C_Delete = new Command(() => Delete());
            this.C_Update = new Command(Update);
            selectedName = "";
            editName = "";
            IsModified = false;
        }

        public string SelectedName
        {
            get { return selectedName; }
            set
            {
                this.selectedName = value;
                OnPropertyChange();
            }
        }

        public string EditName
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
            AllGroupName.Remove(SelectedName);
            dataProvider.Delete(new Group(SelectedName));
            SelectedName = "";
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
                SelectedName = EditName;
                dataProvider.Update(new Group(SelectedName));
            }
            else
            {
                //New Group
                AllGroupName.Add(EditName);
                dataProvider.Create(new Group(EditName));
            }
            IsModified = false;
            EditName = "";
        }

        public void SetDatas()
        {
            this.AllGroupName = dataProvider.GetAllGroupName();
            EditName = "";
            SelectedName = "";
            IsModified = false;
        }
    }
}

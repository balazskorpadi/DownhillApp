using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillApp.Model
{
    public class Group : BaseModel
    {
        private string name;
        public Group()
        {
            this.name = "";
        }

        public Group(string name)
        {
            this.name = name;
        }
        [PrimaryKey]
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                OnPropertyChange();
            }
        }
    }
}

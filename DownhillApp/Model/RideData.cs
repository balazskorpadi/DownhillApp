using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillApp.Model
{
    public class RideData : BaseModel
    {
        private string name;
        private Group groupName;
        private float time;
        private List<float[]> acceleration;
        private List<float[]> rotacion;
        private List<float[]> position;
        private List<float> height;
        public RideData()
        {
            this.name = "";
            this.groupName = new Group();
            this.time = 0;
            this.acceleration = new List<float[]>();
            this.rotacion = new List<float[]>();
            this.position = new List<float[]>();
            this.height = new List<float>();

        }
        public RideData(string name, Group groupName, float time, List<float[]> acceleration, List<float[]> rotacion, List<float[]> position, List<float> height)
        {
            this.name = name;
            this.groupName = groupName;
            this.time = time;
            this.acceleration = acceleration;
            this.rotacion = rotacion;
            this.position = position;
            this.height = height;
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                OnPropertyChange();
            }
        }

        public Group GrpupName
        {
            get { return this.groupName; }
            set
            {
                this.groupName = value;
                OnPropertyChange();
            }
        }

        public float Time
        {
            get { return this.time; }
            set
            {
                this.time = value;
                OnPropertyChange();
            }
        }

        public List<float[]> Acceleration
        {
            get{ return this.acceleration; }
            set
            {
                this.acceleration = value;
                OnPropertyChange();
            }
        }

        public List<float[]> Rotation
        {
            get { return this.rotacion; }
            set
            {
                this.rotacion = value;
                OnPropertyChange();
            }
        }

        public List<float[]> Position
        {
            get { return this.position; }
            set
            {
                this.position= value;
                OnPropertyChange();
            }
        }

        public List<float> Height
        {
            get { return this.height; }
            set
            {
                this.height = value;
                OnPropertyChange();
            }
        }
    }
}

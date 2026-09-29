using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DownhillApp.Model
{
    public class RideData : BaseModel
    {
        private string name;
        private string groupName;
        private int time;
        private List<float[]> acceleration;
        private List<float[]> rotation;
        private List<float[]> position;
        private List<float> height;
        public RideData()
        {
            this.name = "";
            this.groupName = "";
            this.time = 0;
            this.acceleration = new List<float[]>();
            this.rotation = new List<float[]>();
            this.position = new List<float[]>();
            this.height = new List<float>();

        }
        public RideData(string name, string groupName, int time, List<float[]> acceleration, List<float[]> rotacion, List<float[]> position, List<float> height)
        {
            this.name = name;
            this.groupName = groupName;
            this.time = time;
            this.acceleration = acceleration;
            this.rotation = rotacion;
            this.position = position;
            this.height = height;
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

        public string GroupName
        {
            get { return this.groupName; }
            set
            {
                this.groupName = value;
                OnPropertyChange();
            }
        }

        public int Time
        {
            get { return this.time; }
            set
            {
                this.time = value;
                OnPropertyChange();
            }
        }
        [Ignore]
        public List<float[]> Acceleration
        {
            get{ return this.acceleration; }
            set
            {
                this.acceleration = value;
                OnPropertyChange();
            }
        }

        public string AccelerationJson
        {
            get
            {
                return JsonSerializer.Serialize(this.acceleration);
            }
            set
            {
                acceleration = JsonSerializer.Deserialize<List<float[]>>(value) ?? new List<float[]>();
                OnPropertyChange(nameof(Acceleration));
            }
        }
        [Ignore]
        public List<float[]> Rotation
        {
            get { return this.rotation; }
            set
            {
                this.rotation = value;
                OnPropertyChange();
            }
        }
        public string RotationJson
        {
            get
            {
                return JsonSerializer.Serialize(this.rotation);
            }
            set
            {
                rotation = JsonSerializer.Deserialize<List<float[]>>(value) ?? new List<float[]>();
                OnPropertyChange(nameof(Rotation));
            }
        }
        [Ignore]
        public List<float[]> Position
        {
            get { return this.position; }
            set
            {
                this.position= value;
                OnPropertyChange();
            }
        }
        public string PositionJson
        {
            get
            {
                return JsonSerializer.Serialize(this.position);
            }
            set
            {
                position = JsonSerializer.Deserialize<List<float[]>>(value) ?? new List<float[]>();
                OnPropertyChange(nameof(Position));
            }
        }
        [Ignore]
        public List<float> Height
        {
            get { return this.height; }
            set
            {
                this.height = value;
                OnPropertyChange();
            }
        }
        public string HeightJson
        {
            get
            {
                return JsonSerializer.Serialize(this.height);
            }
            set
            {
                height = JsonSerializer.Deserialize<List<float>>(value) ?? new List<float>();
                OnPropertyChange(nameof(Height));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillApp.Model
{
    public class HeightProfilePoint:BaseModel
    {
        private double x;
        private double height;
        private double grade;

        public HeightProfilePoint()
        {
             
        }
        public HeightProfilePoint(double x, double height, double grade)
        {
            this.x = x;
            this.height = height;
            this.grade = grade;
        }

        public double X
        {
            get { return this.x; }
            set
            {
                this.x = value;
                OnPropertyChange();
            }
        }
        public double Height
        {
            get { return this.height; }
            set
            {
                this.height = value;
                OnPropertyChange();
            }
        }
        public double Grade
        {
            get { return this.grade; }
            set
            {
                this.grade = value;
                OnPropertyChange();
            }
        }
    }
}

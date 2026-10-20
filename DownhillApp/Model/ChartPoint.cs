using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillApp.Model
{
    public class ChartPoint : BaseModel
    {
        private double axisX;
        private double dataX;
        private double dataY;
        private double dataZ;

        public ChartPoint()
        {
            
        }
        public ChartPoint(double axisX, double dataX, double dataY, double dataZ)
        {
            this.axisX = axisX;
            this.dataX = dataX;
            this.dataY = dataY;
            this.dataZ = dataZ;
        }

        public double AxisX
        { 
            get { return this.axisX; } 
            set 
            { 
                this.axisX = value;
                OnPropertyChange();
            } 
        }
        public double DataX
        {
            get { return this.dataX; }
            set
            {
                this.dataX = value;
                OnPropertyChange();
            }
        }
        public double DataY
        {
            get { return this.dataY; }
            set
            {
                this.dataY = value;
                OnPropertyChange();
            }
        }
        public double DataZ
        {
            get { return this.dataZ; }
            set
            {
                this.dataZ = value;
                OnPropertyChange();
            }
        }
    }
}

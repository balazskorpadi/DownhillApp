using DownhillApp.Model;
using DownhillApp.Service;
using Microsoft.Maui.Animations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillApp.ViewModel
{
    public class ResultViewModel : BaseViewModel
    {
        Random rnd = new Random();
        private IDataProvider dataProvider;
        public ObservableCollection<RideData> AllRides { get; set; }
        public Command C_BackToMenu { get; private set; }
        public Command C_SelectRide { get; private set; }
        public Command C_Delete { get; private set; }

        private RideData selectedRide;
        public ResultViewModel(IDataProvider service)
        {
            this.dataProvider = service;
            this.AllRides = new ObservableCollection<RideData>();
            AllRides = service.GetAllRides();

            this.C_BackToMenu = new Command(BackToMenu);
            this.C_SelectRide = new Command(AnaliseRide);
            this.C_Delete = new Command(Delete);

            this.selectedRide = new RideData();
        }

        public RideData SelectedRide
        {
            get { return this.selectedRide; }
            set
            {
                this.selectedRide = value;
                OnPropertyChange();
            }
        }

        private async void BackToMenu()
        {
            //Dummy adat hozzáadása
            //Kell egy selectedRide mert az alapján bindongol de utána jó minden :)
            /*for (int asd = 1; asd < 6; asd++)
            {
                RideData temprd1 = new RideData(
                    $"RideTeszt0{asd}",
                    "TestRides",
                    rnd.Next(37, 40),
                    new List<float[]>(),
                    new List<float[]>(),
                    new List<float[]>(),
                    new List<float>()
                    );

                int len = (temprd1.Time) * 4;
                float[] start = [47.51662F, 18.96459F];
                float[] end = [47.51683F, 18.96820F];
                temprd1.Acceleration.Add([0.0F, 0.0F, 0.0F]);
                temprd1.Rotation.Add([0.0F, 0.0F, 0.0F]);
                temprd1.Position.Add(start);
                temprd1.Height.Add(381F);

                //position end - 47.51683F, 18.96820F
                //height end - 308

                for (int i = 1; i < len - 1; i++)
                {
                    temprd1.Acceleration.Add([(float)rnd.Next(-800, 800) / 100F, (float)rnd.Next(-800, 800) / 100F, (float)rnd.Next(-1200, 1200) / 100F]);
                    temprd1.Rotation.Add([(float)rnd.Next(-2000, 2000) / 100F, (float)rnd.Next(-2000, 2000) / 100F, (float)rnd.Next(-2500, 2500) / 100F]);

                    float t = (float)i / (len - 1);
                    float cx = start[0] + (end[0] - start[0]) * t;
                    float cy = start[1] + (end[1] - start[1]) * t;
                    float offsetx = (float)(rnd.NextDouble() * 2 - 1) * 0.000015F;
                    float offsety = (float)(rnd.NextDouble() * 2 - 1) * 0.00002F;
                    temprd1.Position.Add([cx + offsetx, cy + offsety]);

                    float h = 381F + (308F - 381F) * t;
                    float offseth = (float)(rnd.NextDouble() * 2 - 1) * 0.6F;
                    temprd1.Height.Add(h);
                }

                temprd1.Acceleration.Add(temprd1.Acceleration[temprd1.Acceleration.Count - 1]);
                temprd1.Rotation.Add(temprd1.Rotation[temprd1.Rotation.Count - 1]);
                temprd1.Position.Add(end);
                temprd1.Height.Add(308F);
                ;
                this.dataProvider.Create(temprd1);
            }*/
            /*
            for (int asd = 1; asd < 6; asd++)
            {
                RideData temprd1 = new RideData(
                   $"BetterTest0{asd}",
                   "TestRides",
                   rnd.Next(37, 40),
                   new List<float[]>(),
                   new List<float[]>(),
                   new List<float[]>(),
                   new List<float>()
                   );

                int len = temprd1.Time * 4;

                float[] start = [47.51662F, 18.96459F];
                float[] end = [47.51683F, 18.96820F];

                float startHeight = 381F;
                float endHeight = 308F;

                float maxCurveMeters = 5F;
                int curveCount = 10;

                var curves = new List<(float t, float offset)>();

                for (int i = 0; i < curveCount; i++)
                {
                    float t = (float)(i + 1) / (curveCount + 1);
                    float offset = (float)(rnd.NextDouble() * 2 - 1)
                                   * maxCurveMeters;

                    curves.Add((t, offset));
                }

                var heightPoints = new List<(float t, float h)>
{
    (0f, 381f),
    (0.20f, 371f),
    (0.35f, 368f),
    (0.55f, 342f),
    (0.70f, 335f),
    (0.85f, 315f),
    (1f, 308f)
};

                double meanLat = (start[0] + end[0]) / 2.0
                                 * Math.PI / 180.0;

                double dx = (end[1] - start[1])
                            * 111320.0 * Math.Cos(meanLat);
                double dy = (end[0] - start[0]) * 111320.0;

                double distance = Math.Sqrt(dx * dx + dy * dy);

                double perpX = -dy / distance;
                double perpY = dx / distance;

                var positions = new List<float[]>();
                var heights = new List<float>();
                var lateralValues = new List<double>();

                // 1. Pálya és magasság előállítása
                for (int i = 0; i < len; i++)
                {
                    double t = (double)i / (len - 1);

                    double lat = start[0] + (end[0] - start[0]) * t;
                    double lon = start[1] + (end[1] - start[1]) * t;

                    double lateral = 0;

                    foreach (var curve in curves)
                    {
                        double width = 0.12;
                        double d = (t - curve.t) / width;

                        lateral += curve.offset *
                                   Math.Exp(-d * d / 2.0);
                    }

                    lat += perpY * lateral / 111320.0;
                    lon += perpX * lateral /
                           (111320.0 * Math.Cos(meanLat));

                    if (i == 0)
                    {
                        lat = start[0];
                        lon = start[1];
                    }
                    else if (i == len - 1)
                    {
                        lat = end[0];
                        lon = end[1];
                    }

                    positions.Add([(float)lat, (float)lon]);
                    lateralValues.Add(lateral);

                    int segment = 0;

                    while (segment < heightPoints.Count - 2 &&
                           t > heightPoints[segment + 1].t)
                    {
                        segment++;
                    }

                    var a = heightPoints[segment];
                    var b = heightPoints[segment + 1];

                    float localT = (float)((t - a.t) / (b.t - a.t));
                    localT = localT * localT * (3f - 2f * localT);

                    float h = a.h + (b.h - a.h) * localT;
                    heights.Add(h);
                }

                // 2. A pályához igazodó, simított szenzoradatok
                double prevAx = 0, prevAy = 0, prevAz = 0;
                double prevRx = 0, prevRy = 0, prevRz = 0;

                for (int i = 0; i < len; i++)
                {
                    // Magasságváltozás: pozitív érték lefelé haladást jelöl
                    double slope = 0;

                    if (i > 0)
                        slope = (heights[i - 1] - heights[i]) /
                                Math.Max(1, distance / (len - 1));

                    // A kanyar erőssége a szomszédos oldalirányú
                    // kitérések változásából becsülhető
                    double curve = 0;

                    if (i > 0 && i < len - 1)
                    {
                        curve = lateralValues[i + 1]
                              - 2 * lateralValues[i]
                              + lateralValues[i - 1];
                    }

                    // Kis, folytonos zaj
                    double noiseX = (rnd.NextDouble() - 0.5) * 0.8;
                    double noiseY = (rnd.NextDouble() - 0.5) * 0.8;
                    double noiseZ = (rnd.NextDouble() - 0.5) * 1.0;

                    // A gyorsulás függ a lejtéstől és a pálya változásától
                    double targetAx = slope * 1.5 + noiseX;
                    double targetAy = curve * 100 + noiseY;
                    double targetAz = 0.5 + slope * 0.5 + noiseZ;

                    // A forgás a kanyar változását követi
                    double targetRx = noiseX * 5;
                    double targetRy = curve * 120 + noiseY * 8;
                    double targetRz = curve * 180 + noiseZ * 8;

                    // Simítás: az előző mérési pont is számít
                    double alpha = 0.25;

                    double ax = prevAx + alpha * (targetAx - prevAx);
                    double ay = prevAy + alpha * (targetAy - prevAy);
                    double az = prevAz + alpha * (targetAz - prevAz);

                    double rx = prevRx + alpha * (targetRx - prevRx);
                    double ry = prevRy + alpha * (targetRy - prevRy);
                    double rz = prevRz + alpha * (targetRz - prevRz);

                    temprd1.Position.Add(positions[i]);
                    temprd1.Height.Add(heights[i]);

                    temprd1.Acceleration.Add([
                        (float)ax, (float)ay, (float)az
                    ]);

                    temprd1.Rotation.Add([
                        (float)rx, (float)ry, (float)rz
                    ]);

                    prevAx = ax;
                    prevAy = ay;
                    prevAz = az;

                    prevRx = rx;
                    prevRy = ry;
                    prevRz = rz;
                }

                this.dataProvider.Create(temprd1);
            }


            */
            //randomizálás vége
            //Ennyi az annyi


            await Shell.Current.GoToAsync("//MainPage");
        }

        private async void AnaliseRide()
        {
            if(this.SelectedRide.Name is null)
            {
                await Shell.Current.DisplayAlert("ERROR", "No ride selected", "OK");
            }
            else
            {
                RideData best = new RideData();
                best.Time = int.MaxValue;
                best.GroupName = this.SelectedRide.GroupName;
                foreach(RideData rd in AllRides)
                {
                    if(best.Time > rd.Time && best.GroupName == rd.GroupName)
                    {
                        best = rd;
                    }
                }
                var parameter = new ShellNavigationQueryParameters() { {"choosenRide", this.SelectedRide},{"bestRide", best} };
                await Shell.Current.GoToAsync("//Analise", parameter);
            }
        }

        private void Delete()
        {
            this.dataProvider.Delete(SelectedRide);
            this.SelectedRide = new RideData();
        }

        public void SetAllRides()
        {
            this.AllRides = dataProvider.GetAllRides();
        }
    }
}

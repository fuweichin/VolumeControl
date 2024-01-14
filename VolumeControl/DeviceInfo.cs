
using System;
using System.Drawing;
using Newtonsoft.Json;

namespace VolumeControl
{
    public class DeviceInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool MaxVolEnabled { get; set; }
        private int maxVol = 100;
        public int MaxVol
        {
            get { return maxVol; }
            set
            {
                if (value > 100)
                {
                    maxVol = 100;
                }
                else if (value < 2)
                {
                    maxVol = 2;
                }
                else
                {
                    maxVol = value;
                }
            }
        }

        public bool VolStepEnabled { get; set; }

        private float volStep = 2;
        public float VolStep
        {
            get { return volStep; }
            set
            {
                if (value > 8)
                {
                    volStep = 8;
                }
                else if (value < 0.25f)
                {
                    volStep = 0.25f;
                }
                else
                {
                    volStep = (float)Math.Pow(2, Math.Round(Math.Log(value) / Math.Log(2)));
                }
            }
        }

        public bool DontAutoMute { get; set; }


        [JsonIgnore]
        public DeviceHandler Handler { get; set; }

        [JsonIgnore]
        public Icon Icon { get; set; }

        [JsonIgnore]
        public bool IsVolatile { get; set; }

        public bool IsNotDefault()
        {
            return MaxVolEnabled || MaxVol != 100 || VolStepEnabled || VolStep != 2 || DontAutoMute;
        }
    }
}

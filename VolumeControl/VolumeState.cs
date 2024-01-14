using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolumeControl
{
    public class VolumeState
    {
        public VolumeState(float masterVolume, bool muted, long timeStamp)
        {
            this.MasterVolume = masterVolume;
            this.Muted = muted;
            this.TimeStamp = timeStamp;
        }
        public float MasterVolume { get; }
        public bool Muted { get; }
        public long TimeStamp { get; }
    }
}

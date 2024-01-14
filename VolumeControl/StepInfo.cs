namespace VolumeControl
{
    public class StepInfo
    {
        private float step = 2.0f;

        public StepInfo(float stepPercent)
        {
            this.step = stepPercent;
        }

        public float Step
        {
            get
            {
                return step;
            }
        }

        public uint StepCount
        {
            get
            {
                return (uint)(100f / step) + 1;
            }
        }
    }
}

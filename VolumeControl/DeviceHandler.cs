using NAudio.CoreAudioApi;
using System;
using System.Diagnostics;

namespace VolumeControl
{
    public class DeviceHandler
    {
        private DeviceInfo deviceInfo;
        private Stopwatch stopwatch;

        public MMDevice MMDevice { get; }
        public Stopwatch Stopwatch => stopwatch;

        public DeviceHandler(MMDevice device, DeviceInfo deviceInfo)
        {
            this.MMDevice = device;
            this.deviceInfo = deviceInfo;
            this.stopwatch = Stopwatch.StartNew();
        }

        private VolumeState lastState;
        private VolumeState ignoreState;

        public void Reset()
        {
            AudioEndpointVolume volume = MMDevice.AudioEndpointVolume;
            this.lastState = new VolumeState(volume.MasterVolumeLevelScalar, volume.Mute, stopwatch.ElapsedMilliseconds);
        }

        public void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {
            long now = stopwatch.ElapsedMilliseconds;
            if (ignoreState != null)
            {
                long deltaT = now - ignoreState.TimeStamp;
                if (deltaT < 30)
                {
                    if (data.Muted == ignoreState.Muted)
                    {
                        ignoreState = null;
                        return;
                    }
                }
                else
                {
                    ignoreState = null;
                }
            }
            if (data.Muted != lastState.Muted)
            {
                lastState = new VolumeState(data.MasterVolume, data.Muted, now);
                if (data.Muted && now - lastState.TimeStamp < 400 && data.MasterVolume == 0 && deviceInfo.DontAutoMute)
                {
                    // Console.WriteLine("Muted changed to {0}", data.Muted);
                    ignoreState = new VolumeState(data.MasterVolume, false, now);
                    MMDevice.AudioEndpointVolume.Mute = false;
                }
            }
            else if (data.MasterVolume != lastState.MasterVolume)
            {
                // Console.WriteLine("Volume changed from {0} to {1}", lastState.MasterVolume * 100, data.MasterVolume * 100);
                lastState = new VolumeState(data.MasterVolume, data.Muted, now);
                if (Convert.ToInt32(data.MasterVolume * 100) > deviceInfo.MaxVol)
                {
                    float newVol = deviceInfo.MaxVol / 100f;
                    MMDevice.AudioEndpointVolume.MasterVolumeLevelScalar = newVol;
                }
                else if (data.MasterVolume == 0 && data.Muted && deviceInfo.DontAutoMute)
                {
                    ignoreState = new VolumeState(data.MasterVolume, false, now);
                    MMDevice.AudioEndpointVolume.Mute = false;
                }
            }
        }

        public void SetIgnoreState(VolumeState ignoreState)
        {
            this.ignoreState = ignoreState;
        }

        public void LimitVolumeIfNeeded()
        {
            float currVol = MMDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100;
            if (Convert.ToInt32(currVol) > deviceInfo.MaxVol)
            {
                MMDevice.AudioEndpointVolume.MasterVolumeLevelScalar = deviceInfo.MaxVol / 100f;
            }
        }

        public void SetNearestMultipleVolumeIfNeeded(float volStep)
        {
            float currVol = MMDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100;
            float rest = currVol % volStep;
            if (rest > 0)
            {
                MMDevice.AudioEndpointVolume.MasterVolumeLevelScalar = (float)Math.Round((currVol - rest), 2) / 100;
            }
        }
    }
}

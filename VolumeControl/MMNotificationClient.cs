using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;

namespace VolumeControl
{
    public delegate void DefaultDeviceChangedCallBack(string defaultDeviceId);

    public class MMNotificationClient : IMMNotificationClient
    {
        private DefaultDeviceChangedCallBack callback;
        public MMNotificationClient(DefaultDeviceChangedCallBack callback)
        {
            this.callback = callback;
        }
        public void OnDeviceStateChanged(string deviceId, DeviceState newState)
        {

        }

        public void OnDeviceAdded(string pwstrDeviceId)
        {

        }

        public void OnDeviceRemoved(string deviceId)
        {

        }

        public void OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
        {
            if (flow == DataFlow.Render && role == Role.Multimedia)
            {
                callback(defaultDeviceId);
            }
        }

        public void OnPropertyValueChanged(string pwstrDeviceId, PropertyKey key)
        {

        }
    }
}

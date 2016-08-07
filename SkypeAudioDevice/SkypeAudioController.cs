using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using SKYPE4COMLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkypeSystemAudio
{
    class SkypeAudioController : IMMNotificationClient
    {
        static Skype skype;
        static MMDeviceEnumerator de;

        public Skype Skype
        {
            get { return skype; }
            private set { skype = value; }
        }

        public SkypeAudioController()
        {
            skype = new Skype();
            de = new MMDeviceEnumerator();
            SetDefaultAudioDevices();
            de.RegisterEndpointNotificationCallback(this);
        }

        private void SetDefaultAudioDevices()
        {
            MMDevice defaulOuttDevice = de.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            MMDevice defaulIntDevice = de.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
            if (isLoggedIn())
            {
                skype.Settings.AudioOut = defaulOuttDevice.FriendlyName;
                skype.Settings.Ringer = defaulOuttDevice.FriendlyName;
                skype.Settings.AudioIn = defaulIntDevice.FriendlyName;
            }
        }

        public void OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
        {

            Debug.WriteLine(String.Format("Default device changed = {0}, {1}, {2}", defaultDeviceId, role, flow));
            SetDefaultAudioDevices(flow, defaultDeviceId);
        }

        private void SetDefaultAudioDevices(DataFlow flow, string defaultDeviceId)
        {
            if (isLoggedIn())
            {
                if (flow == DataFlow.Render)
                {
                    skype.Settings.AudioOut = de.GetDevice(defaultDeviceId).FriendlyName;
                    skype.Settings.Ringer = de.GetDevice(defaultDeviceId).FriendlyName;
                }
                else
                {
                    skype.Settings.AudioIn = de.GetDevice(defaultDeviceId).FriendlyName;
                }
            }
        }

        public void OnDeviceAdded(string pwstrDeviceId)
        {
            Debug.WriteLine(String.Format("Device added = {0}", pwstrDeviceId));
        }

        public void OnDeviceRemoved(string deviceId)
        {
            Debug.WriteLine(String.Format("Device removed = {0}", deviceId));
        }

        public void OnDeviceStateChanged(string deviceId, DeviceState newState)
        {
            Debug.WriteLine(String.Format("{0}, {1}", deviceId, newState));
        }

        public void OnPropertyValueChanged(string pwstrDeviceId, PropertyKey key)
        {
            Debug.WriteLine(String.Format("{0}, {1}", pwstrDeviceId, key));
        }

        private Boolean isLoggedIn()
        {
            return skype.Client.IsRunning && skype.CurrentUserStatus != TUserStatus.cusLoggedOut;
        }
    }
}

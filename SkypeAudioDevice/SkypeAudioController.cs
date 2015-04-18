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
            MMDevice defaultDevice=de.GetDefaultAudioEndpoint(DataFlow.Render, Role.Console);
            if (isLoggedIn() && skype.Settings.AudioOut != defaultDevice.FriendlyName && skype.Settings.Ringer != defaultDevice.FriendlyName)
            {
                skype.Settings.AudioOut = defaultDevice.FriendlyName;
                skype.Settings.Ringer = defaultDevice.FriendlyName; 
            }
            de.RegisterEndpointNotificationCallback(this);
        }

        public void OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
        {
            
            Debug.WriteLine(String.Format("Default device changed = {0}, {1}, {2}", defaultDeviceId, role, flow));
            if (isLoggedIn() && skype.Settings.AudioOut != de.GetDevice(defaultDeviceId).FriendlyName && skype.Settings.Ringer != de.GetDevice(defaultDeviceId).FriendlyName)
            {
                skype.Settings.AudioOut = de.GetDevice(defaultDeviceId).FriendlyName;
                skype.Settings.Ringer = de.GetDevice(defaultDeviceId).FriendlyName;
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

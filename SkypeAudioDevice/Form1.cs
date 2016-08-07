using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SKYPE4COMLib;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;

namespace SkypeSystemAudio
{
    public partial class Form1 : Form
    {
        SkypeAudioController sc;
        public Form1()
        {
            InitializeComponent();
            sc = new SkypeAudioController();
            IntPtr Hicon = Properties.Resources.social132.GetHicon();
            Icon newIcon = Icon.FromHandle(Hicon);
            notifyIcon1.Icon = newIcon;
            notifyIcon1.Text = String.Format("{0} {1}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Exit_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(false);
        }
    }

}

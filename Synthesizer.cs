using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp.WinForms;
using CefSharp;

namespace Synthesizer.net
{
    public partial class Synthesizer : Form
    {
        private const string startUpPath = @"C:\Git Repos\Synthesizer.net\Synthesizer.net\resources\synthesizer.html";
        public ChromiumWebBrowser Chrome = new ChromiumWebBrowser(startUpPath){Dock = DockStyle.Fill};

        public Synthesizer()
        {
            InitializeComponent();
            InitializeChrome();
            Chrome.RegisterJsObject("synthesizerNET", new SynthesizerUIHelper(Chrome, this)); 
        }

        private void InitializeChrome()
        {
            if ((Properties.Settings.Default.firstUse == true) || (Properties.Settings.Default.culture == "") || (Properties.Settings.Default.voicename == ""))
            {
                Chrome.Load(@"C:\Git Repos\Synthesizer.net\Synthesizer.net\resources\configurator.html");
            }

            this.Controls.Add(Chrome);
            this.WindowState = FormWindowState.Maximized;
            BrowserSettings browserSettings = new BrowserSettings();
            browserSettings.FileAccessFromFileUrlsAllowed = true;
            browserSettings.UniversalAccessFromFileUrlsAllowed = true;
            Chrome.BrowserSettings = browserSettings;
        }

        private void Synthesizer_Load(object sender, EventArgs e)
        {

        }
    }
}

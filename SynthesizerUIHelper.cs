using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Dynamic;

namespace Synthesizer.net
{
    class SynthesizerUIHelper
    {
        private SpeechSynthesizer sintetizador = new SpeechSynthesizer();
        private static ChromiumWebBrowser Chrome = null;
        private static Synthesizer ChromeForm = null;
 
        /// <summary>
        ///  Admin constructor. Access chrome and form from another thread
        /// </summary>
        /// <param name="browser">A ChromiumWebBrowser object</param>
        /// <param name="formulario">A (your formular) object</param>
        public SynthesizerUIHelper(ChromiumWebBrowser browser, Synthesizer formulario) 
        {
            if (browser is ChromiumWebBrowser)
            {
                Chrome = browser;
            }
            else 
            {
                throw new Exception("The first parameter of the class must be a ChromiumWebBrowser object. "+ browser.GetType() + " given.");
            }

            if (formulario is Synthesizer)
            {
                ChromeForm = formulario;
            }
            else
            {
                throw new Exception("The second parameter of the class must be a (Your formular) object. " + formulario.GetType() + " given.");
            }
        }
 
        public void speak(string content = "")
        {
            try
            {
                sintetizador.SelectVoice(Properties.Settings.Default.voicename);
                sintetizador.SpeakProgress += new EventHandler<SpeakProgressEventArgs>(synthesizer_SpeakProgress);
                sintetizador.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(synthesizer_SpeakCompleted);
                sintetizador.SetOutputToDefaultAudioDevice();
                sintetizador.SpeakAsync(content);
            }
            catch (InvalidOperationException e) { Console.WriteLine(e.Message); }
        }

        private void synthesizer_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            this.InjectScript("$('#action-buttons-progress').text('');");
            //this.InjectScript("$('#action-buttons-progress').html('<b>" + e.Text + "</b>')");
            //Console.WriteLine("SpeakProgress: AudioPosition=" + e.AudioPosition + ",\tCharacterPosition=" + e.CharacterPosition + ",\tCharacterCount=" + e.CharacterCount + ",\tText=" + e.Text);
        }

        private void synthesizer_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {
            this.InjectScript("$('#action-buttons-progress').html('<b>" + e.Text + "</b>')");
            Console.WriteLine("SpeakProgress: AudioPosition=" + e.AudioPosition + ",\tCharacterPosition=" + e.CharacterPosition + ",\tCharacterCount=" + e.CharacterCount + ",\tText=" + e.Text);
        }

        private void InjectScript(string script)
        {
            Chrome.ExecuteScriptAsync(script);
        }

        public void stop()
        {
            try
            {
                sintetizador.SpeakAsyncCancelAll();
            }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// Set the sinthesizer volume with an integer (0 - 100)
        /// </summary>
        /// <param name="level"></param>
        public void setVolume(int level)
        {
            if (level > 100)
            {
                sintetizador.Volume = 100;
            }
            else if (level < 0)
            {
                sintetizador.Volume = 0;
            }
            else
            {
                sintetizador.Volume = level;
            }
        }

        public void resume()
        {
            sintetizador.Resume();
        }
        public void pause()
        {
            sintetizador.Pause();
        }

        /// <summary>
        /// Send speechSynthesizer output to a .wav file
        /// </summary>
        /// <param name="content"></param>
        public void toWAVFile(string content = "")
        {
            sintetizador.SelectVoice(Properties.Settings.Default.voicename);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            sintetizador.SetOutputToWaveFile(path + "/test.wav");
            sintetizador.Speak(content);
            MessageBox.Show("File exported succesfully !",".wav File succesfully exported",MessageBoxButtons.OK,MessageBoxIcon.Information);
            sintetizador.SetOutputToDefaultAudioDevice();
        }

        public void listAvailableVoicesConfigurator()
        {
            var list = new List<dynamic>();

            using (sintetizador)
            {
                foreach (InstalledVoice voice in sintetizador.GetInstalledVoices())
                {
                    dynamic VOZ = new ExpandoObject();
                    var info = voice.VoiceInfo;

                    VOZ.name = info.Name;
                    VOZ.culture = info.Culture;
                    VOZ.age = info.Age;
                    list.Add(VOZ);
                }
            }

            string JSON = JsonConvert.SerializeObject(list);
            this.InjectScript("AddVoiceToLayout('" +System.Net.WebUtility.UrlEncode(JSON) + "');");
        }

        public void saveConfiguration(string name, string culture)
        {
            Properties.Settings.Default.voicename = name;
            Properties.Settings.Default.culture = culture;
            Properties.Settings.Default.firstUse = false;
            Properties.Settings.Default.Save();

            Application.Restart();
        }

        public void resetConfiguration()
        {
            Properties.Settings.Default.voicename = "";
            Properties.Settings.Default.culture = "";
            Properties.Settings.Default.firstUse = true;
            Properties.Settings.Default.Save();

            Application.Restart();
        }

        public void showDevTools()
        {
            Chrome.ShowDevTools();
        }
    }
}

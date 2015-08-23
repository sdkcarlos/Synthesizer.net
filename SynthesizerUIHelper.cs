using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Synthesizer.net
{
    class SynthesizerUIHelper
    {
        private SpeechSynthesizer sintetizador = new SpeechSynthesizer();
        private string StartupDocument = @"C:\Git Repos\Synthesizer.net\Synthesizer.net\resources\synthesizer.html";
        private static ChromiumWebBrowser Chrome = null;
        private static Synthesizer ChromeForm = null;
 
        /// <summary>
        ///  Admin constructor. Access your browser and form from another thread
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
                sintetizador.SetOutputToDefaultAudioDevice();
                sintetizador.SpeakAsync(content);
            }
            catch (InvalidOperationException){}
        } 

        public void stop()
        {
            sintetizador.SpeakAsyncCancelAll();
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

        public void showDevTools()
        {
            Chrome.ShowDevTools();
        }
    }
}

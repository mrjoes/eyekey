using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpeechLib;

namespace EyeKey
{
    class Talker
    {
        SpVoice voice;

        public Talker()
        {
            voice = new SpVoice();
            foreach (SpObjectToken v in voice.GetVoices())
            {
                if (v.GetDescription() == "Olga")
                {
                    voice.Voice = v;
                    break;
                }
            }
        }

        public void Say(string text)
        {
            voice.Speak(text, SpeechVoiceSpeakFlags.SVSFlagsAsync);
        }
    }
}

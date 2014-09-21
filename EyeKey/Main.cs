using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TETControls.TrackBox;

namespace EyeKey
{
    public partial class Main : Form
    {
        Talker talker;

        public Main()
        {
            InitializeComponent();
            talker = new Talker();
        }

        private void keyboard1_OnKeyButton(object sender, Keyboard.KeyboardEventArgs e)
        {
            textBox.Text += e.Key;
            textBox.Focus();
            textBox.Select(textBox.MaxLength, 0);
        }

        private void SayLastWord()
        {
            int i = textBox.Text.Length - 2;
            while (i > 0)
            {
                Char c = textBox.Text[i];
                if (c == ' ')
                    break;

                i -= 1;
            }

            if (i < textBox.Text.Length - 2)
            {
                talker.Say(textBox.Text.Substring(i));
            }
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (textBox.Text.Length > 0 && textBox.Text[textBox.Text.Length - 1] == ' ')
            {
                SayLastWord();
            }
        }
    }
}

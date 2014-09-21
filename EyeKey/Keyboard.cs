using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace EyeKey
{
    public partial class Keyboard : UserControl
    {
        private const string Letters = "абвгдеёжзийклмнопрстуфхцчшщыьэюя ";
        private const int NumRows = 3;
        private int RowSize;

        private KeyButton currentButton = null;
        private bool buttonUsed = false;

        // Public API
        public class KeyboardEventArgs
        {
            public KeyboardEventArgs(char letter)
            {
                Key = letter;
            }

            public char Key;
        };

        public delegate void OnKeyButtonHandler(object sender, KeyboardEventArgs e);
        public event OnKeyButtonHandler OnKeyButton;

        EyeTribe tribe;
        EyeCursor cursor;

        // Control-related
        public Keyboard()
        {
            InitializeComponent();

            CreateKeyboard();

            tribe = new EyeTribe();
            cursor = new EyeCursor();
            //cursor.Show();
        }

        private void CreateKeyboard()
        {
            RowSize = Letters.Length / NumRows;

            int step = 0;
            int row = 0;

            foreach (char l in Letters)
            {
                CreateButton(l, step, row);

                step += 1;
                if (step >= RowSize)
                {
                    row += 1;
                    step = 0;
                }
            }
        }

        private void CreateButton(char letter, int column, int row)
        {
            int width = Width / RowSize;
            int height = Height / NumRows;

            KeyButton btn = new KeyButton();
            btn.Text = letter.ToString().ToUpper();
            btn.Location = new Point(width * column, row * height);
            btn.Width = width;
            btn.Height = height;
            btn.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Arial", height / 3, FontStyle.Bold);

            btn.Click += new EventHandler(btnClicked);

            Controls.Add(btn);
        }

        void btnClicked(object sender, EventArgs e)
        {
            if (OnKeyButton != null)
            {
                KeyButton btn = sender as KeyButton;

                OnKeyButton(sender, new KeyboardEventArgs(btn.Text[0]));
            }
        }

        private void UpdateButtons()
        {
            int width = Width / RowSize;
            int height = Height / NumRows;

            int column = 0;
            int row = 0;

            foreach (Control ctrl in Controls) 
            {
                if (ctrl is KeyButton)
                {
                    ctrl.Location = new Point(width * column, row * height);
                    ctrl.Width = width;
                    ctrl.Height = height;
                    ctrl.Font = new Font("Arial", height / 3, FontStyle.Bold);
                }

                column += 1;
                if (column >= RowSize)
                {
                    row += 1;
                    column = 0;
                }
            }
        }

        private void Keyboard_Resize(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateEyeCursor();
        }

        private void UpdateEyeCursor()
        {
            if (!GazeHelper.Enabled)
                return;

            Point coord = tribe.GetCoordinates();

            // Update cursor window
            cursor.Left = coord.X;
            cursor.Top = coord.Y;

            // Track if over the button
            Point relative = this.PointToClient(coord);

            if (relative.X >= 0 && relative.X < Width && relative.Y >= 0 && relative.Y < Height)
            {
                KeyButton btn = GetChildAtPoint(relative) as KeyButton;
                if (btn != null)
                {
                    if (btn == currentButton)
                    {
                        if (btn.KeyUpdate() && !buttonUsed)
                        {
                            btn.Blink();
                            btnClicked(btn, null);
                            buttonUsed = true;
                        }                        
                    }
                    else
                    {
                        ActivateButton(btn);
                    }
                }
                else
                {
                    DeactivateButton();
                }
            }
            else
            {
                DeactivateButton();
            }
        }

        void ActivateButton(KeyButton btn)
        {
            if (currentButton != null)
            {
                currentButton.Deactivate();
            }

            currentButton = btn;
            buttonUsed = false;
            btn.Activate();
        }

        void DeactivateButton()
        {
            if (currentButton != null)
            {
                currentButton.Deactivate();
            }

            currentButton = null;
        }
    }
}

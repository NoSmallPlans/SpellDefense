using CocosSharp;
using SpellDefense.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// This is the code for your desktop app.
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.

namespace SpellDefense.PC
{
    public partial class Form1 : Form
    {
        CCGameView gameView;
        public Form1()
        {
            InitializeComponent();
            // Get our game view from the layout resource,
            // and attach the view created event to it
            CCGameView gameView = new CCGameView();
            gameView.ViewCreated += LoadGame;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Click on the link below to continue learning how to build a desktop app using WinForms!
            System.Diagnostics.Process.Start("http://aka.ms/dotnet-get-started-desktop");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thanks!");
        }


        private void LoadGame(object sender, EventArgs e)
        {
            CCGameView gameView = sender as CCGameView;

            if (gameView != null)
            {
                GameController.Initialize(gameView);
            }
        }
}
}

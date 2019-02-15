using SpellDefense.Server.MyEventArgs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpellDefense.Server
{
    public partial class Form1 : Form
    {
        private Task _task;
        private Server _server;
        private ManagerLogger _managerLogger;
        private CancellationTokenSource _cancellationTokenSource;

        public Form1()
        {
            _managerLogger = new ManagerLogger();
            _managerLogger.NewLogMessageEvent += NewLogMessageEvent;
            InitializeComponent();
        }

        void NewLogMessageEvent(object sender, LogMessageEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<LogMessageEventArgs>(NewLogMessageEvent), sender, e);
                return;
            }
            dgwServerStatusLog.Rows.Add(new[] { e.LogMessage.Id, e.LogMessage.Message });
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            btnStartServer.Enabled = false;
            btnStopServer.Enabled = true;

            _cancellationTokenSource = new CancellationTokenSource();
            _task = new Task(_server.ReadMessages, _cancellationTokenSource.Token);
            _task.Start();
        }

        private void btnStopServer_Click(object sender, EventArgs e)
        {
            if (_task != null && _cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                btnStartServer.Enabled = true;
                btnStopServer.Enabled = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _server = new Server(_managerLogger);
        }
    }
}

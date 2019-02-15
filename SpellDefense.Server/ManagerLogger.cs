//------------------------------------------------------
// 
// Copyright - (c) - 2014 - Mille Boström 
//
// Youtube channel - http://www.speedcoding.net
//------------------------------------------------------
using System;
using System.Collections.Generic;
using SpellDefense.Server.MyEventArgs;
using SpellDefense.Server.Util;

namespace SpellDefense.Server
{
    class ManagerLogger
    {
        private List<LogMessage> _logMessages;
        public event EventHandler<LogMessageEventArgs> NewLogMessageEvent;

        public ManagerLogger()
        {
            _logMessages = new List<LogMessage>();
        }

        public void AddLogMessage(LogMessage logMessage)
        {
            _logMessages.Add(logMessage);

            if (NewLogMessageEvent != null)
            {
                NewLogMessageEvent(this,new LogMessageEventArgs(logMessage));
            }
        }

        public void AddLogMessage(string id, string message)
        {
           AddLogMessage(new LogMessage {Id = id, Message = message});
        }
    }
}

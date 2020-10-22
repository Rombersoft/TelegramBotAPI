using System;
using System.Threading;
using TelegramRSS;

namespace TelegramHost
{
    class MainClass
    {
        static AutoResetEvent _eventResetHandler;

        public static void Main(string[] args)
        {
            _eventResetHandler = new AutoResetEvent(false);
            TelegramAPI telegramAPI = new TelegramAPI("278305922:AAEQmtHVWsJnbbWJ7PMZVDYxi2PKfRGI67M", null);
            telegramAPI.RunListener(HandleAction);
            _eventResetHandler.WaitOne();
        }

        static void HandleAction(long arg1, int arg2, string arg3)
        {
            
        }
    }
}

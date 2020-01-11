using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace TelegramRSS
{
    /// <summary>
    /// Telegram API.
    /// </summary>
    public class TelegramAPI
    {
        TelegramBotClient _bot;
        long _chatId;
        bool _debug;
        Action<long, int, string> _callback;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TelegramRSS.TelegramAPI"/> class.
        /// </summary>
        /// <param name="key">Generated key by botFather</param>
        /// <param name="debug">If set to <c>true</c> it will be Console output</param>
        public TelegramAPI(string key, bool debug = false)
        {
            _bot = new TelegramBotClient(key);
            _debug = debug;
        }

        /// <summary>
        /// Starts the message listener.
        /// </summary>
        /// <param name="callback">Callback for message doworking. Where 1st arg - chat id, 2nd arg - message id, 3rd arg - message text</param>
        /// <param name="chatId">Chat identifier for receiving.</param>
        public void RunListener(Action<long, int, string> callback, long chatId = 0)
        {
            _chatId = chatId;
            _callback = callback;
            _bot.OnMessage += MessageReceived;
            _bot.StartReceiving();
            if (_debug) Console.WriteLine("Message listener is started");
        }

        private void MessageReceived(object sender, MessageEventArgs args)
        {
            if (_debug) Console.WriteLine("{0} from {1}", args.Message.Text, args.Message.Chat.Id);
            if (_chatId == 0 || _chatId == args.Message.Chat.Id)
                _callback(args.Message.Chat.Id, args.Message.MessageId, args.Message.Text);
        }

        /// <summary>
        /// Send the specified chatId, text and messageId.
        /// </summary>
        /// <returns>The send.</returns>
        /// <param name="chatId">Chat identifier.</param>
        /// <param name="text">Message text.</param>
        /// <param name="messageId">Message identifier.</param>
        public async void Send(long chatId, string text, int messageId)
        {
            await _bot.SendTextMessageAsync(chatId, text, replyToMessageId: messageId);
        }
    }
}
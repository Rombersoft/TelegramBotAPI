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
        Action<string> _logging;
        Action<long, int, string> _callback;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TelegramRSS.TelegramAPI"/> class.
        /// </summary>
        /// <param name="key">Generated key by botFather</param>
        /// <param name="debug">If set to <c>true</c> it will be Console output</param>
        public TelegramAPI(string key, Action<string> logging)
        {
            _bot = new TelegramBotClient(key);
            _logging = logging;
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
            _logging?.Invoke("Message listener is started");
        }

        private void MessageReceived(object sender, MessageEventArgs args)
        {
            _logging?.Invoke(string.Format("{0} from {1}", args.Message.Text, args.Message.Chat.Id));
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
            try
            {
                ChatId chat = new ChatId(chatId);
                await _bot.SendTextMessageAsync(chatId, text, replyToMessageId: messageId);
            }
            catch (Exception ex)
            {
                _logging?.Invoke(ex + " for chatId " + chatId);
            }
        }
    }
}
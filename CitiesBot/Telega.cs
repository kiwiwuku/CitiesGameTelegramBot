using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
namespace CitiesBot
{
    class Telega
    {
        public delegate void GotCommandDgt(Telega _bot, string cmd, string[] args, long chatid);
        public event GotCommandDgt GotCommand;

        public delegate void GotMessageDgt(Telega _bot, string msg, long chatid);
        public event GotMessageDgt GotMessage;

        private TelegramBotClient _bot;
        int lastMessageID = -1;

        public Telega(string key)
        {
            _bot = new TelegramBotClient(key);
            _bot.OnMessage += OnMessage;
            _bot.StartReceiving();
        }
        private void OnMessage(object sender, MessageEventArgs e)
        {
            MessageHandler(e.Message.Text, e.Message.Chat.Id);
        }

        private void MessageHandler(string message, long chatid)
        {
            if (message.StartsWith("/"))
            {
                var cmd = (message + " ").Split(' ', (char)StringSplitOptions.RemoveEmptyEntries)[0].ToLower().Substring(1);
                var args = message.Substring(cmd.Length + 1).Trim().Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                GotCommand?.Invoke(this, cmd, args, chatid);
            }
            else
                GotMessage?.Invoke(this, message, chatid);
        }
        async internal Task<int> TextMessage(string message, long chatid)
        {
            try
            {
                var sentMsg = await _bot.SendTextMessageAsync(chatid, message, Telegram.Bot.Types.Enums.ParseMode.Html, true);
                UpdateLastMessageID(sentMsg);
                return sentMsg.MessageId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        private void UpdateLastMessageID(Message sentMsg)
        {
            if (lastMessageID < sentMsg.MessageId) lastMessageID = sentMsg.MessageId;
        }
    }
}

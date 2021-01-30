using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiesBot
{
    public class CommandProcessor
    {
        string[] _commands = { "newgame", "add", "start", "help" };

        public delegate void CmdDgt(long chatid);
        public delegate void CmdArgsDgt(long chatid, string[] args);
        public event CmdDgt NewGame;
        public event CmdDgt Start;
        public event CmdDgt Help;
        public event CmdArgsDgt Add;

        public bool CanProcess(string commandname)
        {
            return _commands.Contains(commandname);
        }

        public void ProcessCommand(string commandname, string[] args, long chatid)
        {
            if (!CanProcess(commandname)) throw new ArgumentException("Возникла ошибка с командой " + commandname);

            switch (commandname)
            {
                case "newgame":
                    NewGame?.Invoke(chatid);
                    break;
                case "add":
                    Add?.Invoke(chatid, args);
                    break;
                case "start":
                    Start?.Invoke(chatid);
                    break;
                case "help":
                    Help?.Invoke(chatid);
                    break;
            }
        }
    }
}

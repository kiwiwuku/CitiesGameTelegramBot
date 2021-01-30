using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiesBot
{
    public class CommandProcessor
    {
        string[] _commands = { "сброс", "добавить" };

        public delegate void CmdDgt(long chatid);
        public delegate void CmdArgsDgt(long chatid, string[] args);
        public event CmdDgt Clear;
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
                case "сброс":
                    Clear?.Invoke(chatid);
                    break;
                case "добавить":
                    Add?.Invoke(chatid, args);
                    break;
            }
        }
    }
}

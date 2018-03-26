using System;
using System.Collections.Generic;
using DevChatter.Bot.Core.Commands;
using DevChatter.Bot.Core.Model;

namespace DevChatter.Bot.Core.Extensions
{
    public static class ListOfStringExtensions
    {
        public static SimpleCommand ToSimpleCommand(this List<string> arguments)
        {
            if (arguments.Count >= 3 && Enum.TryParse(arguments[2], out UserRole role))
            {
                var simpleCommand = new SimpleCommand(arguments[0], arguments[1], role);
                return simpleCommand;
            }

            return null;
        }
    }
}
using System.Collections.Generic;

namespace DevChatter.Bot.Core.Util
{
    public static class CommandParser
    {
        public static (string commandWord, List<string> arguments) Parse(string commandString, int startIndex = 0)
        {
            if (startIndex < 0 || string.IsNullOrWhiteSpace(commandString))
            {
                return (string.Empty, new List<string>());
            }

            int commandWordEndIndex = commandString.IndexOf(' ', startIndex);
            if (commandWordEndIndex == -1)
            {
                commandWordEndIndex = commandString.Length - 1;
            }

            int commandWordLength = commandWordEndIndex - startIndex + 1;
            string commandWord = commandString.Substring(startIndex, commandWordLength).Trim();
            if (commandWordEndIndex == commandString.Length - 1)
            {
                // return early because we have no arguments
                return (commandWord, new List<string>());
            }

            string remainingCommand = commandString.Substring(commandWordLength).Trim();
            var arguments = SplitArguments(remainingCommand);

            return (commandWord, arguments);
        }

        private static List<string> SplitArguments(string arguments)
        {
            const char argumentDelimiter = ' ';
            const char quoteCharacter = '"';

            if (string.IsNullOrWhiteSpace(arguments))
                return new List<string>();

            List<string> splitArguments = new List<string>();
            bool wasInQuotedSection = false;
            bool inQuotedSection = false;
            int argumentStart = -1;

            for (int i = 0; i < arguments.Length; ++i)
            {
                if (arguments[i] == quoteCharacter)
                {
                    if (!inQuotedSection)
                    {
                        if (i == 0 || arguments[i - 1] == argumentDelimiter)
                        {
                            inQuotedSection = true;
                            wasInQuotedSection = false;
                        }
                    }
                    else
                    {
                        if (i == arguments.Length - 1 || arguments[i + 1] == argumentDelimiter)
                        {
                            wasInQuotedSection = true;
                            inQuotedSection = false;
                        }
                    }
                }
                else if(arguments[i] == argumentDelimiter && !inQuotedSection)
                {
                    int argumentLength = i - argumentStart;
                    if (wasInQuotedSection)
                    {
                        // If we were previously in a quoted section we 
                        // need to offset our length by -1 otherwise we get the closing quote
                        argumentLength -= 1;
                    }

                    var argument = arguments.Substring(argumentStart, argumentLength);
                    splitArguments.Add(argument);
                    argumentStart = -1;
                    wasInQuotedSection = false;
                    inQuotedSection = false;
                }
                else if(arguments[i] != argumentDelimiter && argumentStart == -1)
                {
                    // we only want to record the start index 
                    // if the current character is NOT the delimiter
                    argumentStart = i;
                }
            }

            if(argumentStart != -1)
            {
                int argumentLength = arguments.Length - argumentStart;
                if(wasInQuotedSection)
                {
                    argumentLength -= 1;
                }

                var argument = arguments.Substring(argumentStart, argumentLength);
                splitArguments.Add(argument);
            }

            return splitArguments;
        }
    }
}

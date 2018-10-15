using System;
using System.Linq.Expressions;
using DevChatter.Bot.Core.Automation;

namespace DevChatter.Bot.Core.BotModules.VotingModule
{
    public class DelayedVoteEnd : OneTimeCallBackAction
    {
        public DelayedVoteEnd(int delayInSeconds, VotingSystem votingSystem)
            : base(delayInSeconds, () => votingSystem.EndVoting())
        {
        }
    }
}

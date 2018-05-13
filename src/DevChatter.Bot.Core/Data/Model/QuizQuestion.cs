using System;
using System.Collections.Generic;
using System.Linq;

namespace DevChatter.Bot.Core.Data.Model
{
    public class QuizQuestion : DataEntity
    {
        public string MainQuestion { get; set; }
        public string Hint1 { get; set; }
        public string Hint2 { get; set; }
        public string CorrectAnswer { get; set; }
        public string WrongAnswer1 { get; set; }
        public string WrongAnswer2 { get; set; }
        public string WrongAnswer3 { get; set; }

        public Dictionary<char, string> LetterAssignment { get; private set; }

        /// <summary>
        /// Randomizes the list of answers, doing their letter assignment.
        /// </summary>
        /// <returns>String containing the letters and assigned answers.</returns>
        public string GetRandomizedAnswers()
        {
            if (LetterAssignment == null)
            {
                var randomSet = new[] {CorrectAnswer, WrongAnswer1, WrongAnswer2, WrongAnswer3}
                    .OrderBy(x => Guid.NewGuid()).ToList();
                LetterAssignment = new Dictionary<char, string>
                {
                    ['a'] = randomSet[0],
                    ['b'] = randomSet[1],
                    ['c'] = randomSet[2],
                    ['d'] = randomSet[3],
                };
            }

            return string.Join(", ", LetterAssignment.Select(assignment => $"{assignment.Key}) {assignment.Value}"));
        }
    }
}

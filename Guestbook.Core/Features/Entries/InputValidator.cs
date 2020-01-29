using Guestbook.Core.Persistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guestbook.Core.Features.Entries
{
    public class InputValidator
    {
        private const int MaxEntryTextLength = 140;

        public Result ValidateEntryText(string entryText)
        {
            var result = new Result();

            if (string.IsNullOrWhiteSpace(entryText))
            {
                result.ValidationMessages.Add("You cannot enter an empty string as an entry.");
                result.Success = false;
                return result;
            }

            if (!ValidateEntryTextLength(entryText))
            {
                result.ValidationMessages.Add("Your entry cannot exceed 140 characters.");
                result.Success = false;
            }

            return result;
        }

        private bool ValidateEntryTextLength(string entryText)
        {
            return entryText.Length <= MaxEntryTextLength;
        }
    }
    public class Result
    {
        public bool Success { get; set; } = true;
        public List<string> ValidationMessages { get; set; } = new List<string>();
    }
}

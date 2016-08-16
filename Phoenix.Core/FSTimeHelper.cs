using System.Linq;

namespace Phoenix.Core
{
    public class FSTimeHelper
    {
        public int Hour { get; private set; }
        public int Minute { get; private set; }
        private readonly char _timeDelimiter;

        public FSTimeHelper()
        {
            _timeDelimiter = ':';
        }

        public FSTimeHelper(char timeDelimiter)
        {
            _timeDelimiter = timeDelimiter;
        }

        public void ProcessStringRepresentationOfTime(string time)
        {
            var hour = 4;
            var minute = 0;
            if (time.Contains(_timeDelimiter))
            {
                int.TryParse(time.Substring(0, time.IndexOf(_timeDelimiter)), out hour);
                int.TryParse(time.Substring(time.IndexOf(_timeDelimiter) + 1), out minute);
            }
            else
            {
                int.TryParse(time, out hour);
            }
            Hour = hour;
            Minute = minute;
        }
    }
}

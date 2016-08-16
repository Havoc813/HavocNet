namespace Summits.Models
{
    public class Stat
    {
        private readonly string _name;
        private readonly int _total;
        private readonly int _visited;
        private readonly int _steppedIn;
        private readonly int _highpointed;
        private readonly int _climbed;

        public Stat(
            string name,
            int total,
            int visited,
            int steppedIn,
            int highpointed,
            int climbed
            )
        {
            _name = name;
            _total = total;
            _visited = visited;
            _steppedIn = steppedIn;
            _highpointed = highpointed;
            _climbed = climbed;
        }

        public string name
        {
            get { return _name; }
        }

        public int total
        {
            get { return _total; }
        }

        public int visited
        {
            get { return _visited; }
        }

        public int steppedIn
        {
            get { return _steppedIn; }
        }

        public int highpointed
        {
            get { return _highpointed; }
        }

        public int climbed
        {
            get { return _climbed; }
        }
    }
}

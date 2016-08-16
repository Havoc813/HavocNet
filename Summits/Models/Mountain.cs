namespace Summits.Models
{
    public class Mountain
    {
        private readonly int _id;
        private readonly string _name;
        private readonly double _longitude;
        private readonly double _latitude;
        private readonly int _zoom;

        public Mountain(
            int id,
            string name,
            double longitude,
            double latitude,
            int zoom
            )
        {
            _id = id;
            _name = name;
            _longitude = longitude;
            _latitude = latitude;
            _zoom = zoom;
        }

        public int ID
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
        }

        public double Longitude
        {
            get { return _longitude; }
        }

        public double Latitude
        {
            get { return _latitude; }
        }

        public int Zoom
        {
            get { return _zoom; }
        }
    }
}

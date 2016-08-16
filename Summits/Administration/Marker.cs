using System.Collections;

namespace Summits.Administration
{
    public class Marker
    {
        private string _name;
        private string _description;
        private string _longitude;
        private string _latitude;
        private string _colour;

        public Marker(string name, string description, string longitude, string latitude, string colour = "blue")
        {
            _name = name;
            _description = description;
            _longitude = longitude;
            _latitude = latitude;
            _colour = colour;
        }

        public ArrayList Publish()
        {
            var arr = new ArrayList
                {
                    "<Placemark>",
                    "<name>" + this._name + "</name>",
                    "<description>",
                    "<![CDATA[",
                    this._description,
                    "]]>",
                    "</description>",
                    "<styleUrl>#" + this._colour + "</styleUrl>",
                    "<Point>",
                    "<coordinates>" + this._longitude + "," + this._latitude + ",0</coordinates>",
                    "</Point>",
                    "</Placemark>"
                };
            return arr;
        }
    }
}

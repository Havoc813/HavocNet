using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace Summits.Administration
{
    public class Shape
    {
        public List<Polygon> Polygons = new List<Polygon>();
        private string _name;
        private string _description;
        private string _colour;
        private double _width;

        public Shape(string name, string description, Color colour, double width)
        {
            _name = name;
            _description = description;
            _width = width;

            var hex = string.Format("{0:x6}", colour.ToArgb());
            _colour = "BB" + hex.Substring(6, 2) + hex.Substring(4, 2) + hex.Substring(2, 2);
        }

        public ArrayList Publish()
        {
            var arr = new ArrayList
                {
                    "<Placemark>",
                    "<name>" + _name + "</name>",
                    "<description>",
                    "<![CDATA[",
                    _description,
                    "]]>",
                    "</description>",
                    "<Style>",
                    "<LineStyle>",
                    "<width>" + _width + "</width>",
                    "<color>CCFF0000</color>",
                    "</LineStyle>",
                    "<PolyStyle>",
                    "<color>" + _colour + "</color>",
                    "<colorMode>random</colorMode>",
                    "</PolyStyle>",
                    "</Style>"
                };

            if(Polygons.Count > 1) arr.Add("<MultiGeometry>");
            foreach (var polygon in this.Polygons){ arr.AddRange(polygon.Publish()); }
            if(Polygons.Count > 1) arr.Add("</MultiGeometry>");

            arr.Add("</Placemark>");

            return arr;
        }
    }

    public class Polygon
    {
        public string border;
        public Dictionary<int, string> cutOuts = new Dictionary<int, string>();

        public ArrayList Publish()
        {
            var arr = new ArrayList
                {
                    "<Polygon>",
                    "<outerBoundaryIs>",
                    "<LinearRing>",
                    "<coordinates>",
                    border,
                    "</coordinates>",
                    "</LinearRing>",
                    "</outerBoundaryIs>"
                };

            foreach (var cutOut in cutOuts)
            {
                arr.Add("<innerBoundaryIs>");
                arr.Add("<LinearRing>");
                arr.Add("<coordinates>");
                arr.Add(cutOut.Value);
                arr.Add("</coordinates>");
                arr.Add("</LinearRing>");
                arr.Add("</innerBoundaryIs>");
            }

            arr.Add("</Polygon>");

            return arr;
        }

    }
}

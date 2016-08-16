using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Summits.Administration
{
    public class KML
    {
        public List<Marker> Markers = new List<Marker>();
        public List<Shape> Shapes = new List<Shape>();
        private string _name;

        public KML(string name)
        {
            _name = name;
        }

        public void Generate(string path)
        {
            var file = new System.IO.StreamWriter(path);

            try
            {
                file.WriteLine("<?xml version='1.0' encoding='UTF-8'?>");
                file.WriteLine("<kml xmlns='http://www.opengis.net/kml/2.2' xmlns:gx='http://www.google.com/kml/ext/2.2' xmlns:kml='http://www.opengis.net/kml/2.2' xmlns:atom='http://www.w3.org/2005/Atom'>");
                file.WriteLine("<Document>");
                file.WriteLine("<name>" + this._name + "</name>");
                file.WriteLine("<open>1</open>");

                file.WriteLine("<Style>");
                file.WriteLine("<ListStyle>");
                file.WriteLine("<listItemType>checkHideChildren</listItemType>");
                file.WriteLine("<bgColor>00ffffff</bgColor>");
                file.WriteLine("<maxSnippetLines>2</maxSnippetLines>");
                file.WriteLine("</ListStyle>");
                file.WriteLine("</Style>");

                file.WriteLine("<Style id='red'>");
                file.WriteLine("<IconStyle>");
                file.WriteLine("<Icon>");
                file.WriteLine("<href>http://www.google.com/intl/en_us/mapfiles/ms/icons/red-dot.png</href>");
                file.WriteLine("</Icon>");
                file.WriteLine("</IconStyle>");
                file.WriteLine("</Style>");

                file.WriteLine("<Style id='green'>");
                file.WriteLine("<IconStyle>");
                file.WriteLine("<Icon>");
                file.WriteLine("<href>http://www.google.com/intl/en_us/mapfiles/ms/icons/green-dot.png</href>");
                file.WriteLine("</Icon>");
                file.WriteLine("</IconStyle>");
                file.WriteLine("</Style>");

                file.WriteLine("<Style id='blue'>");
                file.WriteLine("<IconStyle>");
                file.WriteLine("<Icon>");
                file.WriteLine("<href>http://www.google.com/intl/en_us/mapfiles/ms/icons/blue-dot.png</href>");
                file.WriteLine("</Icon>");
                file.WriteLine("</IconStyle>");
                file.WriteLine("</Style>");

                file.WriteLine("<Style id='yellow'>");
                file.WriteLine("<IconStyle>");
                file.WriteLine("<Icon>");
                file.WriteLine("<href>http://www.google.com/intl/en_us/mapfiles/ms/icons/blue-dot.png</href>");
                file.WriteLine("</Icon>");
                file.WriteLine("</IconStyle>");
                file.WriteLine("</Style>");

                file.WriteLine("<Folder>");
                file.WriteLine("<name>Markers</name>");

                foreach (var marker in Markers)
                {
                    foreach (var line in marker.Publish())
                    {
                        file.WriteLine(line);
                    }
                }

                file.WriteLine("</Folder>");

                file.WriteLine("<Folder>");
                file.WriteLine("<name>Shapes</name>");

                foreach (var shape in Shapes)
                {
                    foreach (var line in shape.Publish())
                    {
                        file.WriteLine(line);
                    }
                }

                file.WriteLine("</Folder>");

                file.WriteLine("</Document>");
                file.WriteLine("</kml>");
            }
            catch (Exception)
            {
                //TODO: Error Log

                throw;
            }
            finally
            {
                file.Close();
            }
        }
    }
}

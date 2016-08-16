namespace Athletica
{
    public class Route
    {
        public string Location { get; set; }
        public int LocationID { get; set; }
        public double Distance { get; set; }

        public Route(double distance, int locationID, string locationName)
        {
            Location = locationName;
            LocationID = locationID;
            Distance = distance;
        }
    }
}

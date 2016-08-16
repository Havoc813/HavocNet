namespace Phoenix.Core.Models
{
    public class EmployeeCostCentre : IEmployeeCostCentre
    {
        public int ID { get; set; }
        public string Division { get; set; }
        public string CostCentre { get; set; }

        public string LongName
        {
            get { return Division + '.' + CostCentre; }
        }

        public EmployeeCostCentre(int id, string division, string costCentre)
        {
            ID = id;
            Division = division;
            CostCentre = costCentre;
        }
    }
}

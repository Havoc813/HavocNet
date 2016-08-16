namespace Phoenix.Core.Models
{
    public interface IEmployeeCostCentre
    {
        int ID { get; set; }
        string Division { get; set; }
        string CostCentre { get; set; }
        string LongName { get; }
    }
}
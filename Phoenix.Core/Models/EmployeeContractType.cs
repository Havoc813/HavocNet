namespace Phoenix.Core.Models
{
    public static class EmployeeContractType
    {
        public static string Permanent { get { return "Permanent"; } }
        public static string Temporary { get { return "Temporary"; } }
        public static string ExternalApprover { get { return "External Approver"; } }
        public static string Secondee { get { return "Secondee"; } }
        public static string ToppStudent { get { return "TOPP Student"; } }

        public static string[] All
        {
            get { return new[] { Permanent, Temporary, ExternalApprover, Secondee, ToppStudent }; }
        }

        public static string[] InternalTypes
        {
            get { return new[] {Permanent, Temporary, Secondee}; }
        }
    }
}

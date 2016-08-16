using System.Collections.Generic;
using System.Linq;

namespace Phoenix.Core.Models
{
    public class DistributionList : IDistributionList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> MemberEmailAddresses { get; set; }

        public DistributionList(int id, string name, string description)
        {
            Init(id, name, description);
            MemberEmailAddresses = new List<string>();
        }

        public DistributionList(int id, string name, string description, List<string> memberAddressList )
        {
            Init(id, name, description);
            MemberEmailAddresses = memberAddressList;
        }

        public DistributionList(int id, string name, string description, string memberAddresses, char memberAddressSeparator)
        {
            Init(id, name, description);
            MemberEmailAddresses = new List<string>();
            foreach (var email in memberAddresses.Split(memberAddressSeparator).Where(email => email.Length > 0))
            {
                MemberEmailAddresses.Add(email);
            }
            //foreach (var email in memberAddresses.Split(memberAddressSeparator))
            //{
            //    if (email.Length > 0)
            //        MemberEmailAddresses.Add(email);
            //}
        }

        private void Init(int id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;
        }
    }
}

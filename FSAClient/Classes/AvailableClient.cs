using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSAClient.Classes
{
    public class AvailableClient
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public AvailableClient(int id, string name)
        {
            Id = id; Name = name;
        }
    }
}

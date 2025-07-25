using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public class StokGuncellemeBasarisizEvent
    {
        public int SiparisId { get; set; }
        public string Gerekce { get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public class IadeOnaylandiEvent
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public List<IadeEdilenUrun> Products { get; set; } = new();
    }

    public class IadeEdilenUrun
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}

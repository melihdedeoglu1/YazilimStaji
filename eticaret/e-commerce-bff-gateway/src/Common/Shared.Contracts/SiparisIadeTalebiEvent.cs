﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public class SiparisIadeTalebiEvent
    {       
        public int UserId { get; set; }
        public int OrderId { get; set; }

        public double RefundPrice { get; set; }

        public List<IptalUrun> Products { get; set; } = new List<IptalUrun>();
    }

    public class IptalUrun
    {

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int ProductQuantity { get; set; }
    }
}

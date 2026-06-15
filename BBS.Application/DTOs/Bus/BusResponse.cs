using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBS.Application.DTOs.Bus
{
    public class BusResponse
    {
        public int BusId { get; set; }

        public string BusNumber { get; set; }

        public string BusName { get; set; }

        public int TotalSeats { get; set; }
    }
}

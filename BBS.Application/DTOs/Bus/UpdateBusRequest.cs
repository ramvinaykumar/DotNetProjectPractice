using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBS.Application.DTOs.Bus
{
    public class UpdateBusRequest
    {
        public string BusNumber { get; set; } = string.Empty;

        public string BusName { get; set; } = string.Empty;

        public int TotalSeats { get; set; }
    }
}

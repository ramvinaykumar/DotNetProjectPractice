using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBS.Application.DTOs.Route
{
    public class UpdateRouteRequest
    {
        public string SourceCity { get; set; }

        public string DestinationCity { get; set; }

        public decimal DistanceKM { get; set; }
    }
}

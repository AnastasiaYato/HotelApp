using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services_IntegrationTests.Utils
{
    /// <summary>
    /// Just a simple class like StrinPair to make my life easier
    /// </summary>
    public class RequestPayload
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("size")]
        public int? Size { get; set; }

        [JsonProperty("available")]
        public bool? Available { get; set; }
    }
}

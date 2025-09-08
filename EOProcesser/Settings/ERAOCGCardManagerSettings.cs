using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EOProcesser.Settings
{
    public class ERAOCGCardManagerSettings
    {
        [JsonInclude]
        public string RootFolder { get; set; } = "";
        [JsonInclude]
        public string CardFolder { get; set; } = "";
        [JsonInclude]
        public string DeckFolder { get; set; } = "";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CustomJiraTicketClient.Models.FieldTypes
{
    public class StatusType
    {
        [JsonPropertyName("self")]
        public string Self {  get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}

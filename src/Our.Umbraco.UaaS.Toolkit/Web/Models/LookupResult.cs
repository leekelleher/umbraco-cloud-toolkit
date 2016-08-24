using System;
using Newtonsoft.Json;

namespace Our.Umbraco.UaaS.Toolkit
{
    internal class LookupResult
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("key")]
        public Guid Key { get; set; }

        [JsonProperty("path")]
        public int[] Path { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("editUrl")]
        public string EditUrl { get; set; }
    }
}
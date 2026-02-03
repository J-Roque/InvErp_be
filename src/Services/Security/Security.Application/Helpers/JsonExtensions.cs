using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Security.Application.Helpers
{
    public static class JsonExtensions
    {
        public static string? ToJsonStringOrNull(this Dictionary<string, JsonNode?> dict)
        {
            if (dict == null || dict.Count == 0)
                return null;

            var jsonObject = new JsonObject();

            foreach (var kv in dict)
            {
                if (kv.Value != null)
                    jsonObject[kv.Key] = kv.Value.Deserialize<JsonNode>();
            }

            return jsonObject.ToJsonString();
        }

    }
}

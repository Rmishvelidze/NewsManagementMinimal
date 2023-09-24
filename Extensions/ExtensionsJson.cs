using Newtonsoft.Json;

namespace NewsManagementMinimal.Extensions
{
    public static class ExtensionsJson
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        public static string ToJson(this object text, Formatting format = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(text, format, Settings);
        }
    }
}

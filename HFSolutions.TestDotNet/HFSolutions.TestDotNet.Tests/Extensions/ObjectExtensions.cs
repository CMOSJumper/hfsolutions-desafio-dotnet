using System.Text;
using Newtonsoft.Json;

namespace HFSolutions.TestDotNet.Tests.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object _object, JsonSerializerSettings? jsonSerializerSettings = null)
        {
            jsonSerializerSettings ??= new JsonSerializerSettings();

            string objectJson = JsonConvert.SerializeObject(_object, jsonSerializerSettings);

            return objectJson;
        }

        public static HttpContent ToStringHttpContent(this object _object, string mediaType = "application/json", JsonSerializerSettings? jsonSerializerSettings = null)
        {
            jsonSerializerSettings ??= new JsonSerializerSettings();

            string objectJson = _object.ToJson(jsonSerializerSettings);

            var stringContent = new StringContent(objectJson, Encoding.UTF8, mediaType);

            return stringContent;
        }
    }
}

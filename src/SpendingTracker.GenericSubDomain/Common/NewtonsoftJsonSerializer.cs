using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SpendingTracker.GenericSubDomain.Common
{
    internal sealed class NewtonsoftJsonSerializer : IJsonSerializer
    {
        private static readonly JsonSerializerSettings JsonSettings = CreateJsonSettings();

        public string Serialize<T>(T value)
        {
            var result = JsonConvert.SerializeObject(value, JsonSettings);
            return result;
        }

        public dynamic Deserialize(string value, Type type)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("String is null or WhiteSpace", nameof(value));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            var result = JsonConvert.DeserializeObject(value, type, JsonSettings);

            return result;
        }

        public T Deserialize<T>(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("String is null or WhiteSpace", nameof(value));

            var result = JsonConvert.DeserializeObject<T>(value, JsonSettings);

            return result;
        }

        private static JsonSerializerSettings CreateJsonSettings()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            settings.Converters.Add(new StringEnumConverter());

            return settings;
        }
    }
}
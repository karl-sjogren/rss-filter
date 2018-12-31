using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shorthand.RssFilter.Models;

namespace Shorthand.RssFilter.Serialization {
    [UsedImplicitly]
    public class FilterBaseConverter : JsonConverter {
        private static List<KeyValuePair<Type, string>> _typeCache;
        private static readonly object TypeCacheLock = new object();

        public IEnumerable<KeyValuePair<Type, string>> SupportedTypes {
            get {
                if(_typeCache == null) {
                    lock(TypeCacheLock) {
                        if(_typeCache != null)
                            return _typeCache;

                        _typeCache = Assembly.GetAssembly(typeof(FilterBase))
                            .GetTypes().Where(x => x.FullName?.StartsWith("Shorthand.RssFilter.") == true)
                            .Where(t => typeof(FilterBase).IsAssignableFrom(t))
                            .Where(t => !t.IsAbstract)
                            //An instance is created so we can read the name from the subclasses.
                            .Select(t => new KeyValuePair<Type, string>(t, ((FilterBase)Activator.CreateInstance(t)).Type))
                            .ToList();
                    }
                }
                return _typeCache;
            }
        }

        public override bool CanConvert(Type objectType) {
            return objectType == typeof(FilterBase);
        }

        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if(reader.TokenType != JsonToken.StartObject)
                return null;

            var obj = JObject.Load(reader);

            var name = obj.SelectToken("Type")?.Value<string>();
            if(string.IsNullOrWhiteSpace(name))
                return null;

            var type = SupportedTypes.Where(kvp => kvp.Value == name).Select(kvp => kvp.Key).FirstOrDefault();
            if(type == null)
                return null;

            return serializer.Deserialize(obj.CreateReader(), type);
        }
    }
}

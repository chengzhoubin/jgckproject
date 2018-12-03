using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JGCK.Web.General.MVC
{
    public class JsonNetValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");

            if (!controllerContext.HttpContext.Request.ContentType.StartsWith("application/json",
                StringComparison.OrdinalIgnoreCase))
                return null;

            var streamReader = new StreamReader(controllerContext.HttpContext.Request.InputStream);
            var jsonReader = new JsonTextReader(streamReader);
            if (!jsonReader.Read())
                return null;

            var jsonSerializer = new JsonSerializer();
            jsonSerializer.Converters.Add(new ExpandoObjectConverter());

            Object jsonObject;
            if (jsonReader.TokenType == JsonToken.StartArray)
                jsonObject = jsonSerializer.Deserialize<List<ExpandoObject>>(jsonReader);
            else
                jsonObject = jsonSerializer.Deserialize<ExpandoObject>(jsonReader);

            var backingStore = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            AddToBackingStore(backingStore, String.Empty, jsonObject);
            return new DictionaryValueProvider<object>(backingStore, CultureInfo.CurrentCulture);
        }

        private void AddToBackingStore(Dictionary<string, object> backingStore, string prefix, object value)
        {
            var d = value as IDictionary<string, object>;
            if (d != null)
            {
                foreach (var entry in d)
                {
                    AddToBackingStore(backingStore, MakePropertyKey(prefix, entry.Key), entry.Value);
                }

                return;
            }

            var l = value as IList;
            if (l != null)
            {
                for (int i = 0; i < l.Count; i++)
                {
                    AddToBackingStore(backingStore, MakeArrayKey(prefix, i), l[i]);
                }

                return;
            }

            backingStore[prefix] = value;
        }

        private string MakeArrayKey(string prefix, int index)
        {
            return prefix + "[" + index.ToString(CultureInfo.InvariantCulture) + "]";
        }

        private string MakePropertyKey(string prefix, string propertyName)
        {
            return (String.IsNullOrEmpty(prefix)) ? propertyName : prefix + "." + propertyName;
        }
    }
}

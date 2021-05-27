using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VisualDanmakuEditor.Porting
{
    public class TypeToStringConverter : JsonConverter<Type>
    {
        readonly string[] assemblyName;
        readonly ISerializationBinder serializationBinder;

        public TypeToStringConverter(ISerializationBinder serializationBinder = null, params string[] assemblyName)
        {
            this.assemblyName = assemblyName;
            if (serializationBinder == null)
            {
                this.serializationBinder = new DefaultSerializationBinder();
            }
            else
            {
                this.serializationBinder = serializationBinder;
            }
        }

        public override Type ReadJson(JsonReader reader, Type objectType, Type existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Type t = null;
            Exception exception = null;
            for(int i = 0; i < assemblyName.Length; i++)
            {
                try
                {
                    t = serializationBinder.BindToType(assemblyName[i], (string)reader.Value);
                }
                catch (Exception e) 
                {
                    exception = e;
                }
                if (t != null) return t;
            }
            t = serializationBinder.BindToType(null, (string)reader.Value);
            if (t != null) return t;
            throw exception;
        }

        public override void WriteJson(JsonWriter writer, Type value, JsonSerializer serializer)
        {
            serializationBinder.BindToName(value, out _, out string typeName);
            writer.WriteValue(typeName);
        }
    }
}

using System;
using System.IO;
using Newtonsoft.Json;

namespace Memery
{
    [JsonConverter(typeof(ShortCodeJsonConverter))]
    public class ShortCode
    {

        private ShortCode(Guid g)
        {
            Code = Encode(g);
        }

        public static ShortCode Create()
        {
            return new ShortCode(Guid.NewGuid());
        }

        private string Code { get; set; }
        public override string ToString()
        {
            return Code;
        }

        public static implicit operator string(ShortCode c)
        {
            return c.Code;
        }

        public static implicit operator ShortCode(FileInfo fi)
        {
            return new ShortCode(fi.FullName.ToGuid());
        }

        public static bool TryParse(string s, out ShortCode c)
        {
            if (s != null && s.Length == 22)
            {
                var g = Decode(s);
                c = g.HasValue
                    ? new ShortCode(g.Value)
                    : null;
                return g.HasValue;
                // c = new ShortCode(Decode(s).Value);
                // return true;
            }
            c = null;
            return false;
        }

        public static implicit operator Guid(ShortCode c)
        {
            var d = Decode(c.Code);
            return d.HasValue
                ? d.Value
                : throw new InvalidCastException("Not a valid ShortCode!");
        }

        private static string Encode(Guid guid)
        {
            try
            {
                string enc = Convert.ToBase64String(guid.ToByteArray());
                enc = enc.TrimEnd('=');
                enc = enc.Replace("/", "_");
                enc = enc.Replace("+", "-");
                return enc.Substring(0, 22);
            }
            catch
            {
                return null;
            }
        }

        private static Guid? Decode(string encoded)
        {
            try
            {
                encoded = encoded.Replace("_", "/");
                encoded = encoded.Replace("-", "+");
                byte[] buffer = Convert.FromBase64String(encoded + "==");
                return new Guid(buffer);
            }
            catch
            {
                return null;
            }
        }

        internal class ShortCodeJsonConverter : Newtonsoft.Json.JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(ShortCode);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var s = reader.ReadAsString();
                return ShortCode.TryParse(s, out var c)
                    ? c
                    : null;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(((ShortCode)value).Code);
            }
        }
    }
}
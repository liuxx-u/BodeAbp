using System;
using Abp.Timing;
using Abp.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Abp.Json
{
    public class AbpDateTimeConverter : IsoDateTimeConverter
    {
        public AbpDateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var date = AbpReadJson(reader, objectType, existingValue, serializer) as DateTime?;

            if (date.HasValue)
            {
                return Clock.Normalize(date.Value);
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var date = value as DateTime?;
            base.WriteJson(writer, date.HasValue ? Clock.Normalize(date.Value) : value, serializer);
        }

        private bool IsNullableType(Type type)
        {
            type.CheckNotNull(nameof(type));

            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// 重写json转换时间，解决设置DateTimeFormat后使用DateTime.ParseExact强制转换时间失败的bug
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        private object AbpReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            bool nullable = IsNullableType(objectType);
            Type t = (nullable)? Nullable.GetUnderlyingType(objectType): objectType;

            if (reader.TokenType == JsonToken.Null)
            {
                if (!IsNullableType(objectType))
                {
                    throw new JsonSerializationException($"Cannot convert null value to {objectType}.");
                }
                return null;
            }

            if (reader.TokenType == JsonToken.Date)
            {
                if (t == typeof(DateTimeOffset))
                {
                    return (reader.Value is DateTimeOffset) ? reader.Value : new DateTimeOffset((DateTime)reader.Value);
                }

                // converter is expected to return a DateTime
                if (reader.Value is DateTimeOffset)
                {
                    return ((DateTimeOffset)reader.Value).DateTime;
                }
                return reader.Value;
            }

            if (reader.TokenType != JsonToken.String)
            {
                throw new JsonSerializationException($"Unexpected token parsing date. Expected String, got { reader.TokenType}.");
            }

            string dateText = reader.Value.ToString();

            if (string.IsNullOrEmpty(dateText) && nullable)
            {
                return null;
            }
            
            if (t == typeof(DateTimeOffset))
            {
                return DateTimeOffset.Parse(dateText, Culture, DateTimeStyles);
            }

            return DateTime.Parse(dateText, Culture, DateTimeStyles);
        }
    }
}
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace fxl.codes.tumblr.web.Utilities
{
    public class UnixSecondsConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var seconds = reader.GetInt64();
            return DateTime.UnixEpoch.AddSeconds(seconds);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue((value - DateTime.UnixEpoch).TotalSeconds);
        }
    }
}
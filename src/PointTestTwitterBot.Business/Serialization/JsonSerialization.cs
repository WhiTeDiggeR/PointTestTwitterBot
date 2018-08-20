using System.Web.Script.Serialization;

namespace PointTestTwitterBot.Business.Serialization
{
    public static class JsonSerialization
    {
        public static T DeserializeJson<T>(string input)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(input);
        }

        public static string SerializeObject(object obj)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }
    }
}

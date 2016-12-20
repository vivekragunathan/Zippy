using System.Runtime.Serialization;

namespace Zippy.Controllers.Contract
{
    [DataContract]
    public class LocateRequest
    {
        public LocateRequest(string name, string address)
        {
            Name = name;
            Address = address;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Address { get; set; }

        public override string ToString()
        {
            return $"([{Name}] {Address})";
        }
    }
}

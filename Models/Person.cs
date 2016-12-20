using System;
using System.Runtime.Serialization;
using Zippy.Utils;

namespace Zippy.Models
{
    [DataContract]
    public class Person
    {
        public Person()
        {
        }

        public Person(string name, string address, string zip)
        {
            Throw.IfBlank(name, $"{nameof(name)} cannot be blank");
            Throw.IfBlank(address, $"{nameof(address)} cannot be blank");
            Throw.IfBlank(zip, $"{nameof(zip)} cannot be blank");

            Id = new Random(3571).Next();
            Name = name;
            Address = address;
            ZipCode = zip;
        }

        [DataMember]
        public int Id { get; internal set; }

        [DataMember]
        public string Name { get; internal set; }

        [DataMember]
        public string Address { get; internal set; }

        [DataMember]
        public string ZipCode { get; internal set; }

        public override bool Equals(object other)
        {
            if (other is Person)
            {
                var that = (Person)other;
                return string.Equals(this.Name, that.Name);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Name} {ZipCode} ({Address})";
        }
    }
}
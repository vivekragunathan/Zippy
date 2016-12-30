using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Zippy.Utils;

namespace Zippy.Models
{
	[DataContract]
	public class Person
	{
		public const string FIELD_NAME = "name";
		public const string FIELD_ADDRESS = "address";
		public const string FIELD_ZIP = "zipcode";

		public Person()
		{
		}

		public Person(string name, string address, string zip)
		{
			Throw.IfBlank(name, $"{nameof(name)} cannot be blank");
			Throw.IfBlank(address, $"{nameof(address)} cannot be blank");
			Throw.IfBlank(zip, $"{nameof(zip)} cannot be blank");

			Id = ObjectId.GenerateNewId();
			Name = name;
			Address = address;
			ZipCode = zip;
		}

		/*[DataMember]
		public int Id { get; internal set; }*/

		public ObjectId Id { get; internal set; }

		[DataMember]
		[BsonElement(FIELD_NAME)]
		public string Name { get; internal set; }

		[DataMember]
		[BsonElement(FIELD_ADDRESS)]
		public string Address { get; internal set; }

		[DataMember]
		[BsonElement(FIELD_ZIP)]
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
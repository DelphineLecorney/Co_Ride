namespace Trip.Domain.ValueObjects
{
    public sealed class Address : IEquatable<Address>
    {
        public string City { get; } = default!;

        private Address(string city)
        {
            City = city;
        }

        public static Address Create(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be empty", nameof(city));
            return new Address(city);
        }

        public bool Equals(Address? other)
        {
            if (other is null) return false;
            return City == other.City;
        }

        public override bool Equals(object? obj) => Equals(obj as Address);
        public override int GetHashCode() => City.GetHashCode();
    }
}

namespace Trip.Domain.ValueObjects
{
    public sealed class Price : IEquatable<Price>
    {
        public decimal Amount { get; }
        public string Currency { get; }
        private Price(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public static Price Create(decimal amount, string currency = "EUR")
        {
            if (amount < 0)
                throw new ArgumentException("Le prix ne peut pas être négatif", nameof(amount));

            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("La devise ne peut pas être vide", nameof(currency));

            return new Price(amount, currency);
        }
        public bool Equals(Price? other)
        {
            if (other is null) return false;
            return Amount == other.Amount;
        }
        public override bool Equals(object? obj) => Equals(obj as Price);
        public override int GetHashCode() => Amount.GetHashCode();

    }
}
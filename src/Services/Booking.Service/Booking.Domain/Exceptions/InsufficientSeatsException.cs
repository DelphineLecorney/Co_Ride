namespace Booking.Domain.Exceptions
{
    public class InsufficientSeatsException : Exception
    {
        public InsufficientSeatsException(int requested, int available)
            : base($"Nombre de sièges insuffisants. Demandé: {requested}, Disponible: {available}")
        {
        }
    }
}

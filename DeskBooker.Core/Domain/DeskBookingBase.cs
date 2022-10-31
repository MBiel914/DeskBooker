namespace DeskBooker.Core.Domain
{
    public class DeskBookingBase
    {
        public int DeskId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
    }
}
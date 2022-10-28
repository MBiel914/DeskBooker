using DeskBooker.Core.Domain;
using Xunit;

namespace DeskBooker.Core.Processor
{

    public class DeskBookingRequestProcessorTests
    {
        [Fact]
        public void ShouldReturnDeskBookingResultWithRequestValeus()
        {
            var request = new DeskBookingRequest
            {
                FirstName = "Thomas",
                LastName = "Huber",
                Email = "ThomasHuber@Email.com",
                Date = new DateTime(2022, 11, 2)
            };

            var processor = new DeskBookingRequestProcessor();

            DeckBookingResult result = processor.BookDesk(request);

            Assert.NotNull(result);
            Assert.Equal(request.FirstName, result.FirstName);
            Assert.Equal(request.LastName, result.LastName);
            Assert.Equal(request.Email, result.Email);
            Assert.Equal(request.Date, result.Date);
        }
    }
}

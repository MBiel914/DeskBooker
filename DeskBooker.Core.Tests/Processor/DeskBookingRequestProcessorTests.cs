using DeskBooker.Core.DataInterface;
using DeskBooker.Core.Domain;
using Moq;
using Xunit;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessorTests
    {
        private readonly Mock<IDeskBookingRepository> _deskBookingRepositoryMock;
        private readonly Mock<IDeskRepository> _deskRepositoryMock;
        private readonly IList<Desk> _availableDesks;
        private readonly DeskBookingRequestProcessor _processor;
        private readonly DeskBookingRequest _request;

        public DeskBookingRequestProcessorTests()
        {
            _request = new DeskBookingRequest
            {
                FirstName = "Thomas",
                LastName = "Huber",
                Email = "ThomasHuber@Email.com",
                Date = new DateTime(2022, 11, 2)
            };

            _availableDesks = new List<Desk>() { new Desk{
                Id = 7
            } };

            _deskBookingRepositoryMock = new Mock<IDeskBookingRepository>();
            _deskRepositoryMock = new Mock<IDeskRepository>();
            _deskRepositoryMock.Setup(x => x.GetAvailableDesks(_request.Date))
                .Returns(_availableDesks);

            _processor = new DeskBookingRequestProcessor(
                _deskBookingRepositoryMock.Object
                , _deskRepositoryMock.Object);
        }

        [Fact]
        public void ShouldReturnDeskBookingResultWithRequestValeus()
        {
            DeskBookingResult result = _processor.BookDesk(_request);

            TestEndValueWithReturnValue(result);
        }

        [Fact]
        public void ShouldThrowExceptionIfRequestIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookDesk(null));

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public void ShouldSaveDeskBooking()
        {
            DeskBooking? savedDeskBooking = null;

            _deskBookingRepositoryMock.Setup(x => x.Save(It.IsAny<DeskBooking>()))
                .Callback<DeskBooking>(deskBooking =>
                {
                    savedDeskBooking = deskBooking;
                });

            _processor.BookDesk(_request);

            _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Once);

            TestEndValueWithReturnValue<DeskBooking>(savedDeskBooking);

            Assert.Equal(_availableDesks.First().Id, savedDeskBooking.DeskId);
        }

        [Fact]
        public void ShouldNotSaveDeskBookingIfNotDeskIsAvailable()
        {
            _availableDesks.Clear();

            _processor.BookDesk(_request);

            _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Never);
        }

        private void TestEndValueWithReturnValue<T>(T testObject)
            where T : DeskBookingBase
        {
            Assert.NotNull(testObject);
            Assert.Equal(_request.FirstName, testObject.FirstName);
            Assert.Equal(_request.LastName, testObject.LastName);
            Assert.Equal(_request.Email, testObject.Email);
            Assert.Equal(_request.Date, testObject.Date);
        }
    }
}

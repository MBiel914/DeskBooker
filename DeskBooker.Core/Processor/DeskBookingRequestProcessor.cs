using DeskBooker.Core.DataInterface;
using DeskBooker.Core.Domain;
using System;
using System.Linq;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessor
    {
        private readonly IDeskBookingRepository _deskBookingRepository;
        private readonly IDeskRepository _deskRepository;

        public DeskBookingRequestProcessor(IDeskBookingRepository deskBookingRepository, IDeskRepository deskRepositor)
        {
            _deskBookingRepository = deskBookingRepository;
            _deskRepository = deskRepositor;
        }

        public DeskBookingResult BookDesk(DeskBookingRequest? request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var availableDesks = _deskRepository.GetAvailableDesks(request.Date);
            var result = Create<DeskBookingResult>(request);

            if (availableDesks.Any())
            {
                var availableDesk = availableDesks.First();
                var deskBooking = Create<DeskBooking>(request);

                deskBooking.DeskId = availableDesk.Id;

                _deskBookingRepository.Save(deskBooking);

                result.DeskBookingId = deskBooking.Id;
                result.Code = DeskBookingResultCode.Success;
            }
            else
            {
                result.Code = DeskBookingResultCode.NoDeskAvailable;
            }

            return result;
        }

        private static T Create<T>(DeskBookingBase request)
            where T : DeskBookingBase, new()
        {
            return new T
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Date = request.Date
            };
        }
    }
}
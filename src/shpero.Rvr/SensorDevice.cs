using System;
using System.Threading;
using System.Threading.Tasks;
using shpero.Rvr.Commands.SensorDevice;
using shpero.Rvr.Responses.SensorDevice;

namespace shpero.Rvr
{
    public class SensorDevice
    {
        private readonly Driver _driver;

        public SensorDevice(Driver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }

        public Task EnableGyroMaxNotificationsAsync(bool enable, CancellationToken cancellationToken)
        {
            var enableGyroMaxNotifications = new EnableGyroMaxNotifications(enable);
            return _driver.SendAsync(enableGyroMaxNotifications.ToMessage(), cancellationToken);
        }

        public Task ResetLocatorXAndYAsync(CancellationToken cancellationToken)
        {
            var resetLocatorXAndY = new ResetLocatorXAndY();
            return _driver.SendAsync(resetLocatorXAndY.ToMessage(), cancellationToken);
        }

        public Task SetLocatorFlagsAsync(LocatorFlag locatorFlag, CancellationToken cancellationToken)
        {
            var setLocatorFlags = new SetLocatorFlags(locatorFlag);
            return _driver.SendAsync(setLocatorFlags.ToMessage(), cancellationToken);
        }

        public async Task<BotToBotInfraredReadings> GetBotToBotInfraredReadingsAsync(
            CancellationToken cancellationToken)
        {
            var getBotToBotInfraredReadings = new GetBotToBotInfraredReadings();
            var response = await _driver.SendRequestAsync(getBotToBotInfraredReadings.ToMessage(), cancellationToken);
            return new BotToBotInfraredReadings(response);
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using shpero.Rvr.Commands.ConnectionDevice;
using shpero.Rvr.Responses.ConnectionDevice;

namespace shpero.Rvr
{
    public class ConnectionDevice
    {
        private readonly Driver _driver;

        public ConnectionDevice(Driver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }

        public async Task<BluetoothAdvertisingName> GetBluetoothAdvertisingNameAsync(CancellationToken cancellationToken)
        {
            var getBluetoothAdvertisingName = new GetBluetoothAdvertisingName();
            var response = await _driver.SendRequestAsync(getBluetoothAdvertisingName.ToMessage(), cancellationToken);
            return new BluetoothAdvertisingName(response);
        }
    }
}
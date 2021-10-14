using sphero.Rvr.Commands.ConnectionDevice;
using sphero.Rvr.Responses.ConnectionDevice;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace sphero.Rvr.Devices
{
    public class ConnectionDevice
    {
        private readonly IDriver _driver;

        public ConnectionDevice(IDriver driver)
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
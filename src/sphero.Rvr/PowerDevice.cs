using System;
using System.Threading;
using System.Threading.Tasks;
using sphero.Rvr.Commands.PowerDevice;
using sphero.Rvr.Responses.PowerDevice;

namespace sphero.Rvr
{
    public class PowerDevice
    {
        private readonly Driver _driver;

        public PowerDevice(Driver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }

        public Task SleepAsync(CancellationToken cancellationToken)
        {
            var sleep = new Sleep();
            return _driver.SendAsync(sleep.ToMessage(), cancellationToken);
        }

        public Task WakeAsync(CancellationToken cancellationToken)
        {
            var wake = new Wake();
            return _driver.SendAsync(wake.ToMessage(), cancellationToken);
        }

        public async Task<BatteryPercentage> GetBatterPercentageAsync(CancellationToken cancellationToken)
        {
            var getBatterPercentage = new GetBatterPercentage();
            var response = await _driver.SendRequestAsync(getBatterPercentage.ToMessage(), cancellationToken);
            return new BatteryPercentage(response);
        }

        public async Task<BatterVoltageState> GetBatteryVoltageStateAsync(CancellationToken cancellationToken)
        {
            var getBatteryVoltageState = new GetBatteryVoltageState();
            var response = await _driver.SendRequestAsync(getBatteryVoltageState.ToMessage(), cancellationToken);
            return new BatterVoltageState(response);
        }

        public async Task<BatterVoltage> GetBatteryVoltageAsync(BatteryVoltageReadings readings, CancellationToken cancellationToken)
        {
            var getBatteryVoltage = new GetBatteryVoltage(readings);
            var response = await _driver.SendRequestAsync(getBatteryVoltage.ToMessage(), cancellationToken);
            return new BatterVoltage(response);
        }

        public async Task<BatteryVoltageStateThresholds> GetBatteryVoltageStateThresholdsAsync(CancellationToken cancellationToken)
        {
            var getBatteryVoltageStateThresholds = new GetBatteryVoltageStateThresholds();
            var response = await _driver.SendRequestAsync(getBatteryVoltageStateThresholds.ToMessage(), cancellationToken);
            return new BatteryVoltageStateThresholds(response);
        }

        public async Task<CurrentSenseAmplifierCurrent> GetCurrentSenseAmplifierCurrentAsync(AmplifierId amplifierId, CancellationToken cancellationToken)
        {
            var getCurrentSenseAmplifierCurrent = new GetCurrentSenseAmplifierCurrent(amplifierId);
            var response = await _driver.SendRequestAsync(getCurrentSenseAmplifierCurrent.ToMessage(), cancellationToken);
            return new CurrentSenseAmplifierCurrent(response);
        }
    }
}
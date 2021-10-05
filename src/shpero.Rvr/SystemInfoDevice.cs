using System;
using System.Threading;
using System.Threading.Tasks;
using shpero.Rvr.Commands.SystemInfoDevice;
using shpero.Rvr.Responses.SystemInfoDevice;

namespace shpero.Rvr
{
    public class SystemInfoDevice
    {
        private readonly Driver _driver;

        public SystemInfoDevice(Driver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }

        private async Task<FirmwareVersion> GetFirmwareVersionAsync(byte processorId, CancellationToken cancellationToken)
        {
            var getFirmwareVersion = new GetFirmwareVersion(processorId);

            var response = await _driver.SendRequestAsync(getFirmwareVersion.ToMessage(), cancellationToken);

            var version = new FirmwareVersion(response);

            return version;
        }

        public Task<FirmwareVersion> GetFirmwareVersionForNordicProcessorAsync(CancellationToken cancellationToken)
        {
            return GetFirmwareVersionAsync(1, cancellationToken);
        }

        public Task<FirmwareVersion> GetFirmwareVersionForSTProcessorAsync(CancellationToken cancellationToken)
        {
            return GetFirmwareVersionAsync(2, cancellationToken);
        }

        private async Task<BootLoaderVersion> GetBootLoaderVersionAsync(byte processorId, CancellationToken cancellationToken)
        {
            var getMainApplicationVersion = new GetBootLoaderVersion(processorId);

            var response = await _driver.SendRequestAsync(getMainApplicationVersion.ToMessage(), cancellationToken);

            var version = new BootLoaderVersion(response);

            return version;
        }

        public Task<BootLoaderVersion> GetBootLoaderVersionForNordicProcessorAsync(CancellationToken cancellationToken)
        {
            return GetBootLoaderVersionAsync(1, cancellationToken);
        }

        public Task<BootLoaderVersion> GetBootLoaderVersionForSTProcessorAsync(CancellationToken cancellationToken)
        {
            return GetBootLoaderVersionAsync(2, cancellationToken);
        }


        public async Task<BoardRevision> GetBoardRevisionAsync( CancellationToken cancellationToken)
        {
            var getBoardRevision = new GetBoardRevision();

            var response = await _driver.SendRequestAsync(getBoardRevision.ToMessage(), cancellationToken);

            var version = new BoardRevision(response);

            return version;
        }

        public async Task<MacAddress> GetMacAddressAsync(CancellationToken cancellationToken)
        {
            var getMacAddress = new GetMacAddress();

            var response = await _driver.SendRequestAsync(getMacAddress.ToMessage(), cancellationToken);

            var address = new MacAddress(response);

            return address;
        }

        public async Task<StatsId> GetStatsIdAsync(CancellationToken cancellationToken)
        {
            var getStatsId = new GetStatsId();

            var response = await _driver.SendRequestAsync(getStatsId.ToMessage(), cancellationToken);

            var statsId = new StatsId(response);

            return statsId;
        }

        public async Task<ProcessorName> GetProcessorNameAsync(byte processorId, CancellationToken cancellationToken)
        {
            if (processorId != 0x01 && processorId != 0x02)
            {
                throw new ArgumentOutOfRangeException(nameof(processorId));
            }
            var getProcessorName = new GetProcessorName(processorId);

            var response = await _driver.SendRequestAsync(getProcessorName.ToMessage(), cancellationToken);

            var processorName = new ProcessorName(response);

            return processorName;
        }

        public async Task<Sku> GetSkuAsync(CancellationToken cancellationToken)
        {
            var getSku = new GetSku();

            var response = await _driver.SendRequestAsync(getSku.ToMessage(), cancellationToken);

            var skuProduced = new Sku(response);

            return skuProduced;
        }

        public async Task<CoreUpTimeInMilliseconds> GetCoreUpTimeInMillisecondsAsync(CancellationToken cancellationToken)
        {
            var getCoreUpTimeInMilliseconds = new GetCoreUpTimeInMilliseconds();

            var response = await _driver.SendRequestAsync(getCoreUpTimeInMilliseconds.ToMessage(), cancellationToken);

            var coreUpTimeInMillisecondsProduced = new CoreUpTimeInMilliseconds(response);

            return coreUpTimeInMillisecondsProduced;
        }
    }
}
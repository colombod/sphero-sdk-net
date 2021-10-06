using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using shpero.Rvr;

namespace sphero.Rvr.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var driver = new Driver("COM5");

            await TestDevice(new IoDevice(driver));

            await TestDevice(new SystemInfoDevice(driver));

        }

        private static async Task TestDevice(IoDevice io)
        {
            await io.SetAllLedsAsync(LedColor.Colors[LedColorNames.Blue], CancellationToken.None);

            await Task.Delay(1000);

            await io.SetAllLedsAsync(LedColor.Colors[LedColorNames.Pink], CancellationToken.None);

            await Task.Delay(1000);

            await io.SetAllLedsAsync(LedColor.Colors[LedColorNames.Green], CancellationToken.None);

            await Task.Delay(1000);

            await io.SetAllLedsAsync(LedColor.Colors[LedColorNames.White], CancellationToken.None);

            await Task.Delay(1000);
        }

        private static async Task TestDevice(SystemInfoDevice systemInfo)
        {
            var nordicProcessorVersion = await systemInfo.GetFirmwareVersionForNordicProcessorAsync(CancellationToken.None);

            System.Console.WriteLine(nordicProcessorVersion.Version);

            var stProcessorVersion = await systemInfo.GetFirmwareVersionForSTProcessorAsync(CancellationToken.None);

            System.Console.WriteLine(stProcessorVersion.Version);

            var boardRevision = await systemInfo.GetBoardRevisionAsync(CancellationToken.None);

            System.Console.WriteLine(boardRevision.Revision);

            var macAddress = await systemInfo.GetMacAddressAsync(CancellationToken.None);
            
            System.Console.WriteLine(macAddress.Address);

            var statsId = await systemInfo.GetStatsIdAsync(CancellationToken.None);

            System.Console.WriteLine(statsId.Id);

            var proc1Name = await systemInfo.GetProcessorNameAsync(1,CancellationToken.None);

            System.Console.WriteLine(proc1Name.Name);

            var proc2Name = await systemInfo.GetProcessorNameAsync(2,CancellationToken.None);

            System.Console.WriteLine(proc2Name.Name);

            var sku = await systemInfo.GetSkuAsync( CancellationToken.None);

            System.Console.WriteLine(sku.Value);

            var coreUpTimeInMilliseconds = await systemInfo.GetCoreUpTimeInMillisecondsAsync(CancellationToken.None);

            System.Console.WriteLine(coreUpTimeInMilliseconds.Milliseconds);
        }
    }
}

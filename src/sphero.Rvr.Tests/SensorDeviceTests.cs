using FluentAssertions;
using sphero.Rvr.Devices;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace sphero.Rvr.Tests
{
    public class SensorDeviceTests
    {
        [Fact]
        public async Task ConfigureSensorStreamingAsync_sends_correct_bytes()
        {
            var driver = new FakeDriver();
            var sensorDevice = new SensorDevice(driver);

            await sensorDevice.ConfigureSensorStreamingAsync(new[]
            {
                SensorId.Quaternion
            }, CancellationToken.None);
            driver.MessagesSent.Should().BeEquivalentTo(new[]
            {
                new [] { 0x8D, 0x38, 0x02, 0x01, 0x18, 0x39, 0x00, 0x01, 0x00, 0x00, 0x02, 0x70, 0xD8 },
            });

            driver.MessagesSent.Clear();

            await sensorDevice.ConfigureSensorStreamingAsync(
              new[]
              {
                    SensorId.AmbientLight,
                    SensorId.Accelerometer,
                    SensorId.Attitude,
                    SensorId.ColorDetection,
                    SensorId.CoreTimeLower,
                    SensorId.CoreTimeUpper,
                    SensorId.Gyroscope,
                    SensorId.Locator,
                    SensorId.Quaternion,
                    SensorId.Speed,
                    SensorId.Velocity
              }, CancellationToken.None);

            driver.MessagesSent.Should().BeEquivalentTo(new[]
            {
                new[] {0x8D, 0x38, 0x01, 0x01, 0x18, 0x38, 0x00, 0x01, 0x74, 0xD8}, // Enable color detection

                // processor 1, 3 slots
                new[] {0x8D, 0x38, 0x01, 0x01, 0x18, 0x39, 0x00, 0x01, 0x00, 0x03, 0x00, 0x70, 0xD8},
                new[] {0x8D, 0x38, 0x01, 0x01, 0x18, 0x39, 0x00, 0x02, 0x00, 0x05, 0x02, 0x00, 0x09, 0x02, 0x60, 0xD8},
                new[] {0x8D, 0x38, 0x01, 0x01, 0x18, 0x39, 0x00, 0x03, 0x00, 0x0A, 0x01, 0x66, 0xD8},

                // processor 2, 3 slots
                new[] {0x8D, 0x38, 0x02, 0x01, 0x18, 0x39, 0x00, 0x01, 0x00, 0x02, 0x01, 0x00, 0x01, 0x01, 0x00, 0x04, 0x01, 0x00, 0x00, 0x02, 0x66, 0xD8},
                new[] {0x8D, 0x38, 0x02, 0x01, 0x18, 0x39, 0x00, 0x02, 0x00, 0x06, 0x02, 0x00, 0x08, 0x02, 0x00, 0x07, 0x02, 0x56, 0xD8}
            });
        }
    }
}

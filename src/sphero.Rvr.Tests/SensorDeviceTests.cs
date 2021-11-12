using FluentAssertions;
using sphero.Rvr.Devices;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace sphero.Rvr.Tests;

public class SensorDeviceTests
{
    private readonly FakeDriver _driver;
    private readonly SensorDevice _sensorDevice;

    public SensorDeviceTests()
    {
        _driver = new FakeDriver();
        _sensorDevice = new SensorDevice(_driver);
    }

    [Fact]
    public async Task ConfigureSensorStreamingAsync_sends_correct_bytes()
    {
        await _sensorDevice.ConfigureSensorStreamingAsync(new[]
        {
            SensorId.Quaternion
        }, CancellationToken.None);
        _driver.MessagesSent.Should().BeEquivalentMessageSetTo(new[]
        {
            new byte [] { 0x8D, 0x38, 0x02, 0x01, 0x18, 0x39, 0x00, 0x01, 0x00, 0x00, 0x02, 0x70, 0xD8 },
        });

        _driver.MessagesSent.Clear();

        await _sensorDevice.ConfigureSensorStreamingAsync(
            new[]
            {
                SensorId.Quaternion,
                SensorId.Attitude,
                SensorId.Accelerometer,
                SensorId.ColorDetection,
                SensorId.Gyroscope,
                SensorId.Locator,
                SensorId.Velocity,
                SensorId.Speed,
                SensorId.CoreTimeLower,
                SensorId.CoreTimeUpper,
                SensorId.AmbientLight,
                SensorId.Encoders
            }, CancellationToken.None);

        _driver.MessagesSent.Should().BeEquivalentMessageSetTo(new[] {
            new byte[] { 0x8D, 0x38, 0x01, 0x01, 0x18, 0x38, 0x00, 0x01, 0x74, 0xD8 }, // Enable color detection

            // processor 1, 3 slots
            new byte[] { 0x8D, 0x38, 0x01, 0x01, 0x18, 0x39, 0x00, 0x01, 0x00, 0x03, 0x00, 0x70, 0xD8 },
            new byte[] { 0x8D, 0x38, 0x01, 0x01, 0x18, 0x39, 0x00, 0x02, 0x00, 0x05, 0x02, 0x00, 0x09, 0x02, 0x60, 0xD8 },
            new byte[] { 0x8D, 0x38, 0x01, 0x01, 0x18, 0x39, 0x00, 0x03, 0x00, 0x0A, 0x01, 0x66, 0xD8 },

            // processor 2, 3 slots
            new byte[] { 0x8D, 0x38, 0x02, 0x01, 0x18, 0x39, 0x00, 0x01, 0x00, 0x02, 0x01, 0x00, 0x01, 0x01, 0x00, 0x04, 0x01, 0x00, 0x00, 0x02, 0x66, 0xD8 },
            new byte[] { 0x8D, 0x38, 0x02, 0x01, 0x18, 0x39, 0x00, 0x02, 0x00, 0x06, 0x02, 0x00, 0x08, 0x02, 0x00, 0x07, 0x02, 0x56, 0xD8 }
        });
    }

    [Fact]
    public async Task EnableGyroMaxNotificationsAsync_sends_correct_bytes()
    {
        await _sensorDevice.EnableGyroMaxNotificationsAsync(true, CancellationToken.None);
        _driver.MessagesSent.Should().BeEquivalentMessageSetTo(new[]
        {
            new byte[] {0x8D, 0x38, 0x02, 0x01, 0x18, 0x0F, 0x00, 0x01, 0x9C, 0xD8}, // Enable color detection
        });


        _driver.MessagesSent.Clear();
        await _sensorDevice.EnableGyroMaxNotificationsAsync(false, CancellationToken.None);
        _driver.MessagesSent.Should().BeEquivalentMessageSetTo(new[]
        {
            new byte[] {0x8D, 0x38, 0x02, 0x01, 0x18, 0x0F, 0x00, 0x00, 0x9D, 0xD8}, // Enable color detection
        });
    }
}
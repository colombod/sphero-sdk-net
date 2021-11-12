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
            new byte[] { 0x8D, 0x38, 0x2, 0x1, 0x18, 0x39, 0x2, 0x1, 0x0, 0x0, 0x2, 0x0, 0x1, 0x1, 0x0, 0x2, 0x1, 0x0, 0x4, 0x1, 0x64, 0xD8 },
            new byte[] { 0x8D, 0x38, 0x2, 0x1, 0x18, 0x39, 0x3, 0x2, 0x0, 0x6, 0x2, 0x0, 0x7, 0x2, 0x0, 0x8, 0x2, 0x0, 0xB, 0x2, 0x46, 0xD8 },
            new byte[] { 0x8D, 0x38, 0x2, 0x1, 0x18, 0x39, 0x4, 0x3, 0x0, 0x5, 0x2, 0x0, 0x9, 0x2, 0x5A, 0xD8 },

            // processor 2, 3 slots
            new byte[] { 0x8D, 0x38, 0x1, 0x1, 0x18, 0x39, 0x5, 0x1, 0x0, 0x3, 0x0, 0x6B, 0xD8 },
            new byte[] { 0x8D, 0x38, 0x1, 0x1, 0x18, 0x39, 0x6, 0x3, 0x0, 0x5, 0x2, 0x0, 0x9, 0x2, 0x59, 0xD8 },
            new byte[] { 0x8D, 0x38, 0x1, 0x1, 0x18, 0x39, 0x7, 0x2, 0x0, 0xA, 0x1, 0x60, 0xD8 }
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
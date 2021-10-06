using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using shpero.Rvr.Commands.IoDevice;
using shpero.Rvr.Responses.IoDevice;

namespace shpero.Rvr
{
    public class IoDevice
    {
        private readonly Driver _driver;

        public static IReadOnlyDictionary<LedGroup, LedBitMask> GroupToLedMaskMap { get; } = new Dictionary<LedGroup, LedBitMask>
        {
            [LedGroup.StatusIndicationLeft] = LedBitMask.StatusIndicationLeft,
            [LedGroup.StatusIndicationRight] = LedBitMask.StatusIndicationRight,
            [LedGroup.HeadLightLeft] = LedBitMask.HeadLightLeft,
            [LedGroup.HeadLightRight] = LedBitMask.HeadLightRight,
            [LedGroup.BatteryDoorFront] = LedBitMask.BatteryDoorFront,
            [LedGroup.BatteryDoorRear] = LedBitMask.BatteryDoorRear,
            [LedGroup.PowerButtonFront] = LedBitMask.PowerButtonFront,
            [LedGroup.PowerButtonRear] = LedBitMask.PowerButtonRear,
            [LedGroup.BrakeLightLeft] = LedBitMask.BrakeLightLeft,
            [LedGroup.BrakeLightRight] = LedBitMask.BrakeLightRight,
            [LedGroup.UndercarriageWhite] = LedBitMask.UndercarriageWhite
        };

        public IoDevice(Driver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }

        public Task SetLedsAsync(LedBitMask leds, byte[] brightnessValues, CancellationToken cancellationToken)
        {
            var setAllLeds = new SetAllLeds(leds, brightnessValues);
            return _driver.SendAsync(setAllLeds.ToMessage(), cancellationToken);
        }

        public Task SetAllLedsAsync(Color color, CancellationToken cancellationToken)
        {
            var brightnessValues = new byte[3 * 10];
            for (int i = 0; i < 10; i++)
            {
                var position = (i * 3);
                brightnessValues[position] = color.Red;
                brightnessValues[position + 1] = color.Green;
                brightnessValues[position + 2] = color.Blue;
            }

            return SetLedsAsync(LedBitMask.All, brightnessValues, cancellationToken);
        }

        public Task SetAllLedsOffAsync( CancellationToken cancellationToken)
        {
            return SetAllLedsAsync(Color.Colors[ColorNames.Off], cancellationToken);
        }

        public async Task<ActiveColorPalette> GetActiveColorPaletteAsync(CancellationToken cancellationToken)
        {
            var getActiveColorPalette = new GetActiveColorPalette();
            var response = await _driver.SendRequestAsync(getActiveColorPalette.ToMessage(), cancellationToken);

            var activeColorPaletteProduced = new ActiveColorPalette(response);

            return activeColorPaletteProduced;
        }

        public Task SetActiveColorPaletteAsync(byte[] colorPaletteRaw, CancellationToken cancellationToken)
        {
            var setActiveColorPalette = new SetActiveColorPalette(colorPaletteRaw);
            return _driver.SendAsync(setActiveColorPalette.ToMessage(), cancellationToken);
        }

        public async Task<ColorIdentificationReport> GetActiveColorPaletteAsync(byte red, byte green, byte blue, byte confidenceThreshold,CancellationToken cancellationToken
            )
        {
            var getColorIdentificationReport = new GetColorIdentificationReport(red, green, blue, confidenceThreshold);
            var response = await _driver.SendRequestAsync(getColorIdentificationReport.ToMessage(), cancellationToken);

            var colorIdentificationReportProduced = new ColorIdentificationReport(response);

            return colorIdentificationReportProduced;
        }
    }
}
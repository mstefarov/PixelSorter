using System;
using System.Drawing;

namespace PixelSorter {
    internal static class ColorUtil {
        // XN/YN/ZN are illuminant D65 tristimulus values
        const double XN = 95.047,
            YN = 100.000,
            ZN = 108.883;

        // these constant are used in CIEXYZ -> CIELAB conversion
        public const double LinearThreshold = (6/29d)*(6/29d)*(6/29d),
            LinearMultiplier = (1/3d)*(29/6d)*(29/6d),
            LinearConstant = (4/29d);


        // Conversion from RGB to CIELAB, using illuminant D65.
        public static LabColor ToLab(this Color color) {
            // RGB are assumed to be in [0...255] range
            double R = color.R/255d;
            double G = color.G/255d;
            double B = color.B/255d;

            // CIEXYZ coordinates are normalized to [0...1]
            double x = 0.4124564*R + 0.3575761*G + 0.1804375*B;
            double y = 0.2126729*R + 0.7151522*G + 0.0721750*B;
            double z = 0.0193339*R + 0.1191920*G + 0.9503041*B;

            double xRatio = x/XN;
            double yRatio = y/YN;
            double zRatio = z/ZN;

            LabColor result = new LabColor {
                // L is normalized to [0...100]
                L = 116*XyzToLab(yRatio) - 16,
                a = 500*(XyzToLab(xRatio) - XyzToLab(yRatio)),
                b = 200*(XyzToLab(yRatio) - XyzToLab(zRatio))
            };
            return result;
        }


        static double XyzToLab(double ratio) {
            if (ratio > LinearThreshold) {
                return Math.Pow(ratio, 1/3d);
            } else {
                return LinearMultiplier*ratio + LinearConstant;
            }
        }


        public static double GetIntensity(this Color c) {
            return (c.R + c.G + c.B)/3d;
        }


        public static double GetMin(this Color c) {
            return Math.Min(c.R, Math.Min(c.G, c.B))/(double)byte.MaxValue;
        }


        public static double GetMax(this Color c) {
            return Math.Max(c.R, Math.Max(c.G, c.B))/(double)byte.MaxValue;
        }


        public static double GetChroma(this Color c) {
            return GetMax(c) - GetMin(c);
        }


        public static double GetLightness(this Color c) {
            return (GetMin(c) + GetMax(c))/2;
        }
    }
}

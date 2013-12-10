using System;
using System.Drawing;

namespace PixelSorter {
    internal struct Segment {
        public readonly double Value;
        public readonly int OffsetX;
        public readonly int OffsetY;


        public Segment(SortingTask task, int x, int y) {
            OffsetX = x;
            OffsetY = y;
            Value = 0;
            Value = GetValue(task);
        }


        double GetValue(SortingTask task) {
            if (task.Algorithm == SortAlgorithm.Segment) {
                return GetSingleValue(task, OffsetX, OffsetY);
            }
            switch (task.Sampling) {
                case SamplingMode.Center:
                    return GetSingleValue(task, OffsetX + task.SegmentWidth/2, OffsetY + task.SegmentHeight/2);

                case SamplingMode.Mean: {
                    double result = 0;
                    for (int x = 0; x < task.SegmentWidth; ++x) {
                        for (int y = 0; y < task.SegmentHeight; ++y) {
                            result += GetSingleValue(task, OffsetX + x, OffsetY + y);
                        }
                    }
                    return result/(task.SegmentWidth*task.SegmentHeight);
                }

                case SamplingMode.Median: {
                    var all = new double[task.SegmentWidth*task.SegmentHeight];
                    for (int x = 0; x < task.SegmentWidth; ++x) {
                        for (int y = 0; y < task.SegmentHeight; ++y) {
                            all[x*task.SegmentHeight + y] = GetSingleValue(task, OffsetX + x, OffsetY + y);
                        }
                    }
                    Array.Sort(all);
                    return all[all.Length/2];
                }

                case SamplingMode.Maximum: {
                    double result = double.MinValue;
                    for (int x = 0; x < task.SegmentWidth; ++x) {
                        for (int y = 0; y < task.SegmentHeight; ++y) {
                            result = Math.Max(result, GetSingleValue(task, OffsetX + x, OffsetY + y));
                        }
                    }
                    return result;
                }

                case SamplingMode.Minimum: {
                    double result = double.MaxValue;
                    for (int x = 0; x < task.SegmentWidth; ++x) {
                        for (int y = 0; y < task.SegmentHeight; ++y) {
                            result = Math.Min(result, GetSingleValue(task, OffsetX + x, OffsetY + y));
                        }
                    }
                    return result;
                }

                case SamplingMode.Random:
                    return GetSingleValue(task, OffsetX + task.rand.Next(task.SegmentWidth),
                                          OffsetY + task.rand.Next(task.SegmentHeight));

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        static double GetSingleValue(SortingTask task, int x, int y) {
            Color c = task.OriginalImage.GetPixel(x, y);
            switch (task.Metric) {
                case SortMetric.Intensity:
                    return c.GetIntensity();

                case SortMetric.Lightness:
                    return c.GetLightness();

                case SortMetric.Luma:
                    return c.R*0.2126 + c.G*0.7152 + c.B*0.0722;

                case SortMetric.Brightness:
                    return c.GetBrightness();

                case SortMetric.HslHue:
                    return c.GetHue()/360d;

                case SortMetric.LabHue: {
                    LabColor labC = c.ToLab();
                    if (labC.a == 0) {
                        return Math.PI/2;
                    } else {
                        double lh = Math.Atan2(labC.b, labC.a);
                        if (lh <= 0) {
                            lh = Math.PI*2 - Math.Abs(lh);
                        }
                        return lh;
                    }
                }

                case SortMetric.Chroma:
                    return c.GetChroma();

                case SortMetric.LabChroma: {
                    LabColor labColor = c.ToLab();
                    return Math.Sqrt(labColor.a*labColor.a + labColor.b*labColor.b);
                }


                case SortMetric.HsbSaturation: {
                    double chroma = c.GetChroma();
                    if (chroma == 0) {
                        return 0;
                    } else {
                        return chroma/c.GetMax();
                    }
                }

                case SortMetric.HsiSaturation: {
                    double chroma = c.GetChroma();
                    double intensity = c.GetIntensity();
                    if (chroma == 0 || intensity == 0) {
                        return 0;
                    } else {
                        double m = c.GetMin();
                        return 1 - m/intensity;
                    }
                }

                case SortMetric.HslSaturation: {
                    double max = c.GetMax();
                    double min = c.GetMin();
                    double chroma = c.GetChroma();
                    double L = c.GetLightness();
                    if (chroma == 0 || L == 0 || L == 1) {
                        return 0;
                    }
                    if (L < 0.5) {
                        return chroma/(max + min);
                    } else {
                        return chroma/(2 - max - min);
                    }
                }

                case SortMetric.LabSaturation: {
                    LabColor labColor = c.ToLab();
                    if (labColor.L < ColorUtil.LinearThreshold) {
                        return 0;
                    } else {
                        return Math.Sqrt(labColor.a*labColor.a + labColor.b*labColor.b)/labColor.L;
                    }
                }

                case SortMetric.RedChannel:
                    return c.R;

                case SortMetric.GreenChannel:
                    return c.G;

                case SortMetric.BlueChannel:
                    return c.B;

                case SortMetric.Red:
                    return c.R - Math.Max(c.G, c.B);

                case SortMetric.Green:
                    return c.G - Math.Max(c.R, c.B);

                case SortMetric.Blue:
                    return c.B - Math.Max(c.R, c.G);

                case SortMetric.Cyan:
                    return (c.G + c.B) - Math.Max(c.R*1.5, Math.Abs(c.G - c.B));

                case SortMetric.Magenta:
                    return (c.R + c.B) - Math.Max(c.G*1.5, Math.Abs(c.R - c.B));

                case SortMetric.Yellow:
                    return (c.R + c.G) - Math.Max(c.B*1.5, Math.Abs(c.R - c.G));

                case SortMetric.LabA:
                    return c.ToLab().a;

                case SortMetric.LabB:
                    return c.ToLab().b;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

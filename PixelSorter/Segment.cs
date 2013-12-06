using System;
using System.Drawing;

namespace PixelSorter {
    struct Segment {
        public readonly double Value;
        public readonly int OffsetX;
        public readonly int OffsetY;


        public Segment( SortingTask task, int x, int y ) {
            OffsetX = x;
            OffsetY = y;
            Value = 0;
            Value = GetValue( task );
        }


        double GetValue( SortingTask task ) {
            if( task.Algorithm == SortAlgorithm.Segment ) {
                return GetSingleValue( task, OffsetX, OffsetY );
            }
            switch( task.Sampling ) {
                case SamplingMode.Center:
                    return GetSingleValue( task, OffsetX + task.SegmentWidth/2, OffsetY + task.SegmentHeight/2 );

                case SamplingMode.Average: {
                    double result = 0;
                    for( int x = 0; x < task.SegmentWidth; ++x ) {
                        for( int y = 0; y < task.SegmentHeight; ++y ) {
                            result += GetSingleValue( task, OffsetX + x, OffsetY + y );
                        }
                    }
                    return result/(task.SegmentWidth*task.SegmentHeight);
                }

                case SamplingMode.Maximum: {
                    double result = double.MinValue;
                    for( int x = 0; x < task.SegmentWidth; ++x ) {
                        for( int y = 0; y < task.SegmentHeight; ++y ) {
                            result = Math.Max( result, GetSingleValue( task, OffsetX + x, OffsetY + y ) );
                        }
                    }
                    return result;
                }

                case SamplingMode.Minimum: {
                    double result = double.MaxValue;
                    for( int x = 0; x < task.SegmentWidth; ++x ) {
                        for( int y = 0; y < task.SegmentHeight; ++y ) {
                            result = Math.Min( result, GetSingleValue( task, OffsetX + x, OffsetY + y ) );
                        }
                    }
                    return result;
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        static double GetSingleValue( SortingTask task, int x, int y ) {
            Color c = task.Image.GetPixel( x, y );
            switch( task.Metric ) {
                case SortMetric.Intensity:
                    return c.R*c.G*c.B/3d;

                case SortMetric.Lightness: {
                    byte min = Math.Min( c.R, Math.Min( c.G, c.B ) );
                    byte max = Math.Max( c.R, Math.Max( c.G, c.B ) );
                    return (min + max)/2d;
                }

                case SortMetric.Luma:
                    return c.R*0.299 + c.G*0.587 + c.B*0.114;

                case SortMetric.Value:
                    return Math.Max( c.R, Math.Max( c.G, c.B ) );

                case SortMetric.Hue:
                    return c.GetHue();

                case SortMetric.Chroma: {
                    byte min = Math.Min( c.R, Math.Min( c.G, c.B ) );
                    byte max = Math.Max( c.R, Math.Max( c.G, c.B ) );
                    return (max - min);
                }

                case SortMetric.Saturation:
                    return c.GetSaturation();

                case SortMetric.RedChannel:
                    return c.R;

                case SortMetric.GreenChannel:
                    return c.G;

                case SortMetric.BlueChannel:
                    return c.B;

                case SortMetric.Red:
                    return c.R - Math.Max( c.G, c.B );

                case SortMetric.Green:
                    return c.G - Math.Max( c.R, c.B );

                case SortMetric.Blue:
                    return c.B - Math.Max( c.R, c.G );

                case SortMetric.Cyan:
                    return c.G + c.B - c.R;

                case SortMetric.Magenta:
                    return c.R + c.B - c.G;

                case SortMetric.Yellow:
                    return c.R + c.G - c.B;

                case SortMetric.LabA:
                    return RgbToLabConverter.RgbToLab( c ).a;

                case SortMetric.LabB:
                    return RgbToLabConverter.RgbToLab( c ).b;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

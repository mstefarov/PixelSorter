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
                    return GetIntensity( c );

                case SortMetric.Lightness:
                    return GetLightness( c );

                case SortMetric.Luma:
                    return c.R*0.2126 + c.G*0.7152 + c.B*0.0722;

                case SortMetric.Brightness:
                    return c.GetBrightness();

                case SortMetric.HslHue:
                    return c.GetHue()/360d;

                case SortMetric.LabHue: {
                    LabColor labC = RgbToLabConverter.RgbToLab( c );
                    if( labC.a == 0 ) {
                        return Math.PI/2;
                    } else {
                        double lh = Math.Atan2( labC.b,labC.a );
                        if( lh <= 0 ) {
                            lh = Math.PI*2 - Math.Abs( lh );
                        }
                        return lh;
                    }
                }

                case SortMetric.Chroma:
                    return GetChroma( c );

                case SortMetric.HsbSaturation: {
                    double chroma = GetChroma( c );
                    if( chroma == 0 ) {
                        return 0;
                    } else {
                        return chroma/GetMax( c );
                    }
                }
                    //return c.GetSaturation();

                case SortMetric.HsiSaturation: {
                    double C = GetChroma( c );
                    double I = GetIntensity( c );
                    if( C == 0 || I == 0 ) {
                        return 0;
                    } else {
                        double m = GetMin( c );
                        return 1 - m/I;
                    }
                }

                case SortMetric.HslSaturation: {
                    double max = GetMax( c );
                    double min = GetMin( c );
                    double chroma = GetChroma( c );
                    double L = GetLightness( c );
                    if( chroma == 0 || L == 0 || L == 1 ) {
                        return 0;
                    }
                    if( L < 0.5 ) {
                        return chroma/(max + min);
                    } else {
                        return chroma/(2 - max - min);
                    }
                }

                case SortMetric.LabSaturation:
                    LabColor color = RgbToLabConverter.RgbToLab( c );
                    if( color.L < RgbToLabConverter.LinearThreshold ) {
                        return 0;
                    } else {
                        return Math.Sqrt( color.a*color.a + color.b*color.b )/color.L;
                    }

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
                    return (c.G + c.B) - Math.Max( c.R*1.5, Math.Abs( c.G - c.B ) );

                case SortMetric.Magenta:
                    return (c.R + c.B) - Math.Max( c.G*1.5, Math.Abs( c.R - c.B ) );

                case SortMetric.Yellow:
                    return (c.R + c.G) - Math.Max( c.B*1.5, Math.Abs( c.R - c.G ) );

                case SortMetric.LabA:
                    return RgbToLabConverter.RgbToLab( c ).a;

                case SortMetric.LabB:
                    return RgbToLabConverter.RgbToLab( c ).b;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        static double GetIntensity( Color c ) {
            return (c.R + c.G + c.B)/3d;
        }

        static double GetMin( Color c ) {
            return Math.Min( c.R, Math.Min( c.G, c.B ) )/(double)byte.MaxValue;
        }

        static double GetMax( Color c ) {
            return Math.Max( c.R, Math.Max( c.G, c.B ) )/(double)byte.MaxValue;
        }

        static double GetChroma( Color c ) {
            return GetMax( c ) - GetMin( c );
        }

        static double GetLightness( Color c ) {
            return (GetMin( c ) + GetMax( c ))/2;
        }
    }
}

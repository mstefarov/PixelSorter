using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace PixelSorter {
    class SortingTask {
        const int CompositePixelSize = 3;

        public readonly SortAlgorithm Algorithm;
        public readonly SortOrder Order;
        public readonly SortMetric Metric;
        public readonly SamplingMode Sampling;
        public readonly int SegmentWidth, SegmentHeight;
        public readonly Bitmap OriginalImage;
        public readonly double Threshold;

        readonly Random rand = new Random();
        readonly int segmentRows, segmentColumns;
        readonly Segment[][] segments;
        double realThreshold;


        public SortingTask( SortAlgorithm algorithm, SortOrder order, SortMetric metric, SamplingMode sampling,
                            int segmentWidth, int segmentHeight, Bitmap originalImage, double threshold ) {
            Algorithm = algorithm;
            Order = order;
            Metric = metric;
            Sampling = sampling;
            SegmentWidth = segmentWidth;
            SegmentHeight = segmentHeight;
            OriginalImage = originalImage;
            Threshold = threshold;
            segmentRows = OriginalImage.Height/SegmentHeight;
            segmentColumns = OriginalImage.Width/SegmentWidth;

            switch( Algorithm ) {
                case SortAlgorithm.WholeImage:
                    segments = new Segment[1][];
                    segments[0] = new Segment[segmentRows*segmentColumns];
                    break;

                case SortAlgorithm.Column:
                    segments = new Segment[segmentColumns][];
                    for( int col = 0; col < segmentColumns; ++col ) {
                        segments[col] = new Segment[segmentRows];
                    }
                    break;

                case SortAlgorithm.Row:
                    segments = new Segment[segmentRows][];
                    for( int row = 0; row < segmentRows; ++row ) {
                        segments[row] = new Segment[segmentColumns];
                    }
                    break;

                case SortAlgorithm.Segment:
                    segments = new Segment[segmentRows*segmentColumns][];
                    for( int i = 0; i < segments.Length; ++i ) {
                        segments[i] = new Segment[segmentWidth*segmentHeight];
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        void FindDeltaRange() {
            double minDelta = double.MaxValue;
            double maxDelta = double.MinValue;
            if( Algorithm == SortAlgorithm.Column ) {
                for( int col = 0; col < segmentColumns; ++col ) {
                    for( int row = 1; row < segmentRows; ++row ) {
                        double delta = Math.Abs( segments[col][row].Value - segments[col][row - 1].Value );
                        minDelta = Math.Min( minDelta, delta );
                        maxDelta = Math.Max( maxDelta, delta );
                    }
                }
            } else if( Algorithm == SortAlgorithm.Row ) {
                for( int row = 0; row < segmentRows; ++row ) {
                    for( int col = 1; col < segmentColumns; ++col ) {
                        double delta = Math.Abs( segments[row][col].Value - segments[row][col - 1].Value );
                        minDelta = Math.Min( minDelta, delta );
                        maxDelta = Math.Max( maxDelta, delta );
                    }
                }
            } else {
                throw new InvalidOperationException();
            }
            realThreshold = minDelta + (maxDelta - minDelta)*(Threshold*Threshold);
        }


        public void Start() {
            switch( Algorithm ) {
                case SortAlgorithm.WholeImage:
                    for( int row = 0; row < segmentRows; ++row ) {
                        ReportProgress( (row + 1)*50/segmentRows );
                        for( int col = 0; col < segmentColumns; ++col ) {
                            if( cancel ) return;
                            Segment s = new Segment( this, col*SegmentWidth, row*SegmentHeight );
                            segments[0][row*segmentColumns + col] = s;
                        }
                    }
                    segments[0] = SortGroup( segments[0] );
                    break;

                case SortAlgorithm.Column:
                    for( int col = 0; col < segmentColumns; ++col ) {
                        ReportProgress( (col + 1)*50/segmentColumns );
                        for( int row = 0; row < segmentRows; ++row ) {
                            if( cancel ) return;
                            segments[col][row] = new Segment( this, col*SegmentWidth, row*SegmentHeight );
                        }
                    }
                    if( Threshold > 0 ) {
                        FindDeltaRange();
                    }
                    for( int col = 0; col < segmentColumns; ++col ) {
                        segments[col] = SortGroup( segments[col] );
                    }
                    break;

                case SortAlgorithm.Row:
                    for( int row = 0; row < segmentRows; ++row ) {
                        ReportProgress( (row + 1)*50/segmentRows );
                        for( int col = 0; col < segmentColumns; ++col ) {
                            if( cancel ) return;
                            segments[row][col] = new Segment( this, col*SegmentWidth, row*SegmentHeight );
                        }
                    }
                    if( Threshold > 0 ) {
                        FindDeltaRange();
                    }
                    for( int row = 0; row < segmentRows; ++row ) {
                        segments[row] = SortGroup( segments[row] );
                    }
                    break;

                case SortAlgorithm.Segment:
                    for( int row = 0; row < segmentRows; ++row ) {
                        ReportProgress( (row + 1)*50/segmentRows );
                        for( int col = 0; col < segmentColumns; ++col ) {
                            if( cancel ) return;
                            int idx = row*segmentColumns + col;
                            for( int y = 0; y < SegmentHeight; ++y ) {
                                for( int x = 0; x < SegmentWidth; ++x ) {
                                    Segment s = new Segment( this, col*SegmentWidth + x, row*SegmentHeight + y );
                                    segments[idx][y*SegmentWidth + x] = s;
                                }
                            }
                            segments[idx] = SortGroup( segments[idx] );
                        }
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        static Segment[] Center( Segment[] input ) {
            Segment[] temp = new Segment[input.Length];
            int midIndex = input.Length/2;
            for( int i = 0; i < input.Length; ++i ) {
                midIndex -= i*((i%2)*2 - 1);
                temp[midIndex] = input[i];
            }
            return temp;
        }


        Segment[] SortGroup( Segment[] group ) {
            switch( Order ) {
                case SortOrder.Ascending:
                    Array.Sort( group, IncreasingSegmentSorter.Instance );
                    return group;

                case SortOrder.AscendingReflected:
                    Array.Sort( group, IncreasingSegmentSorter.Instance );
                    return Center( group );

                case SortOrder.Descending:
                    Array.Sort( group, DecreasingSegmentSorter.Instance );
                    return group;

                case SortOrder.DescendingReflected:
                    Array.Sort( group, DecreasingSegmentSorter.Instance );
                    return Center( group );

                case SortOrder.AscendingThresholded:
                    SortThresholded( @group, IncreasingSegmentSorter.Instance );
                    return group;

                case SortOrder.DescendingThresholded:
                    SortThresholded( @group, DecreasingSegmentSorter.Instance );
                    return group;

                case SortOrder.Random:
                    for( int i = 0; i < group.Length - 1; ++i ) {
                        int swapIndex = rand.Next( i + 1, group.Length );
                        Segment temp = group[i];
                        group[i] = group[swapIndex];
                        group[swapIndex] = temp;
                    }
                    return group;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        void SortThresholded( Segment[] @group, IComparer<Segment> comparer ) {
            int j = 1;
            while( j < @group.Length ) {
                double delta = Math.Abs( @group[j].Value - @group[j - 1].Value );
                int runStart = j;
                while( delta <= realThreshold ) {
                    j++;
                    if( j == @group.Length ) {
                        break;
                    }
                    delta = Math.Abs( @group[j].Value - @group[j - 1].Value );
                }
                Array.Sort( @group, runStart, j - runStart, comparer );
                j++;
            }
        }


        public Bitmap MakeResult() {
            Bitmap result = new Bitmap( SegmentWidth*segmentColumns,
                                        SegmentHeight*segmentRows,
                                        PixelFormat.Format24bppRgb );
            bool onePixel = (SegmentWidth == 1 && SegmentHeight == 1 || Algorithm == SortAlgorithm.Segment);

            if( onePixel ) {
                BitmapData writeData = null;
                BitmapData readData = null;

                int sourcePixelStride = Image.GetPixelFormatSize( OriginalImage.PixelFormat )/8;
                try {
                    writeData = result.LockBits( new Rectangle( 0, 0, result.Width, result.Height ),
                                                 ImageLockMode.WriteOnly,
                                                 result.PixelFormat );
                    readData = OriginalImage.LockBits( new Rectangle( 0, 0, OriginalImage.Width, OriginalImage.Height ),
                                                       ImageLockMode.ReadOnly,
                                                       OriginalImage.PixelFormat );
                    switch( Algorithm ) {
                        case SortAlgorithm.WholeImage:
                            CompositeWholeImagePixel( readData, writeData, sourcePixelStride );
                            break;
                        case SortAlgorithm.Column:
                            CompositeColumnsPixel( readData, writeData, sourcePixelStride );
                            break;
                        case SortAlgorithm.Row:
                            CompositeRowsPixel( readData, writeData, sourcePixelStride );
                            break;
                        case SortAlgorithm.Segment:
                            CompositeSegmentsPixel( readData, writeData, sourcePixelStride );
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                } finally {
                    if( writeData != null ) {
                        result.UnlockBits( writeData );
                    }
                    if( readData != null ) {
                        OriginalImage.UnlockBits( readData );
                    }
                }

            } else {
                using( Graphics g = Graphics.FromImage( result ) ) {
                    // Since no interpolation takes place, use speediest available parameters
                    g.CompositingMode = CompositingMode.SourceCopy;
                    g.CompositingQuality = CompositingQuality.HighSpeed;
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;

                    switch( Algorithm ) {
                        case SortAlgorithm.WholeImage:
                            CompositeWholeImage( g );
                            break;
                        case SortAlgorithm.Column:
                            CompositeColumns( g );
                            break;
                        case SortAlgorithm.Row:
                            CompositeRows( g );
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            return result;
        }


        void CompositeWholeImage( Graphics g ) {
            Segment[] group = segments[0];
            for( int y = 0; y < segmentRows; ++y ) {
                for( int x = 0; x < segmentColumns; ++x ) {
                    if( cancel ) return;
                    Segment s = @group[y*segmentColumns + x];
                    Rectangle srcRect = new Rectangle( s.OffsetX, s.OffsetY, SegmentWidth, SegmentHeight );
                    Rectangle destRect = new Rectangle( x*SegmentWidth,
                                                        y*SegmentHeight,
                                                        SegmentWidth,
                                                        SegmentHeight );
                    g.DrawImage( OriginalImage, destRect, srcRect, GraphicsUnit.Pixel );
                }
            }
        }


        void CompositeColumns( Graphics g ) {
            for( int col = 0; col < segmentColumns; ++col ) {
                Segment[] group = segments[col];
                for( int row = 0; row < segmentRows; ++row ) {
                    if( cancel ) return;
                    Segment s = @group[row];
                    Rectangle srcRect = new Rectangle( s.OffsetX, s.OffsetY, SegmentWidth, SegmentHeight );
                    Rectangle destRect = new Rectangle( col*SegmentWidth,
                                                        row*SegmentHeight,
                                                        SegmentWidth,
                                                        SegmentHeight );
                    g.DrawImage( OriginalImage, destRect, srcRect, GraphicsUnit.Pixel );
                }
            }
        }


        void CompositeRows( Graphics g ) {
            for( int row = 0; row < segmentRows; ++row ) {
                ReportProgress( 50 + (row + 1)*50/segmentRows );
                Segment[] group = segments[row];
                for( int col = 0; col < segmentColumns; ++col ) {
                    if( cancel ) return;
                    Segment s = @group[col];
                    Rectangle srcRect = new Rectangle( s.OffsetX, s.OffsetY, SegmentWidth, SegmentHeight );
                    Rectangle destRect = new Rectangle( col*SegmentWidth,
                                                        row*SegmentHeight,
                                                        SegmentWidth,
                                                        SegmentHeight );
                    g.DrawImage( OriginalImage, destRect, srcRect, GraphicsUnit.Pixel );
                }
            }
        }


        unsafe void CompositeWholeImagePixel( BitmapData readData, BitmapData writeData, int sourcePixelStride ) {
            Segment[] group = segments[0];
            for( int y = 0; y < segmentRows; ++y ) {
                ReportProgress( 50 + (y + 1)*50/segmentRows );

                byte* writePtr = (byte*)writeData.Scan0 + (y*writeData.Stride);

                for( int x = 0; x < segmentColumns; ++x ) {
                    if( cancel ) return;
                    Segment s = @group[y*segmentColumns + x];
                    byte* readPtr = (byte*)readData.Scan0 + (s.OffsetY*readData.Stride) + s.OffsetX*sourcePixelStride;
                    writePtr[0] = readPtr[0];
                    writePtr[1] = readPtr[1];
                    writePtr[2] = readPtr[2];
                    writePtr += CompositePixelSize;
                }
            }
        }


        unsafe void CompositeColumnsPixel( BitmapData readData, BitmapData writeData, int sourcePixelStride ) {
            for( int col = 0; col < segmentColumns; ++col ) {
                ReportProgress( 50 + (col + 1)*50/segmentColumns );

                Segment[] group = segments[col];
                byte* writePtr = (byte*)writeData.Scan0 + col*CompositePixelSize;

                for( int row = 0; row < segmentRows; ++row ) {
                    if( cancel ) return;
                    Segment s = @group[row];
                    byte* readPtr = (byte*)readData.Scan0 + (s.OffsetY*readData.Stride) + s.OffsetX*sourcePixelStride;
                    writePtr[0] = readPtr[0];
                    writePtr[1] = readPtr[1];
                    writePtr[2] = readPtr[2];
                    writePtr += writeData.Stride;
                }
            }
        }



        unsafe void CompositeRowsPixel( BitmapData readData, BitmapData writeData, int sourcePixelStride ) {
            for( int row = 0; row < segmentRows; ++row ) {
                ReportProgress( 50 + (row + 1)*50/segmentRows );

                Segment[] group = segments[row];
                byte* writePtr = (byte*)writeData.Scan0 + (row*writeData.Stride);

                for( int col = 0; col < segmentColumns; ++col ) {
                    if( cancel ) return;
                    Segment s = @group[col];
                    byte* readPtr = (byte*)readData.Scan0 + (s.OffsetY*readData.Stride) + s.OffsetX*sourcePixelStride;
                    writePtr[0] = readPtr[0];
                    writePtr[1] = readPtr[1];
                    writePtr[2] = readPtr[2];
                    writePtr += CompositePixelSize;
                }
            }
        }


        unsafe void CompositeSegmentsPixel( BitmapData readData, BitmapData writeData, int sourcePixelStride ) {
            for( int row = 0; row < segmentRows; ++row ) {
                ReportProgress( 50 + (row + 1)*50/segmentRows );

                for( int col = 0; col < segmentColumns; ++col ) {
                    if( cancel ) return;
                    Segment[] group = segments[row*segmentColumns + col];

                    for( int y = 0; y < SegmentHeight; ++y ) {
                        byte* writePtr = (byte*)writeData.Scan0 + ((row*SegmentHeight + y)*writeData.Stride) +
                                         col*SegmentWidth*CompositePixelSize;
                        for( int x = 0; x < SegmentWidth; ++x ) {
                            Segment s = @group[y*SegmentWidth + x];
                            byte* readPtr = (byte*)readData.Scan0 + (s.OffsetY*readData.Stride) +
                                            s.OffsetX*sourcePixelStride;
                            writePtr[0] = readPtr[0];
                            writePtr[1] = readPtr[1];
                            writePtr[2] = readPtr[2];
                            writePtr += CompositePixelSize;
                        }
                    }
                }
            }
        }


        #region Progress Reporting and Cancelation

        static readonly TimeSpan ReportingFrequency = TimeSpan.FromSeconds( 0.05 );

        bool cancel;
        DateTime lastReportTime = DateTime.MinValue;

        public event ProgressChangedEventHandler ProgressChanged;


        void ReportProgress( int i ) {
            DateTime now = DateTime.UtcNow;
            if( now.Subtract( lastReportTime ) < ReportingFrequency ) {
                return;
            }
            lastReportTime = now;

            var h = ProgressChanged;
            if( h != null ) h( this, new ProgressChangedEventArgs( i, null ) );
        }

        public void CancelAsync() {
            cancel = true;
        }

        #endregion
    }


    class IncreasingSegmentSorter : IComparer<Segment> {
        public static readonly IncreasingSegmentSorter Instance = new IncreasingSegmentSorter();

        public int Compare( Segment x, Segment y ) {
            return Math.Sign( x.Value - y.Value );
        }
    }

    class DecreasingSegmentSorter : IComparer<Segment> {
        public static readonly DecreasingSegmentSorter Instance = new DecreasingSegmentSorter();

        public int Compare( Segment x, Segment y ) {
            return -Math.Sign( x.Value - y.Value );
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PixelSorter {
    class SortingTask {
        public readonly SortAlgorithm Algorithm;
        public readonly SortOrder Order;
        public readonly SortMetric Metric;
        public readonly SamplingMode Sampling;
        public readonly int SegmentWidth, SegmentHeight;
        public readonly Bitmap Image;

        readonly int segmentRows, segmentColumns;
        readonly Segment[][] segments;


        public SortingTask( SortAlgorithm algorithm, SortOrder order, SortMetric metric, SamplingMode sampling,
                            int segmentWidth, int segmentHeight, Bitmap image ) {
            Algorithm = algorithm;
            Order = order;
            Metric = metric;
            Sampling = sampling;
            SegmentWidth = segmentWidth;
            SegmentHeight = segmentHeight;
            Image = image;
            segmentRows = Image.Height/SegmentHeight;
            segmentColumns = Image.Width/SegmentWidth;

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


        public void Start() {
            switch( Algorithm ) {
                case SortAlgorithm.WholeImage:
                    for( int row = 0; row < segmentRows; ++row ) {
                        for( int col = 0; col < segmentColumns; ++col ) {
                            Segment s = new Segment( this, col*SegmentWidth, row*SegmentHeight );
                            segments[0][row*segmentColumns + col] = s;
                        }
                    }
                    segments[0] = SortGroup( segments[0] );
                    break;

                case SortAlgorithm.Column:
                    for( int col = 0; col < segmentColumns; ++col ) {
                        for( int row = 0; row < segmentRows; ++row ) {
                            segments[col][row] = new Segment( this, col*SegmentWidth, row*SegmentHeight );
                        }
                        segments[col] = SortGroup( segments[col] );
                    }
                    break;

                case SortAlgorithm.Row:
                    for( int row = 0; row < segmentRows; ++row ) {
                        for( int col = 0; col < segmentColumns; ++col ) {
                            segments[row][col] = new Segment( this, col*SegmentWidth, row*SegmentHeight );
                        }
                        segments[row] = SortGroup( segments[row] );
                    }
                    break;

                case SortAlgorithm.Segment:
                    for( int row = 0; row < segmentRows; ++row ) {
                        for( int col = 0; col < segmentColumns; ++col ) {
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

        readonly Random rand = new Random();


        public Bitmap MakeResult() {
            Bitmap result = new Bitmap( SegmentWidth*segmentColumns, SegmentHeight*segmentRows );
            bool onePixel = (SegmentWidth == 1 && SegmentHeight == 1 || Algorithm == SortAlgorithm.Segment);

            if( onePixel ) {
                switch( Algorithm ) {
                    case SortAlgorithm.WholeImage:
                        CompositeWholeImagePixel( result );
                        break;
                    case SortAlgorithm.Column:
                        CompositeColumnsPixel( result );
                        break;
                    case SortAlgorithm.Row:
                        CompositeRowsPixel( result );
                        break;
                    case SortAlgorithm.Segment:
                        CompositeSegmentsPixel( result );
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
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
                    Segment s = @group[y*segmentColumns + x];
                    Rectangle srcRect = new Rectangle( s.OffsetX, s.OffsetY, SegmentWidth, SegmentHeight );
                    Rectangle destRect = new Rectangle( x*SegmentWidth,
                                                        y*SegmentHeight,
                                                        SegmentWidth,
                                                        SegmentHeight );
                    g.DrawImage( Image, destRect, srcRect, GraphicsUnit.Pixel );
                }
            }
        }


        void CompositeColumns( Graphics g ) {
            for( int col = 0; col < segmentColumns; ++col ) {
                Segment[] group = segments[col];
                for( int row = 0; row < segmentRows; ++row ) {
                    Segment s = @group[row];
                    Rectangle srcRect = new Rectangle( s.OffsetX, s.OffsetY, SegmentWidth, SegmentHeight );
                    Rectangle destRect = new Rectangle( col*SegmentWidth,
                                                        row*SegmentHeight,
                                                        SegmentWidth,
                                                        SegmentHeight );
                    g.DrawImage( Image, destRect, srcRect, GraphicsUnit.Pixel );
                }
            }
        }


        void CompositeRows( Graphics g ) {
            for( int row = 0; row < segmentRows; ++row ) {
                Segment[] group = segments[row];
                for( int col = 0; col < segmentColumns; ++col ) {
                    Segment s = @group[col];
                    Rectangle srcRect = new Rectangle( s.OffsetX, s.OffsetY, SegmentWidth, SegmentHeight );
                    Rectangle destRect = new Rectangle( col*SegmentWidth,
                                                        row*SegmentHeight,
                                                        SegmentWidth,
                                                        SegmentHeight );
                    g.DrawImage( Image, destRect, srcRect, GraphicsUnit.Pixel );
                }
            }
        }


        void CompositeWholeImagePixel( Bitmap b ) {
            Segment[] group = segments[0];
            for( int y = 0; y < segmentRows; ++y ) {
                for( int x = 0; x < segmentColumns; ++x ) {
                    Segment s = @group[y*segmentColumns + x];
                    Color c = Image.GetPixel( s.OffsetX, s.OffsetY );
                    b.SetPixel( x, y, c );
                }
            }
        }


        void CompositeColumnsPixel( Bitmap b ) {
            for( int col = 0; col < segmentColumns; ++col ) {
                Segment[] group = segments[col];
                for( int row = 0; row < segmentRows; ++row ) {
                    Segment s = @group[row];
                    Color c = Image.GetPixel( s.OffsetX, s.OffsetY );
                    b.SetPixel( col, row, c );
                }
            }
        }


        void CompositeRowsPixel( Bitmap b ) {
            for( int row = 0; row < segmentRows; ++row ) {
                Segment[] group = segments[row];
                for( int col = 0; col < segmentColumns; ++col ) {
                    Segment s = @group[col];
                    Color c = Image.GetPixel( s.OffsetX, s.OffsetY );
                    b.SetPixel( col, row, c );
                }
            }
        }


        void CompositeSegmentsPixel( Bitmap b ) {
            for( int row = 0; row < segmentRows; ++row ) {
                for( int col = 0; col < segmentColumns; ++col ) {
                    Segment[] group = segments[row*segmentColumns + col];
                    for( int y = 0; y < SegmentHeight; ++y ) {
                        for( int x = 0; x < SegmentWidth; ++x ) {
                            Segment s = @group[y*SegmentWidth + x];
                            Color c = Image.GetPixel( s.OffsetX, s.OffsetY );
                            b.SetPixel( col*SegmentWidth + x, row*SegmentHeight + y, c );
                        }
                    }
                }
            }
        }
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

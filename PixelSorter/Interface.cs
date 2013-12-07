using System;
using System.Drawing;
using System.Windows.Forms;

namespace PixelSorter {
    public partial class Interface : Form {
        // This flag is used to stop auto-processing while [Randomize] button is being pressed.
        bool isRandomizing;
        Bitmap originalImage;
        readonly OpenFileDialog dOpenFile = new OpenFileDialog();
        readonly SaveFileDialog dSaveFile = new SaveFileDialog();


        public Interface( string[] args ) {
            InitializeComponent();
            SetProgressVisible( false );
            SetOptionsEnabled( false );
            cAlgorithm.SelectedIndex = (int)SortAlgorithm.WholeImage;
            cOrder.SelectedIndex = (int)SortOrder.Descending;
            cMetric.SelectedIndex = (int)SortMetric.Intensity;
            cSampling.SelectedIndex = (int)SamplingMode.Average;
            lImageSize.Text = "";

            dOpenFile.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff|All Files|*.*";
            dSaveFile.Filter =
                "PNG Image|*.png|BMP Image|*.bmp|JPEG Image|*.jpg;*.jpeg|TIFF Image|*.tif;*.tiff|All Files|*.*";

            if( args.Length == 1 ) {
                TryLoadFile( args[0] );
            } else {
                Shown += PixelSorter_Shown;
            }
        }


        void PixelSorter_Shown( object sender, EventArgs e ) {
            bOpenFile.PerformClick();
        }


        void SetProgressVisible( bool val ) {
            pbProgress.Visible = val;
            lProgress.Visible = val;
        }


        void SetOptionsEnabled( bool val ) {
            nSegmentWidth.Enabled = val;
            nSegmentHeight.Enabled = val;
            cAlgorithm.Enabled = val;
            cOrder.Enabled = val;
            cMetric.Enabled = val;
            cSampling.Enabled = val;
            bProcess.Enabled = val;
            bSave.Enabled = val;
            bRevert.Enabled = val;
            lImageSize.Visible = val;
            bRandomize.Enabled = val;
        }


        void SetImage( Bitmap img ) {
            if( originalImage != null && originalImage != img ) {
                originalImage.Dispose();
            }
            originalImage = img;
            SetOptionsEnabled( img != null );
            pictureBox.Image = img;
            if( img != null ) {
                lImageSize.Text = String.Format( "{0} x {1}", img.Width, img.Height );
                nSegmentHeight.Maximum = img.Height;
                nSegmentWidth.Maximum = img.Width;
            }
        }


        void bOpenFile_Click( object sender, EventArgs e ) {
            if( dOpenFile.ShowDialog() == DialogResult.OK ) {
                TryLoadFile( dOpenFile.FileName );
            }
        }


        void TryLoadFile( string fileName ) {
            try {
                SetImage( (Bitmap)Image.FromFile( fileName ) );
                tImageFile.Text = fileName;

                // scroll caret to the right
                tImageFile.SelectionStart = tImageFile.Text.ToCharArray().Length;
                tImageFile.SelectionLength = 0;
            } catch( Exception ex ) {
                SetImage( null );
                tImageFile.Text = "";
                MessageBox.Show( ex.ToString(),
                                 "Error loading image",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error );
            }
        }


        void bSave_Click( object sender, EventArgs e ) {
            if( dSaveFile.ShowDialog() == DialogResult.OK ) {
                try {
                    pictureBox.Image.Save( dSaveFile.FileName );
                } catch( Exception ex ) {
                    MessageBox.Show( ex.ToString(),
                                     "Error saving image",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Error );
                }
            }
        }


        void bRevert_Click( object sender, EventArgs e ) {
            SetImage( originalImage );
        }


        void bProcess_Click( object sender, EventArgs e ) {
            Enabled = false;
            SortAlgorithm algo = (SortAlgorithm)cAlgorithm.SelectedIndex;
            SortOrder order = (SortOrder)cOrder.SelectedIndex;
            SortMetric metric = (SortMetric)cMetric.SelectedIndex;
            SamplingMode sampling = (SamplingMode)cSampling.SelectedIndex;
            int segmentWidth = (int)nSegmentWidth.Value;
            int segmentHeight = (int)nSegmentHeight.Value;

            SetProgressVisible( true );
            pbProgress.Value = 0;
            lProgress.Text = "Analyzing";
            Refresh();

            SortingTask task = new SortingTask( algo,
                                                order,
                                                metric,
                                                sampling,
                                                segmentWidth,
                                                segmentHeight,
                                                originalImage );
            pbProgress.Value = 10;
            lProgress.Text = "Sorting";
            lProgress.Refresh();

            task.Start();
            pbProgress.Value = 50;
            lProgress.Text = "Rendering";
            lProgress.Refresh();

            Image oldImage = pictureBox.Image;
            pictureBox.Image = task.MakeResult();

            // Free resources used by the previous rendering
            if( oldImage != null && oldImage != originalImage ) {
                oldImage.Dispose();
            }

            // We're done! Unlock the interface.
            pbProgress.Value = 100;
            SetProgressVisible( false );
            Enabled = true;
        }


        void bRandomize_Click( object sender, EventArgs e ) {
            isRandomizing = true;
            Random rand = new Random();
            cAlgorithm.SelectedIndex = rand.Next( cAlgorithm.Items.Count );
            cOrder.SelectedIndex = rand.Next( cOrder.Items.Count );
            cMetric.SelectedIndex = rand.Next( cMetric.Items.Count );
            cSampling.SelectedIndex = rand.Next( cSampling.Items.Count );
            nSegmentHeight.Value = GetRandomSegmentSize(rand,originalImage.Height);
            nSegmentWidth.Value = GetRandomSegmentSize( rand, originalImage.Width );
            isRandomizing = false;
            bProcess.PerformClick();
        }


        int GetRandomSegmentSize( Random rand, int max ) {
            int medianSegmentSize = (int)Math.Sqrt( max );
            int segHeight = rand.Next( 1, medianSegmentSize );
            if( rand.NextDouble() > .5 ) {
                segHeight = max/segHeight;
            }
            return segHeight;
        }


        void cOrder_SelectedIndexChanged( object sender, EventArgs e ) {
            bool isRandom = ((SortOrder)cOrder.SelectedIndex == SortOrder.Random);
            cMetric.Enabled = !isRandom && cOrder.Enabled;
            cSampling.Enabled = !isRandom && cOrder.Enabled;
            if( !isRandomizing )
                bProcess.PerformClick();
        }


        void cAlgorithm_SelectedIndexChanged( object sender, EventArgs e ) {
            bool isSegment = ((SortAlgorithm)cAlgorithm.SelectedIndex == SortAlgorithm.Segment);
            cSampling.Enabled = !isSegment && cOrder.Enabled;
            if( !isRandomizing )
                bProcess.PerformClick();
        }


        void cMetric_SelectedIndexChanged( object sender, EventArgs e ) {
            if( !isRandomizing )
                bProcess.PerformClick();
        }


        void cSampling_SelectedIndexChanged( object sender, EventArgs e ) {
            if( !isRandomizing )
                bProcess.PerformClick();
        }
    }
}

using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;

namespace QRGenerator.Windows.Render
{
    public class CustomDrawing
    {
        private Brush m_DarkBrush;
        private Brush m_LightBrush;

        private ISizeCalculation m_ISize;
        public CustomDrawing()
        {
        }
        /// <summary>
        /// Initialize Renderer. Default brushes will be black and white.
        /// </summary>
        /// <param name="fixedModuleSize"></param>
        public CustomDrawing(ISizeCalculation iSize)
            : this(iSize, Brushes.Black, Brushes.White)
        {
        }

        /// <summary>
        /// Initialize Renderer.
        /// </summary>
        public CustomDrawing(ISizeCalculation iSize, Brush darkBrush, Brush lightBrush)
        {
            m_ISize = iSize;
            m_DarkBrush = darkBrush;
            m_LightBrush = lightBrush;
        }
        private ErrorCorrectionLevel m_ErrorCorrectLevel = ErrorCorrectionLevel.M;
        public void DrawWithLogo(MemoryStream bms)
        {
            QrEncoder encoder = new QrEncoder(m_ErrorCorrectLevel);
            QrCode qrCode;
            encoder.TryEncode("Test", out qrCode);

            DrawingBrushRenderer dRenderer = new DrawingBrushRenderer(
                new FixedModuleSize(10, QuietZoneModules.Two),
                Brushes.Black, Brushes.AliceBlue);
            //Same as how to use stream geometry. It will be contain inside Path UIElement. 

            MemoryStream ms = new MemoryStream();
            dRenderer.WriteToStream(qrCode.Matrix, ImageFormatEnum.PNG, ms, new Point(96, 96));
        }
    }
}

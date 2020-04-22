using QRGenerator.Positioning.Stencils;

namespace QRGenerator.Positioning
{
    internal static class PositioninngPatternBuilder
    {
        internal static void EmbedBasicPatterns(int version, TriStateMatrix matrix)
        {
            new PositionDetectionPattern(version).ApplyTo(matrix);
            new DarkDotAtLeftBottom(version).ApplyTo(matrix);
            new AlignmentPattern(version).ApplyTo(matrix);
            new TimingPattern(version).ApplyTo(matrix);
        }
    }
}
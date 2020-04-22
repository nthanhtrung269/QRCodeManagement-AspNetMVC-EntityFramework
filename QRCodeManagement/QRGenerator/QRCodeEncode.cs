using System.Collections.Generic;
using QRGenerator.DataEncodation;
using QRGenerator.EncodingRegion;
using QRGenerator.ErrorCorrection;
using QRGenerator.Masking;
using QRGenerator.Masking.Scoring;
using QRGenerator.Positioning;

namespace QRGenerator
{
	internal static class QRCodeEncode
	{
		internal static BitMatrix Encode(string content, ErrorCorrectionLevel errorLevel)
		{
			EncodationStruct encodeStruct = DataEncode.Encode(content, errorLevel);

            return ProcessEncodationResult(encodeStruct, errorLevel);
			
		}

        internal static BitMatrix Encode(IEnumerable<byte> content, ErrorCorrectionLevel errorLevel)
        {
            EncodationStruct encodeStruct = DataEncode.Encode(content, errorLevel);

            return ProcessEncodationResult(encodeStruct, errorLevel);
        }

        private static BitMatrix ProcessEncodationResult(EncodationStruct encodeStruct, ErrorCorrectionLevel errorLevel)
        {
            BitList codewords = ECGenerator.FillECCodewords(encodeStruct.DataCodewords, encodeStruct.VersionDetail);

            TriStateMatrix triMatrix = new TriStateMatrix(encodeStruct.VersionDetail.MatrixWidth);
            PositioninngPatternBuilder.EmbedBasicPatterns(encodeStruct.VersionDetail.Version, triMatrix);

            triMatrix.EmbedVersionInformation(encodeStruct.VersionDetail.Version);
            triMatrix.EmbedFormatInformation(errorLevel, new Pattern0());
            triMatrix.TryEmbedCodewords(codewords);

            return triMatrix.GetLowestPenaltyMatrix(errorLevel);
        }
		
	}
}

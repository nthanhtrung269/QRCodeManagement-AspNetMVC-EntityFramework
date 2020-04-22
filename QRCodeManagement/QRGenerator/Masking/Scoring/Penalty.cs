namespace QRGenerator.Masking.Scoring
{
	public abstract class Penalty
	{
		internal abstract int PenaltyCalculate(BitMatrix matrix);
	}
}

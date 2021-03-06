﻿using System;

namespace QRGenerator.Masking
{
    public abstract class Pattern : BitMatrix
    {
        public override int Width { get { throw new NotSupportedException(); } }
        public override int Height { get { throw new NotSupportedException(); } }
        public override bool[,] InternalArray
        {
            get { throw new NotImplementedException(); }
        }

        public abstract MaskPatternType MaskPatternType { get; }
    }
}

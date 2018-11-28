using System;
using System.Collections.Immutable;
using System.Numerics;

namespace Collatz.Core
{
	public sealed class CollatzSequence
	{
		public CollatzSequence(BigInteger start)
		{
			if(start <= BigInteger.One)
			{
				throw new ArgumentException("Must provide a starting value greater than one.", nameof(start));
			}

			this.Start = start;
			var builder = ImmutableArray.CreateBuilder<BigInteger>();
			builder.Add(start);

			while (start > 1)
			{
				start = start % 2 == 0 ?
					start / 2 : ((3 * start) + 1) / 2;
				builder.Add(start);
			}

			this.Sequence = builder.ToImmutable();
		}

		public ImmutableArray<BigInteger> Sequence { get; }
		public BigInteger Start { get; }
	}
}

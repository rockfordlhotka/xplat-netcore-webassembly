using NUnit.Framework;
using System;
using System.Numerics;

namespace Collatz.Core.Tests
{
	public static class CollatzSequenceTests
	{
		[Test]
		public static void GenerateSequence()
		{
			var start = new BigInteger(8);
			var sequence = new CollatzSequence(start);

			Assert.That(sequence.Start, Is.EqualTo(start), nameof(start));
			Assert.That(sequence.Sequence.Length, Is.EqualTo(4), nameof(sequence.Sequence.Length));
			Assert.That(sequence.Sequence[0], Is.EqualTo(start), "0");
			Assert.That(sequence.Sequence[1], Is.EqualTo(new BigInteger(4)), "1");
			Assert.That(sequence.Sequence[2], Is.EqualTo(new BigInteger(2)), "2");
			Assert.That(sequence.Sequence[3], Is.EqualTo(new BigInteger(1)), "3");
		}

		[Test]
		public static void GenerateSequenceWithIncorrectStartValue() => 
			Assert.That(() => new CollatzSequence(BigInteger.One), Throws.TypeOf<ArgumentException>());
	}
}
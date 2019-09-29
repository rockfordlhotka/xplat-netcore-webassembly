using Collatz.Api.Controllers;
using Collatz.Api.Logging;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Rocks;
using System;
using System.Collections.Immutable;
using System.Numerics;

namespace Collatz.Api.Tests.Controllers
{
	[TestFixture]
	public static class CollatzControllerTests
	{
		[Test]
		public static void CreateWithNull() =>
			Assert.That(() => new CollatzController(null!), Throws.TypeOf<ArgumentNullException>());

		[Test]
		public static void CalculateWithValue()
		{
			var value = "5";
			var logger = Rock.Create<ILogger>();
			logger.Handle(_ => _.Log(Arg.IsAny<string>()), 2);

			var controller = new CollatzController(logger.Make());
			var result = (OkObjectResult)controller.Get(value).Result;
			var resultValue = (ImmutableArray<string>)result.Value;

			Assert.That(result.StatusCode, Is.EqualTo(200));
			Assert.That(resultValue.Length, Is.EqualTo(5));
			Assert.That(resultValue[0], Is.EqualTo("5"));
			Assert.That(resultValue[1], Is.EqualTo("8"));
			Assert.That(resultValue[2], Is.EqualTo("4"));
			Assert.That(resultValue[3], Is.EqualTo("2"));
			Assert.That(resultValue[4], Is.EqualTo("1"));

			logger.Verify();
		}

		[Test]
		public static void CalculateWithInvalidValue()
		{
			var value = "wrong";
			var logger = Rock.Create<ILogger>();
			logger.Handle(_ => _.Log(Arg.IsAny<string>()));

			var controller = new CollatzController(logger.Make());
			var result = (BadRequestObjectResult)controller.Get(value).Result;

			Assert.That(result.StatusCode, Is.EqualTo(400));

			logger.Verify();
		}
	}
}
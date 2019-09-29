using Microsoft.AspNetCore.Mvc;
using Collatz.Api.Logging;
using System;
using System.Numerics;
using Collatz.Core;
using System.Linq;
using System.Collections.Immutable;

namespace Collatz.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public sealed class CollatzController 
		: ControllerBase
	{
		private readonly ILogger logger;

		public CollatzController(ILogger logger) => 
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

		[HttpGet("{value}")]
		public ActionResult<string> Get(string value)
		{
			this.logger.Log(
				$"{nameof(CollatzController)}.{nameof(CollatzController.Get)}, value is {value}");

			if(BigInteger.TryParse(value, out var number))
			{
				var sequence = new CollatzSequence(number);

				this.logger.Log(
					$"{nameof(CollatzController)}.{nameof(CollatzController.Get)}, sequence length for {value} is {sequence.Sequence.Length}");

				return this.Ok(sequence.Sequence.Select(_ => _.ToString()).ToImmutableArray());
			}
			else
			{
				return this.BadRequest($"The given value, {value}, is not a valid integer.");
			}
		}
	}
}

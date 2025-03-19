using Ensek.Application.Features.MeterReadings;
using Ensek.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ensek.WebApi.Controllers
{
	[ApiController]
	public class MeterReadingsController(
		ISender sender,
		IConfiguration configuration,
		ILogger<MeterReadingsController> logger) : ControllerBase
	{
		private readonly ISender _sender = sender;
		private readonly IConfiguration _configuration = configuration;
		private readonly ILogger<MeterReadingsController> _logger = logger;

		[HttpPost("/meter-reading-uploads")]
		public async Task<ActionResult<MeterReadingsUploadResponse>> UploadMeterReadings([FromForm] MeterReadingsUploadRequest request)
		{
			if (request?.File == null || request.File.Length == 0)
			{
				return BadRequest("No file uploaded");
			}

			var tempFile = await StoreTempFile(request.File);

			var processingResult = await _sender.Send(new SubmitMeterReadingsCommand(tempFile));

			DeleteTempFile(tempFile);

			return new MeterReadingsUploadResponse(processingResult.RecordsProcessed, processingResult.Errors);
		}

		private async Task<string> StoreTempFile(IFormFile formFile)
		{
			string tempFilesFolder = _configuration.GetValue<string>("TempFilesPath")!;

			var fileId = Guid.NewGuid().ToString();
			var filePath = Path.Combine(tempFilesFolder, fileId);

			using (var stream = System.IO.File.Create(filePath))
			{
				await formFile.CopyToAsync(stream);
			}

			return filePath;
		}

		private void DeleteTempFile(string filePath)
		{
			try
			{
				if (System.IO.File.Exists(filePath))
				{
					System.IO.File.Delete(filePath);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting temp file: {filePath}", filePath);
			}
		}
	}
}

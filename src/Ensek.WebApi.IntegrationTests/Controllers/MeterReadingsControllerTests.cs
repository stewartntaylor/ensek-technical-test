using Ensek.WebApi.Models;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace Ensek.WebApi.IntegrationTests.Controllers;

[Collection("Collection 1")]
public class MeterReadingsControllerTests : ApiFixture
{
	public static IEnumerable<object[]> InvalidFileData =>
	[
		[
			// Invalid AccountId
			1,
			1,
			"""
AccountId,MeterReadingDateTime,MeterReadValue,
0,22/04/2019 09:24,1002,
"""
		],
		[
			// Invalid MeterReadValue (>1M)
			1,
			1,
			"""
AccountId,MeterReadingDateTime,MeterReadValue,
2344,22/04/2019 09:24,1000001,
"""
		],
		[
			// Invalid MeterReadValue (<0)
			1,
			1,
			"""
AccountId,MeterReadingDateTime,MeterReadValue,
2344,22/04/2019 09:24,-1,
"""
		],
		[
			// Invalid MeterReadValue (>99999)
			1,
			1,
			"""
AccountId,MeterReadingDateTime,MeterReadValue,
2344,22/04/2019 09:24,100000,
"""
		],
		[
			// No matching account
			1,
			1,
			"""
AccountId,MeterReadingDateTime,MeterReadValue,
1000,22/04/2019 09:24,1234,
"""
		]
	];

	public MeterReadingsControllerTests(TestWebApplicationFactory<Program> factory) : base(factory)
	{
	}

	[Fact]
	public async Task AddProduct_Should_Add_Successfully_When_ValidRequest()
	{
		// Arrange
		var testData = """
AccountId,MeterReadingDateTime,MeterReadValue,
2344,22/04/2019 09:24,1002,
2233,22/04/2019 12:25,323,
""";

		var byteArrayContent = new ByteArrayContent(Encoding.UTF8.GetBytes(testData));
		using (var formContent = new MultipartFormDataContent())
		{
			formContent.Add(byteArrayContent, "file", "testfile.csv");

			// Act
			var response = await Client.PostAsync("meter-reading-uploads", formContent);

			// Assert
			response.StatusCode.ShouldBe(HttpStatusCode.OK);
		}
	}

	[Fact]
	public async Task AddProduct_Should_Return_BadRequest_When_NoFile()
	{
		// Act
		var response = await Client.PostAsync("meter-reading-uploads", new MultipartFormDataContent());
		
		// Assert
		response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
	}

	[Theory]
	[MemberData(nameof(InvalidFileData))]
	public async Task AddProduct_Should_Return_Failed_Record_Count_When_InvalidFileData(int totalRows, int rowErrors, string testData)
	{
		// Arrange
		var byteArrayContent = new ByteArrayContent(Encoding.UTF8.GetBytes(testData));
		using (var formContent = new MultipartFormDataContent())
		{
			formContent.Add(byteArrayContent, "file", "testfile.csv");

			// Act
			var response = await Client.PostAsync("meter-reading-uploads", formContent);

			// Assert
			var uploadResponse = await response.Content.ReadFromJsonAsync<MeterReadingsUploadResponse>();

			uploadResponse.ProcessedRecords.ShouldBe(totalRows - rowErrors);
			uploadResponse.Errors.ShouldBe(rowErrors);

			response.StatusCode.ShouldBe(HttpStatusCode.OK);
		}
	}
}

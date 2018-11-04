namespace GroupProjectBackend.Tests
{
    using FluentAssertions;
    using WebNeuralNets.Controllers;
    using NUnit.Framework;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Results;

    [TestFixture]
    public class ValuesControllerTests
    {
        [Test]
        public void Get_WhenNoParamsArePassed_ShouldAllValues()
        {
            // Arrange
            var controller = new ValuesController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.Get() as OkNegotiatedContentResult<string[]>;

            // Assert
            response.Should().NotBeNull();
            response.Content.Should().NotBeNull();
            response.Content.Should().Equal(new string[] { "value1", "value2" });
        }

        [Test]
        public void Get_WhenNoParamsArePassed_ShouldReturnAllValues()
        {
            // Arrange
            var controller = new ValuesController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.Get() as OkNegotiatedContentResult<string[]>;

            // Assert
            response.Should().NotBeNull();
            response.Content.Should().NotBeNull();
            response.Content.Should().BeEquivalentTo(new string[] { "value1", "value2" });
        }

        [Test]
        [TestCase(1, "1 value")]
        [TestCase(999999, "999999 value")]
        [TestCase(-999999, "-999999 value")]
        public void Get_WhenParamIsPassed_ShouldReturnValue(int id, string exptectedValue)
        {
            // Arrange
            var controller = new ValuesController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.Get(id) as OkNegotiatedContentResult<string>;

            // Assert
            response.Should().NotBeNull();
            response.Content.Should().NotBeNull();
            response.Content.Should().Be(exptectedValue);
        }

        [Test]
        [TestCase("value", "value")]
        public void Post_WhenParamIsPassed_ShouldReturnOkStatusResponse(string param, string expectedValue)
        {
            // Arrange
            var controller = new ValuesController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.Post(param) as CreatedNegotiatedContentResult<string>;

            // Assert
            response.Should().NotBeNull();
            response.Content.Should().NotBeNull();
            response.Content.Should().Be(expectedValue);
        }

        [Test]
        [TestCase(1, "value")]
        public void Put_WhenParamsArePassed_ShouldReturnOkStatusResponse(int id, string value)
        {
            // Arrange
            var controller = new ValuesController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.Put(id, value);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<OkResult>();
        }


        [Test]
        [TestCase(1)]
        [TestCase(999)]
        public void Delete_WhenParamIsPassed_ShouldReturnNoContentStatusResponse(int id)
        {
            // Arrange
            var controller = new ValuesController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            var response = controller.Delete(id) as StatusCodeResult;

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}

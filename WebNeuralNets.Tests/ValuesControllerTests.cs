using FluentAssertions;
using WebNeuralNets.Controllers;
using NUnit.Framework;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace GroupProjectBackend.Tests
{
    [TestFixture]
    public class ValuesControllerTests
    {
        [Test]
        public void Get_WhenNoParamsArePassed_ShouldAllValues()
        {
            // Arrange
            var controller = new ValuesController();

            // Act
            var response = controller.Get() as ObjectResult;

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<OkObjectResult>();
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            response.Value.Should().BeOfType<string[]>();
            response.Value.Should().BeEquivalentTo(new string[] { "value1", "value2" });
        }

        [Test]
        [TestCase(1, "1 value")]
        [TestCase(999999, "999999 value")]
        [TestCase(-999999, "-999999 value")]
        public void Get_WhenParamIsPassed_ShouldReturnValue(int id, string exptectedValue)
        {
            // Arrange
            var controller = new ValuesController();

            // Act
            var response = controller.Get(id) as ObjectResult;

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<OkObjectResult>();
            response.Value.Should().NotBeNull();
            response.Value.Should().BeOfType<string>();
            response.Value.Should().Be(exptectedValue);
        }

        [Test]
        [TestCase("value", "value")]
        public void Post_WhenParamIsPassed_ShouldReturnOkStatusResponse(string param, string expectedValue)
        {
            // Arrange
            var controller = new ValuesController();

            // Act
            var response = controller.Post(param) as ObjectResult;

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<CreatedResult>();
            response.Value.Should().NotBeNull();
            response.Value.Should().BeOfType<string>();
            response.Value.Should().Be(expectedValue);
        }

        [Test]
        [TestCase(1, "value")]
        public void Put_WhenParamsArePassed_ShouldReturnOkStatusResponse(int id, string value)
        {
            // Arrange
            var controller = new ValuesController();

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
            var controller = new ValuesController();

            // Act
            var response = controller.Delete(id);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<NoContentResult>();
        }
    }
}

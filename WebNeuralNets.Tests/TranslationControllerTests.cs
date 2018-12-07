using FluentAssertions;
using WebNeuralNets.Controllers;
using NUnit.Framework;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using WebNeuralNets.BusinessLogic;
using Moq;
using WebNeuralNets.Models.DB;
using WebNeuralNets.Models.Enums;

namespace GroupProjectBackend.Tests
{
    [TestFixture]
    public class TranslationControllerTests
    {
        private readonly ITranslationHelper _translationHelper;
        private readonly WebNeuralNetDbContext _dbContext;

        public TranslationControllerTests()
        {
            var translationHelper = new Mock<ITranslationHelper>();
            translationHelper.Setup(x => x.GetTranslation(It.IsAny<LanguageCode>(), It.IsAny<string>())).Returns("NOTFOUND");
            translationHelper.Setup(x => x.GetTranslation(LanguageCode.ENG, "ENGtest")).Returns("English testing");
            translationHelper.Setup(x => x.GetTranslation(LanguageCode.PL, "PLtest")).Returns("Polskie testowanie");
            _translationHelper = translationHelper.Object;
            _dbContext = new Mock<WebNeuralNetDbContext>().Object;
        }

        [Test]
        [TestCase(LanguageCode.ENG, "ENGtest", "English testing")]
        [TestCase(LanguageCode.PL, "ENGtest", "NOTFOUND")]
        [TestCase(LanguageCode.PL, "PLtest", "Polskie testowanie")]
        [TestCase(LanguageCode.ENG, "PLtest", "NOTFOUND")]
        [TestCase(LanguageCode.PL, "XXX", "NOTFOUND")]
        [TestCase(LanguageCode.ENG, "XXX", "NOTFOUND")]
        public void GetTranslation_WhenLanguageCodeAndKeyIsPassed_ShouldReturnExpectedValue(LanguageCode languageCode, string key, string expectedResult)
        {
            // Arrange
            var controller = new TranslationController(_translationHelper, _dbContext);

            // Act
            var response = controller.GetTranslation(key, languageCode) as ObjectResult;

            // Assert
            response.Should().NotBeNull();
            response.Should().BeOfType<OkObjectResult>();
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            response.Value.Should().Be(expectedResult);
        }
    }
}

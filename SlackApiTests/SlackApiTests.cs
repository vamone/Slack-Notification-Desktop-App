using System;

using Xunit;

namespace Slack.Api.Tests
{
    public class SlackApiTests
    {
        [Fact]
        public void InitializeComponentsWithNullTokenShouldThrowException()
        {
            //Arrange
            var slackApi = new SlackApi();

            //Act
            Action action = () => slackApi.InitializeComponents(null);

            //Assert
            Assert.Throws<SlackApiException>(action);
        }

        [Fact]
        public void InitializeComponentsWithEmptyTokenShouldThrowException()
        {
            //Arrange
            var slackApi = new SlackApi();

            //Act
            Action action = () => slackApi.InitializeComponents(string.Empty);

            //Assert
            Assert.Throws<SlackApiException>(action);
        }
    }
}
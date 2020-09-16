namespace CRM.Letstalk.Client.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Letstalk.Client.Configurations;
    using CRM.Letstalk.Client.Services;
    using CRM.Letstalk.Client.Services.Interfaces;
    using NSubstitute;
    using RestSharp;
    using Xunit;

    public class RestClientServiceTests
    {
        private readonly IRestClient restClient;
        private readonly IJwtService jwtHelper;
        private readonly BotConfig botConfig;
        private readonly RestClientService restClientService;
        private readonly Fixture fixture;

        public RestClientServiceTests()
        {
            restClient = Substitute.For<IRestClient>();
            jwtHelper = Substitute.For<IJwtService>();
            fixture = new Fixture();
            botConfig = new BotConfig
            {
                ApiKey = "TestAPI"
            };
            restClientService = new RestClientService(restClient, jwtHelper, botConfig);
        }

        [Fact]
        public void ExecuteGet_InvalidEndPoint_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => restClientService.ExecuteGet<object>(string.Empty));
        }

        [Fact]
        public void ExecuteGet_ValidEndPoint_CallsRestClientExecute()
        {
            // Arrange
            var endPoint = "http://test.com";

            var response = Substitute.For<IRestResponse<object>>();
            response.StatusCode.Returns(HttpStatusCode.OK);
            restClient.Execute<object>(Arg.Any<IRestRequest>()).Returns(response);

            // Act
            restClientService.ExecuteGet<object>(endPoint);

            // Assert
            restClient.Received().Execute<object>(
                Arg.Is<IRestRequest>(
                    r => r.Resource.Equals(endPoint)
                        && r.Method == Method.GET));
        }

        [Fact]
        public void ExecuteGet_FailedResponse_ThrowsInvalidOperationException()
        {
            // Arrange
            var endPoint = "endpoint";

            var response = Substitute.For<IRestResponse<object>>();
            response.IsSuccessful.Returns(false);
            restClient.Execute<object>(Arg.Any<IRestRequest>()).Returns(response);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => restClientService.ExecuteGet<object>(endPoint));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ExecutePost_InvalidEndPoint_ThrowsArgumentNullException(string endPoint)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => restClientService.ExecutePost<object>(endPoint, new Dictionary<string, object>()));
        }

        [Fact]
        public void ExecutePost_ValidEndPoint_CallsRestClientExecute()
        {
            // Arrange
            var endPoint = "endpoint";
            var data = new Dictionary<string, object>();

            var response = Substitute.For<IRestResponse<object>>();
            response.StatusCode.Returns(HttpStatusCode.OK);

            restClient.Execute<object>(Arg.Any<IRestRequest>()).Returns(response);

            // Act
            restClientService.ExecutePost<object>(endPoint, data);

            // Assert
            restClient.Received().Execute<object>(
                Arg.Is<IRestRequest>(
                    r => r.Resource.Equals(endPoint)
                        && r.Method == Method.POST));
        }

        [Fact]
        public void ExecutePost_FailedResponse_ThrowsInvalidOperationException()
        {
            // Arrange
            var endPoint = "endpoint";

            var response = Substitute.For<IRestResponse<object>>();
            response.IsSuccessful.Returns(false);
            restClient.Execute<object>(Arg.Any<IRestRequest>()).Returns(response);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(
                () => restClientService.ExecutePost<object>(endPoint, new Dictionary<string, object>()));
        }

        [Fact]
        public void ExecutePut_InvalidEndPoint_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => restClientService.ExecutePut<object>(string.Empty, new Dictionary<string, object>()));
        }

        [Fact]
        public void ExecutePut_ValidEndPoint_CallsRestClientExecute()
        {
            // Arrange
            var endPoint = "endpoint";
            var data = new Dictionary<string, object>();

            var response = Substitute.For<IRestResponse<object>>();
            response.StatusCode.Returns(HttpStatusCode.OK);

            restClient.Execute<object>(Arg.Any<IRestRequest>()).Returns(response);

            // Act
            restClientService.ExecutePut<object>(endPoint, data);

            // Assert
            restClient.Received().Execute<object>(
                Arg.Is<IRestRequest>(
                    r => r.Resource.Equals(endPoint)
                        && r.Method == Method.PUT));
        }

        [Fact]
        public void ExecutePut_FailedResponse_ThrowsInvalidOperationException()
        {
            // Arrange
            var endPoint = "endpoint";

            var response = Substitute.For<IRestResponse<object>>();
            response.IsSuccessful.Returns(false);
            restClient.Execute<object>(Arg.Any<IRestRequest>()).Returns(response);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(
                () => restClientService.ExecutePut<object>(endPoint, new Dictionary<string, object>()));
        }

        [Fact]
        public void ExecuteDelete_InvalidEndPoint_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(
                () => restClientService.ExecuteDelete<object>(string.Empty, new Dictionary<string, object>()));
        }

        [Fact]
        public void ExecuteDelete_ValidEndPoint_CallsRestClientExecute()
        {
            // Arrange
            var endPoint = "endpoint";

            var response = Substitute.For<IRestResponse<object>>();
            response.StatusCode.Returns(HttpStatusCode.OK);

            restClient.Execute<object>(Arg.Any<IRestRequest>()).Returns(response);

            // Act
            restClientService.ExecuteDelete<object>(endPoint, new Dictionary<string, object>());

            // Assert
            restClient.Received().Execute<object>(
                Arg.Is<IRestRequest>(
                    r => r.Resource.Equals(endPoint)
                        && r.Method == Method.DELETE));
        }

        [Fact]
        public void ExecuteDelete_FailedResponse_ThrowsInvalidOperationException()
        {
            // Arrange
            var endPoint = "endpoint";

            var response = Substitute.For<IRestResponse<object>>();
            response.IsSuccessful.Returns(false);
            restClient.Execute<object>(Arg.Any<IRestRequest>()).Returns(response);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(
                () => restClientService.ExecuteDelete<object>(endPoint, new Dictionary<string, object>()));
        }

        [Fact]
        public void ExecuteGetAsync_InvalidEndPoint_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => restClientService.ExecuteGetAsync<object>(string.Empty));
        }

        [Fact]
        public void ExecuteGetAsync_ValidEndPoint_CallsRestClientExecute()
        {
            // Arrange
            var endPoint = "endpoint";

            var response = Substitute.For<IRestResponse<object>>();
            response.StatusCode.Returns(HttpStatusCode.OK);

            restClient.ExecuteTaskAsync<object>(Arg.Any<IRestRequest>())
                .Returns(Task.FromResult(response));

            // Act
            restClientService.ExecuteGetAsync<object>(endPoint);

            // Assert
            restClient.Received().ExecuteTaskAsync<object>(
                Arg.Is<IRestRequest>(
                    r => r.Resource.Equals(endPoint)
                        && r.Method == Method.GET));
        }

        [Fact]
        public void ExecutePostAsync_InvalidEndPoint_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(
                () => restClientService.ExecutePostAsync<object>(string.Empty, new Dictionary<string, object>()));
        }

        [Fact]
        public void ExecutePostAsync_ValidEndPoint_CallsRestClientExecute()
        {
            // Arrange
            var endPoint = "endpoint";

            var response = Substitute.For<IRestResponse<object>>();
            response.StatusCode.Returns(HttpStatusCode.OK);

            restClient.ExecuteTaskAsync<object>(Arg.Any<IRestRequest>())
               .Returns(Task.FromResult(response));

            // Act
            restClientService.ExecutePostAsync<object>(endPoint, new Dictionary<string, object>());

            // Assert
            restClient.Received().ExecuteTaskAsync<object>(
                Arg.Is<IRestRequest>(
                    r => r.Resource.Equals(endPoint)
                        && r.Method == Method.POST));
        }

        [Fact]
        public void ExecutePutAsync_InvalidEndPoint_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(
                () => restClientService.ExecutePutAsync<object>(string.Empty, new Dictionary<string, object>()));
        }

        [Fact]
        public void ExecutePutAsync_ValidEndPoint_CallsRestClientExecute()
        {
            // Arrange
            var endPoint = "endpoint";

            var response = Substitute.For<IRestResponse<object>>();
            response.StatusCode.Returns(HttpStatusCode.OK);
            restClient.ExecuteTaskAsync<object>(Arg.Any<IRestRequest>())
                .Returns(Task.FromResult(response));

            // Act
            restClientService.ExecutePutAsync<object>(endPoint, new Dictionary<string, object>());

            // Assert
            restClient.Received().ExecuteTaskAsync<object>(
                Arg.Is<IRestRequest>(
                    r => r.Resource.Equals(endPoint)
                        && r.Method == Method.PUT));
        }

        [Fact]
        public void ExecuteDeleteAsync_InvalidEndPoint_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(
                () => restClientService.ExecuteDeleteAsync<object>(string.Empty, new Dictionary<string, object>()));
        }

        [Fact]
        public void ExecuteDeleteAsync_ValidEndPoint_CallsRestClientExecute()
        {
            // Arrange
            var endPoint = "endpoint";

            var response = Substitute.For<IRestResponse<object>>();
            response.StatusCode.Returns(HttpStatusCode.OK);
            restClient.ExecuteTaskAsync<object>(Arg.Any<IRestRequest>())
                .Returns(Task.FromResult(response));

            // Act
            restClientService.ExecuteDeleteAsync<object>(endPoint, new Dictionary<string, object>());

            // Assert
            restClient.Received().ExecuteTaskAsync<object>(
                Arg.Is<IRestRequest>(
                    r => r.Resource.Equals(endPoint)
                        && r.Method == Method.DELETE));
        }
    }
}
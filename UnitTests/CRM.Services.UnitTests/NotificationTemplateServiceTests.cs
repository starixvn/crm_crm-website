namespace CRM.Services.UnitTests
{
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.NotificationTemplate;
    using CRM.Models.NotificationTemplate;
    using Fanex.NAP.Client.Helpers.Web;
    using NSubstitute;
    using Xunit;

    public class NotificationTemplateServiceTests
    {
        private INapSession napSession;
        private IBaseService baseService;

        [Fact]
        public async Task CreateAsync_WhenCalled_ReturnExpectedCommandResult()
        {
            // Arrange
            var fixture = new Fixture();
            var model = fixture.Create<CreateNotificationTemplate>();
            var expected = fixture.Create<CommandResult>();
            var criteria = new CreateNotificationTemplateCommand();
            var service = GetService();
            baseService.ExecuteNonQueryAsync(Arg.Do<CreateNotificationTemplateCommand>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.CreateAsync(model);

            // Assert
            Assert.Equal(expected, actual);

            Assert.Equal(model.NotificationTypeId, criteria.NotificationTypeId);
            Assert.Equal(model.Name, criteria.Name);
            Assert.Equal(model.Title, criteria.Title);
            Assert.Equal(model.ContentEN, criteria.ContentEN);
            Assert.Equal(model.ContentSC, criteria.ContentSC);
            Assert.Equal(model.ContentENSC, criteria.ContentENSC);
            Assert.Equal(model.IsCombineContentENSC, criteria.IsCombineContentENSC);
            Assert.Equal(napSession.UserId, criteria.CreatorId);
        }

        #region Helpers

        private NotificationTemplateService GetService()
        {
            napSession = Substitute.For<INapSession>();
            baseService = Substitute.For<IBaseService>();
            var service = new NotificationTemplateService(napSession, baseService);

            return service;
        }

        #endregion Helpers
    }
}
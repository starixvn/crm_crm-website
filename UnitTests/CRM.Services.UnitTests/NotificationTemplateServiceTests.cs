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

        [Fact]
        public async Task EditAsync_WhenCalled_ReturnExpectedCommandResult()
        {
            // Arrange
            var fixture = new Fixture();
            var model = fixture.Create<EditNotificationTemplate>();
            var expected = fixture.Create<CommandResult>();
            var criteria = new EditNotificationTemplateCommand();
            var service = GetService();
            baseService.ExecuteNonQueryAsync(Arg.Do<EditNotificationTemplateCommand>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.EditAsync(model);

            // Assert
            Assert.Equal(expected, actual);

            Assert.Equal(model.NotificationTemplateId, criteria.NotificationTemplateId);
            Assert.Equal(model.NotificationTypeId, criteria.NotificationTypeId);
            Assert.Equal(model.Name, criteria.Name);
            Assert.Equal(model.Title, criteria.Title);
            Assert.Equal(model.ContentEN, criteria.ContentEN);
            Assert.Equal(model.ContentSC, criteria.ContentSC);
            Assert.Equal(model.ContentENSC, criteria.ContentENSC);
            Assert.Equal(model.IsCombineContentENSC, criteria.IsCombineContentENSC);
            Assert.Equal(napSession.UserId, criteria.ModifierId);
        }

        [Fact]
        public async Task DeleteAsync_WhenCalled_ReturnExpectedCommandResult()
        {
            // Arrange
            var fixture = new Fixture();
            var id = fixture.Create<int>();
            var expected = fixture.Create<CommandResult>();
            var criteria = new DeleteNotificationTemplateCommand();
            var service = GetService();
            baseService.ExecuteNonQueryAsync(Arg.Do<DeleteNotificationTemplateCommand>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.DeleteAsync(id);

            // Assert
            Assert.Equal(expected, actual);

            Assert.Equal(id, criteria.NotificationTemplateId);
            Assert.Equal(napSession.UserId, criteria.UserId);
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
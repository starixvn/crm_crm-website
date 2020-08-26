namespace CRM.Services.UnitTests
{
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Models;
    using CRM.Core.Services;
    using CRM.Criteria.NotificationType;
    using CRM.Models.NotificationType;
    using Fanex.NAP.Client.Helpers.Web;
    using NSubstitute;
    using Xunit;

    public class NotificationTypeServiceTests
    {
        private INapSession napSession;
        private IBaseService baseService;
        private readonly Fixture fixture = new Fixture();

        [Fact]
        public async Task CreateAsync_WhenCalled_ReturnExpectedCommandResult()
        {
            // Arrange
            var model = fixture.Create<CreateNotificationType>();
            var expected = fixture.Create<CommandResult>();
            var criteria = new CreateNotificationTypeCommand();
            var service = GetService();
            baseService.ExecuteNonQueryAsync(Arg.Do<CreateNotificationTypeCommand>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.CreateAsync(model);

            // Assert
            Assert.Equal(expected, actual);

            Assert.Equal(model.NotificationTypeName, criteria.NotificationTypeName);
            Assert.Equal(model.ChatTypeIds, criteria.ChatTypeIds);
            Assert.Equal(model.RecipientTypeIds, criteria.RecipientTypeIds);
            Assert.Equal(model.SystemId, criteria.SystemId);
            Assert.Equal(napSession.UserId, criteria.UserId);
        }

        [Fact]
        public async Task EditAsync_WhenCalled_ReturnExpectedCommandResult()
        {
            // Arrange
            var fixture = new Fixture();
            var model = fixture.Create<EditNotificationType>();
            var expected = fixture.Create<CommandResult>();
            var criteria = new EditNotificationTypeCommand();
            var service = GetService();
            baseService.ExecuteNonQueryAsync(Arg.Do<EditNotificationTypeCommand>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.EditAsync(model);

            // Assert
            Assert.Equal(expected, actual);

            Assert.Equal(model.NotificationTypeId, criteria.NotificationTypeId);
            Assert.Equal(model.NotificationTypeName, criteria.NotificationTypeName);
            Assert.Equal(model.ChatTypeIds, criteria.ChatTypeIds);
            Assert.Equal(model.RecipientTypeIds, criteria.RecipientTypeIds);
            Assert.Equal(model.SystemId, criteria.SystemId);
            Assert.Equal(napSession.UserId, criteria.UserId);
        }

        [Fact]
        public async Task DeleteAsync_WhenCalled_ReturnExpectedCommandResult()
        {
            // Arrange
            var fixture = new Fixture();
            var id = fixture.Create<int>();
            var expected = fixture.Create<CommandResult>();
            var criteria = new DeleteNotificationTypeCommand();
            var service = GetService();
            baseService.ExecuteNonQueryAsync(Arg.Do<DeleteNotificationTypeCommand>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.DeleteAsync(id);

            // Assert
            Assert.Equal(expected, actual);

            Assert.Equal(id, criteria.NotificationTypeId);
            Assert.Equal(napSession.UserId, criteria.UserId);
        }

        #region Helpers

        private NotificationTypeService GetService()
        {
            napSession = Substitute.For<INapSession>();
            baseService = Substitute.For<IBaseService>();
            var service = new NotificationTypeService(napSession, baseService);

            return service;
        }

        #endregion Helpers
    }
}
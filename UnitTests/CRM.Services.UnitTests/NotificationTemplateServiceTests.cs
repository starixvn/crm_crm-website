namespace CRM.Services.UnitTests
{
    using System.Collections.Generic;
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
        public async Task GetAsync_WhenCalled_ReturnExpectedTemplate()
        {
            // Arrange
            var fixture = new Fixture();
            var param = fixture.Create<GetGridDataParam>();
            var totalRows = fixture.Create<int>();
            var expected = fixture.Create<IEnumerable<NotificationTemplate>>();
            var criteria = new GetNotificationTemplateCriteria();
            var service = GetService();
            baseService.GetDataAsync<NotificationTemplate>(Arg.Do<GetNotificationTemplateCriteria>(x => criteria = x)).Returns(expected);
            baseService.When(x => x.GetDataAsync<NotificationTemplate>(Arg.Do<GetNotificationTemplateCriteria>(y => criteria = y)))
                 .Do(x => criteria.TotalRows = totalRows);

            // Act
            var actual = await service.GetAsync(param);

            // Assert
            Assert.Equal(expected, actual);

            Assert.Equal(param.NotificationTypeId, criteria.NotificationTypeId);
            Assert.Equal(param.NotificationTemplateName, criteria.NotificationTemplateName);
            Assert.Equal(param.PageIndex, criteria.PageIndex);
            Assert.Equal(param.PageSize, criteria.PageSize);
            Assert.Equal(totalRows, param.TotalRows);
        }

        [Fact]
        public async Task GetByIdAsync_WhenCalled_ReturnExpectedTemplate()
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Customize<NotificationTemplate>(x => x
                .With(b => b.ContentXml, "<ROOT><Content LanguageId='1' Language='EN' Value='content' /></ROOT>"));
            var id = fixture.Create<int>();
            var template = fixture.Create<NotificationTemplate>();
            var criteria = new GetNotificationTemplateCriteria();
            var service = GetService();
            baseService.GetDataEntityAsync<NotificationTemplate>(Arg.Do<GetNotificationTemplateCriteria>(x => criteria = x)).Returns(template);

            // Act
            var actual = await service.GetByIdAsync(id);

            // Assert
            Assert.Equal(template.NotificationTemplateId, actual.NotificationTemplateId);
            Assert.Equal(template.NotificationTemplateName, actual.NotificationTemplateName);
            Assert.Equal(template.Title, actual.Title);
            Assert.Equal(template.NotificationTypeId, actual.NotificationTypeId);
            Assert.Equal(template.NotificationTypeName, actual.NotificationTypeName);

            Assert.Single(actual.Contents);
            Assert.Collection(actual.Contents,
                item =>
                {
                    Assert.Equal(1, item.LanguageId);
                    Assert.Equal("EN", item.Language);
                    Assert.Equal("content", item.Content);
                });

            Assert.Equal(id, criteria.NotificationTemplateId);
        }

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
            Assert.Equal(model.NotificationTemplateName, criteria.NotificationTemplateName);
            Assert.Equal(model.Title, criteria.Title);
            Assert.Equal(napSession.UserId, criteria.UserId);
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
            Assert.Equal(model.NotificationTemplateName, criteria.NotificationTemplateName);
            Assert.Equal(model.Title, criteria.Title);
            Assert.Equal(napSession.UserId, criteria.UserId);
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
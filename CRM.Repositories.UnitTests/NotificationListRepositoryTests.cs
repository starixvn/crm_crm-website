namespace CRM.Repositories.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Models;
    using CRM.Models.NotificationList;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Xunit;

    public class NotificationListRepositoryTests
    {
        private readonly IObjectDb objectDb;
        private readonly IFixture fixture = new Fixture();

        public NotificationListRepositoryTests()
        {
            DbSettingProviderManager
            .StartTestingSession()
            .Run();

            objectDb = Substitute.For<IObjectDb>();
            ObjectDbFactory.Mock((key, time) => objectDb);
        }

        [Fact]
        public async Task GetNotificationsAsync_WhenCalled_ReturnExpectedResult()
        {
            // Arrange
            var repository = new NotificationListRepository();
            var criteria = Substitute.For<ICriteria>();
            var notifications = fixture.Create<IEnumerable<Notification>>();
            var recipients = fixture.Create<IEnumerable<Recipient>>();
            var headlines = fixture.Create<IEnumerable<Headline>>();
            var multi = Substitute.For<IMultiResultReader>();
            multi.Read<Notification>().Returns(notifications);
            multi.Read<Recipient>().Returns(recipients);
            multi.Read<Headline>().Returns(headlines);
            objectDb.QueryMultipleAsync(criteria).Returns(multi);

            // Action
            var result = await repository.GetNotificationsAsync(criteria);

            // Assert
            Assert.Equal(notifications, result);
        }

        [Fact]
        public async Task ValidateNotificationListAsync_WhenCalled_ReturnExpectedResult()
        {
            // Arrange
            var repository = new NotificationListRepository();
            var criteria = Substitute.For<ICriteria>();
            var multi = Substitute.For<IMultiResultReader>();
            var inActiveNotificationIds = fixture.Create<IEnumerable<int>>();
            var resendDataList = fixture.Create<IEnumerable<NotificationListResend>>();

            multi.Read<int>().Returns(inActiveNotificationIds);
            multi.Read<NotificationListResend>().Returns(resendDataList);
            objectDb.QueryMultipleAsync(criteria).Returns(multi);

            // Action
            var result = await repository.ValidateNotificationListAsync(criteria);

            // Assert
            Assert.Equal(inActiveNotificationIds, result.InActiveNotificationIds);
            Assert.Equal(resendDataList, result.ResendDataList);
        }
    }
}
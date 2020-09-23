namespace CRM.BackgroundJob.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoFixture;
    using CRM.Core.Services;
    using Fanex.CRM.BackgroundJob.Criteria;
    using Fanex.CRM.BackgroundJob.Models;
    using Fanex.CRM.BackgroundJob.Repositories;
    using Fanex.CRM.BackgroundJob.Services;
    using NSubstitute;
    using Xunit;

    public class CustomerServiceTests
    {
        private ICustomerRepository repository;
        private IBaseService baseService;
        private readonly IFixture fixture = new Fixture();

        [Fact]
        public async Task GetCustomerInfoAsync_WhenCalled_ReturnExpectedCustomer()
        {
            // Arrange
            var expected = fixture.Create<CustomerInfo>();
            var username = fixture.Create<string>();
            var isGuest = fixture.Create<bool>();
            var criteria = new GetCustomerInfoByUsernameCriteria();
            var service = GetService();
            repository.GetCustomerInfoByUsernameAsync(Arg.Do<GetCustomerInfoByUsernameCriteria>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.GetCustomerInfoAsync(username, isGuest);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(username, criteria.Username);
            Assert.Equal(isGuest, criteria.IsGuest);
        }

        [Fact]
        public async Task GetGroupHeadlineAsync_WhenCalled_ReturnExpectedHeadlines()
        {
            // Arrange
            var expected = fixture.Create<IEnumerable<GroupHeadline>>();
            var custIds = fixture.Create<IEnumerable<int>>();
            var criteria = new GetGroupHeadlineCriteria();
            var service = GetService();
            baseService.GetDataAsync<GroupHeadline>(Arg.Do<GetGroupHeadlineCriteria>(x => criteria = x)).Returns(expected);

            // Act
            var actual = await service.GetGroupHeadlineAsync(custIds);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(string.Join(",", custIds), criteria.CustIds);
        }

        #region Helpers

        private CustomerService GetService()
        {
            repository = Substitute.For<ICustomerRepository>();
            baseService = Substitute.For<IBaseService>();
            return new CustomerService(repository, baseService);
        }

        #endregion Helpers
    }
}
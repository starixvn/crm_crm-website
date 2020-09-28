namespace CRM.BackgroundJob.UnitTests.Criteria.ChatBot
{
    using AutoFixture;
    using Fanex.CRM.BackgroundJob.Criteria.ChatBot;
    using Xunit;

    public class ImportChatBotTicketVoidingInfoCommandTests
    {
        [Fact]
        public void GetSettingKey_WhenCalled_ReturnExpectedSettingKey()
        {
            // Arrange
            var criteria = new ImportChatBotTicketVoidingInfoCommand();
            var expected = "Acc_CRM_Chatbot_AbnormalBets_Import";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsValid_WhenCalled_ReturnTrue()
        {
            // Arrange
            var fixture = new Fixture();
            var criteria = fixture.Create<ImportChatBotTicketVoidingInfoCommand>();

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}
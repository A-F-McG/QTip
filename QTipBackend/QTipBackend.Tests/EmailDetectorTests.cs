using QTipBackend.PiiDetection;

namespace QTipBackend.Tests
{
    public class EmailDetectorTests
    {
        private readonly EmailDetector _emailDetector;

        public EmailDetectorTests()
        {
            _emailDetector = new EmailDetector();
        }

        [Fact]
        public void ShouldFind0Emails_WhenStringIsEmpty()
        {
            string text = "";
            var emails = _emailDetector.Detect(text);

            Assert.Empty(emails);
        }

        [Fact]
        public void ShouldFind0Emails_WhenNoPeriod()
        {
            string text = "test@emailcom";
            var emails = _emailDetector.Detect(text);

            Assert.Empty(emails);
        }

        [Fact]
        public void ShouldFind1Email()
        {
            string text = "here is an email test@email.com more filler words";

            var emails = _emailDetector.Detect(text);

            Assert.Single(emails);
            Assert.Equal("test@email.com", emails[0]);
        }

        [Fact]
        public void ShouldFindEmail_WhenSurroundedByPunctuation()
        {
            string text = "!test@email.com!";

            var emails = _emailDetector.Detect(text);

            Assert.Single(emails);
        }

        [Fact]
        public void ShouldFindMultipleEmails()
        {
            string text = "!test@email.com! test@email.com";

            var emails = _emailDetector.Detect(text);

            Assert.Equal(2, emails.Count);
        }
        
        //a bug really, but this an edge case not delved into in this project
        [Fact]
        public void ShouldFind1Email_When2EmailsCombinedWithoutASpace()
        {
            string text = "test@email.comtest@email.com";

            var emails = _emailDetector.Detect(text);

            Assert.Single(emails);
        }
    }
}
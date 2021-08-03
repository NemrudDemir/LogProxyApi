using AutoMapper;
using LogProxyApi.Abstractions;
using LogProxyApi.Controllers;
using LogProxyApi.Dtos.AirTableApi;
using LogProxyApi.Dtos.Receiving;
using LogProxyApi.Mappings;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace LogProxyApiTests
{
    public class Tests
    {
        Mock<IThirdPartyApiCommunicator> _communicatorMock;
        Mock<ILogger<LogProxyController>> _loggerMock;
        IMapper _mapper;
        Mock<IIdGenerator> _idGenMock;

        LogProxyController _classUnderTest;

        [SetUp]
        public void Setup()
        {
            _communicatorMock = new Mock<IThirdPartyApiCommunicator>();
            _loggerMock = new Mock<ILogger<LogProxyController>>();
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MessageProfile()); //your automapperprofile 
            });
            _mapper = mockMapper.CreateMapper();
            _idGenMock = new Mock<IIdGenerator>();

            _classUnderTest = new LogProxyController(
                _communicatorMock.Object, _mapper, _idGenMock.Object, _loggerMock.Object);

            _idGenMock.Setup(idGen => idGen.GetId()).Returns(DateTime.Now.Ticks.ToString());
        }

        [TearDown]
        public void TearDown()
        {
            _communicatorMock = null;
            _loggerMock = null;
            _mapper = null;
            _idGenMock = null;

            _classUnderTest = null;
        }

        [Test]
        public async Task GetShouldCallApiCommunicator()
        {
            var _ = await _classUnderTest.Get();
            _communicatorMock.Verify(communicator => communicator.GetMessages(), Times.Once);
        }

        [Test]
        public async Task GetShouldLogInformation()
        {
            var _ = await _classUnderTest.Get();
            _loggerMock.VerifyLog(logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public async Task PostShouldEnrichMessage()
        {
            var postMessage = new PostMessage { Text = "Test", Title = "TitleTest" };
            var _ = await _classUnderTest.Post(postMessage);

            _communicatorMock.Verify(communicator => 
                communicator.TransferMessage(
                    It.Is<Message>(msg => 
                        msg.Id != null && msg.ReceivedAt != default
                    )), Times.Once);
        }
    }
}
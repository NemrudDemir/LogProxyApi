using AutoMapper;
using LogProxyApi.Abstractions;
using LogProxyApi.Dtos.AirTableApi;
using LogProxyApi.Implementations;
using LogProxyApi.Mappings;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LogProxyApiTests.Implementations
{
    public class AirTableApiCommunicatorTests
    {
        Mock<IAirTableRestClient> _restClientMock;
        IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _restClientMock = new Mock<IAirTableRestClient>();
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MessageProfile()); //your automapperprofile 
            });
            _mapper = mockMapper.CreateMapper();
        }

        [TearDown]
        public void TearDown()
        {
            _restClientMock = null;
            _mapper = null;
        }

        [Test]
        public async Task GetMessagesShouldCallRestClientOnce()
        {
            var classUnderTest = new AirTableApiCommunicator(_restClientMock.Object, _mapper);
            var _ = await classUnderTest.GetMessages();

            _restClientMock.Verify(client => client.GetRecords(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task TransferMessageShouldCallRestClientOnce()
        {
            var classUnderTest = new AirTableApiCommunicator(_restClientMock.Object, _mapper);
            var _ = await classUnderTest.TransferMessage(new Message());

            _restClientMock.Verify(client => 
                client.TransferRecords(
                    It.IsAny<IEnumerable<Record>>(), It.IsAny<CancellationToken>()
                    ), Times.Once);
        }
    }
}

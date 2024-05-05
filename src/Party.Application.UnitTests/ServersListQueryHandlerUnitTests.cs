using AutoFixture;
using FluentAssertions;
using Moq;
using Party.Domain;


namespace Party.Application.UnitTests
{
    public class ServersListQueryHandlerUnitTests
    {
        private Mock<IServersListGateway> _serversGatewayMock;
        private Mock<IServersRepository> _serversRepositoryMock;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _serversGatewayMock = new Mock<IServersListGateway>();
            _serversRepositoryMock = new Mock<IServersRepository>();
            _fixture = new Fixture();
        }

        private ServersListQueryHandler CreateUat()
        {
            return new ServersListQueryHandler(_serversGatewayMock.Object, _serversRepositoryMock.Object);
        }

        [Test]
        public void CanBeCreated()
        {
            //Arrange
            //Act
            var uat = CreateUat();

            //Assert
            uat.Should().NotBeNull();
        }

        [Test]
        public void Initiating_WhenGatewayServiceIsNotSet_ShouldThrow()
        {
            //Arrange
            var serversRepositoryMock = new Mock<IServersRepository>();

            //Act
            Action a = () => new ServersListQueryHandler(null, serversRepositoryMock.Object);

            //Assert
            a.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Initiating_WhenRepositoryIsNotSet_ShouldThrow()
        {
            //Arrange
            var serversGatewayMock = new Mock<IServersListGateway>();

            //Act
            Action a = () => new ServersListQueryHandler(serversGatewayMock.Object, null);

            //Assert
            a.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Handle_WhenServicesNotSet_DoNotThrow()
        {
            //Arrange
            var uat = CreateUat();
            var query = new FetchServersQuery();

            //Act
            Action a = () => uat.Handle(query, CancellationToken.None);

            //Assert
            a.Should().NotThrow<Exception>();
        }

        [Test]
        public void Handle_WhenNoArguments_ShouldCallGatewayToGetServers()
        {
            //Arrange
            var uat = CreateUat();
            var query = new FetchServersQuery();

            //Act
            uat.Handle(query, CancellationToken.None);

            //Assert
            _serversGatewayMock.Verify(s => s.GetServers(), Times.Once);
        }

        [TestCaseSource(nameof(CountryList))]
        public void Handle_WhenCountryProvided_ShouldCallGatewayToGetServersByCountry(string country)
        {

            //Arrange
            var uat = CreateUat();
            var query = new FetchServersQuery(country);

            //Act
            uat.Handle(query, CancellationToken.None);

            //Assert
            _serversGatewayMock.Verify(s => s.GetServersByCountry(country), Times.Once);
        }

        [TestCase("TCP")]
        [TestCase("UDP")]
        public void Handle_WhenProtocolProvided_ShouldCallGatewayToGetServersByProtocol(string protocol)
        {

            //Arrange
            var uat = CreateUat();
            var query = new FetchServersQuery(null, protocol);

            //Act
            uat.Handle(query, CancellationToken.None);

            //Assert
            _serversGatewayMock.Verify(s => s.GetServersByProtocol(protocol), Times.Once);
        }

        [Test]
        public void Handle_WhenLocal_ShouldCallRepositoryToGetServers()
        {
            //Arrange
            var uat = new ServersListQueryHandler(_serversGatewayMock.Object, _serversRepositoryMock.Object);
            var query = new FetchServersQuery(null, null, true);

            //Act
            uat.Handle(query, CancellationToken.None);

            //Assert
            _serversRepositoryMock.Verify(s => s.GetServers());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(99)]
        [TestCase(1001)]
        public async Task Handle_WhenLocal_ShouldReturnSameCountOfServers(int countOfServers)
        {
            //Arrange
            var uat = CreateUat();
            var servers = _fixture.CreateMany<Server>(countOfServers);
            _serversRepositoryMock.Setup(g => g.GetServers()).Returns(servers);
            var query = new FetchServersQuery(local: true);

            //Act
            var r = await uat.Handle(query, CancellationToken.None);

            //Assert
            _serversRepositoryMock.Verify(r=>r.GetServers(), Times.Once);
            r.Count().Should().Be(countOfServers);
        }

        [Test]
        public void Handle_WhenLocal_ShouldNotCallGateway()
        {
            //Arrange
            var uat = CreateUat();
            var query = new FetchServersQuery(null, null, true);

            //Act
            uat.Handle(query, CancellationToken.None);

            //Assert
            _serversGatewayMock.VerifyNoOtherCalls();
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(99)]
        [TestCase(1001)]
        public async Task Handle_WhenGatewayReturnsServers_ShouldReturnSameCount(int countOfServers)
        {
            //Arrange
            var uat = CreateUat();
            var servers = _fixture.CreateMany<Server>(countOfServers);
            _serversGatewayMock.Setup(g => g.GetServers()).Returns(servers);
            var query = new FetchServersQuery();

            //Act
            var r = await uat.Handle(query, CancellationToken.None);

            //Assert
            r.Count().Should().Be(countOfServers);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(99)]
        [TestCase(1001)]
        public async Task Handle_WhenCountryProvided_ShouldReturnValuesFromGatewayByCountry(int countOfServers)
        {
            //Arrange
            var uat = CreateUat();
            var servers = _fixture.CreateMany<Server>(countOfServers);
            var country = _fixture.Create<string>();
            _serversGatewayMock.Setup(g => g.GetServersByCountry(It.IsAny<string>())).Returns(servers);
            var query = new FetchServersQuery(country);

            //Act
            var r = await uat.Handle(query, CancellationToken.None);

            //Assert
            r.Count().Should().Be(countOfServers);
        }

        [Test]
        [TestCase(1, "TCP")]
        [TestCase(1, "UDP")]
        [TestCase(2, "TCP")]
        [TestCase(2, "UDP")]
        [TestCase(101, "TCP")]
        [TestCase(101, "UDP")]
        public async Task Handle_WhenProtocolProvided_ShouldReturnValuesFromGatewayByProtocol(int countOfServers, string protocol)
        {
            //Arrange
            var uat = CreateUat();
            var servers = _fixture.CreateMany<Server>(countOfServers);
            _serversGatewayMock.Setup(g => g.GetServersByProtocol(protocol)).Returns(servers);
            var query = new FetchServersQuery(protocol: protocol);

            //Act
            var r = await uat.Handle(query, CancellationToken.None);

            //Assert
            r.Count().Should().Be(countOfServers);
        }

        private static readonly object[] CountryList =
        [
            new object[] {"Belgium"},
            new object[] {"Hungary"},
            new object[] {"United States"},
            new object[] {"Switzerland"},
            new object[] {"Denmark"},
            new object[] {"Norway"},
            new object[] {"France"},
            new object[] {"Switzerland"},
            new object[] {"United Kingdom"},
            new object[] {"Germany"},
            new object[] {"Netherlands"},
            new object[] {"France"},
            new object[] {"Japan"},
            new object[] {"Spain"},
            new object[] {"Italy"},
            new object[] {"Poland"},
            new object[] {"Czech Republic"},
            new object[] {"Austria"},
            new object[] {"Romania"},
            new object[] {"Switzerland"},
            new object[] {"Bulgaria"}
        ];
    }
}
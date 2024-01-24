using ClinicService.Controllers;
using ClinicService.Models;
using ClinicService.Models.Requests;
using ClinicService.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicService.Tests
{
    public class ClientControllerTests
    {
        private ClientController _clientController;
        private Mock<IClientRepository> _mockClientRepository;

        public ClientControllerTests()
        {
            _mockClientRepository = new Mock<IClientRepository>();
            _clientController = new ClientController(_mockClientRepository.Object);
        }


        [Fact]
        public void GetAllClientsTest()
        {
            // [1] Подготовка данных для тестирования

            List<Client> clientsList = new List<Client>();
            clientsList.Add(new Client());
            clientsList.Add(new Client());
            clientsList.Add(new Client());

            _mockClientRepository
                .Setup(repository => repository.GetAll())
                .Returns(clientsList);

            // [2] Исполнение тестируемого метода


            var operationResult = _clientController.GetAll();

            // [3] Подготовка эталонного результата и проверка результата
            Assert.IsType<OkObjectResult>(operationResult.Result);

            var result = (OkObjectResult)operationResult.Result;
            Assert.IsAssignableFrom<List<Client>>(result.Value);

            _mockClientRepository.Verify(repository =>
            repository.GetAll(), Times.AtLeastOnce);

        }

        public static object[][] CorrectCreateClientData =
        {
            new object[] { new DateTime(1986, 1, 22), "AA1 B11222", "Иванов1", "Станислав1", "Андреевич1" },
            new object[] { new DateTime(1986, 1, 22), "AA1 B11222", "Иванов2", "Станислав2", "Андреевич2" },
            //new object[] { new DateTime(2013, 1, 22), "AA1 B11222", "Иванов3", "Станислав3", "Андреевич3" },
            new object[] { new DateTime(2018, 1, 22), "AA1 B11222", "Иванов4", "Станислав4", "Андреевич4" },
            new object[] { new DateTime(1986, 1, 22), "AA1 B11222", "Иванов5", "Станислав5", "Андреевич5" },
            //new object[] { new DateTime(1986, 1, 22), "AA1 B11222", "Иванов", "", "Андреевич" },
            new object[] { new DateTime(1986, 1, 22), "AA1 B11222", "Иванов6", "Станислав6", "Андреевич6" },
            new object[] { new DateTime(1986, 1, 22), "AA1 B11222", "Иванов7", "Станислав7", "Андреевич7" },
            new object[] { new DateTime(1986, 1, 22), "AA1 B11222", "Иванов8", "Станислав8", "Андреевич8" },

        };
        public static List<Client> testClientList() {
            List<Client> clientsList = new List<Client>();
            Client client = new Client();
            for (int i = 0; i < 5; i++)
            {
                client.ClientId = i;
                client.Document = "AAA" + i + "B" + i + "222";
                client.FirstName = "Иванов" + i;
                client.SurName = "Иван" + i;
                client.Patronymic = "Иванович" + i;
                client.Birthday = new DateTime(1986, 1, 22);
                clientsList.Add(client);
            }
            return clientsList;

        }

        [Theory]
        [MemberData(nameof(CorrectCreateClientData))]
        public void CreateClientTest(
            DateTime birthday, string document, string surName, string firstName, string patronymic)
        {
            // [1] Подготовка данных для тестирования

            CreateClientRequest createClientRequest = new CreateClientRequest();
            createClientRequest.Birthday = birthday;
            createClientRequest.Document = document;
            createClientRequest.SurName = surName;
            createClientRequest.FirstName = firstName;
            createClientRequest.Patronymic = patronymic;

            int res = 1;

            _mockClientRepository
                .Setup(repository => repository.Create(It.IsNotNull<Client>()))
                .Returns(res);


            // [2] Исполнение тестируемого метода
            var operationResult = _clientController.Create(createClientRequest);

            // [3] Подготовка эталонного результата и проверка результата
            Assert.IsType<OkObjectResult>(operationResult.Result);

            var result = (OkObjectResult)operationResult.Result;
            Assert.IsAssignableFrom<int>(result.Value);

            _mockClientRepository.Verify(repository =>
            repository.Create(It.IsNotNull<Client>()), Times.AtLeastOnce());
        }
        [Fact]
        public void DeleteClientsTest()
        {
            // [1] Подготовка данных для тестирования
            List<Client> clients = testClientList();

            int testId = 2;
            _mockClientRepository
                .Setup(repository => repository.Delete(It.IsAny<int>())).Returns(testId);
   
            // [2] Исполнение тестируемого метода


            var operationResult = _clientController.Delete(testId);

            // [3] Подготовка эталонного результата и проверка результата
            Assert.IsType<OkObjectResult>(operationResult.Result);

            var result = (OkObjectResult)operationResult.Result; 
            Assert.IsAssignableFrom<int>(result.Value);

            _mockClientRepository.Verify(repository =>
            repository.Delete(testId), Times.AtLeastOnce);
        }
        [Fact]
        public void GetByIdTest()
        {

            // [1] Подготовка данных для тестирования
            int res = 1;
                _mockClientRepository
                    .Setup(repository => repository.GetById(It.IsAny<int>())).Returns(new Client());
            
                // [2] Исполнение тестируемого метода

                var operationResult = _clientController.GetById(res);
                // [3] Подготовка эталонного результата и проверка результата
                Assert.IsType<OkObjectResult>(operationResult.Result);

                var result = (OkObjectResult)operationResult.Result;
                _mockClientRepository.Verify(repository =>
                repository.GetById(res), Times.AtLeastOnce);

        }
    }
}

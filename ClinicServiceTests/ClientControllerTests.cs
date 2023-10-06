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

namespace ClinicServiceTests
{
    public class ClientControllerTests
    {
        private ClientController _clientController;
        private Mock<IClientRepository> _mocClientRepository;

        public ClientControllerTests() {
            _mocClientRepository = new Mock<IClientRepository> ();
            _clientController = new ClientController(_mocClientRepository.Object);
        }

        [Fact]
        public void GetAllClientsTest()
        {
            // [1.1] Подготовка данных для тестирования

            // [1.2]

            List<Client> list = new List<Client>();
            list.Add(new Client());
            list.Add(new Client());
            list.Add(new Client());


            _mocClientRepository.Setup(repository =>
                repository.GetAll()).Returns(list);

            // [2] Исполнение тестирования метода
            var operationResult = _clientController.GetAll();

            // [3] Подготовка эталонного результата, проверка результата
            Assert.IsType<OkObjectResult>(operationResult.Result);
            Assert.IsAssignableFrom<List<Client>>(((OkObjectResult)operationResult.Result).Value);

            _mocClientRepository.Verify(repository =>
                repository.GetAll(), Times.AtLeastOnce());
        }

        public static readonly object[][] CorrectCreateClientData =
        {
            new object[] {new DateTime(2000, 9, 20), "123 123123", "VVVVVVVVV", "VVVVVVV", "VVV"},
            new object[] {new DateTime(2001, 11, 21), "222 123123", "VVVVVVVVV", "VVVVVVV", "VVV"},
            new object[] {new DateTime(2002, 7, 30), "333 123123", "VVVVVVVVV", "VVVVVVV", "VVV"}

        };

        [Theory]
        [MemberData(nameof(CorrectCreateClientData))]
        public void CreateClientTest(DateTime birthday, string document, string surName,
            string firstName, string patronymic)
        {
            // [1]

            _mocClientRepository.Setup(repository =>
                repository.Create(It.IsNotNull<Client>())).Returns(1).Verifiable();

            // [2]

            var operationResult = _clientController.Create(new CreateClientRequest
            {
                Birthday = birthday, 
                Document = document, 
                SurName = surName,
                FirstName = firstName,
                Patronymic = patronymic
            });

            // [3]

            Assert.IsType<OkObjectResult>(operationResult.Result);
            Assert.IsAssignableFrom<int>(((OkObjectResult)operationResult.Result).Value);
            _mocClientRepository.Verify(repository =>
                repository.Create(It.IsNotNull<Client>()), Times.AtLeastOnce());

        }

        [Fact]
        public void GetClientByIdTest()
        {

            // [1]

            int id = 0;

            List<Client> list = new List<Client>();
            list.Add(new Client {Birthday =  new DateTime(2000, 9, 20), 
                Document = "123 123123",
                SurName = "VVVVVVVVV", 
                FirstName = "VVVVVVV", 
                Patronymic = "VVV"});
            list.Add(new Client
            {
                Birthday = new DateTime(2000, 9, 20),
                Document = "2222 123123",
                SurName = "QQQQQQQQQQQ",
                FirstName = "VVVVVVV",
                Patronymic = "VVV"
            });

            Client client = list[0];

            _mocClientRepository.Setup(repository =>
                repository.GetById(id)).Returns(client);

            // [2]

            var operationResult = _clientController.GetById(id);
            
            // [3]

            Assert.IsType<OkObjectResult>(operationResult.Result);
            Assert.IsAssignableFrom<Client>(((OkObjectResult)operationResult.Result).Value);
            Assert.Equal(((OkObjectResult)operationResult.Result).Value, list[id]);
        }

        

        [Fact]
        public void UpdateClientTest()
        {
            // [1]

            Client newClient = new Client
            {
                ClientId = 1,
                Birthday = new DateTime(2000, 9, 20),
                Document = "5555 123123",
                SurName = "MMMM",
                FirstName = "MMMMMM",
                Patronymic = "NNNN"
            };

            List<Client> list = new List<Client>();
            list.Add(new Client
            {
                ClientId = 1,
                Birthday = new DateTime(2000, 9, 20),
                Document = "123 123123",
                SurName = "VVVVVVVVV",
                FirstName = "VVVVVVV",
                Patronymic = "VVV"
            });
            list.Add(new Client
            {
                ClientId = 2,
                Birthday = new DateTime(2000, 9, 20),
                Document = "2222 123123",
                SurName = "QQQQQQQQQQQ",
                FirstName = "VVVVVVV",
                Patronymic = "VVV"
            });

            

            _mocClientRepository.Setup(repository =>
                repository.Update(It.IsNotNull<Client>())).Returns(1).Verifiable();

            // [2]

            var operationResult = _clientController.Update(new UpdateClientRequest
            {
                ClientId = newClient.ClientId,
                Birthday = newClient.Birthday, 
                Document = newClient.Document, 
                SurName = newClient.SurName,
                FirstName = newClient.FirstName,
                Patronymic = newClient.Patronymic
            });

            // [3]

            Assert.IsType<OkObjectResult>(operationResult.Result);
            Assert.IsAssignableFrom<int>(((OkObjectResult)operationResult.Result).Value);
            _mocClientRepository.Verify(repository =>
                repository.Update(It.IsNotNull<Client>()), Times.AtLeastOnce());

        }
    }
}

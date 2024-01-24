using ClinicService.Models.Requests;
using ClinicService.Models;
using ClinicService.Services;
using ClinicService.Services.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {

        private IClientRepository _clientRepository;

        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpPost("create", Name = "ClientCreate")]
        public ActionResult<int> Create([FromBody] CreateClientRequest createRequest)
        {
            if (string.IsNullOrEmpty(createRequest.SurName))
                return Ok(new
                {
                    ErrCode = -10,
                    ErrMessage = "Фамилия указана некорректно."
                });
            if (string.IsNullOrEmpty(createRequest.FirstName))
                return Ok(new
                {
                    ErrCode = -11,
                    ErrMessage = "Имя указано некорректно."
                });
            if(string.IsNullOrEmpty(createRequest.Patronymic))
                return Ok(new
                {
                    ErrCode = -12,
                    ErrMessage = "Отчетство указано некорректно."
                });
            var dateFrom = DateTime.Now.AddYears(-18);
            if (createRequest.Birthday > dateFrom)
                return Ok(new
                {
                    ErrCode = -13,
                    ErrMessage = "Возраст должен быть указан корректно (больше 18 лет)."
                });
            if (string.IsNullOrEmpty(createRequest.Document) || createRequest.Document.Length < 10)
                return Ok(new
                {
                    ErrCode = -14,
                    ErrMessage = "Документ должен быть указан корректно."
                });

            Client client = new Client();
            client.Document = createRequest.Document;
            client.SurName = createRequest.SurName;
            client.FirstName = createRequest.FirstName;
            client.Patronymic = createRequest.Patronymic;
            client.Birthday = createRequest.Birthday;
            return Ok(_clientRepository.Create(client));
        }

        [HttpPut("edit", Name = "ClientUpdate")]
        public ActionResult<int> Update([FromBody] UpdateClientRequest updateRequest)
        {
            Client client = new Client();
            client.ClientId = updateRequest.ClientId;
            client.Document = updateRequest.Document;
            client.SurName = updateRequest.SurName;
            client.FirstName = updateRequest.FirstName;
            client.Patronymic = updateRequest.Patronymic;
            client.Birthday = updateRequest.Birthday;
            return Ok(_clientRepository.Update(client));
        }


        [HttpDelete("delete", Name = "ClientDelete")]
        public ActionResult<int> Delete([FromQuery] int clientId)
        {
            int res = _clientRepository.Delete(clientId);
            return Ok(res);
        }

        [HttpGet("get-all", Name = "ClientGetAll")]
        public ActionResult<List<Client>> GetAll()
        {
            //List<Client> clients = new List<Client>();
            //clients.Add(new Client());
            //return Ok(clients);
            return Ok(_clientRepository.GetAll());
        }

        [HttpGet("get/{clientId}", Name = "ClientGetById")]
        public ActionResult<Client> GetById([FromRoute] int clientId)
        {
            return Ok(_clientRepository.GetById(clientId));
        }

    }
}

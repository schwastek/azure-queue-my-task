using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;

namespace AzureQueueMyTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : Controller
{
    private readonly ServiceBusSender _serviceBusSender;

    public TasksController(IAzureClientFactory<ServiceBusSender> senderFactory)
    {
        _serviceBusSender = senderFactory.CreateClient("tasks");
    }

    [HttpPost]
    public async Task<ActionResult> AddTask([FromBody] string taskDescription)
    {
        var message = new ServiceBusMessage(taskDescription);
        await _serviceBusSender.SendMessageAsync(message);

        return NoContent();
    }
}

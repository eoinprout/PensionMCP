using Microsoft.EntityFrameworkCore;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using PensionMCP.Common;
using PensionMCP.Data;
using PensionMCP.Models;
using System.ComponentModel;

namespace PensionMCP.Mcp
{
    [McpServerToolType]
    public class ClientTools(PensionDbContext context) : BaseTool
    {

        [McpServerTool(Title = "Client List", Destructive = false, ReadOnly = true, Idempotent = true, OpenWorld = false)]
        [Description("Returns all client records as JSON")]
        public async Task<string> GetAllClients()
        {
            var clients = await context.Clients.ToListAsync();
            return ToJson(clients);
        }

        [McpServerTool(Title = "Client Exists", Destructive = false, ReadOnly = true, Idempotent = true, OpenWorld = false)]
        [Description("Checks if a client with the given name and date of birth exists. Returns true or false. dateOfBirth format: yyyy-MM-dd.")]
        public async Task<bool> ClientExists(string name, string dateOfBirth)
        {
            DateOnly dob = ParseDateParam(dateOfBirth, "dateOfBirth");
            return await context.Clients.AnyAsync(c => c.Name == name && c.DateOfBirth == dob);
        }

        [McpServerTool(Title = "Add Client", Destructive = false, ReadOnly = false, Idempotent = false, OpenWorld = false)]
        [Description("Creates a new client record with name and date of birth. Returns the new client as JSON. dateOfBirth format: yyyy-MM-dd.")]
        public async Task<string> AddClient(string name, string dateOfBirth)
        {
            CheckRequired(name, "name");
            DateOnly dob = ParseDateParam(dateOfBirth, "dateOfBirth");

            if (await ClientExists(name, dateOfBirth))
                throw new McpException($"A client with name '{name}' and date of birth '{dateOfBirth}' already exists. If this is a new client the try adding the clients initial to differentiate them.");

            var client = new Client
            {
                Name = name,
                DateOfBirth = dob,
                NetRelevantIncome = Constants.AmountNotSet
            };

            context.Clients.Add(client);
            await context.SaveChangesAsync();

            return ToJson(client);
        }
    }
}

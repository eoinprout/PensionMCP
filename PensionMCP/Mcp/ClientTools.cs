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

        [McpServerTool(Title = "Search Clients", Destructive = false, ReadOnly = true, Idempotent = true, OpenWorld = false)]
        [Description("Searches for client records where the name contains the given search term. Returns matching clients as JSON.")]
        public async Task<string> SearchClients(string name)
        {
            CheckRequired(name, "name");
            var search = name.ToLower();
            var clients = await context.Clients
                .Where(c => c.Name.ToLower().Contains(search))
                .ToListAsync();
            return ToJson(clients);
        }

        [McpServerTool(Title = "Client Exists", Destructive = false, ReadOnly = true, Idempotent = true, OpenWorld = false)]
        [Description("Checks if a client with the given name and date of birth exists. Returns true or false. dateOfBirth format: yyyy-MM-dd.")]
        public async Task<bool> ClientExists(string name, string dateOfBirth)
        {
            DateOnly dob = ParseDateParam(dateOfBirth, nameof(dateOfBirth));
            return await context.Clients.AnyAsync(c => c.Name == name && c.DateOfBirth == dob);
        }

        [McpServerTool(Title = "Add Client", Destructive = false, ReadOnly = false, Idempotent = false, OpenWorld = false)]
        [Description("Creates a new client record with name and date of birth. Returns the new client as JSON. dateOfBirth format: yyyy-MM-dd.")]
        public async Task<string> AddClient(string name, string dateOfBirth)
        {
            CheckRequired(name, "name");
            DateOnly dob = ParseDateParam(dateOfBirth, nameof(dateOfBirth));

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

        /// <summary>
        /// Deletes a client record
        /// </summary>
        /// <param name="id">The clients id</param>
        /// <returns>A JSON string representing the clients details</returns>
        /// <exception cref="McpException"></exception>
        /// <remarks>By returning the deleted client as JSON to the agent, it may allow the agent to undo the delete
        /// is nessacary as it will have all the clients details</remarks>
        [McpServerTool(Title = "Delete Client", Destructive = true, ReadOnly = false, Idempotent = false, OpenWorld = false)]
        [Description("Permanently deletes a client record by their Id. Returns the deleted client as JSON. Use Search Clients to find the Id first. Ask the user to confirm before deleting the record")]
        public async Task<string> DeleteClient(int id)
        {
            var client = await FindClientAsync(id);

            context.Clients.Remove(client);
            await context.SaveChangesAsync();

            return ToJson(client);
        }

        [McpServerTool(Title = "Update Clients Name", Destructive = false, ReadOnly = false, Idempotent = false, OpenWorld = false)]
        [Description("Updates the name of an existing client record. Returns the updated client as JSON.")]
        public async Task<string> UpdateClientName(int id, string name)
        {
            CheckRequired(name, "name");

            var client = await FindClientAsync(id);

            client.Name = name;
            await context.SaveChangesAsync();

            return ToJson(client);
        }

        [McpServerTool(Title = "Update clients date of birth", Destructive = false, ReadOnly = false, Idempotent = false, OpenWorld = false)]
        [Description("Updates the date of birth of an existing client record. Returns the updated client as JSON. dateOfBirth format: yyyy-MM-dd.")]
        public async Task<string> UpdateClientDateOfBirth(int id, string dateOfBirth)
        {
            DateOnly dob = ParseDateParam(dateOfBirth, nameof(dateOfBirth));

            var client = await FindClientAsync(id);

            client.DateOfBirth = dob;
            await context.SaveChangesAsync();

            return ToJson(client);
        }

        [McpServerTool(Title = "Update clients net relevant income", Destructive = false, ReadOnly = false, Idempotent = false, OpenWorld = false)]
        [Description("Updates the net relevant income of an existing client record. Returns the updated client as JSON.")]
        public async Task<string> UpdateClientNetRelevantIncome(int id, decimal netRelevantIncome)
        {
            var client = await FindClientAsync(id);

            client.NetRelevantIncome = netRelevantIncome;
            await context.SaveChangesAsync();

            return ToJson(client);
        }

        private async Task<Client> FindClientAsync(int id)
        {
            var client = await context.Clients.FindAsync(id);
            if (client == null)
                throw new McpException($"Client with Id {id} was not found.");
            return client;
        }
    }
}

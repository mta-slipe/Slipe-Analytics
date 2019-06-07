using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SlipeAnalyticsServer
{
    class AnalyticsServer
    {
        static void Main(string[] args)
        {
            new AnalyticsServer();
        }

        public AnalyticsServer()
        {
            HandleRequests().Wait();
        }

        public async Task HandleRequests()
        {
            new AnalyticsContext();
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(string.Format("{0}://{1}:{2}/", "http", "127.0.0.1", 667));
            listener.Start();
            Console.WriteLine("Started listening on {0}", string.Format("{0}://{1}:{2}/", "http", "127.0.0.1", 667));

            while (true)
            {
                var context = await listener.GetContextAsync();

                try
                {
                    StreamReader reader = new StreamReader(context.Request.InputStream);
                    string requestBody = (await reader.ReadToEndAsync());

                    JsonDocument json = JsonDocument.Parse(requestBody);
                    string project = json.RootElement.GetProperty("project").GetString();
                    string command = json.RootElement.GetProperty("command").GetString();
                    Console.WriteLine(command);

                    var sha = System.Security.Cryptography.SHA256.Create();
                    byte[] ipHash = sha.ComputeHash(Encoding.UTF8.GetBytes(context.Request.RemoteEndPoint.Address.ToString()));
                    using (AnalyticsContext db = new AnalyticsContext())
                    {
                        db.Entries.Add(new AnalyticsEntry()
                        {
                            Project = project,
                            Command = command,
                            IpHash = Convert.ToBase64String(ipHash),
                            Timestamp = DateTime.UtcNow
                        });
                        db.SaveChanges();
                    }
                    context.Response.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                    context.Response.StatusCode = 500;
                    context.Response.Close();
                }
            }
        }
    }
}

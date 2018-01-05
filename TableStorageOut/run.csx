using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, ICollector<SpacePerson> tableBinding, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");

    // parse query parameter
    string name = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
        .Value;
    string fruit = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "fruit", true) == 0)
        .Value;

    // Get request body
    dynamic data = await req.Content.ReadAsAsync<object>();

    // Set name to query string or body data
    name = name ?? data?.name;
    fruit = fruit?? data?.fruit;

    if (name == null)
        return req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body");
    else
    {
        Test t = new Test();
        t.name = name;
        
        log.Info($"Adding Person entity {name}");
            tableBinding.Add(
                new SpacePerson() {  
                    Name = name,
                    Fruit = fruit }
                );

        return req.CreateResponse(HttpStatusCode.OK, t); //"Hello " + name)
    };
}


public class Test
{
    public string name;
}

public class Person
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public string Name { get; set; }
}

public class SpacePerson
{
    public string Name { get; set; }
    public string Fruit { get; set; }
}
// See https://aka.ms/new-console-template for more information
using StackExchange.Redis;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

var endpoint = "redis-18557.c308.sa-east-1-1.ec2.redns.redis-cloud.com:18557";
var apikey = "F4vmeczVKeNIFhnqKgle8F9cw8kcgkd7";

var redisOptions = ConfigurationOptions.Parse(endpoint);
redisOptions.Password = apikey;

var redis = ConnectionMultiplexer.Connect(redisOptions);
IDatabase db = redis.GetDatabase();

if (db.Database != null)
{
    Console.WriteLine("Connected to Redis Cloud!");
    db.StringSet("foo", "bar");
}
else
{
    Console.WriteLine("Failed to connect to Redis Cloud.");
}

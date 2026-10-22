using System;
using System.Collections.Generic;
using IndependentWork7;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Cache<string, string> cache = new Cache<string, string>(3);

Console.WriteLine($"Створено кеш ємністю {cache.Capacity}");
cache.Add("ua", "Україна");
cache.Add("pl", "Польща");
cache.Add("de", "Німеччина");
Console.WriteLine($"Додано ua, pl, de -> Count = {cache.Count}");
Console.WriteLine($"Get(\"pl\") = {cache.Get("pl")}");

Console.WriteLine();
Console.WriteLine("Додаю fr (кеш повний, має видалитися найстаріший - ua)");
cache.Add("fr", "Франція");
Console.WriteLine($"Count = {cache.Count}");
foreach (string key in new[] { "ua", "pl", "de", "fr" })
{
    Console.WriteLine($"  ContainsKey(\"{key}\") = {cache.ContainsKey(key)}");
}

Console.WriteLine();
Console.WriteLine("Додаю it -> має видалитися pl");
cache.Add("it", "Італія");
Console.WriteLine($"ContainsKey(\"pl\") = {cache.ContainsKey("pl")}, ContainsKey(\"it\") = {cache.ContainsKey("it")}");

Console.WriteLine();
Console.WriteLine("Спроба отримати видалений ключ ua:");
try
{
    cache.Get("ua");
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine($"  KeyNotFoundException: {ex.Message}");
}

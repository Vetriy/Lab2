using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main(string[] args)
    {
        string[] urls = {
            "https://geek-jokes.sameerkumar.website/api?format=json",
            "https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY",
            "https://api.quotable.io/random"
        };
        
        var client = new HttpClient();
        var stopwatch = Stopwatch.StartNew();
        Console.Clear();
        Console.WriteLine("Выберите версию консольного приложения:\n\t1. Без использования конструкции асинхронного программирования.\n\t2. С использованием конструкции асинхронного программирования.");
        
        bool useAsync = false;
        while (true)
        { 
            var input = Console.ReadLine();
            if (input == "1")
            {
                useAsync = false;
                break;
            }
            else if (input == "2")
            {
                useAsync = true;
                break;
            }
            else
            {
                Console.WriteLine("Ошибка ввода. Выберите еще раз версию консольного приложения:\n\t1. Без использования конструкции асинхронного программирования.\n\t2. С использованием конструкции асинхронного программирования.");
            }
        }
        
        if (useAsync)
        {
            await FetchDataAsync(client, urls);
        }
        else
        {
            FetchDataSync(client, urls);
        }
        
        stopwatch.Stop();
        Console.WriteLine($"Время работы программы: {stopwatch.ElapsedMilliseconds} ms\n\n\n\n");
    }

    static void FetchDataSync(HttpClient client, string[] urls)
    {
        int i = 0;
        foreach (var url in urls)
        {
            try
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                dynamic json = JsonConvert.DeserializeObject(jsonResponse);
                PrintResponse(json, i);
                i++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data from {url}: {ex.Message}");
                return;
            }
        }
    }

    static async Task FetchDataAsync(HttpClient client, string[] urls)
    {
         int i = 0;
        foreach (var url in urls)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic json = JsonConvert.DeserializeObject(jsonResponse);
                PrintResponse(json, i);
                i++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data from {url}: {ex.Message}");
                return;
            }
        }
    }

    static void PrintResponse(dynamic json, int i)
    {
        if (i == 0)
        {
            string text = json["joke"].ToString();
            Console.WriteLine($"Прикольчик с темой по программированию:\n{text}\n");
        }
        else if (i == 1)
        {
            string text2 = json["explanation"].ToString();
            Console.WriteLine($"Космический факт №1: {text2}\n");
        }
        else
        {
            string text3 = json["author"].ToString();
            Console.WriteLine($"Советики от: {text3}");
            text3 = json["content"].ToString();
            Console.WriteLine($"Цитата: {text3}\n\n\n");
        }
    }
}

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Diagnostics;

namespace Infinit;

public class Program
{
    private static readonly HttpClient client = new();
     private static readonly Dictionary<string, string> jsTsFiles = [];
     private static readonly Dictionary<char, int> alphabetDictionary = [];

    public static async Task Main(string[] args)
    {
        var sw = new Stopwatch();
        //Add missing token
        var token = "";
        var owner = "lodash";
        var repo = "lodash";

        sw.Start();

        ConfigureHttpClient(token);

        await GetFilesFromRepo(owner, repo, "");

        sw.Stop();
        PrinResults();
        Console.WriteLine($"Execution time was {sw.Elapsed}");
    }

    private static void PrinResults()
    {
        foreach (var letter in alphabetDictionary.OrderByDescending(x => x.Value))
        {
            Console.WriteLine($"{letter.Key}: {letter.Value}");
        }
    }

    private static void ConfigureHttpClient(string token)
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", token);
        client.DefaultRequestHeaders.UserAgent.TryParseAdd("InfinitApp");
    }

    private static async Task GetFilesFromRepo(string owner, string repo, string path)
    {
        var url = $"https://api.github.com/repos/{owner}/{repo}/contents/{path}";
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var files = JArray.Parse(responseBody);

        foreach (var file in files)
        {
            var fileType = file["type"]!.ToString();
            var filePath = file["path"]!.ToString();
            var fileName = file["name"]!.ToString();

            if (fileType == "file" && (filePath.EndsWith(".js") || filePath.EndsWith(".ts")))
            {
                jsTsFiles[fileName] = filePath;
                await CheckFileContents(owner, repo, filePath);
            }
            else if (fileType == "dir")
            {
                await GetFilesFromRepo(owner, repo, filePath);
            }
        }
    }

    private static async Task CheckFileContents(string owner, string repo, string filePath)
    {
        var url = $"https://api.github.com/repos/{owner}/{repo}/contents/{filePath}";
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        var fileContent = JObject.Parse(responseBody)["content"]!.ToString();
        var decodedContent = Convert.FromBase64String(fileContent);

        using var memoryStream = new MemoryStream(decodedContent);
        using var reader = new StreamReader(memoryStream);
        var content = reader.ReadToEnd();
        
        CalculateNumberOfLetters(content);
    }

    private static void CalculateNumberOfLetters(string? content){
        for(int i = 0; i < content!.Length; i++){
            AddToCorrectKey(content[i]);
        }
    }

    private static void AddToCorrectKey(char v)
    {
        var lowerCase = Char.ToLower(v);

        switch(lowerCase){
            case 'a': IncrementValue(lowerCase); break;
            case 'b': IncrementValue(lowerCase); break;
            case 'c': IncrementValue(lowerCase); break;
            case 'd': IncrementValue(lowerCase); break;
            case 'e': IncrementValue(lowerCase); break;
            case 'f': IncrementValue(lowerCase); break;
            case 'g': IncrementValue(lowerCase); break;
            case 'h': IncrementValue(lowerCase); break;
            case 'i': IncrementValue(lowerCase); break;
            case 'j': IncrementValue(lowerCase); break;
            case 'k': IncrementValue(lowerCase); break;
            case 'l': IncrementValue(lowerCase); break;
            case 'm': IncrementValue(lowerCase); break;
            case 'n': IncrementValue(lowerCase); break;
            case 'o': IncrementValue(lowerCase); break;
            case 'p': IncrementValue(lowerCase); break;
            case 'q': IncrementValue(lowerCase); break;
            case 'r': IncrementValue(lowerCase); break;
            case 's': IncrementValue(lowerCase); break;
            case 't': IncrementValue(lowerCase); break;
            case 'u': IncrementValue(lowerCase); break;
            case 'v': IncrementValue(lowerCase); break;
            case 'w': IncrementValue(lowerCase); break;
            case 'x': IncrementValue(lowerCase); break;
            case 'y': IncrementValue(lowerCase); break;
            case 'z': IncrementValue(lowerCase); break;
            default: break;
        }
    }

    public static void IncrementValue(char key)
    {
        object value = alphabetDictionary.ContainsKey(key)
        ? alphabetDictionary[key]++
        : alphabetDictionary[key] = 1;
    }
}

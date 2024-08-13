using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace Infinit;

public class Program
{
    private static readonly HttpClient client = new();
     private static readonly Dictionary<string, string> jsTsFiles = [];
     private static Dictionary<char, int> alphabetDictionary = [];

    public static async Task Main(string[] args)
    {
        var token = "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";
        var owner = "lodash";
        var repo = "lodash";
        // var alphabetDictionary = new Dictionary<string, int>();

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", token);
        client.DefaultRequestHeaders.UserAgent.TryParseAdd("InfinitApp");

        var url = $"https://api.github.com/repos/{owner}/{repo}/contents";
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        await GetFilesFromRepo(owner, repo, "");

        foreach(var letter in alphabetDictionary.OrderByDescending(x => x.Value))
        {
            Console.WriteLine($"{letter.Key}: {letter.Value}");
        }
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
        // Console.WriteLine(content);
        
        CalculateNumberOfLetters(content);

        
    }

    private static void CalculateNumberOfLetters(string? content){
        for(int i = 0; i < content!.Length; i++){
            AddToCorrectKey(content[i]);
        }
    }

    private static void AddToCorrectKey(char v)
    {
        switch(v){
            case 'a': IncrementValue(v); break;
            case 'b': IncrementValue(v); break;
            case 'c': IncrementValue(v); break;
            case 'd': IncrementValue(v); break;
            case 'e': IncrementValue(v); break;
            case 'f': IncrementValue(v); break;
            case 'g': IncrementValue(v); break;
            case 'h': IncrementValue(v); break;
            case 'i': IncrementValue(v); break;
            case 'j': IncrementValue(v); break;
            case 'k': IncrementValue(v); break;
            case 'l': IncrementValue(v); break;
            case 'm': IncrementValue(v); break;
            case 'n': IncrementValue(v); break;
            case 'o': IncrementValue(v); break;
            case 'p': IncrementValue(v); break;
            case 'q': IncrementValue(v); break;
            case 'r': IncrementValue(v); break;
            case 's': IncrementValue(v); break;
            case 't': IncrementValue(v); break;
            case 'u': IncrementValue(v); break;
            case 'v': IncrementValue(v); break;
            case 'w': IncrementValue(v); break;
            case 'x': IncrementValue(v); break;
            case 'y': IncrementValue(v); break;
            case 'z': IncrementValue(v); break;
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

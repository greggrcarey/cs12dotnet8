using System.Net;

HttpClient httpClient = new();

HttpResponseMessage response = await httpClient.GetAsync("http://www.apple.com/");

WriteLine($"Apple's home page has {response.Content.Headers.ContentLength} bytes");
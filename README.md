# TasksLikePromises - This was an Amazing Project

Extensions for using C# Task Parallel Library (TPL) in a similar way to Javascript Promises (Then / Catch / Finally). This may be helpfull when developing applications that have .NET 4.0 as a requirement without using async/await keywords.

It also contains a `Promise` static class with some helpul methods:

Name | Type | Description |
--- | --- | --- |
`Canceled` | Property | Singleton `Task` in the canceled state
`Cancel<T>()` | Method | Returns a `Task<T>` in the canceled state
`Resolved` | Property | Singleton `Task` in the completed state
`Resolve<T>(value)` | Method | Returns a `Task<T>` in the completed state with the given value
`Reject<T>(exception)` | Method | Returns a `Task<T>` in the faulted state using the exception as a cause
`Reject<T>(exceptions)` | Method | Returns a `Task<T>` in the faulted state using the exceptions as a cause
`Reject(exception)` | Method | Returns a `Task` in the faulted state using the exception as a cause
`Reject(exceptions)` | Method | Returns a `Task` in the faulted state using the exceptions as a cause
`Timeout(delay)` | Method | Returns a `Task` that will complete after a given delay

## Installation 
This library can be installed via [NuGet](https://www.nuget.org/packages/TasksLikePromises/) package. Just run the following command:

```powershell
Install-Package TasksLikePromises
```

## Compatibility

This library is compatible with the folowing frameworks:

* .NET Framework 4.0
* .NET Framework 4.5
* .NET Standard 1.0
* .NET Standard 2.0

## Usage
```csharp
public static class UsageExample
{
    private static readonly HttpClientHandler HttpHandler = new HttpClientHandler();

    public static Task HttpAsync(CancellationToken ct)
    {
        return Promise
            .Resolve(new HttpClient(HttpHandler, false))
            .Then(client => Promise
                .Resolve(new HttpRequestMessage(HttpMethod.Get, "https://httpstat.us/200"))
                .Then(request => client
                    .SendAsync(request, ct)
                    .Then(response => response.Content
                        .ReadAsStringAsync()
                        .Then(content =>
                        {
                            Console.WriteLine($"StatusCode: {response.StatusCode}");
                            Console.WriteLine($"Content: {content}");
                        })
                        .Finally(response.Dispose))
                    .Finally(request.Dispose))
                .Catch(exception =>
                {
                    Console.WriteLine("Unhandled exception has been thrown");
                    Console.WriteLine(exception);
                })
                .Finally(client.Dispose));
    }
}
```

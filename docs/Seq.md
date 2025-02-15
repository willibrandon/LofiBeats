Getting Started with Seq

Seq is the easiest way for .NET developers to capture, search and integrate structured log events! This page will walk you through the very quick setup process.

Seq is a log server that runs on a central machine. Your applications internally write structured events with a framework like Serilog:

log.Error("Failed to log on user {ContactId}", contactId);

These are sent across the network to Seq, which displays and makes them searchable:
1621

Getting started is easy and quick. You need to:

    Download and install the Seq server
    Add the appropriate NuGet package to your application

And that's it! :-)

Here we go step by step.
Installation

The Seq server is a Windows service that accepts incoming events and hosts the main web user interface over HTTP or HTTPS.

If you're just setting up on your own developer workstation, you machine almost certainly have everything required. If you're hosting Seq on a shared server for your team, check out the System Requirements and Azure Installation Guide.
The Setup Wizard

If you haven't done so already, download the Seq installer now from the https://getseq.net web site.

Running the installer will show the Setup Wizard:
506

Step through each page of the wizard. Accept the defaults where possible - details such as storage locations and ports can be configured later.

After the wizard completes, browse the Seq UI at http://localhost:5341.
Writing Structured Events

Seq has rich support for structured log events, and the optimal way to create those is with the Serilog open source logger.

    📘

    Clients for NLog, log4net, SLAB and PowerShell are also available, though the level of structured data support varies among these libraries.

The Serilog project provides a "sink" for Seq, which is released via NuGet. It currently targets .NET 4+.

At the Visual Studio Package Manager Console type:

PM> Install-Package Serilog.Sinks.Seq

Then, configure the logger and write some events. If you installed Seq to the default port you can use "http://your-seq-server:5341" as the Seq address; if you configured Seq to listen on a different port or hostname, enter those details in the WriteTo.Seq() line.

using System;
using Serilog;
 
class Program
{
    public static void Main()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.ColoredConsole()
            .WriteTo.Seq("http://localhost:5341")
            .CreateLogger();

        Log.Information("Hello, {Name}!", Environment.UserName);
 
        Console.ReadKey(true);
    }
}

Run the application - you will see something like:
1824

Now in the events view of your Seq server, press the Refresh button, which is shown with a circle icon to the right of the filter bar. The events you've just written will appear.

Clicking on the events will expand them to show their structured properties (Name in this example) and some controls for filtering.
1622

Beside the Refresh button is a drop-down from which Auto-refresh can be enabled. Choose this to see events in Seq as they arrive!
Asynchronous Batching and Durable Log Shipping

The sink transmits log events to the server asynchronously to avoid heavy performance penalties. Buffered events will be flushed and any outstanding network requests will be completed when the hosting application terminates normally. To decrease the time between batches, pass the period: parameter to the configuration method.

If the application is terminated via an attached debugger, the Task Manager or as a result of hard termination such as stack overflow, events in transit might be lost. To avoid this, and store events when network or server issues prevent delivery, a buffer file set may be specified.

Log.Logger = new LoggerConfiguration()
    .WriteTo.Seq("http://localhost:5341",
        bufferBaseFilename: @"C:\MyApp\Logs\myapp")
    .CreateLogger();

    🚧

    If multiple apps use the same folder on disk to buffer messages, it is crucial that they use unique file names.

Use with IIS web sites

If you're using durable log shipping with IIS web sites, it is recommended that you set the Disable Overlapped Recycle setting to True on the application pool for the site. This will prevent the buffer file from remaining locked by the old worker process while the new worker process starts up.
Troubleshooting

    Nothing showed up, what can I do?

If the events don't display when Seq is refreshed, the console application was probably unable to contact the Seq server.

Add the following line after the logger is configured to print any error information to the console:

Serilog.Debugging.SelfLog.Out = Console.Error;

It is also important to close the console window by pressing a key; Windows console apps are terminated "hard" if the close button in the title bar is used, so events buffered for sending to Seq may be lost if you use it.
What Next?

Once your apps are happily sending events to Seq, you can:

    Learn about the C#-like Seq query syntax
    Create some signals to provide quick access to different filters
    Set up some retention policies to control disk usage

Have fun!
// See https://aka.ms/new-console-template for more information

using SES.ConsoleApplication;
using SES.ConsoleApplication.Commands;
using SES.ConsoleApplication.Filters;
using ZLogger;

// TODO: When building add all DLLs into one exe only (single file)
// TODO: Performance profile the speed of calling a command line with IHost and one without IHost
// TODO: Write tests to make sure configurations are working properly (especially log levels

//-----------------------

// var app = Startup.CreateHostedConsoleAppBuilder(args);
var app = Startup.CreateNonHostedConsoleAppBuilder(args);

//-----------------------
// TODO: Add filters here
app.UseFilter<ReplaceLogFilter>();

//-----------------------
// TODO: Add commands here
app.Add<MyCommand>();

app.Run(args);
// NOTE: Hosted applications will be disposed after Run();




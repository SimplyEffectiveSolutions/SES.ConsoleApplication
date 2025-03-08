// See https://aka.ms/new-console-template for more information

using SES.ConsoleApplication;
using SES.ConsoleApplication.Commands;
using SES.ConsoleApplication.Filters;

// TODO: Performance profile the speed of calling a command line with IHost and one without IHost
// TODO: Write tests to make sure configurations are working properly (especially log levels)

//-----------------------

#if (UseHosted)
var app = Startup.CreateHostedConsoleAppBuilder(args);
#else
var app = Startup.CreateNonHostedConsoleAppBuilder(args);
#endif

//-----------------------
// TODO: Add filters here
//  Filters can be added to a specific class or method
// app.UseFilter<ReplaceLogFilter>(); // neuecc recommends not using ConsoleApp.Log in standard applications
app.UseFilter<TimerFilter>();

//-----------------------
// TODO: Add commands here
//  Rename the command first
app.Add<MyCommand>();

app.Run(args);
// NOTE: Hosted applications will be disposed after Run();
# nordvpn hw

## Run

### IDE

- Open NordVpn.sln; 
- Set Party.Presentation.CLI project as startup project
- Run: F5 or Ctrl + F5

## Tasks

[] update readme file how to build/run tests/run from cli

[] configuration
- [] extract configurartion info to appsettings
- [] use options parameters to get parameters.

[] Exception handling
- [] error boundaries/details
- [] catching in CLI

[] logging
- [] add logging to file.

[] When querying (from gateway), save data
- [] test case
- [] repository implementation

[] automapper
- [] from app to cli. serverDTO

[] validation
- ...
- [] FetchServersQuery - check for non-null values.

[] cache
- //consider where

[] Attach LiteDB
- [] dbContext should be transient?

[] autofixture
- [v] use in tests
- [] use automoqdata

[] polly

[] make methods async

[] CLI appearance
- [] Console format providers
- [] ViewModels in CLI?

[] make ServersListQueryHandler internal;

[] ar buvo kas orginaliam xml'e naudingo App.config

[] 1. There might be more parameters for the app.
[] 2. Persistent data store provider/storage type/libraries might change.
[] 3. Servers might be displayed differently in the console or even displayed with colors.
[] 4. Different API might be choosen

## Done

[v] salis imti per enuma'

[v] tests

[v] test cases
- [v] server_list
- [v] server_list --france
- [v] server_list --TCP
- [v] server_list --local
	
[v] DI

[v] console argument
- custom solution
	
[v] query from database (inmemory/repository)

[v] do not use string (as server, but full objects)

[v] attach inMemory DB.
# Adding docker container tests for your project

1. Add a new Unit Test Project to your solution and choose .net 6
2. Create a Dockerfile
   3. The dockerfile for the Test project is very similar to the one for the API. 
   4. The main difference is that `we use the SDK as the base for the final image instead of ASPNET`, 
   5. since we need access to the test utility. The second difference comes in the entrypoint where we are calling `dotnet test`.
6. Add a service to the `docker-compose.yaml` file for the test project.
   ``container.tests:
       image: codeapi-server
       build:
          context: .
          dockerfile: ContainerTests/Dockerfile
       depends_on:
          server:
          condition: service_started
   ``
7. T2 Tests uses NUnit, the same as T1 tests. `The main difference is that T1 tests should have a reference to the project under test and therefore has access to the source`. 
   8. While T2 Tests should not have a reference to the system under test. 
   9. It should communicate with the API (running in a container) via an HttpClient (or client library if the API has that).
Docker/Kubernetes Services Demo
-------------------------------
This demo uses a number of services to demonstrate communication between services and an end user via a web site.

* Greeter - a simple hello world implementation - not part of the rest of the demo
* Gateway - the web server providing access to the service-oriented system from outside the docker/k8s cluster
* SandwichMaker - a service that makes sandwiches
* BreadService - a service responsible for managing inventory of bread
* MeatService - a service responsible for managing inventory of meat
* LettuceService - a service responsible for managing inventory of lettuce
* CheeseService - a service responsible for managing inventory of cheese

## docker-compose setup
These instructions should get the demo running in docker-compose on a local dev workstation.

I assume you have Docker for Windows (or Mac) already installed. 

1. Create a docker network for the demo
    1. `docker network create -d bridge --subnet 172.25.0.0/16 demonet`
1. Install rabbitmq in the docker environment
    1. `docker run -d rabbitmq`
    1. Use `docker ps` to get the id of the container
    1. Add rabbitmq to the demonet network
        1. `docker network connect demonet <container id>`
    1. Find the rabbitmq container's ip address in the network
        1. `docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' <container id>`
        1. You'll see two addresses, choose the second - NOT the 172.17.0.??? address
1. Update the docker-compose.yml file in the ServicesDemo solution
    1. Open the ServicesDemo solution in Visual Studio 2017 or higher
    1. Edit the docker-compose.yml file and replace the IP address in all `- RABBITMQ__URL=172.25.0.2` lines with your new rabbitmq IP address
    1. _Optional_: Notice the 'networks:' section and that it uses the `demonet` network - if you change network names make sure to update this as well
    
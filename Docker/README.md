# ASP.NET MVC Core Application Calling an HTTP API Sample

Taken from the "Quickstart #6: IdentityServer and ASP.NET Identity" sample within this repo but updated slightly. This quickstart uses ASP.NET Identity for identity management and stores its data in-memory.

## Running the solution

Getting the project up and running on your machine should be fairly straight forward after you have the necessary tools listed below:

 - `docker`: Installation guide is [here](https://docs.docker.com/engine/installation/). Currently used version is 1.10.2.
 - `docker-compose`: Installation guide is [here](https://docs.docker.com/compose/install/). Currently used version is 1.6.0.

These should be all you need. After getting these, `cd` into this folder and run `docker-compose up` on your terminal. You should see the output smilar to this:

![](./.media/ae9af6aa-0824-11e7-87d0-3ac6d2557916.png)

From this point on, you can visit `http://localhost:5002/` on your browser to follow the example.

For questions and feedback - contact @tugberkugurlu
# TA test tasks

Represents a list of small tasks available from simple WEB API controller.

## Content

There are four tasks.
1. String inversion.
    > /task/invert
    Can have custom string input.
    > /task/invert?input=test
2. Producer-consumer task
    > /task/run-parallel
3. File hash
    > /task/file-hash
    Or with custom full file url. e.g.
    > /task/file-hash?url=http://www.africau.edu/images/default/sample.pdf
4. Query asset prices
    > task/assets

## Run in docker

In order to build and run docker container with the application the following commands can be used.
It uses http port 5050, from the host machine.
Please run from the repository root directory.

`docker compose up`

Swagger is configured for development environment only. So in order to check it in the docker please start with development configuration. e.g.

`docker-compose -f docker-compose.debug.yml up`
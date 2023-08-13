
# Run Kafka

Instruction based on [Getting Started: Apache Kafka + .NET](https://youtu.be/n_IQq3pze0s)

## Install zookeeper

```
docker pull confluentinc/cp-zookeeper
```

## Install kafka

```
docker pull confluentinc/cp-kafka
```

## Create network for kafka and zookeper

```
docker network create kafka
```

Use `docker network ls` to list all already created networks

## Run zookeper container

```
docker run -d --network=kafka --name=zookeeper -e ZOOKEEPER_CLIENT_PORT=2181 -e ZOOKEEPER_TICK_TIME=2000 -p 2181:2181 confluentinc/cp-zookeeper
```

To check if zookeeper run look at logs by `docker logs zookeeper` and check port with `telnet localhost 2181`

## Run kafka container

```
docker run -d --network=kafka --name=kafka -p 9092:9092 -e KAFKA_ZOOKEEPER_CONNECT=zookeeper:2181 -e KAFKA_ADVERTISED_LISTENERS=PLAINTEXT://localhost:9092 confluentinc/cp-kafka
```

To check if kafka run look at logs by `docker logs kafka` and check port with `telnet localhost 9092`


## Download (optionally)

You can download Conduktor app to interact with Kafka via UI
https://www.conduktor.io/get-started/#desktop

# Notes

## .NET SDK for Kafka

.NET Kafka Client doesn't provide `CosumeAsync`. Related issue:
https://github.com/confluentinc/confluent-kafka-dotnet/issues/487

It blocks main thread if run in `BackgroundService` without wrapping in `Task`.

Possible consider an alternative package: https://github.com/BEagle1984/silverback/
# Run Kafka

Instruction based on [Getting Started with Apache Kafka and .NET](https://developer.confluent.io/get-started/dotnet/#introduction)

## Create `docker-compose.yml`


```
version: '3'

services:
  zookeeper:
    image: confluentinc/cp-zookeeper:7.3.0
    hostname: zookeeper
    container_name: zookeeper
    environment:
      ZOOKEEPER_CLIENT_PORT: 2181
      ZOOKEEPER_TICK_TIME: 2000

  broker:
    image: confluentinc/cp-kafka:7.3.0
    container_name: broker
    ports:
      - "9092:9092"
    depends_on:
      - zookeeper
    environment:
      KAFKA_BROKER_ID: 1
      KAFKA_ZOOKEEPER_CONNECT: 'zookeeper:2181'
      KAFKA_LISTENER_SECURITY_PROTOCOL_MAP: PLAINTEXT:PLAINTEXT,PLAINTEXT_INTERNAL:PLAINTEXT
      KAFKA_ADVERTISED_LISTENERS: PLAINTEXT://localhost:9092,PLAINTEXT_INTERNAL://broker:29092
      KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR: 1
      KAFKA_TRANSACTION_STATE_LOG_MIN_ISR: 1
      KAFKA_TRANSACTION_STATE_LOG_REPLICATION_FACTOR: 1
```

## Create topic

Bellow create test topic `ates` (`--topic ates`)

```
docker compose exec broker kafka-topics --create --topic ates --bootstrap-server localhost:9092 --replication-factor 1 --partitions 1
```

## Download (optionally)

You can download Conduktor app to interact with Kafka via UI
https://www.conduktor.io/get-started/#desktop


# Use SDK

Official package: https://www.nuget.org/packages/Confluent.Kafka

```
<PackageReference Include="Confluent.Kafka" Version="2.2.0" />
```

Use this package on both side: consumer and producer

## Producer

```
var config = new ProducerConfig  
{  
	BootstrapServers = "localhost:9092",  
};  
  
_producer = new ProducerBuilder<Null, string>(config).Build();
```

```
await _producer.ProduceAsync(topic, new Message<Null, string>  
{  
	Value = message  
}, cancellationToken);
```

## Consumer

```
_config = new ConsumerConfig  
{  
	GroupId = "ates-group-consumer",  
	BootstrapServers = "localhost:9092",  
  
	AutoOffsetReset = AutoOffsetReset.Earliest  
};
```

```
var consumingTask = Task.Run(() => consumer.Consume(stoppingToken), stoppingToken);
```

## Notes

.NET Kafka Client doesn't provide `CosumeAsync`. Related issue:
https://github.com/confluentinc/confluent-kafka-dotnet/issues/487

It blocks main thread if run in `BackgroundService` without wrapping in `Task`.

Possible consider an alternative package: https://github.com/BEagle1984/silverback/
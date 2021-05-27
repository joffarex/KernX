KernX
=====
KernX is a modular framework. It is aiming to be a complete package for developing distributed software. It can also be used as **infrastructure** layer in any software architecture

This are all the features I want to work on and have packaged with **KernX**:

Packages to add:
-----
- [ ] EventBus
    - [ ] RabbitMQ           
    - [ ] Kafka
    - [ ] SQS/SNS/EventBridge
    - [ ] NATS
    - [ ] Azure Service Bus
- [x] Logger
- [ ] CQRS
- [ ] Data Access Layer
    - [ ] MongoDB            
    - [ ] CassandraDB
    - [ ] DynamoDB
    - [ ] PostgreSQL
    - [ ] SQL Server
- [ ] Metrics
    - [ ] Analytics
    - [ ] Tracing
    - [ ] Prometheus
- [ ] Security
    - [ ] Secrets
    - [ ] Environment management
- [ ] Search
    - [ ] ElasticSearch
    - [ ] Logstash
- [ ] Network
    - [x] HTTP Client wrapper
    - [ ] gRPC wrapper
    - [ ] TCP/UDP


Packages that I really want to add but will need cleaver solution to do so:
---------------------------------------------------------------------------
- [ ] Event-Sourcing with snapshots
- [ ] DDD

Higher level abstractions:
--------------------------
- [ ] Framework
  Basically what MassTransit is
- [ ] IaC (experimental)
    - [ ] Terraform generator
- [ ] Proxy
  AWS API Gateway/Nginx style
KernX
=====
KernX is a modular framework. It is aiming to be a complete package for developing distributed software. It can also be used as **infrastructure** layer in any software architecture

This are all the features I want to work on and have packaged with **KernX**:

Packages to add:
-----
- [ ] EventBus
    - [ ] Providers
        - [x] RabbitMQ           
        - [ ] Kafka
        - [x] SQS/SNS
        - [ ] EventBridge
        - [ ] Kinesis
    - [ ] Patterns
        - [ ] Publish/Subscribe
        - [ ] Request/Response
- [ ] Sagas
- [x] Logger
- [ ] Data Access Layer
    - [ ] Drivers
        - [x] MongoDB
        - [ ] DynamoDB
        - [ ] PostgreSQL/SQL Server
    - [ ] Transactions
- [ ] Security
    - [ ] Secrets
    - [ ] Environment management
- [ ] Search
    - [ ] ElasticSearch
    - [ ] Logstash
- [ ] Network
    - [x] HTTP Client wrapper
    - [ ] gRPC wrapper


Packages that I really want to add but will need cleaver solution to do so:
---------------------------------------------------------------------------
- [ ] Event-Sourcing with snapshots
- [ ] DDD


Higher level abstractions:
--------------------------
- [ ] Framework
  Basically what MassTransit is
- [ ] Proxy
  AWS API Gateway/Nginx style

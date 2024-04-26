# RabbitMq

RabbitMq Test project for TLS connection. 

Includes a API for sending messages to queue. And a Worker for consuming messages from queue.
The worker writes the messages to console.

## Application settings 

```json
  "RabbitMqConfiguration": {
    "HostName": "",
    "Username": "",
    "Password": "",
    "CertPassword": "",
    "ServerName": "",
    "CertPath": ""
  }
```

## Certificates

The project uses a self signed certificate. You need access to the .p12 certificate file to run the project.
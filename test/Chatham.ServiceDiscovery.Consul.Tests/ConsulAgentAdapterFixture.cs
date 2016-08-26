﻿using System.Collections.Generic;
using Consul;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;

namespace Chatham.ServiceDiscovery.Consul.Core.Tests
{
    public class ConsulAgentAdapterFixture
    {
        public string ServiceName { get; set; }
        public List<string> Tags { get; set; }
        public bool OnlyPassing { get; set; }
        public bool Watch { get; set; }

        public IConsulClient Client { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();
        public IConsulClientAdapter EndpointRetriever { get; set; }
        
        public QueryResult<ServiceEntry[]> ClientQueryResult { get; set; }
        public IHealthEndpoint HealthEndpoint { get; set; }

        public ConsulAgentAdapterFixture()
        {
            Client = Substitute.For<IConsulClient>();
            HealthEndpoint = Substitute.For<IHealthEndpoint>();
            EndpointRetriever = Substitute.For<IConsulClientAdapter>();
        }

        public void SetHealthEndpoint()
        {
            HealthEndpoint.Service(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<QueryOptions>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(ClientQueryResult));

            Client.Health.Returns(HealthEndpoint);
        }

        public ConsulClientAdapter CreateSut()
        {
            return new ConsulClientAdapter(Client, ServiceName, Tags, OnlyPassing, CancellationTokenSource.Token, Watch);
        }
    }
}

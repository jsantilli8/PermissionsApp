using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Nest;
using Domain.Entities;
using Domain.Interfaces;

public class ElasticsearchRepository : IElasticsearchRepository
{
    private readonly ElasticClient _client;
    private readonly string _indexName;

    public ElasticsearchRepository(IConfiguration configuration)
    {
        var uri = configuration["Elasticsearch:Uri"];
        _indexName = configuration["Elasticsearch:Index"];

        var settings = new ConnectionSettings(new Uri(uri))
            .DefaultIndex(_indexName)
            .DisableDirectStreaming();

        _client = new ElasticClient(settings);

        var pingResponse = _client.Ping();
        if (!pingResponse.IsValid)
        {
            throw new Exception("Could not connect  Elasticsearch: " + pingResponse.DebugInformation);
        }

    }

    public async Task IndexPermissionAsync(Permission permission)
    {
        var response = await _client.IndexDocumentAsync(permission);
        if (!response.IsValid)
        {
            throw new Exception($"Document index Error : {response.OriginalException?.Message}");
        }
    }

    public async Task<IEnumerable<Permission>> SearchPermissionsAsync(string query)
    {
        var searchResponse = await _client.SearchAsync<Permission>(s => s
            .Index(_indexName)
            .Query(q => q
                .Match(m => m
                    .Field(f => f.EmployeeSurName)
                    .Query(query))));

        return searchResponse.Documents;
    }
}

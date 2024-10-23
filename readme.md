# Elasticsearch Overview

## Video References
- [Elasticsearch Introduction](https://www.youtube.com/watch?v=ksTTlXNLick&t=319s)
- [Elasticsearch Deep Dive](https://www.youtube.com/watch?v=60UsHHsKyN4)

## Key Concepts

1. **Schema-less JSON Documents**: Like NoSQL databases.
2. **Communication**: Interacts with the search server through HTTP REST API (.NET has client SDK).
3. **Near Real-Time Search**: Additions, modifications, and deletions propagate within seconds to clusters.
4. **Cluster**: A collection of nodes (servers). It can consist of one or more nodes and can scale as needed. Identified by a unique name (default is "elasticsearch").
5. **Node**: A single server in a cluster that stores searchable data. Each node participates in indexing and searching. Identified by a name (defaults to a random Marvel character).
6. **Index**: A collection of documents (e.g., products, accounts). Corresponds to a database in a relational database system. Identified by a name (must be lowercased).
7. **Type**: Represents a class/category of similar documents. Think of it as a table in a relational database. Each index can have one or more types defined.
8. **Mapping**: Similar to a database schema, it describes the fields a document of a given type may have, including data types and how fields are indexed and stored.
9. **Document**: The basic unit of information that can be indexed, consisting of key/value pairs. Expressed in JSON.
10. **Shards**: An index can be divided into multiple pieces for better data handling. Each shard is a fully functional and independent index.
11. **Replicas**: Copies of shards for high availability. A replica never resides on the same node as the original shard.
12. **Sense**: A web plugin that helps issue Elasticsearch REST requests (now part of Kibana in newer versions).
13. **Kibana**: A web plugin to view index and document details, facilitating quick searches.
14. **Array Type**: Elasticsearch doesn't have a dedicated array type; it automatically handles arrays when data is sent.
15. **Document ID**: Providing an ID when creating a document is optional; if omitted, Elasticsearch will create one.
16. **Updating Documents**: Use a PUT request with the ID and the entire JSON object, or a POST request with modified fields.
17. **Batch Processing**: Use the bulk API to alleviate network traffic. Allows multiple actions (deletes, updates) in a single request.
18. **Relevancy & Scoring**: Documents are ranked based on a score calculated for each matching document.
19. **Query String Search**: Simple queries via REST request URI. Supports advanced queries.
20. **Query DSL (Domain Specific Language)**: Define queries in the request body using JSON for more advanced queries.
21. **Leaf Queries**: Look for particular values in specific fields.
22. **Compound Queries**: Combine multiple queries using boolean logic.
23. **Full Text Queries**: Used for running full text searches on fields.
24. **Term Level Queries**: Used for exact matching of values.
25. **Joining Queries**: Offers nested queries and parent-child relationships to perform joins.
26. **Query Examples**:
   - Get all: `GET /ecommerce/product/_search?q=*`
   - Search specific field: `GET /ecommerce/product/_search?q=name:pasta`
27. **DSL Examples**:
   - Get all restaurants: 
   ```json
   POST http://localhost:9200/places/restaurant/_search
   {
       "query": {
           "match_all": {}
       }
   }
```

## Get restaurants matching tacos in all fields:

bash``
POST http://localhost:9200/places/restaurant/_search
{
    "query": {
        "query_string": {
            "query": "tacos"
        }
    }
}
```

## Using Synonyms: Set up synonyms in the Elasticsearch config to handle various state names.
## Simple Count Result:

bash```
POST http://localhost:9200/places/restaurant/_count
{
    "query": {
        "match_all": {}
    }
}
```

## Pagination: Use size and from parameters to manage pagination.
json

## Copy code

bash```
POST http://localhost:9200/places/restaurant/_search?size=2&from=0
{
    "query": {
        "match_all": {}
    }
}
```

## Conclusion
This document outlines the fundamental concepts and functionalities of Elasticsearch, along with practical examples for querying and managing your data.

https://www.youtube.com/watch?v=ksTTlXNLick&t=319s
https://www.youtube.com/watch?v=60UsHHsKyN4

01) Schema-less JSON documents (like NoSQL DBs)
02) Communication with search server done through HTTP REST API (.NET has client SDK)
03) Near real-time search (adds, modified, deletions changes are propagrated in seconds to clusters)
04) Cluster: is a collection of nodes (servers). Consists of one or more nodes, depending on the scale, can contain as many nodes as you want.
	Together, these nodes contain all data. A cluster provides indexing and search capability across all nodes. Indentified by a unique name (defaults to "elasticsearch")
05) Node: is a single server that is part of a cluster. It stores searchable data, stores all data if there is only one node in the cluster, or part of the data
	if there are multiple nodes. Participates in a cluster's indexing and search capabilities. Identified by a name (defaults to random Marvel character). A node joins
	a cluster named "elasticsearch" by default. Starting a single node on a network will by default create a new single-node cluster named "elasticsearch"
06) Index: is a collection of documents (e.g., product, account, movie). Each of the above examples would be a type. Corresponds to a database within a 
	relational database system. Identified by a name, which must be lowercased. Used when indexing, searching, updating and deleting documents within the index. 
	You can define as many indexes as you want within a cluster.
07) Type: represents a class/category of similar documents, e.g., "user". Consists of a name and mapping. Simplified, you can think of a type as a table within
	a relational database. An index can have one or more types defined, each with their own mapping. Stored witin a metadata field name _type because Lucene has
	no concept of document types. Searching for specific document types applies a filter on this field.
08) Mapping: is similar to a database schema for a table in a relational database. Describes the fields that a document of a given type may have. Includes the 
	data type for each field, e.g. string, integer, data, ... Also includes information on how fields should be indexed and stored by Lucene. Dynamic mapping means
	that it is optional to define a mapping explicitly. You can think of it as equivalent to column schema within a relational database.
09) Document: is a basic unit of information that can be indexed. Consists of fields, which are key/value pairs. A value can be a string, date, object, etc.
	Corresponds to an object in an OOP language. A document can be a single user, order, product, etc. Documents are express in JSON. You can store as many documents
	within an index as you want. You can think of it as equivalent to a row within a relational database.
10) Shards: an index can be divided into multiple pieces called shards. Useful if an index contains more data than the hardware of a node can store (e.g. 1 TB data
	on a 500 GB disk). A shard is fully functional and independent index. Can be stored on an y node in a cluster. The number of shards can be specified when 
	creating an index. Allows to scale horizontally by content volumne (index space). Allows to distribute and parallelize operations across shards, which increases performance.
11) Replicas: is a copy of a shard. Provides high availability in case a shard or node fails. A replica never resides on the same node as the original shard. Allows
	scaling search volume, because search queries can be executed on all replicas in parallel. By default, Elasticsearch adds 5 primary shards and 1 replica for each index.
12) Look into Sense which is a web plugin that helps issue Elasticsearch REST requests. https://www.found.no/foundation/Sense-Elasticsearch-interface﻿
13) Look into Kibana which looks like a web plugin that helps you view details of your index, document, etc. Helps you do quick searches into documents
14) Elasticsearch doesn't have an array type, when adding a mapping, even if you know it's an array you specify as normal field. Elasticsearch will automatically store
	data as "array" when sent in.
15) When creating a document, it is optional to provide an ID, Elasticsearch will create one for you if you omit it. If you want to provide ID, use PUT, if not, use POST.
16) To update a document, issue a PUT request with the ID and send in the entire JSON object along with the modified fields. You can alternatively issue a POST request
	and only provided the modified field using the following example: 
	POST /ecommerce/product/1001/_update
	{
		"doc": {
			"price": 50.00
		}
	}
17) You can perform batch processing to alleviate network traffic. Example command: POST /ecommerce/product/_bulk
	You can perform different actions on a single batch request such as both deleting and updating in the same request.
	Each action in the bulk request is processed sequentially, if one fails, the others will still process.
	You don't have to specify the index or type in the URL, you can specify this data in the JSON. The following commands are valid:
	POST /ecommerce/product/_bulk
	POST /ecommerce/_bulk
	POST /_bulk
18) Relevancy & Scoring: To rank documents for a query, a score is calculated for each document that matches a query. The higher the score, the more relevant the
	document is to the search query. Queries in query contest affect the scores of matching documents. "How well does the document match?".
	Queries in filter context do not affect the scores of matching documents. "Dos the document match"
19) Query string search: search by sending parameters through the REST request URI. Used for simple queries and ad-hoc queries on the command line. Also supports
	advanced queries. GET http://localhost:9200/ecommerce/product/_search?q=pasta
20) Query DSL (Domain Specific Language): search by defining queries within the request body in JSON. Supports more features than the query sring approach. 
	Used for more advanced queries. Often easier to read, as queries are defined in JSON. 
	GET http://localhost:9200/ecommerce/product/_search
	{
		"query": {
			"match": {
				"name": "pasta"
			}
		}
	}
21) Leaf queries: Look for particular values in particular fields, for instance "pasta" in product names. Can be used by themselves within a query, without being
	part of a compound query. Can also be used within compound queries to construct more advanced queries.
22) Compound queries: Wrap leaf clauses or even other compound query clauses. Used to combine multiple queries in a logical fashion (usually with boolean logic).
	Can also be used to alter the behavior of queries.
23) Full text queries: used for running full text queries on full text fields. E.g. a product name or description. Values are analyzed when adding documents 
	or modifying values. E.g. removing stop words, tokenizing and lowercasing. Will apply each field's analyzer to the query sring before executing.
24) Term level queries: Used for exact matching of values. Usually used for structured data like numbers and dates, rather than full text fields. E.g. finding
	persons born between year 1980 and 2000. Search queries are not analyzed before executing.
25) Joining queries: Performing joins in a distributed system is expensive. Elasticsearch offers two forms of joins that are designed to scale horizontally.
	Nested query: Documents may contain fields of type nested with arrays of objects. Each object can be queried with the nested query as an independent document
	has_child and has_parent queries: A parent-child relationship can exist between two document types within a single index. The has_child query returns documents
	whose child documents match the query. The has_parent query returns child documents whose parent document matches the query
26) Query examples:
	Get all: GET /ecommerce/product/_search?q=*
	Search in all fields: GET /ecommerce/product/_search?q=pasta
	Search in specific field: GET /ecommerce/product/_search?q=name:pasta
	Search in specific field for pasta and spaghetti: GET /ecommerce/product/_search?q=name:(pasta AND spaghetti)
	Search in specific field for pasta or spaghetti: GET /ecommerce/product/_search?q=name:(pasta OR spaghetti)
	Search in specific field for pasta or spaghetti and active: GET /ecommerce/product/_search?q=(name:(pasta OR spaghetti) AND status:active)
	Search in specific field for pasta and not spaghetti: GET /ecommerce/product/_search?q=name:+pasta -spaghetti
	Search in specific field, defaults to OR: GET /ecommerce/product/_search?q=name:pasta spaghetti
	Search in specific field, now both must match and in that order, will match even if "-" between match: GET /ecommerce/product/_search?q=name:"pasta spaghetti"
27) DSL examples:
	Get all restaurant:	
	POST http://localhost:9200/places/restaurant/_search	
	{
		"query": {
			"match_all":{}
		}
	}

	Get all types in index:	
	POST http://localhost:9200/places/_search
	{
		"query": {
			"match_all":{}
		}
	}

	Get restaurants matching tacos in all fields:	
	POST http://localhost:9200/places/restaurant/_search
	{
		"query": {
			"query_string":{
				"query":"tacos"
			}
		}
	}

	Get restaurants matching tacos in tags field:	
	POST http://localhost:9200/places/restaurant/_search
	{
		"query": {
			"query_string":{
				"query":"tacos",
				"fields":["tags"]
			}
		}
	}

	Get restaurants matching tacos in tags field with filter:	
	POST http://localhost:9200/places/restaurant/_search
	{
		"query": {
			"filtered": {
				"filter":{
					"range":{
						"rating":{
							"gte":4.0
						}
					}
				},				
				"query_string":{
					"query":"tacos",
					"fields":["tags"]
				}
			}
		}
	}

	Get restaurants with just filter:	
	POST http://localhost:9200/places/restaurant/_search
	{
		"query": {
			"filtered": {
				"filter":{
					"range":{
						"rating":{
							"gte":4.0
						}
					}
				}
			}
		}
	}

	Get restaurants in NY with filter:	
	POST http://localhost:9200/places/restaurant/_search
	{
		"query": {
			"filtered": {
				"filter":{
					"range":{
						"rating":{
							"gte":4.0
						}
					}
				},				
				"query":{
					"match": {
						"address.state": "ny"
					}
				}
			}
		}
	}

	Get specific geo location:	
	POST http://localhost:9200/places/restaurant/_search
	{
		"query": {
			"filtered": {
				"filter":{
					"geo_distance": {
						"distance": "100km",
						"location": [40.7894537,-739481288]
					}
				}
			}
		}
	}

	Get specific geo location and rating match (must, mustnot, should):	
	POST http://localhost:9200/places/restaurant/_search
	{
		"query": {
			"filtered": {
				"filter":{
					"bool": {
						"must": [
							{
								"range": {
									"rating": {
										"gte": "4.0"
									}
								}
							},
							{
								"geo_distance": {
									"distance": "100km",
									"location": [40.7894537,-739481288]
								}
							}
						]					
					}
				}
			}
		}
	}
28) Use synonyms to resolve issue where state input could be NY, New York, or Big Apple. Set up synonyms.txt in the Elasticsearch config folder. On each like type
	in terms for synonyms (ny,new your, big apple on one row, ca, california, socal, norcal on next row). When you build your index pass in JSON object with
	settings and mappings. The text file synonyms.txt will be noted in the JSON object. This is where you would specify that your location field is of type geo_point
	for geospatial lookups.
29) To get a simple count result:
	POST http://localhost:9200/places/restaurant/_count
	{
		"query": {
			"match_all":{}
		}
	}
30) To specify pagination use size and from (you'll get total in response as well so you don't have to do a count to get that):
	POST http://localhost:9200/places/restaurant/_search?size=2&from=0 
	{
		"query": {
			"match_all":{}
		}
	}
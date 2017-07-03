00) In Sense, instead of clicking with your mouse you can use Ctrl + Enter
01) View all indices: GET /_cat/indices
02) Create my_blog index (if format is specified for date type, data sent in must conform or error is thrown): 
	PUT /my_blog
	{
		"settings": {
			"index": {
				"number_of_shards": 5
			}
		},
		"mappings": {
			"post": {
				"properties": {
					"user_id": {
						"type": "integer"
					},
					"post_text": {
						"type": "text"
					},
					"post_date": {
						"type": "date",
						"format": "YYYY-MM-DD"
					},
					"post_word_count": {
						"type": "integer"
					}
				}	
			}
		}
	}
03) Get schema mapping: GET my_blog/_mapping
04) Create new blog post:
	POST my_blog/post
	{
		"post_date": "2014-08-20",
		"post_text": "This is a real blog post!",
		"user_id": 1
	}
05) Create new blog post with ID:
	POST my_blog/post/1
	{
		"post_date": "2014-08-26",
		"post_text": "This is post with id 1",
		"user_id": 1
	}
06) Get all blog post: GET my_blog/post/_search
07) Get blog by ID: GET my_blog/post/AV0F6IicsCj7-Xzj1Gb_
08) Core data types are: String, Numbers, Boolean, Date, Binary (images or blob, stored in base64 strings, not indexed by default)
09) To delete index: DELETE my_blog
10) Specify to only store the user_id field, you can still search other fields but you'll only get user_id:
	PUT /my_blog
	{
		"mappings": {
			"post": {
				"_source": {
					"enabled": false
				},
				"properties": {
					"user_id": {
						"type": "integer",
						"store": true
					},
					"post_text": {
						"type": "text"
					},
					"post_date": {
						"type": "date",
						"format": "YYYY-MM-DD"
					}
				}	
			}
		}
	}
11) Indexes Routing: helps search performance, without routing search will hit rest endpoint and check all the shards in the cluster
	GET http://localhost:9200/my_blog/post/_search?routing=2&post_text:awesome (route directly to the shard with all of the data owned by user_id 2)
	{
		"mappings": {
			"post": {
				"_routing": {
					"required": true,
					"path": "user_id"
				},
				"_source": {
					"enabled": false
				},
				"properties": {
					"user_id": {
						"type": "integer",
						"store": true
					},
					"post_text": {
						"type": "text"
					},
					"post_date": {
						"type": "date",
						"format": "YYYY-MM-DD"
					}
				}	
			}
		}
	}
12) Indexes Aliases: you can also create an alias across multiple indexes
	POST http://localhost:9200/_aliases
	{
		"actions" : [
			{ "add" : { "index" : "eventLog-2014-08-02", "alias" : "eventLog" } }
		]
	}
13) Get by alias
	GET http://localhost:9200/eventLog/event/_search?q=event:error
14) The highest score will come first, Elasticsearch will return the results ordered by score
15) Query to match wonderful or blog
	GET my_blog/post/_search
	{
	  "query": {
		"match": {
		 "post_text": "wonderful blog"
		}
	  }
	}
16) Query match phrase, finds exact phrases, good for full text searching, more predictable results, will match "wonderful blog" not "blog wonderful"
	GET my_blog/post/_search
	{
	  "query": {
		"match_phrase": {
		 "post_text": "wonderful blog"
		}
	  }
	}
17) Adding filter to query:
	{
		"query": {
			"filtered": {
				"filter": {
					"range": {
						"post_date": {
							"gt": "2014-10-18"
						}
					}
				},
				"query": {
					"match": {
						"post_text": "wonderful blog"
					}
				}
			}
		}
	}
18) Highlighting: will return HTML in highlight field with <em> tags around matched term 
	GET my_blog/post/_search
	{
	  "query": {
		"match": {
		 "post_text": "writing"
		}
	  },
	  "highlight": {
		"fields": {
		  "post_text": {}
		}
	  }
	}
19) Elasticsearch Analytics: aggregations, like GROUP BY but a lot better. Can do hierarchical rollups, min/max, percentiles, histograms, calculating the distance between
	geographical points...TONS of built in aggregations.
	GET my_blog/post/_search
	{
	  "query": {
		"match": {
		 "post_text": "writing"
		}
	  },
	  "aggs": {
		"all_words":{
		  "terms": {
			"field": "post_text"
		  }
		}
	  }
	}

	GET my_blog/post/_search
	{
	  "query": {
		"match": {
		 "post_text": "writing"
		}
	  },
	  "aggs": {
		"avg_word_count":{
		  "avg": {
			"field": "post_word_count"
		  }
		}
	  }
	}
20) Analysis and Analyzers: Tokens are words of complete string. Character filter. Remove html, convert "9" to "nine". Tokenize by whitespace, period, comma, etc.
	Toke filter to remove stop words like "and" or "the". Comes with a lot of built-in analyzers. You can even make your own. The standard (default) analyzer is good general
	purpose analyzer for multiple languages. Breaks up text strings by natural word boundaries, takes away punctuation, and lower-cases terms. The whitespace analyzer breaks
	up text by whitespace only. More useful for strings like computer code or logs. The simple analyzer breaks up strings by anything that isn't a number, lowercases terms.
	
	{
		"mappings": {
			"post": {
				"properties": {
					"user_id": {
						"type": "integer"						
					},
					"post_text": {
						"type": "text",
						"analyzer": "standard"
					},
					"post_date": {
						"type": "date"						
					}
				}	
			}
		}
	}

	GET _analyze
	{
	  "analyzer": "standard",
	  "text": "Convert the title-case text using the ToLower(string) command."
	}
	GET _analyze
	{
	  "analyzer" : "standard",
	  "text" : ["this is a test", "the second text"]
	}
	GET _analyze
	{
	  "tokenizer" : "keyword",
	  "filter" : ["lowercase"],
	  "text" : "this is a test"
	}
	GET _analyze
	{
	  "tokenizer" : "keyword",
	  "filter" : ["lowercase"],
	  "char_filter" : ["html_strip"],
	  "text" : "this is a <b>test</b>"
	}


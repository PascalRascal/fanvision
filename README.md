## NEW JSON SENDING SYSTEM

#### Runthrough

- Server starts websocket, only in charge of broadcasting data and sending to clients
- DataRetriever is a client that connects to websocket
- When DataRetriever gets new data, it sends it to server by sending a game update json
- Server will interperet this and broadcast it back to everyone else.
- When a new client joins the websocket sever, it should send a request json
- The request json will forward to the DataRetriever client, which in turn will return a game update of ALL info about the game, which will only go to THAT CLIENT (Important)



### Server
- needs to keep track as to which client is the DataRetriever
- any new client that connects that is not the DataRetriever needs to get a game update from the DataRetriever
- cardboard -> server -> -> data retriever -> server -> cardboard
- Will need json tags setup to talk to eachother
- Needs to keep track of clients and IDs (not sure how to handle this)


### Data Retriever
- Checks for new data every 5 seconds
- if there is new data, it stores that data and will send it to the server as a game_update json array (as shown in the example below)
- if the server asks for all data, it will send a game_update with ALL DATA

### Cardboard
- Needs to get a game_update with ALL DATA on connect (could use onconnect or could send a hello message)
- Needs game updates sent to it as it comes into the server from the Data Retriever

##EXAMPLES

###ALERTS

- Home Run Alert

```
{
  "data_type": "alert",
  "data": {
    "type": "homerun"
  }
}
```

###UPDATES

- Game Update (can include any of these in an array)

```
{
  "data_type": "game_update",
  "data": [
    {
      "item": "strikes",
      "new_value": 2
    },
    {
      "item": "home_score",
      "new_value": 10
    },
    {
      "item": "away_score",
      "new_value": 7
    },
    {
      "item": "inning",
      "new_value": 4
    }
  ]
}
```


###POIS

- Add POI

```
{
  "data_type": "new_poi",
  "data": {
    "latitude": 10,
    "longitude": -89,
    "title": "Come Here!",
    "description": "This is an interesting area to buy stuff!",
    "color": "blue"
  }
}
```

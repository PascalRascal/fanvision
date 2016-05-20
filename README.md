
http://blog.samuelattard.com/the-tutorial-for-php-websockets-that-i-wish-had-existed/

FanVision Backend

When getting an update from the websocket, I am expecting to receive a JSON formatted in the following way:

{
	“data_type” : string,
	“data”: {
		JSON
		}
}

The data_type describes what data I am receiving, the data types we will be using are as follows:

game_update
alert
new_poi
remove_poi

These are all strings

The data object inside each JSON is as follows

game_update
“data”: {
	“item”: string,
	“new_value”: integer
}
For the item, please use one of the following strings
strike
homeScore
awayScore

This is not an inclusive list, but for now it should suffice

alert
“data”: {
	“type”: string
	}

The type should be one of these two,
strike_out
Homerun
new_poi
“data”: {
	“latitude”: double,
	“longitude”: double,
	“title” string,
	“description”: string,
	“color”: string
}

All of these should be self explanatory, for color stick to red, green, or blue

remove_poi
“data”: {
	“title”: string,
}

Assuming all POIS have different titles, we should be able to remove POIs this way. An alternatiev way would be to give me the index of the POI, but I think we should stick to the simplicity of this way


NEW POI PLAN

Two commands used on clients for POIS

Add_poi
Remove_poi

Each poi has an index, which ensures each one is unique

When adding, need to specify index, name, location, and description

When removing, ONLY specify the index.

These should support an array format which will allow the ability to add more than 1 poi with one JSON request.


TWO ways for clients to get POIS

When client has app open, runs an http request for POIS, and receives a JSON of add_pois which will be the current POIS that are enabled in the database or something of the sort.
When POIS are added, the websocket will need to send a BROADCAST to all clients with the add_poi json format.


POIS will only be removed via a websocket BROADCAST





##EXAMPLES

###ALERTS

#####Home Run Alert

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

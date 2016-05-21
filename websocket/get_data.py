import urllib
import json
from time import sleep
import sys
from twisted.internet import reactor
from autobahn.twisted.websocket import WebSocketClientFactory, WebSocketClientProtocol, connectWS

###################BASE VARIABLES
game_data = {"inning": 0, "home_score": 0, "away_score": 0, "balls": 0, "strikes": 0, "outs": 0, "at_bat": "None", "photo": "None"}
last_game_data = {"inning": -1, "home_score": -1, "away_score": -1, "balls": -1, "strikes": -1, "outs": -1, "at_bat": "-1", "photo": "-1"}
url = "http://gd2.mlb.com/components/game/mlb/year_2016/month_05/day_21/gid_2016_05_21_clemlb_bosmlb_1/plays.json"
photo = "http://mlb.mlb.com/mlb/images/players/head_shot/"
data = {}
last_data = data
jsonData_base = {"data_type": "game_update", "data": []}
global jsonData
###################END BASE VARIABLES



def make_json_all():
    ## Formats a JSON with all current Game Data.  Send When Client Connects
    jsonData = jsonData_base
    for key, value in game_data.iteritems():
        jsonData["data"].append({"item": key, "new_value": value})
    print "Sending ALL Game Data as JSON"
    print jsonData
    print "------------"
    ##SEND HERE


def update_current_data():
    global jsonData
    jsonData = jsonData_base
    global game_data
    global last_game_data
    game_data["inning"] = data["data"]["game"]["inning"]
    game_data["home_score"] = data["data"]["game"]["score"]["hr"]
    game_data["away_score"] = data["data"]["game"]["score"]["ar"]
    game_data["balls"] = data["data"]["game"]["b"]
    game_data["strikes"] = data["data"]["game"]["s"]
    game_data["outs"] = data["data"]["game"]["o"]
    game_data["at_bat"] = data["data"]["game"]["players"]["batter"]["boxname"]
    game_data["photo"] = photo + str(data["data"]["game"]["players"]["batter"]["pid"]) + ".jpg"

    if str(game_data) != str(last_game_data):
        for key, value in game_data.iteritems():
            if str(game_data[key]) != str(last_game_data[key]):
                jsonData["data"].append({"item": key, "new_value": value})
        print "Game Update! JSON:"
        print jsonData
	reactor.run()
        print "-------------"

    last_game_data = game_data



#Class for connecting to websocket
class BroadcastClientProtocol(WebSocketClientProtocol):
    def sendMsg(self):
	print "sending msg"
	print str(game_data)
        self.sendMessage(str(game_data).encode('utf8'))
    def onOpen(self):
	global jsonData
	print "<><>><<><><><"
	print jsonData
	#jsonDump = json.dumps(jsonData)
	print "server" + str(game_data)
        self.sendMessage(("server" + str(game_data).encode('utf8')))
	print "message sent"
	#sleep(2)
	#self.factory.reactor.stop()
    def onMessage(self, payload, isBinary):
        if not isBinary:
            print("Message Received: {}".format(payload.decode('utf8')))
	    self.factory.reactor.stop()
if __name__== '__main__':
    if len(sys.argv) < 2:
        print ("Need WebSocket server address")
        sys.exit(1)
    factory = WebSocketClientFactory(sys.argv[1])
    factory.protocol = BroadcastClientProtocol
    connectWS(factory)
    while True:
	print "running"
   	response = urllib.urlopen(url)
        data = json.loads(response.read())
        "GOT DATA"
        if str(last_data) != str(data):
            print "DATA CHANGED"
            update_current_data()
        last_data = data
        sleep(3)
